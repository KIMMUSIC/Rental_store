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
    public delegate void loe(object sender, EventArgs e);
    public partial class Admin : UserControl
    {
        public event loe adminlogout;
        DataTable table;
        string[] del;
        int k = 0;
        Stack<string> s = new Stack<string>();

        public Admin()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel9.Visible = true;

            panel1.Dock = DockStyle.Fill;
            panel2.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.Fill;
            panel4.Dock = DockStyle.Fill;
            panel5.Dock = DockStyle.Fill;
            panel6.Dock = DockStyle.Fill;
            panel7.Dock = DockStyle.Fill;
            panel9.Dock = DockStyle.Fill;
            panel8.Dock = DockStyle.Fill;

            System.Windows.Forms.DataVisualization.Charting.ChartArea CA = chart1.ChartAreas[0];
            CA.CursorX.IsUserEnabled = true;
            CA.AxisX.ScaleView.Zoom(0, 10);

            System.Windows.Forms.DataVisualization.Charting.ChartArea CA2 = chart2.ChartAreas[0];
            CA2.CursorX.IsUserEnabled = true;
            CA2.AxisX.ScaleView.Zoom(0, 10);

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

            label32.ForeColor = Color.FromArgb(5, 21, 64);
            label31.ForeColor = Color.FromArgb(5, 21, 64);

            panel10.BackColor = Color.FromArgb(5, 21, 64);


            label19.Click += button16_Click;
            label20.Click += button22_Click;

            flowLayoutPanel2.AutoScroll = true;
            monthCalendar1.MaxSelectionCount = 30;
            monthCalendar1.BackColor = Color.FromArgb(238, 242, 247);
            monthCalendar1.ForeColor = Color.FromArgb(5, 21, 64);
            monthCalendar1.Size = new Size(50, 50);

  
            dataGridView2.BackgroundColor = Color.FromArgb(238, 242, 247);
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
            dataGridView2.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView2.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 13);
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            flowLayoutPanel3.AutoScroll = true;

            chart6.Series[0].Label = "#PERCENT";
            chart6.Series[0].LegendText = "#VALX";
            chart6.Visible = false;

            chart5.Legends[0].Enabled = false;
            chart5.BackColor = Color.FromArgb(238, 242, 247);
            chart6.BackColor = Color.FromArgb(238, 242, 247);
            chart5.Series[0].IsValueShownAsLabel = true;
            chart5.Series[0].LabelForeColor = Color.FromArgb(5, 21, 64);
            chart6.ChartAreas[0].BackColor = Color.FromArgb(238, 242, 247);
            chart6.Legends[0].BackColor = Color.FromArgb(238, 242, 247);

            chart1.Series[0].IsValueShownAsLabel = true;

            label13.BackColor = Color.FromArgb(200, 206, 235);




        }

        private void button1_Click(object sender, EventArgs e)
        {
            view(panel2);

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";

            comboBox1.SelectedIndex = -1;
            pictureBox1.Image = null;
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
            string item_name, item_size, item_color, item_loc, item_ql, item_cate, item_price, item_count;
            item_name = textBox1.Text;
            item_size = textBox2.Text;
            item_color = textBox3.Text;
            item_loc = textBox4.Text;
            item_ql = comboBox1.SelectedItem.ToString();
            item_cate = textBox5.Text;
            item_price = textBox6.Text;
            item_count = textBox7.Text;

            FileStream fs = new FileStream(pictureBox1.Tag.ToString(), FileMode.Open, FileAccess.Read);
            byte[] bImage = new byte[fs.Length];
            fs.Read(bImage, 0, (int)fs.Length);

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM_CATE WHERE ITEM_CATE_NAME = '" + item_cate + "'";
            OracleCommand com2 = new OracleCommand("", conn);
            com2.CommandText = "INSERT INTO ITEM_CATE VALUES(ITEM_CATE_SEQ.nextval, '" + item_cate + "')";

            object cate = com1.ExecuteScalar();

            if (cate == null)
            {
                com2.ExecuteNonQuery();
            }

            OracleCommand com4 = new OracleCommand("", conn);
            com4.CommandText = "SELECT ITEM_CATE_NUM FROM ITEM_CATE WHERE ITEM_CATE_NAME = '" + item_cate + "'";
            string item_caten = com4.ExecuteScalar().ToString();

            OracleCommand com3 = new OracleCommand("", conn);
            com3.CommandText = "INSERT INTO ITEM VALUES(ITEM_SEQ.nextval, '" + item_name + "','" + item_size + "','" + item_color + "','" + item_loc + "','" + item_ql + "'," + ":BlobParameter" + ",'" + item_caten + "','" + item_price + "','0', '" + item_count+"')";

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

            chart1_view(sender, e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*chartinit(sender, e);*/
            chart1_view(sender, e);

        }

        private void chart2init(object sender, EventArgs e)
        {

            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel1.Controls[ix].Dispose();
            }


            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM ITEM_CATE";

            OracleDataReader rdr = com1.ExecuteReader();


            while (rdr.Read())
            {
                string catename = rdr["ITEM_CATE_NAME"].ToString();
                Label lb = new Label();

                flowLayoutPanel1.Controls.Add(lb);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(140, 0);
                lb.Text = catename;
                lb.ForeColor = Color.FromArgb(5, 21, 64);
                lb.Font =  new Font("휴먼엑스포", 16, FontStyle.Bold);
                lb.Tag = catename;
                lb.Click += ic;
                lb.Margin = new Padding(0, 0, 0, 10);
            }

            itemchart(sender, e);
            



        }

        private void button10_Click(object sender, EventArgs e)
        {
            view(panel4);
            dateTimePicker3.Value = DateTime.Today.AddDays(-5);
            dateTimePicker4.Value = DateTime.Today;
            comboBox3.SelectedIndex = 0;
            chart2init(sender, e);
        }

        private void ic(object sender, EventArgs e)
        {
            if (((Label)sender).BackColor != Color.FromArgb(5, 21, 64))
            {
                ((Label)sender).BackColor = Color.FromArgb(5, 21, 64);
                ((Label)sender).ForeColor = Color.White;
            }
            else
            {
                ((Label)sender).BackColor = Color.FromArgb(238, 242, 247);
                ((Label)sender).ForeColor = Color.FromArgb(5, 21, 64);
            }
            itemchart(sender, e);
        }

        private void itemchart(object sender, EventArgs e)
        {
            

            for(int ix = chart2.Series.Count - 1; ix >= 0; ix--)
            {
                chart2.Series.RemoveAt(ix);
            }
            int cost = -1;

            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
            {
                if (flowLayoutPanel1.Controls[ix].BackColor == Color.FromArgb(5, 21, 64))
                {
                    string catename = flowLayoutPanel1.Controls[ix].Text;




                    chart2.Series.Add(catename);
                    chart2.Series[catename].IsValueShownAsLabel = true;

                    OracleConnection conn = Form1.oracleconnect();
                    OracleCommand com1 = new OracleCommand("", conn);



                    int s = comboBox3.SelectedIndex;
                    string ymd = "";
                    string dtp = "";
                    string dtp2 = "";

                    if (s == 0 || s == -1)
                    {
                        ymd = "yyyy-MM-dd";
                        dtp = dateTimePicker3.Value.ToString("yyyy-MM-dd");
                        dtp2 = dateTimePicker4.Value.ToString("yyyy-MM-dd");
                    }
                    else if (s == 1)
                    {
                        ymd = "yyyy-MM";
                        dtp = dateTimePicker3.Value.ToString("yyyy-MM");
                        dtp2 = dateTimePicker4.Value.ToString("yyyy-MM");
                    }
                    else
                    {
                        ymd = "yyyy";
                        dtp = dateTimePicker3.Value.ToString("yyyy");
                        dtp2 = dateTimePicker4.Value.ToString("yyyy");
                    }



                    com1.CommandText = "SELECT TO_CHAR(STATISTIC_DATE, '" + ymd + "') as day,COUNT(*) as COST FROM (SELECT ITEM_NUM FROM ITEM, ITEM_CATE WHERE ITEM.ITEM_CATE = item_cate.item_cate_num aND item_cate.item_cate_name = '" + catename + "') PP, STATISTIC WHERE PP.ITEM_NUM = statistic.statistic_itemitemnum AND STATISTIC_TYPE=1 AND TO_CHAR(STATISTIC.STATISTIC_DATE, '" + ymd + "') >= '" + dtp + "' AND TO_CHAR(STATISTIC.STATISTIC_DATE, '" + ymd + "') <= '" + dtp2 + "'GROUP BY TO_CHAR(STATISTIC_DATE, '" + ymd + "') order by TO_CHAR(STATISTIC_DATE, '" + ymd + "') desc";
                    OracleDataReader rdr = com1.ExecuteReader();


                    if (s == 0 || s == -1)
                    {
                        if (checkBox2.Checked)
                        {
                            while (rdr.Read())
                            {
                                chart2.Series[catename].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                                cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
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
                                    chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                                    i++;

                                }

                                chart2.Series[catename].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                                cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
                                i++;

                            }
                            if (((System.DateTime.Today).AddDays(-1 * (i - 1))).ToString("yyyy-MM-dd") != dateTimePicker3.Value.ToString("yyyy-MM-dd"))
                            {
                                while (true)
                                {
                                    if (((System.DateTime.Today).AddDays(-1 * i)) >= dateTimePicker3.Value)
                                    {
                                        chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddDays(-1 * i)).ToString("yyyy-MM-dd"), 0);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    i++;
                                }
                            }

                        }
                    }
                    else if (s == 1)
                    {
                        if (checkBox2.Checked)
                        {
                            while (rdr.Read())
                            {
                                chart2.Series[catename].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                            }
                        }
                        else
                        {

                            int i = 0;
                            while (rdr.Read())
                            {
                                string nd = rdr["day"].ToString();
                                while (nd != ((System.DateTime.Today).AddMonths(-1 * i)).ToString("yyyy-MM"))
                                {
                                    chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddMonths(-1 * i)).ToString("yyyy-MM"), 0);
                                    i++;

                                }

                                chart2.Series[catename].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                                cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
                                i++;

                            }
                            if (((System.DateTime.Today).AddMonths(-1 * (i - 1))) != dateTimePicker3.Value)
                            {
                                while (true)
                                {
                                    if (((System.DateTime.Today).AddMonths(-1 * i)) >= dateTimePicker3.Value)
                                    {
                                        chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddMonths(-1 * i)).ToString("yyyy-MM"), 0);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    i++;
                                }
                            }

                        }
                    }
                    else
                    {

                        if (checkBox2.Checked)
                        {
                            while (rdr.Read())
                            {
                                chart2.Series[catename].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                            }
                        }
                        else
                        {

                            int i = 0;
                            while (rdr.Read())
                            {
                                string nd = rdr["day"].ToString();
                                while (nd != ((System.DateTime.Today).AddYears(-1 * i)).ToString("yyyy"))
                                {
                                    chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddYears(-1 * i)).ToString("yyyy"), 0);
                                    i++;

                                }

                                chart2.Series[catename].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                                cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
                                i++;

                            }
                            if (((System.DateTime.Today).AddYears(-1 * (i - 1))) != dateTimePicker3.Value)
                            {
                                while (true)
                                {
                                    if (((System.DateTime.Today).AddYears(-1 * i)) >= dateTimePicker3.Value)
                                    {
                                        chart2.Series[catename].Points.AddXY(((System.DateTime.Today).AddYears(-1 * i)).ToString("yyyy"), 0);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    i++;
                                }
                            }

                        }
                    }
                }
            }
            

            
            //chart2.Legends[0].Enabled = false;
            chart2.BackColor = Color.FromArgb(238, 242, 247);
            System.Windows.Forms.DataVisualization.Charting.Axis at = chart2.ChartAreas[0].AxisY;
            at.Minimum = 0;
            at.Maximum = 10;

            System.Windows.Forms.DataVisualization.Charting.StripLine st = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            st.BackColor = Color.FromArgb(238, 242, 247);
            st.StripWidth = 10;
            st.IntervalOffset = 0;



            chart2.ChartAreas[0].AxisY.StripLines.Add(st);

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

            com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM order by STATISTIC_DATE desc";
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
                if(flowLayoutPanel6.Controls[ix].BackColor == Color.FromArgb(5, 21, 64))
                {
                    nt += "AND ITEM_CATE = '" +flowLayoutPanel6.Controls[ix].Tag +"'";
                }
            }

            com1.CommandText = "SELECT * FROM STATISTIC, ITEM,ITEM_CATE WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM " + nt + "order by statistic_date desc";
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
            if (((Label)sender).BackColor == Color.FromArgb(5, 21, 64))
            {
                for (int ix = flowLayoutPanel6.Controls.Count - 1; ix >= 0; ix--)
                {
                    flowLayoutPanel6.Controls[ix].BackColor = Color.FromArgb(238, 242, 247);
                    flowLayoutPanel6.Controls[ix].ForeColor = Color.FromArgb(5, 21, 64);
                }
                ((Label)sender).BackColor = Color.FromArgb(238,242,247);
                ((Label)sender).ForeColor = Color.FromArgb(5, 21, 64);
            }
            else
            {
                for (int ix = flowLayoutPanel6.Controls.Count - 1; ix >= 0; ix--)
                {
                    flowLayoutPanel6.Controls[ix].BackColor = Color.FromArgb(238, 242, 247);
                    flowLayoutPanel6.Controls[ix].ForeColor = Color.FromArgb(5, 21, 64);
                }
                ((Label)sender).BackColor = Color.FromArgb(5, 21, 64);
                ((Label)sender).ForeColor = Color.White;
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



            if (((Label)sender).BackColor != Color.FromArgb(5, 21, 64))
            {
                ((Label)sender).BackColor = Color.FromArgb(5, 21, 64);
                ((Label)sender).ForeColor = Color.White;
            }
            else
            {
                ((Label)sender).BackColor = Color.FromArgb(238, 242, 247);
                ((Label)sender).ForeColor = Color.FromArgb(5, 21, 64);
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            k = 0;

            view(panel7);

            table = new DataTable();

            dataGridView2.Columns.Clear();
            dataGridView2.Rows.Clear();

            dataGridView2.ColumnCount = 9;
            dataGridView2.Columns[0].Name = "상품ID";
            dataGridView2.Columns[1].Name = "상품명";
            dataGridView2.Columns[2].Name = "상품사이즈";
            dataGridView2.Columns[3].Name = "상품색";
            dataGridView2.Columns[4].Name = "상품위치";
            dataGridView2.Columns[5].Name = "상품품질";
            dataGridView2.Columns[6].Name = "상품분류";
            dataGridView2.Columns[7].Name = "상품가격";
            dataGridView2.Columns[8].Name = "상품개수";

            dataGridView2.Columns[0].Visible = false;

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM ITEM, ITEM_CATE WHERE ITEM_CATE = ITEM_CATE_NUM";

            OracleDataReader rdr = com1.ExecuteReader();

            while (rdr.Read())
            {
                if (rdr["ITEM_STATUS"].ToString() != "3")
                {
                    dataGridView2.Rows.Add(rdr["ITEM_NUM"], rdr["ITEM_NAME"], rdr["ITEM_SIZE"], rdr["ITEM_COLOR"], rdr["ITEM_LOCATION"], rdr["ITEM_QUALITY"], rdr["ITEM_CATE_NAME"], rdr["ITEM_PRICE"], rdr["ITEM_COUNT"]);
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

            while(s.Count() != 0)
            {
                
                    com1.CommandText = "UPDATE ITEM SET ITEM_STATUS = 3 WHERE ITEM_NUM = '" + s.Pop() +"'";
                    com1.ExecuteNonQuery();
                
            }
        
        }

        public void uiBtn_Delete_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < dataGridView2.Rows.Count; i++)

            {
                //dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                if (dataGridView2.Rows[i].Selected == true)
                {
                    string t = (dataGridView2.Rows[i].Cells[0].Value).ToString();
                    s.Push(t);
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
            


        }

        private void bc(object sender, EventArgs e)
        {


            
        }

        private void button18_Click(object sender, EventArgs e)
        {
            


            

        }

        public void homeview()
        {

            view(panel9);

            for (int ix = flowLayoutPanel3.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel3.Controls[ix].Dispose();
            }

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

            label27.ForeColor = Color.FromArgb(5, 21, 64);
            label28.ForeColor = Color.FromArgb(5, 21, 64);
            label29.ForeColor = Color.FromArgb(5, 21, 64);



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

            int cntprice = 0;
            while (rdr2.Read())
            {
                Label lb = new Label();
                lb.Text = rdr2["ITEM_NAME"].ToString() + " - " + rdr2["STATISTIC_LENTALCOST"].ToString();
                lb.AutoSize = true;
                lb.Font = new Font("휴먼엑스포", 10);
                lb.ForeColor = Color.FromArgb(5, 21, 64);
                cntprice += Convert.ToInt32(rdr2["STATISTIC_LENTALCOST"]);
                flowLayoutPanel3.Controls.Add(lb);
            }

            label29.Text = cntprice.ToString() + "원";



        }

        private void button19_Click(object sender, EventArgs e)
        {
            panel9.Visible = false;
    
        }

        private void button20_Click(object sender, EventArgs e)
        {
            
            
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
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            panel6.Visible = false;
            panel7.Visible = false;
            panel9.Visible = false;
            panel8.Visible = false;

            vp.Visible = true;
        }

        private void button23_Click(object sender, EventArgs e)
        {
           
            chart1_view(sender, e);
        }

        private void label13_Click(object sender, EventArgs e)
        {
            homeview();
            menu(sender, e);
        }

        private void label21_Click(object sender, EventArgs e)
        {
            button3_Click(sender, e);
            menu(sender, e);
        }

        private void label22_Click(object sender, EventArgs e)
        {
            button5_Click_1(sender, e);
            menu(sender, e);
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            view(panel3);
            dateTimePicker1.Value = DateTime.Today.AddDays(-5);
            dateTimePicker2.Value = DateTime.Today;
            comboBox2.SelectedIndex = 0;
            chart1_view(sender, e);
        }

        private void chart1_view(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            int s = comboBox2.SelectedIndex;
            string ymd = "";
            string dtp = "";
            string dtp2 = "";

            if (s == 0 || s == -1)
            {
                ymd = "yyyy-MM-dd";
                dtp = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                dtp2 = dateTimePicker2.Value.ToString("yyyy-MM-dd");
            }
            else if (s == 1)
            {
                ymd = "yyyy-MM";
                dtp = dateTimePicker1.Value.ToString("yyyy-MM");
                dtp2 = dateTimePicker2.Value.ToString("yyyy-MM");
            }
            else
            {
                ymd = "yyyy";
                dtp = dateTimePicker1.Value.ToString("yyyy");
                dtp2 = dateTimePicker2.Value.ToString("yyyy");
            }



            com1.CommandText = "select* from(SELECT sum(statistic_lentalcost) as cost ,TO_CHAR(STATISTIC_DATE, '" + ymd + "') as day from statistic group by to_char(statistic_date, '" + ymd + "') order by to_char(statistic_date, '" + ymd + "') desc) where DAY >= '" + dtp + "' AND DAY<= '" + dtp2 + "'";
            OracleDataReader rdr = com1.ExecuteReader();
            int cost = -1;

            if (s == 0 || s == -1)
            {
                if (checkBox1.Checked)
                {
                    while (rdr.Read())
                    {
                        chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                        cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
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
                        cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
                        i++;

                    }
                    if (((System.DateTime.Today).AddDays(-1 * (i - 1))).ToString("yyyy-MM-dd") != dateTimePicker1.Value.ToString("yyyy-MM-dd"))
                    {
                        while (true)
                        {
                            if (((System.DateTime.Today).AddDays(-1 * i)) >= dateTimePicker1.Value)
                            {
                                int r = ((System.DateTime.Today).AddDays(-1 * i).Day);
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
            }else if (s == 1)
            {
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
                        while (nd != ((System.DateTime.Today).AddMonths(-1 * i)).ToString("yyyy-MM"))
                        {
                            chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddMonths(-1 * i)).ToString("yyyy-MM"), 0);
                            i++;

                        }

                        chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                        cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
                        i++;

                    }
                    if (((System.DateTime.Today).AddMonths(-1 * (i - 1))) != dateTimePicker1.Value)
                    {
                        while (true)
                        {
                            if (((System.DateTime.Today).AddMonths(-1 * i)) >= dateTimePicker1.Value)
                            {
                                chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddMonths(-1 * i)).ToString("yyyy-MM"), 0);
                            }
                            else
                            {
                                break;
                            }
                            i++;
                        }
                    }

                }
            }
            else
            {
                
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
                            while (nd != ((System.DateTime.Today).AddYears(-1 * i)).ToString("yyyy"))
                            {
                                chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddYears(-1 * i)).ToString("yyyy"), 0);
                                i++;

                            }

                            chart1.Series[0].Points.AddXY(rdr["day"].ToString(), rdr["cost"]);
                        cost = System.Math.Max(cost, Convert.ToInt32(rdr["cost"]));
                        i++;

                        }
                        if (((System.DateTime.Today).AddYears(-1 * (i-1))) != dateTimePicker1.Value) {
                        while (true)
                        {
                            if (((System.DateTime.Today).AddYears(-1 * i)) >= dateTimePicker1.Value)
                            {
                                chart1.Series[0].Points.AddXY(((System.DateTime.Today).AddYears(-1 * i)).ToString("yyyy"), 0);
                            }
                            else
                            {
                                break;
                            }
                            i++;
                        }
                    }

                    }
                
            }

            chart1.Legends[0].Enabled = false;
            chart1.BackColor = Color.FromArgb(238, 242, 247);

            System.Windows.Forms.DataVisualization.Charting.Axis ay = chart1.ChartAreas[0].AxisY;
            ay.Minimum = 0;
            ay.Maximum = cost + 100000;

            System.Windows.Forms.DataVisualization.Charting.StripLine sl0 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            sl0.BackColor = Color.FromArgb(238, 242, 247);
            sl0.StripWidth = cost + 100000;
            sl0.IntervalOffset = 0;



            chart1.ChartAreas[0].AxisY.StripLines.Add(sl0);
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            chart1_view(sender, e);
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            chart1_view(sender, e);
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            itemchart(sender, e);
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            itemchart(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            itemchart(sender, e);
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemchart(sender, e);
        }

        private void button5_Click_2(object sender, EventArgs e)
        {

            for (int ix = flowLayoutPanel2.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel2.Controls[ix].Dispose();
            }
            view(panel1);

            string st = monthCalendar1.SelectionStart.ToString("yyyy-MM-dd");
            string en = monthCalendar1.SelectionEnd.ToString("yyyy-MM-dd");

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM STATISTIC,ITEM WHERE STATISTIC_ITEMITEMNUM = ITEM_NUM AND STATISTIC_DATE >='" + st + "' AND STATISTIC_DATE <= '" +en +"' AND NOT STATISTIC_LENTALCOST = 0";
            OracleDataReader rdr = com1.ExecuteReader();
            int cntprice = 0;
            while(rdr.Read())
            {
                Label lb = new Label();
                lb.Text = Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd");

                flowLayoutPanel2.Controls.Add(lb);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(50, 0);
                lb.Font = new Font("휴먼엑스포", 10);

                Label lb2 = new Label();
                lb2.Text = rdr["STATISTIC_USERID"].ToString();

                flowLayoutPanel2.Controls.Add(lb2);
                lb2.AutoSize = true;
                lb2.MinimumSize = new Size(130, 0);
                lb2.Font = new Font("휴먼엑스포", 10);

                Label lb4 = new Label();
                lb4.Text = rdr["ITEM_NAME"].ToString();

                flowLayoutPanel2.Controls.Add(lb4);
                lb4.AutoSize = true;
                lb4.MinimumSize = new Size(200, 0);
                lb4.Font = new Font("휴먼엑스포", 10);

                Label lb3 = new Label();
                lb3.Text = rdr["STATISTIC_LENTALCOST"].ToString() + "원";
                cntprice += Convert.ToInt32(rdr["STATISTIC_LENTALCOST"]);

                flowLayoutPanel2.Controls.Add(lb3);
                lb3.AutoSize = true;
                lb3.MinimumSize = new Size(120, 0);
                lb3.Font = new Font("휴먼엑스포", 10);
            }

            label23.Text = cntprice.ToString() + "원";
            label23.Font = new Font("휴먼엑스포", 18, FontStyle.Bold);
            label23.ForeColor = Color.FromArgb(5, 21, 64);




        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

        private void label11_Click(object sender, EventArgs e)
        {
            button5_Click_2(sender, e);
        }

        private void label12_Click(object sender, EventArgs e)
        {
            button5_Click_2(sender, e);
            menu(sender, e);
        }

        private void label24_Click(object sender, EventArgs e)
        {
            button2_Click(sender, e);
            menu(sender, e);
        }

        private void label25_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            menu(sender, e);
        }

        private void label26_Click(object sender, EventArgs e)
        {
            button10_Click(sender, e);
            menu(sender, e);
        }

        private void label30_Click(object sender, EventArgs e)
        {
            view(panel8);
            menu(sender, e);

            if (checkBox3.Checked == true)
            {
                chart6.Visible = true;
                chart5.Visible = false;
            }
            else
            {
                chart5.Visible = true;
                chart6.Visible = false;
            }

            chart5.Series[0].Points.Clear();
            chart6.Series[0].Points.Clear();

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);

            com1.CommandText = "SELECT ITEM.ITEM_NAME as ta, NVL(OP.COST,0) as ca from (SELECT ITEM_NAME, COST FROM(SELECT STATISTIC_ITEMITEMNUM AS ITN, SUM(STATISTIC_LENTALCOST) AS COST FROM STATISTIC WHERE STATISTIC_dATE > '" + (dateTimePicker5.Value).ToString("yyyy-MM-dd") + "' AND STATISTIC_DATE < '" + (dateTimePicker6.Value).ToString("yyyy-MM-dd") + "' GROUP BY statistic_itemitemnum) PP, ITEM WHERE PP.ITN = ITEM.ITEM_NUM) OP RIGHT JOIN ITEM ON OP.ITEM_NAME = ITEM.ITEM_NAME order by ca desc";
            com2.CommandText = "SELECT SUM(CA) FROM (SELECT ITEM.ITEM_NAME as ta, NVL(OP.COST,0) as ca from (SELECT ITEM_NAME, COST FROM(SELECT STATISTIC_ITEMITEMNUM AS ITN, SUM(STATISTIC_LENTALCOST) AS COST FROM STATISTIC WHERE STATISTIC_dATE > '" + (dateTimePicker5.Value).ToString("yyyy-MM-dd") + "' AND STATISTIC_DATE < '" + (dateTimePicker6.Value).ToString("yyyy-MM-dd") + "' GROUP BY statistic_itemitemnum) PP, ITEM WHERE PP.ITN = ITEM.ITEM_NUM) OP RIGHT JOIN ITEM ON OP.ITEM_NAME = ITEM.ITEM_NAME order by ca desc)";

            OracleDataReader rdr = com1.ExecuteReader();
            int sum = Convert.ToInt32(com2.ExecuteScalar());
            decimal per1 = sum / 100;
            int t = -1;
            while(rdr.Read())
            {
                chart5.Series[0].Points.AddXY(rdr["ta"].ToString(), rdr["ca"]);
                chart6.Series[0].Points.AddXY(rdr["ta"].ToString(), rdr["ca"]);
                t = System.Math.Max(t, Convert.ToInt32(rdr["ca"]));

                if(per1 > Convert.ToDecimal(rdr["ca"]))
                {
                    chart6.Series[0].Points[chart6.Series[0].Points.Count - 1].Label = " ";
                }
            }

            System.Windows.Forms.DataVisualization.Charting.Axis ay = chart5.ChartAreas[0].AxisY;
            ay.Minimum = 0;
            ay.Maximum = t+100000;

            System.Windows.Forms.DataVisualization.Charting.StripLine sl0 = new System.Windows.Forms.DataVisualization.Charting.StripLine();
            sl0.BackColor = Color.FromArgb(238, 242, 247);
            sl0.StripWidth = t+100000;
            sl0.IntervalOffset = 0;
            chart5.ChartAreas[0].AxisY.StripLines.Add(sl0);
        }

        private void dateTimePicker5_ValueChanged(object sender, EventArgs e)
        {
            label30_Click(sender, e);
        }

        private void dateTimePicker6_ValueChanged(object sender, EventArgs e)
        {
            label30_Click(sender, e);
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {
            button7_Click(sender, e);
        }

        private void label35_Click(object sender, EventArgs e)
        {
            button6_Click(sender, e);
        }

        private void label37_Click(object sender, EventArgs e)
        {
            button13_Click(sender, e);
        }

        private void label38_Click(object sender, EventArgs e)
        {
            uiBtn_Delete_Click(sender, e);
        }

        private void label41_Click(object sender, EventArgs e)
        {
            this.adminlogout(sender, e);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            label30_Click(sender, e);
        }

        private void label46_Click(object sender, EventArgs e)
        {
            button3_Click_1(sender, e);
            menu(sender, e);
        }

        private void menu(object sender, EventArgs e)
        {
            for (int ix = panel10.Controls.Count - 1; ix >= 0; ix--)
            {
                panel10.Controls[ix].BackColor = Color.Transparent;
            }
            //((Label)sender).BackColor = Color.FromArgb(200, 206, 235);
        }
    }
}
