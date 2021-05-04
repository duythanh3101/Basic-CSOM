using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Lists
{
    public class ProjectList : BaseList
    {
        private ClientContext context;
        public ProjectList(ClientContext context) : base(context)
        {
            this.context = context;
            Title = "ProjectList";
            ContentTypeName = "Project";
            ViewTitle = "All Items";
            ShowColumns = new List<string>
            {
                "ProjectName",
                "Description",
                "State",
                "StartDate",
                "_EndDate"
            };

        }

        

      
    }
}
