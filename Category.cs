using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class Category
    {
        int id;

        public Category(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                CategoryEntity data = new CategoryEntity("88888888", Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim());
                data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString();
                data.Url = Program._DATA_TABLE.Rows[i]["Name"].ToString().ToLower().Replace("/", "-").Replace(" ", "_").Replace("_-_", "-");
                data.Active = true;
                data.Priority = 99;
                data.ProductCount = int.Parse(Program._DATA_TABLE.Rows[i]["cnt"].ToString());
                batchOperation.InsertOrMerge(data);
            }

            try
            {
                Program._RECORD++;
                Console.WriteLine("Insert Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD*100);
                Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }

        }
    }
}
