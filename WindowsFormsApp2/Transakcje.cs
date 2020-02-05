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
            this.refresh_months();
            this.refresh_days();
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
            SqlDataAdapter years = DB_handling.get_years();
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
            bool is_shop_filt = !(shop_filter == "");

            string product_filter = this.comboBox2.Text;
            string sql_prod = $"Produkt like \'{product_filter}%\' ";
            bool is_prod_filt = !(product_filter == "");

            string year_filter = this.comboBox3.Text;
            string sql_year = $"Rok like \'{year_filter}%\' ";
            bool is_year_filt = !(year_filter == "");

            string month_filter = this.comboBox4.Text;
            string sql_month = $"[Nazwa miesiąca] like \'{month_filter}%\' ";
            bool is_month_filt = !(month_filter == "");

            string day_filter = this.comboBox5.Text;
            string sql_days = $"Dzień like \'{day_filter}%\' ";
            bool is_day_filt = !(day_filter == "");

            bool[] filter_flags = {is_shop_filt, is_prod_filt, is_year_filt, is_month_filt, is_day_filt};
            Dictionary<int, string> filtering_insertions = new Dictionary<int, string>();
            filtering_insertions.Add(0, sql_shop);
            filtering_insertions.Add(1, sql_prod);
            filtering_insertions.Add(2, sql_year);
            filtering_insertions.Add(3, sql_month);
            filtering_insertions.Add(4, sql_days);

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
        void refresh_months()
        {
            string year = comboBox3.Text;
            SqlDataAdapter months = DB_handling.get_months(year);

            DataTable dt = new DataTable();
            months.Fill(dt);

            this.comboBox4.DataSource = dt;
            this.comboBox4.DisplayMember = "Nazwa miesiąca";
            this.comboBox4.SelectedIndex = -1;
        }
        void refresh_days()
        {
            string year = comboBox3.Text;
            string month = comboBox4.Text;

            SqlDataAdapter days = DB_handling.get_days(year, month);
            DataTable dt = new DataTable();
            days.Fill(dt);

            this.comboBox5.DataSource = dt;
            this.comboBox5.DisplayMember = "Dzień";
            this.comboBox5.SelectedIndex = -1;
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
            this.refresh_months();
            this.refresh_days();
        }
        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            this.refresh_summary();
            this.refresh_days();
        }
        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            this.refresh_summary();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql_insert = "insert into factZakup values ";
            int row_count = 0;
            for (int i = 0; i < this.dataGridView2.Rows.Count - 1; i++)
            {
                if (this.dataGridView2[0, i].Value.ToString() != "")
                    row_count++;
            }

            for (int i = 0; i < row_count; i++)
                sql_insert += this.read_row(i, row_count);

            DB_handling.open_connection();
            DB_handling.insert(sql_insert);
            DB_handling.close_connection();

            this.refresh_summary();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            int row = this.dataGridView1.CurrentCell.RowIndex;
            string deleted_PK = this.dataGridView1[0, row].Value.ToString();
            string sql_delete = $"delete from factZakup where Numer = {deleted_PK}";

            DB_handling.open_connection();
            DB_handling.delete(sql_delete);
            DB_handling.close_connection();

            this.refresh_summary();
        }

        private string read_row(int row, int row_count)
        {
            string insert_obs = $"(\'{this.dataGridView2[0, row].Value.ToString()}\', " +
                                $"\'{this.dataGridView2[1, row].Value.ToString()}\', " +
                                $"\'{this.dataGridView2[2, row].Value.ToString()}\', " +
                                $"\'{this.dataGridView2[3, row].Value.ToString()}\', " +
                                $"{this.dataGridView2[4, row].Value.ToString()})";

            insert_obs = (row == row_count - 1) ? insert_obs : insert_obs += ",";
            return insert_obs;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sklepy shops_scren = new Sklepy();
            shops_scren.Show();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            Produkty prod_screen = new Produkty();
            prod_screen.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string insertion = this.textBox1.Text;
            for (int i = 0; i < this.dataGridView2.SelectedCells.Count; i++)
            {
                this.dataGridView2.SelectedCells[i].Value = insertion;
            }
        }
    }
}
