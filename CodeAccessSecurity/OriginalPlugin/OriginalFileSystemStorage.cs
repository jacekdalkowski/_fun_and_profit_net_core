using NETInfrastructure.CodeAccessSecurity.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

[assembly: SecurityRules(SecurityRuleSet.Level2)]
[assembly: SecurityTransparent]

namespace NETInfrastructure.CodeAccessSecurity.OriginalPlugin
{

    //[FileIOPermission(SecurityAction.RequestMinimum, Unrestricted=true)]
    //Warning	1	'System.Security.Permissions.SecurityAction.RequestMinimum' is obsolete: 
    //'"Assembly level declarative security is obsolete and is no longer enforced by the CLR by default.
    //See http://go.microsoft.com/fwlink/?LinkID=155570 for more information."'	

    public class OriginalFileSystemStorage : MarshalByRefObject, IFileSystemStorage
    {
        public AccountData LoadAccountData()
        {
            return new AccountData()
            {
                Id = 1,
                Amount = 150,
                OwnerSurname = "Dalkowsky Jacek"
            };
        }


        public void SaveAccountData(AccountData accountData)
        {
            unsafe
            {
                byte[] array = new byte[10];
                fixed(byte* pArray = array) // will fail in runtime
                {
                    (*pArray) = 4;
                }
            }
        }


    }
}
