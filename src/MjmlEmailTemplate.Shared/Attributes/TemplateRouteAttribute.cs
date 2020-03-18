using System;

namespace MjmlEmailTemplate.Shared.Attributes
{
    public class TemplateRouteAttribute : Attribute
    {
        public string Route { get; }
        public TemplateRouteAttribute(string route)
        {
            Route = route;
        }
    }
}