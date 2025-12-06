using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GJ.Services;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User?.Identity?.IsAuthenticated == true)
        {
            // Usuário autenticado via cookie
            return new AuthenticationState(httpContext.User);
        }

        // Usuário não autenticado
        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }
}