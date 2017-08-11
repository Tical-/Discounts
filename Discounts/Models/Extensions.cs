using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Discounts.Models
{
    public class TextAttribute : Attribute
    {
        public string Text;
        public TextAttribute(string text)
        {
            Text = text;
        }
    }

    public static class EnumExtender
    {
        public static string ToText(this Enum enumeration)
        {
            MemberInfo[] memberInfo = enumeration.GetType().GetMember(enumeration.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(TextAttribute), false);
                if (attributes != null && attributes.Length > 0)
                {
                    return ((TextAttribute)attributes[0]).Text;
                }
            }
            return enumeration.ToString();
        }
    }
}