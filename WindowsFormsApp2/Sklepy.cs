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

        private void button1_Click(object sender, EventArgs e)
        {
            string shop_new = textBox1.Text;
            if (shop_new != "")
            {
                string sql = $"insert into dimSklep values (\'{shop_new}\')";
                DB_handling.open_connection();
                DB_handling.insert(sql);
                DB_handling.close_connection();

                this.refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string shop_delete = dataGridView1.CurrentCell.Value.ToString();
            string sql = $"delete from dimSklep where Nazwa = \'{shop_delete}\'";

            DB_handling.open_connection();
            DB_handling.delete(sql);
            DB_handling.close_connection();

            this.refresh();
        }
    }
}
