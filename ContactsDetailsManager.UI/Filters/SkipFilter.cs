using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactDetailsManager.Filters
{
    public class SkipFilter: Attribute, IFilterMetadata
    {
    }
}
