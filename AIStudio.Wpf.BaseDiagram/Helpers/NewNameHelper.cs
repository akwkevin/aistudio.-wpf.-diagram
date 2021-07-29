using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Wpf.BaseDiagram.Helpers
{
    public class NewNameHelper
    {
        public static string GetNewName(IEnumerable<string> names, string firstName = null)
        {
            string result = string.Empty;
            int i = 1;
            if (firstName == null)
            {
                firstName = "新建-";               
            }
            
            result = firstName + i;            

            while (names.Any(o => { return o == result; })) //存在同名则继续累加
            {
                result = firstName + ++i;
            }

            return result;
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            public static extern int StrCmpLogicalW(string psz1, string psz2);
        }

        public sealed class NaturalStringComparer : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                return SafeNativeMethods.StrCmpLogicalW(a, b);
            }
        }

        public sealed class NaturalFileInfoNameComparer : IComparer<FileInfo>
        {
            public int Compare(FileInfo a, FileInfo b)
            {
                return SafeNativeMethods.StrCmpLogicalW(a.Name, b.Name);
            }
        }
    }
}
