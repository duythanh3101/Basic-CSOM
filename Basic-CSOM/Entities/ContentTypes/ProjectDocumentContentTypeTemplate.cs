using Basic_CSOM.Entities.Fields;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.ContentTypes
{
    public class ProjectDocumentContentTypeTemplate : BaseContentType
    {
        public ProjectDocumentContentTypeTemplate(ClientContext context) : base(context)
        {
        }

        public override void CreateContentTypeTemplate(ClientContext context)
        {
            Name = "Project Document";
            ParentType = "Document";
            Fields = new List<BaseField>()
            {
                new NewSiteColumn(context)
                {
                    InternalName = "DocDescription",
                    DisplayName = "Description",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Note' Name='DocDescription' StaticName='DocDescription' DisplayName='Description' NumLines='6' RichText='FALSE' Sortable='FALSE' />"
                },
                new NewSiteColumn(context)
                {
                    InternalName = "DocType",
                    DisplayName = "Document Type",
                    SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Choice' Name='DocType' StaticName='DocType' DisplayName='Document Type' Format='Dropdown'><CHOICES>" +
                                "<CHOICE>Business requirement</CHOICE>" +
                                "<CHOICE>Technical document</CHOICE>" +
                                "<CHOICE>User guide</CHOICE>" +
                                "</CHOICES></Field>"
                }
            };
            CreateFieldList();

        }
    }
}
