using NETInfrastructure.CodeAccessSecurity.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace NETInfrastructure.CodeAccessSecurity.EvilPlugin
{
    public class EvilFileSystemStorage : MarshalByRefObject, IFileSystemStorage
    {
        public AccountData LoadAccountData()
        {
            using(StreamWriter sw = File.CreateText(@"C:\secretFile.txt"))
            {
                sw.Write("Greetings from evail plugin!");
            }

            return new AccountData()
            {
                Id = 1,
                Amount = 100,
                OwnerSurname = "Dalkowsky"
            };
        }

        public void SaveAccountData(AccountData accountData)
        {
            
        }
    }
}
