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

        public Strutture() {
            InitializeComponent();
            stampaLista();
            groupBox1.ForeColor = Color.White;
            this.datiPazienti.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void stampaLista() {
            string comandosql = "SELECT * FROM strutture";
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
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM strutture WHERE indirizzo = '{0}' AND email = '{1}'", txtIndirizzo.Text, txtEmail.Text)) == 0)
                {
                    string id = db.UUID(18, 3, 5);
                    db.esegui(string.Format("INSERT INTO strutture(ids, indirizzo, massimali, email) VALUES('{0}', '{1}', {2}, '{3}')", id, txtIndirizzo.Text, Convert.ToInt32(txtMassimali.Text), txtEmail.Text));
                    db.esegui(string.Format("INSERT INTO scorte(ids, quantita) VALUES('{0}', 0)",id));
                    stampaLista();
                    txtEmail.Text = txtIndirizzo.Text = txtMassimali.Text = "";
                }
                else
                {
                    MessageBox.Show("Struttura già inserita");
                }
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");
            }
        }

        private void datiPazienti_CellMouseDoubleClick_1(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
                if (e.Button == MouseButtons.Right)
                {
                    DialogResult dialogResult = MessageBox.Show("Sei sicuro di voler eliminare questa riga?", "eliminazione", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format(@"
                                                    DELETE FROM strutture WHERE ids = '{0}'; 
                                                    DELETE FROM turni WHERE ids = '{0}';
                                                    DELETE FROM spedizioni WHERE ids = '{0}';
                                                    DELETE FROM scorte WHERE ids = '{0}';
                                                    DELETE FROM orari WHERE id = '{0}';", id));
                        stampaLista();
                    }
                }
                else
                {
                    ViewStruttura view = new ViewStruttura(id);
                    this.Hide();
                    view.ShowDialog();
                    stampaLista();
                    this.Show();
                }
            }
        }
    }
}
