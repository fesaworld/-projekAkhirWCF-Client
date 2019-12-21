using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Admin
{
    public partial class Menu_Utama : Form
    {
        public Menu_Utama()
        {
            InitializeComponent();
        }

        //pilih kelola barang
        private void btBarang_Click(object sender, EventArgs e)
        {
            KelolaBarang barang = new KelolaBarang();
            barang.Show();
            this.Hide();
        }

        //pilih kelola transaksi
        private void btTransaksi_Click(object sender, EventArgs e)
        {
            PilihanTransaksi pilih = new PilihanTransaksi();
            pilih.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Menu_Utama_Load(object sender, EventArgs e)
        {
            label3.Text = Login.namaAdmin;
        }

        //kembali/logout
        private void button4_Click(object sender, EventArgs e)
        {
            Login logout = new Login();
            logout.Show();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
