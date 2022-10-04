using System.ComponentModel;
using System.Reflection;

namespace Entity.Enums
{
    public static class EnumExtension
    {
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string GetValue(this System.Enum value)
        {
            return value.GetAttribute<ValueAttribute>().Value;
        }

        public static TAttribute GetAttribute<TAttribute>(this System.Enum value) where TAttribute : Attribute
        {
            var enumType = value.GetType();
            var name = System.Enum.GetName(enumType, value);
            return enumType.GetField(name).GetCustomAttributes(false).OfType<TAttribute>().SingleOrDefault();
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class ValueAttribute : Attribute
    {
        public string Value { get; }

        public ValueAttribute(string value)
        {
            Value = value;
        }

    }
}
