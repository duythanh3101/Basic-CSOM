using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_CSOM.Services
{
    public class SearchHandler
    {
        private ClientContext clientContext;
        
        public SearchHandler(ClientContext context)
        {
            clientContext = context;
        }

        public ClientResult<ResultTableCollection> Search(string keyword)
        {
            KeywordQuery keywordQuery = new KeywordQuery(clientContext);
            keywordQuery.QueryText = keyword;
            SearchExecutor searchExecutor = new SearchExecutor(clientContext);
            ClientResult<ResultTableCollection> results = searchExecutor.ExecuteQuery(keywordQuery);
            clientContext.ExecuteQuery();

            return results;
        }
    }
}
