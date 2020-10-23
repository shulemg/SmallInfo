using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace SmallInfo.Desktop
{
    class CreateDocuments
    {
        FixedDocument fixedDocument = new FixedDocument();
        void doc()
        {
            fixedDocument.DocumentPaginator.PageSize = new Size(96 * 8.5, 96 * 11);
            PageContent pageContent = new PageContent();
            
            DataTemplate dataTemplate = new DataTemplate();
            Table table = new Table();
            
        }
    }
    class DocumentW_OneCampaign
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Amount { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public decimal TotalBalance { get; set; }
    }
}
