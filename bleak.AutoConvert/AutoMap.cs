using System;
using System.ComponentModel;
using System.Linq;

namespace bleak.AutoConvert
{
    public static class AutoMap
    {
        public static void Update(object input, object output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("sourceObject");
            }
            if (output == null)
            {
                throw new ArgumentNullException("destinationObject");
            }
            
            var convertProperties = TypeDescriptor.GetProperties(output.GetType()).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(input.GetType()).Cast<PropertyDescriptor>();
            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name);
                if (convertProperty != null)
                {
                    if (entityProperty.GetValue(input) != null)
                    {
                        PropertySetter.SetValue(output, convertProperty, entityProperty.GetValue(input).ToString());
                    }
                }
            }
        }
    }
}