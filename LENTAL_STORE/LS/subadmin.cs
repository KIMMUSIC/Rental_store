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
    public partial class subadmin : UserControl
    {
        public subadmin()
        {
            InitializeComponent();
            panel1.BackColor = Color.FromArgb(5, 21, 64);
            panel2.Dock = DockStyle.Fill;
        }

        private void view(Panel vp)
        {
            panel2.Visible = false;

            vp.Visible = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            lab1();
        }
        private void resrver_ok(object sender, EventArgs e, string uid, string ldy, string dt, string exdate, string lc, string lin, string rn)
        {
            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);
            OracleCommand com3 = new OracleCommand("", conn);
            com1.CommandText = "INSERT INTO LENTAL VALUES(LENTAL_SEQ.nextval, '" + uid + "'," + ldy + ",TO_DATE('" + dt + "'),TO_DATE('" + exdate + "'), '" + lc + "'," + lin + ")";
            com1.ExecuteNonQuery();

            com2.CommandText = "DELETE FROM RESERVE WHERE RESERVE_NUM = '" + rn + "'";
            com2.ExecuteNonQuery();


            com3.CommandText = "INSERT INTO STATISTIC VALUES(STATISTIC_SEQ.nextval, '" + uid + "',TO_DATE('" +dt + "'),'" + lin + "','" + ldy + "','" + lc + "','1',TO_DATE('" + exdate + "'),LENTAL_SEQ.CURRVAL)";
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
            com1.CommandText = "INSERT INTO STATISTIC VALUES(STATISTIC_SEQ.nextval, '" + uid + "',TO_DATE('" + dt + "'),'" + lin + "',NULL,'" + Fine + "','2',NULL,'" + Lental_num + "')";
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

            OracleConnection conn = Form1.oracleconnect();
            OracleCommand com1 = new OracleCommand("", conn);
            OracleCommand com2 = new OracleCommand("", conn);

            com1.CommandText = "SELECT * FROM RESERVE,ITEM WHERE RESERVE_ITEM = ITEM_NUM AND RESERVE_TYPE = '1' AND (RESERVE_USERID LIKE '%" + textBox1.Text + "%' OR ITEM_NAME LIKE '%" + textBox1.Text + "%')";
            com2.CommandText = "SELECT * FROM RESERVE,ITEM WHERE RESERVE_ITEM = ITEM_NUM AND RESERVE_TYPE = '2' AND (RESERVE_USERID LIKE '%" + textBox1.Text + "%' OR ITEM_NAME LIKE '%" + textBox1.Text + "%')";
            OracleDataReader rdr = com1.ExecuteReader();
            OracleDataReader rdr2 = com2.ExecuteReader();

            while (rdr.Read())
            {

                Label lb = new Label();
                lb.Text = Convert.ToDateTime(rdr["RESERVE_DATE"]).ToString("yy-MM-dd");

                flowLayoutPanel1.Controls.Add(lb);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(70, 0);
                lb.Font = new Font("휴먼엑스포", 10);

                Label lb2 = new Label();
                lb2.Text = rdr["RESERVE_USERID"].ToString();

                flowLayoutPanel1.Controls.Add(lb2);
                lb2.AutoSize = true;
                lb2.MinimumSize = new Size(70, 0);
                lb2.Font = new Font("휴먼엑스포", 10);

                Label lb3 = new Label();
                lb3.Text = rdr["ITEM_NAME"].ToString();

                flowLayoutPanel1.Controls.Add(lb3);
                lb3.AutoSize = true;
                lb3.MinimumSize = new Size(70, 0);
                lb3.Font = new Font("휴먼엑스포", 10);

                Label lb4 = new Label();
                lb4.Text = rdr["RESERVE_COUNT"].ToString() + "일";

                flowLayoutPanel1.Controls.Add(lb4);
                lb4.AutoSize = true;
                lb4.MinimumSize = new Size(70, 0);
                lb4.Font = new Font("휴먼엑스포", 10);

                Label lb5 = new Label();
                lb5.Text = "수락";

                string a = Convert.ToDateTime(rdr["RESERVE_EXDATE"]).ToString("yyyy-MM-dd");
                string g = Convert.ToDateTime(rdr["RESERVE_DATE"]).ToString("yyyy-MM-dd"); 
                string b = rdr["RESERVE_COST"].ToString();
                string c = rdr["RESERVE_ITEM"].ToString();
                string d = rdr["RESERVE_COUNT"].ToString();
                string f = rdr["RESERVE_NUM"].ToString();

                lb5.Click += delegate (object sender, EventArgs e) { resrver_ok(sender, e, lb2.Text, d, g,a,b,c,f); };



                flowLayoutPanel1.Controls.Add(lb5);
                lb5.AutoSize = true;
                lb5.MinimumSize = new Size(70, 0);
                lb5.Font = new Font("휴먼엑스포", 10);
            }

            while (rdr2.Read())
            {

                Label lb = new Label();
                lb.Text = Convert.ToDateTime(rdr2["RESERVE_DATE"]).ToString("yy-MM-dd");

                flowLayoutPanel2.Controls.Add(lb);
                lb.AutoSize = true;
                lb.MinimumSize = new Size(70, 0);
                lb.Font = new Font("휴먼엑스포", 10);

                Label lb2 = new Label();
                lb2.Text = rdr2["RESERVE_USERID"].ToString();

                flowLayoutPanel2.Controls.Add(lb2);
                lb2.AutoSize = true;
                lb2.MinimumSize = new Size(70, 0);
                lb2.Font = new Font("휴먼엑스포", 10);

                Label lb3 = new Label();
                lb3.Text = rdr2["ITEM_NAME"].ToString();

                flowLayoutPanel2.Controls.Add(lb3);
                lb3.AutoSize = true;
                lb3.MinimumSize = new Size(70, 0);
                lb3.Font = new Font("휴먼엑스포", 10);

                Label lb4 = new Label();
                lb4.Text = rdr2["RESERVE_COUNT"].ToString() + "일";

                flowLayoutPanel2.Controls.Add(lb4);
                lb4.AutoSize = true;
                lb4.MinimumSize = new Size(70, 0);
                lb4.Font = new Font("휴먼엑스포", 10);

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
                lb5.Font = new Font("휴먼엑스포", 10);
            }
        }

    }
}
