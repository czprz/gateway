namespace Gateway.Routing.Services;

public enum ProxyManagerResult
{
    Ok,
    AlreadyExists,
    OneOrMoreDidNotExist,
    NotFound,
    Error
}