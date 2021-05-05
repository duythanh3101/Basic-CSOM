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
            Load(listName);
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
                        new Language() { LanguageName = "C#", IsChecked = IsContain(lang, "C#") },
                        new Language() { LanguageName = "F#", IsChecked = IsContain(lang, "F#")},
                        new Language() { LanguageName = "Visual Basic", IsChecked = IsContain(lang, "Visual Basic")},
                        new Language() { LanguageName = "JQuery", IsChecked = IsContain(lang, "JQuery")},
                        new Language() { LanguageName = "Angular Js", IsChecked = IsContain(lang, "Angular Js")},
                        new Language() { LanguageName = "Other", IsChecked = IsContain(lang, "Other")}
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
                    newItem.Update();

                    context.ExecuteQuery();
                }

            }
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
