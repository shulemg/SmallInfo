using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallInfo.Data;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;

namespace SmallInfo.Reports
{
    public class DocumentCreater
    {
        public static ObservableCollection<DocumnetW_OneCampaign> documnetsW_OneCampaign;

        public static ObservableCollection<string> Reports()
        {
            return new ObservableCollection<string>{
                {"List" },
                {"List W Campaign" },
                { "One Campaign W Note" },
                { "One Campaign W Note En"},             
            };
        }

        public static void CreateReport(string _report, List<Account> accounts, List<string> campaigns, ReportViewer reportViewer,string reportHeader ="")
        {
            switch (_report)
            {
                case "One Campaign W Note":
                    CreateDocumentW_OneCampaign(_report,accounts, campaigns, reportViewer,reportHeader);
                    break;
                case "One Campaign W Note En":
                    CreateDocumentW_OneCampaign_En(_report, accounts, campaigns, reportViewer, reportHeader);
                    break;
                case "List":
                    CreateDocumentList(accounts, reportViewer);
                    break;
                case "List W Campaign":
                    CreateDocumentListW_Campaign(accounts,campaigns, reportViewer);
                    break;
                default:
                    break;
            }
        }

        
        public static void CreateDocumentW_OneCampaign(string _report, List<Account> _accounts, List<string> _campaigns, ReportViewer _reportViewer,string reportHeader)
        {
            DataSet1 dataset = new DataSet1();

            foreach (string item in _campaigns)
            {         
                ReportDataSource reportDataSource1 = new ReportDataSource();    
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                //reportDataSource1.Value = dataset.OneCampaign;
                reportDataSource1.Value = dataset.Onecampaign;
                _reportViewer.LocalReport.DataSources.Clear();
                _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                _reportViewer.LocalReport.ReportPath = "Reports/ReportOneCampaign.rdlc";
                _reportViewer.ZoomMode = ZoomMode.PageWidth;
                dataset.EndInit();
                
                string campaign = item;
                List<Account> CurrentInfo = _accounts;
                ObservableCollection<AccountW_OneCampaign> AccountsW_OneCampaign = new ObservableCollection<AccountW_OneCampaign>();
                foreach (var account in CurrentInfo.Where(a => a.Pledges.Where(p => p.Campaign == campaign).Count() > 0).OrderBy(a => a.HLastName).ThenBy(a => a.HFirstName))
                {
                    AccountW_OneCampaign accountW_One = new AccountW_OneCampaign();
                    accountW_One.Name = account.HFirstName + " " + account.HLastName;
                    foreach (Address address in account.Addresses)
                    {
                        accountW_One.Address = address.Address1;
                        accountW_One.Location = address.Location;
                    }
                    if(account.Phones.FirstOrDefault() != null)
                        accountW_One.Phone = account.Phones.FirstOrDefault().Number;
                    //accountW_One.Address = account.defaultAddress.Address1;
                    account.SetAccountsRecords();

                    foreach (Pledge pledge in account.Pledges.Where(p => p.Campaign == campaign))
                    {
                        accountW_One.Amount += pledge.Amount;
                        accountW_One.Paid += decimal.Parse(pledge.PaidString.Remove(0, 1));
                        accountW_One.Balance += pledge.Balance;
                        accountW_One.Note = pledge.Note;
                    }
                    
                    accountW_One.TotalBalance = account.Balance;
                    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Phone, accountW_One.Address,accountW_One.Location ,accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //for (int i = 0; i < 100; i++)
                    //{
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);

                    //}
                }
            }
            //ReportParameterCollection reportParameters = new ReportParameterCollection();
            //reportParameters.Add(new ReportParameter("ReportHeader", reportHeader));
            //_reportViewer.LocalReport..SetParameters(reportParameters);
        }

