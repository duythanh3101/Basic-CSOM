using Basic_CSOM.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Entities.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public LanguageEnum Language { get; set; }
        public ObservableCollection<Language> Languages { get; set; } = new ObservableCollection<Language>();
    }

    //"C#", "F#", "Visual Basic", "JQuery", "Angular Js", "Other"

    public class Language
    {
        public string LanguageName { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
