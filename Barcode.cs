using Microsoft.WindowsAzure.Storage.Table;
using System;
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

        public Barcode(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                BarcodeEntity data = new BarcodeEntity("00000001", Program._DATA_TABLE.Rows[i]["Barcode"].ToString().Trim());
                data.Product = Program._DATA_TABLE.Rows[i]["Product"].ToString().Trim();
                data.DocNo = Program._DATA_TABLE.Rows[i]["DocNumber"].ToString().Trim();
                data.SellNo = Program._DATA_TABLE.Rows[i]["SellNumber"].ToString().Trim();
                data.OrderNo = Program._DATA_TABLE.Rows[i]["BillNumber"].ToString().Trim();
                try { data.ReceivedDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["ReceivedDate"].ToString()); }
                catch { }
                try { data.SellDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["SellDate"].ToString()); }
                catch { }
                data.Customer = Program._DATA_TABLE.Rows[i]["CustomerNumber"].ToString().Trim();
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
