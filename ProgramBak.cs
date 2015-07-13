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

            /*
            _CLOUD_TABLE = tableClient.GetTableReference("BarcodeStock");
            _CLOUD_TABLE.DeleteIfExists();
            */

            Console.WriteLine("Getting All Barcode Data for Insert");
            _DATA_TABLE = QueryData(@"
                SELECT '00000001' shop, sn.SN Barcode,p.PN Product,sn.CanUseTime ReceivedDate,s.DocNumber, d.Price SellPrice,
	                MAX(s.SellNumber) SellNumber,s.BillNumber,s.CustomerNumber,TODATETIMEOFFSET (s.DateTime, '+07:00') SellDate
                FROM NewStock.dbo.Sell s
	                LEFT JOIN NewStock.dbo.SellSN sn
	                ON s.SellNumber = sn.SellNumber
	                AND s.BranchNumber = sn.BranchNumber
	                LEFT JOIN NewStock.dbo.Product p
	                ON p.PN = sn.PN
	                LEFT JOIN NewStock.dbo.SellD d
	                ON sn.BranchNumber = d.BranchNumber
	                AND sn.SellNumber = d.SellNumber
	                AND sn.PN = d.PN
	                AND sn.Item = d.Item
                WHERE s.BranchNumber = '1'
	                AND s.Status IS NULL
	                AND s.CustomerNumber = 613
                GROUP BY sn.SN ,p.PN,sn.CanUseTime, s.DocNumber,d.Price, s.BillNumber,s.CustomerNumber,s.DateTime
            ");

            /*Console.WriteLine("Getting All Customer Data for Insert");
            _DATA_TABLE = QueryData(@"
                SELECT CustomerNumber RowKey, CustomerCode Member, Name,
	                RDBranchName ShopName, ContactName, EMail Email, Address, StreetAddress Address2,
	                District, Province, ZipCode Zipcode, REPLACE(Tel, '-', '') Tel, REPLACE(Mobile, '-', '') Mobile, REPLACE(Fax, '-', '') Fax, 
	                TaxCode, Credit, DiscountP DiscountPercent, PriceType SellPrice, Comment, TODATETIMEOFFSET (CTime, '+07:00') AddDate
                FROM NewStock.dbo.Customer
                WHERE Status IS NULL
            ");*/

            /*Console.WriteLine("Getting All Category Data for Insert");
            _DATA_TABLE = QueryData(@"
                SELECT id RowKey, Name, cnt
                FROM NewStock.dbo.ProductGroup g 
                    LEFT JOIN (SELECT GroupID, COUNT(*) cnt FROM NewStock.dbo.Product WHERE Status IS NULL GROUP BY GroupID) c
                    ON g.id = c.GroupID
                WHERE Status IS NULL");*/

            /*Console.WriteLine("Getting All Brand Data for Insert");
            _DATA_TABLE = QueryData(@"
                SELECT id RowKey, Name, cnt
                FROM NewStock.dbo.Brand b
                    LEFT JOIN (SELECT BrandID, COUNT(*) cnt FROM NewStock.dbo.Product WHERE Status IS NULL GROUP BY BrandID) c
                    ON b.id = c.BrandID
                WHERE Status IS NULL");*/

            /*Console.WriteLine("Getting All Product Data for Insert");
            _DATA_TABLE = QueryData(@"
                SELECT p.PN, p.ProductCode, p.Barcode, p.Name, p.MinPrice, a.price1, a.price2, a.price3, a.price4, a.price5, a.price6, a.cost, stock-onCart-onOrder stock, publicPath CoverImage,
	                p.Width, p.Length, p.Height, p.Weight, p.GrossWeight, p.GroupID, p.BrandID, p.Warranty, p.Location, CASE WHEN p.Status IS NULL THEN 1 ELSE 0 END Active, TODATETIMEOFFSET (p.CTime, '+07:00') AddDate
                FROM NewStock.dbo.Product p
	                LEFT JOIN (SELECT id, Cover, publicPath FROM ProductImage WHERE Cover LIKE '%N') i
	                ON p.ProductCode = i.id
	                LEFT JOIN (SELECT id, price1, price2, price3, price4, price5, price6, cost, stock, onCart, onOrder FROM Azure24fin.shop24fin_db.dbo.Product) a
	                ON p.PN = a.id
                ");*/

            Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);


            _CLOUD_TABLE = tableClient.GetTableReference("BarcodeStock");
            //_CLOUD_TABLE.DeleteIfExists();
            _CLOUD_TABLE.CreateIfNotExists();

            int count = _DATA_TABLE.Rows.Count;
            Parallel.For(0, count/100+1, i =>
            {
                ShopBarcode b = new ShopBarcode(i);
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
