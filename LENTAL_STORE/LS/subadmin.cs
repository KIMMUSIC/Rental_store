using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace LENTAL_STORE.LS
{
    public delegate void Mainevent(object sender, EventArgs e);
        
        public partial class subadmin : UserControl
        {
        public event Mainevent sublogout;
        public subadmin()
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(5, 21, 64);
            panel2.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.Fill;
            panel4.Dock = DockStyle.Fill;
            panel7.Dock = DockStyle.Fill;

            lab1();
            label2.ForeColor = Color.FromArgb(5, 21, 64);
            label3.ForeColor = Color.FromArgb(5, 21, 64);
            label9.ForeColor = Color.FromArgb(5, 21, 64);
            label10.ForeColor = Color.FromArgb(5, 21, 64);
            label11.ForeColor = Color.FromArgb(5, 21, 64);
            label12.ForeColor = Color.FromArgb(5, 21, 64);
            checkBox1.ForeColor = Color.FromArgb(5, 21, 64);
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel2.AutoScroll = true;
            textBox2.BackColor = Color.FromArgb(238, 242, 247);
            textBox3.BackColor = Color.FromArgb(238, 242, 247);

            label21.ForeColor = Color.FromArgb(5, 21, 64);
            label16.ForeColor = Color.FromArgb(5, 21, 64);
            label17.ForeColor = Color.FromArgb(5, 21, 64);
            label18.ForeColor = Color.FromArgb(5, 21, 64);
            label10.ForeColor = Color.FromArgb(5, 21, 64);
        }

        private void view(Panel vp)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
            panel7.Visible = false;

            vp.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            lab1();
        }
        private void reserve_ok(object sender, EventArgs e, string uid, string ldy, string dt, string exdate, string lc, string lin, string rn)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);
            com1.CommandText = "INSERT INTO LENTAL VALUES(LENTAL_SEQ.nextval, '" + uid + "'," + ldy + ",TO_DATE('" + dt + "'),TO_DATE('" + exdate + "'), '" + lc + "'," + lin + ",'"+Form1.usersession + "')";
            com1.ExecuteNonQuery();

            com2.CommandText = "DELETE FROM RESERVE WHERE RESERVE_NUM = '" + rn + "'";
            com2.ExecuteNonQuery();


            com3.CommandText = "INSERT INTO STATISTIC VALUES(STATISTIC_SEQ.nextval, '" + uid + "',TO_DATE('" +dt + "'),'" + lin + "','" + ldy + "','" + lc + "','1',TO_DATE('" + exdate + "'),LENTAL_SEQ.CURRVAL, '" + Form1.usersession+"')";
            com3.ExecuteNonQuery();
            lab1();

        }

        private void return_ok(object sender, EventArgs e, string uid, string ldy, string dt, string lc, string lin, string rn, string Lental_num)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);
            OracleCommand com4 = new OracleCommand("", conn);
            OracleCommand com5 = new OracleCommand("", conn);


            
            OracleCommand com6 = new OracleCommand("", conn);
            com6.CommandText = "SELECT LENTAL_ITEM, LENTAL_EXPIRATION, ITEM_QUALITY, ITEM_PRICE FROM LENTAL,ITEM WHERE LENTAL.LENTAL_ITEM = ITEM.ITEM_NUM AND LENTAL_NUM = '" + Lental_num + "'" ;
            OracleDataReader rdr = com6.ExecuteReader();
            int Fine = 0;
            string lental_item = "";
            string iq = "";
            int fineday = 0;
            while (rdr.Read())
            {
                lental_item = rdr["LENTAL_ITEM"].ToString();

                if (Convert.ToDateTime(rdr["LENTAL_EXPIRATION"]) < System.DateTime.Now)
                {
                    fineday = (System.DateTime.Now - Convert.ToDateTime(rdr["LENTAL_EXPIRATION"])).Days;
                }
                iq = rdr["ITEM_QUALITY"].ToString();
                int ip = Convert.ToInt32(rdr["ITEM_PRICE"]);
                if (iq == "S")
                {
                    Fine = fineday * (ip) + (ip / 2) * fineday;
                }
                else if (iq == "A")
                {
                    Fine = fineday * (ip) + (ip / 3) * fineday;
                }
                else if (iq == "B")
                {
                    Fine = fineday * (ip) + (ip / 4) * fineday;
                }
                else
                {
                    Fine = fineday * (ip) + (ip / 6) * fineday;
                }

            }

            com5.CommandText = "SELECT ITEM_COUNT FROM ITEM WHERE ITEM_NUM = '" + lin + "'";
            int k = Convert.ToInt32(com5.ExecuteScalar());
            com1.CommandText = "INSERT INTO STATISTIC VALUES(STATISTIC_SEQ.nextval, '" + uid + "',TO_DATE('" + dt + "'),'" + lin + "',NULL,'" + Fine + "','2',NULL,'" + Lental_num + "','"+Form1.usersession+")";
            com1.ExecuteNonQuery();

            com2.CommandText = "DELETE FROM RESERVE WHERE RESERVE_NUM = '" + rn + "'";
            com2.ExecuteNonQuery();


            com3.CommandText = "DELETE FROM LENTAL WHERE LENTAL_NUM = '" + Lental_num+"'";
            com3.ExecuteNonQuery();


            com4.CommandText = "UPDATE ITEM SET ITEM_STATUS = '" + (k+1).ToString() + "' WHERE ITEM_NUM = '" + lin + "'";
            com4.ExecuteNonQuery();
            lab1();

        }

        private void lab1()
        {
            view(panel2);

            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel1.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel2.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel2.Controls[ix].Dispose();
            }

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM RESERVE,ITEM WHERE RESERVE_ITEM = ITEM_NUM AND RESERVE_TYPE = '1' AND (RESERVE_USERID LIKE '%" + textBox1.Text + "%' OR ITEM_NAME LIKE '%" + textBox1.Text + "%') order by reserve_date asc";
            com2.CommandText = "SELECT * FROM RESERVE,ITEM WHERE RESERVE_ITEM = ITEM_NUM AND RESERVE_TYPE = '2' AND (RESERVE_USERID LIKE '%" + textBox1.Text + "%' OR ITEM_NAME LIKE '%" + textBox1.Text + "%') order by reserve_date asc";
            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while (rdr.Read())
            {

                Label lb = new Label();
                lb.Text = Convert.ToDateTime(rdr["RESERVE_DATE"]).ToString("yy-MM-dd");

                flowLayoutPanel1.Controls.Add(lb);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(150, 0);
                lb.Font = new Font("휴먼엑스포", 13);

                Label lb2 = new Label();
                lb2.Text = rdr["RESERVE_USERID"].ToString();

                flowLayoutPanel1.Controls.Add(lb2);
                lb2.AutoSize = true;
                lb2.MinimumSize = new Size(200, 0);
                lb2.Font = new Font("휴먼엑스포", 13);

                Label lb3 = new Label();
                lb3.Text = rdr["ITEM_NAME"].ToString();

                flowLayoutPanel1.Controls.Add(lb3);
                lb3.AutoSize = true;
                lb3.MinimumSize = new Size(250, 0);
                lb3.Font = new Font("휴먼엑스포", 13);

                Label lb4 = new Label();
                lb4.Text = rdr["RESERVE_COUNT"].ToString() + "일";

                flowLayoutPanel1.Controls.Add(lb4);
                lb4.AutoSize = true;
                lb4.MinimumSize = new Size(70, 0);
                lb4.Font = new Font("휴먼엑스포", 13);

                Label lb5 = new Label();
                lb5.Text = "수락";

                string a = Convert.ToDateTime(rdr["RESERVE_EXDATE"]).ToString("yyyy-MM-dd");
                string g = Convert.ToDateTime(rdr["RESERVE_DATE"]).ToString("yyyy-MM-dd"); 
                string b = rdr["RESERVE_COST"].ToString();
                string c = rdr["RESERVE_ITEM"].ToString();
                string d = rdr["RESERVE_COUNT"].ToString();
                string f = rdr["RESERVE_NUM"].ToString();

                lb5.Click += delegate (object sender, EventArgs e) { reserve_ok(sender, e, lb2.Text, d, g,a,b,c,f); };



                flowLayoutPanel1.Controls.Add(lb5);
                lb5.AutoSize = true;
                lb5.MinimumSize = new Size(70, 0);
                lb5.Font = new Font("휴먼엑스포", 13);
            }

            while (rdr2.Read())
            {

                Label lb = new Label();
                lb.Text = Convert.ToDateTime(rdr2["RESERVE_DATE"]).ToString("yy-MM-dd");

                flowLayoutPanel2.Controls.Add(lb);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(150, 0);
                lb.Font = new Font("휴먼엑스포", 13);

                Label lb2 = new Label();
                lb2.Text = rdr2["RESERVE_USERID"].ToString();

                flowLayoutPanel2.Controls.Add(lb2);
                lb2.AutoSize = true;
                lb2.MinimumSize = new Size(200, 0);
                lb2.Font = new Font("휴먼엑스포", 13);

                Label lb3 = new Label();
                lb3.Text = rdr2["ITEM_NAME"].ToString();

                flowLayoutPanel2.Controls.Add(lb3);
                lb3.AutoSize = true;
                lb3.MinimumSize = new Size(250, 0);
                lb3.Font = new Font("휴먼엑스포", 13);

                Label lb4 = new Label();
                lb4.Text = rdr2["RESERVE_COST"].ToString() + "원";

                flowLayoutPanel2.Controls.Add(lb4);
                lb4.AutoSize = true;
                lb4.MinimumSize = new Size(150, 0);
                lb4.Font = new Font("휴먼엑스포", 13);

                Label lb5 = new Label();
                lb5.Text = "수락";

 
                string g = Convert.ToDateTime(rdr2["RESERVE_DATE"]).ToString("yyyy-MM-dd");
                string b = rdr2["RESERVE_COST"].ToString();
                string c = rdr2["RESERVE_ITEM"].ToString();
                string d = rdr2["RESERVE_COUNT"].ToString();
                string f = rdr2["RESERVE_NUM"].ToString();
                string h = rdr2["RESERVE_LENTALNUM"].ToString();

                lb5.Click += delegate (object sender, EventArgs e) { return_ok(sender, e, lb2.Text, d, g, b, c, f,h ); };

                flowLayoutPanel2.Controls.Add(lb5);
                lb5.AutoSize = true;
                lb5.MinimumSize = new Size(70, 0);
                lb5.Font = new Font("휴먼엑스포", 13);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {


            view(panel3);

            DataTable table = new DataTable();

            table.Columns.Add("ID");
            table.Columns.Add("상품명");
            table.Columns.Add("대여일수");
            table.Columns.Add("대여일");
            table.Columns.Add("반납일");
            table.Columns.Add("가격");
            table.Columns.Add("담당자");


            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);

            if(textBox3.Text == "")
            {
                if(checkBox2.Checked == true)
                {
                    com1.CommandText = "SELECT * FROM LENTAL, ITEM WHERE LENTAL_ITEM = ITEM_NUM AND LENTAL_APID = '" + Form1.usersession + "' order by lental_date desc";
                }
                else
                {
                    com1.CommandText = "SELECT * FROM LENTAL, ITEM WHERE LENTAL_ITEM = ITEM_NUM order by lental_date desc ";
                }
            }
            else
            {
                if (checkBox2.Checked == true)
                {
                    
                    com1.CommandText = "SELECT * FROM LENTAL, ITEM WHERE LENTAL_ITEM = ITEM_NUM AND(LENTAL_USERID = '" + textBox3.Text + "'OR ITEM_NAME = '" + textBox3.Text + "') AND LENTAL_APID = '" + Form1.usersession + "' order by lental_date desc";
                }
                else
                {
                    com1.CommandText = "SELECT * FROM LENTAL, ITEM WHERE LENTAL_ITEM = ITEM_NUM AND(LENTAL_USERID = '" + textBox3.Text + "'OR ITEM_NAME = '" + textBox3.Text + "') order by lental_date desc ";
                }
            }
            
            OracleDataReader rdr = com1.ExecuteReader();

            while (rdr.Read())
            {

                table.Rows.Add(rdr["LENTAL_USERID"].ToString(), rdr["ITEM_NAME"].ToString(), rdr["LENTAL_COUNT"].ToString(),Convert.ToDateTime(rdr["LENTAL_DATE"]).ToString("yy-MM-dd"), Convert.ToDateTime(rdr["LENTAL_EXPIRATION"]).ToString("yy-MM-dd"), rdr["LENTAL_COST"].ToString(), rdr["LENTAL_APID"].ToString());

            }

            dataGridView3.DataSource = table;
            dataGridView3.BackgroundColor = Color.FromArgb(238, 242, 247);
            dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView3.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView3.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
            dataGridView3.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
            dataGridView3.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView3.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView3.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView3.RowHeadersVisible = false;

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.sublogout(sender, e);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            view(panel4);

            DataTable table = new DataTable();
            DataTable table2 = new DataTable();

            table.Columns.Add("ID");
            table.Columns.Add("상품명");
            table.Columns.Add("날짜");
            table.Columns.Add("대여일수");
            table.Columns.Add("반납일");
            table.Columns.Add("분류");
            table.Columns.Add("담당자");

            table2.Columns.Add("ID");
            table2.Columns.Add("상품명");
            table2.Columns.Add("날짜");
            table2.Columns.Add("대여일수");
            table2.Columns.Add("반납일");
            table2.Columns.Add("분류");
            table2.Columns.Add("담당자");

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);


            if (textBox2.Text != "")
            {
                com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '1' AND STATISTIC_APID = '" + Form1.usersession + "' AND (STATISTIC_USERID ='" + textBox2.Text + "'OR ITEM_NAME = '" + textBox2.Text + "') order by statistic_date desc";
                com2.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '2' AND STATISTIC_APID = '" + Form1.usersession + "' AND (STATISTIC_USERID ='" + textBox2.Text + " 'OR ITEM_NAME = '" + textBox2.Text + "') order by statistic_date desc";
            }
            else
            {
                com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '1' AND STATISTIC_APID = '" + Form1.usersession + "'order by statistic_date desc";
                com2.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '2' AND STATISTIC_APID = '" + Form1.usersession + "'order by statistic_date desc";

            }

            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while (rdr.Read())
            {
                
                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), rdr["STATISTIC_ITEMCOUNT"], Convert.ToDateTime(rdr["STATISTIC_EXDATE"]).ToString("yy-MM-dd"), "대여", rdr["STATISTIC_APID"].ToString());
                
            }

            while(rdr2.Read())
            {
                table2.Rows.Add(rdr2["STATISTIC_USERID"], rdr2["ITEM_NAME"], Convert.ToDateTime(rdr2["STATISTIC_DATE"]).ToString("yy-MM-dd"), "-", "-", "반납", rdr2["STATISTIC_APID"].ToString());
            }

            dataGridView1.DataSource = table;
            dataGridView1.BackgroundColor = Color.FromArgb(238, 242, 247);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
            dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView1.RowHeadersVisible = false;

            dataGridView2.DataSource = table2;
            dataGridView2.BackgroundColor = Color.FromArgb(238, 242, 247);
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
            dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
            dataGridView2.RowsDefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView2.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dataGridView2.RowHeadersVisible = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                view(panel4);

                DataTable table = new DataTable();
                DataTable table2 = new DataTable();

                table.Columns.Add("ID");
                table.Columns.Add("상품명");
                table.Columns.Add("날짜");
                table.Columns.Add("대여일수");
                table.Columns.Add("반납일");
                table.Columns.Add("분류");
                table.Columns.Add("담당자");

                table2.Columns.Add("ID");
                table2.Columns.Add("상품명");
                table2.Columns.Add("날짜");
                table2.Columns.Add("대여일수");
                table2.Columns.Add("반납일");
                table2.Columns.Add("분류");
                table2.Columns.Add("담당자");

                OracleConnection conn = Form1.oracleconnect();
                OracleCommand com1 = new OracleCommand("", conn);
                OracleCommand com2 = new OracleCommand("", conn);



                if (textBox2.Text != "")
                {
                    com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '1' AND STATISTIC_APID = '" + Form1.usersession + "' AND (STATISTIC_USERID ='" + textBox2.Text + "'OR ITEM_NAME = '" + textBox2.Text + "') order by statistic_date desc";
                    com2.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '2' AND STATISTIC_APID = '" + Form1.usersession + "' AND (STATISTIC_USERID ='" + textBox2.Text + "'OR ITEM_NAME = '" + textBox2.Text + "') order by statistic_date desc";
                }
                else
                {
                    com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '1' AND STATISTIC_APID = '" + Form1.usersession + "' order by statistic_date desc";
                    com2.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '2' AND STATISTIC_APID = '" + Form1.usersession + "' order by statistic_date desc";

                }
                OracleDataReader rdr = com1.ExecuteReader();
                OracleDataReader rdr2 = com2.ExecuteReader();

                while (rdr.Read())
                {

                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), rdr["STATISTIC_ITEMCOUNT"], Convert.ToDateTime(rdr["STATISTIC_EXDATE"]).ToString("yy-MM-dd"), "대여", rdr["STATISTIC_APID"].ToString());

                }

                while (rdr2.Read())
                {
                    table2.Rows.Add(rdr2["STATISTIC_USERID"], rdr2["ITEM_NAME"], Convert.ToDateTime(rdr2["STATISTIC_DATE"]).ToString("yy-MM-dd"), "-", "-", "반납", rdr2["STATISTIC_APID"].ToString());
                }

                dataGridView1.DataSource = table;
                dataGridView1.BackgroundColor = Color.FromArgb(238, 242, 247);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
                dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
                dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.RowHeadersVisible = false;

                dataGridView2.DataSource = table2;
                dataGridView2.BackgroundColor = Color.FromArgb(238, 242, 247);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
                dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
                dataGridView2.RowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView2.RowHeadersVisible = false;
            }
            else
            {
                label5_Click(sender, e);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                view(panel4);

                DataTable table = new DataTable();
                DataTable table2 = new DataTable();

                table.Columns.Add("ID");
                table.Columns.Add("상품명");
                table.Columns.Add("날짜");
                table.Columns.Add("대여일수");
                table.Columns.Add("반납일");
                table.Columns.Add("분류");
                table.Columns.Add("담당자");

                table2.Columns.Add("ID");
                table2.Columns.Add("상품명");
                table2.Columns.Add("날짜");
                table2.Columns.Add("대여일수");
                table2.Columns.Add("반납일");
                table2.Columns.Add("분류");
                table2.Columns.Add("담당자");

                OracleConnection conn = Form1.oracleconnect();
                OracleCommand com1 = new OracleCommand("", conn);
                OracleCommand com2 = new OracleCommand("", conn);


                if (textBox2.Text != "")
                {
                    com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '1' AND STATISTIC_APID = '" + Form1.usersession + "' AND (STATISTIC_USERID ='" + textBox2.Text + "'OR ITEM_NAME = '" + textBox2.Text + "') order by statistic_date desc";
                    com2.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '2' AND STATISTIC_APID = '" + Form1.usersession + "' AND (STATISTIC_USERID ='" + textBox2.Text + "'OR ITEM_NAME = '" + textBox2.Text + "') order by statistic_date desc";
                }
                else
                {
                    com1.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '1' AND STATISTIC_APID = '" + Form1.usersession + "' order by statistic_date desc";
                    com2.CommandText = "SELECT * FROM STATISTIC, ITEM WHERE STATISTIC.STATISTIC_ITEMITEMNUM = ITEM.ITEM_NUM AND STATISTIC_TYPE = '2' AND STATISTIC_APID = '" + Form1.usersession + "' order by statistic_date desc";

                }
                OracleDataReader rdr = com1.ExecuteReader();
                OracleDataReader rdr2 = com2.ExecuteReader();

                while (rdr.Read())
                {

                    table.Rows.Add(rdr["STATISTIC_USERID"], rdr["ITEM_NAME"], Convert.ToDateTime(rdr["STATISTIC_DATE"]).ToString("yy-MM-dd"), rdr["STATISTIC_ITEMCOUNT"], Convert.ToDateTime(rdr["STATISTIC_EXDATE"]).ToString("yy-MM-dd"), "대여", rdr["STATISTIC_APID"].ToString());

                }

                while (rdr2.Read())
                {
                    table2.Rows.Add(rdr2["STATISTIC_USERID"], rdr2["ITEM_NAME"], Convert.ToDateTime(rdr2["STATISTIC_DATE"]).ToString("yy-MM-dd"), "-", "-", "반납", rdr2["STATISTIC_APID"].ToString());
                }

                dataGridView1.DataSource = table;
                dataGridView1.BackgroundColor = Color.FromArgb(238, 242, 247);
                dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
                dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
                dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView1.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView1.RowHeadersVisible = false;

                dataGridView2.DataSource = table2;
                dataGridView2.BackgroundColor = Color.FromArgb(238, 242, 247);
                dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView2.RowsDefaultCellStyle.BackColor = Color.FromArgb(238, 242, 247);
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("휴먼엑스포", 14, FontStyle.Bold);
                dataGridView2.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 21, 64);
                dataGridView2.RowsDefaultCellStyle.SelectionForeColor = Color.White;
                dataGridView2.RowsDefaultCellStyle.Font = new Font("휴먼엑스포", 11);
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                dataGridView2.RowHeadersVisible = false;
            }
            else
            {
                label5_Click(sender, e);
            }
        
    }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {


        }

        private void label14_Click(object sender, EventArgs e)
        {
            label4_Click(sender, e);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            label4_Click(sender, e);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            view(panel7);

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

            com1.CommandText = "SELECT NVL(PI.USERID,0) AS I,PI.CONTENT, FN.USERID, FN.P, FN.P2 FROM BLACKLIST PI RIGHT OUTER JOIN (SELECT O1.USERID, NVL(P,0) as P, NVL(P2,0) as P2 FROM (SELECT USERS.USERID, P FROM USERS LEFT OUTER JOIN (SELECT USERID, COUNT(*) AS P FROM USERS, LENTAL WHERE USERS.USERID = lental.lental_userid AND LENTAL_EXPIRATION < '" + System.DateTime.Now.ToString("yyyy-MM-dd") + "' GROUP BY USERID)PP ON users.userid = PP.USERID) O1,(SELECT USERS.USERID, P2 FROM USERS LEFT OUTER JOIN(SELECT STATISTIC_USERID, COUNT(*) AS P2 FROM STATISTIC WHERE STATISTIC_TYPE = 2 AND NOT STATISTIC_LENTALCOST = 0 GROUP BY STATISTIC_USERID) KK ON USERS.USERID = KK.STATISTIC_USERID) O2 WHERE O1.USERID = O2.USERID) FN ON PI.userid = FN.USERID";
            OracleDataReader rdr = com1.ExecuteReader();

            while (rdr.Read())
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


            if (((Label)sender).BackColor != Color.Red)
            {
                ((Label)sender).BackColor = Color.Red;
            }
            else
            {
                ((Label)sender).BackColor = Color.FromArgb(238, 242, 247);
            }



        }

        private void label19_Click(object sender, EventArgs e)
        {
            ltr(sender, e);
        }

        private void ltr(object sender, EventArgs e)
        {

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);



            for (int ix = flowLayoutPanel5.Controls.Count - 1; ix >= 0; ix--)
            {
                if (flowLayoutPanel5.Controls[ix].BackColor == Color.Red)
                {
                    string ID = flowLayoutPanel5.Controls[ix].Text;
                    com1.CommandText = "INSERT INTO BLACKLIST VALUES('" + ID + "', 'a')";
                    com1.ExecuteNonQuery();
                }
            }

            label15_Click(sender, e);
        }

        private void label20_Click(object sender, EventArgs e)
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

            label15_Click(sender, e);
        }
    }
}
