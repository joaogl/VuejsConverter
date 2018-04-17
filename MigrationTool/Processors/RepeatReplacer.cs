using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MigrationTool.Helpers;

namespace MigrationTool.Processors
{

    /**
     * Solves the following vuejs warning:
     * [Vue warn]: {DEPRECATION} v-repeat will be deprecated in favor of v-for in 1.0.0. See https://github.com/yyx990803/vue/issues/1200 for details.
     */
    public class RepeatReplacer : ProcessorInterface
    {

        /**
         * Indicate which file types this processor requires to inspect. 
         */
        public List<string> FileTypesToAffect()
        {
            // Since this processor only treats the v-class bind this is a html requirement only.
            return new List<string>(new string[] { ".html", ".ts" });
        }

        /**
         * From vuejs warning we can't get much information, but after a few test ew understood 
         * that the v-on="submit: " bind is no longer used, instead v-on:submit="" is used.
         */
        public bool Execute(string fileName, ref string contents)
        {
            
            if (contents.Contains("v-repeat=\""))
            {
                // This is a dynamic template so use :is binding.
                contents = contents.Replace("v-repeat=\"", "v-for=\"");

                return true;
            }

            return false;
        }

    }

}
