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
            if (textBox4.Text != "" && comboBox1.Text == "" && comboBox2.Text == "")
                sql += $" where Nazwa like \'{textBox4.Text}%\'";

            if (textBox4.Text != "" && comboBox1.Text == "" && comboBox2.Text != "")
                sql += $" where Nazwa like \'{textBox4.Text}%\' " +
                    $"and Podkategoria like \'{comboBox2.Text}\'";

            if (textBox4.Text != "" && comboBox1.Text != "" && comboBox2.Text == "")
                sql += $" where Nazwa like \'{textBox4.Text}%\' " +
                    $"and Kategoria like \'{comboBox1.Text}\' ";

            if (textBox4.Text != "" && comboBox1.Text != "" && comboBox2.Text != "")
                sql += $" where Nazwa like \'{textBox4.Text}%\' " +
                    $"and Kategoria like \'{comboBox1.Text}\' " +
                    $"and Podkategoria like \'{comboBox2.Text}\'";

            if (textBox4.Text == "" && comboBox1.Text != "" && comboBox2.Text == "")
                sql += $" where Kategoria like \'{comboBox1.Text}\'";

            if (textBox4.Text == "" && comboBox1.Text != "" && comboBox2.Text != "")
                sql += $" where Kategoria like \'{comboBox1.Text}\' " +
                    $"and Podkategoria like \'{comboBox2.Text}\'";

            if (textBox4.Text == "" && comboBox1.Text == "" && comboBox2.Text != "")
                sql += $" where PodKategoria like \'{comboBox2.Text}\' ";

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

        void refresh_subcats()
        {
            string sql_where = "";
            if (comboBox1.Text != "")
                sql_where = $"where Kategoria like \'{comboBox1.Text}\' ";
            string sql_subcats = $"select Podkategoria from dimProdukt {sql_where}group by Podkategoria";
            DB_handling.open_connection();
            SqlDataAdapter subcats = DB_handling.select_query(sql_subcats);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            subcats.Fill(dt);

            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Podkategoria";
            comboBox2.SelectedIndex = -1;
        }
        public Produkty()
        {
            InitializeComponent();
            this.refresh_table();
            this.refresh_categories();
            this.refresh_subcats();
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
            this.refresh_subcats();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.refresh_table();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            this.refresh_table();
            this.refresh_subcats();
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            this.refresh_table();
        }

        private void comboBox1_TextUpdate(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int row = this.dataGridView1.CurrentCell.RowIndex;
            int col = this.dataGridView1.CurrentCell.ColumnIndex;
            string new_val = this.dataGridView1.CurrentCell.Value.ToString();

            string Nazwa_PK     = this.dataGridView1[0, row].Value.ToString();
            string Kategoria    = this.dataGridView1[1, row].Value.ToString();
            string Podkategoria = this.dataGridView1[2, row].Value.ToString();

            Dictionary<int, string> updated_field = new Dictionary<int, string>();
            updated_field.Add(0, "Nazwa");
            updated_field.Add(1, "Kategoria");
            updated_field.Add(2, "Podkategoria");

            string sql_update = $"update dimProdukt set {updated_field[col]} = \'{new_val}\' where Nazwa = \'{Nazwa_PK}\'";
            DB_handling.open_connection();
            DB_handling.update(sql_update);
            DB_handling.close_connection();

            this.refresh_table();
            this.refresh_categories();
            this.refresh_subcats();
        }
    }
}
