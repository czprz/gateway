using System.Net.Http.Headers;
using System.Net.Http.Json;
using AutoMapper;
using Gateway.Auth.Util.Models;
using Gateway.Common.Config;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace Gateway.Auth.Util;

public class AuthorityFacade : IAuthorityFacade
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfig _config;
    private readonly IMapper _mapper;

    private DiscoveryDocument _discoveryDocument = new();

    public AuthorityFacade(IHttpClientFactory httpClientFactory, IConfig config, IMapper mapper)
    {
        _httpClientFactory = httpClientFactory;
        _config = config;
        _mapper = mapper;

        LoadDiscoveryDocument();
    }

    private async void LoadDiscoveryDocument()
    {
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        var doc = await client.GetFromJsonAsync<DiscoveryDocument>(_config.Authority.DiscoveryUrl);

        _discoveryDocument = doc ?? throw new Exception("Unable to load discovery document.");
    }

    public async Task<TokenResponse?> GetToken(Dictionary<string, string> payload)
    {
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        var encodedPayload = new FormUrlEncodedContent(payload);
        
        var tokenEndpoint = _discoveryDocument.TokenEndpoint.LocalPath;
        var response = await client.PostAsync(tokenEndpoint, encodedPayload);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }

    public void Logout(RedirectContext context)
    {
        var logoutUrl = _discoveryDocument.EndSessionEndpoint.AbsoluteUri;
        context.Response.Redirect(logoutUrl);
        context.HandleResponse();
    }

    public async Task<UserInfo?> GetUserInfo(string accessToken)
    {
        var client = _httpClientFactory.CreateClient("authority_endpoint");
        
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var userInfoEndpoint = _discoveryDocument.UserInfoEndpoint.LocalPath;
        var response = await client.GetAsync(userInfoEndpoint);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }
        
        var responseUserInfo = await response.Content.ReadFromJsonAsync<UserInfoResponse>();
        var userInfo = _mapper.Map<UserInfo>(responseUserInfo);

        return userInfo;
    }
}