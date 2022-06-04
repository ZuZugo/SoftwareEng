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
using System.Collections;


namespace WindowsFormsApp1
{
    public partial class frmGoodsReceived : Form
    {   
        
        DataTable dt3 = null;
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
        int count = 0;
        List<ReceivedGoods> list = new List<ReceivedGoods>();
        public frmGoodsReceived()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(textBox4.Text != null)
            {
                SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
                SqlCommand cmd3 = null;
                bool flag = false;
                String query = null;
                //DataTable dt3 = LoadData();
                if (dt3 == null)
                {
                    count = 0;
                    dt3 = LoadData();
                }
                ArrayList row = new ArrayList();
                count++;
                row.Add(count);
                row.Add(txtProduct.Text);
                row.Add(int.Parse(txtQuantity.Text));
                row.Add(Convert.ToDecimal(txtUnitPrice.Text));


                dt3.Rows.Add(row.ToArray());
                dataGridView1.DataSource = dt3;

                conn.Open();
                SqlCommand cmd2 = new SqlCommand();
                cmd2.CommandText = "select * from [Warehouse]";
                cmd2.Connection = conn;
                SqlDataReader rd = cmd2.ExecuteReader();
                String itemInData = null;
                while (rd.Read())
                {
                    if (rd[1].ToString() == null)
                    {
                        break;
                    }
                    if (rd[1].ToString() == txtProduct.Text)
                    {
                        flag = true;
                        itemInData = rd[2].ToString();
                        break;
                    }
                }

                conn2.Open();
                if (flag == false)
                {

                    query = "INSERT[dbo].[Warehouse]( [Item], [Quantity], [Price]) " +
                               "VALUES('" + txtProduct.Text + "'," + txtQuantity.Text + "," + txtUnitPrice.Text + ")";
                    cmd3 = new SqlCommand(query, conn2);
                    cmd3.ExecuteNonQuery();
                    MessageBox.Show("OK");
                }
                else
                {
                    itemInData = (int.Parse(txtQuantity.Text) + int.Parse(itemInData)).ToString();
                    query = "update [dbo].[Warehouse] set [Quantity] = " + itemInData + " where item = '" + txtProduct.Text + "'";
                    cmd3 = new SqlCommand(query, conn2);
                    cmd3.ExecuteNonQuery();
                    MessageBox.Show("OK");
                }
                conn2.Close();
                conn.Close();




                //Take new
                query = "select top 1 percent GoodID, Quantity from Warehouse ORDER BY GoodID DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                list.Add(new ReceivedGoods(dt.Rows[0].Field<int>(0), dt.Rows[0].Field<int>(1), Convert.ToDecimal(txtUnitPrice.Text)));
            }
            else
            {
                MessageBox.Show("Input Mã đơn");
            }
            
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            conn.Open();
            String sql = "select No from Warehouse_Receipt where " +
                         "No='" + textBox4.Text + "'";

            
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();
            adapter.Fill(dt);
            //------------------------
            

            if (dt.Rows.Count > 0)
            {   
                dataGridView1.DataSource = LoadData();
                dt3 = LoadData();
                dataGridView1.Refresh();
            }
            else
            {
                MessageBox.Show("Mã đơn không tồn tại");
            }
            conn.Close();
        }

        private DataTable LoadData()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
           
            String sql2 = "select Received_Goods.GoodID, Warehouse.Item, Received_Goods.Quantity, Received_Goods.Price " +
                " from warehouse, Received_Goods where Received_Goods.GoodID = Warehouse.GoodID and Warehouse.GoodID  in (" +
                                "select Received_Goods.GoodID from Received_Goods " +
                                "inner join Warehouse_Receipt on Received_Goods.No = Warehouse_Receipt.No " +
                                "where Warehouse_Receipt.No='" + textBox4.Text + "'" +
                           ")";
            SqlCommand cmd2 = new SqlCommand(sql2, conn);
            SqlDataAdapter adapter2 = new SqlDataAdapter(cmd2);

            DataTable dt2 = new DataTable();
            adapter2.Fill(dt2);
            return dt2;
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            
            DataTable dt = new DataTable();
            String getDateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            
            String sql2 = "INSERT[dbo].[Warehouse_Receipt]([No], [CreateTime]) VALUES(" +textBox4.Text +",'" + getDateTime +"')";
            String sql3 = "INSERT[dbo].[Received_Goods]([GoodID], [No], [Quantity], [Price]) " +
                          "VALUES(@GoodID, @No, @Quantity, @Price)";
                           
            String query = null;
           
            conn.Open();
            SqlCommand cmd2 = null;

            cmd2 = new SqlCommand(sql2, conn);
            try
            {
                cmd2.ExecuteNonQuery();

                cmd2 = new SqlCommand(sql3, conn);
                using (SqlCommand cmd = new SqlCommand(sql3, conn))
                {
                    cmd.Parameters.Add("@No", SqlDbType.Int).Value = textBox4.Text;
                    cmd.Parameters.Add("@Quantity", SqlDbType.Int);
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal);
                    cmd.Parameters.Add("@GoodID", SqlDbType.Int);

                    foreach (var lst in list)
                    {
                        cmd.Parameters["@GoodID"].Value = lst.GoodID;
                        cmd.Parameters["@Price"].Value = lst.Price;
                        cmd.Parameters["@Quantity"].Value = lst.Quantity;
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("OK");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Mã đơn đã tồn tại");
            }
           

            conn.Close();
            
           

           
            
        }

        private void frmGoodsReceived_Load(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
