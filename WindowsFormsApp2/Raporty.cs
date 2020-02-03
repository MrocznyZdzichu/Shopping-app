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
    public partial class Raporty : Form
    {
        int report_mode;
        public Raporty()
        {
            InitializeComponent();
            report_mode = 0;
            this.label1.Text = "";
            this.comboBox1.Visible = false;
            this.label2.Text = "";
            this.comboBox2.Visible = false;
            this.label3.Text = "";
            this.comboBox3.Visible = false;
            this.label4.Text = "";
            this.comboBox4.Visible = false;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            report_mode = 1;

            this.comboBox1.Visible = true;
            this.label1.Text = "Rok";

            this.comboBox2.Visible = true;
            this.label2.Text = "Miesiąc";

            this.refresh_years();
            this.refresh_months();
            this.refresh_report1();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            report_mode = 2;

            this.label1.Text = "Rok";
            this.label2.Text = "Miesiąc";
            this.label3.Text = "Dzień";
            this.label4.Text = "Sklep";

            this.comboBox1.Visible = true;
            this.comboBox2.Visible = true;
            this.comboBox3.Visible = true;
            this.comboBox4.Visible = true;

            refresh_report2();
            refresh_years();
            refresh_months();
            refresh_days();
            refresh_shops();
        }

        private void refresh_years()
        {
            SqlDataAdapter years = DB_handling.get_years();
            DataTable dt0 = new DataTable();
            years.Fill(dt0);

            this.comboBox1.DataSource = dt0;
            this.comboBox1.DisplayMember = "Rok";
            this.comboBox1.SelectedIndex = -1;
        }
        private void refresh_months()
        {
            string year = this.comboBox1.Text;
            SqlDataAdapter months = DB_handling.get_months(year);
            DataTable dt1 = new DataTable();
            months.Fill(dt1);

            this.comboBox2.DataSource = dt1;
            this.comboBox2.DisplayMember = "Nazwa miesiąca";
            this.comboBox2.SelectedIndex = -1;
        }
        private void refresh_days()
        {
            string year = this.comboBox1.Text;
            string month = this.comboBox2.Text;

            SqlDataAdapter days = DB_handling.get_days(year, month);
            DataTable dt = new DataTable()
;
            days.Fill(dt);

            this.comboBox3.DataSource = dt;
            this.comboBox3.DisplayMember = "Dzień";
            this.comboBox3.SelectedIndex = -1;
        }
        private void refresh_shops()
        {
            string sql = "select Nazwa from dimSklep group by Nazwa";

            DB_handling.open_connection();
            SqlDataAdapter shops = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            shops.Fill(dt);

            this.comboBox4.DataSource = dt;
            this.comboBox4.DisplayMember = "Nazwa";
            this.comboBox4.SelectedIndex = -1;
        }
        private void refresh_report1()
        {
            string sql_beg = "select ROK, MIESIĄC, sum(KWOTA) as RACHUNEK, sum([DO ODDANIA]) as ZWROT " +
                             "from factZakup zak left join dimData dat on zak.DATA = dat.KLUCZ ";
            string sql_and = "and ";

            string sql_where = "where ";
            string sql_end = "group by ROK, MIESIĄC order by ROK desc, MIESIĄC desc";
            Dictionary<int, string> filter_config = new Dictionary<int, string>();
            

            string year_filter = this.comboBox1.Text;
            bool is_year_filter = year_filter != "";
            string year_sql = $"ROK like {year_filter} ";
            filter_config.Add(0, year_sql);

            string month_filter = this.comboBox2.Text;
            bool is_month_filter = month_filter != "";
            string month_sql = $"[Nazwa miesiąca] like \'{month_filter}\' ";
            filter_config.Add(1, month_sql);

            bool[] filters_list = {is_year_filter, is_month_filter};
            string sql = sql_beg;
            bool first_filter_added = false;

            for (int i = 0; i < filters_list.Length; i++)
            {
                if (filters_list[i] == true)
                {
                    sql = (first_filter_added == false) ? sql + sql_where : sql + sql_and;
                    sql += filter_config[i];
                    first_filter_added = true;
                }
            }
            sql += sql_end;
          
            DB_handling.open_connection();
            SqlDataAdapter da = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            da.Fill(dt);
            this.dataGridView1.DataSource = dt;
        }
        private void refresh_report2()
        {
            string sql_beg = "select DATA, SKLEP, RACHUNEK from ( " +
                    "select DATA, SKLEP, sum(KWOTA) as RACHUNEK " +
                    "from factZakup group by DATA, SKLEP) t1 " +
                 "left join dimData dat on t1.DATA = dat.KLUCZ ";
            string sql_where = "where ";
            string sql_and = "and ";
            string sql_end = "order by ROK desc, MIESIĄC desc, DZIEŃ desc";
            Dictionary<int, string> filter_config = new Dictionary<int, string>();

            string year_filter = this.comboBox1.Text;
            bool is_year_filter = year_filter != "";
            string year_sql = $"ROK like {year_filter} ";
            filter_config.Add(0, year_sql);

            string month_filter = this.comboBox2.Text;
            bool is_month_filter = month_filter != "";
            string month_sql = $"[Nazwa miesiąca] like \'{month_filter}%\' ";
            filter_config.Add(1, month_sql);

            string day_filter = this.comboBox3.Text;
            bool is_day_filter = day_filter != "";
            string day_sql = $"Dzień like {day_filter} ";
            filter_config.Add(2, day_sql);

            string shop_filter = this.comboBox4.Text;
            bool is_shop_filter = shop_filter != "";
            string shop_sql = $"SKLEP like \'{shop_filter}%\' ";
            filter_config.Add(3, shop_sql);

            bool[] filters_list = { is_year_filter, is_month_filter, is_day_filter, is_shop_filter };
            string sql = sql_beg;
            bool first_filter_added = false;

            for (int i = 0; i < filters_list.Length; i++)
            {
                if (filters_list[i] == true)
                {
                    sql = (first_filter_added == false) ? sql + sql_where : sql + sql_and;
                    sql += filter_config[i];
                    first_filter_added = true;
                }
            }
            sql += sql_end;

            DB_handling.open_connection();
            SqlDataAdapter da = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            da.Fill(dt);
            this.dataGridView1.DataSource = dt;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (report_mode == 1)
            {
                this.refresh_report1();
            }
            if (report_mode == 2)
            {
                this.refresh_report2();
                this.refresh_days();
            }
            this.refresh_months();
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (report_mode == 1)
            {
                this.refresh_report1();
            }
            if (report_mode == 2)
            {
                this.refresh_report2();
                this.refresh_days();
            }
        }
        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (report_mode == 2)
            {
                this.refresh_report2();
            }
        }
        private void comboBox4_TextChanged(object sender, EventArgs e)
        {
            if (report_mode == 2)
            {
                this.refresh_report2();
            }
        }
    }
}
