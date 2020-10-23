using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmallInfo.Data;
using System.Windows.Threading;

namespace SmallInfo
{
    /// <summary>
    /// Interaction logic for GeneralInfo.xaml
    /// </summary>
    public partial class GeneralInfo : Window
    {
        AccountManager accountManager { get; set; }
        public bool _ScrollViewer { get; set; }

        public GeneralInfo(SmallInfoEntities smallInfoEntities)
        {
            InitializeComponent();
            accountManager = new AccountManager(smallInfoEntities, AccountsTabControl);
            SetControlsData();
            SettingTabItem.DataContext = accountManager;
        }

        private void HebrewControl_LostFocus(object sender, RoutedEventArgs e)
        {
            setInputLanguageToEnglish();
        }

        private void HebrewControl_GotFocus(object sender, RoutedEventArgs e)
        {
            setInputLanguageToHebrew();
        }

        private void setInputLanguageToEnglish()
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        }

        private void setInputLanguageToHebrew()
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("he-IL");
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                accountManager.SaveInfo();
            });

            if (sender is ComboBox)
            {
                ComboBox comboBox = sender as ComboBox;
                if (comboBox.SelectedItem != null)
                {
                    accountManager.SetCurrentAccount(comboBox.SelectedItem as Account);

                    H_ComboBox.SelectedItem = comboBox.SelectedItem;
                    E_ComboBox.SelectedItem = comboBox.SelectedItem;
                    DonorComboBox.SelectedItem = comboBox.SelectedItem;
                    balanceTextBlock.Text = accountManager.CurrenAccount.Balance.ToString("C");
                }

            }

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Changed to ComboBox_DropDownClosed
        }

        private void newAccountButton_Click(object sender, RoutedEventArgs e)
        {
            accountManager.AddAccount();
            SetControlsData();
        }

        private void SetControlsData()
        {
            E_ComboBox.ItemsSource = accountManager.Accounts.OrderBy(a => a.LastName).ThenBy(a => a.FirstName);
            DonorComboBox.ItemsSource = accountManager.Accounts.OrderBy(a => a.AccountId);
            H_ComboBox.ItemsSource = accountManager.Accounts.OrderBy(a => a.HLastName).ThenBy(a => a.HFirstName);
        }

        //temp
        private void PhoneDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            //Phone phone = e.Row.DataContext as Phone;
            //accountManager.PreparePhone(phone);
        }

        private void AccountsTabControl_Unloaded(object sender, RoutedEventArgs e)
        {
            accountManager.SaveInfo();
        }

        private void PhoneDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Phone phone = e.NewItem as Phone;
            accountManager.PreparePhone(phone);
        }

        private void PledgeDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Pledge pledge = e.NewItem as Pledge;
            accountManager.PreparePledge(pledge);
        }

        private void SettingCampaignsDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Campaign campaign = e.NewItem as Campaign;
            accountManager.PrepareCampaign(campaign);
        }

        private void PledgeCampainComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.ItemsSource = accountManager.Campaigns;

            Binding binding = new Binding("Campaign");
            binding.Source = comboBox.DataContext;
            binding.Path = new PropertyPath("Campaign");
            comboBox.SetBinding(ComboBox.TextProperty, binding);
        }

        private void PledgeNoteComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.ItemsSource = accountManager.Notes;

            Binding binding = new Binding("Note");
            binding.Source = comboBox.DataContext;
            binding.Path = new PropertyPath("Note");
            comboBox.SetBinding(ComboBox.TextProperty, binding);
        }

        private void PledgeDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

        }

        private void PaymentDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Payment payment = e.NewItem as Payment;
            accountManager.PreparePayment(payment);
        }

        private void PaymentCampaignComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.ItemsSource = accountManager.Campaigns;

            Binding binding = new Binding();
            binding.Source = comboBox.DataContext;
            binding.Path = new PropertyPath("PaymentCampaign");
            comboBox.SetBinding(ComboBox.TextProperty, binding);
        }

        private void PaymentCampaignComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            Campaign campaign = comboBox.SelectedItem as Campaign;
            accountManager.SetPaymentCampaign(comboBox.DataContext as Payment, campaign.Campaign1);
            //accountManager.SetPayment(comboBox.SelectedItem as Pledge, comboBox.DataContext as Payment);
            //comboBox.IsEnabled = false;
        }

        private void PaymentDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            //accountManager.DeletePayment(button.DataContext as Payment);
        }

        private void PledgeDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                DataGrid dataGrid = sender as DataGrid;
                accountManager.DeletePledge(dataGrid, e);
            }

        }

        private void PaymentDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            if (e.Key == Key.Delete)
            {
                accountManager.DeletePayment(dataGrid, e);
            }
            if (e.Key == Key.Enter)
            {
                if (e.OriginalSource is TextBox)
                {
                    TextBox textBox = e.OriginalSource as TextBox;
                    Payment payment;
                    if (textBox.DataContext is Payment)
                    {
                        payment = textBox.DataContext as Payment;
                        decimal amount;
                        if (decimal.TryParse(textBox.Text, out amount))
                            payment.Amount = amount;
                        dataGrid.CommitEdit(DataGridEditingUnit.Row, true);
                        //dataGrid.MoveFocus()
                    }

                    else
                    {
                        MessageBox.Show("First enter the amount");
                        return;
                    }
                    ApplyPaymentWindow applyPaymentWindow = new ApplyPaymentWindow(accountManager, payment);
                    applyPaymentWindow.Show();
                }

            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Payment payment;
            if (button.DataContext is Payment)
                payment = button.DataContext as Payment;
            else
            {
                MessageBox.Show("First enter the amount");
                return;
            }
            ApplyPaymentWindow applyPaymentWindow = new ApplyPaymentWindow(accountManager, payment);
            applyPaymentWindow.Show();
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            accountManager.SaveInfo();
            if (accountManager.CurrenAccount == null)
                return;
            accountManager.SetCurrentAccount(accountManager.CurrenAccount);
            balanceTextBlock.Text = accountManager.CurrenAccount.Balance.ToString("C");
        }

        private void CampaignIsDefaultCheckBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            if (checkBox.DataContext != null)
            {
                Campaign campaign = checkBox.DataContext as Campaign;
                if (checkBox.IsChecked == true)
                    accountManager.SetDefaultCampaign(campaign);
            }

        }

        private void NotesDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Note note = e.NewItem as Note;
            accountManager.PrepareNote(note);
        }

        private void LocationDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            Location location = e.NewItem as Location;
            accountManager.PrepareLocation(location);
        }

        private void AccountLocationComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.ItemsSource = accountManager.Locations;

            Binding binding = new Binding();
            binding.Source = comboBox.DataContext;
            binding.Path = new PropertyPath("Location");
            comboBox.SetBinding(ComboBox.TextProperty, binding);
        }

        private void AddressDataGrid_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
        {
            //Address address = e.NewItem as Address;
            //address.AptNum = "try";
        }

        private void AddressDataGrid_Selected(object sender, RoutedEventArgs e)
        {

            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                DataGrid dataGrid = (DataGrid)sender;
                //if (dataGrid.CurrentItem is Address)
                //{
                //    Address address = (Address)dataGrid.CurrentCell.Item;
                //    if ((string)dataGrid.CurrentCell.Column.Header == "Location" && address.Location != null)
                //dataGrid.BeginEdit(e);
                //}

            }
        }

        private void LocationComboBox_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            var RowData = comboBox.DataContext.ToString();
            Location location = comboBox.SelectedItem as Location;
            if (RowData == "{DataGrid.NewItemPlaceholder}" && location != null)
            {
                accountManager.CurrenAccount.Addresses.Add(new Address() { Location = location.Location1 });
                //comboBox.EndInit();
                comboBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                comboBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
                comboBox.Focus();

            }
        }

        private void H_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ComboBox comboBox = sender as ComboBox;
            //ComboBoxItem comboBoxItem = comboBox.ItemContainerGenerator.ContainerFromItem(comboBox.SelectedItem) as ComboBoxItem;
            //if (comboBoxItem != null)
            //    comboBoxItem.BringIntoView();

        }

        private void H_ComboBox_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            RequestBringIntoViewEventArgs eventArgs = e as RequestBringIntoViewEventArgs;
            Console.Write("Bottom :");
            Console.WriteLine(eventArgs.TargetRect.Bottom.ToString());
            Console.Write("BottomLeft :");
            Console.WriteLine(eventArgs.TargetRect.BottomLeft.ToString());
            Console.Write("BottomRight :");
            Console.WriteLine(eventArgs.TargetRect.BottomRight.ToString());
            Console.Write("Hight :");
            Console.WriteLine(eventArgs.TargetRect.Height.ToString());
            Console.Write("Left :");
            Console.WriteLine(eventArgs.TargetRect.Left.ToString());
            Console.Write("Location :");
            Console.WriteLine(eventArgs.TargetRect.Location.ToString());
            Console.Write("Right :");
            Console.WriteLine(eventArgs.TargetRect.Right.ToString());
            Console.Write("Size :");
            Console.WriteLine(eventArgs.TargetRect.Size.ToString());
            Console.Write("Top :");
            Console.WriteLine(eventArgs.TargetRect.Top.ToString());
            Console.Write("TopLeft :");
            Console.WriteLine(eventArgs.TargetRect.TopLeft.ToString());
            Console.Write("TopRight :");
            Console.WriteLine(eventArgs.TargetRect.TopRight.ToString());
            Console.Write("Width :");
            Console.WriteLine(eventArgs.TargetRect.Width.ToString());
            Console.Write("X :");
            Console.WriteLine(eventArgs.TargetRect.X.ToString());
            Console.Write("Y :");
            Console.WriteLine(eventArgs.TargetRect.Y.ToString());
            Console.Write("Finish :");
            Console.WriteLine(); Console.WriteLine(); Console.WriteLine();


        }

        private void H_ComboBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_ScrollViewer)
            {
                ScrollViewer scrollViewer = e.OriginalSource as ScrollViewer;
                scrollViewer.PageDown();
                _ScrollViewer = false;
            }
            
        }

        private void H_ComboBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            _ScrollViewer = true;
        }

        private void H_ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.te)
            //{

            //}
        }
    }
}
