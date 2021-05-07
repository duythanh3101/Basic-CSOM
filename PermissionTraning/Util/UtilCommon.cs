using Microsoft.SharePoint.Client;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PermissionTraning.Util
{
    public static class UtilCommon
    {

        /// <summary>    
        /// This funtion get the site/list/list item permission details. And return it by a dictonary.    
        /// </summary>    
        /// <param name="clientContext">type ClientContext</param>    
        /// <param name="queryString">type IQueryable<RoleAssignment></param>    
        /// <returns>return type is Dictionary<string, string></returns>    
        public static Dictionary<string, string> GetPermissionDetails(ClientContext clientContext, IQueryable<RoleAssignment> queryString)
        {
            IEnumerable roles = clientContext.LoadQuery(queryString);
            clientContext.ExecuteQuery();

            Dictionary<string, string> permisionDetails = new Dictionary<string, string>();
            foreach (RoleAssignment ra in roles)
            {
                var rdc = ra.RoleDefinitionBindings;
                string permission = string.Empty;
                foreach (var rdbc in rdc)
                {
                    permission += rdbc.Name.ToString() + ", ";
                }
                permisionDetails.Add(ra.Member.Title, permission);
            }
            return permisionDetails;
        }
    }
}
