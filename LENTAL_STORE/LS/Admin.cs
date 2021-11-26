using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace LENTAL_STORE.LS
{
    public partial class Admin : UserControl
    {
        DataTable table;
        string[] del;
        int k = 0;
        
        public Admin()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel9.Visible = true;
            button9.Click += back;
            button8.Click += back;

            panel8.Dock = DockStyle.Fill;
            panel2.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.Fill;
            panel4.Dock = DockStyle.Fill;
            panel5.Dock = DockStyle.Fill;
            panel6.Dock = DockStyle.Fill;
            panel7.Dock = DockStyle.Fill;
            panel9.Dock = DockStyle.Fill;

            System.Windows.Forms.DataVisualization.Charting.ChartArea CA = chart1.ChartAreas[0];
            CA.CursorX.IsUserEnabled = true;
            CA.AxisX.ScaleView.Zoom(0, 10);

            label15.ForeColor = Color.FromArgb(5, 21, 64);
            label16.ForeColor = Color.FromArgb(5, 21, 64);
            label17.ForeColor = Color.FromArgb(5, 21, 64);
            label15.ForeColor = Color.FromArgb(5, 21, 64);
            label16.ForeColor = Color.FromArgb(5, 21, 64);
            label17.ForeColor = Color.FromArgb(5, 21, 64);
            label18.ForeColor = Color.FromArgb(5, 21, 64);
            label10.ForeColor = Color.FromArgb(5, 21, 64);

            label19.ForeColor = Color.FromArgb(5, 21, 64);
            label20.ForeColor = Color.FromArgb(5, 21, 64);

            label19.Click += button16_Click;
            label20.Click += button22_Click;







        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        private void back(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string image_file = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = @"D:\";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(dialog.FileName);
                pictureBox1.Tag = dialog.FileName;
                
            }
            else if (dialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            


        }

        private void button6_Click(object sender, EventArgs e)
        {
            string item_name, item_size, item_color, item_loc, item_ql, item_cate, item_price;
            item_name = textBox1.Text;
            item_size = textBox2.Text;
            item_color = textBox3.Text;
            item_loc = textBox4.Text;
            item_ql = comboBox1.SelectedItem.ToString();
            item_cate = textBox5.Text;
            item_price = textBox6.Text;

            FileStream fs = new FileStream(pictureBox1.Tag.ToString(), FileMode.Open, FileAccess.Read);
            byte[] bImage = new byte[fs.Length];
            fs.Read(bImage, 0, (int)fs.Length);

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM_CATE WHERE ITEM_CATE_NAME = '" + item_cate + "'";
            OracleCommand com2 = new OracleCommand("", conn);
            com2.CommandText = "INSERT INTO ITEM_CATE VALUES(ITEM_CATE_SEQ.nextval, '" + item_cate + "')";

            object cate = com1.ExecuteScalar();

            if(cate == null)
            {
                com2.ExecuteNonQuery();
            }

            OracleCommand com4 = new OracleCommand("", conn);
            com4.CommandText = "SELECT ITEM_CATE_NUM FROM ITEM_CATE WHERE ITEM_CATE_NAME = '" + item_cate + "'";
            string item_caten = com4.ExecuteScalar().ToString();

            OracleCommand com3 = new OracleCommand("", conn);
            com3.CommandText = "INSERT INTO ITEM VALUES(ITEM_SEQ.nextval, '" + item_name + "','" + item_size + "','" + item_color + "','" + item_loc + "','" + item_ql + "'," + ":BlobParameter" + ",'" + item_caten + "','" + item_price + "','0')";

            OracleParameter blobParameter = new OracleParameter();
            blobParameter.OracleDbType = OracleDbType.Blob;
            blobParameter.ParameterName = "BlobParameter";
            blobParameter.Value = bImage;

            com3.Parameters.Add(blobParameter);
            com3.ExecuteNonQuery();

            MessageBox.Show("추가되었습니다.");



        }

        private void chartinit(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            
            com1.CommandText = "select * from (SELECT sum(statistic_lentalcost) as cost ,TO_CHAR(STATISTIC_DATE,'yyyy-MM-dd') as day from statistic group by to_char(statistic_date, 'yyyy-MM-dd') order by to_char(statistic_date, 'yyyy-MM-dd') desc) where rownum <= 5";

            OracleDataReader rdr = com1.ExecuteReader();


            if (checkBox1.Checked)
            {
                while (rdr.Read())
                {
                    chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                }
            }
            else
            {

                int i = 0;
                while (rdr.Read())
                {
                    string nd = rdr["day"].ToString();
                    while (nd != ((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"))
                    {
                        chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                        i++;
                        if (i >= 5) break;
                    }
                    if (i >= 5) break;
                    chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                    i++;

                }

            }
        }

        

        private void button4_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel4.Visible = false;
            panel3.Visible = false;
            panel9.Visible = true;
            chartinit(sender, e);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);


            int s = comboBox2.SelectedIndex;
            string ymd = "";
            if (s == 0)
            {
                chartinit(sender, e);
                return;
            }
            else if (s == 1)
            {
                ymd = "yyyy-mm";
            }
            else
            {
                ymd = "yyyy";
            }
            com1.CommandText = "select * from (SELECT sum(statistic_lentalcost) as cost ,TO_CHAR(STATISTIC_DATE,'" + ymd + "') as day from statistic group by to_char(statistic_date, '" + ymd + "') order by to_char(statistic_date, '" + ymd + "') desc) where rownum <= 5";

            OracleDataReader rdr = com1.ExecuteReader();

            while (rdr.Read())
            {
                chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            chartinit(sender, e);

        }

        private void chart2init()
        {
            
            Button[] bt = new Button[100];

            
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT ITEM_CATE_NAME FROM ITEM_CATE";

            OracleDataReader rdr = com1.ExecuteReader();

            int t = 0;
            int HEIGHT = 100;
            int WIDTH = 100;
            while(rdr.Read())
            {
                string catename = rdr["ITEM_CATE_NAME"].ToString();
                bt[t] = new Button();
                panel4.Controls.Add(bt[t]);
                bt[t].Text = catename;
                bt[t].Location = new System.Drawing.Point(WIDTH, HEIGHT);
                
                WIDTH += 80;
                bt[t].Tag = catename;
                bt[t].Click += itemchart;
                t++;
            }

            

        }

        private void button10_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = true;
            chart2init();
        }

        private void itemchart(object sender, EventArgs e)
        {
            string catename = ((Button)sender).Tag.ToString();

            if(((Button)sender).BackColor != Color.Blue)
            {
                ((Button)sender).BackColor = Color.Blue;
                chart2.Series.Add(catename);

                OracleConnection conn = Form1.oracleconnect();
                OracleCommand com1 = new OracleCommand("", conn);

                com1.CommandText = "SELECT TO_CHAR(STATISTIC_DATE, 'YYYY-mm-DD') as day,COUNT(*) as n FROM (SELECT ITEM_NUM FROM ITEM, ITEM_CATE WHERE ITEM.ITEM_CATE = item_cate.item_cate_num aND item_cate.item_cate_name = '" + catename + "') PP, STATISTIC WHERE PP.ITEM_NUM = statistic.statistic_itemitemnum AND STATISTIC_TYPE=1 GROUP BY TO_CHAR(STATISTIC_DATE, 'YYYY-mm-DD') order by TO_CHAR(STATISTIC_DATE, 'YYYY-mm-DD') desc";
                OracleDataReader rdr = com1.ExecuteReader();

                /*while (rdr.Read())
                {
                    chart2.Series[catename].Points.AddXY(rdr["day"], rdr["n"]);
                }*/

                int i = 0;
                while (rdr.Read())
                {
                    string nd = rdr["day"].ToString();
                    while (nd != ((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"))
                    {
                        chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                        i++;
                        if (i >= 5) break;
                    }
                    if (i >= 5) break;
                    chart2.Series[catename].Points.AddXY(rdr["day"], rdr["n"]);
                    i++;

                }
            }
            else
            {
                ((Button)sender).BackColor = Color.White;
                chart2.Series.Remove(chart2.Series[catename]);
            }
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            for (int ix = flowLayoutPanel6.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel6.Controls[ix].Dispose();
            }

            view(panel5);

            DataTable table = new DataTable();

            table.Columns.Add("ID");
            table.Columns.Add("상품명");
            table.Columns.Add("날짜");
            table.Columns.Add("대여일수");
            table.Columns.Add("반납일");
            table.Columns.Add("분류");

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM";
            com2.CommandText = "SELECT * FROM ITEM_CATE";

            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while(rdr2.Read())
            {
                Label lb = new Label();
                lb.AutoSize = true;
                lb.ForeColor = Color.FromArgb(5, 21, 64);

                lb.Text = rdr2["ITEM_CATE_NAME"].ToString();
                lb.Tag = rdr2["ITEM_CATE_NUM"].ToString();

                lb.MinimumSize = new Size(0, 0);
                lb.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                lb.Click += cate_filter;
                flowLayoutPanel6.Controls.Add(lb);
            }

            while(rdr.Read())
            {
                if(rdr["STATISTIC_EXDATE"].ToString() != "")
                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), rdr["STATISTIC_ITEMCOUNT"], Convert.ToDateTime(rdr["STATISTIC_EXDATE"]).ToString("yy-MM-dd"), "대여");
                else
                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), "-", "-", "반납");
            }

            dataGridView1.DataSource = table;
            dataGridView1.BackgroundColor = Color.FromArgb(238, 242, 247);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
            dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 13);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.RowHeadersVisible = false;
        }

        private void button5_Click_2()
        {

            view(panel5);

            DataTable table = new DataTable();

            table.Columns.Add("ID");
            table.Columns.Add("상품명");
            table.Columns.Add("날짜");
            table.Columns.Add("대여일수");
            table.Columns.Add("반납일");
            table.Columns.Add("분류");

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            string nt = "";

            for (int ix = flowLayoutPanel6.Controls.Count - 1; ix >= 0; ix--)
            {
                if(flowLayoutPanel6.Controls[ix].BackColor == Color.Red)
                {
                    nt += "AND STATISTIC_ITEMITEMNUM = '" +flowLayoutPanel6.Controls[ix].Tag +"'";
                }
            }

            com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM " + nt;
            com2.CommandText = "SELECT * FROM ITEM_CATE";

            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr["STATISTIC_EXDATE"].ToString() != "")
                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), rdr["STATISTIC_ITEMCOUNT"], Convert.ToDateTime(rdr["STATISTIC_EXDATE"]).ToString("yy-MM-dd"), "대여");
                else
                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), "-", "-", "반납");
            }

            dataGridView1.DataSource = table;
            dataGridView1.BackgroundColor = Color.FromArgb(238, 242, 247);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
            dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 13);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.RowHeadersVisible = false;
        }


        private void cate_filter(object sender, EventArgs e)
        {
            if(((Label)sender).BackColor == Color.Red)
            {
                for (int ix = flowLayoutPanel6.Controls.Count - 1; ix >= 0; ix--)
                {
                    flowLayoutPanel6.Controls[ix].BackColor = Color.FromArgb(238, 242, 247);
                }
            }
            else
            {
                for (int ix = flowLayoutPanel6.Controls.Count - 1; ix >= 0; ix--)
                {
                    flowLayoutPanel6.Controls[ix].BackColor = Color.FromArgb(238, 242, 247);
                }
                ((Label)sender).BackColor = Color.Red;
            }

            button5_Click_2();
                    
                
        }

        private void button12_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            view(panel6);

            for (int ix = flowLayoutPanel4.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel4.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel5.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel5.Controls[ix].Dispose();
            }

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT NVL(PI.USERID,0) AS I,PI.CONTENT, FN.USERID, FN.P, FN.P2 FROM BLACKLIST PI RIGHT OUTER JOIN (SELECT O1.USERID, NVL(P,0) as P, NVL(P2,0) as P2 FROM (SELECT USERS.USERID, P FROM USERS LEFT OUTER JOIN (SELECT USERID, COUNT(*) AS P FROM USERS, LENTAL WHERE USERS.USERID = lental.lental_userid AND LENTAL_EXPIRATION < '" + System.DateTime.Now.ToString("yyyy-MM-dd")+ "' GROUP BY USERID)PP ON users.userid = PP.USERID) O1,(SELECT USERS.USERID, P2 FROM USERS LEFT OUTER JOIN(SELECT STATISTIC_USERID, COUNT(*) AS P2 FROM STATISTIC WHERE STATISTIC_TYPE = 2 AND NOT STATISTIC_LENTALCOST = 0 GROUP BY STATISTIC_USERID) KK ON USERS.USERID = KK.STATISTIC_USERID) O2 WHERE O1.USERID = O2.USERID) FN ON PI.userid = FN.USERID";
             OracleDataReader rdr =   com1.ExecuteReader();

            while(rdr.Read())
            {
                Label lb = new Label();
                Label bc = new Label();

                lb.ForeColor = Color.FromArgb(5, 21, 64);
                lb.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(180, 0);

                bc.ForeColor = Color.FromArgb(5, 21, 64);
                bc.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                bc.AutoSize = false;
                bc.TextAlign = ContentAlignment.MiddleRight;
                bc.Size = new Size(50, lb.Height);

                int cnt = Convert.ToInt32(rdr["P"]) + Convert.ToInt32(rdr["P2"]);
                lb.Text = rdr["USERID"].ToString();
                bc.Text = cnt.ToString();

                lb.Click += blacklabelclick;
                
                if (rdr["I"].ToString() != "0")
                {
                    flowLayoutPanel4.Controls.Add(lb);
                    flowLayoutPanel4.Controls.Add(bc);
                }
                else
                {
                    flowLayoutPanel5.Controls.Add(lb);
                    flowLayoutPanel5.Controls.Add(bc);
                }
                
                
            }


        }

        private void blacklabelclick(object sender, EventArgs e)
        {


                if(((Label)sender).BackColor != Color.Red)
                {
                    ((Label)sender).BackColor = Color.Red;
                }
                else
                {
                ((Label)sender).BackColor = Color.FromArgb(238, 242, 247);
                }
            

 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            del = new string[100];
            k = 0;
            
            panel7.Visible = true;

            table = new DataTable();

            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();

            dataGridView2.ColumnCount = 8;
            dataGridView2.Columns[0].Name = "상품ID";
            dataGridView2.Columns[1].Name = "상품명";
            dataGridView2.Columns[2].Name = "상품사이즈";
            dataGridView2.Columns[3].Name = "상품색";
            dataGridView2.Columns[4].Name = "상품위치";
            dataGridView2.Columns[5].Name = "상품품질";
            dataGridView2.Columns[6].Name = "상품분류";
            dataGridView2.Columns[7].Name = "상품가격";

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM ITEM";

            OracleDataReader rdr = com1.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr["ITEM_STATUS"].ToString() != "3")
                {
                    dataGridView2.Rows.Add(rdr["ITEM_NUM"], rdr["ITEM_NAME"], rdr["ITEM_SIZE"], rdr["ITEM_COLOR"], rdr["ITEM_LOCATION"], rdr["ITEM_QUALITY"], rdr["ITEM_CATE"], rdr["ITEM_PRICE"]);
                }
                
            }

            table = GetDataGridViewAsDataTable(dataGridView2);


        }

        public static DataTable GetDataGridViewAsDataTable(DataGridView _DataGridView)
        {
            try
            {
                if (_DataGridView.ColumnCount == 0)
                    return null;
                DataTable dtSource = new DataTable();
                //////create columns
                foreach (DataGridViewColumn col in _DataGridView.Columns)
                {
                    if (col.ValueType == null)
                        dtSource.Columns.Add(col.Name, typeof(string));
                    else
                        dtSource.Columns.Add(col.Name, col.ValueType);
                    dtSource.Columns[col.Name].Caption = col.HeaderText;
                }
                ///////insert row data
                foreach (DataGridViewRow row in _DataGridView.Rows)
                {
                    DataRow drNewRow = dtSource.NewRow();
                    foreach (DataColumn col in dtSource.Columns)
                    {
                        drNewRow[col.ColumnName] = row.Cells[col.ColumnName].Value;
                    }
                    dtSource.Rows.Add(drNewRow);
                }
                return dtSource;
            }
            catch
            {
                return null;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            DataTable dtChanges = GetDataGridViewAsDataTable(dataGridView2);

            

            string update_query = string.Empty;

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            if (dtChanges != null)
            {
                for (int i = 0; i < dtChanges.Rows.Count; ++i)
                {
                    update_query = "UPDATE ITEM SET ITEM_NAME = '#ITEM_NAME', ITEM_SIZE = '#ITEM_SIZE' , ITEM_COLOR='#ITEM_COLOR', ITEM_LOCATION='#ITEM_LOCATION' , ITEM_QUALITY='#ITEM_QUALITY', ITEM_CATE='#ITEM_CATE', ITEM_PRICE='#ITEM_PRICE' WHERE ITEM_NUM='#ITEM_NUM'";


                    update_query = update_query.Replace("#ITEM_NAME", dtChanges.Rows[i]["상품명"].ToString());
                    update_query = update_query.Replace("#ITEM_SIZE", dtChanges.Rows[i]["상품사이즈"].ToString());
                    update_query = update_query.Replace("#ITEM_COLOR", dtChanges.Rows[i]["상품색"].ToString());
                    update_query = update_query.Replace("#ITEM_LOCATION", dtChanges.Rows[i]["상품위치"].ToString());
                    update_query = update_query.Replace("#ITEM_QUALITY", dtChanges.Rows[i]["상품품질"].ToString());
                    update_query = update_query.Replace("#ITEM_CATE", dtChanges.Rows[i]["상품분류"].ToString());
                    update_query = update_query.Replace("#ITEM_PRICE", dtChanges.Rows[i]["상품가격"].ToString());
                    update_query = update_query.Replace("#ITEM_NUM", dtChanges.Rows[i]["상품ID"].ToString());

                    com1.CommandText = update_query;
                    com1.ExecuteNonQuery();
                }
            }

            for(int i = 0; i < 100; ++i)
            {
                if(del[i] != null)
                {
                    com1.CommandText = "UPDATE ITEM SET ITEM_STATUS = 3 WHERE ITEM_NUM = '" +del[i] +"'";
                    com1.ExecuteNonQuery();
                }
            }
        
        }

        public void uiBtn_Delete_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)

            {
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                if (dataGridView2.Rows[i].Selected == true)
                {
                    string t = (dataGridView2.Rows[i].Cells[0].Value).ToString();
                    del[k] = t;
                    k++;
                    dataGridView2.Rows.Remove(dataGridView2.Rows[i]);
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            uiBtn_Delete_Click(sender, e);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            panel7.Visible = false;
            
        }

        private void button16_Click(object sender, EventArgs e)
        {

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            
            

            for (int ix = flowLayoutPanel5.Controls.Count - 1; ix >= 0; ix--)
            {
                if(flowLayoutPanel5.Controls[ix].BackColor == Color.Red)
                {
                    string ID = flowLayoutPanel5.Controls[ix].Text;
                    com1.CommandText = "INSERT INTO BLACKLIST VALUES('" + ID + "', 'a')";
                    com1.ExecuteNonQuery();
                }
            }

            button3_Click(sender, e);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            
            panel8.Visible = true;
            //flowLayoutPanel1.AutoSize = true;
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM ITEM WHERE NOT ITEM_STATUS = 3";

            OracleDataReader rdr = com1.ExecuteReader();

            while (rdr.Read())
            {
                Button bt = new Button();
                bt.Text = rdr["ITEM_NAME"].ToString();
                bt.Tag = rdr["ITEM_NUM"].ToString();
                bt.AutoSize = true;
                bt.Click += bc;

                flowLayoutPanel1.Controls.Add(bt);
                
            }

        }

        private void bc(object sender, EventArgs e)
        {

            for (int ix = flowLayoutPanel2.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel2.Controls[ix].Dispose();
            }
            string t = ((Button)sender).Tag.ToString();

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM LENTAL WHERE LENTAL_ITEM = '" + t +"'";
            com2.CommandText = "SELECT * FROM STATISTIC WHERE STATISTIC_ITEMITEMNUM = '" + t + "'";

            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            label11.Text = "";

            while(rdr.Read())
            {
                label11.Text = "현재 대여자:" + rdr["LENTAL_USERID"].ToString();
            }
            while(rdr2.Read())
            {
                Panel pn = new Panel();
                pn.AutoSize = true;
                pn.BackColor = Color.Gainsboro;

                Label lb = new Label();
                lb.AutoSize = true;
                lb.Text = rdr2["STATISTIC_USERID"].ToString() + "\n"+rdr2["STATISTIC_DATE"].ToString() + rdr2["STATISTIC_ITEMCOUNT"].ToString() +"\n" +rdr2["STATISTIC_LENTALCOST"].ToString() + rdr2["STATISTIC_TYPE"].ToString();

                pn.Controls.Add(lb);
                flowLayoutPanel2.Controls.Add(pn);

            }
            
        }

        private void button18_Click(object sender, EventArgs e)
        {
            
            panel9.Visible = true;

            chart3.Series[0].Points.Clear();
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);


            com1.CommandText = "select * from (SELECT sum(statistic_lentalcost) as cost ,TO_CHAR(STATISTIC_DATE,'yyyy-MM-dd') as day from statistic group by to_char(statistic_date, 'yyyy-MM-dd') order by to_char(statistic_date, 'yyyy-MM-dd') desc) where rownum <= 5";
            com2.CommandText = "SELECT * FROM STATISTIC,ITEM WHERE statistic.statistic_itemitemnum = item.item_num AND STATISTIC_DATE = '"+System.DateTime.Now.ToString("yyyy-MM-dd")+"' AND NOT STATISTIC_LENTALCOST = 0";
            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();


            

            int i = 0;
            while (rdr.Read())
            {
                string nd = rdr["day"].ToString();
                while (nd != ((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"))
                {
                    chart3.Series[0].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                    i++;
                    if (i >= 5) break;
                }
                if (i >= 5) break;
                chart3.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                i++;

            }

            while(rdr2.Read())
            {
                Label lb = new Label();
                lb.Text = rdr2["ITEM_NAME"].ToString() +" - " +rdr2["STATISTIC_LENTALCOST"].ToString();
                lb.AutoSize = true;
                flowLayoutPanel3.Controls.Add(lb);
            }



            

        }

        public void homeview()
        {

            view(panel9);

            chart3.Legends[0].Enabled = false;
            chart4.Legends[0].Enabled = false;
            chart3.BackColor = Color.FromArgb(238, 242, 247);
            chart4.BackColor = Color.FromArgb(238, 242, 247);

            chart3.Series[0].Points.Clear();
            chart3.Series[0].Color = Color.FromArgb(5, 21, 64);
            chart3.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("휴먼엑스포", 8);
            chart3.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial 3", 10);

            chart4.Series[0].Points.Clear();
            chart4.Series[0].Color = Color.FromArgb(5, 21, 64);
            chart4.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("휴먼엑스포", 8);
            chart4.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Arial 3", 10);



            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);

            com1.CommandText = "SELECT ITEM_NAME, COST FROM(SELECT STATISTIC_ITEMITEMNUM AS ITN, SUM(STATISTIC_LENTALCOST) AS COST FROM STATISTIC WHERE STATISTIC_dATE > '"+ ((System.DateTime.Today).AddDays(-10)).ToString("yyyy-MM-dd") + "' GROUP BY statistic_itemitemnum) PP, ITEM WHERE PP.ITN = ITEM.ITEM_NUM order by cost asc";
            com2.CommandText = "SELECT * FROM STATISTIC,ITEM WHERE statistic.statistic_itemitemnum = item.item_num AND STATISTIC_DATE = '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' AND NOT STATISTIC_LENTALCOST = 0";
            com3.CommandText = "SELECT STATISTIC_dATE as day, SUM(statistic_lentalcost) as cost FROM STATISTIC GROUP BY STATISTIC_DATE HAVING STATISTIC_dATE > '" + ((System.DateTime.Today).AddDays(-10)).ToString("yyyy-MM-dd") + "' ORDER BY statistic_date ";
            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();
            OracleDataReader rdr3 = com3.ExecuteReader();

            int i = 10;
            int rdr3maxcost = -1;



            while (rdr3.Read())
            {
                DateTime nd = Convert.ToDateTime(rdr3["day"].ToString()).Date;

                    while (i>= 0)
                    {
                        DateTime today = (System.DateTime.Today).AddDays(-1 * i);
                        if(nd < today)
                        {
                            break;
                        }
                        if (nd == today)
                        {
                            chart3.Series[0].Points.AddXY(nd.ToString("MM-dd"), rdr3["cost"]);
                            rdr3maxcost = System.Math.Max(rdr3maxcost, Convert.ToInt32(rdr3["cost"]));
                            i--;
                            break;
                        }
                        else
                        {
                            chart3.Series[0].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("MM-dd"), 0);
                            i--;

                        }
                    }
                    
            }

            while(i>= 0)
            {
                DateTime today = (System.DateTime.Today).AddDays(-1 * i);
                chart3.Series[0].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("MM-dd"), 0);
                i--;
            }

            System.Windows.Forms.DataVisualization.Charting.Axis ay = chart3.ChartAreas[0].AxisY;
            ay.Minimum = 0;
            ay.Maximum = rdr3maxcost+100000;

            System.Windows.Forms.DataVisualization.Charting.StripLine sl0 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            sl0.BackColor = Color.FromArgb(238, 242, 247);
            sl0.StripWidth = rdr3maxcost+100000;
            sl0.IntervalOffset = 0;



            chart3.ChartAreas[0].AxisY.StripLines.Add(sl0);




            int rdr1maxcost = -1;
            while (rdr.Read())
            {
                chart4.Series[0].Points.AddXY(rdr["ITEM_NAME"], rdr["COST"]);
                rdr1maxcost = System.Math.Max(rdr1maxcost, Convert.ToInt32(rdr["cost"]));
            }

            System.Windows.Forms.DataVisualization.Charting.Axis at = chart4.ChartAreas[0].AxisY;
            at.Minimum = 0;
            at.Maximum = rdr1maxcost + 100000;

            System.Windows.Forms.DataVisualization.Charting.StripLine st = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            st.BackColor = Color.FromArgb(238, 242, 247);
            st.StripWidth = rdr1maxcost + 100000;
            st.IntervalOffset = 0;



            chart4.ChartAreas[0].AxisY.StripLines.Add(st);


            while (rdr2.Read())
            {
                Label lb = new Label();
                lb.Text = rdr2["ITEM_NAME"].ToString() + " - " + rdr2["STATISTIC_LENTALCOST"].ToString();
                lb.AutoSize = true;
                flowLayoutPanel3.Controls.Add(lb);
            }





        }

        private void button19_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
    
        }

        private void button20_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            
        }

        private void button21_Click(object sender, EventArgs e)
        {
            panel6.Visible = false;
            
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            for (int ix = flowLayoutPanel4.Controls.Count - 1; ix >= 0; ix--)
            {
                if (flowLayoutPanel4.Controls[ix].BackColor == Color.Red)
                {
                    string ID = flowLayoutPanel4.Controls[ix].Text;
                    com1.CommandText = "DELETE FROM BLACKLIST WHERE USERID = '" + ID + "'";
                    com1.ExecuteNonQuery();
                }
            }

            button3_Click(sender, e);

        }

        private void view(Panel vp)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel8.Visible = false;
            panel9.Visible = false;

            vp.Visible = true;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);


            com1.CommandText = "select * from (SELECT sum(statistic_lentalcost) as cost ,TO_CHAR(STATISTIC_DATE,'yyyy-MM-dd') as day from statistic group by to_char(statistic_date, 'yyyy-MM-dd') order by to_char(statistic_date, 'yyyy-MM-dd') desc) where DAY > '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' AND DAY < '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'";

            OracleDataReader rdr = com1.ExecuteReader();


            if (checkBox1.Checked)
            {
                while (rdr.Read())
                {
                    chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                }
            }
            else
            {

                int i = 0;
                while (rdr.Read())
                {
                    string nd = rdr["day"].ToString();
                    while (nd != ((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"))
                    {
                        chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                        i++;
                        
                    }
                    
                    chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                    i++;

                }
                while (true)
                {
                    if (((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd") != dateTimePicker1.Value.ToString("yyyy-MM-dd"))
                    {
                        chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                    }
                    else
                    {
                        break;
                    }
                    i++;
                }

            }
        }

        private void label13_Click(object sender, EventArgs e)
        {
            homeview();
        }

        private void label21_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
        }

        private void label22_Click(object sender, EventArgs e)
        {
            button5_Click_1(sender, e);
        }
    }
}
