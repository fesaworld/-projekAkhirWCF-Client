using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Admin
{
    public partial class KelolaTransaksiManual : Form
    {
        string baseurl = "http://192.168.137.1/service/Service1.svc/";

        string _namaAdmin = Login.namaAdmin;
        string _idAdmin = Login.idAdmin;
        string _namaUser, _namaBarang, _idUser, _idBarang, _idTransaksi;
        int _parameter = 0;
        string _search, _cmbFilter, _cmbStatus;

        public KelolaTransaksiManual()
        {
            InitializeComponent();
            timer1.Start();
        }

        //Kondisi Saat Pertama Kali Load
        private void KelolaTransaksiManual_Load(object sender, EventArgs e)
        {
            SemuaTransaksi();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //Fungsi Button & Textbox
            cmbFilterStatus.Visible = false;

            cmbNamaUser.Enabled = false;
            cmbNamaBarang.Enabled = false;
            txtNamaAdmin.Enabled = false;
            txtTotalHarga.Enabled = false;
            txtTanggalTransaksi.Enabled = false;
            txtNomorTelpon.Enabled = false;
            txtAlamat.Enabled = false;
            cmbStatus.Enabled = false;

            btnSimpan.Enabled = false;
            btnHapus.Enabled = false;
            btnBatal.Enabled = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _parameter = 1;

            _idTransaksi = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            string namaUser = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            string namaBarang = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            string namaAdmin = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[3].Value); ;
            string totalHarga = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            string tanggalTransaksi = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            string NomorTelepon = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
            string Alamat = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
            string Status = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[8].Value);

            cmbNamaUser.Text = namaUser;
            cmbNamaBarang.Text = namaBarang;
            txtNamaAdmin.Text = namaAdmin;
            txtTotalHarga.Text = totalHarga;
            txtTanggalTransaksi.Text = tanggalTransaksi;
            txtNomorTelpon.Text = NomorTelepon;
            txtAlamat.Text = Alamat;
            cmbStatus.Text = Status;

            OpenImageStream(namaBarang);

            if (cmbNamaUser.Items.Count == 0)
            {
                SemuaUser();
            }
            if (cmbNamaBarang.Items.Count == 0)
            {
                SemuaBarang();
            }

            //Kondisi Dan fungsi Button & Textbox
            cmbNamaUser.Enabled = true;
            cmbNamaBarang.Enabled = true;
            cmbStatus.Enabled = true;

            btnSimpan.Text = "Update";

            btnTambah.Enabled = false;
            btnSimpan.Enabled = true;
            btnHapus.Enabled = true;
            btnBatal.Enabled = true;
        }

        //Tampil Semua User
        public void SemuaUser()
        {
            var transaksi = new List<dataPenggunaManual>();
            string result = new WebClient().DownloadString(baseurl + "getallusermanual");
            transaksi = JsonConvert.DeserializeObject<List<dataPenggunaManual>>(result);

            foreach (var Item in transaksi)
            {
                cmbNamaUser.Items.Add(Item.Nama_User);
            }
        }

        //Tampil Semua User Nama
        public void UserNama(string nama)
        {
            var transaksi = new List<dataPenggunaManual>();
            string result = new WebClient().DownloadString(baseurl + "getusernamamanual/nama=" + nama);
            transaksi = JsonConvert.DeserializeObject<List<dataPenggunaManual>>(result);

            foreach (var Item in transaksi)
            {
                _idUser = Convert.ToString(Item.ID_User);
                txtNomorTelpon.Text = Item.No_Telpon;
                txtAlamat.Text = Item.Alamat;
            }
        }



        //Tampil Semua User Nama 
        public void BarangNama(string nama)
        {
            var transaksi = new List<dataBarangManual>();
            string result = new WebClient().DownloadString(baseurl + "getbarangnamamanual/nama=" + nama);
            transaksi = JsonConvert.DeserializeObject<List<dataBarangManual>>(result);

            foreach (var Item in transaksi)
            {
                _idBarang = Convert.ToString(Item.ID_Barang);
                txtTotalHarga.Text = Convert.ToString(Item.Harga);
            }

            OpenImageStream(nama);
        }

        //Tampil Semua Transaksi
        public void SemuaTransaksi()
        {
            var transaksi = new List<dataViewManual>();
            string result = new WebClient().DownloadString(baseurl + "getalltransaksiview");
            transaksi = JsonConvert.DeserializeObject<List<dataViewManual>>(result);

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

        //Fungsi menampilkan gambar dengan stream
        public void OpenImageStream(string nama)
        {
            var transaksi = new List<dataBarangManual>();
            string result = new WebClient().DownloadString(baseurl + "getbarangnamamanual/nama=" + nama);
            transaksi = JsonConvert.DeserializeObject<List<dataBarangManual>>(result);

            string foto = "";

            foreach (var Item in transaksi)
            {
                foto = Item.Foto;
            }

            byte[] bytes = Convert.FromBase64String(foto);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            pictureBox1.Image = image;
        }

        private void btnTambah_Click(object sender, EventArgs e)
        {
            cmbNamaUser.Text = "-- Pilih Nama User --";
            cmbNamaBarang.Text = "-- Pilih Nama Barang --";
            cmbStatus.Text = "-- Pilih Status Transaksi --";

            if (cmbNamaUser.Items.Count == 0)
            {
                SemuaUser();
            }
            if (cmbNamaBarang.Items.Count == 0)
            {
                SemuaBarang();
            }

            txtNamaAdmin.Text = _namaAdmin;
            txtTanggalTransaksi.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");

            //Fungsi Button & Textbox
            cmbNamaUser.Enabled = true;
            cmbNamaBarang.Enabled = true;
            cmbStatus.Enabled = true;

            btnTambah.Enabled = false;
            btnSimpan.Enabled = true;
            btnBatal.Enabled = true;
        }

        private void cmbNamaUser_TextChanged(object sender, EventArgs e)
        {
            if (cmbNamaUser.Text == "" || cmbNamaUser.Text == "-- Pilih Nama User --")
            {
            }
            else
            {
                _namaUser = cmbNamaUser.Text;

                UserNama(_namaUser);
            }
        }

        private void btnKembali_Click(object sender, EventArgs e)
        {
            PilihanTransaksi menu = new PilihanTransaksi();
            menu.Show();
            this.Hide();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
        }

        private void btnBatal_Click(object sender, EventArgs e)
        {
            _parameter = 0;

            //Fungsi Button & Textbox
            pictureBox1.Image = null;

            cmbNamaUser.Text = "";
            cmbNamaBarang.Text = "";
            txtNamaAdmin.Text = "";
            txtTotalHarga.Text = "";
            txtTanggalTransaksi.Text = "";
            txtNomorTelpon.Text = "";
            txtAlamat.Text = "";
            cmbStatus.Text = "";

            cmbNamaUser.Enabled = false;
            cmbNamaBarang.Enabled = false;
            cmbStatus.Enabled = false;

            btnSimpan.Text = "Simpan";

            btnTambah.Enabled = true;
            btnSimpan.Enabled = false;
            btnHapus.Enabled = false;
            btnBatal.Enabled = false;
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            HapusTransaksi(_idTransaksi);

            MessageBox.Show("Data Terhapus");

            SemuaTransaksi();

            //Fungsi Button & Textbox
            pictureBox1.Image = null;

            cmbNamaUser.Text = "";
            cmbNamaBarang.Text = "";
            txtNamaAdmin.Text = "";
            txtTotalHarga.Text = "";
            txtTanggalTransaksi.Text = "";
            txtNomorTelpon.Text = "";
            txtAlamat.Text = "";
            cmbStatus.Text = "";

            cmbNamaUser.Enabled = false;
            cmbNamaBarang.Enabled = false;
            cmbStatus.Enabled = false;

            btnSimpan.Text = "Simpan";

            btnTambah.Enabled = true;
            btnSimpan.Enabled = false;
            btnHapus.Enabled = false;
            btnBatal.Enabled = false;

        }

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

        private void btnFilter_Click(object sender, EventArgs e)
        {
            _search = txtFilter.Text;
            _cmbFilter = cmbFilter.Text;
            _cmbStatus = cmbFilterStatus.Text;

            CariTransaksi(_search, _cmbFilter, _cmbStatus);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbNamaUser.Text == "--Pilih Nama User --" || cmbNamaBarang.Text == "--Pilih Nama Barang --" || txtNamaAdmin.Text == "" || txtTotalHarga.Text == "" || txtTanggalTransaksi.Text == "" || txtNomorTelpon.Text == "" || txtAlamat.Text == "" || cmbStatus.Text == "-- Pilih Status Transaksi --")
            {
                MessageBox.Show("Isi Lengkap Semua Data");
            }
            else if (_parameter == 0)
            {
                try
                {
                    dataTransaksiManual trasaksi = new dataTransaksiManual();
                    trasaksi.ID_User = Convert.ToInt16(_idUser);
                    trasaksi.ID_Barang = Convert.ToInt16(_idBarang);
                    trasaksi.ID_Admin = Convert.ToInt16(_idAdmin);
                    trasaksi.Total_Harga = Convert.ToInt32(txtTotalHarga.Text);
                    trasaksi.Tanggal_Transaksi = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
                    trasaksi.Status = cmbStatus.SelectedItem.ToString();

                    SimpanTransaksi(trasaksi);

                    MessageBox.Show("Data Berhasil Ditambahkan");
                }
                catch
                {
                    MessageBox.Show("Data Gagal Ditambahkan ");
                }

                SemuaTransaksi();
            }
            else if (_parameter == 1)
            {
                _parameter = 0;

                try
                {
                    dataTransaksiManual trasaksi = new dataTransaksiManual();
                    trasaksi.ID_Transaksi = Convert.ToInt32(_idTransaksi);
                    trasaksi.ID_User = Convert.ToInt16(_idUser);
                    trasaksi.ID_Barang = Convert.ToInt16(_idBarang);
                    trasaksi.ID_Admin = Convert.ToInt16(_idAdmin);
                    trasaksi.Total_Harga = Convert.ToInt32(txtTotalHarga.Text);
                    trasaksi.Tanggal_Transaksi = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
                    trasaksi.Status = cmbStatus.SelectedItem.ToString();

                    UpdateTransaksi(trasaksi);

                    MessageBox.Show("Data Berhasil Ditambahkan");
                }
                catch
                {
                    MessageBox.Show("Data Gagal Ditambahkan ");
                }

                SemuaTransaksi();
            }

            if (cmbNamaUser.Text == "--Pilih Nama User --" || cmbNamaBarang.Text == "--Pilih Nama Barang --" || txtNamaAdmin.Text == "" || txtTotalHarga.Text == "" || txtTanggalTransaksi.Text == "" || txtNomorTelpon.Text == "" || txtAlamat.Text == "" || cmbStatus.Text == "-- Pilih Status Transaksi --")
            {
            }
            else
            {
                //Fungsi Button & Textbox
                pictureBox1.Image = null;

                cmbNamaUser.Text = "";
                cmbNamaBarang.Text = "";
                txtNamaAdmin.Text = "";
                txtTotalHarga.Text = "";
                txtTanggalTransaksi.Text = "";
                txtNomorTelpon.Text = "";
                txtAlamat.Text = "";
                cmbStatus.Text = "";

                cmbNamaUser.Enabled = false;
                cmbNamaBarang.Enabled = false;
                cmbStatus.Enabled = false;

                btnTambah.Enabled = true;
                btnSimpan.Enabled = false;
                btnBatal.Enabled = false;
            }
        }

        //Fungsi Ketika Combobox Diklik
        private void cmbNamaBarang_TextChanged(object sender, EventArgs e)
        {
            if (cmbNamaBarang.Text == "" || cmbNamaBarang.Text == "-- Pilih Nama Barang --")
            {
            }
            else
            {
                _namaBarang = cmbNamaBarang.Text;

                BarangNama(_namaBarang);
            }
        }

        //BAGIAN FUNGSI & TAMPILAN ----

        //--------------------------------------------------------//

        //BAGIAN LOGIC ----

        public void CariTransaksi(string search, string jenis, string cmbParameter)
        {
            if (search == "" && txtFilter.Visible == true)
            {
                SemuaTransaksi();
            }

            else if (jenis == "Cari Nama")
            {
                var transaksi = new List<dataViewManual>();
                string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewnamamanual/nama=" + search);
                transaksi = JsonConvert.DeserializeObject<List<dataViewManual>>(result);

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
                var transaksi = new List<dataViewManual>();
                string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewidmanual/id=" + search);
                transaksi = JsonConvert.DeserializeObject<List<dataViewManual>>(result);

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
                if (cmbFilterStatus.Text == "Semua")
                {
                    SemuaTransaksi();
                }
                else
                {
                    var transaksi = new List<dataViewManual>();
                    string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewstatusmanual/status=" + cmbParameter);
                    transaksi = JsonConvert.DeserializeObject<List<dataViewManual>>(result);

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
        }

        //Tampil Semua Barang
        public void SemuaBarang()
        {
            var transaksi = new List<dataBarangManual>();
            string result = new WebClient().DownloadString(baseurl + "getallbarangmanual");
            transaksi = JsonConvert.DeserializeObject<List<dataBarangManual>>(result);

            foreach (var Item in transaksi)
            {
                cmbNamaBarang.Items.Add(Item.Nama_Barang);
            }
        }

        public void UpdateTransaksi(dataTransaksiManual transaksi)
        {
            string request = JsonConvert.SerializeObject(transaksi);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "updatetransaksimanual", request);
        }

        public void SimpanTransaksi(dataTransaksiManual transaksi)
        {
            string request = JsonConvert.SerializeObject(transaksi);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "addtransaksimanual", request);
        }

        //Delete Data
        public void HapusTransaksi(string id)
        {
            string request = JsonConvert.SerializeObject(id);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "deletetransaksimanual/id=" + id, request);
        }
    }

    public class dataViewManual
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
        public string Foto { get; set; }
    }

    public class dataTransaksiManual
    {
        public int ID_Transaksi { get; set; }
        public int ID_User { get; set; }
        public int ID_Barang { get; set; }
        public int ID_Admin { get; set; }
        public int Total_Harga { get; set; }
        public string Tanggal_Transaksi { get; set; }
        public string Status { get; set; }
    }

    public class dataBarangManual
    {
        public int ID_Barang { get; set; }
        public string Nama_Barang { get; set; }
        public string Jenis_Barang { get; set; }
        public string Merek { get; set; }
        public int Harga { get; set; }
        public int Stok { get; set; }
        public string Foto { get; set; }
    }

    public class dataPenggunaManual
    {
        public int ID_User { get; set; }
        public string Nama_User { get; set; }
        public string Password { get; set; }
        public string Alamat { get; set; }
        public string No_Telpon { get; set; }
    }
}
