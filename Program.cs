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
        public static DataTable _DATATABLE;
        public static CloudTable _CLOUD_TABLE;
        public static int _RECORD;
        public static int _COUNT;
        public static bool _UPDATE = false;
        public static string database_server = ConfigurationManager.AppSettings["dbHost"];
        public static string database_name = ConfigurationManager.AppSettings["dbName"];
        public static string database_username = ConfigurationManager.AppSettings["dbUsername"];
        public static string database_password = ConfigurationManager.AppSettings["dbPassword"];
        public static string Start_Hour = ConfigurationManager.AppSettings["StartHours"];
        public static string End_Hour = ConfigurationManager.AppSettings["EndHours"];
        public static string _SHOP;
        public static string _CUS_NO;


        static void Main(string[] args)
        {
            _SQL_CONN = new SqlConnection(@"Data Source=" + database_server + ";Initial Catalog=" + database_name + ";User ID=" + database_username + ";Password=" + database_password);
            _SQL_CMD.Connection = _SQL_CONN;
            _SQL_CMD.CommandTimeout = 3000;

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

            //DataSyn();
            while (true)
            {
                DataSyn();
                Thread.Sleep(60000);
            }
            //Console.ReadLine();
        }

        public static void WriteErrorLog(string message)
        {
            string filename = "error-" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            try
            {
                if (command == "sp_BarcodeStockInsert")
                {
                    _SQL_CMD.Parameters.Clear();
                    _SQL_CMD.CommandText = command;
                    _SQL_CMD.CommandType = CommandType.StoredProcedure;

                    _SQL_CMD.Parameters.AddWithValue("@shop", _SHOP);
                    _SQL_CMD.Parameters.AddWithValue("@customer", _CUS_NO);
                    _SQL_CMD.ExecuteNonQuery();
                }
                else if (command == "sp_BarcodeStockDataUpdate")
                {
                    _SQL_CMD.Parameters.Clear();
                    _SQL_CMD.CommandText = command;
                    _SQL_CMD.CommandType = CommandType.StoredProcedure;

                    _SQL_CMD.Parameters.AddWithValue("@shop", _SHOP);
                    _SQL_CMD.ExecuteNonQuery();
                }
                else
                {
                    _SQL_CMD.CommandText = command;
                    _SQL_CMD.ExecuteNonQuery();
                }
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
            if (command == "sp_BarcodeStock")
            {
                _SQL_CMD.Parameters.Clear();
                _SQL_CMD.CommandText = command;
                _SQL_CMD.CommandType = CommandType.StoredProcedure;
                _SQL_CMD.Parameters.AddWithValue("@shop", _SHOP);
                _SQL_CMD.Parameters.AddWithValue("@customerNumber", _CUS_NO);
                _SQL_CMD.ExecuteNonQuery();
            }
            else if (command == "sp_BarcodeStockData")
            {
                _SQL_CMD.Parameters.Clear();
                _SQL_CMD.CommandText = command;
                _SQL_CMD.CommandType = CommandType.StoredProcedure;
                _SQL_CMD.Parameters.AddWithValue("@shop", _SHOP);
                _SQL_CMD.ExecuteNonQuery();
            }
            else
            {
                _SQL_CMD.CommandText = command;
            }
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

        public static void DataSyn()
        {
            DateTime dt1 = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy ") + Start_Hour);
            DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy ") + End_Hour);

            if ((DateTime.Now > dt1) && (DateTime.Now < dt2) && (DateTime.Now.DayOfWeek.ToString() != DayOfWeek.Sunday.ToString()))
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + ConfigurationManager.AppSettings["storageName"] + ";AccountKey=" + ConfigurationManager.AppSettings["storageKey"]);
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();


                //// Sync Brand
                //_CLOUD_TABLE = tableClient.GetTableReference("Brand");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All Brand Data for Insert");
                //_DATA_TABLE = QueryData("sp_Brand");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BrandInsert");
                //    Brand.recBrand = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Brand b = new Brand(i);
                //        b.Insert();
                //    });

                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", Brand.recBrand);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_BrandData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BrandDataUpdate");
                //    Brand.recBrand = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Brand b = new Brand(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", Brand.recBrand);
                //    }

                //}



                //// Sync Category
                //_CLOUD_TABLE = tableClient.GetTableReference("Category");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All Category Data for Insert");
                //_DATA_TABLE = QueryData("sp_Category");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_CategoryInsert");
                //    Category.recCategory = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Category c = new Category(i);
                //        c.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", Category.recCategory);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_CategoryData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_CategoryDataUpdate");
                //    Category.recCategory = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Category c = new Category(i);
                //        c.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", Category.recCategory);
                //    }
                //}


                //// Sync Product
                //_CLOUD_TABLE = tableClient.GetTableReference("Product");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All Product Data for Insert");
                //_DATA_TABLE = QueryData("sp_Product");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_ProductInsert");
                //    Product.recProduct = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Product p = new Product(i);
                //        p.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", Product.recProduct);
                //    }

                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_ProductData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_ProductDataUpdate");
                //    Product.recProduct = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Product p = new Product(i);
                //        p.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", Product.recProduct);
                //    }

                //}



                //// Sync SellHeader
                //_CLOUD_TABLE = tableClient.GetTableReference("SellHeader");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All SellHeader Data for Insert");
                //_DATA_TABLE = QueryData("sp_SellHeader");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_SellHeaderInsert");
                //    SellHeader.recSellH = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        SellHeader b = new SellHeader(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", SellHeader.recSellH);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_SellHeaderData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_SellHeaderDataUpdate");
                //    SellHeader.recSellH = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        SellHeader b = new SellHeader(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", SellHeader.recSellH);
                //    }
                //}


                //// Sync SellDetail
                //_CLOUD_TABLE = tableClient.GetTableReference("SellDetail");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All SellDetail Data for Insert");
                //_DATA_TABLE = QueryData("sp_SellDetail");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_SellDetailInsert");
                //    SellDetail.recSellD = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        SellDetail b = new SellDetail(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", SellDetail.recSellD);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_SellDetailData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_SellDetailDataUpdate");
                //    SellDetail.recSellD = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        SellDetail b = new SellDetail(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", SellDetail.recSellD);
                //    }
                //}


                //// Sync Bank
                //_CLOUD_TABLE = tableClient.GetTableReference("Bank");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All Bank Data for Insert");
                //_DATA_TABLE = QueryData("sp_Bank");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BankInsert");
                //    Bank.recBank = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Bank b = new Bank(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", Bank.recBank);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_BankData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BankDataUpdate");
                //    Bank.recBank = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Bank b = new Bank(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", Bank.recBank);
                //    }
                //}


                //// Sync BankTransferHeader
                //_CLOUD_TABLE = tableClient.GetTableReference("BankTransferHeader");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All BankTransferHeader Data for Insert");
                //_DATA_TABLE = QueryData("sp_BankTransferHeader");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BankTransferHeaderInsert");
                //    BankTransferHeader.recTransferH = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        BankTransferHeader b = new BankTransferHeader(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", BankTransferHeader.recTransferH);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_BankTransferHeaderData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BankTransferHeaderDataUpdate");
                //    BankTransferHeader.recTransferH = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        BankTransferHeader b = new BankTransferHeader(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", BankTransferHeader.recTransferH);
                //    }

                //}


                //// Sync BankTransferDetail
                //_CLOUD_TABLE = tableClient.GetTableReference("BankTransferDetail");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All BankTransferDetail Data for Insert");
                //_DATA_TABLE = QueryData("sp_BankTransferDetail");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BankTransferDetailInsert");
                //    BankTransferDetail.recTransferD = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        BankTransferDetail b = new BankTransferDetail(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", BankTransferDetail.recTransferD);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_BankTransferDetailData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BankTransferDetailDataUpdate");
                //    BankTransferDetail.recTransferD = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        BankTransferDetail b = new BankTransferDetail(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", BankTransferDetail.recTransferD);
                //    }

                //}



                //// Sync Barcode
                //_CLOUD_TABLE = tableClient.GetTableReference("Barcode");
                //_CLOUD_TABLE.CreateIfNotExists();

                //Console.WriteLine("Getting All Barcode Data for Insert");
                //_DATA_TABLE = QueryData("sp_Barcode");
                //_RECORD = 0;

                //if (_DATA_TABLE.Rows.Count > 0)
                //{
                //    _UPDATE = false;
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BarcodeInsert");
                //    Barcode.recBarcode = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Barcode b = new Barcode(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Insert successed total Record(s) = {0}\n", Barcode.recBarcode);
                //    }
                //}
                //else
                //{
                //    _UPDATE = true;
                //    _DATA_TABLE = QueryData("sp_BarcodeData");
                //    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                //    ExecuteCommand("sp_BarcodeDataUpdate");
                //    Barcode.recBarcode = 0;
                //    _COUNT = _DATA_TABLE.Rows.Count;
                //    Parallel.For(0, _COUNT / 100 + 1, i =>
                //    {
                //        Barcode b = new Barcode(i);
                //        b.Insert();
                //    });
                //    if (_DATA_TABLE.Rows.Count > 0)
                //    {
                //        Console.WriteLine("Update successed total Record(s) = {0}\n", Barcode.recBarcode);
                //    }

                //}


                // Sync BarcodeStock
                _DATATABLE = QueryData("SELECT * FROM Shop WHERE ShopCustomer IS NOT NULL");
                for (int r = 0; r < _DATATABLE.Rows.Count; r++)
                {
                    _SHOP = _DATATABLE.Rows[r]["ShopId"].ToString();
                    _CUS_NO = _DATATABLE.Rows[r]["ShopCustomer"].ToString();
                    _CLOUD_TABLE = tableClient.GetTableReference("BarcodeStock");
                    _CLOUD_TABLE.CreateIfNotExists();

                    Console.WriteLine("Getting All BarcodeStock(Shop " + _SHOP + ") Data for Insert");
                    _DATA_TABLE = QueryData("sp_BarcodeStock");
                    _RECORD = 0;

                    if (_DATA_TABLE.Rows.Count > 0)
                    {
                        _UPDATE = false;
                        Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                        ExecuteCommand("sp_BarcodeStockInsert");
                        BarcodeStock.recBarcodeST = 0;
                        _COUNT = _DATA_TABLE.Rows.Count;
                        Parallel.For(0, _COUNT / 100 + 1, i =>
                        {
                            BarcodeStock b = new BarcodeStock(i);
                            b.Insert();
                        });
                        if (_DATA_TABLE.Rows.Count > 0)
                        {
                            Console.WriteLine("Insert successed total Record(s) = {0}\n", BarcodeStock.recBarcodeST);
                        }
                    }
                    else
                    {
                        _UPDATE = true;
                        _DATA_TABLE = QueryData("sp_BarcodeStockData");
                        Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                        ExecuteCommand("sp_BarcodeStockDataUpdate");
                        BarcodeStock.recBarcodeST = 0;
                        _COUNT = _DATA_TABLE.Rows.Count;
                        Parallel.For(0, _COUNT / 100 + 1, i =>
                        {
                            BarcodeStock b = new BarcodeStock(i);
                            b.Insert();
                        });
                        if (_DATA_TABLE.Rows.Count > 0)
                        {
                            Console.WriteLine("Update successed total Record(s) = {0}\n", BarcodeStock.recBarcodeST);
                        }
                    }
                }


                // Sync Customer Shop 88888888
                _CLOUD_TABLE = tableClient.GetTableReference("Customer");
                _CLOUD_TABLE.CreateIfNotExists();

                Console.WriteLine("Getting All Customer(Shop 88888888) Data for Insert");
                _DATA_TABLE = QueryData("sp_Customer");
                _RECORD = 0;

                if (_DATA_TABLE.Rows.Count > 0)
                {
                    _UPDATE = false;
                    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                    ExecuteCommand("sp_CustomerInsert");
                    Customer.recCustomer = 0;
                    _COUNT = _DATA_TABLE.Rows.Count;
                    Parallel.For(0, _COUNT / 100 + 1, i =>
                    {
                        Customer c = new Customer(i);
                        c.Insert();
                    });
                    if (_DATA_TABLE.Rows.Count > 0)
                    {
                        Console.WriteLine("Insert successed total Record(s) = {0}\n", Customer.recCustomer);
                    }
                }
                else
                {
                    _UPDATE = true;
                    _DATA_TABLE = QueryData("sp_CustomerData");
                    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                    ExecuteCommand("sp_CustomerDataUpdate");
                    Customer.recCustomer = 0;
                    _COUNT = _DATA_TABLE.Rows.Count;
                    Parallel.For(0, _COUNT / 100 + 1, i =>
                    {
                        Customer c = new Customer(i);
                        c.Insert();
                    });
                    if (_DATA_TABLE.Rows.Count > 0)
                    {
                        Console.WriteLine("Update successed total Record(s) = {0}\n", Customer.recCustomer);
                    }
                }

                // Sync Customer Shop 00000001
                _CLOUD_TABLE = tableClient.GetTableReference("Customer");
                _CLOUD_TABLE.CreateIfNotExists();

                Console.WriteLine("Getting All Customer(Shop 00000001) Data for Insert");
                _DATA_TABLE = QueryData("sp_CustomerShop");
                _RECORD = 0;

                if (_DATA_TABLE.Rows.Count > 0)
                {
                    _UPDATE = false;
                    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                    ExecuteCommand("sp_CustomerShopInsert");
                    Customer.recCustomer = 0;
                    _COUNT = _DATA_TABLE.Rows.Count;
                    Parallel.For(0, _COUNT / 100 + 1, i =>
                    {
                        Customer c = new Customer(i);
                        c.Insert();
                    });
                    if (_DATA_TABLE.Rows.Count > 0)
                    {
                        Console.WriteLine("Insert successed total Record(s) = {0}\n", Customer.recCustomer);
                    }
                }
                else
                {
                    _UPDATE = true;
                    _DATA_TABLE = QueryData("sp_CustomerDataShop");
                    Console.WriteLine("Total Record(s) = {0}\n", _DATA_TABLE.Rows.Count);

                    ExecuteCommand("sp_CustomerDataShopUpdate");
                    Customer.recCustomer = 0;
                    _COUNT = _DATA_TABLE.Rows.Count;
                    Parallel.For(0, _COUNT / 100 + 1, i =>
                    {
                        Customer c = new Customer(i);
                        c.Insert();
                    });
                    if (_DATA_TABLE.Rows.Count > 0)
                    {
                        Console.WriteLine("Update successed total Record(s) = {0}\n", Customer.recCustomer);
                    }
                }
            }
        }
    }
}