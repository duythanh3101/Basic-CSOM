using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Enums
{
    public enum LanguageEnum
    {
        [DefaultValue("C#")]
        CSharp = 0,
        [DefaultValue("F#")]
        FSharp = 1,
        [DefaultValue("Visual Basic")]
        VisualBasic = 2,
        [DefaultValue("Java")]
        Java = 3,
        [DefaultValue("JQuery")]
        JQuery = 4,
        [DefaultValue("AngularJs")]
        AngularJs = 5,
        [DefaultValue("Other")]
        Other = 6
    }
}
