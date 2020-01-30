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
    }
}
