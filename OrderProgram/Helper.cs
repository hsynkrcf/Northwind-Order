﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderProgram
{
    class Helper
    {
        SqlConnection con;
        SqlCommand cmd;
        public Helper()
        {
            con = new SqlConnection(Properties.Settings.Default.NWD);
        }
        public double GetSumPrice(string unitprice,string quantity,string discount)
        {
            double toplamfiyat = 0.0;
            double ucret = 0.0;
            int adet = 0;
            double indirim = 0.0;
            if (unitprice != string.Empty)
            {
                ucret = Convert.ToDouble(unitprice);
            }

            if (quantity != string.Empty)
            {
                adet = int.Parse(quantity);
            }

            if (discount.Length < 2)
            {
                indirim = double.Parse("0,0" + discount);
            }
            else
            {
                indirim = double.Parse("0," + discount);
            }
            toplamfiyat = ucret * adet;
            toplamfiyat = toplamfiyat - (toplamfiyat * indirim);
            return toplamfiyat;
        }
        public string ProductUnitPrice(string productName)
        {
            CheckConnection();
            string unitprice = "";
            cmd = new SqlCommand("sp_ProductNameUnitPrice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productname", productName);

            SqlDataReader reader = GetReader();
            while (reader.Read())
            {
                unitprice = reader["UnitPrice"].ToString();
            }
            reader.Close();
            unitprice.Replace(',', '.');

            return unitprice;            
        }
        public SqlDataReader GetReader()
        {
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
        public int ıdGetirInt(string query)
        {
            CheckConnection();
            cmd = new SqlCommand(query, con);
            int ıd = Convert.ToInt16(cmd.ExecuteScalar());
            return ıd;
        }
        public string ıdGetirString(string query)
        {
            CheckConnection();            
            cmd = new SqlCommand(query, con);
            string ıd = cmd.ExecuteScalar().ToString();
            return ıd;
        }
        private void CheckConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }     
        public void FillAddOrderListBox(string query1, ListBox lb1)
        {
            CheckConnection();
            cmd = new SqlCommand(query1, con);
            SqlDataReader reader = GetReader();
            while (reader.Read())
            {
                lb1.Items.Add(reader.GetString(0));
            }
            reader.Close();            
        }
        public bool UnitCount(string productName,int adet)
        {
            string query = "";
            if (productName.Contains((char)39) == true)
            {
                query = "SELECT UnitsInStock FROM Products WHERE ProductName LIKE '" +
                    productName.Substring(0, productName.IndexOf((char)39)) + "'";
            }
            else
            {
                query = "SELECT UnitsInStock FROM Products WHERE ProductName ='"+productName+"'";
            }
            if (adet > ıdGetirInt(query))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void InsertDBOrder(string query)
        {            
            int affected = 0;
            CheckConnection();
            cmd = new SqlCommand(query, con);
            affected = (int)cmd.ExecuteNonQuery();
            if (affected > 0)
            {
                MessageBox.Show("Sipariş Eklemesi Tamam!");
            }
            else
            {
                MessageBox.Show("Sipariş Eklemesi Başarısız");
            }
        }       
        public void InsertDBOrderDetails(ListView lv)
        {
            int ProductID = 0;
            CheckConnection();
            SqlCommand cmd2 = new SqlCommand("SELECT IDENT_CURRENT('Orders')", con);
            int orderID = Convert.ToInt16(cmd2.ExecuteScalar());
            foreach (ListViewItem item in lv.Items)
            {
                int i = 0;
                string ProductName = item.SubItems[i].Text;
                i++;
                if (ProductName.Contains((char)39) == true)
                {
                    ProductID = ıdGetirInt("SELECT ProductID FROM Products WHERE ProductName LIKE '" +
                    ProductName.Substring(0, ProductName.IndexOf((char)39)) + "'");
                }
                else
                {
                   ProductID  = ıdGetirInt("SELECT ProductID FROM Products WHERE ProductName ='" + ProductName + "'");
                }
                string UnitPrice = item.SubItems[i].Text;
                UnitPrice=UnitPrice.Replace(',', '.');
                i++;
                string Quantity = item.SubItems[i].Text;
                i++;
                string Discount = "0."+item.SubItems[i].Text;
                string query = @"INSERT INTO [Order Details](OrderID,ProductID,UnitPrice,Quantity,Discount)VALUES('" + orderID + "', '"+ProductID+"', '"+UnitPrice+"','"+Quantity+"', '"+Discount+"')";
                cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
            }

        }
        public void FillWithQuery(string query, ListView lv)
        {
            CheckConnection();
            cmd = new SqlCommand(query, con);
            SqlDataReader reader = GetReader();

            ListViewItem lvi;
            while (reader.Read())
            {
                lvi = new ListViewItem();
                lvi.Text = reader[0].ToString();
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    if (i != reader.FieldCount-1)
                    {
                        lvi.SubItems.Add(reader[i].ToString());
                    }
                    else
                    {
                        int sayı = reader["DelayedDay"] == DBNull.Value ? 0 : reader.GetInt32(i);
                        if (sayı < 0)
                        {
                            lvi.SubItems.Add("0");
                        }
                        else
                        {
                            lvi.BackColor = Color.Red;
                            lvi.SubItems.Add(sayı.ToString());
                        }
                    }
                }
                lv.Items.Add(lvi);
            }
            reader.Close();
        }
    }
}