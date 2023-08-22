using Gateway.Routing.Models;
using Gateway.Routing.Storage.Rational.Models;

namespace Gateway.Routing.Storage.Rational.Maps;

public static class MapFromTransformDbsToTransforms
{
    public static Transforms Map(this ICollection<TransformDb>? collection)
    {
        if (collection == null)
        {
            return new Transforms();
        }
        
        var transforms = new Transforms();

        if (!collection.Any())
        {
            return transforms;
        }

        return collection.Aggregate(transforms, (tf, db) =>
        {
            AddRequestTransform(tf, db);
            
            return tf;
        });
    }

    private static void AddRequestTransform(Transforms transforms, TransformDb db)
    {
        transforms.RequestTransform = new RequestTransform();

        switch (db.Type)
        {
            case TransformTypeDb.PathPrefix:
                transforms.RequestTransform.PathPrefix = db.Values.First().Value;
                break;
            case TransformTypeDb.PathRemovePrefix:
                transforms.RequestTransform.PathRemovePrefix = db.Values.First().Value;
                break;
            case TransformTypeDb.PathSet:
                transforms.RequestTransform.PathSet = db.Values.First().Value;
                break;
            case TransformTypeDb.XForwarded:
                CreateXForwarded(transforms, db);
                break;
            case TransformTypeDb.Forwarded:
                CreateForwarded(transforms, db);
                break;
        }
    }

    private static void CreateXForwarded(Transforms transforms, TransformDb db)
    {
        var values = db.Values.ToDictionary(x => x.Key, transformValues => transformValues.Value);
        
        transforms.RequestTransform!.XForwarded = new XForwarded
        {
            Action = Get(values, "Action") ?? "",
            For = Get(values, "For"),
            Host = Get(values, "Host"),
            Proto = Get(values, "Proto"),
            Prefix = Get(values, "Prefix"),
            HeaderPrefix = Get(values, "HeaderPrefix")
        };
    }

    private static void CreateForwarded(Transforms transforms, TransformDb db)
    {
        var values = db.Values.ToDictionary(x => x.Key, transformValues => transformValues.Value);
        
        transforms.RequestTransform!.Forwarded = new Forwarded
        {
            Values = Get(values, "Values") ?? "",
            ForFormat = Get(values, "ForFormat"),
            ByFormat = Get(values, "ByFormat"),
            Action = Get(values, "Action")
        };
    }

    private static string? Get(IReadOnlyDictionary<string, string> values, string key)
    {
        return values.TryGetValue(key, out var value) ? value : default;
    }
}