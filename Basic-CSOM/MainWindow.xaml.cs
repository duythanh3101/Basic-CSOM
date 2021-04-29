using Microsoft.SharePoint.Client;
using System;
using System.Security;
using System.Windows;
using System.Linq;
using Basic_CSOM.Utils;
using Basic_CSOM.Entities;
using Basic_CSOM.Entities.ContentTypes;
using Basic_CSOM.Entities.Fields;

namespace Basic_CSOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = "https://m365b326364.sharepoint.com/sites/testcsom";
        string user = "admin@m365b326364.onmicrosoft.com";
        SecureString password = UtilApp.GetSecureString("Fgakdhsj123");

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            Uri site = new Uri(url);

            using (var context = AuthenticationManager.CreateClientContext(url, user, password))
            {
                var web = context.Web;
                context.Load(web, w => w.Title, w => w.Description);
                //var query = from list in web.Lists.Include(x => x.Fields) where list.Hidden == false && list.ItemCount > 0 select list;
                //var lists = context.LoadQuery(query);
                //context.ExecuteQuery();
                //Console.WriteLine($"Title: {web.Title}");

                var a = new ProjectDocumentContentTypeTemplate(context);
                a.Create();

                //var a = new NewSiteColumn(context);
                //a.SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='Field123' StaticName='Field123' DisplayName='Test Field 2' />";
                //a.InternalName = "Field123";
                //a.Create();
            }
        }


      
    }
}
