namespace Gateway.Routing.Models;

public enum LoadBalancingPolicy
{
    PowerOfTwoChoices,
    FirstAlphabetical,
    RoundRobin,
    LeastRequests,
    Random
}