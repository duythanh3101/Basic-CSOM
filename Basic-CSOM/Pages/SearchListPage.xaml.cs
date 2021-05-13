using Basic_CSOM.Entities.Models;
using Basic_CSOM.Services;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace Basic_CSOM.Pages
{
    /// <summary>
    /// Interaction logic for SearchListPage.xaml
    /// </summary>
    public partial class SearchListPage : Page
    {
        public SearchListPage()
        {
            InitializeComponent();
        }

        private ClientContext clientContext;
        public ObservableCollection<SearchResultItem> DataList;
        
        public SearchListPage(ClientContext context)
        {
            InitializeComponent();
            clientContext = context;
            SearchTextBox.Text = "NGO";
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            DataList = new ObservableCollection<SearchResultItem>();
            var searchHandler = new SearchHandler(clientContext);
            var result = searchHandler.Search(SearchTextBox.Text.ToString());
            foreach (var item in result.Value)
            {
                foreach (var res in item.ResultRows)
                {
                    try
                    {
                        var searchItem = new SearchResultItem()
                        {
                            Title = res["Title"].ToString(),
                            Description = res["Description"].ToString(),
                            ParentUrl = res["ParentLink"].ToString(),
                        };
                        DataList.Add(searchItem);
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            searchListGrid.ItemsSource = DataList;
        }

        private void Copy_Clicked(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.DataContext is SearchResultItem result)
                {
                    Clipboard.SetText(result.ParentUrl);
                }
            }
        }
    }
}
