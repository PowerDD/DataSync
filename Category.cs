using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class Category
    {
         int id;
         public static int recCategory = 0;


         public Category(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable category = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!category.ContainsKey(Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim()))
                {
                    try
                    {
                        CategoryEntity data = new CategoryEntity("88888888", Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim().PadLeft(5, '0'));
                        data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString();
                        data.Url = Program._DATA_TABLE.Rows[i]["Name"].ToString().ToLower().Replace("/", "-").Replace(" ", "_").Replace("_-_", "-");
                        data.Active = true;
                        data.Priority = 99;
                        data.ProductCount = int.Parse(Program._DATA_TABLE.Rows[i]["cnt"].ToString());
                        batchOperation.InsertOrMerge(data);
                        recCategory++;
                        category[Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim()] = true;
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
