using GJ.Components;
using GJ.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Adicionar controladores
builder.Services.AddControllers();

// Registrar servi�os (apenas os necess�rios)
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITerceiroService, TerceiroService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

// Configurar autentica��o com cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "GJ.Auth";
        options.LoginPath = "/login";
        options.LogoutPath = "/auth/signout";
        options.ExpireTimeSpan = TimeSpan.FromDays(5);
        options.SlidingExpiration = true;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        options.Cookie.SameSite = SameSiteMode.Lax;
    });

// Autoriza��o
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// IMPORTANTE: Adicionar middleware na ordem correta
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();