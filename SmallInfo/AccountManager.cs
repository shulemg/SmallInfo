using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SmallInfo.Data;

namespace SmallInfo
{
    public class AccountManager : INotifyPropertyChanged
    {
        public ObservableCollection<Account> Accounts { get; set; }
        public SmallInfoEntities smallInfoEntities;
        //public List<Account> Accounts { get { return accounts; } set { accounts = value; OnPropertyChanged("Accounts");  } }
        public Account CurrenAccount { get; set; }
        public ObservableCollection<Note> Notes { get; set; }
        public ObservableCollection<Campaign> Campaigns { get; set; }
        public ObservableCollection<Location> Locations { get; set; }
        public DateTime DefaultDate { get; set; }
        public Campaign DefaultCampaign { get; set; }

        public ObservableCollection<string> PhoneTypes { get; set; }
        public ObservableCollection<string> PaymentTypes { get; set; }

        List<Phone> phones;
        List<Pledge> pledges;
        List<int> paymentsID;
        FrameworkElement frameworkElement;
        //temp
        List<Account> accnt;

        public AccountManager(SmallInfoEntities _smallInfoEntities, FrameworkElement _frameworkElement)
        {
            smallInfoEntities = _smallInfoEntities;
            //TMP
            accnt = smallInfoEntities.Accounts.SqlQuery("SELECT * FROM Accounts where (AccountId < 2500)").ToList<Account>();
            Accounts = new ObservableCollection<Account>(smallInfoEntities.Accounts);
            Campaigns = new ObservableCollection<Campaign>();
            Notes = new ObservableCollection<Note>();
            Locations = new ObservableCollection<Location>();
            paymentsID = new List<int>();
            foreach (Location location in smallInfoEntities.Locations)
                Locations.Add(location);
            
            foreach (Note note in smallInfoEntities.Notes)
            {
                Notes.Add(note);
            }
            foreach (Campaign campaign in smallInfoEntities.Campaigns)
            {
                Campaigns.Add(campaign);
                if (campaign.IsDefault == true)
                    DefaultCampaign = campaign;
            }
            //foreach (Account account in smallInfoEntities.Accounts)
            //{             
            //    Accounts.Add(account);
            //}
            frameworkElement = _frameworkElement;
            //temp
            
            phones = smallInfoEntities.Phones.ToList();
            pledges = smallInfoEntities.Pledges.ToList();
            List<Address> adr = smallInfoEntities.Addresses.ToList();
            foreach (Payment payment in smallInfoEntities.Payments)
                paymentsID.Add(payment.PaymentId);

            DefaultDate = DateTime.Now;

            List<Apply> applies = smallInfoEntities.Applies.ToList();
        }

        public void SetCurrentAccount(Account _CurrentAccount)
        {
            //if (_CurrentAccount == null)
            //    return;
            CurrenAccount = _CurrentAccount;
            CurrenAccount.SetAccountsRecords();
            frameworkElement.DataContext = CurrenAccount;
        }

        public void AddAccount()
        {
            Account newAccount = new Account();
            if (Accounts.Count == 0)
                newAccount.AccountId = 1000;
            else
                newAccount.AccountId = Accounts.OrderBy(a => a.AccountId).Last().AccountId + 1;
            Accounts.Add(newAccount);
            smallInfoEntities.Accounts.Add(newAccount);
            SetCurrentAccount(newAccount);
        }

        public void SaveInfo()
        {
            try
            {
                smallInfoEntities.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Can't save the information");
            }
        }

        public void PreparePhone(Phone phone)
        {
            if (phones.Count == 0)
                phone.PhoneId = 1000;
            else
                phone.PhoneId = phones.OrderBy(p => p.PhoneId).Last().PhoneId + 1;
            phones.Add(phone);
        }

