namespace Gateway.Common.Extensions;

public static class AddIfNotEmptyExtension
{
    public static void AddIfNotEmpty<T>(this IDictionary<string, T> dict, string key, T? value)
    {
        if (dict.ContainsKey(key))
        {
            throw new ArgumentException($"Key {key} already exists in dictionary");
        }
        
        if (value is string str && string.IsNullOrEmpty(str))
        {
            return;
        }

        if (value == null)
        {
            return;
        }
        
        dict[key] = value;
    }
}