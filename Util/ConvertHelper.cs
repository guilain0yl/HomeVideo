using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml;

namespace Util
{
    public static class ConvertHelper
    {
        public static int ToInt(this object obj)
            => Convert.ToInt32(obj);

        public static int SecToInt(this string str, int para = default)
            => int.TryParse(str.ToString(), out int result) ? result : para;

        public static decimal ToDecimal(this object obj)
            => Convert.ToDecimal(obj);

        public static decimal SecToDecimal(this string str, decimal para = default)
            => decimal.TryParse(str, out var result) ? result : para;

        public static bool ToBool(this object obj)
            => Convert.ToBoolean(obj);

        public static bool SecToBool(this string str)
        {
            if (str == null) return false;

            if (decimal.TryParse(str, out decimal result))
            {
                return result != 0m;
            }

            return str.Length > 0;
        }

        public static object ToTargetType(this object obj, Type type)
        {
            if (type == null) return obj;

            if (obj == null)
                return type.IsValueType ? Activator.CreateInstance(type) : null;

            if (type.IsAssignableFrom(obj.GetType()))
                return obj;

            Type underlyingType = Nullable.GetUnderlyingType(type);

            if ((underlyingType ?? type).IsEnum)
            {
                if (underlyingType != null && string.IsNullOrEmpty(obj.ToString()))
                {
                    return null;
                }

                return Enum.Parse(underlyingType ?? type, obj.ToString());
            }

            if (typeof(IConvertible).IsAssignableFrom(underlyingType ?? type))
            {
                try
                {
                    Convert.ChangeType(obj, underlyingType ?? type, null);
                }
                catch
                {
                    return underlyingType == null ? Activator.CreateInstance(type) : null;
                }
            }

            TypeConverter converter = TypeDescriptor.GetConverter(type);
            if (converter.CanConvertFrom(obj.GetType()))
                return converter.ConvertFrom(obj);

            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            if (constructor != null)
            {
                object o = constructor.Invoke(null);
                PropertyInfo[] propertys = type.GetProperties();
                Type oldType = obj.GetType();
                foreach (PropertyInfo property in propertys)
                {
                    PropertyInfo p = oldType.GetProperty(property.Name);
                    if (property.CanWrite && p != null && p.CanRead)
                    {
                        property.SetValue(o, ToTargetType(p.GetValue(obj, null), property.PropertyType), null);
                    }
                }
                return o;
            }

            return obj;
        }

        /// <summary>
        /// 默认排除非空属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="jsonSerializerOptions"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, JsonSerializerOptions jsonSerializerOptions = null)
            => JsonSerializer.Serialize(obj, jsonSerializerOptions ?? new JsonSerializerOptions { IgnoreNullValues = true });

        public static T ToT<T>(this string str)
            => JsonSerializer.Deserialize<T>(str);

        public static object ToT(this string str, Type returnType)
            => JsonSerializer.Deserialize(str, returnType);

        //public static string ToXml<T>(this T @class, bool IsIgnoreNull = true)
        //{
        //    XmlDocument document = new XmlDocument();
        //    XmlElement rootNode = document.CreateElement("xml");

        //    var properties = typeof(T).GetProperties();
        //    foreach (var item in properties)
        //    {
        //        // 设置忽略或者此属性不可读
        //        if (item.GetCustomAttribute(typeof(XmlIgnoreAttribute)) != null || !item.CanRead)
        //            continue;

        //        var value = item.GetValue(@class);
        //        if (IsIgnoreNull && value == null)
        //            continue;

        //        var element = document.CreateNode(XmlNodeType.Element, item.Name, null);
        //        element.InnerText = value == null ? string.Empty : value.ToString();
        //        rootNode.AppendChild(element);
        //    }

        //    document.AppendChild(rootNode);

        //    return ConvertXmlToString(document);
        //}

        //private static string ConvertXmlToString(XmlDocument xmlDoc)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    XmlTextWriter writer = new XmlTextWriter(stream, null);
        //    writer.Formatting = Formatting.Indented;
        //    xmlDoc.Save(writer);
        //    StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
        //    stream.Position = 0;
        //    string xmlString = sr.ReadToEnd();
        //    sr.Close();
        //    writer.Close();
        //    stream.Close();
        //    return xmlString;
        //}


        //private static string[] CHNumber = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖", "仟", "佰", "拾", "", "万", "亿", "圆", "角", "分", "整" };
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class XmlIgnoreAttribute : Attribute
    {
    }
}
