namespace Gateway.Components.Routing.Models;

public enum HeaderMatchMode
{
    ExactHeader,
    HeaderPrefix,
    Contains,
    NotContains,
    Exists,
    NotExists,
}