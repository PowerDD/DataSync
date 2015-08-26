using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class BankTransferHeader
    {
         int id;
         public static int recTransferH = 0;

         public BankTransferHeader(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable TransferH = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!TransferH.ContainsKey(Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim()))
                {
                    try
                    {
                        DynamicTableEntity data = new DynamicTableEntity();
                        data.PartitionKey = "88888888";
                        data.RowKey = Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim().PadLeft(6, '0');
                        Dictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();

                        properties.Add("BankAccount", new EntityProperty(Program._DATA_TABLE.Rows[i]["BankAccountID"].ToString().Trim()));
                        properties.Add("TransferMoney", new EntityProperty(double.Parse(Program._DATA_TABLE.Rows[i]["TransferMoney"].ToString().ToLower().Trim())));
                        try
                        {
                            properties.Add("ReceiveDate", new EntityProperty(Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["ReceiveDate"].ToString())));
                        }
                        catch { }

                        //BankTransferHeaderEntity data = new BankTransferHeaderEntity("88888888", Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim().PadLeft(6, '0'));
                        //data.BankAccount = Program._DATA_TABLE.Rows[i]["BankAccountID"].ToString().Trim();
                        //data.TransferMoney = double.Parse(Program._DATA_TABLE.Rows[i]["TransferMoney"].ToString().ToLower().Trim());
                        //try { data.ReceiveDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["ReceiveDate"].ToString()); }
                        //catch { }
                        batchOperation.InsertOrMerge(data);
                        recTransferH++;
                        TransferH[Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim()] = true;
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
