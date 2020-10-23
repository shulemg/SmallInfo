using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallInfo.Data;

namespace SmallInfo
{
    static class Querys
    {
        public static List<Account> CurrentInfo(SmallInfoEntities smallInfoEntities)
        {
            return smallInfoEntities.Accounts.ToList();
        }
    }
}
