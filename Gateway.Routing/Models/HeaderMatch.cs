namespace Gateway.Routing.Models;

public class HeaderMatch
{
    public string Name { get; init; }
    public IReadOnlyList<string>? Values { get; init; }
    public HeaderMatchMode Mode { get; init; }
    public bool IsCaseSensitive { get; init; }
    
    public override int GetHashCode()
    {
        var hash = 17;
        hash = hash * 23 + Name.GetHashCode();
        hash = (Values ?? Array.Empty<string>()).Aggregate(hash, (current, value) => current * 23 + value.GetHashCode());
        hash = hash * 23 + Mode.GetHashCode();
        hash = hash * 23 + IsCaseSensitive.GetHashCode();
        return hash;
    }
}