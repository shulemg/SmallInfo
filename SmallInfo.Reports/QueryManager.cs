using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallInfo.Reports
{
    public class QueryManager
    {

        string MainQueryString = "select * from Accounts a inner join Address d on a.AccountId = d.AccountId";
        public ObservableCollection<QueryPieceStrings> MainQueryPieces = new ObservableCollection<QueryPieceStrings>();
        bool firstQueryPiece;

        public ObservableCollection<QueryPiece> GetQueryCollection(queryType type)
        {
            ObservableCollection<QueryPiece> queryPieces = new ObservableCollection<QueryPiece>();
            foreach (QueryPiece query in QueryPieces)
            {
                if (query.Type == type)
                    queryPieces.Add(query);
            }

            return queryPieces;
        }

        public List<QueryPiece> QueryPieces = new List<QueryPiece>()
        {
            new QueryPiece(){Name="En First Name", Type=queryType.generalInfo,Query_Start="select Distinct firstName from Accounts", QueryString="a.firstName"},
            new QueryPiece(){Name="En Last Name", Type=queryType.generalInfo,Query_Start="select Distinct lastName from Accounts", QueryString="a.lastName"},
            new QueryPiece(){Name="ערשטע נאמען",Type=queryType.generalInfo,Query_Start="select Distinct HFirstName from Accounts", QueryString="a.HFirstName"},
            new QueryPiece(){Name="לעצטע נאמען",Type=queryType.generalInfo,Query_Start="select Distinct HFirstName from Accounts", QueryString="a.HFirstName"},
            new QueryPiece(){Name="Location",Type=queryType.generalInfo,Query_Start="select DISTINCT Location from Accounts a inner join Address ad on a.AccountId = ad.AccountId where Location != ''",QueryString="d.location"},
            new QueryPiece(){Name="Location2",Type=queryType.generalInfo,Query_Start="select Location from Locations",QueryString="d.location"},

        };

        public void AddQuery(string query,string dispalyQuery)
        {
            if (!firstQueryPiece)
            {
                MainQueryPieces.Add(new QueryPieceStrings() { MainQueryPiece= " where " + query , MyQuery = dispalyQuery });
                //MainQueryString += " where " + query;
                firstQueryPiece = true;
            }
            else
                MainQueryPieces.Add(new QueryPieceStrings() { MainQueryPiece = " and " + query, MyQuery = dispalyQuery });
            //MainQueryString += " and " + query;
        }

        public string GetQuery()
        {
            foreach (QueryPieceStrings mainQueryPiece in MainQueryPieces)
                MainQueryString += mainQueryPiece.MainQueryPiece;
            return MainQueryString;
        }
    }

    public class QueryPiece
    {
        public string Name { get; set; }
        public string Query_Start { get; set; }
        public string QueryString { get; set; }
        public queryType Type { get; set; }
    }
    public class QueryPieceStrings
    {
        public string MainQueryPiece { get; set; }
        public string MyQuery { get; set; }
    }

    public enum queryType
    {
        generalInfo,
        accountingInfo
    }
}
