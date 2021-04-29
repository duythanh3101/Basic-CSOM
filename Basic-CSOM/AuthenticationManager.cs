using Microsoft.SharePoint.Client;
using System.Security;

namespace Basic_CSOM
{
    public static class AuthenticationManager
    {

        public static ClientContext CreateClientContext(string siteUrl, string username, SecureString securePassword)
        {
            ClientContext context = new ClientContext(siteUrl);
            context.Credentials = new SharePointOnlineCredentials(username, securePassword);
            return context;
        }
    }
}
