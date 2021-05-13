using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Lists
{
    public class ProjectList : BaseList
    {
        private ClientContext context;
        private string DependListTitle = "EmployeeList";
        public ProjectList(ClientContext context) : base(context)
        {
            this.context = context;
            Title = "ProjectList";
            ContentTypeName = "Project";
            ViewTitle = "All Items";
            ShowColumns = new List<string>
            {
                "ProjectName",
                "Description",
                "State",
                "StartDate",
                "_EndDate",
                "DemoNewState"
            };

        }

        public override void UpdateListitemLookup(List list, ListCollection webListCollection, ContentType contentType)
        {
            var relatedList = webListCollection.GetByTitle(DependListTitle);
            if (relatedList == null || !UtilApp.IsExist(context, DependListTitle, Basic_CSOM.Enums.TypeSharepointEnum.List))
            {
                return;
            }

            context.Load(relatedList, li => li.Id);
            context.ExecuteQuery();

            string schema = $"<Field ID='{Guid.NewGuid()}' Type='Lookup' Name='Leader' StaticName='Leader' DisplayName='Leader' List='{relatedList.Id}' ShowField='Title' />";
            Field leaderField = list.Fields.AddFieldAsXml(schema, true, AddFieldOptions.AddFieldInternalNameHint);
            leaderField.SetShowInEditForm(true);
            leaderField.SetShowInNewForm(true);
            context.Load(leaderField);
            UpdateFieldToContentType(contentType, leaderField, "Leader");

            // Add member field
            string memberFieldSchema = $"<Field ID='{Guid.NewGuid()}' Type='LookupMulti' Name='Member' StaticName='Member' DisplayName='Member' List='{relatedList.Id}' ShowField='Title' Mult='TRUE' />";
            Field memberField = list.Fields.AddFieldAsXml(memberFieldSchema, true, AddFieldOptions.AddFieldInternalNameHint);
            memberField.SetShowInEditForm(true);
            memberField.SetShowInNewForm(true);
            context.Load(memberField);
            UpdateFieldToContentType(contentType, memberField, "Member");

            // Add metadata field
            // Create as a regular field setting the desired type in XML
            string metatSchema = $"<Field DisplayName='New State' Name='DemoNewState' ID='{Guid.NewGuid()}' ShowField='Title' Type='TaxonomyFieldTypeMulti' />";
            Field field = list.Fields.AddFieldAsXml(metatSchema, false, AddFieldOptions.AddFieldInternalNameHint);
            context.ExecuteQuery();

            Guid termStoreId = Guid.Empty;
            Guid termSetId = Guid.Empty;
            GetTaxonomyFieldInfo("DepartmentSet", out termStoreId, out termSetId);

            // Retrieve as Taxonomy Field
            TaxonomyField taxonomyField = context.CastTo<TaxonomyField>(field);
            taxonomyField.SspId = termStoreId;
            taxonomyField.TermSetId = termSetId;
            taxonomyField.TargetTemplate = String.Empty;
            taxonomyField.AnchorId = Guid.Empty;
            taxonomyField.Update();
            UpdateFieldToContentType(contentType, taxonomyField, "DepartmentTest");

            list.Update();

          
        }


    }
}
