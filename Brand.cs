using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    class Brand
    {
         int id;
         public static int recBrand = 0;

         public Brand(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable brand = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!brand.ContainsKey(Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim()))
                {
                    try
                    {
                        BrandEntity data = new BrandEntity("88888888", Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim().PadLeft(5, '0'));
                        data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim();
                        data.Url = Program._DATA_TABLE.Rows[i]["Name"].ToString().ToLower().Trim();
                        data.Priority = 99;
                        data.ProductCount = int.Parse(Program._DATA_TABLE.Rows[i]["cnt"].ToString());
                        data.Active = true;
                        batchOperation.InsertOrMerge(data);
                        recBrand++;
                        brand[Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim()] = true;
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

        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
