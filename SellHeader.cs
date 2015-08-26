using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class SellHeader
    {
        int id;
         public static int recSellH = 0;

         public SellHeader(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable sellH = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!sellH.ContainsKey(Program._DATA_TABLE.Rows[i]["SellNo"].ToString().Trim().PadLeft(8, '0')))
                {
                    try
                    {
                        DynamicTableEntity data = new DynamicTableEntity();
                        data.PartitionKey = "88888888";
                        data.RowKey = Program._DATA_TABLE.Rows[i]["SellNo"].ToString().Trim().PadLeft(8, '0');
                        Dictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();

                        properties.Add("DocNo", new EntityProperty(Program._DATA_TABLE.Rows[i]["DocNo"].ToString().Trim().PadLeft(8, '0')));
                        properties.Add("Customer", new EntityProperty(Program._DATA_TABLE.Rows[i]["Customer"].ToString().Trim().PadLeft(6, '0')));
                        properties.Add("BillNo", new EntityProperty(Program._DATA_TABLE.Rows[i]["BillNo"].ToString().Trim()));
                        properties.Add("Credit", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Credit"].ToString().Trim())));
                        properties.Add("PayType", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["PayType"].ToString().Trim())));
                        properties.Add("Cash", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Cash"].ToString().Trim())));
                        properties.Add("DiscountPercent", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["DiscountPercent"].ToString().Trim())));
                        properties.Add("DiscountCash", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["DiscountCash"].ToString().Trim())));
                        properties.Add("Paid", new EntityProperty(Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Paid"].ToString().Trim())));
                        properties.Add("Profit", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Profit"].ToString().Trim())));
                        properties.Add("TotalPrice", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["TotalPrice"].ToString().Trim())));
                        properties.Add("PointReceived", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["PointReceived"].ToString().Trim())));
                        properties.Add("PointUse", new EntityProperty(Convert.ToDouble(Program._DATA_TABLE.Rows[i]["PointUse"].ToString().Trim())));
                        properties.Add("Comment", new EntityProperty(Program._DATA_TABLE.Rows[i]["Comment"].ToString().Trim()));
                        try { properties.Add("SellDate", new EntityProperty(Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["SellDate"].ToString())));  }
                        catch { }

                        //SellHeaderEntity data = new SellHeaderEntity("88888888", Program._DATA_TABLE.Rows[i]["SellNo"].ToString().Trim().PadLeft(8, '0'));
                        //data.DocNo = Program._DATA_TABLE.Rows[i]["DocNo"].ToString().Trim().PadLeft(8, '0');
                        //data.Customer = Program._DATA_TABLE.Rows[i]["Customer"].ToString().Trim().PadLeft(6, '0');
                        //data.BillNo = Program._DATA_TABLE.Rows[i]["BillNo"].ToString().Trim();
                        //data.Credit = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Credit"].ToString().Trim());
                        //data.PayType = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["PayType"].ToString().Trim());
                        //data.Cash = Convert.ToDouble( Program._DATA_TABLE.Rows[i]["Cash"].ToString().Trim());
                        //data.DiscountPercent = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["DiscountPercent"].ToString().Trim());
                        //data.DiscountCash = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["DiscountCash"].ToString().Trim());
                        //try { data.SellDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["SellDate"].ToString()); }
                        //catch { }
                        //data.Paid = Convert.ToInt32(Program._DATA_TABLE.Rows[i]["Paid"].ToString().Trim());
                        //data.Profit = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["Profit"].ToString().Trim());
                        //data.TotalPrice = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["TotalPrice"].ToString().Trim());
                        //data.PointReceived = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["PointReceived"].ToString().Trim());
                        //data.PointUse = Convert.ToDouble(Program._DATA_TABLE.Rows[i]["PointUse"].ToString().Trim());
                        //data.Comment = Program._DATA_TABLE.Rows[i]["Comment"].ToString().Trim();
                        batchOperation.InsertOrMerge(data);
                        recSellH++;
                        sellH[Program._DATA_TABLE.Rows[i]["SellNo"].ToString().Trim()] = true;
                    }
                    catch { }
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
