using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSync
{
    public class Customer
    {
        int id;
        public static int recCustomer = 0;


        public Customer(int id)
        {
            this.id = id*100;
        }

        public void Insert()
        {
            TableBatchOperation batchOperation = new TableBatchOperation();
            Hashtable customer = new Hashtable();

            for (int i = id; i < id + 100 && i < Program._DATA_TABLE.Rows.Count; i++)
            {
                if (!customer.ContainsKey(Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim()))
                {
                    try
                    {
                        DynamicTableEntity data = new DynamicTableEntity();
                        data.PartitionKey = Program._DATA_TABLE.Rows[i]["shop"].ToString().Trim();
                        data.RowKey = Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim().PadLeft(6, '0');
                        Dictionary<string, EntityProperty> properties = new Dictionary<string, EntityProperty>();

                        properties.Add("Member", new EntityProperty(Program._DATA_TABLE.Rows[i]["Member"].ToString().Trim()));
                        properties.Add("Name", new EntityProperty(Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim()));
                        properties.Add("ShopName", new EntityProperty(Program._DATA_TABLE.Rows[i]["ShopName"].ToString().Trim()));
                        properties.Add("ContactName", new EntityProperty(Program._DATA_TABLE.Rows[i]["ContactName"].ToString().Trim()));
                        properties.Add("Email", new EntityProperty(Program._DATA_TABLE.Rows[i]["Email"].ToString().Trim()));
                        properties.Add("Address", new EntityProperty(Program._DATA_TABLE.Rows[i]["Address"].ToString().Trim()));
                        properties.Add("Address2", new EntityProperty(Program._DATA_TABLE.Rows[i]["Address2"].ToString().Trim()));
                        properties.Add("District", new EntityProperty(Program._DATA_TABLE.Rows[i]["District"].ToString().Trim()));
                        properties.Add("Province", new EntityProperty(Program._DATA_TABLE.Rows[i]["Province"].ToString().Trim()));
                        properties.Add("ZipCode", new EntityProperty(Program._DATA_TABLE.Rows[i]["ZipCode"].ToString().Trim()));
                        properties.Add("Tel", new EntityProperty(Program._DATA_TABLE.Rows[i]["Tel"].ToString().Trim()));
                        properties.Add("Mobile", new EntityProperty(Program._DATA_TABLE.Rows[i]["Mobile"].ToString().Trim()));
                        properties.Add("Fax", new EntityProperty(Program._DATA_TABLE.Rows[i]["Fax"].ToString().Trim()));
                        properties.Add("TaxCode", new EntityProperty(Program._DATA_TABLE.Rows[i]["TaxCode"].ToString().Trim()));
                        properties.Add("Credit", new EntityProperty(int.Parse(Program._DATA_TABLE.Rows[i]["Credit"].ToString().Trim())));
                        properties.Add("DiscountPercent", new EntityProperty((int)double.Parse(Program._DATA_TABLE.Rows[i]["DiscountPercent"].ToString().Trim())));
                        properties.Add("SellPrice", new EntityProperty(int.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim())));
                        properties.Add("Comment", new EntityProperty(Program._DATA_TABLE.Rows[i]["Comment"].ToString().Trim()));
                        try { properties.Add("AddDate", new EntityProperty(Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()))); }
                        catch { }
                        string ac = Program._DATA_TABLE.Rows[i]["Active"].ToString().Trim();
                        if (ac == "1") { properties.Add("Active", new EntityProperty(true)); } else { properties.Add("Active", new EntityProperty(false)); }
                        
                        ////CustomerEntity data = new CustomerEntity(Program._DATA_TABLE.Rows[i]["shop"].ToString().Trim(), Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim().PadLeft(6, '0'));
                        //data.Member = Program._DATA_TABLE.Rows[i]["Member"].ToString().Trim();
                        //data.Name = Program._DATA_TABLE.Rows[i]["Name"].ToString().Trim();
                        //data.ShopName = Program._DATA_TABLE.Rows[i]["ShopName"].ToString().Trim();
                        //data.ContactName = Program._DATA_TABLE.Rows[i]["ContactName"].ToString().Trim();
                        //data.Email = Program._DATA_TABLE.Rows[i]["Email"].ToString().Trim();
                        //data.Address = Program._DATA_TABLE.Rows[i]["Address"].ToString().Trim();
                        //data.Address2 = Program._DATA_TABLE.Rows[i]["Address2"].ToString().Trim();
                        //data.District = Program._DATA_TABLE.Rows[i]["District"].ToString().Trim();
                        //data.Province = Program._DATA_TABLE.Rows[i]["Province"].ToString().Trim();
                        //data.ZipCode = Program._DATA_TABLE.Rows[i]["Zipcode"].ToString().Trim();
                        //data.Tel = Program._DATA_TABLE.Rows[i]["Tel"].ToString().Trim();
                        //data.Mobile = Program._DATA_TABLE.Rows[i]["Mobile"].ToString().Trim();
                        //data.Fax = Program._DATA_TABLE.Rows[i]["Fax"].ToString().Trim();
                        //data.TaxCode = Program._DATA_TABLE.Rows[i]["TaxCode"].ToString().Trim();
                        //data.Credit = int.Parse(Program._DATA_TABLE.Rows[i]["Credit"].ToString().Trim());
                        //data.DiscountPercent = (int)double.Parse(Program._DATA_TABLE.Rows[i]["DiscountPercent"].ToString().Trim());
                        //data.SellPrice = int.Parse(Program._DATA_TABLE.Rows[i]["SellPrice"].ToString().Trim());
                        //data.Comment = Program._DATA_TABLE.Rows[i]["Comment"].ToString().Trim();
                        //try { data.AddDate = Convert.ToDateTime(Program._DATA_TABLE.Rows[i]["AddDate"].ToString()); }
                        //catch { }
                        //string ac = Program._DATA_TABLE.Rows[i]["Active"].ToString().Trim();
                        //if (ac == "1") { data.Active = true; } else { data.Active = false; }
                        batchOperation.InsertOrMerge(data);
                        recCustomer++;
                        customer[Program._DATA_TABLE.Rows[i]["RowKey"].ToString().Trim()] = true;
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
