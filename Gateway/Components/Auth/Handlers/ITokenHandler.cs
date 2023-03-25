using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Components.Auth.Handlers;

public interface ITokenHandler
{
    void HandleToken(TokenValidatedContext context);
}