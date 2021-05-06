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
        }
    }
}
