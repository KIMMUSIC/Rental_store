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
        public Admin()
        {
            InitializeComponent();
            panel1.Visible = true;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            button9.Click += back;
            button8.Click += back;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;
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
            panel1.Visible = true;
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
            panel1.Visible = false;
            panel4.Visible = false;
            panel3.Visible = true;
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
            panel1.Visible = false;
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
            panel1.Visible = true;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = false;
            
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel5.Visible = true;

            DataTable table = new DataTable();

            table.Columns.Add("ID");
            table.Columns.Add("상품명");
            table.Columns.Add("날짜");
            table.Columns.Add("대여일수");
            table.Columns.Add("반납일");
            table.Columns.Add("분류");

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM";

            OracleDataReader rdr = com1.ExecuteReader();

            while(rdr.Read())
            {
                table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], rdr["STATISTIC_DATE"], rdr["STATISTIC_ITEMCOUNT"], rdr["STATISTIC_EXDATE"], rdr["STATISTIC_TYPE"]);
            }

            dataGridView1.DataSource = table;
        }
    }
}
