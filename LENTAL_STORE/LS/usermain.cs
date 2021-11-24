using System;
using System.Collections;
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
    
    public partial class usermain : UserControl
    {
        public event Mainevent um;
        public static string select;
        public static string select_status;
        public string price = "";
        public int WIDTH = 0;
        public int ITEM_NUM;
        public int HEIGHT = 0;
        public Control[] lb = new Control[100];
        public Control[] lb2 = new Control[100];
        public Control[] pb = new Control[100];
        public Control[] rl = new Control[100];
        public TextBox rtb = new TextBox();
        public Button rbt = new Button();
        public int lastpage;
        

        public usermain()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel1.Visible = true;
            panel3.Visible = true;
            panel3.AutoScroll = true;
            panel2.AutoScroll = true;
        }
        
        public void refresh2()
        {
            
            lastpage = 2;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM";

            OracleDataReader rdr = com1.ExecuteReader();
            ArrayList rowList = new ArrayList();
            object[] row;

            WIDTH = 200;
            HEIGHT = 30;

            int start = 0;
            int end = 0;

            int k = 0;
            while (rdr.Read())
            {
                start += 100;
                
                Panel pn = new Panel();
                pn.AutoSize = true;
                pn.BackColor = Color.White;
                flowLayoutPanel1.AutoScroll = true;


                

                if (k % 3 == 0)
                {
                    HEIGHT += 100;
                    WIDTH = 200;
                    end += 100;
                }
                lb[k] = new Label();

                lb[k].AutoSize = true;
                lb[k].Click += new_Click;

                pn.Controls.Add(lb[k]);

                lb2[k] = new Label();

                lb2[k].AutoSize = true;
                lb2[k].Click += new_Click;

                pn.Controls.Add(lb2[k]);
                string b = rdr["ITEM_STATUS"].ToString();
                lb[k].Name = rdr["ITEM_NUM"].ToString();
                if (rdr["ITEM_STATUS"].ToString() == "1")
                {
                    lb2[k].Text = "품절";
                }
                else
                {
                    lb2[k].Text = rdr["ITEM_PRICE"].ToString();
                }


                lb[k].Text = rdr["ITEM_NAME"].ToString();
                lb[k].Location = new System.Drawing.Point(0, 0);

                pb[k] = new PictureBox();
                ((PictureBox)(pb[k])).SizeMode = PictureBoxSizeMode.StretchImage;
                byte[] bytedata = (byte[])rdr["ITEM_IMAGE"];
                System.IO.MemoryStream msData = new System.IO.MemoryStream(bytedata);
                ((PictureBox)(pb[k])).Image = Image.FromStream(msData);
                pb[k].Location = new System.Drawing.Point(10, 10);
                pn.Controls.Add(pb[k]);
                lb2[k].Location = new System.Drawing.Point(20,20);
                k++;
                WIDTH += 180;

                flowLayoutPanel1.Controls.Add(pn);
            }
            conn.Close();
            
            
        }

        public void refresh3(String st)
        {
            
            lastpage = 2;
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM WHERE ITEM_NAME LIKE '%" + st + "%'";

            OracleDataReader rdr = com1.ExecuteReader();
            ArrayList rowList = new ArrayList();
            object[] row;

            WIDTH = 200;
            HEIGHT = 30;

            int k = 0;
            while (rdr.Read())
            {

                
                Panel pn = new Panel();
                pn.AutoSize = true;
                pn.BackColor = Color.White;
                flowLayoutPanel1.AutoScroll = true;


                if (k % 3 == 0)
                {
                    HEIGHT += 100;
                    WIDTH = 200;
                }
                lb[k] = new Label();

                lb[k].AutoSize = true;
                lb[k].Click += new_Click;
                lb2[k] = new Label();

                lb2[k].AutoSize = true;
                lb2[k].Click += new_Click;
                pn.Controls.Add(lb[k]);
                pn.Controls.Add(lb2[k]);
                string b = rdr["ITEM_STATUS"].ToString();
                lb[k].Name = rdr["ITEM_NUM"].ToString();
                if (rdr["ITEM_STATUS"].ToString() == "1")
                {
                    lb2[k].Text = "품절";
                }
                else
                {
                    lb2[k].Text = rdr["ITEM_PRICE"].ToString();
                }


                lb[k].Text = rdr["ITEM_NAME"].ToString();
                lb[k].Location = new System.Drawing.Point(WIDTH, HEIGHT);

                pb[k] = new PictureBox();
                ((PictureBox)(pb[k])).SizeMode = PictureBoxSizeMode.StretchImage;
                byte[] bytedata = (byte[])rdr["ITEM_IMAGE"];
                System.IO.MemoryStream msData = new System.IO.MemoryStream(bytedata);
                ((PictureBox)(pb[k])).Image = Image.FromStream(msData);
                pb[k].Location = new System.Drawing.Point(WIDTH, HEIGHT);
                pn.Controls.Add(pb[k]);
                lb2[k].Location = new System.Drawing.Point(WIDTH + 20, HEIGHT + pb[k].Size.Height + lb[k].Size.Height);
                k++;
                WIDTH += 180;

                flowLayoutPanel1.Controls.Add(pn);
            }
            conn.Close();
                
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void new_Click(object sender, EventArgs e)
        {
            panel2.Controls.Remove(rtb);
            panel2.Controls.Remove(rbt);
            for(int i = 0; i < 100; ++i)
            {
                panel2.Controls.Remove(rl[i]);
            }
            rbt.Click -= review_click;
            rbt.Click +=review_click;
            label3.Text = "1";
            button4.Text = "대여하기";
            label3.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button4.Click -= button4_Click;
            button4.Click += button4_Click;



            select = ((Control)sender).Name;

            panel2.Visible = true;
            panel3.Visible = false;


            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM WHERE ITEM_NUM = '" + usermain.select + "'";
            com2.CommandText = "SELECT * FROM REVIEW WHERE REVIEW_ITEM_NUM = '" + usermain.select + "'";
            com3.CommandText = "SELECT * FROM STATISTIC WHERE STATISTIC_ITEMITEMNUM = '" + usermain.select + "' AND STATISTIC_USERID = '" + Form1.usersession + "'";

            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();
            object rdr3 = com3.ExecuteScalar();
            int item_status = 0;
            //ArrayList rowList = new ArrayList();
            //object[] row;



            
            while (rdr.Read())
            {
                
                ITEM_NUM = Convert.ToInt32(rdr["ITEM_NUM"]);
                label5.Text = rdr["ITEM_NAME"].ToString();

                byte[] bytedata = (byte[])rdr["ITEM_IMAGE"];
                System.IO.MemoryStream msData = new System.IO.MemoryStream(bytedata);
                pictureBox2.Image = Image.FromStream(msData);

                price = rdr["ITEM_PRICE"].ToString() ;
                label1.Text = "사이즈 : " + rdr["ITEM_SIZE"].ToString();
                label2.Text = "색상 : " + rdr["ITEM_COLOR"].ToString();
                label6.Text = "품질 : " + rdr["ITEM_QUALITY"].ToString();
                item_status = Convert.ToInt32(rdr["ITEM_STATUS"]);

                if(rdr3 != null)
                {
                    
                    rtb.Location = new System.Drawing.Point(0, 600);
                    panel2.Controls.Add(rtb);

                    
                    rbt.Location = new System.Drawing.Point(100, 600);
                    panel2.Controls.Add(rbt);
                    
                }
            }
            label4.Text = price;

            if(item_status == 1)
            {
                button4.Text = "품절";
                label3.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
                button4.Click -= button4_Click;
            }
            int k = 0;
            int t = 0;
            while (rdr2.Read())
            {

                rl[k] = new Label();

                rl[k].AutoSize = true;
                
                panel2.Controls.Add(rl[k]);
                string b = rdr2["REVIEW_CONTENT"].ToString();


                rl[k].Text = b;

                rl[k].Location = new System.Drawing.Point(300, 400 + t);
                t += 30;
                k++;
            }
            conn.Close();

        }

        private void review_click(object sender, EventArgs e)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "INSERT INTO REVIEW VALUES(REVIEW_SEQ.nextval, '" + Form1.usersession + "','" + select + "','" + rtb.Text + "','" + System.DateTime.Now.ToString("yy-MM-dd") + "')";

            com1.ExecuteNonQuery();
            MessageBox.Show("리뷰가 작성되었습니다");
            ((Control)sender).Name = select;
            new_Click(sender, e);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = (Convert.ToInt32(label3.Text) + 1).ToString();
            label4.Text = (Convert.ToInt32(price) * Convert.ToInt32(label3.Text)).ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label3.Text = (Convert.ToInt32(label3.Text) - 1).ToString();
            label4.Text = (Convert.ToInt32(price) * Convert.ToInt32(label3.Text)).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lastpage == 1)
            {
                panel2.Visible = false;
                panel3.Visible = false;
                prefresh();
                
            }
            else if(lastpage == 2)
            {

                button5_Click_2(sender, e);
            }

        }
        public void prefresh()
        {
            for (int i = 0; i < 100; i++)
            {
                this.Controls.Remove(lb[i]);
                this.Controls.Remove(pb[i]);
                this.Controls.Remove(lb2[i]);  
            }
        }
        public void prefresh2()
        {
            for (int i = 0; i < 100; i++)
            {/*
                flowLayoutPanel1.Controls.Remove(lb[i]);
                flowLayoutPanel1.Controls.Remove(lb2[i]);
                flowLayoutPanel1.Controls.Remove(pb[i]);*/

                for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
                {
                    flowLayoutPanel1.Controls[ix].Dispose();
                }

            }
        }

        
        public void button5_Click(object sender, EventArgs e)
        {
            
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show(""+label5.Text+" 제품을 " +label4.Text +"에 " + label3.Text + "일 간 대여 하시겠습니까?","YesOrNo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                OracleConnection conn = Form1.oracleconnect();
                OracleCommand com1 = new OracleCommand("", conn);
                OracleCommand com2 = new OracleCommand("", conn);
                OracleCommand com3 = new OracleCommand("", conn);
                com2.CommandText = "UPDATE ITEM SET ITEM_STATUS = 1 WHERE ITEM_NUM = " + ITEM_NUM + "";
                
                string current_time1 = System.DateTime.Now.ToString("yyyy-MM-dd");
                DateTime current_time = Convert.ToDateTime(current_time1);
                string exp_time = (current_time.AddDays(Convert.ToDouble(label3.Text))).ToString("yyyy-MM-dd");

                com1.CommandText = "INSERT INTO LENTAL VALUES(LENTAL_SEQ.nextval, '" + Form1.usersession + "'," + label3.Text + ",TO_DATE('" + current_time1 + "'),TO_DATE('" + exp_time + "'), '" + label4.Text + "'," +ITEM_NUM + ")";
                com3.CommandText = "INSERT INTO STATISTIC VALUES(STATISTIC_SEQ.nextval, '" + Form1.usersession + "',TO_DATE('" + current_time1 + "'),'" + ITEM_NUM + "','" + label3.Text + "','" + label4.Text + "','1',TO_DATE('" + exp_time + "'),LENTAL_SEQ.CURRVAL)";
                com1.ExecuteNonQuery();
                com2.ExecuteNonQuery();
                com3.ExecuteNonQuery();
                MessageBox.Show("대여되었습니다.");
                button3.PerformClick();
                //prefresh();
                //refresh();

                conn.Close();

            }
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_2(object sender, EventArgs e)
        {
            panel3.Visible = true;
            panel2.Visible = false;
            prefresh2();
            refresh2();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.um(sender, e); 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           prefresh2();

            if(textBox1.Text == "")
            {
                refresh2();
            }
            else
            {
                refresh3(textBox1.Text);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel3.Visible = false;
            this.Visible = true;
            prefresh();
            prefresh2();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button5_Click_2(sender, e);
        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void usermain_Load(object sender, EventArgs e)
        {

        }
    }
}
