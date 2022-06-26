using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace Debi
{
    public class Config
    {
        public MySqlConnection db_connect()
        {
            string str = "datasource=localhost; username=root; password=; database=debi_db";
            MySqlConnection sqlConnection = new MySqlConnection(str);

            try
            {
                sqlConnection.Open();
                return sqlConnection;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}