﻿using System;
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
        string cell_prev_text;

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

            string prev = comboBox1.Text;
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "Kategoria";
            if (prev == "")
                comboBox1.SelectedIndex = -1;
            else
                comboBox1.Text = prev;
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

            string prev = comboBox2.Text;
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "Podkategoria";
            if (prev == "")
                comboBox2.SelectedIndex = -1;
            else
                comboBox2.Text = prev;
            
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

            string sql_update;
            if (col != 0)
                sql_update = $"update dimProdukt set {updated_field[col]} = \'{new_val}\' where Nazwa = \'{Nazwa_PK}\'";
            else
                sql_update = $"update dimProdukt set {updated_field[col]} = \'{new_val}\' where Nazwa = \'{cell_prev_text}\'";

            DB_handling.open_connection();
            DB_handling.update(sql_update);
            DB_handling.close_connection();

            this.refresh_table();
            this.refresh_categories();
            this.refresh_subcats();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string new_name, new_category, new_subcat;
            if (textBox1.Text != ""){
                if (textBox2.Text != ""){
                    if (textBox3.Text != ""){
                        new_name = textBox1.Text;
                        new_category = textBox2.Text;
                        new_subcat = textBox3.Text;

                        string sql_insert = $"insert into dimProdukt values " +
                            $"(\'{new_name}\', \'{new_category}\', \'{new_subcat}\')";

                        DB_handling.open_connection();
                        DB_handling.insert(sql_insert);
                        DB_handling.close_connection();

                        this.refresh_table();
                        this.refresh_categories();
                        this.refresh_subcats();
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string product_delete = dataGridView1.CurrentCell.Value.ToString();
            int row = dataGridView1.CurrentCell.RowIndex;
            string delete_PK = dataGridView1[0, row].Value.ToString();

            string sql = $"delete from dimProdukt where Nazwa = \'{delete_PK}\'";

            DB_handling.open_connection();
            DB_handling.delete(sql);
            DB_handling.close_connection();

            this.refresh_table();
            this.refresh_categories();
            this.refresh_subcats();
        }
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.cell_prev_text = this.dataGridView1.CurrentCell.Value.ToString();
        }

        private void Produkty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Q)
            {
                DB_handling.close_connection();
                MessageBox.Show("Zamknięto połączenie do DB");
            }
        }
    }
}
