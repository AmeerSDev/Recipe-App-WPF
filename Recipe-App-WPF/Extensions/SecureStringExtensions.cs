using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Extensions
{
    public static class SecureStringExtensions
    {
        public static string ToUnsecuredString(this SecureString secureString)
        {
            if (secureString == null)
            {
                // If SecureString is null, return an empty string
                return string.Empty;
            }

            if (secureString.Length == 0)
            {
                // If SecureString is empty, return an empty string
                return string.Empty;
            }

            IntPtr ptr = IntPtr.Zero;

            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        public static SecureString ToSecureString(this string value)
        {
            SecureString secureString = new SecureString();

            if (!string.IsNullOrEmpty(value))
            {
                foreach (char c in value)
                {
                    secureString.AppendChar(c);
                }
            }

            // Make the SecureString read-only to enhance security
            secureString.MakeReadOnly();

            return secureString;
        }
    }

}
