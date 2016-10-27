using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CustomExtensions
{
    public static class StringExtension
    {
        public static string ToCpf(this string value)
        {
            if (value == null)
                return "";
            if (value.Length >= 9)
            {
                value = value.Insert(9, "-");
                value = value.Insert(6, ".");
                value = value.Insert(3, ".");
                return value;
            }
            return value;
        }
        public static string ToTelefone(this string value)
        {
            if (value == null)
                return "";
            if (value.Length >= 6)
            {
                if (value.Length == 10)
                {
                    value = value.Insert(6, "-");
                    value = value.Insert(2, ") ");
                    value = value.Insert(0, "(");
                    return value;
                }
                else
                {
                    value = value.Insert(7, "-");
                    value = value.Insert(2, ") ");
                    value = value.Insert(0, "(");
                    return value;
                }
            }
            return value;
        }
      
        public static string ToCep(this string value)
        {
            if (value == null)
                return "";
            if (value.Length >= 5)
            {
                value = value.Insert(5, "-");
                value = value.Insert(2, ".");            
                return value;
            }
            return value;
        }

        public static string ToCnpj(this string value)
        {
            if (value == null)
                return "";
            if (value.Length >= 12)
            {
                value = value.Insert(12, "-");
                value = value.Insert(8, "/");
                value = value.Insert(5, ".");
                value = value.Insert(2, ".");
                return value;
            }
            return value;
        }

        
    }
}
