namespace Gateway.Common.Extensions;

public static class StringToGuidExtension
{
    public static Guid ToGuid(this string value)
    {
        return Guid.Parse(value);
    }
}