namespace Gateway.Components.Routing.Models;

public enum HeaderMatchMode
{
    ExactHeader = 0,
    HeaderPrefix = 1,
    Contains = 2,
    NotContains = 3,
    Exists = 4,
    NotExists = 5,
}