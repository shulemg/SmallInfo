using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallInfo.Data;

namespace Reports
{
    public class DocumentCreater
    {
        static int DocumentNum;
        static int AccountsNum;
        public static ObservableCollection<DocumnetW_OneCampaign> documnetsW_OneCampaign;
        static DocumnetW_OneCampaign documnetW_OneCampaign;

        public static ObservableCollection<DocumnetW_OneCampaign> CreateDocumentW_OneCampaign(List<Account> _accounts, string _campaign)
        {
            List<Account> CurrentInfo = _accounts;
            documnetsW_OneCampaign = new ObservableCollection<DocumnetW_OneCampaign>();
            foreach (var account in CurrentInfo.Where(a => a.Pledges.Where(p => p.Campaign == _campaign).Count() > 0).OrderBy(a => a.HLastName).ThenBy(a => a.HLastName))
            {
                AccountW_OneCampaign accountW_One = new AccountW_OneCampaign();
                accountW_One.Name = account.HFirstName + " " + account.HLastName;
                accountW_One.Address = account.defaultAddress.Address1;
                foreach (Pledge pledge in account.Pledges)
                {
                    accountW_One.Amount = pledge.Amount;
                    accountW_One.Paid = pledge.PaidString;
                    accountW_One.Balance = pledge.BalanceString;
                }
                AccountsNum--;
                if (AccountsNum == 0)
                    CreateNewDocument();
                
            }
            return documnetsW_OneCampaign;
        }
        public static void CreateNewDocument()
        {
            DocumnetW_OneCampaign documnetW_OneCampaign = new DocumnetW_OneCampaign();
            documnetW_OneCampaign.Accounts = new ObservableCollection<AccountW_OneCampaign>();
            documnetsW_OneCampaign.Add(documnetW_OneCampaign);
            DocumentNum++;
            documnetW_OneCampaign.DocumentNum = DocumentNum;
            AccountsNum = 33;
        }
    }
    public class DocumnetW_OneCampaign
    {
        public ObservableCollection<AccountW_OneCampaign> Accounts { get; set; }
        public string Campaign { get; set; }
        public int DocumentNum { get; set; }
    }
    public class AccountW_OneCampaign
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public string Paid { get; set; }
        public string Balance { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
