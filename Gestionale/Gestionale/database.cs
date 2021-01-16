using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestionale
{
    class database
    {
        public string stringaConnessione = @"Data Source=database.db";
        public void CreateTable() {

        }

        private void esegui(string comandosql) {
            using (SQLiteConnection connessione = new SQLiteConnection(stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    comando.ExecuteNonQuery();
                }
                connessione.Close();
            }
        }
    }
}
