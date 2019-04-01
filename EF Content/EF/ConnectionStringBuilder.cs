using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.EntityClient;

namespace EF
{
    static public class ConnectionStringBuilder
    {
        static public string ConnectionString { get; set; }
        static public void CreateConnectionString(string username, string password)
        {
            var connString = new EntityConnectionStringBuilder();
            connString.ConnectionString = ConfigurationManager.ConnectionStrings["SerialsEntities"].ConnectionString;

            SqlConnectionStringBuilder providerString = new SqlConnectionStringBuilder();
            providerString.ConnectionString = connString.ProviderConnectionString;
            providerString.IntegratedSecurity = false;
            providerString.Password = password;
            providerString.UserID = username;
            providerString.ApplicationName = "EntityFramework";
            connString.ProviderConnectionString = providerString.ConnectionString;
            ConnectionString = connString.ConnectionString;
            //Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //configuration.ConnectionStrings.ConnectionStrings["SerialsEntities"].ConnectionString = connString.ConnectionString;
            //configuration.Save();
            //ConfigurationManager.RefreshSection("ConnectionStrings");
        }
    }
}
