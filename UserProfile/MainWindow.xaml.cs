using Basic_CSOM;
using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using UserProfile.Entities;

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
            Init();
            Load();
        }

        string serverUrl = "https://m365b326364-my.sharepoint.com";
        string adminUrl = "https://m365b326364-admin.sharepoint.com";
        string targetUser = "admin@m365b326364.onmicrosoft.com";
        SecureString password = UtilApp.GetSecureString("Fgakdhsj123");
        private ClientContext clientContext;
        private ObservableCollection<UserProfileModel> UserProfileList;
        private PeopleManager people;
        private void Init()
        {
            UserProfileList = new ObservableCollection<UserProfileModel>();
        }

        private void Load()
        {
            // Connect to the client context.
            clientContext = AuthenticationManager.CreateClientContext(serverUrl, targetUser, password);

            // Get the PeopleManager object and then get the target user's properties.
            people = new PeopleManager(clientContext);

            UserCollection users = clientContext.Web.SiteUsers;
            clientContext.Load(users);
            clientContext.ExecuteQuery();

            foreach (User item in users)
            {
                PersonProperties personProperties = people.GetPropertiesFor(item.LoginName);
                clientContext.Load(personProperties, p => p.AccountName, p => p.UserProfileProperties);
                clientContext.ExecuteQuery();

                try
                {
                    var userProfile = new UserProfileModel();
                    userProfile.FirstName = personProperties.UserProfileProperties["FirstName"];
                    userProfile.LastName = personProperties.UserProfileProperties["LastName"];
                    userProfile.UserName = personProperties.UserProfileProperties["UserName"];
                    userProfile.WorkPhone = personProperties.UserProfileProperties["WorkPhone"];
                    userProfile.Department = personProperties.UserProfileProperties["Department"];
                    userProfile.AccountName = personProperties.UserProfileProperties["AccountName"];
                    UserProfileList.Add(userProfile);
                }
                catch (Exception)
                {

                }


            }
            UserProfileGrid.ItemsSource = UserProfileList;
        }

        private void Edit_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is UserProfileModel us)
                {
                    // Display message box
                    MessageBoxResult result = MessageBox.Show("Do you want to save this data", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Process message box results
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            Edit(us);
                            // Refresh the values.
                            RefreshUIValues();
                            MessageBox.Show("Save successfully");
                            break;
                        case MessageBoxResult.Cancel:
                        case MessageBoxResult.No:
                        default:
                            break;
                    }

                }
            }

        }

        private void Edit(UserProfileModel us)
        {
            if (us != null)
            {
                using (ClientContext context = AuthenticationManager.CreateClientContext(adminUrl, targetUser, password))
                {
                    // Get the people manager instance and initialize the account name.
                    var peopleManager = new PeopleManager(context);

                    // Update the property for the user using account name from the user's profile.
                    peopleManager.SetSingleValueProfileProperty(us.AccountName, "FirstName", us.FirstName);
                    peopleManager.SetSingleValueProfileProperty(us.AccountName, "LastName", us.LastName);
                    peopleManager.SetSingleValueProfileProperty(us.AccountName, "WorkPhone", us.WorkPhone);
                    context.ExecuteQuery();
                }
            }
        }

        private void RefreshUIValues()
        {

        }
    }
}
