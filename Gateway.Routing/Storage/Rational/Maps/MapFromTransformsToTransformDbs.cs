using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Storage.Rational.Maps;

public static class MapFromTransformsToTransformDbs
{
    public static ICollection<TransformDb> Map(this Transforms? transforms)
    {
        if (transforms == null)
        {
            return new List<TransformDb>();
        }

        var transformDbs = new List<TransformDb>();

        AddTransform(transformDbs, TransformTypeDb.PathPrefix, transforms.RequestTransform?.PathPrefix);
        AddTransform(transformDbs, TransformTypeDb.PathRemovePrefix, transforms.RequestTransform?.PathRemovePrefix);
        AddTransform(transformDbs, TransformTypeDb.PathSet, transforms.RequestTransform?.PathSet);

        return transformDbs;
    }

    private static void AddTransform(ICollection<TransformDb> transformDbs, TransformTypeDb type, string? value)
    {
        if (value == null)
        {
            return;
        }

        transformDbs.Add(new()
        {
            Values = new List<TransformValuesDb> { new() { Key = type.ToKey(), Value = value } }
        });
    }
}