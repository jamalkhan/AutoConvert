using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace bleak.AutoConvert
{
    public static class AutoConvertExtensionMethods
    {
        public static T AutoConvert<T>(this string input)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                return (T)tc.ConvertFrom(input);
            }
            catch (Exception ex)
            {

            }
            var type = typeof(T);
            if (!type.IsValueType || Nullable.GetUnderlyingType(type) != null)
            {
                return default(T);
            }
            throw new ArgumentOutOfRangeException($"AutoConvert {input} to {type.Name} failed");
        }

        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

        public static T AutoConvert<T>(this Dictionary<string, object> input)
        {
            if (input == null)
            {
                return default(T);
            }

            Type type = typeof(T);

            var output = Activator.CreateInstance(type);
            var convertProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();
            foreach (var kv in input)
            {
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == kv.Key);
                if (convertProperty != null)
                {
                    if (kv.Value != null)
                    {
                        if (convertProperty.PropertyType.Name == "Nullable`1")
                        {
                            convertProperty.SetValue(output, Convert.ChangeType(kv.Value, convertProperty.PropertyType.GetGenericArguments().FirstOrDefault()));
                        }
                        else
                        {
                            convertProperty.SetValue(output, Convert.ChangeType(kv.Value, convertProperty.PropertyType));
                        }
                    }
                }
            }
            return (T)output;
        }

        public static T AutoConvert<T>(this object input) where T : new()
        {
            if (input == null)
            {
                return default(T);
            }

            var convertProperties = TypeDescriptor.GetProperties(typeof(T)).Cast<PropertyDescriptor>();
            var entityProperties = TypeDescriptor.GetProperties(input).Cast<PropertyDescriptor>();
            var output = new T();
            foreach (var entityProperty in entityProperties)
            {
                var property = entityProperty;
                var convertProperty = convertProperties.FirstOrDefault(prop => prop.Name == property.Name);
                if (convertProperty != null)
                {
                    if (entityProperty.GetValue(input) != null)
                    {
                        if (convertProperty.PropertyType.Name == "Nullable`1")
                        {
                            convertProperty.SetValue(output, Convert.ChangeType(entityProperty.GetValue(input), convertProperty.PropertyType.GetGenericArguments().FirstOrDefault()));
                        }
                        else
                        {
                            convertProperty.SetValue(output, Convert.ChangeType(entityProperty.GetValue(input), convertProperty.PropertyType));
                        }
                    }
                }
            }
            return output;
        }
    }
}
