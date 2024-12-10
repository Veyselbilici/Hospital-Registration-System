using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using static System.Net.Mime.MediaTypeNames;
namespace Hastabilgisistemi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private bool KullaniciBulunduMu;
        private string KullaniciTC;

        public static NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=HASTA_BILGI_SISTEMI;User Id = postgres; Password=1234");

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            VerileriGetir();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VerileriGetir();
        }

        private void VerileriGetir()
        {

            var txt = "SELECT * FROM public.\"HASTA\"";
            conn.Open();
            NpgsqlCommand komut = new NpgsqlCommand();
            komut.Connection = conn;
            komut.CommandType = CommandType.Text;
            komut.CommandText = txt;
            NpgsqlDataReader dr = komut.ExecuteReader();
            if (dr.HasRows)
            {
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView2.DataSource = dt;
            }
            komut.Dispose();
            conn.Close();

        }
        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }


        private void button3_Click(object sender, EventArgs e)
        {
            string tc = textBox1.Text;

            KullaniciTC = "";
            KullaniciBulunduMu = false;

            var sqlStr = $"SELECT * FROM public.\"HASTA\" WHERE \"TC_KIMLIK_NO\" = '{tc}'";

            using (NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; Port = 5432; Database = HASTA_BILGI_SISTEMI; User Id = postgres; Password = 12345"))
            {
                conn.Open();

                using (NpgsqlCommand komut = new NpgsqlCommand(sqlStr, conn))
                {
                    using (NpgsqlDataReader dr = komut.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            textBox2.Text = dr.GetString(2);
                            textBox2.Enabled = true;
                            textBox3.Text = dr.GetString(3);
                            textBox3.Enabled = true;
                            textBox4.Text = dr.GetString(4);
                            textBox4.Enabled = true;
                            dateTimePicker1.Value = DateTime.Parse(dr.GetString(5));
                            dateTimePicker1.Enabled = true;
                            textBox6.Text = dr.GetString(6);
                            textBox6.Enabled = true;
                            textBox7.Text = dr.GetString(7);
                            textBox7.Enabled = true;

                            if (!dr.IsDBNull(8))
                            {
                                textBox8.Text = dr.GetString(8);
                                textBox8.Enabled = true;
                            }
                            else
                            {
                                textBox8.Text = "";
                                textBox8.Enabled = false;
                            }

                            if (!dr.IsDBNull(9))
                            {
                                textBox9.Text = dr.GetString(9);
                                textBox9.Enabled = true;
                            }
                            else
                            {
                                textBox9.Text = "";
                                textBox9.Enabled = false;
                            }

                            KullaniciBulunduMu = true;
                            KullaniciTC = tc;
                        }
                      
                    }
                }
            }

            VerileriGetir();

        }

        private void button2_Click(object sender, EventArgs e)
        {

         
            
                if (!KullaniciBulunduMu)
                {
                    MessageBox.Show("Kullanıcı bulunamadı! Lütfen doğru bir TC girdikten sonra tekrar deneyiniz.");
                    return;
                }

                var sqlStr = $"DELETE FROM public.\"HASTA\" WHERE \"TC_KIMLIK_NO\" = '{KullaniciTC}'";

                try
                {
                    using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=HASTA_BILGI_SISTEMI;User Id=postgres;Password=1234"))
                    {
                        conn.Open();

                        using (NpgsqlCommand komut = new NpgsqlCommand(sqlStr, conn))
                        {
                            int affectedRows = komut.ExecuteNonQuery();

                            if (affectedRows > 0)
                            {
                                MessageBox.Show("İşlem Başarılı");
                                VerileriGetir();
                            }
                            else
                            {
                                MessageBox.Show("İşlem Başarısız");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }
            }


        private void button1_Click(object sender, EventArgs e)
        {


                using (NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;Database=HASTA_BILGI_SISTEMI;User Id=postgres;Password=1234"))
                {
                    conn.Open();

                    var sqlStr = "INSERT INTO public.\"HASTA\" " +
                                  "(\"AD\", \"SOYAD\", \"TELEFON_NO\", \"DOGUM_TARIHI\", \"DOGUM_YERI\", \"ADRES\", \"TESHIS\", \"ILAC\") " +
                                  "VALUES " +
                                  "(@Ad, @Soyad, @Telefon, @DogumTarihi, @DogumYeri, @Adres, @Teshis, @Ilac)";

                    using (NpgsqlCommand komut = new NpgsqlCommand(sqlStr, conn))
                    {
                        komut.Parameters.AddWithValue("@Ad", NpgsqlDbType.Text, textBox2.Text);
                        komut.Parameters.AddWithValue("@Soyad", NpgsqlDbType.Text, textBox3.Text);
                        komut.Parameters.AddWithValue("@Telefon", NpgsqlDbType.Text, textBox4.Text);
                        komut.Parameters.AddWithValue("@DogumTarihi", NpgsqlDbType.Date, dateTimePicker1.Value);
                        komut.Parameters.AddWithValue("@DogumYeri", NpgsqlDbType.Text, textBox6.Text);
                        komut.Parameters.AddWithValue("@Adres", NpgsqlDbType.Text, textBox7.Text);
                        komut.Parameters.AddWithValue("@Teshis", NpgsqlDbType.Text, textBox8.Text);
                        komut.Parameters.AddWithValue("@Ilac", NpgsqlDbType.Text, textBox9.Text);

                        int affectedRows = komut.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Yeni hasta başarıyla eklendi.");
                            VerileriGetir();
                        }
                        else
                        {
                            MessageBox.Show("Hasta ekleme işlemi başarısız.");
                        }
                    }
                }
            }




            private void button4_Click(object sender, EventArgs e)
        {
            
            
                // Kullanıcının girdiği verileri al
                string ad = textBox2.Text;
                string soyad = textBox3.Text;
                string telefon = textBox4.Text;
                string dogumTarihi = dateTimePicker1.Value.ToString("yyyy-MM-dd");
                string dogumYeri = textBox6.Text;
                string adres = textBox7.Text;
                string teshis = textBox8.Text;
                string ilac = textBox9.Text;

                // Güncellenecek hastanın TC Kimlik Numarası
                string tcKimlikNo = KullaniciTC;

                using (NpgsqlConnection conn = new NpgsqlConnection("Server = localhost; Port = 5432; Database = HASTA_BILGI_SISTEMI; User Id = postgres; Password = 1234"))
                {
                    conn.Open();

                    var sqlStr = "UPDATE public.\"HASTA\" " +
                                  "SET \"AD\"= @Ad, \"SOYAD\"= @Soyad, \"TELEFON_NO\"= @Telefon, " +
                                  "\"DOGUM_TARIHI\"= @DogumTarihi, \"DOGUM_YERI\"= @DogumYeri, " +
                                  "\"ADRES\"= @Adres, \"TESHIS\"= @Teshis, \"ILAC\"= @Ilac " +
                                  "WHERE \"TC_KIMLIK_NO\" = @TC";

                try
                {
                    using (NpgsqlCommand komut = new NpgsqlCommand(sqlStr, conn))
                    {
                        komut.Parameters.AddWithValue("@TC", NpgsqlDbType.Text, (object)tcKimlikNo ?? DBNull.Value);
                        komut.Parameters.AddWithValue("@Ad", NpgsqlDbType.Text, ad);
                        komut.Parameters.AddWithValue("@Soyad", NpgsqlDbType.Text, soyad);
                        komut.Parameters.AddWithValue("@Telefon", NpgsqlDbType.Text, telefon);
                        komut.Parameters.AddWithValue("@DogumTarihi", NpgsqlDbType.Date, dateTimePicker1.Value);
                        komut.Parameters.AddWithValue("@DogumYeri", NpgsqlDbType.Text, dogumYeri);
                        komut.Parameters.AddWithValue("@Adres", NpgsqlDbType.Text, adres);
                        komut.Parameters.AddWithValue("@Teshis", NpgsqlDbType.Text, teshis);
                        komut.Parameters.AddWithValue("@Ilac", NpgsqlDbType.Text, ilac);


                        int affectedRows = komut.ExecuteNonQuery();

                        if (affectedRows > 0)
                        {
                            MessageBox.Show("Hasta bilgileri başarıyla güncellendi.");
                            VerileriGetir(); // Verileri güncelleme işlemi
                        }
                        else
                        {
                            MessageBox.Show("Hasta bilgileri güncelleme işlemi başarısız.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata oluştu: " + ex.Message);
                }


            }


        }

        

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
 }

