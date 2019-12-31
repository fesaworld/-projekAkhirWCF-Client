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
using System.IO;

namespace Client_Admin
{
    public partial class KelolaBarang : Form
    {
        string baseurl = "http://192.168.137.1/service/Service1.svc/";

        string id;
        int kondisi = 0;

        public KelolaBarang()
        {
            InitializeComponent();
            SemuaBarang();

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        //Cari Data Barang
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;

            string _parameter = txtFilter.Text;
            string _jenis = cmbFilter.Text;

            CariBarang(_parameter, _jenis);

            //Kondisi Dan fungsi Button & Textbox
            txtNama.Enabled = false;
            txtJenis.Enabled = false;
            txtMerek.Enabled = false;
            txtHarga.Enabled = false;
            txtStok.Enabled = false;

            txtNama.Text = "";
            txtJenis.Text = "";
            txtMerek.Text = "";
            txtHarga.Text = "";
            txtStok.Text = "";

            btnTambah.Enabled = true;
            btnSimpan.Enabled = false;
            btnHapus.Enabled = false;
            btnBatal.Enabled = false;
        }

        //Back Menu
        private void button4_Click(object sender, EventArgs e)
        {
            Menu_Utama menu = new Menu_Utama();
            menu.Show();
            this.Hide();
        }

