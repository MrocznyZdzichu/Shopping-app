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
    public partial class Custom_SQL : Form
    {
        public Custom_SQL()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = this.textBox1.Text;

            if (sql.Substring(0, sql.IndexOf(' ')) != "select")
            {
                MessageBox.Show("Podany SQL to nie SELECT");
                return;
            }

            DB_handling.open_connection();
            SqlDataAdapter results = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            results.Fill(dt);

            this.dataGridView1.DataSource = dt;
            return;
        }

        private void Custom_SQL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                DB_handling.close_connection();
                MessageBox.Show("Zamknięto połączenie do DB");
            }
        }
    }
}
