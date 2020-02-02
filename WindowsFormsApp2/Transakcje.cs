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
            this.refresh_years();
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
        void refresh_years()
        {
            string sql = "select Rok from dimData dat " +
                "left join factZakup zak on zak.Data = dat.Klucz " +
                "group by Rok having count(Numer) > 0 " +
                "order by Rok desc";

            DB_handling.open_connection();
            SqlDataAdapter years = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            years.Fill(dt);

            this.comboBox3.DataSource = dt;
            this.comboBox3.DisplayMember = "Rok";
            this.comboBox3.SelectedIndex = -1;
        }
        void refresh_summary()
        {
            string shop_filter = this.comboBox1.Text;
            string sql_shop = $"Sklep like \'{shop_filter}%\' ";
            bool is_shop_filt = (shop_filter == "") ? false : true;

            string product_filter = this.comboBox2.Text;
            string sql_prod = $"Produkt like \'{product_filter}%\' ";
            bool is_prod_filt = (product_filter == "") ? false : true;

            string year_filter = this.comboBox3.Text;
            string sql_year = $"Rok like \'{year_filter}%\' ";
            bool is_year_filt = (year_filter == "") ? false : true;

            bool[] filter_flags = {is_shop_filt, is_prod_filt, is_year_filt };
            Dictionary<int, string> filtering_insertions = new Dictionary<int, string>();
            filtering_insertions.Add(0, sql_shop);
            filtering_insertions.Add(1, sql_prod);
            filtering_insertions.Add(2, sql_year);

            string sql = "";
            string sql_where = "where ";
            string sql_and = "and ";
            string sql_beg = "select Numer, Produkt, Sklep, Data, KtoKomu, Kwota " +
                          "from factZakup zak left join dimData dat on zak.Data = dat.Klucz "; 
            string sql_end = "order by dat.Rok desc, dat.Miesiąc desc, dat.Dzień desc";

            if (!filter_flags.Contains(true))
                sql = sql_beg + sql_end;
            else
            {
                sql = sql_beg + sql_where;
                bool first_added = false;
                for (int i = 0; i < filter_flags.Length; i++)
                {

                    if (filter_flags[i] == true)
                    {
                        if (first_added == true)
                            sql += sql_and;
                        sql += filtering_insertions[i];
                        first_added = true;
                    }
                }
                sql += sql_end;
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

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            this.refresh_summary();
        }
    }
}
