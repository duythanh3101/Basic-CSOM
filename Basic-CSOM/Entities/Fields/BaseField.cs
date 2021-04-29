using Microsoft.SharePoint.Client;
using System;
using System.Linq;

namespace Basic_CSOM.Entities
{
    public class BaseField : IDisposable
    {
        public string InternalName { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string SchemaXml { get; set; }

        protected ClientContext Context;

        public Field CurrentField { get; set; }

        public BaseField(ClientContext context)
        {
            Context = context;
        }

        public virtual Field Create()
        {
            return default(Field);
        }

        protected Field GetField()
        {
            if (!string.IsNullOrEmpty(InternalName))
            {
                var fieldCollection = Context.Web.Fields;
                Context.Load(Context.Web);
                Context.Load(fieldCollection);
                try
                {
                    Context.ExecuteQuery();
                    bool isExist = fieldCollection.Where(x => x.InternalName == InternalName).Count() > 0;
                    if (isExist)
                    {
                        return Context.Web.AvailableFields.GetByInternalNameOrTitle(InternalName);
                    }
                }
                catch (Exception)
                {
                    return null;
                }
            }

            return null;
        }

        public void Dispose()
        {
            Context.Dispose();
        }


    }
}