        internal void PreparePledge(Pledge pledge)
        {
            if (pledges.Count == 0)
                pledge.PledgeId = 1000;
            else
                pledge.PledgeId = pledges.OrderBy(p => p.PledgeId).Last().PledgeId + 1;
            if (DefaultCampaign != null)
                pledge.Campaign = DefaultCampaign.Campaign1;
            else
                pledge.Campaign = Campaigns.Last().Campaign1;
            pledge.DateEnterd = DefaultDate;
            pledges.Add(pledge);
        }

        internal void PrepareCampaign(Campaign campaign)
        {
            if (Campaigns.Count == 0)
                campaign.CampaignId = 1000;
            else
                campaign.CampaignId = Campaigns.OrderBy(c => c.CampaignId).Last().CampaignId + 1;
            //Campaigns.Add(campaign);
            SetDefaultCampaign(campaign);
            smallInfoEntities.Campaigns.Add(campaign);
        }

        internal void PrepareNote(Note note)
        {
            smallInfoEntities.Notes.Add(note);
        }

        internal void PrepareLocation(Location location)
        {
            smallInfoEntities.Locations.Add(location);
        }

        internal void PreparePayment(Payment payment)
        {
            if (paymentsID.Count == 0)
                payment.PaymentId = 1000;
            else
                payment.PaymentId = paymentsID.Last() + 1;
            paymentsID.Add(payment.PaymentId);
            if (DefaultCampaign != null)
                payment.PaymentCampaign = DefaultCampaign.Campaign1;
            else
                payment.PaymentCampaign = Campaigns.Last().Campaign1;
            payment.Date = DefaultDate;
        }

        public void SetPaymentCampaign(Payment _payment,string _campaign)
        {
            if(_payment != null)
                _payment.PaymentCampaign = _campaign;
        }

        public void SetPayment(Pledge pledge, Payment _payment)
        {
            //pledge.Payments.Add(_payment);
            try
            {
                _payment.PledgeId = pledge.PledgeId;
                _payment.pledge = pledge;
            }
            catch (Exception)
            {

                return;
            }
            
        }
        public void DeletePledge(DataGrid _dataGrid, KeyEventArgs e)
        {
            Pledge pledge = _dataGrid.SelectedItem as Pledge;
            if (e.Device.Target.GetType().Name == "DataGridCell" && pledge != null)
            {
                MessageBoxResult result = MessageBox.Show("About to delete pledge " + pledge.Campaign + "\n with the amount of " + pledge.Amount.ToString("C") + "\n and the payments of the pledge", "DELETE", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

                e.Handled = (result == MessageBoxResult.No);
                if (result != MessageBoxResult.No)
                {
                    smallInfoEntities.Pledges.Remove(pledge);
                    smallInfoEntities.Payments.RemoveRange(smallInfoEntities.Payments.Where(p => p.PledgeId == pledge.PledgeId));
                }
            }
        }

        public void DeletePayment(DataGrid _dataGrid, KeyEventArgs e)
        {
            Payment payment = _dataGrid.SelectedItem as Payment;
            if (e.Device.Target.GetType().Name == "DataGridCell" && payment != null)
            {
                MessageBoxResult result = MessageBox.Show("About to delete payment #" + payment.PaymentId + "\n with the amount of " + payment.Amount.ToString("C"), "DELETE PAYMENT", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                e.Handled = (result == MessageBoxResult.No);
                if (result != MessageBoxResult.No)
                    smallInfoEntities.Applies.RemoveRange(smallInfoEntities.Applies.Where(a => a.PaymentId == payment.PaymentId));
                    smallInfoEntities.Payments.Remove(payment);
            }
        }

        public void SetDefaultCampaign(Campaign _campaign)
        {
            foreach (var campaign in Campaigns)
            {
                campaign.IsDefault = false;
            }
            _campaign.IsDefault = true;
            DefaultCampaign = _campaign;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChangedEvent = PropertyChanged;
            if (propertyChangedEvent != null)
                propertyChangedEvent(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
