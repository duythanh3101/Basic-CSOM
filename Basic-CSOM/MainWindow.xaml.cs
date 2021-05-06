using Microsoft.SharePoint.Client;
using System;
using System.Security;
using System.Windows;
using System.Linq;
using Basic_CSOM.Utils;
using Basic_CSOM.Entities;
using Basic_CSOM.Entities.ContentTypes;
using Basic_CSOM.Entities.Fields;
using Basic_CSOM.Entities.Lists;
using System.Collections.Generic;
using Basic_CSOM.Services;
using Basic_CSOM.Pages;

namespace Basic_CSOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = "https://m365b326364.sharepoint.com/sites/basiccsom";
        string user = "admin@m365b326364.onmicrosoft.com";
        SecureString password = UtilApp.GetSecureString("Fgakdhsj123");
        private ClientContext context;

        public MainWindow()
        {
            InitializeComponent();

            Load();
            MainFrame.Content = new ContentTypeCreatorPage(context);
        }

        public void Load()
        {
            Uri site = new Uri(url);

            context = AuthenticationManager.CreateClientContext(url, user, password);
            {
                var web = context.Web;
                context.Load(web, w => w.Title, w => w.Description);
                //var query = from list in web.Lists.Include(x => x.Fields) where list.Hidden == false && list.ItemCount > 0 select list;
                //var lists = context.LoadQuery(query);
                //context.ExecuteQuery();
                //Console.WriteLine($"Title: {web.Title}");

                //var a = new ProjectContentTypeTemplate(context);
                //a.Create();

                //var a = new NewSiteColumn(context);
                //a.SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='Field123' StaticName='Field123' DisplayName='Test Field 2' />";
                //a.InternalName = "Field123";
                //a.Create();

                //var list = new ProjectList(context);
                //list.Generate();

                //SiteHandler siteHandler = new SiteHandler(context);
                //siteHandler.CreateHRSubsite();
            }

           
        }

      
        private void ContentTypeButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ContentTypeCreatorPage(context);
        }

       
        private void EmployeeListButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new EmployeeListPage(context);
        }

        private void ProjectListButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProjectListPage(context);
        }

        private void ProjectDocListButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ProjectDocListPage(context);
        }

        private void ListButton_Clicked(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new ListCreatorPage(context);
        }

        private void CreatePage_Clicked(object sender, RoutedEventArgs e)
        {

            PageHandler pageHandler = new PageHandler(context);
            pageHandler.AddPublishingPage();
        }
    }
}
