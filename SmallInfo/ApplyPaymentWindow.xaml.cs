using SmallInfo.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.ComponentModel;

namespace SmallInfo
{
    /// <summary>
    /// Interaction logic for ApplyPaymentWindow.xaml
    /// </summary>
    public partial class ApplyPaymentWindow : Window
    {
        AccountManager accountManager;
        Account CurrentAccount { get; set; }
        public Payment payment { get; set; }
        public decimal TotalAmount { get; private set ; }
        public decimal AppliedAmount{ get; private set; }
        public decimal RemainingAmount { get; private set; }
        public int ApplyIds { get; set; }
        public ObservableCollection<Pledge> ApplyCollection { get; set; }
        bool allCampaigns = false;

        public ApplyPaymentWindow(AccountManager _accountManager, Payment _payment)
        {
            accountManager = _accountManager;
            CurrentAccount = _accountManager.CurrenAccount;
            payment = _payment;
            InitializeComponent();
            ApplyDataGrid.ItemsSource = GetApplyCollection();
            ApplyGrid.DataContext = CurrentAccount;
            AppliedToDataGrid.DataContext = payment;
            setAmounts();
            OnPropertyChanged("ApplyDataGrid.ItemsSource");
        }

        private ObservableCollection<Pledge> GetApplyCollection()
        {
            ApplyCollection = new ObservableCollection<Pledge>();
            if (allCampaigns)
            {
                ApplyCollection = CurrentAccount.PledgesW_Balance;
                return ApplyCollection;
            }
            foreach (Pledge pledge in CurrentAccount.PledgesW_Balance.Where(p => p.Campaign == payment.PaymentCampaign))
                ApplyCollection.Add(pledge);
            return ApplyCollection;
        }

        void setAmounts()
        {
            TotalAmount = payment.Amount;
            AppliedAmount = 0;
            RemainingAmount = payment.Amount;
            foreach (Apply apply in payment.Applies)
            {
                AppliedAmount += apply.ApplyAmount;
                RemainingAmount -= apply.ApplyAmount;
            }
            TotalAmountTB.Text = TotalAmount.ToString("C");
            AppliedAmountTB.Text = AppliedAmount.ToString("C");
            RemainingAmountTB.Text = RemainingAmount.ToString("C");
            ApplyDataGrid.ItemsSource = GetApplyCollection();
        }

        private void FinishedButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Pledge pledge = button.DataContext as Pledge;
            if (RemainingAmount <= 0)
            {
                MessageBox.Show("The Remaining Amount is 0");
                return;
            }

            Apply apply = new Apply();
            apply.pledge = pledge;
            apply.PledgeId = pledge.PledgeId;
            apply.Payment = payment;
            payment.Applies.Add(apply);

            if (RemainingAmount >= pledge.Balance)
            {
                apply.ApplyAmount = pledge.Balance;
                RemainingAmount =- pledge.Balance;
            }
            else
            {
                apply.ApplyAmount = RemainingAmount;
                RemainingAmount = 0;
            }
            CurrentAccount.SetAccountsRecords();
            setAmounts();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Apply apply = button.DataContext as Apply;

            //apply.Payment.Applies.Remove(apply);
            accountManager.smallInfoEntities.Applies.Remove(apply);
            //payment.Applies.Remove(apply);
            RemainingAmount += apply.ApplyAmount;
            CurrentAccount.SetAccountsRecords();
            setAmounts();
        }

        private void createChargeButton_Click(object sender, RoutedEventArgs e)
        {
            if (RemainingAmount <= 0)
            {
                MessageBox.Show("The Remaining Amount is 0");
                return;
            }

            if(CurrentAccount.PledgesW_Balance.Count == 0)
            {
                CreateA_Charge();
                return;

            }

            MessageBoxResult create = MessageBox.Show("Do you want to create a new Charge with the amount of " + RemainingAmount.ToString("C"), "Create New Charge",MessageBoxButton.YesNo);
            if (create == MessageBoxResult.Yes)
            {
                CreateA_Charge();
            }
        }

        private void CreateA_Charge()
        {
            Pledge pledge = new Pledge();
            accountManager.PreparePledge(pledge);
            pledge.Amount = RemainingAmount;
            pledge.Detail = true;
            CurrentAccount.Pledges.Add(pledge);

            Apply apply = new Apply();
            apply.pledge = pledge;
            apply.PledgeId = pledge.PledgeId;
            apply.Payment = payment;
            payment.Applies.Add(apply);

            apply.ApplyAmount = pledge.Amount;
            RemainingAmount = -pledge.Amount;
            CurrentAccount.SetAccountsRecords();
            setAmounts();
            Close();
        }

        private void autoApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (RemainingAmount <= 0)
            {
                MessageBox.Show("The Remaining Amount is 0");
                return;
            }
            List<Pledge> pledges = ApplyCollection.OrderBy(a => a.DateEnterd).ToList(); //CurrentAccount.PledgesW_Balance.Where(p => p.Campaign == accountManager.DefaultCampaign.Campaign1).OrderBy(p => p.DateEnterd).ToList();
            foreach (Pledge pledge in pledges)
            {
                Apply apply = new Apply();
                apply.pledge = pledge;
                apply.PledgeId = pledge.PledgeId;
                apply.Payment = payment;
                payment.Applies.Add(apply);

                if (RemainingAmount >= pledge.Balance)
                {
                    apply.ApplyAmount = pledge.Balance;
                    RemainingAmount =- pledge.Balance;
                }
                else
                {
                    apply.ApplyAmount = RemainingAmount;
                    RemainingAmount = 0;
                }
                CurrentAccount.SetAccountsRecords();
                setAmounts();
                if (RemainingAmount <= 0)
                {
                    this.Close(); return;
                }
                
            }

        }

        private void AllCampaignsButton_Click(object sender, RoutedEventArgs e)
        {
            if(allCampaigns == true)
            {
                allCampaigns = false;
                ApplyDataGrid.ItemsSource = GetApplyCollection();
            }
            if (!allCampaigns)
            {
                ApplyDataGrid.ItemsSource = CurrentAccount.PledgesW_Balance;
                allCampaigns = true;
            }
            
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