        public static void CreateDocumentW_OneCampaign_En(string _report, List<Account> _accounts, List<string> _campaigns, ReportViewer _reportViewer, string reportHeader)
        {
            DataSet1 dataset = new DataSet1();

            foreach (string item in _campaigns)
            {
                ReportDataSource reportDataSource1 = new ReportDataSource();
                dataset.BeginInit();
                reportDataSource1.Name = "DataSet1";
                //Name of the report dataset in our .RDLC file
                //reportDataSource1.Value = dataset.OneCampaign;
                reportDataSource1.Value = dataset.Onecampaign;
                _reportViewer.LocalReport.DataSources.Clear();
                _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
                _reportViewer.LocalReport.ReportPath = "Reports/ReportOneCampaign.rdlc";
                _reportViewer.ZoomMode = ZoomMode.PageWidth;
                dataset.EndInit();

                string campaign = item;
                List<Account> CurrentInfo = _accounts;
                ObservableCollection<AccountW_OneCampaign> AccountsW_OneCampaign = new ObservableCollection<AccountW_OneCampaign>();
                foreach (var account in CurrentInfo.Where(a => a.Pledges.Where(p => p.Campaign == campaign).Count() > 0).OrderBy(a => a.LastName).ThenBy(a => a.FirstName))
                {
                    AccountW_OneCampaign accountW_One = new AccountW_OneCampaign();
                    accountW_One.Name = account.FirstName + " " + account.LastName;
                    foreach (Address address in account.Addresses)
                    {
                        accountW_One.Address = address.Address1;
                        accountW_One.Location = address.Location;
                    }
                    if (account.Phones.FirstOrDefault() != null)
                        accountW_One.Phone = account.Phones.FirstOrDefault().Number;
                    //accountW_One.Address = account.defaultAddress.Address1;
                    account.SetAccountsRecords();

                    foreach (Pledge pledge in account.Pledges.Where(p => p.Campaign == campaign))
                    {
                        accountW_One.Amount += pledge.Amount;
                        accountW_One.Paid += decimal.Parse(pledge.PaidString.Remove(0, 1));
                        accountW_One.Balance += pledge.Balance;
                        accountW_One.Note = pledge.Note;
                    }

                    accountW_One.TotalBalance = account.Balance;
                    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Phone, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //for (int i = 0; i < 100; i++)
                    //{
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);
                    //    dataset.Onecampaign.AddOnecampaignRow(accountW_One.Name, accountW_One.Address, accountW_One.Location, accountW_One.Amount, accountW_One.Paid, accountW_One.Balance, accountW_One.Note, accountW_One.TotalBalance);

                    //}
                }
            }
            //ReportParameterCollection reportParameters = new ReportParameterCollection();
            //reportParameters.Add(new ReportParameter("ReportHeader", reportHeader));
            //_reportViewer.LocalReport..SetParameters(reportParameters);
        }

        public static void CreateDocumentList(List<Account> _accounts, ReportViewer _reportViewer)
        {
            DataSet1 dataset = new DataSet1();

            ReportDataSource reportDataSource1 = new ReportDataSource();
            dataset.BeginInit();
            reportDataSource1.Name = "DataSetList";
            reportDataSource1.Value = dataset.MList;
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            _reportViewer.LocalReport.ReportPath = "Reports/List.rdlc";
            _reportViewer.ZoomMode = ZoomMode.PageWidth;
            dataset.EndInit();

            foreach (Account account in _accounts.OrderBy(a => a.HLastName).ThenBy(a => a.HFirstName))
            {
                MList mList = new MList();


                mList.H_Name = account.HFirstName + " " + account.HLastName;
                mList.E_Name = account.FirstName + " " + account.LastName;

                int num = 0;
                foreach (Phone phone in account.Phones.OrderBy(p => p.Type))
                {
                    num++;
                    if (phone.Type != null)
                    {
                        if ((phone.Type.Contains("Cell")) || (phone.Type == "" && num == 1))
                            mList.Cell = phone.Number;

                        if ((phone.Type.Contains("Home")) || (phone.Type == "" && num == 2))
                            mList.HomePhone = phone.Number;
                    }
                    else
                    {
                        if(num == 1)
                            mList.Cell = phone.Number;
                        if(num == 2)
                            mList.HomePhone = phone.Number;
                    }
                    
                }
                foreach (Address address in account.Addresses)
                {
                    mList.Address = address.Address1;
                    mList.Location = address.Location;
                }

                dataset.MList.AddMListRow(mList.H_Name, mList.E_Name, mList.Cell, mList.HomePhone, mList.Address, mList.Location);
            }
        }

        public static void CreateDocumentListW_Campaign(List<Account> _accounts,List<string> campaign, ReportViewer _reportViewer)
        {
            DataSet1 dataset = new DataSet1();

            ReportDataSource reportDataSource1 = new ReportDataSource();
            dataset.BeginInit();
            reportDataSource1.Name = "DataSetListW_Campaign";
            reportDataSource1.Value = dataset.MListWCampaign;
            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.DataSources.Add(reportDataSource1);
            _reportViewer.LocalReport.ReportPath = "Reports/ListW_Campaign.rdlc";
            _reportViewer.ZoomMode = ZoomMode.PageWidth;
            Margins margins = new Margins { Bottom = 50, Left = 50, Top = 50, Right = 50 };
            _reportViewer.SetPageSettings(new System.Drawing.Printing.PageSettings { Margins = margins });
            dataset.EndInit();

            foreach (Account account in _accounts.OrderBy(a => a.HLastName).ThenBy(a => a.HFirstName))
            {
                MList mList = new MList();


                mList.H_Name = account.HFirstName + " " + account.HLastName;
                mList.E_Name = account.FirstName + " " + account.LastName;

                int num = 0;
                foreach (Phone phone in account.Phones.OrderBy(p => p.Type))
                {
                    num++;
                    if (phone.Type != null)
                    {
                        if ((phone.Type.Contains("Cell")) || (phone.Type == "" && num == 1))
                            mList.Cell = phone.Number;

                        if ((phone.Type.Contains("Home")) || (phone.Type == "" && num == 2))
                            mList.HomePhone = phone.Number;
                    }
                    else
                    {
                        if (num == 1)
                            mList.Cell = phone.Number;
                        if (num == 2)
                            mList.HomePhone = phone.Number;
                    }

                }
                foreach (Address address in account.Addresses)
                {
                    mList.Address = address.Address1;
                    mList.Location = address.Location;
                }
                foreach (var Pledge in account.PledgesW_Balance.Where(p => p.Campaign == campaign.First()))
                {
                    mList.Amount = Pledge.Balance;
                }

                dataset.MListWCampaign.AddMListWCampaignRow(mList.H_Name, mList.E_Name, mList.Cell, mList.HomePhone, mList.Address, mList.Location, mList.Amount);
            }
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
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public decimal Amount { get; set; }
        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public string Note { get; set; }
        public decimal TotalBalance { get; set; }
    }

    public class MList
    {
        public string H_Name { get; set; }
        public string E_Name { get; set; }
        public string Cell { get; set; }
        public string HomePhone { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public decimal Amount { get; set; }
    }
}
