namespace Gateway.Routing.Storage.Rational.Models;

public enum LoadBalancingPolicyDb
{
    PowerOfTwoChoices = 0,
    FirstAlphabetical = 1,
    RoundRobin = 2,
    LeastRequests = 3,
    Random = 4
}