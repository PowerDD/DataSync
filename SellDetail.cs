using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class SellDetail
    {
        int id;
        public static int recSellD = 0;

         public SellDetail(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable sellD = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                string Rowkey = (Program._DATA_TABLE.Rows[i]["SellNo"].ToString().Trim().PadLeft(8, '0') + "-" + Program._DATA_TABLE.Rows[i]["Product"].ToString().Trim().PadLeft(8, '0'));
                if (!sellD.ContainsKey(Rowkey))
                {
                    try
                    {
                        SellDetailEntity data = new SellDetailEntity("88888888", Rowkey);
                        data.SellPrice = double.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim());
                        data.Quantity = double.Parse(Program._DATA_TABLE.Rows[i]["Quantity"].ToString().Trim());
                        batchOperation.InsertOrMerge(data);
                        recSellD++;
                        sellD[Rowkey] = true;
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
