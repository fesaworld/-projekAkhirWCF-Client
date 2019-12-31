using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Admin
{
    public partial class KelolaTransaksiAuto : Form
    {
        string baseurl = "http://192.168.137.1/service/Service1.svc/";

        string id;
        string _idAdmin = Login.idAdmin;
        string _namaAdmin = Login.namaAdmin;
        string _cmbParameter;
        string _parameter;
        string _jenis;

        public KelolaTransaksiAuto()
        {
            InitializeComponent();
            SemuaTransaksi();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        //kondisi saat pertamakali load
        private void KelolaTransaksi_Load(object sender, EventArgs e)
        {
            //Kondisi Dan fungsi Button & Textbox
            txtNamaUser.Enabled = false;
            txtNamaBarang.Enabled = false;
            txtNamaAdmin.Enabled = false;
            txtTotalHarga.Enabled = false;
            txtTanggalTransaksi.Enabled = false;
            txtNomorTelpon.Enabled = false;
            txtAlamat.Enabled = false;
            cmbStatus.Enabled = false;

            cmbFilterStatus.Visible = false;

            btnSimpan.Enabled = false;
            btnBatal.Enabled = false;
        }

        //get data dari grid ke textbox
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            string namaUser = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            string namaBarang = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            string namaAdmin = _namaAdmin;
            string totalHarga = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            string tanggalTransaksi = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            string NomorTelepon = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
            string Alamat = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
            string Status = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[8].Value);

            //Kondisi Dan fungsi Button & Textbox
            txtNamaUser.Text = namaUser;
            txtNamaBarang.Text = namaBarang;
            txtNamaAdmin.Text = namaAdmin;
            txtTotalHarga.Text = totalHarga;
            txtTanggalTransaksi.Text = tanggalTransaksi;
            txtNomorTelpon.Text = NomorTelepon;
            txtAlamat.Text = Alamat;
            cmbStatus.Text = Status;

            cmbStatus.Enabled = true;

            btnSimpan.Enabled = true;
            btnBatal.Enabled = true;
        }

        //Back Menu
        private void btnKembali_Click(object sender, EventArgs e)
        {
            PilihanTransaksi menu = new PilihanTransaksi();
            menu.Show();
            this.Hide();
        }

        //Cari Data Barang
        private void button3_Click(object sender, EventArgs e)
        {
            _parameter = txtFilter.Text;
            _jenis = cmbFilter.Text;
            _cmbParameter = cmbFilterStatus.Text;

            CariTransaksi(_parameter, _jenis, _cmbParameter);
        }

        //fungsi pilihan ketika combo dipilih & get data dari teks yang terpilih di combo box
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.Text == "Cari Status")
            {
                cmbFilterStatus.Visible = true;
                txtFilter.Visible = false;
            }
            else
            {
                cmbFilterStatus.Visible = false;
                txtFilter.Visible = true;
            }
        }

        //get data dari teks yang terpilih di combo box
        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            _cmbParameter = cmbFilterStatus.Text;
        }

        //Fungsi Simpan
        private void btnSimpan_Click(object sender, EventArgs e)
        {
            dataTransaksi transaksi = new dataTransaksi();

            try
            {
                transaksi.ID_Transaksi = Convert.ToInt32(id);
                transaksi.ID_Admin = Convert.ToInt32(_idAdmin);
                transaksi.Status = cmbStatus.SelectedItem.ToString();

                UpdateBarang(transaksi);

                MessageBox.Show("Data Terupdate");
            }
            catch
            {
                MessageBox.Show("Eror, Semua Data Harus Diisi!");
            }

            SemuaTransaksi();

            //Kondisi Dan fungsi Button & Textbox
            txtNamaUser.Text = "";
            txtNamaBarang.Text = "";
            txtNamaAdmin.Text = "";
            txtTotalHarga.Text = "";
            txtTanggalTransaksi.Text = "";
            txtNomorTelpon.Text = "";
            txtAlamat.Text = "";
            cmbStatus.Text = "-- Pilih Status Transaksi --";

            cmbStatus.Enabled = false;

            btnSimpan.Enabled = false;
            btnBatal.Enabled = false;
        }

        //fungsi batal
        private void btnBatal_Click(object sender, EventArgs e)
        {
            //Kondisi Dan fungsi Button & Textbox
            txtNamaUser.Text = "";
            txtNamaBarang.Text = "";
            txtNamaAdmin.Text = "";
            txtTotalHarga.Text = "";
            txtTanggalTransaksi.Text = "";
            txtNomorTelpon.Text = "";
            txtAlamat.Text = "";
            cmbStatus.Text = "-- Pilih Status Transaksi --";

            cmbStatus.Enabled = false;

            btnSimpan.Enabled = false;
            btnBatal.Enabled = false;
        }

        //BAGIAN FUNGSI & TAMPILAN ----

        //--------------------------------------------------------//

        //BAGIAN LOGIC ----

        //Tampil Semua Transaksi
        public void SemuaTransaksi()
        {
            var transaksi = new List<dataView>();
            string result = new WebClient().DownloadString(baseurl + "getalltransaksiview");
            transaksi = JsonConvert.DeserializeObject<List<dataView>>(result);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID Transaksi");
            dt.Columns.Add("Nama User");
            dt.Columns.Add("Nama Barang");
            dt.Columns.Add("Nama Admin");
            dt.Columns.Add("Tanggal Transaksi");
            dt.Columns.Add("Total Harga");
            dt.Columns.Add("Alamat Pengiriman");
            dt.Columns.Add("Nomor HP");
            dt.Columns.Add("Status");
            foreach (var Item in transaksi)
            {
                dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
            }
            dataGridView1.DataSource = dt;
        }

        //Cari Barang
        public void CariTransaksi(string parameter, string jenis, string cmbParameter)
        {
            if (parameter == "" && txtFilter.Visible == true)
            {
                SemuaTransaksi();
            }

            else if (jenis == "Cari Nama")
            {
                var transaksi = new List<dataView>();
                string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewnama/nama=" + parameter);
                transaksi = JsonConvert.DeserializeObject<List<dataView>>(result);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID Transaksi");
                dt.Columns.Add("Nama User");
                dt.Columns.Add("Nama Barang");
                dt.Columns.Add("Nama Admin");
                dt.Columns.Add("Tanggal Transaksi");
                dt.Columns.Add("Total Harga");
                dt.Columns.Add("Alamat Pengiriman");
                dt.Columns.Add("Nomor HP");
                dt.Columns.Add("Status");
                foreach (var Item in transaksi)
                {
                    dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
                }
                dataGridView1.DataSource = dt;

            }

            else if (jenis == "Cari ID")
            {
                var transaksi = new List<dataView>();
                string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewid/id=" + parameter);
                transaksi = JsonConvert.DeserializeObject<List<dataView>>(result);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID Transaksi");
                dt.Columns.Add("Nama User");
                dt.Columns.Add("Nama Barang");
                dt.Columns.Add("Nama Admin");
                dt.Columns.Add("Tanggal Transaksi");
                dt.Columns.Add("Total Harga");
                dt.Columns.Add("Alamat Pengiriman");
                dt.Columns.Add("Nomor HP");
                dt.Columns.Add("Status");
                foreach (var Item in transaksi)
                {
                    dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
                }
                dataGridView1.DataSource = dt;
            }

            else if (jenis == "Cari Status")
            {
                var transaksi = new List<dataView>();
                string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewstatus/status=" + cmbParameter);
                transaksi = JsonConvert.DeserializeObject<List<dataView>>(result);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID Transaksi");
                dt.Columns.Add("Nama User");
                dt.Columns.Add("Nama Barang");
                dt.Columns.Add("Nama Admin");
                dt.Columns.Add("Tanggal Transaksi");
                dt.Columns.Add("Total Harga");
                dt.Columns.Add("Alamat Pengiriman");
                dt.Columns.Add("Nomor HP");
                dt.Columns.Add("Status");
                foreach (var Item in transaksi)
                {
                    dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
                }
                dataGridView1.DataSource = dt;
            }
        }

        //Update Data
        public void UpdateBarang(dataTransaksi transaksi)
        {
            string request = JsonConvert.SerializeObject(transaksi);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "updatetransaksi", request);

        }
    }

    public class dataTransaksi
    {
        public int ID_Transaksi { get; set; }
        public int ID_Admin { get; set; }
        public string Status { get; set; }
    }

    public class dataView
    {
        public int ID_Transaksi { get; set; }
        public string Nama_User { get; set; }
        public string Nama_Barang { get; set; }
        public string Nama_Admin { get; set; }
        public int Total_Harga { get; set; }
        public string Tanggal_Transaksi { get; set; }
        public string No_Telpon { get; set; }
        public string Alamat { get; set; }
        public string Status { get; set; }
    }
}
