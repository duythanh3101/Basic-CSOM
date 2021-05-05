using Basic_CSOM.Enums;
using Microsoft.SharePoint.Client;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Security;

namespace Basic_CSOM.Utils
{
    public static class UtilApp
    {
        public static SecureString GetSecureString(string password)
        {

            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        public static bool IsExist(ClientContext context, string fieldToCheck, TypeSharepointEnum type)
        {
            bool isExist = false;
            ListCollection listCollection = context.Web.Lists;
            ContentTypeCollection cntCollection = context.Web.ContentTypes;
            FieldCollection fldCollection = context.Web.Fields;
            switch (type)
            {
                case TypeSharepointEnum.List:
                    context.Load(listCollection, lsts => lsts.Include(list => list.Title).Where(list => list.Title == fieldToCheck));
                    context.ExecuteQuery();
                    isExist = listCollection.Count > 0;
                    break;
                case TypeSharepointEnum.ContentType:
                    context.Load(cntCollection, cntyp => cntyp.Include(ct => ct.Name).Where(ct => ct.Name == fieldToCheck));
                    context.ExecuteQuery();
                    isExist = cntCollection.Count > 0;
                    break;
                //case "contenttypeName":
                //    context.Load(cntCollection, cntyp => cntyp.Include(ct => ct.Name, ct => ct.Id).Where(ct => ct.Name == fieldToCheck));
                //    context.ExecuteQuery();
                //    //foreach (ContentType ct in cntCollection)
                //    //{
                //    //    return ct.Id.ToString();
                //    //}
                //    isExist = cntCollection.Count > 0;
                //    break;
                case TypeSharepointEnum.Site:
                    context.Load(fldCollection, fld => fld.Include(ft => ft.Title).Where(ft => ft.Title == fieldToCheck));
                    try
                    {
                        context.ExecuteQuery();
                        isExist = fldCollection.Count > 0;
                    }
                    catch (Exception e)
                    {
                        if (e.Message == "Unknown Error")
                        {
                            isExist = fldCollection.Count > 0;
                        }
                    }
                    break;
                case TypeSharepointEnum.ListContentType:
                    List lst = context.Web.Lists.GetByTitle(fieldToCheck);
                    ContentTypeCollection lstcntype = lst.ContentTypes;
                    context.Load(lstcntype, lstc => lstc.Include(lc => lc.Name).Where(lc => lc.Name == fieldToCheck));
                    context.ExecuteQuery();
                    isExist = lstcntype.Count > 0;
                    break;
            }
            return isExist;
        }

        public static string GetStringFromEnum<TEnum>(this TEnum enumVal)
        {
            string res = null;

            Type t = typeof(TEnum);
            var val = t.GetEnumName(enumVal);

            if (val == null)
            {
                return null;
            }
            var memInfo = t.GetMember(val);
            if (memInfo.Length > 0)
            {
                var defaultVal = memInfo[0].GetCustomAttribute<DefaultValueAttribute>();
                if (defaultVal != null && defaultVal.Value != null && defaultVal.Value.GetType() == typeof(string))
                {
                    return (string)defaultVal.Value;
                }
            }

            return res;
        }
    }
}
