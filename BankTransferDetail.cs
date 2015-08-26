using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class BankTransferDetail
    {
         int id;
         public static int recTransferD = 0;

         public BankTransferDetail(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable TransferD = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                string Rowkey = (Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim() + "-" + Program._DATA_TABLE.Rows[i]["SellNumber"].ToString().Trim());
                if (!TransferD.ContainsKey(Rowkey))
                {
                    try
                    {
                        DynamicTableEntity data = new DynamicTableEntity();
                        data.PartitionKey = "88888888";
                        data.RowKey = Rowkey;
                        Dictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();

                        properties.Add("SellNumber", new EntityProperty(Program._DATA_TABLE.Rows[i]["SellNumber"].ToString().Trim().PadLeft(8, '0')));
                        properties.Add("ReceiveMoney", new EntityProperty(double.Parse(Program._DATA_TABLE.Rows[i]["ReceiveMoney"].ToString().ToLower().Trim())));

                        //BankTransferDetailEntity data = new BankTransferDetailEntity("88888888", Rowkey);
                        //data.SellNumber = Program._DATA_TABLE.Rows[i]["SellNumber"].ToString().Trim().PadLeft(8, '0');
                        //data.ReceiveMoney = double.Parse(Program._DATA_TABLE.Rows[i]["ReceiveMoney"].ToString().ToLower().Trim());
                        batchOperation.InsertOrMerge(data);
                        recTransferD++;
                        TransferD[Rowkey] = true;
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
