using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Gotcha1.Models
{
    public class DbConnect
    {
        string connectionString = "";
        public static SqlConnection Conn;
        private IConfiguration connect;
        public DbConnect(IConfiguration r)
        {
            connect = r;
            connectionString = connect["ConnectionStrings:DefaultConnection"];
            Conn = new SqlConnection(connectionString);
        }
    }
}
