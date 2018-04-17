using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigrationTool.Models;

namespace MigrationTool.Helpers
{

    public static class HtmlHelper
    {
        
        public static IEnumerable<HtmlTag> GetHTMLTags(ref string contents, IEnumerable<int> positions)
        {
            var toReturn = new List<HtmlTag>();

            foreach (var position in positions)
            {
                var pair = HtmlHelper.GetStartAndEndIndexesOfHtmlTag(contents, position);

                var FromIndex = pair[0];
                var ToIndex = pair[1];

                var RawTag = contents.Substring(FromIndex, ToIndex - FromIndex);

                if (RawTag.StartsWith("<!--"))
                {
                    continue;
                }

                toReturn.Add(new HtmlTag(ref contents, position));
            }
            
            return toReturn.ToArray();
        }

        public static int[] GetStartAndEndIndexesOfHtmlTag(string contents, int position)
        {
            var FromIndex = contents.LastIndexOf('<', position);
            var ToIndex = 0;

            while (contents[FromIndex + 1] == ' ')
            {
                FromIndex = contents.LastIndexOf('<', FromIndex - 1);
            }

            var inQuotes = false;

            for (int i = FromIndex; i < contents.Length; i++)
            {
                if (contents[i] == '"')
                {
                    inQuotes = !inQuotes;
                }

                if (!inQuotes && contents[i] == '>')
                {
                    ToIndex = i + 1;
                    break;
                }
            }

            return new int[] { FromIndex, ToIndex };
        }

    }

}
