using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MigrationTool.Processors
{

    /**
     * Solves the following vuejs warning:
     * [Vue warn]: Invalid expression. Generated function body:  ({scope.view)+"}"
     */
    public class DynamicTemplateBinderReplacer : ProcessorInterface
    {

        /**
         * Indicate which file types this processor requires to inspect. 
         */
        public List<string> FileTypesToAffect()
        {
            // Since this processor only treats the component bind this is a html and ts requirement only.
            // Note that dynamically created content might exists and thats why we are using ts files also.
            return new List<string>(new string[] { ".html", ".ts" });
        }

        /**
         * From vuejs warning we can understand that the only thing we need to convert this is to replace 
         * every <component is="{{view}}"> by <component v-bind:is="view"> or <component :is="view"> 
         * in our case we prefer to use the <component :is="view">.
         */
        public bool Execute(string fileName, ref string contents)
        {
            /*
             * We are checking for component is=" and not <component is=\" because in the menu these components are added dynamically without the <
             */
            if (contents.Contains("component is=\"{{"))
            {
                string expression = @"(:is=.)({{2})(\s.*)(}{2})(.)";

                // This is a dynamic template so use :is binding.
                contents = contents.Replace("component is=\"{{", "component :is=\"{{");

                MatchCollection result = Regex.Matches(contents, expression, RegexOptions.IgnoreCase);

                int offset = 0;

                foreach (Match match in result)
                {
                    contents = contents.Remove(match.Groups[2].Index - offset, match.Groups[2].Length);
                    offset += match.Groups[2].Length;
                    contents = contents.Remove(match.Groups[4].Index - offset, match.Groups[4].Length);
                    offset += match.Groups[4].Length;
                }

                return true;
            }

            return false;
        }

    }

}
