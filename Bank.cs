using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class Bank
    {
         int id;
         public static int recBank = 0;

         public Bank(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable bank = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!bank.ContainsKey(Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim()))
                {
                    try
                    {
                        BankEntity data = new BankEntity("88888888", Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim().PadLeft(6, '0'));
                        data.Bank = Program._DATA_TABLE.Rows[i]["Bank"].ToString().Trim();
                        data.BranchName = Program._DATA_TABLE.Rows[i]["BranchName"].ToString().ToLower().Trim();
                        data.AccountNumber = Program._DATA_TABLE.Rows[i]["AccountNumber"].ToString().Trim();
                        data.AccountName = Program._DATA_TABLE.Rows[i]["AccountName"].ToString().Trim();
                        data.AccountType = int.Parse(Program._DATA_TABLE.Rows[i]["AccountType"].ToString().Trim());
                        batchOperation.InsertOrMerge(data);
                        recBank++;
                        bank[Program._DATA_TABLE.Rows[i]["ID"].ToString().Trim()] = true;
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
