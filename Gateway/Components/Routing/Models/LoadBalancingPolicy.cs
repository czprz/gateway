namespace Gateway.Components.Routing.Models;

public enum LoadBalancingPolicy
{
    PowerOfTwoChoices,
    FirstAlphabetical,
    RoundRobin,
    LeastRequests,
    Random
}