using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        public Form2()
        {
            InitializeComponent();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            String sSQL = "select TotalPrice from [Delivery_Note] where month(Delivery_Note.CreateTime) = " +
                           txtMonth.Text + " and year(Delivery_Note.CreateTime) = " + txtYear.Text;
            

            conn.Open();
            SqlCommand cmd = new SqlCommand(sSQL, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            int count = dt.Rows.Count;
            
            Decimal sum = 0;
            while (count > 0)
            {   
                count--;
                sum+= dt.Rows[count].Field<Decimal>(0);
            }
            txtSum.Text = sum.ToString();
            
            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnReceived_Click(object sender, EventArgs e)
        {
            conn.Open();
            String sql = "select Warehouse.Item, Detail_Order.Quantity from [Detail_Order], [Warehouse] " +
                          "where[Detail_Order].[GoodID] = [Warehouse].[GoodID] and[Detail_Order].[OrderID]  in ( " +
                           "select Delivery_Note.OrderID from[Delivery_Note] where month(Delivery_Note.CreateTime) = " +
                           txtMonth.Text + " and year(Delivery_Note.CreateTime) = " + txtYear.Text + ")";


            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //------------------------

            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("No data");
            }
            conn.Close();
        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            
            String sql = "select Warehouse.Item, Received_Goods.Quantity from [Received_Goods], [Warehouse] " +
                          " where [Received_Goods].GoodID = [Warehouse].GoodID and  Warehouse.GoodID in ( " +
                           "select Received_Goods.[GoodID] from [Warehouse_Receipt], [Received_Goods] where Warehouse_Receipt.[No] = Received_Goods.[No] and month(Warehouse_Receipt.CreateTime) = " +
                           txtMonth.Text + " and year(Warehouse_Receipt.CreateTime) = " + txtYear.Text + " )";

            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //------------------------

            if (dt.Rows.Count > 0)
            {
                dataGridView2.DataSource = dt;
                dataGridView2.Refresh();
            }
            else
            {
                MessageBox.Show("No data");
            }
            conn.Close();
        }
    }
}
