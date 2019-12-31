using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client_Admin
{
    public partial class KelolaTransaksiManual : Form
    {
        // Static Duration Variable adalah lokal variabel yang menggunakan keyword static. membuat variabel tersebut akan tetap hidup (ada) dan tidak menghilangkan nilai di dalamnya, 
        // karena Static Duration Variable tidak terpengaruh oleh lingkungan (ruang linkup), meskipun block/ruang lingkup tersebut tersebut telah berakhir, 
        // variabel tersebut tetap akan ada dan tetap mempertahankan data yang variabel itu simpan.

        //string baseurl = "http://192.168.137.1/service/Service1.svc/";
        string baseurl = "http://localhost:61458/Service1.svc/";

        string _namaAdmin = Login.namaAdmin;
        string _idAdmin = Login.idAdmin;
        string _namaUser, _namaBarang, _idUser, _idBarang, _idTransaksi;
        int _parameter = 0;
        string _search, _cmbFilter, _cmbStatus;

        public KelolaTransaksiManual()
        {
            InitializeComponent();

            //Menjalankan fungsi jam
            timer1.Start();
        }

        //Kondisi Saat Pertama Kali Load
        private void KelolaTransaksiManual_Load(object sender, EventArgs e)
        {
            //Menampilkan Semua Transaksi
            SemuaTransaksi();

            //agar ukuran datagrid sesuai dengan desain yang dibuat 
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //-----------------------
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

        //Kondisi Saat Datagrid Di Klik
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //merubah parameter menjadi 1 agar masuk  ke kondisi update
            _parameter = 1;

            //get data dari data grid sesuai posisi cell dan row
            _idTransaksi = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            string namaUser = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            string namaBarang = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            string namaAdmin = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[3].Value); ;
            string totalHarga = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            string tanggalTransaksi = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[5].Value);
            string NomorTelepon = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
            string Alamat = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[7].Value);
            string Status = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[8].Value);

            //set data kedalam textbox/combobox dari variabel diatas
            cmbNamaUser.Text = namaUser;
            cmbNamaBarang.Text = namaBarang;
            txtNamaAdmin.Text = namaAdmin;
            txtTotalHarga.Text = totalHarga;
            txtTanggalTransaksi.Text = tanggalTransaksi;
            txtNomorTelpon.Text = NomorTelepon;
            txtAlamat.Text = Alamat;
            cmbStatus.Text = Status;

            //menampilkan gambar sesuai nama barang yang di klik di datagrid
            OpenImageStream(namaBarang);

            //kondisi saat combobox belum terisi, maka akan di isi.
            //Namun jika sudah terisi, combobox tidak akan di diisi kembali
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

        //Kondisi Saat Button Tambah Di Klik
        private void btnTambah_Click(object sender, EventArgs e)
        {
            //menambah teks pada semua combobox
            cmbNamaUser.Text = "Pilih Nama User";
            cmbNamaBarang.Text = "Pilih Nama Barang";
            cmbStatus.Text = "Pilih Status Transaksi";

            //kondisi saat combobox belum terisi, maka akan di isi.
            //Namun jika sudah terisi, combobox tidak akan di diisi kembali
            if (cmbNamaUser.Items.Count == 0)
            {
                SemuaUser();
            }
            if (cmbNamaBarang.Items.Count == 0)
            {
                SemuaBarang();
            }

            //menambah nama admin dari data login yang tersedia
            txtNamaAdmin.Text = _namaAdmin;

            //set waktu sekarang ketika menambah transaksi
            txtTanggalTransaksi.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");

            //Fungsi Button & Textbox
            dataGridView1.Enabled = false;

            cmbNamaUser.Enabled = true;
            cmbNamaBarang.Enabled = true;
            cmbStatus.Enabled = true;

            btnTambah.Enabled = false;
            btnSimpan.Enabled = true;
            btnBatal.Enabled = true;
        }

        //Kondisi Saat Combobox User Berganti Nama
        private void cmbNamaUser_TextChanged(object sender, EventArgs e)
        {
            //kondisi combo box berganti nama
            if (cmbNamaUser.Text == "" || cmbNamaUser.Text == "Pilih Nama User")
            {
            }
            else
            {
                _namaUser = cmbNamaUser.Text;

                UserNama(_namaUser);
            }
        }

        //Kondisi Saat Button Kembali Di Klik
        private void btnKembali_Click(object sender, EventArgs e)
        {
            PilihanTransaksi menu = new PilihanTransaksi();
            menu.Show();
            this.Hide();
        }

        //Fungsi Tampil Waktu
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            txtTime.Text = DateTime.Now.ToString("dddd, dd/MM/yyyy hh:mm:ss tt");
        }

        //Kondisi Saat Button Batal Di Klik
        private void btnBatal_Click(object sender, EventArgs e)
        {
            //merubah parameter menjadi 0 agar menjadi kondisi simpan.
            _parameter = 0;

            //Fungsi Button & Textbox
            dataGridView1.Enabled = true;

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

        //Kondisi Saat Button Hapus Di Klik
        private void btnHapus_Click(object sender, EventArgs e)
        {
            //mengakses methode delete dengan parameter id yang sidah dipilih
            HapusTransaksi(_idTransaksi);

            MessageBox.Show("Data Terhapus");

            //menampilkan semua data, ketika data sudah terhapus
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

        //Kondisi Saat Combobox Filter Berganti Nama
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //kondisi saat combobox berubah nama, agar mempermudah dalam fitur search.
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

        //Kondisi Saat Button Filter Di Klik
        private void btnFilter_Click(object sender, EventArgs e)
        {
            _search = txtFilter.Text;
            _cmbFilter = cmbFilter.Text;
            _cmbStatus = cmbFilterStatus.Text;

            //memanggil methode cari transaksi dengan value yang sudah dimasukkan
            CariTransaksi(_search, _cmbFilter, _cmbStatus);
            
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

        //Kondisi Saat Button Simpan Di Klik
        private void button2_Click(object sender, EventArgs e)
        {
            //kondisi simpan atau update
            //ketika parameter 0 maka akan masuk kondisi simpan, jika 1 akan masuk kondisi update
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

                //menampilkan semua transaksi setelah data tersimpan
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

                //menampilkan semua transaksi setelah data terupdate
                SemuaTransaksi();
            }

            if (cmbNamaUser.Text == "--Pilih Nama User --" || cmbNamaBarang.Text == "--Pilih Nama Barang --" || txtNamaAdmin.Text == "" || txtTotalHarga.Text == "" || txtTanggalTransaksi.Text == "" || txtNomorTelpon.Text == "" || txtAlamat.Text == "" || cmbStatus.Text == "-- Pilih Status Transaksi --")
            {
            }
            else
            {
                //Fungsi Button & Textbox
                pictureBox1.Image = null;

                dataGridView1.Enabled = true;

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
            //kondisi combo box berganti nama
            //agar bisa menampilkan foto sesuai nama barang yang dipilih
            if (cmbNamaBarang.Text == "" || cmbNamaBarang.Text == "Pilih Nama Barang")
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

        //Fungsi menampilkan gambar dengan stream
        public void OpenImageStream(string nama)
        {
            //get data dari service dengan value yang dipilih
            var transaksi = new List<dataBarangManual>();
            string result = new WebClient().DownloadString(baseurl + "getbarangnamamanual/nama=" + nama);
            transaksi = JsonConvert.DeserializeObject<List<dataBarangManual>>(result);

            string foto = "";

            //hanya mengambil data foto
            foreach (var Item in transaksi)
            {
                foto = Item.Foto;
            }

            //merubah base64 String menjadi byte[]
            byte[] bytes = Convert.FromBase64String(foto);

            //merubah byte[] menjadi gambar dengan memorystream
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            //set gambar dari hasil convert diatas
            pictureBox1.Image = image;
        }

        //Tampil Semua Transaksi
        public async void SemuaTransaksi()
        //public void SemuaTransaksi()
        {
            ////get data dari service
            //var transaksi = new List<dataViewManual>();
            //string result = new WebClient().DownloadString(baseurl + "getalltransaksiviewmanual");
            //transaksi = JsonConvert.DeserializeObject<List<dataViewManual>>(result);

            //DataTable dt = new DataTable();
            //dt.Columns.Add("ID Transaksi");
            //dt.Columns.Add("Nama User");
            //dt.Columns.Add("Nama Barang");
            //dt.Columns.Add("Nama Admin");
            //dt.Columns.Add("Tanggal Transaksi");
            //dt.Columns.Add("Total Harga");
            //dt.Columns.Add("Alamat Pengiriman");
            //dt.Columns.Add("Nomor HP");
            //dt.Columns.Add("Status");

            ////menampilkan data yang didapat dengan perulangan, agar bisa tampil di datagrid
            //foreach (var Item in transaksi)
            //{
            //    dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
            //}
            //dataGridView1.DataSource = dt;

            HttpClient client = new HttpClient();
            HttpResponseMessage result = new HttpResponseMessage();
            result = client.GetAsync(baseurl + "getalltransaksiviewmanual").Result;
            string data = await result.Content.ReadAsStringAsync();

            //MessageBox.Show("Kode = " + (int)result.StatusCode + " \nDeskripsi : " + result.ReasonPhrase);

            if (result.StatusCode == HttpStatusCode.OK)
            {
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

                var transaksi = new List<dataViewManual>();
                transaksi = JsonConvert.DeserializeObject<List<dataViewManual>>(data);

                foreach (var Item in transaksi)
                {
                    dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
                }
                dataGridView1.DataSource = dt;
            }
            else
            {
                ErrorHandling err = new ErrorHandling();
                err = JsonConvert.DeserializeObject<ErrorHandling>(data);
                MessageBox.Show("ERROR : " + err.Code + "\nDetail : " + err.Detail);
            }
        }

        //Tampil Semua User Nama 
        public void BarangNama(string nama)
        {
            //get data dari service dengan value yang dipilih
            var transaksi = new List<dataBarangManual>();
            string result = new WebClient().DownloadString(baseurl + "getbarangnamamanual/nama=" + nama);
            transaksi = JsonConvert.DeserializeObject<List<dataBarangManual>>(result);

            //hanya mengambil data idbarang dan harga
            //menggunakan perulangan, karena variabel dalam bentuk list
            foreach (var Item in transaksi)
            {
                _idBarang = Convert.ToString(Item.ID_Barang);
                txtTotalHarga.Text = Convert.ToString(Item.Harga);
            }

            //memanggil method untuk menampilkan gambar/foto
            OpenImageStream(nama);
        }

        //Tampil Semua User Nama
        public void UserNama(string nama)
        {
            //get data dari service dengan value yang dipilih
            var transaksi = new List<dataPenggunaManual>();
            string result = new WebClient().DownloadString(baseurl + "getusernamamanual/nama=" + nama);
            transaksi = JsonConvert.DeserializeObject<List<dataPenggunaManual>>(result);

            //hanya mengambil data iduser, nomor handphone dan alamat
            //menggunakan perulangan, karena variabel dalam bentuk list
            foreach (var Item in transaksi)
            {
                _idUser = Convert.ToString(Item.ID_User);
                txtNomorTelpon.Text = Item.No_Telpon;
                txtAlamat.Text = Item.Alamat;
            }
        }

        //Tampil Semua User
        public void SemuaUser()
        {
            //get data dari service dengan value yang dipilih
            var transaksi = new List<dataPenggunaManual>();
            string result = new WebClient().DownloadString(baseurl + "getallusermanual");
            transaksi = JsonConvert.DeserializeObject<List<dataPenggunaManual>>(result);

            //hanya mengambil data namauser
            //menggunakan perulangan, karena variabel dalam bentuk list
            foreach (var Item in transaksi)
            {
                //set data kedalam combobox namauser
                cmbNamaUser.Items.Add(Item.Nama_User);
            }
        }

        //Tampil Cari Transaksi
        public void CariTransaksi(string search, string jenis, string cmbParameter)
        {
            //kondisi pada saat cari
            //terdapat pilihan fitur cari bisa dengan 3 variabel, yaitu berdasar namauser, berdasar idtransaksi, dan berdasar status transaksi
            if (search == "" && txtFilter.Visible == true)
            {
                //menampilkan semua transaksi
                SemuaTransaksi();
            }

            else if (jenis == "Cari Nama")
            {
                //get data dari service dengan value yang dipilih
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

                //menampilkan data yang didapat dengan perulangan, agar bisa tampil di datagrid
                foreach (var Item in transaksi)
                {
                    dt.Rows.Add(new object[] { Item.ID_Transaksi, Item.Nama_User, Item.Nama_Barang, Item.Nama_Admin, Item.Total_Harga, Item.Tanggal_Transaksi, Item.Alamat, Item.No_Telpon, Item.Status });
                }
                dataGridView1.DataSource = dt;

            }

            else if (jenis == "Cari ID")
            {
                //get data dari service dengan value yang dipilih
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

                //menampilkan data yang didapat dengan perulangan, agar bisa tampil di datagrid
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
                    //menampilkan semua transaksi
                    SemuaTransaksi();
                }
                else
                {
                    //get data dari service dengan value yang dipilih
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

                    //menampilkan data yang didapat dengan perulangan, agar bisa tampil di datagrid
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
            //get data dari service dengan value yang dipilih
            var transaksi = new List<dataBarangManual>();
            string result = new WebClient().DownloadString(baseurl + "getallbarangmanual");
            transaksi = JsonConvert.DeserializeObject<List<dataBarangManual>>(result);

            //hanya mengambil data namabarang
            //menggunakan perulangan, karena variabel dalam bentuk list
            foreach (var Item in transaksi)
            {
                //set data kedalam combobox namauser
                cmbNamaBarang.Items.Add(Item.Nama_Barang);
            }
        }

        //Update Data
        public void UpdateTransaksi(dataTransaksiManual transaksi)
        {
            //memanggil fungsi updatedata
            string request = JsonConvert.SerializeObject(transaksi);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "updatetransaksimanual", request);
        }

        //Simpan Data
        public void SimpanTransaksi(dataTransaksiManual transaksi)
        {
            //memanggil fungsi simpandata
            string request = JsonConvert.SerializeObject(transaksi);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "addtransaksimanual", request);
        }

        //Delete Data
        public void HapusTransaksi(string id)
        {
            //memanggil fungsi Hapusdata
            string request = JsonConvert.SerializeObject(id);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "deletetransaksimanual/id=" + id, request);
        }
    }

    //--------------------------------
    //Class Model Khusus Ujian Mandiri
    //--------------------------------

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

    public class ErrorHandling
    {
        public string Code { get; set; }
        public string Detail { get; set; }
    }
}
