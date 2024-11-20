using Entities;
using Shared.DataTransferObjects;

namespace Shared.PropertyMapping;

public class PropertyMappingService : IPropertyMappingService
{
    private readonly Dictionary<string, PropertyMappingValue> _projectPropertyMapping = new(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new(["Id"]) },
            { "ProjectStart", new(["ProjectStart"]) },
            { "ProjectName", new(["ProjectName"]) }
        };

    private readonly List<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

    public PropertyMappingService()
    {
        _propertyMappings.Add(new PropertyMapping<ProjectDto, Project>(_projectPropertyMapping));
    }

    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        // get matching mapping
        var matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Count() == 1)
        {
            return matchingMapping.First().MappingDictionary;
        }

        throw new ArgumentException($"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}");
    }

    public bool ValidMappingExistsFor<TSource, TDestination>(string fields)
    {
        var propertyMapping = GetPropertyMapping<TSource, TDestination>();

        if (string.IsNullOrWhiteSpace(fields))
        {
            return true;
        }

        // the string is separated by ",", so we split it.
        var fieldsAfterSplit = fields.Split(',');

        // run through the fields clauses
        foreach (var field in fieldsAfterSplit)
        {
            // trim
            var trimmedField = field.Trim();

            // remove everything after the first " " - if the fields are coming from an orderBy
            // string, this part must be ignored
            int indexOfFirstSpace = trimmedField.IndexOf(' ');
            var propertyName = indexOfFirstSpace == -1 ?
                trimmedField : trimmedField.Remove(indexOfFirstSpace);

            // find the matching property
            if (!propertyMapping.ContainsKey(propertyName))
            {
                return false;
            }
        }
        return true;
    }
}