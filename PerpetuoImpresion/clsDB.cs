using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerpetuoImpresion
{
    public class clsDB
    {
        //parametros de conexion a MyQSL
        private static string server = "localhost";
        private static string user = "root";
        private static string password = "";
        private static string database = "bullshop";

        public static DataTable GetData(string qry)
        {
            try
            {
                string strConn = $"server= {server};uid={user};database={database};pwd={password}";
                MySqlConnection MyConn = new MySqlConnection(strConn);
                MySqlCommand MyCommand = new MySqlCommand(qry, MyConn);
                MySqlDataAdapter MyAdapter = new MySqlDataAdapter();
                MyAdapter.SelectCommand = MyCommand;
                DataTable Info = new DataTable();
                MyAdapter.Fill(Info);
                return Info;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
