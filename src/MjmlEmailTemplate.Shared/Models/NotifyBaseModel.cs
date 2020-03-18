using System;
using System.Linq;
using MjmlEmailTemplate.Shared.Attributes;

namespace MjmlEmailTemplate.Shared.Models
{
    public class NotifyBaseModel
    {
        private static readonly Type NotificationRouteAttributeType = typeof(TemplateRouteAttribute);

        public string GetRoute()
        {
            var type = GetType();

            var attribute = type.GetCustomAttributes(NotificationRouteAttributeType, false).OfType<TemplateRouteAttribute>().FirstOrDefault();

            return attribute?.Route;
        }
    }
}