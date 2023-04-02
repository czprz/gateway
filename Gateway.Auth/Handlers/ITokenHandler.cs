using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Auth.Handlers;

public interface ITokenHandler
{
    void HandleToken(TokenValidatedContext context);
}