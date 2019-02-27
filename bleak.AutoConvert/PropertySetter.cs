using System;
using System.ComponentModel;
using System.Linq;

namespace bleak.AutoConvert
{

    public static class PropertySetter
    {
        public static void SetValue(object output, PropertyDescriptor propertyDescriptor, string value)
        {
            if (propertyDescriptor.PropertyType.Name == "Nullable`1")
            {
                var genericType = propertyDescriptor.PropertyType.GenericTypeArguments.FirstOrDefault();
                SetValue(output, propertyDescriptor, genericType, value);
            }
            else
            {
                SetValue(output, propertyDescriptor, propertyDescriptor.PropertyType, value);
            }
        }

        public static void SetValue(object output, PropertyDescriptor propertyDescriptor, Type propertyType, string value)
        {
            if (propertyType.IsEnum)
            {
                propertyDescriptor.SetValue(output, Enum.Parse(propertyType, value: value, ignoreCase: true));
            }
            else if (propertyType.Name == "Guid")
            {
                propertyDescriptor.SetValue(output, Guid.Parse(value));
            }
            else
            {
                propertyDescriptor.SetValue(output, Convert.ChangeType(value, propertyType));
            }
        }
    }
}