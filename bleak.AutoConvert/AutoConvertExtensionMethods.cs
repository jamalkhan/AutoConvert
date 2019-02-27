using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace bleak.AutoConvert
{
    public static class AutoConvertExtensionMethods
    {
        /// <summary>
        /// Converts an input string into a scalar type, such as an integer
        /// </summary>
        /// <returns>The convert.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T AutoConvert<T>(this string input)
        {
            try
            {
                TypeConverter tc = TypeDescriptor.GetConverter(typeof(T));
                return (T)tc.ConvertFrom(input);
            }
            catch { }
            var type = typeof(T);
            if (!type.IsValueType || Nullable.GetUnderlyingType(type) != null)
            {
                return default(T);
            }
            throw new ArgumentOutOfRangeException($"AutoConvert {input} to {type.Name} failed");
        }

        /// <summary>
        /// Registers the type converter. Generally, does not need to be called by a developer
        /// </summary>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <typeparam name="TC">The 2nd type parameter.</typeparam>
        public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
        {
            TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
        }

        /// <summary>
        /// Converts a Dictionary to an Object
        /// </summary>
        /// <returns>The convert.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <remarks>Does not recurse, therefore does not AutoConvert subclasses</remarks>
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
                        SetValue(output, convertProperty, kv.Value.ToString());
                    }
                }
            }
            return (T)output;
        }

        private static void SetValue(object output, PropertyDescriptor convertProperty, string value)
        {
            if (convertProperty.PropertyType.IsEnum)
            {
                convertProperty.SetValue(output, Enum.Parse(convertProperty.PropertyType, value: value, ignoreCase: true));
            }
            else if (convertProperty.PropertyType.Name == "Nullable`1")
            {
                var genericType = convertProperty.PropertyType.GenericTypeArguments.FirstOrDefault();
                if (genericType != null && genericType.IsEnum)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        convertProperty.SetValue(output, Enum.Parse(genericType, value: value, ignoreCase: true));
                    }
                }
                else
                {
                    convertProperty.SetValue(output, Convert.ChangeType(value, genericType));
                }
            }
            else
            {
                convertProperty.SetValue(output, Convert.ChangeType(value, convertProperty.PropertyType));
            }
        }

        /// <summary>
        /// Converts an Object to another type with properties of the same name.
        /// </summary>
        /// <returns>The convert.</returns>
        /// <param name="input">Input.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <remarks>Does not recurse, therefore does not AutoConvert subclasses</remarks>
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
                        SetValue(output, convertProperty, entityProperty.GetValue(input).ToString());
                    }
                }
            }
            return output;
        }
    }
}
