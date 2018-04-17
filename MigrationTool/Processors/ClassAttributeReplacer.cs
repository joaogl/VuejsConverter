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
     * [Vue warn]: {DEPRECATION} v-class will be deprecated in 1.0.0. Use v-bind:class or just :class instead. See https://github.com/yyx990803/vue/issues/1325 for details.
     */
    public class ClassAttributeReplacer : ProcessorInterface
    {

        /**
         * Indicate which file types this processor requires to inspect. 
         */
        public List<string> FileTypesToAffect()
        {
            // Since this processor only treats the v-class bind this is a html requirement only.
            return new List<string>(new string[] { ".html" });
        }

        /**
         * From vuejs warning we can't get much information, but after a few test ew understood 
         * that the v-on="submit: " bind is no longer used, instead v-on:submit="" is used.
         */
        public bool Execute(string fileName, ref string contents)
        {
            if (contents.Contains("v-class=\""))
            {
                var tags = FileHelper.GetAllTags(ref contents, "v-class=\"");

                foreach (HtmlTag tag in tags)
                {
                    for (int i = 0; i < tag.Attributes.Count; i++)
                    {
                        var attr = tag.Attributes[i];
                        if (attr.GetType() == typeof(VClassOldAttribute))
                        {
                            if (((VClassOldAttribute)attr).rulessClasses.Count > 0 && ((VClassOldAttribute)attr).classes.Count == 0)
                            {
                                ((VClassOldAttribute)attr).IgnoreAttribute();
                            }

                            foreach (string cssClass in ((VClassOldAttribute)attr).rulessClasses)
                            {
                                var classAttr = tag.Attributes.Where(x => x.attribute == "class").FirstOrDefault();

                                if (classAttr == null)
                                {
                                    classAttr = new ClassAttribute("class=\"\"", "class", "");
                                    tag.Attributes.Add(classAttr);
                                }

                                if (classAttr.rawValue.Length > 0)
                                {
                                    classAttr.rawValue += " " + cssClass;
                                }
                                else
                                {
                                    classAttr.rawValue += cssClass;
                                }
                            }

                            ((VClassOldAttribute)attr).ConvertToNewVOn();
                        }
                    }
                }

                FileHelper.Apply(ref contents, tags);

                /*
                string expression = "(:class=.)([^{}])(\\S.*)(\")";

                // This is a dynamic template so use :is binding.
                contents = contents.Replace("v-class=\"", ":class=\"");

                MatchCollection result = Regex.Matches(contents, expression, RegexOptions.IgnoreCase);

                bool escape = false;

                while (result == null || result.Count > 0)
                {
                    Match match = result[0];
                    int offset = 0;

                    // First we need to add the starting char of the object
                    contents = contents.Insert(match.Groups[1].Index + match.Groups[1].Length + offset, "{");
                    offset++;

                    // Now we pick the middle of group which contains all the classes and their rules to be processed
                    var classRules = (match.Groups[2].Value + match.Groups[3].Value).Split(',');

                    foreach (string rule in classRules)
                    {
                        if (!rule.Contains(":"))
                        {
                            escape = true;
                        }
                    }

                    if (escape)
                    {
                        Console.WriteLine("============= Manual Input Required =============");
                        Console.WriteLine("This file should be validated by hand!! ");
                        Console.WriteLine("File: " + fileName);
                        Console.WriteLine("=================================================");
                        return false;
                    }

                    // Now we need to add the ending char of the object
                    contents = contents.Insert(match.Groups[match.Groups.Count - 1].Index + match.Groups[match.Groups.Count - 1].Length, "}");

                    // Now we remove everything in the middle because we'll later add them back already processed
                    contents = contents.Remove(match.Groups[3].Index, match.Groups[3].Length + 1);

                    // Now we process the classes
                    var processResult = ProcessClasses(classRules);

                    // Now we add back the processed parameters.
                    contents = contents.Insert(match.Groups[1].Index + match.Groups[1].Length + offset, processResult);

                    // Then we go back into the loop and redo it!
                    result = Regex.Matches(contents, expression, RegexOptions.IgnoreCase);
                }
                */

                return true;
            }

            return false;
        }

        public string ProcessClasses(string[] classRules)
        {
            string returnResult = "";

            foreach (string rule in classRules)
            {
                string expression = @"(\S.*)(:)(.*)";

                MatchCollection result = Regex.Matches(rule, expression, RegexOptions.IgnoreCase);

                if (result.Count == 1)
                {
                    returnResult += string.Format("'{0}': {1}, ", result[0].Groups[1].Value, result[0].Groups[3].Value);
                }
                else
                {
                    throw new Exception("This should never happen!!!! What happened?");
                }
            }

            if (returnResult.EndsWith(", "))
            {
                returnResult = returnResult.Substring(0, returnResult.Length - 2);
            }

            return returnResult;
        }

    }

}
