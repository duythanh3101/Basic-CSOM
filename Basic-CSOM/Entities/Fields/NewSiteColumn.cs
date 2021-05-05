using Basic_CSOM.Enums;
using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;

namespace Basic_CSOM.Entities.Fields
{
    public class NewSiteColumn : BaseField
    {
        public NewSiteColumn(ClientContext context) : base(context)
        {
        }

        public override Field Create()
        {
            var filed = GetField();
            if (filed != null)
            {
                // Existed this site column
                return filed;
            }

            if (UtilApp.IsExist(Context, DisplayName, TypeSharepointEnum.SiteColumn))
            {
                return null;
            }
            var web = Context.Web;
            Field newField = web.Fields.AddFieldAsXml(SchemaXml, false, AddFieldOptions.AddFieldInternalNameHint);

            //Context.Load(filed);
            Context.Load(web);
            Context.Load(newField);
            Context.ExecuteQuery();

            return newField;
        }
    }
}
