using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrderProgram
{
    public partial class frmAddOrder : Form
    {
        Helper help;
        public frmAddOrder()
        {
            InitializeComponent();
            help = new Helper();
        }
        private void BtnAddProc_Click(object sender, EventArgs e)
        {    
            string productname = lstProducts.SelectedItem.ToString();
            string unitprice = help.ProductUnitPrice(productname);
            ListViewItem lvi = new ListViewItem();
            lvi.Text = productname;
            lvi.SubItems.Add(unitprice);
            lvi.SubItems.Add(1.ToString());
            lvi.SubItems.Add(0.ToString());
            lvi.SubItems.Add(unitprice);

            lvProducts.Items.Add(lvi);
        }

        private void LvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProducts.SelectedIndices.Count != 0)
            {
                txtGuncUnitPrice.Text = lvProducts.SelectedItems[0].SubItems[1].Text;
                txtGuncQuantity.Text = lvProducts.SelectedItems[0].SubItems[2].Text;
                txtGuncDiscount.Text = lvProducts.SelectedItems[0].SubItems[3].Text;
            }
        }

        private void TxtGuncUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            lblToplamFiyat.Text = help.GetSumPrice(txtGuncUnitPrice.Text, txtGuncQuantity.Text, txtGuncDiscount.Text).ToString();
        }
        private void TxtGuncQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            lblToplamFiyat.Text = help.GetSumPrice(txtGuncUnitPrice.Text, txtGuncQuantity.Text, txtGuncDiscount.Text).ToString();
        }
        private void TxtGuncDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            lblToplamFiyat.Text = help.GetSumPrice(txtGuncUnitPrice.Text, txtGuncQuantity.Text, txtGuncDiscount.Text).ToString();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (txtGuncUnitPrice.Text != null && txtGuncQuantity.Text != null && txtGuncDiscount.Text != null)
            {
                int sayı = int.Parse(txtGuncQuantity.Text);
                if (help.UnitCount(lvProducts.SelectedItems[0].SubItems[0].Text, sayı))
                {
                    lvProducts.SelectedItems[0].SubItems[1].Text = txtGuncUnitPrice.Text;
                    lvProducts.SelectedItems[0].SubItems[2].Text = txtGuncQuantity.Text;
                    lvProducts.SelectedItems[0].SubItems[3].Text = txtGuncDiscount.Text;
                    lvProducts.SelectedItems[0].SubItems[4].Text = lblToplamFiyat.Text;
                }
                else
                {
                    MessageBox.Show("Gardaşım napıyorsun bu kadar stoğumuz yok");
                }
            }
            else
            {
                MessageBox.Show("Tüm değerleri girmeniz zorunludur");
            }
        }
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (lstCustomers.SelectedItem != null && lstEmployees.SelectedItem != null)
            {
                string cusQ = "";
                DateTime deneme = dtpOrderDate.Value;
                string orderdate = deneme.ToString("yyyy-MM-dd HH:mm:ss.fff");
                DateTime deneme2 = dtpRequiredDate.Value;
                string requireddate = deneme2.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string isim = lstEmployees.SelectedItem.ToString();
                string customer = lstCustomers.SelectedItem.ToString();
                string[] fullname = isim.Split(' ');

                if (customer.Contains((char)39) == true)
                {
                    cusQ = "SELECT CustomerID FROM Customers WHERE CompanyName LIKE '" + 
                        customer.Substring(0,customer.IndexOf((char)39)) + "'+'%'";
                }
                else
                {
                    cusQ = "SELECT CustomerID FROM Customers WHERE CompanyName='" + lstCustomers.Text + "'";
                }
                string empQ = "SELECT EmployeeID FROM Employees WHERE FirstName='" + fullname[0] + "'" +
                    "AND LastName='" + fullname[1] + "'";

                string query = @"INSERT INTO Orders(CustomerID,EmployeeID,OrderDate,RequiredDate,Freight,ShipAddress,ShipCity,ShipCountry)VALUES('" + help.ıdGetirString(cusQ) + "', '" + help.ıdGetirInt(empQ) + "', '" + orderdate + "', '" + requireddate + "', '" + txtFreight.Text + "', '" + txtShipAddress.Text + "', '" + txtShipCity.Text + "', '" + txtShipCountry.Text + "')";

                help.InsertDBOrder(query);
                if (lvProducts.Items.Count > 0)
                {
                    help.InsertDBOrderDetails(lvProducts);
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Çalışan ve Müşteri seçimi zorunludur.");
            }
        }

        private void AddOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            decimal toplam = 0;
            foreach (ListViewItem item in lvProducts.Items)
            {
                toplam+=decimal.Parse(item.SubItems[4].Text);
            }
            lblToplam.Text = toplam.ToString();
        }

        private void AddOrder_Load_1(object sender, EventArgs e)
        {
            string customerQuery = "SELECT CompanyName FROM Customers";
            string employeeQuery = "SELECT FirstName+' '+LastName FROM Employees";
            string productQuery = "SELECT ProductName FROM Products";
            help.FillAddOrderListBox(customerQuery, lstCustomers);
            help.FillAddOrderListBox(employeeQuery, lstEmployees);
            help.FillAddOrderListBox(productQuery, lstProducts);
        }

        private void AddOrder_DoubleClick(object sender, EventArgs e)
        {
            decimal toplam = 0;
            foreach (ListViewItem item in lvProducts.Items)
            {
                toplam += decimal.Parse(item.SubItems[4].Text);
            }
            lblToplam.Text = toplam.ToString();
        }
    }
}
