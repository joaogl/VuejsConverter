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
     * [Vue warn]: {DEPRECATION} v-el will no longer take an attribute value in 1.0.0. Use "v-el:id" syntax instead. Also, nodes will be registered under vm.$els instead of vm.$$. See https://github.com/yyx990803/vue/issues/1292 for more details.
     */
    public class VElReplacer : ProcessorInterface
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
            if (contents.Contains("v-el=\""))
            {
                var tags = FileHelper.GetAllTags(ref contents, "v-el=\"");

                // Convertion happens automatically by the ExportHtml of the VElAttribute
                FileHelper.Apply(ref contents, tags);

                return true;
            }

            return false;
        }

    }

}
