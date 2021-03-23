using System;
using System.Linq;
using System.Reflection;

namespace SupplierPlatform.Extension
{
    public static class ObjectExtension
    {
        public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
        {
            Type attributeType = typeof(T);
            PropertyInfo property = instance.GetType().GetProperty(propertyName);

            return property == null ? null : (T)property.GetCustomAttributes(attributeType, false).First();
        }
    }
}