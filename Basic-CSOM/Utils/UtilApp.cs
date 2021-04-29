using System;
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
    }
}
