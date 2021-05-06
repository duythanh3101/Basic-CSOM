using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;

namespace Basic_CSOM.Entities.Lists
{
    public class DocProjectList : BaseList
    {
        private ClientContext context;
        private string DependListTitle = "ProjectList";

        public DocProjectList(ClientContext context) : base(context)
        {
            this.context = context;
            Title = "ProjectDocumentList";
            ContentTypeName = "Project Document";
            TemplateType = (int)ListTemplateType.DocumentLibrary;
            ViewTitle = "All Documents";
            ShowColumns = new List<string>
            {
                "DocDescription",
                "DocType"
            };
        }

        public override void UpdateListitemLookup(List targetList, ListCollection webListCollection, ContentType contentType)
        {
            var relatedList = webListCollection.GetByTitle(DependListTitle);
            if (relatedList == null)
            {
                return;
            }
            var fields = targetList.Fields;

            context.Load(relatedList);
            context.ExecuteQuery();

            string schema = $"<Field ID='{Guid.NewGuid()}' Type='Lookup' Name='{DependListTitle}' StaticName='Project' DisplayName='Project' List='{relatedList.Id}' ShowField='ProjectName' />";
            Field newField = fields.AddFieldAsXml(schema, true, AddFieldOptions.AddFieldInternalNameHint);
            newField.SetShowInEditForm(true);
            newField.SetShowInNewForm(true);
            context.Load(newField);

            context.Load(fields);
            context.ExecuteQuery();
        }
    }
}
