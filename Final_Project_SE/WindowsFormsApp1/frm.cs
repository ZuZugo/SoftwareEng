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
    public partial class frm : Form
    {
        OleDbCommand cmd;
        OleDbDataAdapter daOrder, daWarehouse;
        private OleDbCommandBuilder oleCommandBuilder = null;
        private BindingSource bindingSource = null;
        List<int> list = new List<int>();
        DataTable dtOrder = new DataTable();
        DataTable dtWarehouse = new DataTable();
        List<TotalPrice> PriceDelivery = new List<TotalPrice>(); 
        String strConn = ConfigurationManager.ConnectionStrings["MyConnOleDB"].ConnectionString;

        public frm()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frm_Load(object sender, EventArgs e)
        {
            DataBind();
            LoadData();
        }

        private void DataBind()
        {

            DataTable dt = new DataTable();

            OleDbConnection conn = new OleDbConnection(strConn);

            String sSQL = "select  [Order].OrderID, [Customer].CustomerID, [Customer].Name, [Order].Paid, [Order].[Status] from  [Customer], [Order] " +
                           "where Customer.CustomerID = [Order].CustomerID ";
            cmd = conn.CreateCommand();
            cmd.CommandText = sSQL;
            try
            {
                OleDbDataAdapter da = new OleDbDataAdapter(sSQL, conn);
                oleCommandBuilder = new OleDbCommandBuilder(da);

                da.Fill(dt);
                this.daOrder = da;
                this.dtOrder = dt;

                bindingSource = new BindingSource { DataSource = dt };
                dataGridViewOrder.DataSource = bindingSource;
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridViewOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
            dataGridViewOrder.Refresh();
            dataGridViewWarehouse.Refresh();
        }

        private void dataGridViewWarehouse_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            DataTable dt = new DataTable();
            String sSQL = "update [dbo].[Order] set [Status] = '" + txtStatus.Text + "' where OrderID = " + txtOrderID.Text + "";

            conn.Open();
            SqlCommand cmd2 = null;
            cmd2 = new SqlCommand(sSQL, conn);
            cmd2.ExecuteNonQuery();
            list.Add(int.Parse(txtOrderID.Text));
            MessageBox.Show("Thành công");
            conn.Close();

        }

        private void btnDelivery_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConn"].ConnectionString);
            SqlCommand cmd = null;

            foreach (var lst in list)
            {
                String query = "select Quantity, Price from [Detail_Order] where [OrderID] = " + lst;
                cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataTable dt1 = new DataTable();
                adapter.Fill(dt1);
                int countData = dt1.Rows.Count;
                while(countData > 0)
                {
                    countData--;
                    int i = 0;
                    
                    PriceDelivery.Add(new TotalPrice(dt1.Rows[countData].Field<int>(0), dt1.Rows[countData].Field<Decimal>(1)));
                    
                }
            }
            Decimal sum = 0;
            foreach(var x in PriceDelivery)
            {
                sum += x.CalTotalPrice();
                
            }


            
            String sql3 = "INSERT[dbo].[Delivery_Note]([DeliveryID],[CreateTime], [TotalPrice], [OrderID]) " +
                          "VALUES(@DeliveryID, @CreateTime, @TotalPrice, @OrderID)";
            String getDateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            conn.Open();
            using ( cmd = new SqlCommand(sql3, conn))
            {
                cmd.Parameters.Add("@DeliveryID", SqlDbType.Int).Value = txtDelivery.Text;
                cmd.Parameters.Add("@CreateTime", SqlDbType.DateTime).Value = getDateTime;
                cmd.Parameters.Add("@TotalPrice", SqlDbType.Decimal).Value = sum;
                cmd.Parameters.Add("@OrderID", SqlDbType.Int);

                foreach (var lst in list)
                {
                    // MessageBox.Show(lst.ToString());
                    cmd.Parameters["@OrderID"].Value = lst;
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("OK");
            }
            conn.Close();            
        }

        private void LoadData()
        {

            try
            {
                DataTable dt = new DataTable();

                int index = dataGridViewOrder.SelectedCells[0].RowIndex;
                if (index < 0 || index >= dataGridViewOrder.RowCount)
                {
                    MessageBox.Show("Please select a department first");
                    return;
                }

                DataGridViewRow row = dataGridViewOrder.Rows[index];
                int iDNo = int.Parse(row.Cells[0].Value.ToString());


                OleDbConnection conn = new OleDbConnection(strConn);

                //String sSQL = "select * from Employee where DNo=" + iDNo;
                String sSQL = "select [Warehouse].GoodID, [Warehouse].Item, [Warehouse].Price, [Detail_Order].Quantity from [Order], [Detail_Order], [Warehouse] " +
                              "where[Order].OrderID = Detail_Order.OrderID and[Detail_Order].GoodID = [Warehouse].GoodID " +
                                "and[Order].OrderID = " + iDNo;
                cmd = conn.CreateCommand();
                cmd.CommandText = sSQL;


                try
                {
                    OleDbDataAdapter da = new OleDbDataAdapter(sSQL, conn);
                    oleCommandBuilder = new OleDbCommandBuilder(da);

                    da.Fill(dt);
                    this.daWarehouse = da;
                    this.dtWarehouse = dt;

                    bindingSource = new BindingSource { DataSource = dt };
                    dataGridViewWarehouse.DataSource = bindingSource;
                }
                catch (Exception ex)
                {

                }

                dataGridViewWarehouse.Refresh();
            }
            catch (Exception ex)
            {
            }
        
        }


       



    }
}
