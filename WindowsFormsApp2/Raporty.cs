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
        public Raporty()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql_beg = "select ROK, MIESIĄC, sum(KWOTA) as RACHUNEK, sum([DO ODDANIA]) as ZWROT " +
                             "from factZakup zak left join dimData dat on zak.DATA = dat.KLUCZ " +
                             "group by ROK, MIESIĄC order by ROK desc, MIESIĄC desc";

            DB_handling.open_connection();
            SqlDataAdapter da = DB_handling.select_query(sql_beg);
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
    }
}
