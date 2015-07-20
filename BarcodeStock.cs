using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class BarcodeStock
    {
        int id;
        public static int recBarcodeST = 0;
        

        public BarcodeStock(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable barcode = new Hashtable();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!barcode.ContainsKey(Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()))
                {
                    try
                    {
                        BarcodeStockEntity data = new BarcodeStockEntity(Program._DATA_TABLE.Rows[i]["Shop"].ToString().Trim(), Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim().PadLeft(8, '0'));
                        data.OrderNo = Program._DATA_TABLE.Rows[i]["BillNumber"].ToString().Trim();
                        data.Product = Program._DATA_TABLE.Rows[i]["Product"].ToString().Trim().PadLeft(8, '0');
                        data.Cost = double.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim());
                        data.SellFinished = false;
                        batchOperation.InsertOrMerge(data);
                        recBarcodeST++;
                        barcode[Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()] = true;
                    }
                    catch { }
                }
            }

            try
            {
                if (Program._UPDATE)
                {
                    Program._RECORD++;
                    Console.WriteLine("Update Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD * 100);
                    Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
                }
                else
                {
                    Program._RECORD++;
                    Console.WriteLine("Insert Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD * 100);
                    Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
                }
            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }

        }
    }
}
