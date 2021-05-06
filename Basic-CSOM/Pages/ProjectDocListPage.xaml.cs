using Basic_CSOM.Utils;
using Microsoft.SharePoint.Client;
using System.Windows;
using System.Windows.Controls;

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for ProjectDocListPage.xaml
    /// </summary>
    public partial class ProjectDocListPage : Page
    {
        public ProjectDocListPage()
        {
            InitializeComponent();
        }

        private ClientContext context;
        private ListSP oList;
        public ProjectDocListPage(ClientContext context, string listName = "ProjectList")
        {
            InitializeComponent();

            this.context = context;
            if (UtilApp.IsExist(context, listName, Enums.TypeSharepointEnum.List))
            {
                Load(listName);
            }
            else
            {
                MessageBox.Show($"List name {listName} is not existed");
            }
        }

        private void Load(string listName)
        {
            oList = context.Web.Lists.GetByTitle(listName);

            CamlQuery camlQuery = new CamlQuery();
            camlQuery.ViewXml = @"<View><RowLimit>100</RowLimit></View>";
            ListItemCollection collListItem = oList.GetItems(camlQuery);
            context.Load(collListItem, items => items.Include(item => item.Id, item => item.DisplayName, item => item.FieldValuesForEdit));
            context.ExecuteQuery();
        }
    }
}
