using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gemeenschap
{
    public class TuinDBManager
    {
        
            private static ConnectionStringSettings conTuinSetting = ConfigurationManager.ConnectionStrings["Tuin"];
            private static DbProviderFactory factory = DbProviderFactories.GetFactory(conTuinSetting.ProviderName);

            public DbConnection GetConnection()
            {
                var conTuin = factory.CreateConnection();
                conTuin.ConnectionString = conTuinSetting.ConnectionString;
                return conTuin;
            }
        
    }
}
