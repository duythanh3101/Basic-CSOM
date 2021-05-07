using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ListItemCollection = Microsoft.SharePoint.Client.ListItemCollection;
using ListSP = Microsoft.SharePoint.Client.List;

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for ProjectDocListPage.xaml
    /// </summary>
    public partial class ProjectDocListPage : Page
    {
        public ProjectDocListPage()
        {
            InitializeComponent();
        }

        private ClientContext context;
        private ListSP oList;
        string sourcePath = @"C:\Users\thp2\OneDrive - Precio Fishbone AB\Skrivbordet\btSharepoint2.txt";

        public ProjectDocListPage(ClientContext context, string listName = "Project Document List")
        {
            InitializeComponent();

            this.context = context;
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

            //foreach (ListItem oListItem in collListItem)
            //{

            //}
            //Seeding();
            //Update();
        }

        private void Seeding()
        {
            FileCreationInformation _file = new FileCreationInformation();
            _file.Content = System.IO.File.ReadAllBytes(sourcePath);
            _file.Overwrite = true;
            _file.Url = System.IO.Path.GetFileName(sourcePath);

            File uploadfile = oList.RootFolder.Files.Add(_file);

            //Add Data. 
            var newItem1 = uploadfile.ListItemAllFields;
            newItem1["Title"] = $"Project Doc {Guid.NewGuid()}";
            newItem1["DocDescription"] = "A12345";
            newItem1["DocType"] = "Business requirement";

            // Leader
            FieldLookupValue lv = new FieldLookupValue();
            lv.LookupId = 1;
            newItem1["ProjectList"] = lv;

            newItem1.Update();
            //context.Load(uploadfile);
            context.ExecuteQuery();
        }

        private void Update()
        {
            File uploadfile = oList.RootFolder.Files.GetByUrl(sourcePath);

            //Add Data. 
            var newItem1 = uploadfile.ListItemAllFields;
            newItem1["Title"] = $"Project Doc {Guid.NewGuid()}";
            newItem1["DocDescription"] = "A12345";
            newItem1["DocType"] = "Business requirement";

            // Leader
            FieldLookupValue lv = new FieldLookupValue();
            lv.LookupId = 2;
            newItem1["ProjectList"] = lv;

            newItem1.Update();
            //context.Load(uploadfile);
            context.ExecuteQuery();
        }
    }
}
