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
    public partial class PilihanTransaksi : Form
    {
        public PilihanTransaksi()
        {
            InitializeComponent();
        }

        private void btTransaksi_Click(object sender, EventArgs e)
        {
            KelolaTransaksiManual pilih = new KelolaTransaksiManual();
            pilih.Show();
            this.Hide();
        }

        private void btTransaksiManual_Click(object sender, EventArgs e)
        {
            KelolaTransaksiAuto pilih = new KelolaTransaksiAuto();
            pilih.Show();
            this.Hide();
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            Menu_Utama menu = new Menu_Utama();
            menu.Show();
            this.Hide();
        }
    }
}
