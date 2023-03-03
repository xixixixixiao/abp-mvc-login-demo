using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Acme.BookStore.Web.Pages;

public class LoginModel : PageModel
{
    public async Task<IActionResult> OnGet()
    {
        using var http = new HttpClient();
        var configuration = await http.GetDiscoveryDocumentAsync("https://localhost:44397");
        if (configuration.IsError)
        {
            throw new Exception(configuration.Error);
        }

        var passwordTokenRequest = new PasswordTokenRequest
        {
            Address = configuration.TokenEndpoint,
            ClientId = "BookStore_App",
            UserName = "admin",
            Password = "1q2w3E*",
            Scope = string.Join(" ", new[]
            {
                "address",
                "email",
                "phone",
                "profile",
                "roles",
                "BookStore"
            }),
        };
        var tokenResponse = await http.RequestPasswordTokenAsync(passwordTokenRequest);
        if (tokenResponse.IsError)
        {
            throw new Exception(tokenResponse.Error);
        }

        if (string.IsNullOrEmpty(tokenResponse.AccessToken))
        {
            throw new ArgumentNullException(nameof(tokenResponse.AccessToken));
        }

        var userInfoRequest = new UserInfoRequest
        {
            Address = configuration.UserInfoEndpoint,
            Token = tokenResponse.AccessToken,
        };
        var userInfo = await http.GetUserInfoAsync(userInfoRequest);
        var principal = new ClaimsPrincipal(
            new ClaimsIdentity(userInfo.Claims, CookieAuthenticationDefaults.AuthenticationScheme));
        var props = new AuthenticationProperties();

        props.StoreTokens(new[]
        {
            new AuthenticationToken { Name = "access_token", Value = tokenResponse.AccessToken }
        });
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

        return Redirect("/");
    }
}