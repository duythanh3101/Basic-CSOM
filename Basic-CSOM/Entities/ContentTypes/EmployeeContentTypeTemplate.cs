using Basic_CSOM.Constants;
using Basic_CSOM.Entities.Fields;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace Basic_CSOM.Entities.ContentTypes
{
    public class EmployeeContentTypeTemplate : BaseContentType
    {
        public EmployeeContentTypeTemplate(ClientContext context) : base(context)
        {
        }

        public override void CreateContentTypeTemplate(ClientContext context)
        {
            //Name = "EmployeeTestList";
            Fields = new List<BaseField>()
            {
                new ExistSiteColumn(context) { InternalName = "FirstName" },
                 new NewSiteColumn(context)
                {
                    InternalName = ScreenConstants.EmailAdd,
                    DisplayName = "Last Name",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='LastName' StaticName='LastName' DisplayName='Last Name' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = ScreenConstants.EmailAdd,
                    DisplayName = "Email Address",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='EmailAdd' StaticName='EmailAdd' DisplayName='Email Address' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = ScreenConstants.ShortDesc,
                    DisplayName = "Short Description",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Note' Name='ShortDesc' StaticName='ShortDesc' DisplayName='Short Description' NumLines='6' RichText='TRUE' RichTextMode='FullHtml' IsolateStyles='TRUE' Sortable='FALSE' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = ScreenConstants.ProgramLanguage,
                    DisplayName = "Programming Languages",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='MultiChoice' Name='ProgrammingLanguages' StaticName='ProgrammingLanguages' DisplayName='Programming Languages'>" +
                                    $"<Default>{ScreenConstants.CSharp}</Default>" +
                                    "<CHOICES>" +
                                        $"<CHOICE>{ScreenConstants.CSharp}</CHOICE>" +
                                        $"<CHOICE>{ScreenConstants.FSharp}</CHOICE>" +
                                        $"<CHOICE>{ScreenConstants.Java}</CHOICE>" +
                                        $"<CHOICE>{ScreenConstants.VisualBasic}</CHOICE>" +
                                        $"<CHOICE>{ScreenConstants.Jquery}</CHOICE>" +
                                        $"<CHOICE>{ScreenConstants.AngularJS}</CHOICE>" +
                                        $"<CHOICE>{ScreenConstants.Other}</CHOICE>" +
                                    "</CHOICES>" +
                                "</Field>"
                }
            };
            CreateFieldList();
        }
    }
}
