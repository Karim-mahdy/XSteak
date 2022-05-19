﻿using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xSteak.Extentions
{
    public static class ReflectionExtention
    {
        public static string GetPropertyValue<T>(this T item , string propertyName)
        {
            return item.GetType().GetProperty(propertyName).GetValue(item, null).ToString();
        }
          
    }
}
