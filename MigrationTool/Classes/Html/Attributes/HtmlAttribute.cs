using System.Text.RegularExpressions;

namespace MigrationTool.Models
{
    
    public class HtmlAttribute
    {

        public HtmlAttributeType attributeType { get; set; }
        public string attribute { get; set; }
        
        public string rawAttribute { get; set; }
        public string rawValue { get; set; }

        public HtmlAttribute(string raw, string attr, string value)
        {
            rawAttribute = raw;
            attribute = attr;
            rawValue = value; 
        }

        public virtual string ExportHtml()
        {
            return rawAttribute;
        }
        
    }
    
}