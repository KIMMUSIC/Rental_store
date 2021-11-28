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
    public delegate void MyEvent(object sender, EventArgs e);
    public partial class home : UserControl
    {
        public event MyEvent bu3;
        public event MyEvent bu4;
        public event MyEvent bu5;
        public event MyEvent bu6;

        public home()
        {
            InitializeComponent();
            login_pw.KeyDown += tb_keyDown;
            login_id.KeyDown += tb_keyDown;

            panel1.BackColor = Color.FromArgb(5, 21, 64);
            this.BackColor = Color.FromArgb(238, 242, 247);
            label3.ForeColor = Color.FromArgb(5, 21, 64);
            button1.BackColor = Color.FromArgb(5, 21, 64);
            button1.ForeColor = Color.White;
            label1.ForeColor = Color.FromArgb(5, 21, 64);
            label2.ForeColor = Color.FromArgb(5, 21, 64);
            login_id.BackColor = Color.FromArgb(238, 242, 247);
            login_pw.BackColor = Color.FromArgb(238, 242, 247);
            label7.ForeColor = Color.FromArgb(5, 21, 64);
            
            label7.Click += button3_Click;
            label7.MouseHover += label7_MouseHover;

            login_pw.PasswordChar = '*';

        }

        public void button3_Click(object sender, EventArgs e)
        {
            if(this.bu3 != null)
                this.bu3(sender, e);
        }

        private void label7_MouseHover(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void home_Load(object sender, EventArgs e)
        {

        }

        private void tb_keyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();
            OracleCommand com1 = new OracleCommand("", conn);
            com1.CommandText = "SELECT USERTYPE FROM USERS WHERE USERID = '" + login_id.Text + "' AND USERPW = '" + login_pw.Text + "'" ;

            object login = com1.ExecuteScalar();



            if(login != null)
            {
                Form1.usertype = Convert.ToInt32(com1.ExecuteScalar().ToString());
                Form1.usersession = login_id.Text;

                login_id.Text = "";
                login_pw.Text = "";
                if (Form1.usertype == 1)
                {
                    if (this.bu4 != null)
                        this.bu4(sender, e);
                }
                else if(Form1.usertype == 3)
                {
                    if (this.bu5 != null)
                        this.bu5(sender, e);
                }
                else if(Form1.usertype == 2)
                {
                    if (this.bu6 != null)
                        this.bu6(sender, e);
                }
            }
            else
            {
                login_pw.Text = "";
                MessageBox.Show("아이디 혹은 비밀번호가 틀렸습니다.");
            }



           
        }
    }
}
