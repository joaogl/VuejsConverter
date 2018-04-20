using MigrationTool.Helpers;
using MigrationTool.Models;
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
     * [Vue warn]: {DEPRECATION} v-style will be deprecated in 1.0.0. Use v-bind:style or just :style instead. See https://github.com/yyx990803/vue/issues/1325 for details.
     */
    public class StyleReplacer : ProcessorInterface
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
            if (contents.Contains("v-style=\""))
            {
                var tags = FileHelper.GetAllTags(ref contents, "v-style=\"");

                // Convertion happens automatically by the ExportHtml of the StyleAttribute
                FileHelper.Apply(ref contents, tags);

                return true;
            }

            return false;
        }

    }

}
