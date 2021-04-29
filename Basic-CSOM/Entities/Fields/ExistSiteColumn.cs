using Microsoft.SharePoint.Client;

namespace Basic_CSOM.Entities.Fields
{
    public class ExistSiteColumn : BaseField
    {
        public ExistSiteColumn(ClientContext context) : base(context)
        {
        }

        public override Field Create()
        {
            var filed = GetField();
            if (filed == null)
            {
                // Not existed this site column
                return null;
            }

            return filed;

            //FieldLinkCreationInformation fldLink = new FieldLinkCreationInformation
            //{
            //    Field = filed
            //};

            //fldLink.Field.Required = false;
            //fldLink.Field.Hidden = false;

            //TargetContentType.FieldLinks.Add(fldLink);
            //TargetContentType.Update(false);
            //var web = _context.Web;
            //Field newField = web.Fields.AddFieldAsXml(SchemaXml, false, AddFieldOptions.AddFieldInternalNameHint);

            //_context.Load(web);
            //_context.Load(newField);
            //_context.ExecuteQuery();
        }
    }
}
