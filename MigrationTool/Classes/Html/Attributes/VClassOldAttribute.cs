using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigrationTool.Models
{
    
    public class VClassOldAttribute : HtmlAttribute
    {
        public List<Tuple<string, string>> classes { get; set; }
        public List<string> rulessClasses { get; set; }
        private bool isToConvert = false, isToIgnore = false;

        public VClassOldAttribute(string raw, string attr, string value) : base(raw, attr, value)
        {
            this.classes = new List<Tuple<string, string>>();
            this.rulessClasses = new List<string>();

            if (value.Contains(",") && value.Contains("("))
            {
                var inFunc = false;
                var deepness = 0;

                var inVal = false;

                var eName = "";
                var eVal = "";

                var index = 0;

                foreach (char c in value)
                {
                    index++;
                    if (c == '(')
                    {
                        inFunc = true;
                        deepness++;
                    }
                    else if (c == ')')
                    {
                        deepness--;

                        if (deepness == 0)
                        {
                            inFunc = false;
                        }
                    }

                    if (c == ':')
                    {
                        inVal = true;
                    }
                    else if (c == ',' && !inFunc)
                    {
                        this.classes.Add(new Tuple<string, string>(eName.Trim(), eVal.Trim()));

                        eName = "";
                        eVal = "";
                        inVal = false;
                    }
                    else
                    {
                        if (inVal)
                        {
                            eVal += c;
                        }
                        else
                        {
                            eName += c;
                        }
                    }

                    if (index == value.Length)
                    {
                        this.classes.Add(new Tuple<string, string>(eName.Trim(), eVal.Trim()));
                    }
                }
            }
            else
            {
                var events = value.Split(',');

                foreach (string e in events)
                {
                    var args = e.Split(':');

                    if (args.Length == 1)
                    {
                        this.rulessClasses.Add(args[0].Trim());
                    }
                    else if (args.Length > 1)
                    {
                        this.classes.Add(new Tuple<string, string>(args[0].Trim(), args[1].Trim()));
                    }
                }
            }

        }
        

        public void ConvertToNewVOn()
        {
            this.isToConvert = true;
        }

        public void IgnoreAttribute()
        {
            this.isToIgnore = true;
        }

        public override string ExportHtml()
        {
            if (this.isToIgnore)
            {
                return "";
            }

            if (!this.isToConvert)
            {
                return rawAttribute;
            }

            string output = "";

            foreach (Tuple<string, string> tuple in this.classes)
            {
                var className = tuple.Item1;

                if (!className.Contains("'"))
                {
                    className = "'" + className + "'";
                }
                output += string.Format("{0}: {1}, ", className, tuple.Item2);
            }

            if (output.EndsWith(", "))
            {
                output = output.Substring(0, output.Length - 2);
            }

            if (output.Length > 0)
            {
                output = ":class=\"{ " + output + " }\"";
            }

            return output;
        }
        
    }
    
}