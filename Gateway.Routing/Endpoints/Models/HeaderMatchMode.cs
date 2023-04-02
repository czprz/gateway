namespace Gateway.Routing.Endpoints.Models;

public enum HeaderMatchMode
{
    ExactHeader,
    HeaderPrefix,
    Contains,
    NotContains,
    Exists,
    NotExists,
}