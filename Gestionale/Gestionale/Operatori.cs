using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gestionale
{
    public partial class Operatori : Form
    {
        database db = new database();
        public Operatori() {
            InitializeComponent();
            stampaLista();
            this.datiPazienti.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void stampaLista() {
            string comandosql = @"
                                SELECT 
                                    personale.idpe,
                                    operatori.idop,
                                    personale.cognome,
                                    personale.nome,
                                    personale.tipo
                                FROM personale, operatori WHERE personale.idpe = operatori.idpe";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(comando);
                    DataSet ds = new DataSet("tabelle");
                    da.Fill(ds, "tabella");
                    datiPazienti.DataSource = ds.Tables["tabella"];
                    datiPazienti.Refresh();
                }
                connessione.Close();
            }
        }
    }
}
