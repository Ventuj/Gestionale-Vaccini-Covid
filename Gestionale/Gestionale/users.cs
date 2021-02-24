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
    public partial class users : Form
    {
        database db = new database();

        public users() {
            InitializeComponent();
            stampaLista();
            groupBox1.ForeColor = Color.White;
            this.datiPazienti.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e) {
            if(txtNome.Text != "" && txtCognome.Text != "" && textCF.Text != "" && txtDataDiNascita.Text != "" && txtLuogoDN.Text != "" && txtIndirizzo.Text != "" && txtTel.Text != "" && txtCel.Text != "" && txtEmail.Text != "")
            {
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM pazienti WHERE codiceFiscale = '{0}'", textCF.Text)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO pazienti(idp, nome, cognome, codiceFiscale, datadinascita, luogodinascita, indirizzo, telefono, cellulare, email) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", db.UUID(15, 3, 12), txtNome.Text, txtCognome.Text, textCF.Text, db.converter(txtDataDiNascita.Text), txtLuogoDN.Text, txtIndirizzo.Text, txtTel.Text, txtCel.Text, txtEmail.Text));
                    stampaLista();
                    txtNome.Text = txtCognome.Text = textCF.Text = txtDataDiNascita.Text = txtLuogoDN.Text = txtIndirizzo.Text = txtTel.Text = txtCel.Text = txtEmail.Text = "";
                }
                else
                {
                    MessageBox.Show("Paziente già inserito");
                }
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");
            }
        }

        private void stampaLista() {
            string comandosql = "SELECT idp,cognome,nome,codiceFiscale FROM pazienti";
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

        private void datiPazienti_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
                if (e.Button == MouseButtons.Right)
                {
                    DialogResult dialogResult = MessageBox.Show("Sei sicuro di voler eliminare questa riga?", "eliminazione", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format(@"
                                                    DELETE FROM pazienti WHERE idp = '{0}'; 
                                                    DELETE FROM vacciniPazienti WHERE idp = '{0}';
                                                    DELETE FROM vacciniCovid WHERE idp = '{0}';
                                                    DELETE FROM prenotazioniCovid WHERE idp = '{0}';
                                                    DELETE FROM effettiCollaterali WHERE idac = (SELECT idac FROM accertamenti WHERE idp = '{0}');
                                                    DELETE FROM Accertamento WHERE idp = '{0}';
                                                    ", id));
                        stampaLista();
                    }
                }
                else
                {
                    ViewUser view = new ViewUser(id);
                    this.Hide();
                    view.ShowDialog();
                    stampaLista();
                    this.Show();
                }
            }
        }
    }
}
