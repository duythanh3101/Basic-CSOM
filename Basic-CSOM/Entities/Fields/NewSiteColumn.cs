using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (UtilApp.IsExist(Context, DisplayName, Enums.TypeSharepointEnum.SiteColumn))
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
