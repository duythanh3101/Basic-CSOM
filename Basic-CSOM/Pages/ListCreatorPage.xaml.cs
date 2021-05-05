using Basic_CSOM.Entities.Lists;
using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for ListCreatorPage.xaml
    /// </summary>
    public partial class ListCreatorPage : Page
    {
        private ClientContext context;

        public ListCreatorPage()
        {
            InitializeComponent();
        }

        public ListCreatorPage(ClientContext context)
        {
            InitializeComponent();
            this.context = context;

            EmployeeListName.Text = "EmployeeList";
            ProjectListName.Text = "Project";
            ProjectDocListName.Text = "Project Document";
        }

        private void EmployeeList_OnClick(object sender, RoutedEventArgs e)
        {
            string name = EmployeeListName.Text.ToString().Trim();
            if (UtilApp.IsExist(context, name, Enums.TypeSharepointEnum.List))
            {
                MessageBox.Show("List is existed. Please change the another name", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var template = new EmployeeList(context)
                {
                    Title = name
                };
                template.Generate();
                MessageBox.Show("List is created successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ProjectList_OnClick(object sender, RoutedEventArgs e)
        {

        }

        private void ProjectDocList_OnClick(object sender, RoutedEventArgs e)
        {

        }

        //private void CreateList<BaseList>(string name)
        //{
        //    if (UtilApp.IsExist(context, name, Enums.TypeSharepointEnum.List))
        //    {
        //        MessageBox.Show("List is existed. Please change the another name", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //    else
        //    {
        //        var template = new BaseList(context)
        //        {
        //            Title = name
        //        };
        //        template.Generate();
        //        MessageBox.Show("List is created successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}
    }
}
