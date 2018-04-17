using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MigrationTool.Helpers;
using MigrationTool.Models;

namespace MigrationTool.Processors
{

    /**
     * Migrates from the old v-on which had everything inside it to sepeared v-on attributes.
     */
    public class OnDirectiveReplacer : ProcessorInterface
    {

        /**
         * Indicate which file types this processor requires to inspect. 
         */
        public List<string> FileTypesToAffect()
        {
            // Since this processor only treats the v-on bind this is a html and ts requirement.
            return new List<string>(new string[] { ".html", ".ts" });
        }

        /**
         * From vuejs warning we can't get much information, but after a few test we understood 
         * that the v-on="click: something" bind is no longer used, instead v-on:click="something" is used.
         */
        public bool Execute(string fileName, ref string contents)
        {
            if (contents.Contains("v-on=\""))
            {
                var tags = FileHelper.GetAllTags(ref contents, "v-on=\"");

                foreach (HtmlTag tag in tags)
                {
                    foreach (HtmlAttribute attr in tag.Attributes)
                    {
                        if (attr.GetType() == typeof(VOnOldAttribute))
                        {
                            ((VOnOldAttribute) attr).ConvertToNewVOn();
                        }
                    }
                }

                FileHelper.Apply(ref contents, tags);
                
                return true;
            }

            return false;
        }

    }

}
