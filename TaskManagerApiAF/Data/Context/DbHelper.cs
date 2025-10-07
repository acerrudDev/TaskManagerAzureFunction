using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApiAF.Data.Context
{
    public class DbHelper
    {
        private readonly IConfiguration _config;

        public DbHelper(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection GetConnection()
        {
            var connString = _config["ConnectionStrings__DefaultConnection"];
            return new SqlConnection(connString);
        }
    }
}
