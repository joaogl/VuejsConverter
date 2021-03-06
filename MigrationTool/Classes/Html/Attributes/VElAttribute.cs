﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MigrationTool.Models
{
    
    public class VElAttribute : HtmlAttribute
    {

        public VElAttribute(string raw, string attr, string value) : base(raw, attr, value)
        {
        }

        public override string ExportHtml()
        {
            return attribute + ":" + rawValue;
        }

    }
    
}