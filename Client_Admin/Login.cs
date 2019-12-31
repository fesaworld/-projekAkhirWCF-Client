using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net;

namespace Client_Admin
{
    public partial class Login : Form
    {
        string baseurl = "http://192.168.137.1/service/Service1.svc/";

        public Login()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }

        private void btLogin_Click(object sender, EventArgs e)
        {

        }

        public static string idAdmin = "";
        public static string namaAdmin = "";

        private void btLogin_Click_1(object sender, EventArgs e)
        {
            try
            {
                dataAdmin admin = new dataAdmin();
                string result = new WebClient().DownloadString(baseurl + "getadmin/nama=" + textBox1.Text);
                admin = JsonConvert.DeserializeObject<dataAdmin>(result);

                if (textBox1.Text == admin.Nama_Admin && textBox2.Text == admin.Password)
                {
                    idAdmin = Convert.ToString(admin.ID_Admin);
                    namaAdmin = admin.Nama_Admin;

                    Menu_Utama menu = new Menu_Utama();
                    menu.Show();
                    this.Hide();
                }
                else
                {
                    textBox1.Text = "";
                    textBox2.Text = "";
                    MessageBox.Show("Username Atau Password Salah");
                    textBox1.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Username & Password Harus Diisi");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }

    public class dataAdmin
    {
        public int ID_Admin { get; set; }
        public string Nama_Admin { get; set; }
        public string Password { get; set; }
    }

}
