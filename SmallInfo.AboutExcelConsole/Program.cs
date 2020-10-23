using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmallInfo.Data;
using System.Windows;

namespace SmallInfo.AboutExcelConsole
{
    class Program
    {
        
        static void Main(string args, SmallInfoEntities _smallInfoEntities)
        {
            TheExcelClass theExcelClass = new TheExcelClass(_smallInfoEntities);
            switch (args)
            {
                case "One":
                    theExcelClass.NumOne();
                    break;
                case "Two":
                    theExcelClass.NumOne();
                    break;
                case "Three":
                    theExcelClass.NumOne();
                    break;
                case "Four":
                    theExcelClass.NumOne();
                    break;
                default:
                    break;
            }
        }
    }
}
