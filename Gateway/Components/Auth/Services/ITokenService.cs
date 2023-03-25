namespace Gateway.Components.Auth.Services;

public interface ITokenService
{
    Task AddToken(HttpContext ctx);
}