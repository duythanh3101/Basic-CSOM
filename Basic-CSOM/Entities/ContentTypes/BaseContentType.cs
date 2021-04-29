using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basic_CSOM.Entities.ContentTypes
{
    public abstract class BaseContentType : IDisposable
    {
        public List<BaseField> Fields { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
        public string ParentType { get; set; } = "Item";

        private ClientContext context;

        public BaseContentType(ClientContext context)
        {
            this.context = context;
            CreateContentTypeTemplate(context);
        }

        public ContentType Create()
        {
            ContentTypeCollection cntTypeList = context.Web.ContentTypes;
            context.Load(cntTypeList);
            context.ExecuteQuery();

            var targetContentType = GetContentType(cntTypeList);

            // not exist content type yet
            if (targetContentType == null)
            {
                var parentContentType = GetContentType(cntTypeList, ParentType);
                // Create content Type
                ContentTypeCreationInformation newCntType = new ContentTypeCreationInformation
                {
                    Name = Name,
                    Description = Description,
                    Group = Group,
                    ParentContentType = parentContentType
                };

                targetContentType = cntTypeList.Add(newCntType);

                LoadSiteColumn(targetContentType);
                context.Load(targetContentType);
                context.ExecuteQuery();
            }
            return targetContentType;
        }

        private void LoadSiteColumn(ContentType targetContentType)
        {
            if (Fields != null && Fields.Count > 0)
            {
                foreach (var targetField in Fields)
                {
                    if (targetField.CurrentField != null)
                    {
                        FieldLinkCreationInformation fldLink = new FieldLinkCreationInformation();
                        fldLink.Field = targetField.CurrentField;

                        // If uou set this to "true", the column getting added to the content type will be added as "required" field
                        fldLink.Field.Required = false;

                        // If you set this to "true", the column getting added to the content type will be added as "hidden" field
                        fldLink.Field.Hidden = false;

                        targetContentType.FieldLinks.Add(fldLink);
                        targetContentType.Update(false);
                        context.ExecuteQuery();
                    }
                }
            }
        }

        private ContentType GetContentType(ContentTypeCollection collection, string contentTypeName = "")
        {
            contentTypeName = string.IsNullOrEmpty(contentTypeName) ? contentTypeName : Name;
            return collection.FirstOrDefault(x => x.Name.Equals(contentTypeName));
        }

        protected void CreateFieldList()
        {
            if (Fields != null && Fields.Count > 0)
            {
                Fields.ForEach(x =>
                {
                    if (x.CurrentField == null)
                    {
                        x.CurrentField = x.Create();
                    }
                });
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public abstract void CreateContentTypeTemplate(ClientContext context);
    }
}
