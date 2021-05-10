using Basic_CSOM;
using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using PermissionTraning.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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
using ListItem = Microsoft.SharePoint.Client.ListItem;
using ListSP = Microsoft.SharePoint.Client.List;

namespace PermissionTraning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string url = "https://m365b326364.sharepoint.com/sites/csom-training/finance";
        string user = "admin@m365b326364.onmicrosoft.com";
        string userAn = "anhoang@m365b326364.onmicrosoft.com";
        string listName = "Accounts";
        SecureString password = UtilApp.GetSecureString("Fgakdhsj123");
        private ClientContext context;

        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        public void Load()
        {
            Uri site = new Uri(url);

            context = AuthenticationManager.CreateClientContext(url, user, password);
            {
                var web = context.Web;
                context.Load(web, w => w.Title, w => w.Description);
            }

            //GetListPermission();
           
        }

        private void GetListPermission()
        {
            ListSP list = context.Web.Lists.GetByTitle(listName);
            context.Load(list, a => a.RoleAssignments);
            context.ExecuteQuery();

            IQueryable<RoleAssignment> queryForList = list.RoleAssignments.Include(roleAsg => roleAsg.Member,
                                                                                   roleAsg => roleAsg.RoleDefinitionBindings.Include(roleDef => roleDef.Name));
            Dictionary<string, string> permission = UtilCommon.GetPermissionDetails(context, queryForList);
        }

        private void ResetRole(ListCollection lists)
        {
            foreach (var item in lists)
            {
                item.BreakRoleInheritance(false, true);
            }
            context.ExecuteQuery();
        }

        private bool AssignPermssionDesigner(string listTitle, string accountAdd)
        {
            if (!UtilApp.IsExist(context, listTitle, Basic_CSOM.Enums.TypeSharepointEnum.List))
            {
                return false;
            }
            // get list 
            var oList = context.Web.Lists.GetByTitle(listTitle);

            // break role permission
            oList.BreakRoleInheritance(false, true);

            Web web = context.Web;
          
            context.Load(web, a => a.SiteUsers);
            context.ExecuteQuery();

            // Change permission
            Principal user = web.SiteUsers.GetByEmail(accountAdd);

            var designRole = new RoleDefinitionBindingCollection(context);
            designRole.Add(context.Web.RoleDefinitions.GetByType(RoleType.WebDesigner));
        
            RoleAssignment newRoleAssignment = oList.RoleAssignments.Add(user, designRole);

            context.Load(newRoleAssignment);
            context.ExecuteQuery();

            return true;
        }

        private void DeleteUniquePermissions(string listTitle, string accountAdd)
        {
            var list = context.Web.Lists.GetByTitle(listTitle);
            list.ResetRoleInheritance();

            context.ExecuteQuery();
        }

        private void AssignPermission_Click(object sender, RoutedEventArgs e)
        {
            AssignPermssionDesigner(listName, userAn);
        }

        private void DeletePermission_Click(object sender, RoutedEventArgs e)
        {
            DeleteUniquePermissions(listName, userAn);
        }
    }
}
