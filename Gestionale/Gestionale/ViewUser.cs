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
        string nomem, cognomem, idpe = "";

        public ViewUser(string id) {
            InitializeComponent();
            idp = id;
            caricaDati();
            fillComboVaccini();
            datiPazienti.Columns.Add("1", "ID");
            datiPazienti.Columns.Add("1", "Data");
            datiPazienti.Columns.Add("1", "Dose");
            datiPazienti.Columns.Add("1", "Tipo");
            datiPazienti.Columns.Add("1", "Malattia");
            stampaTabellaVaccini();
            stampaListaMedici();
            checkMedico();
            label1.Text = cognome + " " + nome;
            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = groupBox4.ForeColor = Color.White;
            this.datiPazienti.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e) {
            if (txtNome.Text != "" && txtCognome.Text != "" && textCF.Text != "" && txtLuogoDN.Text != "" && txtIndirizzo.Text != "" && txtTel.Text != "" && txtCel.Text != "" && txtEmail.Text != "")
            {
                db.esegui(string.Format(@"UPDATE pazienti SET nome = '{0}', cognome = '{1}', codiceFiscale = '{2}',luogodinascita = '{3}',indirizzo = '{4}',telefono = '{5}', cellulare = '{6}', email = '{7}' WHERE idp = '{8}'", txtNome.Text, txtCognome.Text, textCF.Text, txtLuogoDN.Text, txtIndirizzo.Text, txtTel.Text, txtCel.Text, txtEmail.Text, idp));
            }
        }

        private void datiPazienti_CellDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left) { 
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
        }

        private void caricaDati() {
            nome = txtNome.Text = Convert.ToString(db.getData(string.Format("SELECT nome FROM pazienti WHERE idp = '{0}'", idp)));
            cognome = txtCognome.Text = Convert.ToString(db.getData(string.Format("SELECT cognome FROM pazienti WHERE idp = '{0}'", idp)));
            indirizzo = txtIndirizzo.Text = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM pazienti WHERE idp = '{0}'", idp)));
            luogodinascita = txtLuogoDN.Text = Convert.ToString(db.getData(string.Format("SELECT luogodinascita FROM pazienti WHERE idp = '{0}'", idp)));
            cellulare = txtCel.Text = Convert.ToString(db.getData(string.Format("SELECT cellulare FROM pazienti WHERE idp = '{0}'", idp)));
            telefono = txtTel.Text = Convert.ToString(db.getData(string.Format("SELECT telefono FROM pazienti WHERE idp = '{0}'", idp)));
            email = txtEmail.Text = Convert.ToString(db.getData(string.Format("SELECT email FROM pazienti WHERE idp = '{0}'", idp)));
            codicefiscale = textCF.Text = Convert.ToString(db.getData(string.Format("SELECT codiceFiscale FROM pazienti WHERE idp = '{0}'", idp)));
            idpe = Convert.ToString(db.getData(string.Format("SELECT idpe FROM pazienti WHERE idp = '{0}'", idp)));
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
                string idv = c.Value.ToString();
                db.esegui(string.Format("INSERT INTO vacciniPazienti(idv, idp, data, datascadenza, lotto, dataproduzione, dose, idrVP) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}' , '{7}')", idv, idp, db.converter(dataSomm.Text), db.converter(dataScad.Text), txtLotto.Text, db.converter(dataProd.Text), txtDose.Text, db.UUID(15, 3, 6)));
                stampaTabellaVaccini();
            }
        }

        private void stampaTabellaVaccini() {
            datiPazienti.Rows.Clear();
            string comandosql = "SELECT vacciniPazienti.idrVP as ID, vacciniPazienti.data, vacciniPazienti.dose, vaccini.tipo, vaccini.malattia FROM vaccini,vacciniPazienti WHERE vacciniPazienti.idp = '" + idp +"' AND vacciniPazienti.idv = vaccini.idv";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        datiPazienti.Rows.Add(dr["ID"].ToString(), db.inverter(dr["data"].ToString()), dr["dose"].ToString(), dr["tipo"].ToString(), dr["malattia"].ToString());
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }

        private void stampaListaMedici() {
            string comandosql = "SELECT cognome, nome, idpe FROM personale WHERE idpe != '" + idpe + "' AND idpe IN (SELECT idpe FROM studioPersonale) AND tipo = 'Medico'";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(comando);
                    DataSet ds = new DataSet("tabelle");
                    da.Fill(ds, "tabella");
                    dataGridView1.DataSource = ds.Tables["tabella"];
                    dataGridView1.Refresh();
                }
                connessione.Close();
            }
        }

        private void checkMedico() {
            if (db.rowCount(string.Format("SELECT COUNT(*) FROM pazienti WHERE idp = '{0}' AND idpe != ''", idp)) > 0)
            {
                button5.Visible = button4.Visible = button6.Visible = true;
                cognomem = Convert.ToString(db.getData(string.Format("SELECT cognome FROM personale WHERE idpe = '{0}'", idpe)));
                nomem = Convert.ToString(db.getData(string.Format("SELECT nome FROM personale WHERE idpe = '{0}'", idpe)));
                label7.Text = "Medico: " + cognomem + " " + nomem;
                dataGridView1.Enabled = false;
            }
            else
            {
                label7.Text = "Medico: ";
                button5.Visible = button4.Visible = button6.Visible = false;
                dataGridView1.Enabled = true;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView1[2, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi assegnare questo medico a questo paziente?", "informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("UPDATE pazienti SET idpe = '{0}' WHERE idp = '{1}'", id, idp));
                        idpe = Convert.ToString(db.getData(string.Format("SELECT idpe FROM pazienti WHERE idp = '{0}'", idp)));
                        checkMedico();
                        stampaListaMedici();
                    }
                }
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            if (dataGridView1.Enabled)
            {
                dataGridView1.Enabled = false;
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Vuoi cambiare medico o semplicemente toglierlo?", "informazioni", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    db.esegui(string.Format("UPDATE pazienti SET idpe = '' WHERE idp = '{0}'", idp));
                    idpe = Convert.ToString(db.getData(string.Format("SELECT idpe FROM pazienti WHERE idp = '{0}'", idp)));
                    checkMedico();
                    stampaListaMedici();
                }
                else
                {
                    dataGridView1.Enabled = true;
                }

            }
        }

        private void button4_Click(object sender, EventArgs e) {
            string idst = Convert.ToString(db.getData(string.Format("SELECT idst FROM studioPersonale WHERE idpe = '{0}'", idpe)));
            viewStudioMedico v = new viewStudioMedico(idst);
            this.Hide();
            v.ShowDialog();
            this.Show();
        }

        private void button6_Click(object sender, EventArgs e) {
            ViewMedico v = new ViewMedico(idpe);
            this.Hide();
            v.ShowDialog();
            this.Show();
        }

        // gestione operatori per effettuare il vaccino covid
        // update in base alla struttura selezionata 
        // string struttura -> string operatore (dipendenza)
    }
}
