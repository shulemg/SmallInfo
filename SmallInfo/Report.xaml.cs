using System;
using System.Collections.Generic;
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
using SmallInfo.Data;
using SmallInfo.Reports;
using SmallInfo.AboutExcel;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace SmallInfo
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : Window
    {

        SmallInfoEntities smallInfoEntities;
        QueryManager queryManager;
        QueryPiece queryPeace;
        TheExcelClass theExcelClass;
        public ObservableCollection<string> Reports { get; set; }
        public string SelectedReport { get; set; }
        public ObservableCollection<Campaign> Campaigns { get; set; }
        public List<string> SelectedCampaigns = new List<string>();
        List<Account> currentAccounts;
        public string ReportHeader { get; set; }

        public Report(SmallInfoEntities _smallInfoEntities)
        {
            InitializeComponent();
            smallInfoEntities = _smallInfoEntities;
            queryManager = new QueryManager();
            theExcelClass = new TheExcelClass(smallInfoEntities);
            ReportListComboBox.ItemsSource = DocumentCreater.Reports();
            Campaigns = new ObservableCollection<Campaign>(smallInfoEntities.Campaigns.ToList());
            ReportCampaignListBox.ItemsSource = Campaigns;
            currentAccounts = smallInfoEntities.Accounts.ToList();
            QueryListBox.ItemsSource = queryManager.MainQueryPieces;
        }

        private void ExcelButton_Click1(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                theExcelClass.NumOne();
            });
        }

        private void ExcelButton_Click2(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                theExcelClass.NumTwo();
            });

        }

        private void ExcelButton_Click3(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                theExcelClass.NumTree();
            });
        }

        private void ExcelButton_Click4(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                theExcelClass.NumFour();
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource accountViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("accountViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // accountViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource addressViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("addressViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // addressViewSource.Source = [generic data source]
        }
        private bool _isReportViewerLoaded;
        

        private void ReportListComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            SelectedReport = comboBox.SelectedValue.ToString();
        }

        private void ViewReportButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedCampaigns.Clear();
            foreach (var item in ReportCampaignListBox.SelectedItems)
            {
                Campaign campaign = item as Campaign;
                SelectedCampaigns.Add(campaign.Campaign1);
            }
            //fill data into WpfApplication4DataSet
            _reportViewer.Clear();
            currentAccounts = smallInfoEntities.Accounts.SqlQuery(queryManager.GetQuery()).ToList<Account>();
            //currentAccounts = smallInfoEntities.Database.SqlQuery<Account>(queryManager.GetQuery()).ToList<Account>();
            DocumentCreater.CreateReport(SelectedReport, currentAccounts, SelectedCampaigns, _reportViewer,ReportHeader);
                _reportViewer.RefreshReport();
                _isReportViewerLoaded = true;

        }

        private void GeneralInfoListBox_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            listBox.ItemsSource = queryManager.GetQueryCollection(queryType.generalInfo);
        }

        private void GeneralInfoListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sqlGenInfoTextBlock.Text = "";
            ListBox listBox = sender as ListBox;
            queryPeace = listBox.SelectedItem as QueryPiece;

            //List<Account> accounts = smallInfoEntities.Accounts.SqlQuery(queryPeace.Query_Start).ToList<Account>();
            var abc = smallInfoEntities.Database.SqlQuery<string>(queryPeace.Query_Start);

            queryItemsListBox.ItemsSource = abc.ToList<string>();


            ObservableCollection<string> queryItems = new ObservableCollection<string>()
            {
                "shulem","meir","abraham","chaim","jacob","hershy","yitzchok","kalmen"
            };
            //queryItemsListBox.ItemsSource = queryItems;
        }

        private void queryTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            sqlGenInfoTextBlock.Text = textBlock.Text;
        }

        private void queryItemsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if(listBox.SelectedItem is string)
                sqlGenInfoTextBlock.Text = listBox.SelectedItem.ToString();
        }

        private void Include_Click(object sender, RoutedEventArgs e)
        {
            if (sqlGenInfoTextBlock.Text == "")
            {
                MessageBox.Show("");
                return;
            }
            queryManager.AddQuery(queryPeace.QueryString + " = N'" + sqlGenInfoTextBlock.Text + "'", "Include " + queryPeace.QueryString.Remove(0,2) + " " + sqlGenInfoTextBlock.Text);
        }

        private void Exclude_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IncludeAmount_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExcludeAmount_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

