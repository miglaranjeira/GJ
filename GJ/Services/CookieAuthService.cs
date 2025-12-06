using GJ.Models;
using GJ.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace GJ.Services;

public class CookieAuthService
{
    private readonly AuthService _authService;
    private readonly NavigationManager _navigationManager;

    public CookieAuthService(
        AuthService authService,
        NavigationManager navigationManager)
    {
        _authService = authService;
        _navigationManager = navigationManager;
    }

    public async Task<string> LoginAsync(string username, string password)
    {
        var utilizador = await _authService.AuthenticateAsync(username, password);

        if (utilizador != null)
        {
            // Retorna URL para redirecionamento com parâmetros
            var returnUrl = "/terceiros";
            return $"/auth/signin?username={username}&returnUrl={returnUrl}";
        }

        return null;
    }

    public string GetLogoutUrl()
    {
        return "/auth/signout";
    }
}
