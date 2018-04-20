using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigrationTool.Models
{
    
    public class PlaceholderAttribute : HtmlAttribute
    {

        public PlaceholderAttribute(string raw, string attr, string value) : base(raw, attr, value)
        {
        }

        public override string ExportHtml()
        {
            if (rawValue.Contains("{{") && rawValue.Contains("}}"))
            {
                var output = ":" + attribute.Trim() + "=\"";

                rawValue = rawValue.Replace("{{", "").Trim();
                rawValue = rawValue.Replace("}}", "").Trim();

                output += rawValue + "\"";

                return output;
            }

            return rawAttribute;
        }

    }
    
}