        //get data dari grid ke textbox
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            id = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            string nama = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            string jenis = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            string merek = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[3].Value);
            string harga = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            string stok = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[5].Value);

            //Kondisi Dan fungsi Button & Textbox
            txtNama.Text = nama;
            txtJenis.Text = jenis;
            txtMerek.Text = merek;
            txtHarga.Text = harga;
            txtStok.Text = stok;

            //Pakai Streams
            OpenImageStream(id);

            txtNama.Enabled = true;
            txtJenis.Enabled = true;
            txtMerek.Enabled = true;
            txtHarga.Enabled = true;
            txtStok.Enabled = true;

            btnGambarOpen.Enabled = true;
            btnTambah.Enabled = false;
            btnHapus.Enabled = true;
            btnSimpan.Enabled = true;
            btnBatal.Enabled = true;

            btnSimpan.Text = "Update";
            kondisi = 1;

        }

        //kondisi saat pertamakali load
        private void KelolaBarang_Load(object sender, EventArgs e)
        {
            //Kondisi Dan fungsi Button & Textbox
            txtNama.Enabled = false;
            txtJenis.Enabled = false;
            txtMerek.Enabled = false;
            txtHarga.Enabled = false;
            txtStok.Enabled = false;
            txtGambarPath.Enabled = false;

            btnGambarOpen.Enabled = false;
            btnSimpan.Enabled = false;
            btnHapus.Enabled = false;
            btnBatal.Enabled = false;
        }

        //fungsi simpan dan update barang
        private void btLogin_Click(object sender, EventArgs e)
        {
            dataBarang barang = new dataBarang();

            if (txtNama.Text == "" || txtJenis.Text == "" || txtMerek.Text == "" || txtHarga.Text == "" || txtStok.Text == "")
            {
                MessageBox.Show("Semua Kolom Harus Diisi..");
            }
            else if (kondisi == 0)
            {
                if (txtGambarPath.Text == "")
                {
                    MessageBox.Show("Semua Kolom Harus Diisi..");
                }
                else
                {
                    //Menambahkan data ke Database
                    try
                    {
                        //ini pake base64 string
                        Image image = Image.FromFile(txtGambarPath.Text);
                        ImageConverter conv = new ImageConverter();
                        byte[] bitimg = (byte[])conv.ConvertTo(image, typeof(byte[]));
                        string foto = Convert.ToBase64String(bitimg);

                        barang.ID_Barang = Convert.ToInt32(id);
                        barang.Nama_Barang = txtNama.Text;
                        barang.Jenis_Barang = txtJenis.Text;
                        barang.Merek = txtMerek.Text;
                        barang.Harga = Convert.ToInt32(txtHarga.Text);
                        barang.Stok = Convert.ToInt32(txtStok.Text);
                        barang.Foto = foto;

                        SimpanBarang(barang);

                        MessageBox.Show("Data Tersimpan");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eror, Semua Data Harus Diisi! : " + ex);
                    }
                }
            }
            else if (kondisi == 1)
            {
                kondisi = 0;
                btnSimpan.Text = "Simpan";

                //Mengupdate data ke Database
                if (txtGambarPath.Text == "")
                {
                    NewMethod(barang);
                }
                else
                {
                    try
                    {
                        //ini pake base64 string
                        Image image = Image.FromFile(txtGambarPath.Text);
                        ImageConverter conv = new ImageConverter();
                        byte[] bitimg = (byte[])conv.ConvertTo(image, typeof(byte[]));
                        string foto = Convert.ToBase64String(bitimg);

                        barang.ID_Barang = Convert.ToInt32(id);
                        barang.Nama_Barang = txtNama.Text;
                        barang.Jenis_Barang = txtJenis.Text;
                        barang.Merek = txtMerek.Text;
                        barang.Harga = Convert.ToInt32(txtHarga.Text);
                        barang.Stok = Convert.ToInt32(txtStok.Text);
                        barang.Foto = foto;
                        UpdateBarang(barang);

                        MessageBox.Show("Data Terupdate");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Eror, Semua Data Harus Diisi! : " + ex);
                    }
                }
            }
            SemuaBarang();

            if (txtNama.Text == "" || txtJenis.Text == "" || txtMerek.Text == "" || txtHarga.Text == "" || txtStok.Text == "")
            {

            }
            else
            {
                //Kondisi Dan fungsi Button & Textbox
                pictureBox1.Image = null;

                dataGridView1.Enabled = true;

                txtNama.Enabled = false;
                txtJenis.Enabled = false;
                txtMerek.Enabled = false;
                txtHarga.Enabled = false;
                txtStok.Enabled = false;

                cmbFilter.Enabled = true;
                txtFilter.Enabled = true;
                btnCari.Enabled = true;

                txtNama.Text = "";
                txtJenis.Text = "";
                txtMerek.Text = "";
                txtHarga.Text = "";
                txtStok.Text = "";
                txtGambarPath.Text = "";

                btnGambarOpen.Enabled = false;
                btnTambah.Enabled = true;
                btnSimpan.Enabled = false;
                btnBatal.Enabled = false;
                btnHapus.Enabled = false;

            }
        }

        private void NewMethod(dataBarang barang)
        {
            try
            {
                barang.ID_Barang = Convert.ToInt32(id);
                barang.Nama_Barang = txtNama.Text;
                barang.Jenis_Barang = txtJenis.Text;
                barang.Merek = txtMerek.Text;
                barang.Harga = Convert.ToInt32(txtHarga.Text);
                barang.Stok = Convert.ToInt32(txtStok.Text);
                UpdateBarangNoFoto(barang);

                MessageBox.Show("Data Terupdate");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eror, Semua Data Harus Diisi! : " + ex);
            }
        }

        //fungsi Mengaktifkan Textbox buat tambah data
        private void button5_Click(object sender, EventArgs e)
        {
            //Kondisi Dan fungsi Button & Textbox
            txtNama.Enabled = true;
            txtJenis.Enabled = true;
            txtMerek.Enabled = true;
            txtHarga.Enabled = true;
            txtStok.Enabled = true;

            cmbFilter.Enabled = false;
            txtFilter.Enabled = false;
            btnCari.Enabled = false;

            btnGambarOpen.Enabled = true;
            btnSimpan.Enabled = true;
            btnBatal.Enabled = true;
            btnTambah.Enabled = false;
            btnHapus.Enabled = false;

            dataGridView1.Enabled = false;
        }

        //fungsi hapus Data Barang
        private void button1_Click(object sender, EventArgs e)
        {
            HapusBarang(id);

            SemuaBarang();

            MessageBox.Show("Data Terhapus");

            //Kondisi Dan fungsi Button & Textbox
            pictureBox1.Image = null;

            txtNama.Enabled = false;
            txtJenis.Enabled = false;
            txtMerek.Enabled = false;
            txtHarga.Enabled = false;
            txtStok.Enabled = false;

            cmbFilter.Enabled = true;
            txtFilter.Enabled = true;
            btnCari.Enabled = true;

            txtNama.Text = "";
            txtJenis.Text = "";
            txtMerek.Text = "";
            txtHarga.Text = "";
            txtStok.Text = "";
            txtGambarPath.Text = "";

            btnSimpan.Text = "Simpan";

            btnHapus.Enabled = false;
            btnBatal.Enabled = false;
            btnSimpan.Enabled = false;
            btnTambah.Enabled = true;
        }

        //Fungsi menampilkan open image
        private void button1_Click_1(object sender, EventArgs e)
        { 
            try
            {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
                DialogResult dlgRes = dlg.ShowDialog();
                if (dlgRes != DialogResult.Cancel)
                {
                    //Provide file path in txtImagePath text box.
                    txtGambarPath.Text = dlg.FileName;

                    Image image = Image.FromFile(txtGambarPath.Text);
                    pictureBox1.Image = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Eror, Cek File Gambar Anda  : " + ex.Message);
            }
        }

        //fungsi batal
        private void button2_Click(object sender, EventArgs e)
        {
            //Kondisi Dan fungsi Button & Textbox
            kondisi = 0;

            pictureBox1.Image = null;

            txtNama.Enabled = false;
            txtJenis.Enabled = false;
            txtMerek.Enabled = false;
            txtHarga.Enabled = false;
            txtStok.Enabled = false;

            cmbFilter.Enabled = true;
            txtFilter.Enabled = true;
            btnCari.Enabled = true;

            txtNama.Text = "";
            txtJenis.Text = "";
            txtMerek.Text = "";
            txtHarga.Text = "";
            txtStok.Text = "";
            txtGambarPath.Text = "";

            btnSimpan.Text = "Simpan";

            btnTambah.Enabled = true;
            btnBatal.Enabled = false;
            btnSimpan.Enabled = false;
            btnHapus.Enabled = false;
            btnGambarOpen.Enabled = false;

            dataGridView1.Enabled = true;
        }

        private void txtHarga_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtStok_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        //BAGIAN FUNGSI & TAMPILAN ----

        //--------------------------------------------------------//

        //BAGIAN LOGIC ----

        //Fungsi menampilkan gambar dengan stream
        public void OpenImageStream(string id)
        {
            dataBarang pesanan = new dataBarang();
            string result = new WebClient().DownloadString(baseurl + "getbarangid/id=" + id);
            pesanan = JsonConvert.DeserializeObject<dataBarang>(result);

            string foto = pesanan.Foto;

            byte[] bytes = Convert.FromBase64String(foto);

            Image imagge;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                imagge = Image.FromStream(ms);
            }

            pictureBox1.Image = imagge;
        }

        //Cari Barang
        public void CariBarang(string parameter, string jenis)
        {
            if (parameter == "")
            {
                SemuaBarang();
            }

            else if (jenis == "Cari Nama")
            {
                var barangnama = new List<dataBarang>();
                string result = new WebClient().DownloadString(baseurl + "getbarangnama/nama=" + parameter);
                barangnama = JsonConvert.DeserializeObject<List<dataBarang>>(result);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID Barang");
                dt.Columns.Add("Nama Barang");
                dt.Columns.Add("Jenis Barang");
                dt.Columns.Add("Merek");
                dt.Columns.Add("Harga");
                dt.Columns.Add("Stok");
                dt.Columns.Add("Foto");
                foreach (var Item in barangnama)
                {
                    dt.Rows.Add(new object[] { Item.ID_Barang, Item.Nama_Barang, Item.Jenis_Barang, Item.Merek, Item.Harga, Item.Stok });
                }

                dataGridView1.DataSource = dt;
            }

            else if (jenis == "Cari Jenis")
            {
                var barangnama = new List<dataBarang>();
                string result = new WebClient().DownloadString(baseurl + "getbarangjenis/jenis=" + parameter);
                barangnama = JsonConvert.DeserializeObject<List<dataBarang>>(result);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID Barang");
                dt.Columns.Add("Nama Barang");
                dt.Columns.Add("Jenis Barang");
                dt.Columns.Add("Merek");
                dt.Columns.Add("Harga");
                dt.Columns.Add("Stok");
                dt.Columns.Add("Foto");
                foreach (var Item in barangnama)
                {
                    dt.Rows.Add(new object[] { Item.ID_Barang, Item.Nama_Barang, Item.Jenis_Barang, Item.Merek, Item.Harga, Item.Stok });
                }

                dataGridView1.DataSource = dt;
            }

            else if (jenis == "Cari Merek")
            {
                var barangnama = new List<dataBarang>();
                string result = new WebClient().DownloadString(baseurl + "getbarangmerek/merek=" + parameter);
                barangnama = JsonConvert.DeserializeObject<List<dataBarang>>(result);

                DataTable dt = new DataTable();
                dt.Columns.Add("ID Barang");
                dt.Columns.Add("Nama Barang");
                dt.Columns.Add("Jenis Barang");
                dt.Columns.Add("Merek");
                dt.Columns.Add("Harga");
                dt.Columns.Add("Stok");
                foreach (var Item in barangnama)
                {
                    dt.Rows.Add(new object[] { Item.ID_Barang, Item.Nama_Barang, Item.Jenis_Barang, Item.Merek, Item.Harga, Item.Stok });
                }
                dataGridView1.DataSource = dt;
            }
        }

        //Tampil Semua Data Ke Grid
        public void SemuaBarang()
        {
            var barang = new List<dataBarang>();
            string result = new WebClient().DownloadString(baseurl + "getallbarang");
            barang = JsonConvert.DeserializeObject<List<dataBarang>>(result);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID Barang");
            dt.Columns.Add("Nama Barang");
            dt.Columns.Add("Jenis Barang");
            dt.Columns.Add("Merek");
            dt.Columns.Add("Harga");
            dt.Columns.Add("Stok");

            foreach (var Item in barang)
            {
                dt.Rows.Add(new object[] { Item.ID_Barang, Item.Nama_Barang, Item.Jenis_Barang, Item.Merek, Item.Harga, Item.Stok});
            }
            dataGridView1.DataSource = dt;
        }

        //Tambah Data
        public void SimpanBarang(dataBarang barang)
        {
            string request = JsonConvert.SerializeObject(barang);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "addbarang", request);
        }

        //Update Data With foto
        public void UpdateBarang(dataBarang barang)
        {
            string request = JsonConvert.SerializeObject(barang);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "updatebarang", request);
        }

        //Update Data no foto
        public void UpdateBarangNoFoto(dataBarang barang)
        {
            string request = JsonConvert.SerializeObject(barang);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "updatebarangnofoto", request);
        }

        //Delete Data
        public void HapusBarang(string id)
        {
            string request = JsonConvert.SerializeObject(id);
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            string result = client.UploadString(baseurl + "deletebarang/id=" + id, request);
        }
    }

    public class dataBarang
    {
        public int ID_Barang { get; set; }
        public string Nama_Barang { get; set; }
        public string Jenis_Barang { get; set; }
        public string Merek { get; set; }
        public int Harga { get; set; }
        public int Stok { get; set; }
        public string Foto { get; set; }
    }
}
