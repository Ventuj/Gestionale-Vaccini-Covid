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
    public partial class VacciniN : Form
    {
        database db = new database();

        public string[] valori = new string[36];
        public VacciniN() {
            InitializeComponent();
            carica();
            stampaLista();
        }

        private void stampaLista() {
            string comandosql = "SELECT idv,tipo,malattia,casaFarmaceutica AS CasaF FROM vaccini";
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

        public void carica() {
            for (int i = 0; i < 36; i++)
            {
                if (i < 10)
                {
                    valori[i] = Convert.ToString(i);
                }
                else
                {
                    valori[i] = Convert.ToString((char)('a' + i - 10));
                }
            }
        }

        public string UUID() {
            string cu = "";
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                if (cu.Length == 3 || cu.Length == 12)
                {
                    cu += "-";
                }
                else
                {
                    cu += valori[rnd.Next(0, 36)];
                }
            }
            return cu.ToUpper();
        }

        private void button1_Click(object sender, EventArgs e) {
            if (txtTipo.Text != "" && txtCasa.Text != "" && txtMalattia.Text != "")
            {
                if(db.rowCount(string.Format("SELECT COUNT(*) FROM vaccini WHERE tipo = '{0}' AND malattia = '{1}' AND casaFarmaceutica = '{2}'", txtTipo.Text, txtMalattia.Text, txtCasa.Text)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO vaccini(idv, tipo, malattia, casaFarmaceutica) VALUES('{0}', '{1}', '{2}', '{3}')", UUID(), txtTipo.Text, txtMalattia.Text, txtCasa.Text));
                    stampaLista();
                    txtTipo.Text = txtMalattia.Text = txtCasa.Text = "";
                }
                else
                {
                    MessageBox.Show("Questo vaccino esiste già");
                }
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");
            }
        }
    }
}
