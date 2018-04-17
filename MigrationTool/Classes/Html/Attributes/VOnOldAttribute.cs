using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigrationTool.Models
{
    
    public class VOnOldAttribute: HtmlAttribute
    {
        public List<Tuple<string, string>> events { get; set; }
        private bool isToConvert = false;

        public VOnOldAttribute(string raw, string attr, string value) : base(raw, attr, value)
        {
            this.events = new List<Tuple<string, string>>();

            if (value.Contains(",") && value.Contains("("))
            {
                var inFunc = false;
                var deepness = 0;

                var inVal = false;

                var eName = "";
                var eVal = "";

                foreach (char c in value)
                {
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
                        this.events.Add(new Tuple<string, string>(eName.Trim(), eVal.Trim()));

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
                }
            }
            else
            {
                var events = value.Split(',');

                foreach (string e in events)
                {
                    var args = e.Split(':');

                    this.events.Add(new Tuple<string, string>(args[0].Trim(), args[1].Trim()));
                }
            }

        }

        public void ConvertToNewVOn()
        {
            this.isToConvert = true;
        }

        public override string ExportHtml()
        {
            if (!this.isToConvert)
            {
                return rawAttribute;
            }

            string output = "";

            foreach (Tuple<string, string> tuple in this.events)
            {
                output += string.Format("v-on:{0}=\"{1}\"", tuple.Item1, tuple.Item2);
            }

            if (output.EndsWith(" "))
            {
                output = output.Substring(0, output.Length - 1);
            }

            return output;
        }
        
    }
    
}