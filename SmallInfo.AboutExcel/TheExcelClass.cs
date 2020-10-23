using SmallInfo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace SmallInfo.AboutExcel
{
    public class TheExcelClass
    {
        SmallInfoEntities smallInfoEntities;

        public TheExcelClass(SmallInfoEntities _smallInfoEntities)
        {
            smallInfoEntities = _smallInfoEntities;
        }

        public void NumOne()
        {
            Excel._Application xlApll;
            Excel._Workbook xlWoorkbook;
            Excel._Worksheet xlWoorksheet;

            int accountNum = 1;
            try
            {
                xlApll = new Excel.Application();
                xlApll.Visible = true;

                xlWoorkbook = (Excel._Workbook)xlApll.Workbooks.Add(Missing.Value);
                xlWoorksheet = (Excel._Worksheet)xlApll.ActiveSheet;

                xlWoorksheet.Cells[1, 5] = "First Name";
                xlWoorksheet.Cells[1, 4] = "Last Name";
                xlWoorksheet.Cells[1, 3] = "Total Pledges";
                xlWoorksheet.Cells[1, 2] = "Total Payments";
                xlWoorksheet.Cells[1, 1] = "Balance";

                foreach (Account account in smallInfoEntities.Accounts.OrderBy(a => a.HLastName).ThenBy(a => a.HFirstName))
                {
                    decimal pledges = 0;
                    decimal payments = 0;
                    decimal balance = 0;
                    foreach (Pledge pledge in account.Pledges)
                        pledges += pledge.Amount;
                    foreach (Payment payment in account.Payments)
                        payments += payment.Amount;
                    balance = pledges - payments;
                    if (balance > 0)
                    {
                        accountNum++;

                        xlWoorksheet.Cells[accountNum, 5] = account.HFirstName;
                        xlWoorksheet.Cells[accountNum, 4] = account.HLastName;
                        xlWoorksheet.Cells[accountNum, 3] = pledges.ToString("C");
                        xlWoorksheet.Cells[accountNum, 2] = payments.ToString("C");
                        xlWoorksheet.Cells[accountNum, 1] = balance.ToString("C");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void NumTwo()
        {
            Excel._Application xlApll;
            Excel._Workbook xlWoorkbook;
            Excel._Worksheet xlWoorksheet;

            int accountNum = 1;
            try
            {
                xlApll = new Excel.Application();
                xlApll.Visible = true;

                xlWoorkbook = (Excel._Workbook)xlApll.Workbooks.Add(Missing.Value);
                xlWoorksheet = (Excel._Worksheet)xlApll.ActiveSheet;

                xlWoorksheet.Cells[1, 7] = "First Name";
                xlWoorksheet.Cells[1, 6] = "Last Name";
                xlWoorksheet.Cells[1, 5] = "Address";
                xlWoorksheet.Cells[1, 4] = "Home Phone";
                xlWoorksheet.Cells[1, 3] = "Total Pledges";
                xlWoorksheet.Cells[1, 2] = "Total Payments";
                xlWoorksheet.Cells[1, 1] = "Balance";

                foreach (Account account in smallInfoEntities.Accounts)
                {
                    accountNum++;
                    decimal pledges = 0;
                    decimal payments = 0;
                    decimal balance = 0;
                    xlWoorksheet.Cells[accountNum, 7] = account.HFirstName;
                    xlWoorksheet.Cells[accountNum, 6] = account.HLastName;
                    string address = null;
                    foreach (Address _address in account.Addresses)
                        address = _address.Address1;
                    xlWoorksheet.Cells[accountNum, 5] = address;
                    foreach (Phone phone in account.Phones)
                        if(phone.Type == "Home      ")
                            xlWoorksheet.Cells[accountNum, 4] = phone.Number;
                    foreach (Pledge pledge in account.Pledges)
                        pledges += pledge.Amount;
                    foreach (Payment payment in account.Payments)
                        payments += payment.Amount;
                    xlWoorksheet.Cells[accountNum, 3] = pledges.ToString("C");
                    xlWoorksheet.Cells[accountNum, 2] = payments.ToString("C");
                    balance = pledges - payments;
                    xlWoorksheet.Cells[accountNum, 1] = balance.ToString("C");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        public void NumTree()
        {
            Excel._Application xlApll;
            Excel._Workbook xlWoorkbook;
            Excel._Worksheet xlWoorksheet;

            int accountNum = 1;
            try
            {
                xlApll = new Excel.Application();
                xlApll.Visible = true;

                xlWoorkbook = (Excel._Workbook)xlApll.Workbooks.Add(Missing.Value);
                xlWoorksheet = (Excel._Worksheet)xlApll.ActiveSheet;               
                xlWoorksheet.Cells[1, 5] = "ערשטע נאמען";
                xlWoorksheet.Cells[1, 4] = "לעצטע נאמען";
                xlWoorksheet.Cells[1, 3] = "Total Pledges";
                xlWoorksheet.Cells[1, 2] = "Total Payments";
                xlWoorksheet.Cells[1, 1] = "Balance";

                foreach (Account account in smallInfoEntities.Accounts)
                {
                    accountNum++;
                    decimal pledges = 0;
                    decimal payments = 0;
                    decimal balance = 0;
                    xlWoorksheet.Cells[accountNum, 5] = account.HFirstName;
                    xlWoorksheet.Cells[accountNum, 4] = account.HLastName;
                    foreach (Pledge pledge in account.Pledges)
                        pledges += pledge.Amount;
                    foreach (Payment payment in account.Payments)
                        payments += payment.Amount;
                    xlWoorksheet.Cells[accountNum, 3] = pledges.ToString("C");
                    xlWoorksheet.Cells[accountNum, 2] = payments.ToString("C");
                    balance = pledges - payments;
                    xlWoorksheet.Cells[accountNum, 1] = balance.ToString("C");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void NumFour()
        {
            Excel._Application xlApll;
            Excel._Workbook xlWoorkbook;
            Excel._Worksheet xlWoorksheet;

            int accountNum = 1;
            try
            {
                xlApll = new Excel.Application();
                xlApll.Visible = true;

                xlWoorkbook = (Excel._Workbook)xlApll.Workbooks.Add(Missing.Value);
                xlWoorksheet = (Excel._Worksheet)xlApll.ActiveSheet;

                xlWoorksheet.Cells[1, 5] = "First Name";
                xlWoorksheet.Cells[1, 4] = "Last Name";
                xlWoorksheet.Cells[1, 3] = "Total Pledges";
                xlWoorksheet.Cells[1, 2] = "Total Payments";
                xlWoorksheet.Cells[1, 1] = "Balance";

                foreach (Account account in smallInfoEntities.Accounts.OrderBy(a => a.HLastName).ThenBy(a => a.HFirstName))
                {
                    decimal pledges = 0;
                    decimal payments = 0;
                    decimal balance = 0;
                    foreach (Pledge pledge in account.Pledges)
                        pledges += pledge.Amount;
                    foreach (Payment payment in account.Payments)
                        payments += payment.Amount;
                    balance = pledges - payments;
                    if (pledges > 0)
                    {
                        accountNum++;

                        xlWoorksheet.Cells[accountNum, 5] = account.HFirstName;
                        xlWoorksheet.Cells[accountNum, 4] = account.HLastName;
                        xlWoorksheet.Cells[accountNum, 3] = pledges.ToString("C");
                        xlWoorksheet.Cells[accountNum, 2] = payments.ToString("C");
                        xlWoorksheet.Cells[accountNum, 1] = balance.ToString("C");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
