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
    public partial class Medici : Form
    {
        database db = new database();

        public Medici() {
            InitializeComponent();
            stampaLista();
        }

        private void Medici_Load(object sender, EventArgs e) {

        }


        private void stampaLista() {
            string comandosql = "SELECT idpe,cognome,nome,codiceFiscale,tipo FROM personale";
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

        // 0 Medico 
        // 1 Infermiere

        private void button1_Click_1(object sender, EventArgs e) {
            if (txtNome.Text != "" && comboBox1.Text != "" && txtCognome.Text != "" && textCF.Text != "" && txtDataDiNascita.Text != "" && txtLuogoDN.Text != "" && txtIndirizzo.Text != "" && txtCel.Text != "" && txtEmail.Text != "")
            {
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM personale WHERE codiceFiscale = '{0}'", textCF.Text)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO personale(idpe, tipo, nome, cognome, codiceFiscale, datadinascita, luogodinascita, indirizzo, cellulare, email) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", db.UUID(16, 4, 11), comboBox1.Text, txtNome.Text, txtCognome.Text, textCF.Text, db.converter(txtDataDiNascita.Text), txtLuogoDN.Text, txtIndirizzo.Text, txtCel.Text, txtEmail.Text));
                    stampaLista();
                    txtNome.Text = txtCognome.Text = textCF.Text = txtDataDiNascita.Text = txtLuogoDN.Text = txtIndirizzo.Text = txtCel.Text = txtEmail.Text = "";
                }
                else
                {
                    MessageBox.Show("Medico già inserito già inserito");
                }
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }

        private void datiPazienti_CellClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
                ViewMedico view = new ViewMedico(id);
                this.Hide();
                view.ShowDialog();
                stampaLista();
                this.Show();
            }
        }
    }
}
