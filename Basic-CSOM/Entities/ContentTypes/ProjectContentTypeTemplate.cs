using Basic_CSOM.Entities.Fields;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace Basic_CSOM.Entities.ContentTypes
{
    public class ProjectContentTypeTemplate : BaseContentType
    {
        public ProjectContentTypeTemplate(ClientContext context) : base(context)
        {
        }

        public override void CreateContentTypeTemplate(ClientContext context)
        {
            Name = "Project";
            Fields = new List<BaseField>
            {
                new NewSiteColumn(context)
                {
                    InternalName = "ProjectName",
                    DisplayName = "Project Name",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='Project Name' StaticName='ProjectName' DisplayName='Project Name' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = "ProjDescription",
                    DisplayName = "Project Description",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Note' Name='ProjDescription' StaticName='ProjDescription' DisplayName='Description' NumLines='6' RichText='FALSE' Sortable='FALSE' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = "State",
                    DisplayName = "State",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Choice' Name='State' StaticName='State' DisplayName='State' Format='Dropdown'><CHOICES>" +
                    "<CHOICE>Signed</CHOICE>" +
                    "<CHOICE>Design</CHOICE>" +
                    "<CHOICE>Development</CHOICE>" +
                    "<CHOICE>Maintenance</CHOICE>" +
                    "<CHOICE>Closed</CHOICE>" +
                    "</CHOICES></Field>"
                },
                new ExistSiteColumn(context)
                {
                    InternalName = "StartDate"
                },
                new ExistSiteColumn(context) 
                { 
                    InternalName = "_EndDate" 
                }
            };
            CreateFieldList();
        }
    }
}
