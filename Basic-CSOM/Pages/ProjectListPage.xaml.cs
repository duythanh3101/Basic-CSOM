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
using Basic_CSOM.Constants;
using Language = Basic_CSOM.Entities.Models.Language;
using Basic_CSOM.Utils;

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for ProjectListPage.xaml
    /// </summary>
    public partial class ProjectListPage : Page
    {
        private ClientContext context;
        public ObservableCollection<Project> ProjectList { get; set; }
        private ListSP oList;
        private ListSP employeeList;
        private string employeeListName = "EmployeeList";

        public ProjectListPage()
        {
            InitializeComponent();
        }

        public ProjectListPage(ClientContext context, string listName = "ProjectList")
        {
            InitializeComponent();
            this.context = context;
            ProjectList = new ObservableCollection<Project>();

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
                ProjectList.Add(new Project()
                {
                    Id = oListItem.Id,
                    ProjectName = oListItem.FieldValuesForEdit.FieldValues["ProjectName"],
                    Description = oListItem.FieldValuesForEdit.FieldValues["ProjDescription"],
                    StartDate = DateTime.Parse(oListItem.FieldValuesForEdit.FieldValues["StartDate"].ToString()),
                    EndDate = DateTime.Parse(oListItem.FieldValuesForEdit.FieldValues["_EndDate"].ToString()),
                    StateList = new ObservableCollection<string>() { "Signed", "Design", "Development", "Maintenance", "Closed" },
                    State = oListItem.FieldValuesForEdit.FieldValues["State"],
                    //Leader = 
                    //Leader = oListItem.FieldValuesForEdit.FieldValues["FirstName"],
                    //Languages = new ObservableCollection<Language>()
                    //{
                    //    new Language() { LanguageName = ScreenConstants.CSharp, IsChecked = IsContain(lang, ScreenConstants.CSharp) },
                    //    new Language() { LanguageName = ScreenConstants.FSharp, IsChecked = IsContain(lang, ScreenConstants.FSharp)},
                    //    new Language() { LanguageName = ScreenConstants.VisualBasic, IsChecked = IsContain(lang, ScreenConstants.VisualBasic)},
                    //    new Language() { LanguageName = ScreenConstants.Java, IsChecked = IsContain(lang, ScreenConstants.Java)},
                    //    new Language() { LanguageName = ScreenConstants.Jquery, IsChecked = IsContain(lang, ScreenConstants.Jquery)},
                    //    new Language() { LanguageName = ScreenConstants.AngularJS, IsChecked = IsContain(lang, ScreenConstants.AngularJS)},
                    //    new Language() { LanguageName = ScreenConstants.Other, IsChecked = IsContain(lang, ScreenConstants.Other)}
                    //}

                });
            }
            employeeGrid.ItemsSource = ProjectList;
        }

        public bool IsContain(string target, string text)
        {
            return target.Contains(text);
        }


        private void Edit_Clicked(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is Project pro)
                {
                    // Display message box
                    MessageBoxResult result = MessageBox.Show("Do you want to save this data", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Edit(pro);
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

        private void Edit(Project pro)
        {
            if (oList != null)
            {
                // Edit
                if (pro.Id != 0)
                {
                    // Assume there is a list item with ID=1.
                    ListItem listItem = oList.GetItemById(pro.Id);

                    // Write a new value to the Body field of the Announcement item.
                    listItem["ProjectName"] = pro.ProjectName;
                    listItem["ProjDescription"] = pro.Description;
                    listItem["StartDate"] = pro.StartDate;
                    listItem["_EndDate"] = pro.EndDate;
                    listItem["State"] = pro.State;

                    // Leader
                    FieldLookupValue lookup = listItem["Leader"] as FieldLookupValue;
                    lookup.LookupId = 2;
                    listItem["Leader"] = lookup;

                    // Members
                    List<FieldLookupValue> lvList = new List<FieldLookupValue>();
                    lvList.Add(lookup);
                    lvList.Add(new FieldLookupValue() { LookupId = 1 });
                    listItem["Member"] = lvList;
                    listItem.Update();
                    context.ExecuteQuery();

                    context.ExecuteQuery();
                }
                // Add new item
                else
                {
                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                    ListItem newItem = oList.AddItem(itemCreateInfo);
                    newItem["Title"] = Guid.NewGuid();
                    newItem["ProjectName"] = pro.ProjectName;
                    newItem["ProjDescription"] = pro.Description;
                    newItem["StartDate"] = pro.StartDate;
                    newItem["_EndDate"] = pro.EndDate;
                    newItem["State"] = pro.State;
                    newItem.Update();

                    context.ExecuteQuery();
                }

            }
        }

        private void Delete_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is Project emp)
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

        private void Delete(Project emp)
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
            Project pro = null;
            foreach (var item in ProjectList)
            {
                if (item.Id == id)
                {
                    pro = item;
                    break;
                }
            }
            if (pro != null)
            {
                ProjectList.Remove(pro);
                employeeGrid.ItemsSource = ProjectList;
            }
        }

        private void Seeding(object sender, RoutedEventArgs e)
        {
            //Add Data. 
            ListItem newItem1 = oList.AddItem(new ListItemCreationInformation());
            newItem1["Title"] = $"Project {Guid.NewGuid()}";
            newItem1["ProjectName"] = "Project 1";
            newItem1["ProjDescription"] = "A12345";
            newItem1["StartDate"] = new DateTime(2021, 6, 4).ToString("o");
            newItem1["_EndDate"] = new DateTime(2021, 6, 4).ToString("o");
            newItem1["State"] = "Signed";

            // Leader
            FieldLookupValue lv = new FieldLookupValue();
            lv.LookupId = 1;
            newItem1["Leader"] = lv;

            // Members
            List<FieldLookupValue> lvList = new List<FieldLookupValue>();
            lvList.Add(lv);
            lvList.Add(new FieldLookupValue() { LookupId = 2 });
            newItem1["Member"] = lvList;
            newItem1.Update();
            context.ExecuteQuery();

            //Add Data. 
            ListItem newItem2 = oList.AddItem(new ListItemCreationInformation());
            newItem2["Title"] = $"Project {Guid.NewGuid()}";
            newItem2["ProjectName"] = "Project 3";
            newItem2["ProjDescription"] = "A78901";
            newItem2["StartDate"] = new DateTime(2021, 8, 7).ToString("o");
            newItem2["_EndDate"] = new DateTime(2021, 8, 7).ToString("o");
            newItem2["State"] = "Signed";

            lv = new FieldLookupValue();
            lv.LookupId = 1;
            newItem2["Leader"] = lv;
            newItem2.Update();

            context.ExecuteQuery();
        }

      
    }
}
