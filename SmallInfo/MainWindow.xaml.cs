using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SmallInfo.Data;

namespace SmallInfo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<SmallInfo.Data.Account> Accounts { get; set; }
        SmallInfoEntities smallInfoEntities;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                smallInfoEntities = new SmallInfoEntities();
                
                Accounts = smallInfoEntities.Accounts.ToList();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.InnerException,"DataBase ERORR!!!");
                //this.Close();
                Application.Current.Shutdown();
            }
        }
        GeneralInfo accountWindow;
        Report report;

        private void AccountButton_Click(object sender, RoutedEventArgs e)
        {
    
            accountWindow = new GeneralInfo(smallInfoEntities);
            accountWindow.Show();
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            report = new Report(smallInfoEntities);
            report.Show();
        }

    }
}
