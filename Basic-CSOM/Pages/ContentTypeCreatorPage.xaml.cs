using Basic_CSOM.Entities.ContentTypes;
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
    /// Interaction logic for ContentTypeCreatorPage.xaml
    /// </summary>
    public partial class ContentTypeCreatorPage : Page
    {
        private ClientContext context;

        public ContentTypeCreatorPage()
        {
            InitializeComponent();
        }

        public ContentTypeCreatorPage(ClientContext context)
        {
            InitializeComponent();
            this.context = context;

            EmployeeContentTypeName.Text = "EmployeeTestList";
            ProjectContentTypeName.Text = "Project";
            ProjectDocContentTypeName.Text = "Project Document";
        }

        private void EmployeeContentType_OnClick(object sender, RoutedEventArgs e)
        {
            string name = EmployeeContentTypeName.Text.ToString().Trim();
            if (UtilApp.IsExist(context, name, Enums.TypeSharepointEnum.ContentType))
            {
                MessageBox.Show("Content type is existed. Please change the another name", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                EmployeeContentTypeTemplate template = new EmployeeContentTypeTemplate(context)
                {
                    Name = name
                };
                template.Create();
                MessageBox.Show("Content type is created successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void ProjectContentType_OnClick(object sender, RoutedEventArgs e)
        {
            string name = ProjectContentTypeName.Text.ToString().Trim();
            if (UtilApp.IsExist(context, name, Enums.TypeSharepointEnum.ContentType))
            {
                MessageBox.Show("Content type is existed. Please change the another name", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ProjectContentTypeTemplate template = new ProjectContentTypeTemplate(context)
                {
                    Name = name
                };
                template.Create();
                MessageBox.Show("Content type is created successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ProjectDocContentType_OnClick(object sender, RoutedEventArgs e)
        {
            string name = ProjectDocContentTypeName.Text.ToString().Trim();
            if (UtilApp.IsExist(context, name, Enums.TypeSharepointEnum.ContentType))
            {
                MessageBox.Show("Content type is existed. Please change the another name", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                ProjectDocumentContentTypeTemplate template = new ProjectDocumentContentTypeTemplate(context)
                {
                    Name = name
                };
                template.Create();
                MessageBox.Show("Content type is created successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }
    }
}
