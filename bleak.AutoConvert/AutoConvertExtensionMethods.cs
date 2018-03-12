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
