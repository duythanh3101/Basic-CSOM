using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Lists
{
    public class EmployeeList : BaseList
    {
        public EmployeeList(ClientContext context) : base(context)
        {
            //Title = "EmployeeList";
            ShowColumns = new List<string>()
            {
                "FirstName",
                "ProgrammingLanguages",
                "ShortDesc",
            };
            ContentTypeName = "EmployeeTestList";


        }
    }
}
