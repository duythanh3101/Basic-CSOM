using Microsoft.Online.SharePoint.TenantAdministration;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Services
{
    public class SiteHandler
    {
        private readonly ClientContext tenantContext;

        public SiteHandler(ClientContext clientContext)
        {
            tenantContext = clientContext;
        }

        public string CreateSite(string rootSiteUrl, string siteUrl, string userName, string siteTitle)
        {
            siteUrl = rootSiteUrl + "/sites/" + siteUrl;

            var tenant = new Tenant(tenantContext);
            //Properties of the New SiteCollection
            var siteCreationProperties = new SiteCreationProperties();

            //New SiteCollection Url
            siteCreationProperties.Url = siteUrl;

            //Title of the Root Site
            siteCreationProperties.Title = siteTitle;

            //Login name of Owner
            siteCreationProperties.Owner = userName;

            //Template of the Root Site. Using Team Site for now.
            siteCreationProperties.Template = "BLANKINTERNETCONTAINER#0";

            //Storage Limit in MB
            siteCreationProperties.StorageMaximumLevel = 100;

            //UserCode Resource Points Allowed
            siteCreationProperties.UserCodeMaximumLevel = 50;
            siteCreationProperties.TimeZoneId = 7;

            //Create the SiteCollection
            SpoOperation spo = tenant.CreateSite(siteCreationProperties);

            tenantContext.Load(tenant);

            //We will need the IsComplete property to check if the provisioning of the Site Collection is complete.
            tenantContext.Load(spo, i => i.IsComplete);

            tenantContext.ExecuteQuery();

            //Check if provisioning of the SiteCollection is complete.
            while (!spo.IsComplete)
            {
                //Wait for 30 seconds and then try again
                System.Threading.Thread.Sleep(30000);
                spo.RefreshLoad();
                tenantContext.Load(spo);
                tenantContext.ExecuteQuery();
            }

            Console.WriteLine("Site Created.");
            return siteUrl;
        }

        public Web CreateHRSubsite()
        {
            try
            {
                WebCreationInformation webCreationInfo = new WebCreationInformation();
                // This is relative URL of the url provided in context
                webCreationInfo.Url = "HR";
                webCreationInfo.Title = "HR Department";
                webCreationInfo.Description = "Subsite for HR";

                // This will inherit permission from parent site
                webCreationInfo.UseSamePermissionsAsParentSite = true;

                // "STS#0" is the code for 'Team Site' template
                webCreationInfo.WebTemplate = "STS#0";
                webCreationInfo.Language = 1033;

                Web web = tenantContext.Site.RootWeb.Webs.Add(webCreationInfo);
                tenantContext.Load(web);
                tenantContext.ExecuteQuery();
                return web;
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
