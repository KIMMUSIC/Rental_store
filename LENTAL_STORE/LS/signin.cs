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
    
    public partial class signin : UserControl
    {
        
        
        public event EventHandler signinsubmit;
        private int idp = 0;
        public signin()
        {
            InitializeComponent();
            for(int i = 1990; i <= 2021; ++i)
                comboBox1.Items.Add(i);

            for (int i = 1; i <= 31; ++i)
                if(i < 10)
                    comboBox3.Items.Add("0"+i);
                else
                    comboBox3.Items.Add(i);

            comboBox1.SelectedText = "0000";
            comboBox2.SelectedText = "00";
            comboBox3.SelectedText = "00";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();
            OracleCommand com1 = new OracleCommand("", conn);
            string userid = textBox1.Text;
            string userpw = textBox2.Text;
            string birthyear = comboBox1.SelectedItem.ToString();
            string birthmonth = comboBox2.SelectedItem.ToString();
            string birthday = comboBox3.SelectedItem.ToString();
            string birth = birthyear + "-" + birthmonth + "-" + birthday;
            string phonenumber = textBox3.Text;
            string email = textBox4.Text;
            
            com1.CommandText = "INSERT INTO USERS(USERID,USERPW,BIRTH,PHONE_NUMBER,EMAIL,USERTYPE) " +
                "VALUES('" + textBox1.Text + "','" + textBox2.Text +  "','" + birth +"','" + phonenumber + "','" + email + "'," + 1 + ")";
            if (idp == 1)
            {
                com1.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("아이디 중복 검사를 하십시오");
            }
            conn.Close();

            this.signinsubmit(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OracleConnection conn = new OracleConnection();
            conn.ConnectionString = "DATA SOURCE=XEPDB1;USER ID=S5469744;PASSWORD=S5469744";
            conn.Open();
            OracleCommand com1 = new OracleCommand("", conn);
            string userid = textBox1.Text;
            com1.CommandText = "SELECT(CASE WHEN EXISTS(SELECT * FROM USERS WHERE USERID = '" + userid + "') THEN 'Y' ELSE 'N' END) FROM DUAL";
            string i = (com1.ExecuteScalar()).ToString();

            if(i == "N")
            {
                idp = 1;
                label9.Visible = true;

            }
            else
            {
                MessageBox.Show("존재하는 아이디입니다.");
            }



            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            idp = 0;
            label9.Visible = false;
        }
    }
}
