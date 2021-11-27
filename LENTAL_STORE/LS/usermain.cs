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
    public delegate void Mail(object sender, EventArgs e);
    
    public partial class usermain : UserControl
    {
        public event Mail um;
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
        public int lastpage;




        public usermain()
        {
            InitializeComponent();
            panel2.Visible = false;
            panel1.Visible = true;
            panel3.Visible = true;
            panel5.Visible = false;
            panel3.AutoScroll = true;
            panel2.AutoScroll = true;
            panel2.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.Fill;
            panel5.Dock = DockStyle.Fill;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel3.AutoScroll = true;
            flowLayoutPanel4.AutoScroll = true;
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.MaximumSize = new Size(668,10000);
            flowLayoutPanel1.MaximumSize = new Size(890, 10000);

            panel1.BackColor = Color.FromArgb(5, 21, 64);
            panel2.BackColor = Color.FromArgb(238, 242, 247);
            panel3.BackColor = Color.FromArgb(238, 242, 247);
            /*button7.BackColor = Color.FromArgb(5, 21, 64);
            button7.ForeColor = Color.White;
            button7.FlatAppearance.BorderSize = 0;
            button7.FlatStyle = FlatStyle.Flat;*/
            
            refresh2();
            label9.BackColor = Color.FromArgb(5, 21, 64);
            label9.Click += button5_Click_2;
            label9.ForeColor = Color.White;

            label10.BackColor = Color.FromArgb(5, 21, 64);
            label10.Click += rental_;
            label10.ForeColor = Color.White;

            label11.ForeColor = Color.FromArgb(5, 21, 64);
            label12.ForeColor = Color.FromArgb(5, 21, 64);
            label13.ForeColor = Color.FromArgb(5, 21, 64);
            label14.ForeColor = Color.FromArgb(5, 21, 64);
            label15.ForeColor = Color.FromArgb(5, 21, 64);
            label16.ForeColor = Color.FromArgb(5, 21, 64);
            label17.ForeColor = Color.FromArgb(5, 21, 64);
            label18.ForeColor = Color.FromArgb(5, 21, 64);

            textBox1.BackColor = Color.FromArgb(238, 242, 247);

            label19.Click += button2_Click;
            label20.Click += button1_Click;
            label19.ForeColor = Color.FromArgb(5, 21, 64);
            label20.ForeColor = Color.FromArgb(5, 21, 64);

            label1.ForeColor = Color.FromArgb(5, 21, 64);
            label2.ForeColor = Color.FromArgb(5, 21, 64);
            label6.ForeColor = Color.FromArgb(5, 21, 64);
            label3.ForeColor = Color.FromArgb(5, 21, 64);
            label4.ForeColor = Color.FromArgb(5, 21, 64);
            label5.ForeColor = Color.FromArgb(5, 21, 64);
            label22.ForeColor = Color.FromArgb(5, 21, 64);
            label23.ForeColor = Color.FromArgb(5, 21, 64);

            label1.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            label2.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            label6.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            label4.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            label3.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            label22.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            label23.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);

            label21.Click += Back_btn;
            label23.Click += button4_Click;







            //login_pw.BackColor = Color.FromArgb(238, 242, 247);
            //label7.ForeColor = Color.FromArgb(5, 21, 64);
        }
        
        public void refresh2()
        {
            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel1.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel3.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel3.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel5.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel5.Controls[ix].Dispose();
            }
            panel3.Visible = true;
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM WHERE NOT ITEM_STATUS = 3";
            com2.CommandText = "SELECT * FROM ITEM_CATE";
            

            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while(rdr2.Read())
            {
                Label lb = new Label();
                lb.Text = "#" + rdr2["ITEM_CATE_NAME"].ToString();
                lb.Name = rdr2["ITEM_CATE_NUM"].ToString();
                lb.Tag = 0;
                lb.AutoSize = true;
                lb.BackColor = Color.FromArgb(245, 247, 250);
                lb.ForeColor = Color.FromArgb(5, 21, 64);
                lb.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                lb.Click += filter_Click;
                flowLayoutPanel5.Controls.Add(lb);

            }

            while (rdr.Read())
            {
                FlowLayoutPanel pn = new FlowLayoutPanel();
                //pn.AutoSize = true;
                pn.Size = new Size(200, 200);
                pn.Click += new_Click;

                pn.BackColor = Color.FromArgb(245,247,250);

                Label lb = new Label();

                PictureBox pb = new PictureBox();
                ((PictureBox)(pb)).SizeMode = PictureBoxSizeMode.StretchImage;
                byte[] bytedata = (byte[])rdr["ITEM_IMAGE"];
                System.IO.MemoryStream msData = new System.IO.MemoryStream(bytedata);
                ((PictureBox)(pb)).Image = Image.FromStream(msData);
                pn.Controls.Add(pb);
                pb.Size = new Size(160, 130);
                

                lb.AutoSize = false;
                lb.TextAlign = ContentAlignment.BottomRight;
                lb.MinimumSize = new Size(200, 0);
                


                pn.Controls.Add(lb);

                Label lb2 = new Label();

                lb2.AutoSize = false;
                lb2.TextAlign = ContentAlignment.BottomRight;
                lb2.MinimumSize = new Size(200, 0);
                lb2.Click += new_Click;
                lb.Click += new_Click;
                pb.Click += new_Click;
                pb.Name = rdr["ITEM_NUM"].ToString();
                pn.Controls.Add(lb2);
                string b = rdr["ITEM_STATUS"].ToString();
                lb.Name = rdr["ITEM_NUM"].ToString();
                pn.Name = rdr["ITEM_NUM"].ToString();
                lb2.Name = rdr["ITEM_NUM"].ToString();
                if (rdr["ITEM_COUNT"].ToString() == "0")
                {
                    lb2.Text = "품절";
                    lb2.ForeColor = Color.Red;
                }
                else
                {
                    lb2.Text = rdr["ITEM_PRICE"].ToString() + "원/일";
                }
                lb.Font = new Font("휴먼엑스포", 10, FontStyle.Bold);
                lb2.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);

                lb.Text = rdr["ITEM_NAME"].ToString();
                lb.Location = new System.Drawing.Point(0, 140);

                flowLayoutPanel1.Controls.Add(pn);
            }
            conn.Close();

        }

        public void refresh3(String st)
        {
            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel1.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel3.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel3.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel5.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel5.Controls[ix].Dispose();
            }
            panel3.Visible = true;
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM ITEM WHERE ITEM_NAME LIKE '%" + st + "%'";
            com2.CommandText = "SELECT * FROM ITEM_CATE";


            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while (rdr2.Read())
            {
                Label lb = new Label();
                lb.Text = "#" + rdr2["ITEM_CATE_NAME"].ToString();
                lb.Name = rdr2["ITEM_CATE_NUM"].ToString();
                lb.Tag = 0;
                lb.AutoSize = true;
                lb.BackColor = Color.FromArgb(245, 247, 250);
                lb.ForeColor = Color.FromArgb(5, 21, 64);
                lb.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                lb.Click += filter_Click;
                flowLayoutPanel5.Controls.Add(lb);

            }

            while (rdr.Read())
            {
                FlowLayoutPanel pn = new FlowLayoutPanel();
                //pn.AutoSize = true;
                pn.Size = new Size(200, 200);
                pn.Click += new_Click;

                pn.BackColor = Color.FromArgb(245, 247, 250);

                Label lb = new Label();

                PictureBox pb = new PictureBox();
                ((PictureBox)(pb)).SizeMode = PictureBoxSizeMode.StretchImage;
                byte[] bytedata = (byte[])rdr["ITEM_IMAGE"];
                System.IO.MemoryStream msData = new System.IO.MemoryStream(bytedata);
                ((PictureBox)(pb)).Image = Image.FromStream(msData);
                pn.Controls.Add(pb);
                pb.Size = new Size(160, 130);

                lb.AutoSize = false;
                lb.TextAlign = ContentAlignment.BottomRight;
                lb.MinimumSize = new Size(200, 0);



                pn.Controls.Add(lb);

                Label lb2 = new Label();

                lb2.AutoSize = false;
                lb2.TextAlign = ContentAlignment.BottomRight;
                lb2.MinimumSize = new Size(200, 0);


                pn.Controls.Add(lb2);
                string b = rdr["ITEM_STATUS"].ToString();
                lb.Name = rdr["ITEM_NUM"].ToString();
                pn.Name = rdr["ITEM_NUM"].ToString();
                lb2.Name = rdr["ITEM_NUM"].ToString();
                lb.Click += new_Click;
                pb.Click += new_Click;
                lb2.Click += new_Click;
                pb.Name = rdr["ITEM_NUM"].ToString();
                if (rdr["ITEM_COUNT"].ToString() == "0")
                {
                    lb2.Text = "품절";
                    lb2.ForeColor = Color.Red;
                }
                else
                {
                    lb2.Text = rdr["ITEM_PRICE"].ToString() + "원/일";
                }
                lb.Font = new Font("휴먼엑스포", 10, FontStyle.Bold);
                lb2.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);

                lb.Text = rdr["ITEM_NAME"].ToString();
                lb.Location = new System.Drawing.Point(0, 140);

                flowLayoutPanel1.Controls.Add(pn);
                lb2.Click += new_Click;
                lb.Click += new_Click;
                pb.Click += new_Click;
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

            for (int ix = flowLayoutPanel2.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel2.Controls[ix].Dispose();
            }
            TextBox rtb = new TextBox();
            Button rbt = new Button();

            label3.Text = "1";
            label3.Visible = true;
            label19.Visible = true;
            label20.Visible = true;



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
                label22.Text = "가격 : " + rdr["ITEM_PRICE"].ToString() + "원/일";
                item_status = Convert.ToInt32(rdr["ITEM_COUNT"]);

            }
            label4.Text = price +"원";

            if(item_status == 0)
            {
                label23.Text = "품절";
                label3.Visible = false;
                label19.Visible = false;
                label20.Visible = false;
                label23.Click -= button4_Click;
            }
            else
            {
                label23.Text = "대여하기";
                label23.Click -= button4_Click;
                label23.Click += button4_Click;
            }
            Label rbtl = new Label();
            rbtl.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
            rbtl.ForeColor = Color.FromArgb(5, 21, 64);
            rbtl.AutoSize = false;
            rbtl.Text = "등록";
            rbtl.Size= new Size(100, 50);
            rbtl.TextAlign = ContentAlignment.MiddleCenter;
            rtb.Size = new Size(550, 50);
            rtb.Multiline = true;

            rbtl.Click += delegate (object s, EventArgs ea) { review_click(sender, e, rtb); }; ;
            //delegate (object sender, EventArgs e) { reserve_ok(sender, e, lb2.Text, d, g, a, b, c, f); };
            while (rdr2.Read())
            {

                Label rn = new Label();
                Label rl = new Label();
                

                rn.AutoSize = true;
                rl.AutoSize = true;
                
                flowLayoutPanel2.Controls.Add(rn);
                flowLayoutPanel2.Controls.Add(rl);

                rn.MinimumSize = new Size(200, 0);
                rl.MinimumSize = new Size(400, 0);

                rn.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                rn.ForeColor = Color.FromArgb(5, 21, 64);
                rl.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);
                rl.ForeColor = Color.FromArgb(5, 21, 64);

                rn.Text = rdr2["REVIEW_USERID"].ToString();
                rl.Text = rdr2["REVIEW_CONTENT"].ToString();


                

            }
            if (rdr3 != null)
            {
                flowLayoutPanel2.Controls.Add(rtb);
                flowLayoutPanel2.Controls.Add(rbtl);

            }

            conn.Close();

        }

        private void review_click(object sender, EventArgs e, TextBox tb)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "INSERT INTO REVIEW VALUES(REVIEW_SEQ.nextval, '" + Form1.usersession + "','" + select + "','" + tb.Text + "','" + System.DateTime.Now.ToString("yy-MM-dd") + "')";

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
            }

                for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
                {
                    flowLayoutPanel1.Controls[ix].Dispose();
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
                OracleCommand com4 = new OracleCommand("", conn);
                com4.CommandText = "SELECT ITEM_COUNT FROM ITEM WHERE ITEM_NUM = " + ITEM_NUM + "";
                int k = Convert.ToInt32(com4.ExecuteScalar());
                com2.CommandText = "UPDATE ITEM SET ITEM_COUNT = '" + (k-1).ToString() + "' WHERE ITEM_NUM = " + ITEM_NUM + "";
                
                string current_time1 = System.DateTime.Now.ToString("yyyy-MM-dd");
                DateTime current_time = Convert.ToDateTime(current_time1);
                string exp_time = (current_time.AddDays(Convert.ToDouble(label3.Text))).ToString("yyyy-MM-dd");

                com1.CommandText = "INSERT INTO RESERVE VALUES(RESERVE_SEQ.nextval, '" + Form1.usersession + "',TO_DATE('" + current_time1 + "'),TO_DATE('" + exp_time + "'), '" + label3.Text + "','" +label4.Text +"',"+ITEM_NUM + ",'1','0')";
            
                com1.ExecuteNonQuery();
                com2.ExecuteNonQuery();
                //com3.ExecuteNonQuery();
                MessageBox.Show("대여신청되었습니다.");
                button3_Click(sender, e);
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
            panel5.Visible = false;
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
        private void Back_btn(object sender, EventArgs e)
        {
            panel2.Visible = false;
            refresh2();
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

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void rental_(object sender, EventArgs e)
        {
            panel2.Visible = false;
            for (int ix = flowLayoutPanel3.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel3.Controls[ix].Dispose();
            }

            for (int ix = flowLayoutPanel4.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel4.Controls[ix].Dispose();
            }
            panel3.Visible = false;
            panel5.Visible = true;
            for (int ix = flowLayoutPanel3.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel3.Controls[ix].Dispose();
            }

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM LENTAL, ITEM WHERE LENTAL.LENTAL_ITEM = ITEM.ITEM_NUM AND LENTAL_USERID = '" + Form1.usersession + "'";
            OracleDataReader rdr = com1.ExecuteReader();

            int k = 0;
            while (rdr.Read())
            {

                HEIGHT += 20;
                Label lb = new Label();
                Label bt = new Label();
                Label st = new Label();
                Label end = new Label();
                bt.Text = "반납";
                bt.Name = rdr["LENTAL_NUM"].ToString();
                lb.Text = rdr["ITEM_NAME"].ToString();
                st.Text = ((DateTime)rdr["LENTAL_DATE"]).ToString("yyyy-MM-dd");
                end.Text = ((DateTime)rdr["LENTAL_EXPIRATION"]).ToString("yyyy-MM-dd");

                lb.AutoSize = true;
                lb.MinimumSize = new Size(330, 30);
                st.AutoSize = true;
                st.MinimumSize = new Size(100, 30);
                end.AutoSize = true;
                end.MinimumSize = new Size(100, 30);
                lb.Font = new Font("휴먼엑스포", 18, FontStyle.Bold);
                st.Font = new Font("휴먼엑스포", 18, FontStyle.Bold);
                end.Font = new Font("휴먼엑스포", 18, FontStyle.Bold);

                bt.AutoSize = true;
                
                bt.Click += return_event;
                
                if (Convert.ToDateTime(rdr["LENTAL_EXPIRATION"]) < System.DateTime.Now)
                {
                    flowLayoutPanel3.Controls.Add(lb);
                    flowLayoutPanel3.Controls.Add(st);
                    flowLayoutPanel3.Controls.Add(end);
                    flowLayoutPanel3.Controls.Add(bt);

                    lb.ForeColor = Color.Red;
                    st.ForeColor = Color.Red;
                    end.ForeColor = Color.Red;
                    bt.ForeColor = Color.Red;

                    bt.Font = new System.Drawing.Font("휴먼엑스포", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(129)));

                }
                else
                {

                    flowLayoutPanel4.Controls.Add(lb);
                    flowLayoutPanel4.Controls.Add(st);
                    flowLayoutPanel4.Controls.Add(end);
                    flowLayoutPanel4.Controls.Add(bt);

                    lb.ForeColor = Color.FromArgb(5, 21, 64);
                    st.ForeColor = Color.FromArgb(5, 21, 64);
                    end.ForeColor =Color.FromArgb(5, 21, 64);
                    bt.ForeColor = Color.FromArgb(5, 21, 64);
                    bt.Font = new Font("휴먼엑스포", 18, FontStyle.Bold);

                }
                k++;


            }
        }

        private void return_event(object sender, EventArgs e)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);
            com3.CommandText = "SELECT LENTAL_ITEM, LENTAL_EXPIRATION, ITEM_QUALITY, ITEM_PRICE FROM LENTAL,ITEM WHERE LENTAL.LENTAL_ITEM = ITEM.ITEM_NUM AND LENTAL_NUM = " + ((Control)sender).Name;
            OracleDataReader rdr = com3.ExecuteReader();
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

            if (fineday > 0)
            {
                MessageBox.Show("대여 반납일이 " + fineday + "만큼 지났으므로 " + Fine + "원의 벌금이 부여됩니다.");
            }

            string current_time1 = System.DateTime.Now.ToString("yyyy-MM-dd");

            com1.CommandText = "SELECT * FROM LENTAL WHERE LENTAL_NUM = '" + ((Control)sender).Name + "'";
            com2.CommandText = "INSERT INTO RESERVE VALUES(RESERVE_SEQ.nextval, '" + Form1.usersession + "',TO_DATE('" + current_time1 + "'),NULL,NULL, '" + Fine + "','" + lental_item + "','2','" + ((Control)sender).Name + "')";

            com1.ExecuteNonQuery();
            com2.ExecuteNonQuery();

            MessageBox.Show("반납신청되었습니다.");
            rental_(sender, e);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void filter_Click(object sender, EventArgs e)
{
            ((Control)sender).Tag = ~Convert.ToInt32(((Control)sender).Tag);
            bool isfilter = false;
            for (int ix = flowLayoutPanel1.Controls.Count - 1; ix >= 0; ix--)
            {
                flowLayoutPanel1.Controls[ix].Dispose();
            }
            string nat = "";
            for (int ix = flowLayoutPanel5.Controls.Count - 1; ix >= 0; ix--)
            {

                if ((flowLayoutPanel5.Controls[ix].Tag).ToString() == "-1")
                {
                    isfilter = true;
                    flowLayoutPanel5.Controls[ix].BackColor = Color.Red;
                    string na = flowLayoutPanel5.Controls[ix].Name;
                    if (ix != 0)
                    {
                        nat +="OR ITEM_CATE = '" + na + "'";
                    }
                }
                else
                {
                    flowLayoutPanel5.Controls[ix].BackColor = Color.FromArgb(245, 247, 250);
                }

            }

            panel3.Visible = true;
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);

            if (isfilter == true)
            {
                com1.CommandText = "SELECT * FROM ITEM WHERE ITEM_CATE = '-1'" + nat;
            }
            else
            {
                com1.CommandText = "SELECT * FROM ITEM";
            }
            


            OracleDataReader rdr = com1.ExecuteReader();
            

            while (rdr.Read())
            {
                FlowLayoutPanel pn = new FlowLayoutPanel();
                //pn.AutoSize = true;
                pn.Size = new Size(200, 200);
                pn.Click += new_Click;

                pn.BackColor = Color.FromArgb(245, 247, 250);

                Label lb = new Label();
                

                PictureBox pb = new PictureBox();
                ((PictureBox)(pb)).SizeMode = PictureBoxSizeMode.StretchImage;
                byte[] bytedata = (byte[])rdr["ITEM_IMAGE"];
                System.IO.MemoryStream msData = new System.IO.MemoryStream(bytedata);
                ((PictureBox)(pb)).Image = Image.FromStream(msData);
                pn.Controls.Add(pb);
                pb.Size = new Size(160, 130);

                lb.AutoSize = false;
                lb.TextAlign = ContentAlignment.BottomRight;
                lb.MinimumSize = new Size(200, 0);



                pn.Controls.Add(lb);

                Label lb2 = new Label();

                lb2.AutoSize = false;
                lb2.TextAlign = ContentAlignment.BottomRight;
                lb2.MinimumSize = new Size(200, 0);


                pn.Controls.Add(lb2);
                string b = rdr["ITEM_STATUS"].ToString();
                lb.Name = rdr["ITEM_NUM"].ToString();
                pn.Name = rdr["ITEM_NUM"].ToString();
                lb2.Name = rdr["ITEM_NUM"].ToString();
                pb.Name = rdr["ITEM_NUM"].ToString();
                lb.Click += new_Click;
                pb.Click += new_Click;
                lb2.Click += new_Click;
                if (rdr["ITEM_COUNT"].ToString() == "0")
                {
                    lb2.Text = "품절";
                    lb2.ForeColor = Color.Red;
                }
                else
                {
                    lb2.Text = rdr["ITEM_PRICE"].ToString() + "원/일";
                }
                lb.Font = new Font("휴먼엑스포", 10, FontStyle.Bold);
                lb2.Font = new Font("휴먼엑스포", 16, FontStyle.Bold);

                lb.Text = rdr["ITEM_NAME"].ToString();
                lb.Location = new System.Drawing.Point(0, 140);

                flowLayoutPanel1.Controls.Add(pn);
            }
            conn.Close();

        }

        private void label24_Click(object sender, EventArgs e)
        {
            this.um(sender, e);
        }
    }
}
