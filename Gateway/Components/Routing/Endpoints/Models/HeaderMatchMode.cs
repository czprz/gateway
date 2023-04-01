namespace Gateway.Components.Routing.Endpoints.Models;

public enum HeaderMatchMode
{
    ExactHeader,
    HeaderPrefix,
    Contains,
    NotContains,
    Exists,
    NotExists,
}