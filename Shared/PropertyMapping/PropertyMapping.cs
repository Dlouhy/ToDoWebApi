namespace Shared.PropertyMapping;

public class PropertyMapping<TSource, TDestination> : IPropertyMapping
{
    public Dictionary<string, PropertyMappingValue> MappingDictionary { get; }

    public PropertyMapping(Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        MappingDictionary = mappingDictionary ?? throw new ArgumentNullException(nameof(mappingDictionary));
    }
}