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
     * [Vue warn]: {DEPRECATION} v-transition will no longer be a directive in 1.0.0; It will become a special attribute without the prefix. Use "transition" instead.
     */
    public class TransitionReplacer : ProcessorInterface
    {

        /**
         * Indicate which file types this processor requires to inspect. 
         */
        public List<string> FileTypesToAffect()
        {
            // Since this processor only treats the transition bind this is a html requirement only.
            return new List<string>(new string[] { ".html", ".ts" });
        }

        /**
         * From vuejs warning we can't get much information, but after a few test ew understood 
         * that the v-on="submit: " bind is no longer used, instead v-on:submit="" is used.
         */
        public bool Execute(string fileName, ref string contents)
        {
            if (contents.Contains("v-transition=\""))
            {
                contents = contents.Replace("v-transition=\"", "transition=\"");

                return true;
            }

            return false;
        }

    }

}
