using System.ComponentModel;
using System.Reflection;

namespace CleanArchAcceleratorTools.Domain.Util
{
    public static class EnumExtensions
    {
        public static string? GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value) ?? string.Empty;
            
            if (!string.IsNullOrEmpty(name))
            {
                FieldInfo? field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute? attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }

            return null;
        }
    }
}
