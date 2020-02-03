using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Zakupy());
        }
    }

   public static class DB_handling
    {
        static string db_connection_string = 
            "Server= DESKTOP-HU1RMS9; Database= zakupy; Integrated Security=True;";

        static SqlConnection db_con = new SqlConnection(db_connection_string);

        static public void open_connection()
        {
            db_con.Open();
        }
        static public void close_connection()
        {
            db_con.Close();
        }

        static public SqlDataAdapter select_query(string sql)
        {
            SqlDataAdapter results = new SqlDataAdapter(sql, db_con);
            return results;
        }

        static public SqlDataAdapter get_years()
        {
            SqlDataAdapter years;

            string sql = "select ROK " +
                         "from factZakup zak left join dimData dat on zak.DATA = dat.KLUCZ " +
                         "group by ROK";

            open_connection();
            years = select_query(sql);
            close_connection();

            return years;
        }
        static public SqlDataAdapter get_months(string year)
        {
            SqlDataAdapter months;

            string sql_beg = $"select [Nazwa miesiąca] " +
                         $"from factZakup zak left join dimData dat " +
                         $"on zak.DATA = dat.KLUCZ ";
            string sql_where = (year == "") ? "" : $"where ROK = {year} ";
            string sql_end = "group by [Nazwa miesiąca], Miesiąc " +
                             "order by Miesiąc";
            string sql = sql_beg + sql_where + sql_end;

            open_connection();
            months = select_query(sql);
            close_connection();

            return months;
        }
        static public SqlDataAdapter get_days(string year, string month)
        { 
            string sql = "";
            if (year != "" && month != "")
            {
                sql = "select Dzień from dimData " +
                      $"where Rok = {year} and [Nazwa miesiąca] = \'{month}\' " +
                      "group by Dzień order by Dzień";
            }
            else
            {
                sql = "select Dzień from dimData " +
                      "group by Dzień order by Dzień";
            }

            open_connection();
            SqlDataAdapter days = select_query(sql);
            close_connection();
            return days;
        }

        static public void insert(string sql)
        {
            SqlCommand command = new SqlCommand(sql, db_con);
            SqlDataAdapter da = new SqlDataAdapter();

            da.InsertCommand = new SqlCommand(sql, db_con);
            da.InsertCommand.ExecuteNonQuery();

            command.Dispose();
        }
        static public void delete(string sql)
        {
            SqlCommand command = new SqlCommand(sql, db_con);
            SqlDataAdapter da = new SqlDataAdapter();

            da.DeleteCommand = new SqlCommand(sql, db_con);
            da.DeleteCommand.ExecuteNonQuery();

            command.Dispose();
        }
        static public void update(string sql)
        {
            SqlCommand command = new SqlCommand(sql, db_con);
            SqlDataAdapter da = new SqlDataAdapter();

            da.UpdateCommand = new SqlCommand(sql, db_con);
            da.UpdateCommand.ExecuteNonQuery();

            command.Dispose();
        }
    }
}
