namespace Gateway.Routing.Models;

public enum QueryParameterMatchMode
{
    Exact = 0,
    Contains = 1,
    NotContains = 2,
    Prefix = 3,
    Exists = 4
}