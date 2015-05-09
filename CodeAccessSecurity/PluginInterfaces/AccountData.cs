using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NETInfrastructure.CodeAccessSecurity.PluginInterfaces
{
    [Serializable]
    public class AccountData
    {
        public int Id { get; set; }
        public string OwnerSurname { get; set; }
        public decimal Amount { get; set; }

        public override string ToString()
        {
            return "Id: " + Id + ", OwnerSurname: " + OwnerSurname + ", Amount: " + Amount;
        }
    }
}
