using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Fuzzy_Logic_Tsukamoto
{
    public partial class form_main : Form
    {
        public List<AlphaData> alphaDataList { get; set; }
        public List<ZData> ZDataList { get; set; }
        //Declare Variable
        double persediaan, penjualan, pembelian, total_alpha, alpha_mult_z;
        double[] mu_persediaan = new double[3];
        double[] mu_penjualan = new double[3]; // Index guide (0= Banyak, 1 = Sedang, 2 = Sedikit)
        double[,] alpha = new double[3, 3];
        double[,] Z = new double[3, 3];

        public form_main()
        {
            InitializeComponent();
            InitializeList();
            datagridview.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        //Initialisasi List
        private void InitializeList()
        {
            alphaDataList = new List<AlphaData>
             {
                new AlphaData { NilaiAlpha = "Persediaan Banyak" },
                new AlphaData { NilaiAlpha = "Persediaan Sedang" },
                new AlphaData { NilaiAlpha = "Persediaan Sedikit" }
                };
            ZDataList = new List<ZData>
            {
                new ZData { NilaiZ = "Persediaan Banyak" },
                new ZData { NilaiZ = "Persediaan Sedang" },
                new ZData { NilaiZ = "Persediaan Sedikit" }
            };
        }
        //Customized Function
       private void UpdateDataGridValue()
        {
            alphaDataList[0].PenjualanBanyak = Math.Round(alpha[0, 0], 4);//Penjualan Banyak
            alphaDataList[0].PenjualanSedang = Math.Round(alpha[0, 1], 4);
            alphaDataList[0].PenjualanSedikit = Math.Round(alpha[0, 2], 4);

            alphaDataList[1].PenjualanBanyak = Math.Round(alpha[1, 0], 4);//Penjualan Sedang
            alphaDataList[1].PenjualanSedang = Math.Round(alpha[1, 1], 4);
            alphaDataList[1].PenjualanSedikit = Math.Round(alpha[1, 2], 4);

            alphaDataList[2].PenjualanBanyak = Math.Round(alpha[2, 0], 4);//Penjualan Sedikit
            alphaDataList[2].PenjualanSedang = Math.Round(alpha[2, 1], 4);
            alphaDataList[2].PenjualanSedikit = Math.Round(alpha[2, 2], 4);

            ZDataList[0].PenjualanBanyak = Math.Round(Z[0, 0], 4);
            ZDataList[0].PenjualanSedang = Math.Round(Z[0, 1], 4);
            ZDataList[0].PenjualanSedikit = Math.Round(Z[0, 2], 4);

            ZDataList[1].PenjualanBanyak = Math.Round(Z[1, 0], 4);
            ZDataList[1].PenjualanSedang = Math.Round(Z[1, 1], 4);
            ZDataList[1].PenjualanSedikit = Math.Round(Z[1, 2], 4);

            ZDataList[2].PenjualanBanyak = Math.Round(Z[2, 0], 4);
            ZDataList[2].PenjualanSedang = Math.Round(Z[2, 1], 4);
            ZDataList[2].PenjualanSedikit = Math.Round(Z[2, 2], 4);
        }
        private double Min(double value_1, double value_2)
        {
            if (value_1 < value_2)
            {
                return value_1;
            }
            else
            {
                return value_2;
            }
        }

        private void btn_updategambar_Click(object sender, EventArgs e)
        {
            try
            {
                string pilihan = combobox_visualisasi.SelectedItem.ToString();
                switch (pilihan)
                {
                    case "Tabel Nilai α":
                        datagridview.DataSource = null;
                        datagridview.DataSource = alphaDataList;
                        datagridview.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        datagridview.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "Tabel Nilai Z":
                        datagridview.DataSource = null;
                        datagridview.DataSource = ZDataList;
                        datagridview.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        datagridview.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        break;

                    case "Grafik Fuzzification Persediaan":
                        break;

                    case "Grafik Fuzzification Penjualan":
                        break;

                    case "Grafik Fuzzification Pembelian":
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                return;
            }
        }

        private double pembelian_banyak(double alpha)
        {
            if (alpha >= 1)
            {
                return 150;
            }
            else if (alpha >= 0)
            {
                return ((90 * alpha) + 60);
            }
            else
            {
                return 60;
            }
        }
        private double pembelian_sedikit(double alpha)
        {
            if (alpha >= 1)
            {
                return 90;
            }
            else if (alpha >= 0)
            {
                return (90 - (90 * alpha));
            }
            else
            {
                return 0;
            }
        }
        // Event Function
        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_calculate_Click(object sender, EventArgs e)
        {
            try
            {
                persediaan = double.Parse(txtbox_persediaan.Text);
                penjualan = double.Parse(txtbox_penjualan.Text);
            }
            catch (FormatException)
            {
                // Handle the case where the text is not a valid double
                Console.WriteLine("Error: Bukan Angka");
                return;
            }
            catch (OverflowException)
            {
                // Handle the case where the entered number is too large or too small
                Console.WriteLine("Error: Terlalu besar/Kecil");
                return;
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
                return;
            }

            // Membership Persediaan
            if (persediaan >= 176)
            {
                mu_persediaan[0] = 1;
            }
            else if (persediaan >= 159.7)
            {
                mu_persediaan[0] = (persediaan - 110.8) / 65.2;
            }
            else if (persediaan >= 110.8)
            {
                mu_persediaan[0] = (persediaan - 110.8) / 65.2;
                mu_persediaan[1] = (159.7 - persediaan) / 65.2;
            }
            else if (persediaan >= 94.5)
            {
                mu_persediaan[1] = (159.7 - persediaan) / 65.2;
            }
            else if (persediaan >= 78.2)
            {
                mu_persediaan[1] = (persediaan - 29.3) / 65.2;
            }
            else if (persediaan >= 29.3)
            {
                mu_persediaan[1] = (persediaan - 29.3) / 65.2;
                mu_persediaan[2] = (78.2 - persediaan) / 78.2;
            }
            else if (persediaan >= 0)
            {
                mu_persediaan[2] = (78.2 - persediaan) / 78.2;
            }
            else
            {
                mu_persediaan[2] = 1;
            }
            //Membership Penjualan
            // Membership Penjualan
            if (penjualan >= 209)
            {
                mu_penjualan[0] = 1;
            }
            else if (penjualan >= 192.1)
            {
                mu_penjualan[0] = (penjualan - 141.4) / 67.6;
            }
            else if (penjualan >= 141.1)
            {
                mu_penjualan[0] = (penjualan - 141.4) / 67.6;
                mu_penjualan[1] = (192.1 - penjualan) / 67.6;
            }
            else if (penjualan >= 124.5)
            {
                mu_penjualan[1] = (192.1 - penjualan) / 67.6;
            }
            else if (penjualan >= 107.6)
            {
                mu_penjualan[1] = (penjualan - 56.9) / 67.6;
            }
            else if (penjualan >= 56.9)
            {
                mu_penjualan[1] = (penjualan - 56.9) / 67.6;
                mu_penjualan[2] = (107.6 - penjualan) / 107.6;
            }
            else if (penjualan >= 0)
            {
                mu_penjualan[2] = (107.6 - penjualan) / 107.6;
            }
            else
            {
                mu_penjualan[2] = 1;
            }

            //Menghitung Alpha (1-9)
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    alpha[i, j] = Min(mu_persediaan[i], mu_penjualan[j]);
                    /*
                       tabel alpha   0 1 2 penjualan   
                        0            B S S      
                        1            B B S
                        2            B B S
                       persediaan
                       artinya : alpha[persediaan,penjualan]
                    */
                    if (i == 0 && (j != 0))
                    {
                        Z[i, j] = pembelian_sedikit(alpha[i, j]);
                    }
                    else if (i == 1 && j == 2)
                    {
                        Z[i, j] = pembelian_sedikit(alpha[i, j]);
                    }
                    else if (i == 2 && j == 2)
                    {
                        Z[i, j] = pembelian_sedikit(alpha[i, j]);
                    }
                    else
                    {
                        Z[i, j] = pembelian_banyak(alpha[i, j]);
                    }
                    total_alpha += alpha[i, j];
                    alpha_mult_z += alpha[i, j] * Z[i, j];
                }
            }
            pembelian = alpha_mult_z / total_alpha;
            label4.Text = "Rekomendasi Jumlah Pembelian : " + pembelian.ToString("F2");
            btn_updategambar.Enabled = true;
            combobox_visualisasi.Enabled = true;
            UpdateDataGridValue();
        }
    }
}
