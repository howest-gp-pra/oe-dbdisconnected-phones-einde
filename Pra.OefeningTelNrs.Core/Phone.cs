using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Pra.OefeningTelNrs.Core
{
    public class Phone
    {

        public static DataSet CreateDataSet()
        {
            DataSet dsPhone = new DataSet();

            DataTable dt = new DataTable();
            dt.TableName = "telefoonnummers";
            dsPhone.Tables.Add(dt);

            DataColumn dc;
            dc = new DataColumn();
            dc.ColumnName = "Nummer";
            dc.DataType = typeof(string);
            dc.MaxLength = 50;
            dc.AllowDBNull = false;
            dc.Unique = true;
            dt.Columns.Add(dc);
            dt.PrimaryKey = new DataColumn[] { dc };

            dc = new DataColumn();
            dc.ColumnName = "Mobile";
            dc.DataType = typeof(byte); // 0 = geen mobiel, 1 = wel mobiel
            dc.DefaultValue = 1;
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "Naam";
            dc.DataType = typeof(string);
            dc.MaxLength = 100;
            dc.AllowDBNull = false;
            dt.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "Voornaam";
            dc.DataType = typeof(string);
            dc.MaxLength = 100;
            dt.Columns.Add(dc);

            return dsPhone;

        }

        public static void DoSomeSeedings(DataSet dataSet)
        {
            DataRow rw = dataSet.Tables[0].NewRow();
            rw["nummer"] = "0497123456";
            rw["naam"] = "Jansens";
            rw["voornaam"] = "Jan";
            rw["mobile"] = 1;
            dataSet.Tables[0].Rows.Add(rw);
            rw = dataSet.Tables[0].NewRow();
            rw["nummer"] = "050123456";
            rw["naam"] = "Peeters";
            rw["voornaam"] = "Omer";
            rw["mobile"] = 2;
            dataSet.Tables[0].Rows.Add(rw);
        }
    }
}
