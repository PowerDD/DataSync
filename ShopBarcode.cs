﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class ShopBarcode
    {
        int id;

        public ShopBarcode(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable barcode = new Hashtable();
            StringBuilder sb = new StringBuilder();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!barcode.ContainsKey(Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()))
                {
                    try
                    {
                        sb.Append(Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim() + "|");
                        ShopBarcodeEntity data = new ShopBarcodeEntity(Program._DATA_TABLE.Rows[i]["Shop"].ToString().Trim(), Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim());
                        data.OrderNo = Program._DATA_TABLE.Rows[i]["BillNumber"].ToString().Trim();
                        data.Product = Program._DATA_TABLE.Rows[i]["Product"].ToString().Trim().PadLeft(8, '0');
                        data.Cost = double.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim());
                        data.SellFinished = false;
                        batchOperation.InsertOrMerge(data);
                        barcode[Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim()] = true;
                    }
                    catch { }
                }
            }

            try
            {
                Program._RECORD++;
                Console.WriteLine("Insert Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD*100);
                Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + sb.ToString() + "\n" + e.Message + "\n" + e.StackTrace);
            }

        }


    }
}
