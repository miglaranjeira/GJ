using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using GJ.Services;

namespace GJ.Controllers;

[Route("auth")]
public class AuthController : Controller
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromForm] string username, [FromForm] string password, [FromForm] string returnUrl = "/")
    {
        Console.WriteLine($"AuthController: Tentando login para {username}");

        var utilizador = await _authService.AuthenticateAsync(username, password);

        Console.WriteLine($"AuthController: Resultado do AuthenticateAsync - {(utilizador != null ? "Sucesso" : "Falha")}");

        if (utilizador != null)
        {
            Console.WriteLine($"AuthController: Utilizador {utilizador.username} autenticado com sucesso");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, utilizador.id.ToString()),
                new Claim(ClaimTypes.Name, utilizador.username),
                new Claim("Cliente", utilizador.cliente.ToString()),
                new Claim("Role", utilizador.role.ToString())
            };

            if (!string.IsNullOrEmpty(utilizador.nome))
            {
                claims.Add(new Claim(ClaimTypes.GivenName, utilizador.nome));
            }

            if (!string.IsNullOrEmpty(utilizador.email))
            {
                claims.Add(new Claim(ClaimTypes.Email, utilizador.email));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(5)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return LocalRedirect(returnUrl);
        }

        Console.WriteLine($"AuthController: Falha na autenticação para {username}");
        return RedirectToPage("/login", new { error = "Credenciais inválidas" });
    }

    [HttpGet("signout")]
    public async Task<IActionResult> SignOut([FromQuery] string returnUrl = "/login")
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return LocalRedirect(returnUrl);
    }
}