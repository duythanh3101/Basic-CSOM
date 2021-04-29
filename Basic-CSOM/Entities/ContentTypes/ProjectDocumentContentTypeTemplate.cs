using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.ContentTypes
{
    public class ProjectDocumentContentTypeTemplate : BaseContentType
    {
        public ProjectDocumentContentTypeTemplate(ClientContext context) : base(context)
        {
        }

        public override void CreateContentTypeTemplate(ClientContext context)
        {
            throw new NotImplementedException();
        }
    }
}
