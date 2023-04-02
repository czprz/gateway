namespace Gateway.Routing.Endpoints.Models;

public enum LoadBalancingPolicy
{
    PowerOfTwoChoices,
    FirstAlphabetical,
    RoundRobin,
    LeastRequests,
    Random
}