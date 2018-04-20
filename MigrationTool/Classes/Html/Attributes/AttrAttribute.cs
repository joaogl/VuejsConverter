using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigrationTool.Models
{
    
    public class AttrAttribute : HtmlAttribute
    {

        public AttrAttribute(string raw, string attr, string value) : base(raw, attr, value)
        {
        }

        public override string ExportHtml()
        {
            var output = "";
            var attrs = rawValue.Split(',');

            foreach (var attr in attrs)
            {
                var tuple = attr.Split(':');

                output += ":" + tuple[0].Trim() + "=\"" + tuple[1].Trim() + "\"";
            }

            return output;
        }

    }
    
}