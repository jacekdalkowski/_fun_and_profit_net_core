using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETInfrastructure.CodeAccessSecurity.PluginInterfaces
{
    public interface IFileSystemStorage
    {
        AccountData LoadAccountData();

        void SaveAccountData(AccountData accountData);
    }
}
