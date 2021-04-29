using Microsoft.SharePoint.Client;
using System;

namespace Basic_CSOM.Entities
{
    public class BaseField : IDisposable
    {
        //public ContentType TargetContentType { get; set; }
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string SchemaXml { get; set; }

        protected ClientContext _context;

        public BaseField(ClientContext context)
        {
            _context = context;
        }

        public virtual bool Create()
        {
            Field targetField = GetField();

            if (targetField == null)
            {
                return false;
            }
            var rootWeb = _context.Site.RootWeb;

            Field newField = rootWeb.Fields.AddFieldAsXml(SchemaXml, false, AddFieldOptions.AddFieldInternalNameHint);
            newField.Group = "New Custom Field";

            //FieldLinkCreationInformation fldLink = new FieldLinkCreationInformation
            //{
            //    Field = targetField
            //};

            //fldLink.Field.Required = false;
            //fldLink.Field.Hidden = false;

            //TargetContentType.FieldLinks.Add(fldLink);
            //TargetContentType.Update(false);
            //_context.Load(newField);
            
            _context.ExecuteQuery();
            return true;
        }

        protected Field GetField()
        {
            return _context.Web.AvailableFields.GetByInternalNameOrTitle(InternalName);
        }

        public void Dispose()
        {
            _context.Dispose();
        }


    }
}
