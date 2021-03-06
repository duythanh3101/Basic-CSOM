using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using Microsoft.SharePoint.Client.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic_CSOM.Entities.Lists
{
    public class BaseList
    {
        public string Title { get; set; }
        public string ContentTypeName { get; set; }
        public int TemplateType { get; set; } = (int)ListTemplateType.GenericList;
        public string ViewTitle { get; set; } = "All Items";
        public List<string> ShowColumns { get; set; }
        public List<BaseField> ColumnFields { get; set; }

        public string Description { get; set; } = "New Description";

        protected List currentList;
        protected ClientContext Context;

        public BaseList(ClientContext context)
        {
            this.Context = context;
        }

        public List Generate()
        {
            // Get content type collection
            Web web = Context.Web;
            var contentTypes = Context.Web.ContentTypes;
            Context.Load(contentTypes);

            // Create new list information
            ListCreationInformation creationInfo = new ListCreationInformation
            {
                Title = Title,
                Description = Description,
                TemplateType = TemplateType
            };
            List newList = web.Lists.Add(creationInfo);
            Context.Load(newList, li => li.ContentTypes);
            Context.ExecuteQuery();

            // Get content type before importing to list
            var contentType = GetContentType(contentTypes, ContentTypeName);
            if (contentType == null)
            {
                return null;
            }
            newList.ContentTypesEnabled = true;
            newList.ContentTypes.AddExistingContentType(contentType);
            if (TemplateType == (int)ListTemplateType.GenericList)
            {
                var itemContentType = GetContentType(newList.ContentTypes, "Item");
                if (itemContentType != null)
                    itemContentType.DeleteObject();
            }
            newList.Update();
            //Context.ExecuteQuery();

            UpdateListitemLookup(newList, web.Lists, contentType);
            LoadView(newList);
            Context.ExecuteQuery();

            return newList;
        }

        public virtual void UpdateListitemLookup(List list, ListCollection webListCollection, ContentType contentType) { }

        private void LoadView(List list)
        {
            Context.Load(list.Fields);

            // Get required view by specifying view Title here
            var targetView = list.Views.GetByTitle(ViewTitle);
            Context.Load(targetView, x => x.ViewFields);
            Context.ExecuteQuery();

            // Get all columns need to show
            var fields = list.Fields.Where(x => ShowColumns.Contains(x.InternalName)).ToList();

            // Loop for each site column and add to view
            foreach (var item in fields)
            {
                targetView.ViewFields.Add(item.InternalName);
            }
            targetView.Update();
        }

        private ContentType GetContentType(ContentTypeCollection contentTypes ,string contentTypeName)
        {
            ContentType content = null;
            if (contentTypes == null || contentTypes.Count <= 0 || string.IsNullOrEmpty(contentTypeName))
            {
                return content;
            }

            return contentTypes.FirstOrDefault(x => x.Name.Equals(contentTypeName));
        }

        public void Delete()
        {
            // The SharePoint web at the URL.
            Web web = Context.Web;

            List list = web.Lists.GetByTitle(Title);
            list.DeleteObject();

            Context.ExecuteQuery();
        }

        public void AddFiled(List list, BaseField baseField)
        {
            if (list != null)
            {
                Field field = list.Fields.Add(baseField.CurrentField);

                Context.ExecuteQuery();
            }
        }

        public void AddFiledAsXml(List list, BaseField baseField)
        {
            if (list != null)
            {
                Field field = list.Fields.AddFieldAsXml(baseField.SchemaXml,
                                           true,
                                           AddFieldOptions.DefaultValue);

                Context.ExecuteQuery();
            }

        }

        public void UpdateItem(int id, string fieldName, string newValue)
        {
            if (currentList == null)
            {
                currentList = Context.Web.Lists.GetByTitle(ViewTitle);
            }

            ListItem item = currentList.GetItemById(id);
            item[fieldName] = newValue;
            item.Update();
            Context.ExecuteQuery();
        }

        public void DeleteItem(int rowPos)
        {
            if (currentList == null)
            {
                currentList = Context.Web.Lists.GetByTitle(ViewTitle);
            }

            // Option 1: Get Item by ID
            ListItem oItem = currentList.GetItemById(11);

            // Option 2: Get Item using CAML Query
            CamlQuery oQuery = new CamlQuery();
            oQuery.ViewXml = $@"<View><Query><Where>
                                <Eq>
                                <FieldRef Name='{Title}' />
                                <Value Type='Text'>New List Item</Value>
                                </Eq>
                                </Where></Query></View>";

            ListItemCollection oItems = currentList.GetItems(oQuery);
            Context.Load(oItems);
            Context.ExecuteQuery();

            oItem = oItems.FirstOrDefault();
            // Option 2: Ends Here(Above line)

            oItem.DeleteObject();
            Context.ExecuteQuery();
        }

        public void UpdateFieldToContentType(ContentType targetContentType, Field targetField, string fieldName)
        {
            //if (UtilApp.IsExist(Context, fieldName, Basic_CSOM.Enums.TypeSharepointEnum.SiteColumn))
            //{
            //    return;
            //}

            Context.Load(targetContentType, x => x.FieldLinks.Include(item => item.Name));
            Context.ExecuteQuery();

            // Update content type
            FieldLinkCreationInformation fldLink = new FieldLinkCreationInformation();
            fldLink.Field = targetField;

            // If uou set this to "true", the column getting added to the content type will be added as "required" field
            fldLink.Field.Required = false;

            // If you set this to "true", the column getting added to the content type will be added as "hidden" field
            fldLink.Field.Hidden = false;

            if (targetContentType.FieldLinks.FirstOrDefault(x => x.Name.Equals(fieldName)) == null)
            {
                targetContentType.FieldLinks.Add(fldLink);
            }
            targetContentType.Update(false);
            Context.Load(targetContentType);
            Context.ExecuteQuery();
        }

        protected void GetTaxonomyFieldInfo(string ternSetName, out Guid termStoreId, out Guid termSetId)
        {
            termStoreId = Guid.Empty;
            termSetId = Guid.Empty;

            TaxonomySession session = TaxonomySession.GetTaxonomySession(Context);
            TermStore termStore = session.GetDefaultSiteCollectionTermStore();
            TermSetCollection termSets = termStore.GetTermSetsByName(ternSetName, 1033);

            Context.Load(termSets, tsc => tsc.Include(ts => ts.Id));
            Context.Load(termStore, ts => ts.Id);
            Context.ExecuteQuery();

            termStoreId = termStore.Id;
            termSetId = termSets.FirstOrDefault().Id;
        }

    }
}
