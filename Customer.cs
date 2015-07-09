using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class Customer
    {
        int id;

        public Customer(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                CustomerEntity data = new CustomerEntity("88888888", Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim());
                data.Member = Program._DATA_TABLE.Rows[i]["Member"].ToString().Trim();
                data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim();
                data.ShopName = Program._DATA_TABLE.Rows[i]["ShopName"].ToString().Trim();
                data.ContactName = Program._DATA_TABLE.Rows[i]["ContactName"].ToString().Trim();
                data.Email = Program._DATA_TABLE.Rows[i]["Email"].ToString().Trim();
                data.Address = Program._DATA_TABLE.Rows[i]["Address"].ToString().Trim();
                data.Address2 = Program._DATA_TABLE.Rows[i]["Address2"].ToString().Trim();
                data.District = Program._DATA_TABLE.Rows[i]["District"].ToString().Trim();
                data.Province = Program._DATA_TABLE.Rows[i]["Province"].ToString().Trim();
                data.ZipCode = Program._DATA_TABLE.Rows[i]["Zipcode"].ToString().Trim();
                data.Tel = Program._DATA_TABLE.Rows[i]["Tel"].ToString().Trim();
                data.Mobile = Program._DATA_TABLE.Rows[i]["Mobile"].ToString().Trim();
                data.Fax = Program._DATA_TABLE.Rows[i]["Fax"].ToString().Trim();
                data.TaxCode = Program._DATA_TABLE.Rows[i]["TaxCode"].ToString().Trim();
                data.Credit = int.Parse(Program._DATA_TABLE.Rows[i]["Credit"].ToString().Trim());
                data.DiscountPercent = (int)double.Parse(Program._DATA_TABLE.Rows[i]["DiscountPercent"].ToString().Trim());
                data.SellPrice = int.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim());
                data.Comment = Program._DATA_TABLE.Rows[i]["Comment"].ToString().Trim();
                try { data.AddDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()); }
                catch { }
                batchOperation.InsertOrMerge(data);
            }

            try
            {
                Program._RECORD++;
                Console.WriteLine("Insert Record {0}-{1}\t\tTotal {2} Records", id + 1, id + 100, Program._RECORD * 100);
                Program._CLOUD_TABLE.ExecuteBatch(batchOperation);
            }
            catch (Exception e)
            {
                Program.WriteErrorLog("Record " + (id + 1) + "-" + (id + 100) + " Error \n" + e.Message + "\n" + e.StackTrace);
            }

        }

    }
}
