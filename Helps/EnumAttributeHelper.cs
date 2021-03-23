using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SupplierPlatform.Helps
{
    public class EnumAttributeHelper
    {
        /// <summary>
        /// 取得 Enum 的 Description
        /// </summary>
        /// <param name="value">Enum</param>
        /// <returns>Enum 的 Description</returns>
        public static string GetEnumDescription(System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute customAttribute = field.GetCustomAttribute<DescriptionAttribute>(false);

            if (customAttribute != null)
            {
                string name = string.IsNullOrWhiteSpace(customAttribute.Description) ? string.Empty : customAttribute.Description;
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return value.ToString();
        }

        /// <summary>
        /// 取得 Enum 的 DisplayName
        /// </summary>
        /// <param name="value">Enum</param>
        /// <returns>Enum 的 DisplayName</returns>
        public static string GetEnumDisplayName(System.Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DisplayAttribute customAttribute = field.GetCustomAttribute<DisplayAttribute>(false);
            if (customAttribute != null)
            {
                string name = string.IsNullOrWhiteSpace(customAttribute.GetName()) ? string.Empty : customAttribute.GetName();
                if (!string.IsNullOrEmpty(name))
                {
                    return name;
                }
            }
            return value.ToString();
        }
    }
}