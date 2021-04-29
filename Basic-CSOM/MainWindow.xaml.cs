using Microsoft.SharePoint.Client;
using System;
using System.Security;
using System.Windows;
using System.Linq;
using Basic_CSOM.Utils;
using Basic_CSOM.Entities;

namespace Basic_CSOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string url = "https://m365b326364.sharepoint.com/sites/sharepoint";
        string user = "admin@m365b326364.onmicrosoft.com";
        SecureString password = UtilApp.GetSecureString("");

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
                var query = from list in web.Lists.Include(x => x.Fields) where list.Hidden == false && list.ItemCount > 0 select list;
                var lists = context.LoadQuery(query);
                //context.ExecuteQuery();
                //Console.WriteLine($"Title: {web.Title}");

                var a = new BaseField(context);
                a.SchemaXml = $"<Field ID='{Guid.NewGuid()}' Type='Text' Name='TestFiled2' StaticName='TestFiled2' DisplayName='Test Field 2' />";
                a.Description = "Test create new field";
                a.DisplayName = "Test Field";
                a.InternalName = "TestFiled2";
                a.Create();

            }
        }


      
    }
}
