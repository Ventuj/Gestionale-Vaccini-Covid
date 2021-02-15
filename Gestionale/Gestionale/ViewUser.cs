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
    public partial class ViewUser : Form
    {
        database db = new database();
        string nome, cognome, idp, indirizzo, luogodinascita, cellulare, telefono, email, codicefiscale = "";
        public string[] valori = new string[36];

        public ViewUser(string id) {
            InitializeComponent();
            idp = id;
            caricaDati();
            fillComboVaccini();
            stampaTabellaVaccini();
            carica();
            label1.Text = cognome + " " + nome;
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

        private void datiPazienti_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            /*
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Sei sicuro di voler eliminare questa riga?", "eliminazione", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    db.esegui(string.Format("DELETE FROM vacciniPazienti WHERE idrVP = '{0}'", id));
                    stampaTabellaVaccini();
                }
            }
             */
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
                DialogResult dialogResult = MessageBox.Show("Sei sicuro di voler eliminare questa riga?", "eliminazione", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    db.esegui(string.Format("DELETE FROM vacciniPazienti WHERE idrVP = '{0}'", id));
                    stampaTabellaVaccini();
                }
            }
        }

        private void caricaDati() {
            nome = Convert.ToString(db.getData(string.Format("SELECT nome FROM pazienti WHERE idp = '{0}'", idp)));
            cognome = Convert.ToString(db.getData(string.Format("SELECT cognome FROM pazienti WHERE idp = '{0}'", idp)));
            indirizzo = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM pazienti WHERE idp = '{0}'", idp)));
            luogodinascita = Convert.ToString(db.getData(string.Format("SELECT luogodinascita FROM pazienti WHERE idp = '{0}'", idp)));
            cellulare = Convert.ToString(db.getData(string.Format("SELECT cellulare FROM pazienti WHERE idp = '{0}'", idp)));
            telefono = Convert.ToString(db.getData(string.Format("SELECT telefono FROM pazienti WHERE idp = '{0}'", idp)));
            email = Convert.ToString(db.getData(string.Format("SELECT email FROM pazienti WHERE idp = '{0}'", idp)));
            codicefiscale = Convert.ToString(db.getData(string.Format("SELECT codiceFiscale FROM pazienti WHERE idp = '{0}'", idp)));
        }

        private void fillComboVaccini() {
            string cmd = "SELECT * FROM vaccini";
            comboBox1.DisplayMember = "Text";
            comboBox1.ValueMember = "Value";

            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(cmd, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        comboBox1.Items.Add(new ComboboxItem(dr["tipo"].ToString(), dr["idv"].ToString()));
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            if (comboBox1.Text != "" && txtLotto.Text != "" && txtDose.Text != "")
            {
                ComboboxItem c = (ComboboxItem)comboBox1.SelectedItem;
                label7.Text = c.Text.ToString();
                string idv = c.Value.ToString();
                db.esegui(string.Format("INSERT INTO vacciniPazienti(idv, idp, data, datascadenza, lotto, dataproduzione, dose, idrVP) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' , '{7}')", idv, idp, db.converter(dataSomm.Text), db.converter(dataScad.Text), txtLotto.Text, db.converter(dataProd.Text), txtDose.Text, UUID()));
                stampaTabellaVaccini();
            }
        }

        private void stampaTabellaVaccini() {
            string comandosql = "SELECT vacciniPazienti.idrVP as ID, vacciniPazienti.data, vacciniPazienti.dose, vaccini.tipo, vaccini.malattia FROM vaccini,vacciniPazienti WHERE vacciniPazienti.idp = '" + idp +"' AND vacciniPazienti.idv = vaccini.idv";
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

        private void label7_Click(object sender, EventArgs e) {}
        private void ViewUser_Load(object sender, EventArgs e) {}
        private void label12_Click(object sender, EventArgs e) { }
        private void groupBox2_Enter(object sender, EventArgs e) {}
    }
}
