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
    public partial class Strutture : Form
    {
        database db = new database();
        public string[] valori = new string[36];
        public Strutture() {
            InitializeComponent();
            carica();
            stampaLista();
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
        private void stampaLista() {
            string comandosql = "SELECT ids,massimali,indirizzo FROM strutture";
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

        private void txtLuogoDN_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            if (txtEmail.Text != "" && txtIndirizzo.Text != "" && txtMassimali.Text != "") {
                db.esegui(string.Format("INSERT INTO strutture(ids, indirizzo, massimali, email) VALUES('{0}', '{1}', {2}, '{3}')", UUID(), txtIndirizzo.Text, Convert.ToInt32(txtMassimali.Text), txtEmail.Text));
                stampaLista();
            }
        }
    }
}
