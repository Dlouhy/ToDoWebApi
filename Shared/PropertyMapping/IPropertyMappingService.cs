namespace Shared.PropertyMapping;

public interface IPropertyMappingService
{
    Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();

    bool ValidMappingExistsFor<TSource, TDestination>(string fields);
}