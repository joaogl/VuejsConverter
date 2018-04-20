using MigrationTool.Helpers;
using MigrationTool.VersionMigrationTool;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MigrationTool.Models
{

    public class HtmlTag
    {

        public HtmlTagType TagType { get; set; }
        public string Tag { get; set; }
        
        public int FromIndex { get; set; }
        public int ToIndex { get; set; }
        
        public string RawTag { get; set; }
        public string PlaceHolder { get; set; }
        
        public List<HtmlAttribute> Attributes { get; set; }

        public HtmlTag(ref string contents, int currentPosition)
        {
            Attributes = new List<HtmlAttribute>();

            var pair = HtmlHelper.GetStartAndEndIndexesOfHtmlTag(contents, currentPosition);

            var FromIndex = pair[0];
            var ToIndex = pair[1];

            RawTag = contents.Substring(FromIndex, ToIndex - FromIndex);
            
            const string expression = @"(<)(\S*)";
            
            var result = Regex.Matches(RawTag, expression, RegexOptions.IgnoreCase);

            if (result.Count > 0)
            {
                Tag = result[0].Groups[2].Value.ToLower();
                this.FindBestType();
            }

            PlaceHolder = $"{{{Guid.NewGuid()}}}";

            while (PlaceHolder.Length < ToIndex - FromIndex)
            {
                PlaceHolder += " ";
            }

            contents = contents.Remove(FromIndex, ToIndex - FromIndex);
            contents = contents.Insert(FromIndex, PlaceHolder);

            if (TagType == HtmlTagType.Unknown)
            {
                if (!MigrationTool0_12_16To1_0_7.Instance.MissingTags.Contains(Tag))
                {
                    MigrationTool0_12_16To1_0_7.Instance.MissingTags.Add(Tag);
                }
            }
            
            ProcessAttributes();
        }

        private void FindBestType()
        {
            switch (Tag)
            {
                case "a":
                    TagType = HtmlTagType.HtmlA;
                    break;
                case "ul":
                    TagType = HtmlTagType.HtmlUl;
                    break;
                case "div":
                    TagType = HtmlTagType.HtmlDiv;
                    break;
                case "li":
                    TagType = HtmlTagType.HtmlLi;
                    break;
                case "i":
                    TagType = HtmlTagType.HtmlI;
                    break;
                case "button":
                    TagType = HtmlTagType.HtmlButton;
                    break;
                case "span":
                    TagType = HtmlTagType.HtmlSpan;
                    break;
                case "th":
                    TagType = HtmlTagType.HtmlTh;
                    break;
                case "td":
                    TagType = HtmlTagType.HtmlTd;
                    break;
                case "tr":
                    TagType = HtmlTagType.HtmlTr;
                    break;
                case "input":
                    TagType = HtmlTagType.HtmlInput;
                    break;
                case "form":
                    TagType = HtmlTagType.HtmlForm;
                    break;
                case "label":
                    TagType = HtmlTagType.HtmlLabel;
                    break;
                case "pre":
                    TagType = HtmlTagType.HtmlPre;
                    break;
                case "textarea":
                    TagType = HtmlTagType.HtmlTextarea;
                    break;
                case "table":
                    TagType = HtmlTagType.HtmlTable;
                    break;
                case "img":
                    TagType = HtmlTagType.HtmlImg;
                    break;
                case "h4":
                    TagType = HtmlTagType.HtmlH4;
                    break;
                case "component":
                    TagType = HtmlTagType.VueComponent;
                    break;
                default:
                    TagType = HtmlTagType.Unknown;
                    break;
            }            
        }

        public string ExportHtml()
        {
            return $"<{Tag} {string.Join(" ", Attributes.Select(x => x.ExportHtml()))}>";
        }

        private void ProcessAttributes()
        {
            var inVal = false;
            var inQuotes = false;

            var attrFullRaw = "";
            var attrName = "";
            var attrValue = "";

            var starting = true;
            var skipFirst = true;

            foreach (char c in RawTag)
            {
                if (starting && c != ' ')
                {
                    continue;
                }
                else
                {
                    starting = false;
                }

                if (!skipFirst)
                {
                    attrFullRaw += c;
                }
                else
                {
                    skipFirst = false;
                }

                if (c == '=' && !inVal)
                {
                    inVal = true;
                }

                if (c == '"')
                {
                    inQuotes = !inQuotes;

                    if (!inQuotes)
                    {
                        Regex rgx = new Regex("[^a-zA-Z0-9-]");
                        attrName = rgx.Replace(attrName, "");
                        attrValue = attrValue.Substring(2).Trim();

                        HtmlAttribute attr = null;

                        switch (attrName)
                        {
                            case "v-on":
                                attr = new VOnOldAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "v-class":
                                attr = new VClassOldAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "class":
                                attr = new ClassAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "v-el":
                                attr = new VElAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "v-style":
                                attr = new StyleAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "v-attr":
                                attr = new AttrAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "href":
                                attr = new HrefAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            case "placeholder":
                                attr = new PlaceholderAttribute(attrFullRaw, attrName, attrValue);
                                break;
                            default:
                                attr = new HtmlAttribute(attrFullRaw, attrName, attrValue);
                                break;
                        }

                        Attributes.Add(attr);

                        attrFullRaw = "";
                        attrName = "";
                        attrValue = "";
                        inVal = false;
                    }
                }

                if (inVal)
                {
                    attrValue += c;
                }
                else
                {
                    attrName += c;
                }

            }


            /*
            var expression = "(\\S+)=[\"]?((?:.(?![\"]?\\s+(?:\\s+)=|[>\"]))+.)[\"]?";
            
            var result = Regex.Matches(RawTag, expression, RegexOptions.IgnoreCase);

            foreach (Match match in result)
            {
                HtmlAttribute attr = null;

                switch (match.Groups[1].Value)
                {
                    case "v-on":
                        attr = new VOnOldAttribute(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value);
                        break;
                    case "v-class":
                        attr = new VClassOldAttribute(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value);
                        break;
                    case "class":
                        attr = new ClassAttribute(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value);
                        break;
                    default:
                        attr = new HtmlAttribute(match.Groups[0].Value, match.Groups[1].Value, match.Groups[2].Value);
                        break;
                }
                
                Attributes.Add(attr);
            }
            */
        }

    }

}
