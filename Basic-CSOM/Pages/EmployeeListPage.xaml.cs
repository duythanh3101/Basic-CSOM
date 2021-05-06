using Basic_CSOM.Entities.Models;
using Microsoft.SharePoint.Client;
using System;
using System.Windows.Controls;
using ListSP = Microsoft.SharePoint.Client.List;
using ListItem = Microsoft.SharePoint.Client.ListItem;
using ListItemCollection = Microsoft.SharePoint.Client.ListItemCollection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using Language = Basic_CSOM.Entities.Models.Language;
using System.Linq;
using Basic_CSOM.Constants;
using Basic_CSOM.Utils;

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for EmployeeListPage.xaml
    /// </summary>
    public partial class EmployeeListPage : Page
    {
        private ClientContext context;
        public ObservableCollection<Employee> Employees { get; set; }
        //public ObservableCollection<string> LanguageList { get; set; } 
        private ListSP oList;

        public EmployeeListPage()
        {
            InitializeComponent();
        }

        public EmployeeListPage(ClientContext context, string listName = "EmployeeList")
        {
            InitializeComponent();
            this.context = context;
            Employees = new ObservableCollection<Employee>();
            if (UtilApp.IsExist(context, listName, Enums.TypeSharepointEnum.List))
            {
                Load(listName);
            }
            else
            {
                MessageBox.Show($"List name {listName} is not existed");
            }
        }

        private void Load(string listName)
        {
            oList = context.Web.Lists.GetByTitle(listName);

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = @"<View><RowLimit>100</RowLimit></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);

            context.Load(collListItem, items => items.Include(item => item.Id, item => item.DisplayName, item => item.FieldValuesForEdit));
            context.ExecuteQuery();

            foreach (ListItem oListItem in collListItem)
            {
                string lang = oListItem.FieldValuesForEdit.FieldValues["ProgrammingLanguages"];
                Employees.Add(new Employee()
                {
                    Id = oListItem.Id,
                    Title = oListItem.FieldValuesForEdit.FieldValues["Title"],
                    Email = oListItem.FieldValuesForEdit.FieldValues["EmailAdd"],
                    ShortDescription = oListItem.FieldValuesForEdit.FieldValues["ShortDesc"],
                    FirstName = oListItem.FieldValuesForEdit.FieldValues["FirstName"],
                    Languages = new ObservableCollection<Language>()
                    {
                        new Language() { LanguageName = ScreenConstants.CSharp, IsChecked = IsContain(lang, ScreenConstants.CSharp) },
                        new Language() { LanguageName = ScreenConstants.FSharp, IsChecked = IsContain(lang, ScreenConstants.FSharp)},
                        new Language() { LanguageName = ScreenConstants.VisualBasic, IsChecked = IsContain(lang, ScreenConstants.VisualBasic)},
                        new Language() { LanguageName = ScreenConstants.Java, IsChecked = IsContain(lang, ScreenConstants.Java)},
                        new Language() { LanguageName = ScreenConstants.Jquery, IsChecked = IsContain(lang, ScreenConstants.Jquery)},
                        new Language() { LanguageName = ScreenConstants.AngularJS, IsChecked = IsContain(lang, ScreenConstants.AngularJS)},
                        new Language() { LanguageName = ScreenConstants.Other, IsChecked = IsContain(lang, ScreenConstants.Other)}
                    }

                });
            }
            employeeGrid.ItemsSource = Employees;
        }

        public bool IsContain(string target, string text)
        {
            return target.Contains(text);
        }


        private void Edit_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is Employee emp)
                {
                    // Display message box
                    MessageBoxResult result = MessageBox.Show("Do you want to save this data", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Edit(emp);
                            MessageBox.Show("Save successfully");
                            break;
                        case MessageBoxResult.Cancel:
                        case MessageBoxResult.No:
                        default:
                            break;
                    }

                }
            }

        }

        private void Edit(Employee emp)
        {
            if (oList != null)
            {
                string lang = GetLanguage(emp.Languages);
                // Edit
                if (emp.Id != 0)
                {
                    // Assume there is a list item with ID=1.
                    ListItem listItem = oList.GetItemById(emp.Id);

                    // Write a new value to the Body field of the Announcement item.
                    listItem["EmailAdd"] = emp.Email;
                    listItem["ShortDesc"] = emp.ShortDescription;
                    listItem["FirstName"] = emp.FirstName;
                    listItem["Title"] = emp.Title;
                    listItem["ProgrammingLanguages"] = lang;
                    listItem.Update();

                    context.ExecuteQuery();
                }
                // Add new item
                else
                {
                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                    ListItem newItem = oList.AddItem(itemCreateInfo);
                    newItem["EmailAdd"] = emp.Email;
                    newItem["ShortDesc"] = emp.ShortDescription;
                    newItem["FirstName"] = emp.FirstName;
                    newItem["Title"] = emp.Title;
                    newItem["ProgrammingLanguages"] = lang;
                    newItem.Update();

                    context.ExecuteQuery();
                }

            }
        }

        private string GetLanguage(ObservableCollection<Language> languages)
        {
            string result = string.Empty;

            if (languages != null && languages.Count > 0)
            {
                result = languages[0].IsChecked ? languages[0].LanguageName : string.Empty;
                for (int i = 1; i < languages.Count; i++)
                {
                    if (languages[i].IsChecked)
                    {
                        result = result + ";#" + languages[i].LanguageName;
                    }
                }
            }

            return result;
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is Employee emp)
                {
                    // Display message box
                    MessageBoxResult result = MessageBox.Show("Do you want to delete this row", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Delete(emp);
                            MessageBox.Show("Delete successfully");
                            break;
                        case MessageBoxResult.Cancel:
                        case MessageBoxResult.No:
                        default:
                            break;
                    }

                }
            }

        }

        private void Delete(Employee emp)
        {
            if (oList != null)
            {
                // Delete
                if (emp.Id != 0)
                {
                    // Assume that there is a list item with ID=2.
                    ListItem listItem = oList.GetItemById(emp.Id);
                    if (listItem != null)
                    {
                        listItem.DeleteObject();
                        context.ExecuteQuery();
                        RemoveItemOnView(emp.Id);
                    }
                }

            }
        }

        private void RemoveItemOnView(int id)
        {
            Employee em = null;
            foreach (var item in Employees)
            {
                if (item.Id == id)
                {
                    em = item;
                    break;
                }
            }
            if (em != null)
            {
                Employees.Remove(em);
                employeeGrid.ItemsSource = Employees;
            }
        }
    }
}
