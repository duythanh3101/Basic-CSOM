using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Models
{
    public class Project
    {
        public string NameOfProject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Employee Leader{ get; set; }

        public List<Employee> Members { get; set; }
        public string Description { get; set; }
        

    }
}
