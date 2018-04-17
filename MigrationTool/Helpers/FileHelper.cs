using System;
using System.Collections.Generic;
using MigrationTool.Models;

namespace MigrationTool.Helpers
{
    
    public static class FileHelper
    {

        public static IEnumerable<HtmlTag> GetAllTags(ref string contents, string toSearch, bool ignoreCase = true)
        {
            var indexes = FileHelper.AllIndexesOf(contents, toSearch, ignoreCase);

            return HtmlHelper.GetHTMLTags(ref contents, indexes);
        }
        
        public static void Apply(ref string contents, IEnumerable<HtmlTag> tags)
        {

            foreach (var htmlTag in tags)
            {
                contents = contents.Replace(htmlTag.PlaceHolder, htmlTag.ExportHtml());
            }
            
        }

        private static IEnumerable<int> AllIndexesOf(string str, string substr, bool ignoreCase = false)
        {
            if (string.IsNullOrWhiteSpace(str) ||
                string.IsNullOrWhiteSpace(substr))
            {
                throw new ArgumentException("String or substring is not specified.");
            }

            var indexes = new List<int>();
            var index = 0;

            while ((index = str.IndexOf(substr, index, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)) != -1)
            {
                indexes.Add(index++);
            }

            return indexes.ToArray();
        }
        
    }
    
}