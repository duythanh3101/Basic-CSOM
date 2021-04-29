using Microsoft.SharePoint.Client;

namespace Basic_CSOM.Entities.Lists
{
    public class BaseList
    {
        public string Name { get; set; }

        protected ClientContext Context;

        public BaseList(ClientContext context)
        {
            this.Context = context;
        }
    }
}
