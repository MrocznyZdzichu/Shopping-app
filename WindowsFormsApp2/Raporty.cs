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
            bool is_month_filter = year_filter != "";
            string month_sql = $"[Nazwa miesiąca] like \'{month_filter}%\' ";
            filter_config.Add(1, month_sql);

            bool[] filters_list = { is_year_filter, is_month_filter};
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
            this.textBox1.Text = sql;
            DB_handling.open_connection();
            SqlDataAdapter da = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            da.Fill(dt);
            this.dataGridView1.DataSource = dt;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string sql_beg = "select DATA, SKLEP, RACHUNEK from ( " +
                                "select DATA, SKLEP, sum(KWOTA) as RACHUNEK " +
                                "from factZakup group by DATA, SKLEP) t1 " +
                             "left join dimData dat on t1.DATA = dat.KLUCZ " +
                             "order by ROK desc, MIESIĄC desc, DZIEŃ desc";

            DB_handling.open_connection();
            SqlDataAdapter da = DB_handling.select_query(sql_beg);
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
                this.refresh_months();
            }
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (report_mode == 1)
            {
                this.refresh_report1();
            }
        }
    }
}
