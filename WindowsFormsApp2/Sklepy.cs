using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Sklepy : Form
    {
        void refresh()
        {
            string filter = textBox2.Text;
            string sql = (filter == "")
                ? "select * from dimSklep"
                : $"select * from dimSklep where Nazwa like \'{filter}%\'";

            DB_handling.open_connection();
            SqlDataAdapter sklepy_list = DB_handling.select_query(sql);
            DB_handling.close_connection();

            DataTable dt = new DataTable();
            sklepy_list.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        public Sklepy()
        {
            InitializeComponent();
            this.refresh();
        }

        private void Sklepy_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            this.refresh();
        }
    }
}
