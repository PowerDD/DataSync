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
    public class Barcode
    {
        int id;
        public static int recBarcode = 0;

        public Barcode(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable barcode = new Hashtable();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!barcode.ContainsKey(Program._DATA_TABLE.Rows[i]["Shop"].ToString().Trim()) && !barcode.ContainsKey(Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()) || Program._UPDATE == true)
                {
                    try
                    {
                        BarcodeEntity data = new BarcodeEntity(Program._DATA_TABLE.Rows[i]["Shop"].ToString().Trim(), Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim().PadLeft(8, '0'));
                        data.Product = Program._DATA_TABLE.Rows[i]["ProductID"].ToString().Trim().PadLeft(8, '0');
                        data.DocNo = Program._DATA_TABLE.Rows[i]["DocNumber"].ToString().Trim().PadLeft(8, '0');
                        data.SellNo = Program._DATA_TABLE.Rows[i]["SellNumber"].ToString().Trim().PadLeft(8, '0');
                        data.OrderNo = Program._DATA_TABLE.Rows[i]["BillNumber"].ToString().Trim();
                        try { data.ReceivedDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["ReceivedDate"].ToString()); }
                        catch { }
                        try { data.SellDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["SellDate"].ToString()); }
                        catch { }
                        data.Customer = Program._DATA_TABLE.Rows[i]["CustomerNumber"].ToString().Trim().PadLeft(6, '0');
                        data.SellPrice = double.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim());
                        data.Cost = 0;
                        data.OperationCost = 0;
                        data.SellFinished = true;
                        batchOperation.InsertOrMerge(data);
                        recBarcode++;
                        barcode[Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()] = true;
                    }
                    catch 
                    {
                        Console.WriteLine(Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim());
                    }
                }
            }

            try
            {
                if (Program._DATA_TABLE.Rows.Count > 0)
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

            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }
        }
    }
}
