using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading;

namespace DataSync
{
    class Program
    {
        public static SqlCommand _SQL_CMD = new SqlCommand();
        public static SqlConnection _SQL_CONN;
        public static DataTable _DATA_TABLE;
        public static CloudTable _CLOUD_TABLE;
        public static int _RECORD;

        static void Main(string[] args)
        {
            string database_server = ConfigurationManager.AppSettings["dbHost"];
            string database_name = ConfigurationManager.AppSettings["dbName"];
            string database_username = ConfigurationManager.AppSettings["dbUsername"];
            string database_password = ConfigurationManager.AppSettings["dbPassword"];

            _SQL_CONN = new SqlConnection(@"Data Source=" + database_server + ";Initial Catalog=" + database_name + ";User ID=" + database_username + ";Password=" + database_password);
            _SQL_CMD.Connection = _SQL_CONN;
            _SQL_CMD.CommandTimeout = 3000;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + ConfigurationManager.AppSettings["storageName"] + ";AccountKey=" + ConfigurationManager.AppSettings["storageKey"]);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _CLOUD_TABLE = tableClient.GetTableReference("Barcode5");
            _CLOUD_TABLE.CreateIfNotExists();
            //_CLOUD_TABLE.DeleteIfExists();

            /*Console.WriteLine("Getting Barcode Records");
            DataTable dt = QueryData(@"
                SELECT COUNT(*) cnt
                FROM (
		                SELECT BranchNumber, DocNumber, MAX(SellNumber) SellNumber
		                FROM NewStock.dbo.Sell sl
		                WHERE Status IS NULL
		                GROUP BY BranchNumber, DocNumber
	                ) s
	                LEFT JOIN NewStock.dbo.SellSN sn
	                ON sn.SellNumber = s.SellNumber
	                AND sn.BranchNumber = s.BranchNumber
                WHERE sn.BranchNumber = 1
            ");

            int count = int.Parse(dt.Rows[0]["cnt"].ToString());
            Console.WriteLine("Total Record(s) = {0}\n", count);
            */

            Console.WriteLine("Getting All Barcode Data for Insert");
           _DATA_TABLE = QueryData("sp_InsertBarcode");
           Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

            ExecuteCommand("sp_InsertBarcodeProduct");

            int count = _DATA_TABLE.Rows.Count;
            Parallel.For(0, count / 100, i =>
            {
                Barcode b = new Barcode(i);
                b.Insert();
            });
            Console.Read();
        }

        public static void WriteErrorLog(string message)
        {
            string filename = "error-"+DateTime.Now.ToString("yyyyMMdd") + ".txt";
            StreamWriter sw = new StreamWriter(filename, true);
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss") + "\t" + message);
            sw.Close();
        }

        public static void ExecuteCommand(string command)
        {
            if (_SQL_CONN.State == ConnectionState.Closed || _SQL_CONN.State == ConnectionState.Connecting || _SQL_CONN.State == ConnectionState.Broken)
            {
                _SQL_CONN.Open();
            }
            _SQL_CMD.CommandText = command;

            try
            {
                _SQL_CMD.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                WriteErrorLog("SQL = " + command + " : " + ex.Message);
            }
        }

        public static DataTable QueryData(string command)
        {
            if (_SQL_CONN.State == ConnectionState.Closed || _SQL_CONN.State == ConnectionState.Connecting || _SQL_CONN.State == ConnectionState.Broken)
            {
                _SQL_CONN.Open();
            }
            _SQL_CMD.CommandText = command;
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(_SQL_CMD);
                sqlDataAdapter.Fill(dt);
                
            }
            catch (Exception ex)
            {
                WriteErrorLog("SQL = " + command + " : " + ex.Message);
            }

            return dt;
        }


    }
}
