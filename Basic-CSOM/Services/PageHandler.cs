using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Publishing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Services
{
    public class PageHandler
    {
        public PageHandler()
        {

        }

        private ClientContext context;
        public PageHandler(ClientContext context)
        {
            this.context = context;
        }

        public void AddPublishingPage()
        {
            string pageName = "CustomPage2.aspx";
            Web webSite = context.Web;
            context.Load(webSite);
            PublishingWeb web = PublishingWeb.GetPublishingWeb(context, webSite);
            context.Load(web);

            if (web != null)
            {
                List pages = context.Site.RootWeb.Lists.GetByTitle("Pages");
                ListItemCollection defaultPages = pages.GetItems(CamlQuery.CreateAllItemsQuery());
                context.Load(defaultPages, items => items.Include(item => item.DisplayName).Where(obj => obj.DisplayName == pageName));
                context.ExecuteQuery();
                if (defaultPages != null && defaultPages.Count > 0)
                {
                }
                else
                {
                    List publishingLayouts = context.Site.RootWeb.Lists.GetByTitle("Master Page Gallery");
                    ListItemCollection allItems = publishingLayouts.GetItems(CamlQuery.CreateAllItemsQuery());
                    context.Load(allItems, items => items.Include(item => item.DisplayName).Where(obj => obj.DisplayName == "PageLayoutTemplate"));
                    context.ExecuteQuery();
                    ListItem layout = allItems.Where(x => x.DisplayName == "PageLayoutTemplate").FirstOrDefault();
                    context.Load(layout);
                    PublishingPageInformation publishingPageInfo = new PublishingPageInformation();
                    publishingPageInfo.Name = pageName;
                    publishingPageInfo.PageLayoutListItem = layout;
                    PublishingPage publishingPage = web.AddPublishingPage(publishingPageInfo);
                    publishingPage.ListItem.File.CheckIn(string.Empty, CheckinType.MajorCheckIn);
                    publishingPage.ListItem.File.Publish(string.Empty);
                    publishingPage.ListItem.File.Approve(string.Empty);
                    context.Load(publishingPage);
                    context.Load(publishingPage.ListItem.File, obj => obj.ServerRelativeUrl);
                    context.ExecuteQuery();
                }
            }
        }

    }
}
