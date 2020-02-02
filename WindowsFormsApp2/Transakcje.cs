using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Transakcje : Form
    {
        public Transakcje()
        {
            InitializeComponent();
            this.refresh_summary();
            this.refresh_shops();
            this.refresh_products();
        }

        void refresh_shops()
        {
            string sql = "select Nazwa from dimSklep group by Nazwa";

            DB_handling.open_connection();
            SqlDataAdapter sklepy = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            sklepy.Fill(dt);

            this.comboBox1.DataSource = dt;
            this.comboBox1.DisplayMember = "Nazwa";
            this.comboBox1.SelectedIndex = -1;
        }

        void refresh_products()
        {
            string sql = "select Nazwa from dimProdukt group by Nazwa";

            DB_handling.open_connection();
            SqlDataAdapter sklepy = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            sklepy.Fill(dt);

            this.comboBox2.DataSource = dt;
            this.comboBox2.DisplayMember = "Nazwa";
            this.comboBox2.SelectedIndex = -1;
        }
        void refresh_summary()
        {
            string shop_filter = this.comboBox1.Text;
            int is_shop_filt = (shop_filter == "") ? 0 : 1;

            string product_filter = this.comboBox2.Text;
            int  is_prod_filt = (product_filter == "") ? 0: 2;

            int query_flag = is_prod_filt + is_shop_filt;
            string sql = "";
            switch (query_flag)
            {
                case 0:
                    sql = "select Numer, Produkt, Sklep, Data, KtoKomu, Kwota " +
                          "from factZakup zak left join dimData dat on zak.Data = dat.Klucz " +
                          "order by dat.Rok desc, dat.Miesiąc desc, dat.Dzień desc";
                    break;

                case 1:
                    sql = "select Numer, Produkt, Sklep, Data, KtoKomu, Kwota " +
                          "from factZakup zak left join dimData dat on zak.Data = dat.Klucz " +
                          $"where Sklep like \'{shop_filter}%\'" +
                          "order by dat.Rok desc, dat.Miesiąc desc, dat.Dzień desc";
                    break;

                case 2:
                    sql = "select Numer, Produkt, Sklep, Data, KtoKomu, Kwota " +
                          "from factZakup zak left join dimData dat on zak.Data = dat.Klucz " +
                          $"where Produkt like \'{product_filter}%\'" +
                          "order by dat.Rok desc, dat.Miesiąc desc, dat.Dzień desc";
                    break;

                case 3:
                    sql = "select Numer, Produkt, Sklep, Data, KtoKomu, Kwota " +
                          "from factZakup zak left join dimData dat on zak.Data = dat.Klucz " +
                          $"where Sklep like \'{shop_filter}%\' and Produkt like \'{product_filter}%\'" +
                          "order by dat.Rok desc, dat.Miesiąc desc, dat.Dzień desc";
                    break;
            }
            DB_handling.open_connection();
            SqlDataAdapter zakupy = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            zakupy.Fill(dt);
            this.dataGridView1.DataSource = dt;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            this.refresh_summary();
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            this.refresh_summary();
        }
    }
}
