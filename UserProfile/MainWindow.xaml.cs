using Basic_CSOM;
using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Security;
using System.Text;
using System.Windows;

namespace UserProfile
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Load();
        }

        string serverUrl = "https://m365b326364-my.sharepoint.com";
        string targetUser = "admin@m365b326364.onmicrosoft.com";
        SecureString password = UtilApp.GetSecureString("Fgakdhsj123");
        private ClientContext clientContext;


        private void Load()
        {
            // Connect to the client context.
            clientContext = AuthenticationManager.CreateClientContext(serverUrl, targetUser, password);

            // Get the PeopleManager object and then get the target user's properties.
            PeopleManager people = new PeopleManager(clientContext);
            //PersonProperties personProperties = peopleManager.GetPropertiesFor(targetUser);


            UserCollection users = clientContext.Web.SiteUsers;
            clientContext.Load(users);
            clientContext.ExecuteQuery();

            //StringBuilder items = new StringBuilder();

            //string[] userProfileProperties = { "AccountName", "FirstName", "LastName", "PreferredName", "Manager", "AboutMe", "PersonalSpace", "PictureURL", "UserName", "WorkEmail", "SPS-Birthday" };

            //foreach (string propertyKey in userProfileProperties)
            //{
            //    items.Append(propertyKey);
            //    items.Append(",");
            //}
            //items.AppendLine();
            foreach (User item in users)
            {
                PersonProperties personProperties = people.GetPropertiesFor(item.LoginName);
                clientContext.Load(personProperties, p => p.AccountName, p => p.UserProfileProperties);
                clientContext.ExecuteQuery();

                if (personProperties.UserProfileProperties != null)
                {
                    //Loop through user properties
                    foreach (var property in personProperties.UserProfileProperties)
                    {
                        Console.WriteLine(string.Format("{0}: {1}",
                            property.Key.ToString(), property.Value.ToString()));
                    }
                }
               
            }

        }
    }
}
