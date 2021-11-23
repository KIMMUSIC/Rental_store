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
    public delegate void detailevent(object sender, EventArgs e);
    public partial class detail : UserControl
    {
        public event detailevent deba;
        public Control[] lb = new Control[100];
        public Control[] bt = new Control[100];
        public int WIDTH = 100;
        public int HEIGHT = 0;
        public detail()
        {
            InitializeComponent();
            

        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void DataRecevieEvent(object sender, EventArgs e)
        {
            MessageBox.Show("aa");
        }

        private void detail_Load(object sender, EventArgs e)
        {

        }
        public void showm()
        {
            
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT * FROM LENTAL, ITEM WHERE LENTAL.LENTAL_ITEM = ITEM.ITEM_NUM AND LENTAL_USERID = '" + Form1.usersession + "'";
            OracleDataReader rdr = com1.ExecuteReader();

            int k = 0;
            while (rdr.Read())
            {
                HEIGHT += 20;
                lb[k] = new Label();
                bt[k] = new Button();
                bt[k].Text = "반납";
                bt[k].Name = rdr["LENTAL_NUM"].ToString();
                lb[k].Text = rdr["ITEM_NAME"].ToString() + "  " +((DateTime)rdr["LENTAL_DATE"]).ToString("yyyy-MM-dd") + " - " + ((DateTime)rdr["LENTAL_EXPIRATION"]).ToString("yyyy-MM-dd");
                lb[k].AutoSize = true;
                lb[k].Location = new System.Drawing.Point(WIDTH, HEIGHT);
                bt[k].Location = new System.Drawing.Point(400, HEIGHT);
                bt[k].Click += return_event;
                this.Controls.Add(lb[k]);
                this.Controls.Add(bt[k]);
                if(Convert.ToDateTime(rdr["LENTAL_EXPIRATION"]) < System.DateTime.Now)
                {
                    lb[k].BackColor = Color.Red;
                    
                }
                k++;


            }
        }

        public void button1_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < 100; i++)
            {
                this.Controls.Remove(lb[i]);
                this.Controls.Remove(bt[i]);
                HEIGHT = 0;
            }
            this.deba(sender, e);
        }

        private void return_event(object sender, EventArgs e)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);
            OracleCommand com4 = new OracleCommand("", conn);
            com3.CommandText = "SELECT LENTAL_ITEM, LENTAL_EXPIRATION, ITEM_QUALITY, ITEM_PRICE FROM LENTAL,ITEM WHERE LENTAL.LENTAL_ITEM = ITEM.ITEM_NUM AND LENTAL_NUM = " + ((Button)sender).Name;
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
                if(iq == "S")
                {
                    Fine = fineday * (ip) + (ip / 2) * fineday;
                }
                else if(iq == "A")
                {
                    Fine = fineday * (ip) + (ip / 3) * fineday;
                }
                else if(iq == "B")
                {
                    Fine = fineday * (ip) + (ip / 4) * fineday;
                }
                else
                {
                    Fine = fineday * (ip) + (ip / 6) * fineday;
                }

            }

            if(fineday > 0)
            {
                MessageBox.Show("대여 반납일이 " + fineday + "만큼 지났으므로 " + Fine + "원의 벌금이 부여됩니다.");
            }
            
            string current_time1 = System.DateTime.Now.ToString("yyyy-MM-dd");
            
            com2.CommandText = "INSERT INTO STATISTIC VALUES(STATISTIC_SEQ.nextval, '" + Form1.usersession + "',TO_DATE('" + current_time1 + "'),'" + lental_item + "',NULL,'" + Fine + "','2',NULL,'" + ((Button)sender).Name + "')";
            com1.CommandText = "DELETE FROM LENTAL WHERE LENTAL_NUM = " + ((Button)sender).Name;
            com1.ExecuteNonQuery();
            com2.ExecuteNonQuery();
            for (int i = 0; i < 100; i++)
            {
                this.Controls.Remove(lb[i]);
                this.Controls.Remove(bt[i]);
                HEIGHT = 0;
            }
            MessageBox.Show("반납되었습니다.");
            showm();
        }
    }
}
