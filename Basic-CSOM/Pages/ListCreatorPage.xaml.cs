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
using List = Microsoft.SharePoint.Client.List;
using ListItemCollection = Microsoft.SharePoint.Client.ListItemCollection;

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for ListCreatorPage.xaml
    /// </summary>
    public partial class ListCreatorPage : Page
    {
        private ClientContext context;
        private List currentList;

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

            EmployeeContentType.Text = "EmployeeTestList2";
            ProjectContentType.Text = "Project Ver 2";
            ProjectDocContentType.Text = "Project Document";
        }

        private void EmployeeList_OnClick(object sender, RoutedEventArgs e)
        {
            string name = EmployeeListName.Text.ToString().Trim();
            string contentType = EmployeeContentType.Text.ToString().Trim();
            CreateList<EmployeeList>(name, contentType);
        }

        private void ProjectList_OnClick(object sender, RoutedEventArgs e)
        {
            string name = ProjectListName.Text.ToString().Trim();
            string contentType = ProjectContentType.Text.ToString().Trim();
            CreateList<ProjectList>(name, contentType);
        }

        private void ProjectDocList_OnClick(object sender, RoutedEventArgs e)
        {
            string name = ProjectDocListName.Text.ToString().Trim();
            string contentType = ProjectDocContentType.Text.ToString().Trim();
            CreateList<DocProjectList>(name, contentType);
        }

        private void CreateList<T>(string name, string contentType) where T: BaseList
        {
            if (UtilApp.IsExist(context, name, Enums.TypeSharepointEnum.List))
            {
                MessageBox.Show("List is existed. Please change the another name", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var instance = (BaseList)Activator.CreateInstance(typeof(T), context);
                instance.Title = name;
                instance.ContentTypeName = contentType;

                currentList = instance.Generate();
                MessageBox.Show("List is created successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
