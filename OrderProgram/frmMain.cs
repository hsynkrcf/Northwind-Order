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
    public partial class frmMain : Form
    {
        Helper help;
        public frmMain()
        {
            InitializeComponent();
            help = new Helper();
        }
        void Refresh()
        {
            listView1.Items.Clear();
            string query = @"SELECT o.OrderID,o.OrderDate,o.RequiredDate,c.CompanyName,e.FirstName+' '+e.LastName AS Employee,o.ShipAddress + ' /' + o.ShipCity + ' /' + o.ShipCountry AS ShipAddress,o.Freight,DATEDIFF(DAY,RequiredDate,ShippedDate) AS DelayedDay FROM Orders o JOIN Customers c ON c.CustomerID = o.CustomerID JOIN Employees e ON e.EmployeeID = o.EmployeeID";
            help.FillWithQuery(query, listView1);
        }         // ORDERS TABLOSUNU LİSTEYE AKTAR SÜREKLİ YENİLEME GÖREVİ..
        private void Form1_Load(object sender, EventArgs e)
        {
            Refresh();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            frmAddOrder frm = new frmAddOrder();
            frm.ShowDialog();
            Refresh();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
