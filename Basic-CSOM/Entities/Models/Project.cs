using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Employee Leader{ get; set; }

        public List<MemberChoice> MemberList { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public ObservableCollection<string> StateList { get; set; }

    }

    public class MemberChoice
    {
        public Employee Member { get; set; }
        public bool IsChecked { get; set; }
    }
}
