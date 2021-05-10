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

        string rootUrl = "https://m365b326364.sharepoint.com/sites/csom-training";
        string url = "https://m365b326364.sharepoint.com/sites/csom-training/finance";
        string user = "admin@m365b326364.onmicrosoft.com";
        string userAn = "anhoang@m365b326364.onmicrosoft.com";
        string listName = "Accounts";
        string testlevel = "Test Level";
        string testgroup = "Test Group";
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

            //context.Load(web, a => a.SiteUsers);
            context.ExecuteQuery();

            // Change permission
            Principal user = web.SiteUsers.GetByEmail(accountAdd);

            var designRole = new RoleDefinitionBindingCollection(context);
            designRole.Add(context.Web.RoleDefinitions.GetByType(RoleType.WebDesigner));

            oList.RoleAssignments.Add(user, designRole);

            //context.Load(newRoleAssignment);
            context.ExecuteQuery();

            return true;
        }

        private void DeleteUniquePermissions(string listTitle, string accountAdd)
        {
            var list = context.Web.Lists.GetByTitle(listTitle);
            list.ResetRoleInheritance();

            context.ExecuteQuery();
        }

        /// <summary>
        /// 3-1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssignPermission_Click(object sender, RoutedEventArgs e)
        {
            AssignPermssionDesigner(listName, userAn);
        }

        /// <summary>
        /// 3-2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeletePermission_Click(object sender, RoutedEventArgs e)
        {
            DeleteUniquePermissions(listName, userAn);
        }

        public void CreateCustomPermissionLevel(string permissionName)
        {
            try
            {
                context = AuthenticationManager.CreateClientContext(rootUrl, user, password);
                var web = context.Web;
                context.Load(web, w => w.Title, w => w.Description);


                // Set up permissions.
                BasePermissions permissions = new BasePermissions();
                permissions.Set(PermissionKind.ManageLists);
                permissions.Set(PermissionKind.CreateAlerts);
                permissions.Set(PermissionKind.ViewListItems);

                // create
                RoleDefinitionCreationInformation roleDefinitionCreationInformation = new RoleDefinitionCreationInformation();
                roleDefinitionCreationInformation.BasePermissions = permissions;
                roleDefinitionCreationInformation.Name = permissionName;
                roleDefinitionCreationInformation.Description = "Custom Permission Level";
                context.Web.RoleDefinitions.Add(roleDefinitionCreationInformation);

                //context.Load(roleDefinitionCreationInformation);
                context.ExecuteQuery();
            }
            catch (Exception)
            {

            }
        }

        public void CreateCustomGroup(string groupName)
        {
            context = AuthenticationManager.CreateClientContext(rootUrl, user, password);
            var web = context.Web;
            context.Load(web.RoleDefinitions);
            context.Load(web, w => w.Title, w => w.Description);

            GroupCreationInformation groupCreationInfo = new GroupCreationInformation();
            groupCreationInfo.Title = groupName;
            groupCreationInfo.Description = "Custom Group Created...";
            User owner = web.EnsureUser(user);
            User member = web.EnsureUser(userAn);
            Group group = web.SiteGroups.Add(groupCreationInfo);
            group.Owner = owner;
            group.Users.AddUser(member);
            group.Update();
            context.ExecuteQuery();

            var roleDefinitions = web.RoleDefinitions;

            // Get test level Role Definition
            var permissionLevel = roleDefinitions.GetByName(testlevel);
            context.Load(permissionLevel);
            context.ExecuteQuery();

            RoleDefinitionBindingCollection collRDB = new RoleDefinitionBindingCollection(context);
            collRDB.Add(permissionLevel);

            // Bind the Newly Created Permission Level to Owners Group
            web.RoleAssignments.Add(group, collRDB);

            context.ExecuteQuery();
        }

        /// <summary>
        /// 4-2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatePermission_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomPermissionLevel(testlevel);
        }

        /// <summary>
        /// 4-4
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateGroup_Click(object sender, RoutedEventArgs e)
        {
            CreateCustomGroup(testgroup);
        }
    }
}
