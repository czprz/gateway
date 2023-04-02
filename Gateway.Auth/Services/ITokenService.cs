using Microsoft.AspNetCore.Http;

namespace Gateway.Auth.Services;

public interface ITokenService
{
    Task AddToken(HttpContext ctx);
}