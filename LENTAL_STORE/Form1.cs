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

namespace LENTAL_STORE
{
    
    public partial class Form1 : Form
    {
        public static string usersession;
        public static int usertype;


        public Form1()
        {
            InitializeComponent();
            home1.BackColor = Color.FromArgb(238, 242, 247);
            button2.BackColor = Color.FromArgb(238, 242, 247);
            usermain1.BackColor = Color.FromArgb(238, 242, 247);
            panel1.Controls.Add(home1);
            panel1.Controls.Add(signin1);
            panel1.Controls.Add(usermain1);
            panel1.Controls.Add(detail1);
            panel1.Controls.Add(admin1);
            home1.bu5 += new MyEvent(bu_5);
            home1.bu3 += new MyEvent(bu_3);
            home1.bu4 += new MyEvent(bu_4);
            signin1.signinsubmit += signin;
            usermain1.um += new LS.Mainevent(um3);
            detail1.deba += new LS.detailevent(ddbb);



        }


        private void bu_3(object sender, EventArgs e)
        {
            home1.Visible = false;
            signin1.Visible = true;

        }
        private void bu_5(object sender, EventArgs e)
        {
            home1.Visible = false;
            usermain1.Visible = false;
            detail1.Visible = false;
            admin1.Visible = true;

        }
        private void ddbb(object sender, EventArgs e)
        {
            detail1.Visible = false;
            bu_4(sender, e);

        }

        private void bu_4(object sender, EventArgs e)
        {
            home1.Visible = false;
            signin1.Visible = false;
            usermain1.Visible = true;
            //usermain1.prefresh2();
            //usermain1.refresh2();
        }
        private void um3(object sender, EventArgs e)
        {
            usermain1.Visible = false;
            detail1.Visible = true;
            detail1.showm();

 
            
            
            
        }

        private void signin(object sender, EventArgs e)
        {
            
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void signin1_Load(object sender, EventArgs e)
        {

        }

        private void home1_Load(object sender, EventArgs e)
        {

        }
        public static OracleConnection oracleconnect()
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();

            return conn;
        }
    }
}
