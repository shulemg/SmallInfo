using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using SmallInfo.Data;

namespace SmallInfo
{
    class CreateDocuments
    {
        static int DocumentNum;
        static int AccountsNum;
        static ObservableCollection<DocumnetW_OneCampaign> documnetsW_OneCampaign;
        static DocumnetW_OneCampaign documnetW_OneCampaign;

        public static ObservableCollection<DocumnetW_OneCampaign> CreateDocumentW_OneCampaign(SmallInfoEntities smallInfoEntities,string _campaign)
        {
            List<Account> CurrentInfo = Querys.CurrentInfo(smallInfoEntities);
            documnetsW_OneCampaign = new ObservableCollection<DocumnetW_OneCampaign>();
            foreach (var account in CurrentInfo)
            {
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
    class DocumnetW_OneCampaign
    {
        public ObservableCollection<AccountW_OneCampaign> Accounts { get; set; }
        public string Campaign { get; set; }
        public int DocumentNum { get; set; }
    }
    class AccountW_OneCampaign
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
