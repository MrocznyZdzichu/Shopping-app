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
        void refresh()
        {
            string sql = "select * from dimProdukt";
            if (textBox4.Text != "")
                sql += $" where Nazwa like \'{textBox4.Text}%\'";
            
            DB_handling.open_connection();
            SqlDataAdapter prod_list = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            prod_list.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        public Produkty()
        {
            InitializeComponent();
            this.refresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            this.refresh();
        }
    }
}
