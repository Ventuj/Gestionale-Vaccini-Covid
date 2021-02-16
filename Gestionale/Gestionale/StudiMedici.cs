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
    public partial class StudiMedici : Form
    {
        database db = new database();
        public StudiMedici() {
            InitializeComponent();
            stampaLista();
        }

        private void StudiMedici_Load(object sender, EventArgs e) {

        }

        private void button1_Click(object sender, EventArgs e) {
            if (txtEmail.Text != "" && txtIndirizzo.Text != "" && txtTelefono.Text != "")
            {
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM studiomedico WHERE email = '{0}' AND indirizzo = '{1}' AND telefono = '{2}'", txtEmail.Text, txtIndirizzo.Text, txtTelefono.Text)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO studiomedico(idst, telefono, email, indirizzo) VALUES('{0}', '{1}', '{2}', '{3}')", db.UUID(18, 5, 16), txtTelefono.Text, txtEmail.Text, txtIndirizzo.Text));
                    stampaLista();
                }
                else
                {
                    MessageBox.Show("Studio medico già presente");
                }
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");

            }
        }

        private void stampaLista() {
            string comandosql = "SELECT * FROM studiomedico";
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
