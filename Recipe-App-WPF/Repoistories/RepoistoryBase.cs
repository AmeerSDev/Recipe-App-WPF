using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_App_WPF.Repoistories
{
    public abstract class RepoistoryBase
    {
        private readonly string _connectionString;
        public RepoistoryBase()
        {
            _connectionString = "Server=(local); Database=MVVMLogicDB; Intergrated Security=true";
        }
        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
