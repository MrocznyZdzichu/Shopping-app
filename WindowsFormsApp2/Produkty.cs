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
    public partial class Produkty : Form
    {
        void refresh_table()
        {
            string sql = "select * from dimProdukt";
            if (textBox4.Text != "" && comboBox1.Text == "")
                sql += $" where Nazwa like \'{textBox4.Text}%\'";

            if (textBox4.Text != "" && comboBox1.Text != "")
                sql += $" where Nazwa like \'{textBox4.Text}%\' and Kategoria like \'{comboBox1.Text}\' ";

            if (textBox4.Text == "" && comboBox1.Text != "")
                sql += $" where Kategoria like \'{comboBox1.Text}\'";

            DB_handling.open_connection();
            SqlDataAdapter prod_list = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            prod_list.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void refresh_categories()
        {
            string sql_categories = "select Kategoria from dimProdukt group by Kategoria";
            DB_handling.open_connection();
            SqlDataAdapter categories = DB_handling.select_query(sql_categories);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            categories.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Kategoria";
            comboBox1.SelectedIndex = -1;
        }
        public Produkty()
        {
            InitializeComponent();
            this.refresh_table();
            this.refresh_categories();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            this.refresh_table();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.refresh_table();
        }
    }
}
