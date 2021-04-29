using Basic_CSOM.Entities.Fields;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.ContentTypes
{
    public class EmployeeContentTypeTemplate : BaseContentType
    {
        public EmployeeContentTypeTemplate(ClientContext context) : base(context)
        {
            Name = "EmployeeTestList";
            Fields = new List<BaseField>()
            {
                new ExistSiteColumn(context) { InternalName = "FirstName" },
                new NewSiteColumn(context)
                {
                    InternalName = "EmailAdd",
                    DisplayName = "Email Address",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='EmailAdd' StaticName='EmailAdd' DisplayName='Email Address' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = "ShortDesc",
                    DisplayName = "Short Description",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Note' Name='ShortDesc' StaticName='ShortDesc' DisplayName='Short Description' NumLines='6' RichText='TRUE' RichTextMode='FullHtml' IsolateStyles='TRUE' Sortable='FALSE' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = "ProgrammingLanguages",
                    DisplayName = "Programming Languages",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Choice' Name='ProgrammingLanguages' StaticName='ProgrammingLanguages' DisplayName='Programming Languages' Format='Dropdown' FillInChoice='FALSE'><CHOICES>" +
                    "<CHOICE>C#</CHOICE>" +
                    "<CHOICE>F#</CHOICE>" +
                    "<CHOICE>Java</CHOICE>" +
                    "<CHOICE>Jquery</CHOICE>" +
                    "<CHOICE>AngularJS</CHOICE>" +
                    "<CHOICE>Other</CHOICE>" +
                    "</CHOICES></Field>"
                }
            };
            CreateFieldList();
        }


    }
}
