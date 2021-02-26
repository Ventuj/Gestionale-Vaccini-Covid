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
    public partial class ViewUser : Form {
        database db = new database();
        string nome, cognome, idp, indirizzo, luogodinascita, cellulare, telefono, email, codicefiscale, idst, idop = "";
        string nomem, cognomem, idpe = "";

        public ViewUser(string id) {
            InitializeComponent();
            idp = id;
            caricaDati();
            textBox3.Enabled = false;
            // header tabelle
            datiPazienti.Columns.Add("1", "ID"); datiPazienti.Columns.Add("1", "Data"); datiPazienti.Columns.Add("1", "Dose"); datiPazienti.Columns.Add("1", "Tipo"); datiPazienti.Columns.Add("1", "Malattia");
            dataGridView2.Columns.Add("1", "Indirizzo"); dataGridView2.Columns.Add("1", "Email"); dataGridView2.Columns.Add("1", "Quantià"); dataGridView2.Columns.Add("1", "ids");
            dataGridView6.Columns.Add("1", "Data"); dataGridView6.Columns.Add("1", "Ora"); dataGridView6.Columns.Add("1", "id");
            dataGridView4.Columns.Add("1", "Data"); dataGridView4.Columns.Add("1", "Lotto"); dataGridView4.Columns.Add("1", "Data Di Produzione"); dataGridView4.Columns.Add("1", "Dose"); dataGridView4.Columns.Add("1", "idv");
            dataGridView5.Columns.Add("1", "Data"); dataGridView5.Columns.Add("1", "Operatore"); dataGridView5.Columns.Add("1", "Struttura"); dataGridView5.Columns.Add("1", "id");
            // stilistica
            label1.Text = cognome + " " + nome;
            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = groupBox4.ForeColor = groupBox5.ForeColor = groupBox6.ForeColor = groupBox7.ForeColor = groupBox8.ForeColor = groupBox9.ForeColor = Color.White;
            this.datiPazienti.DefaultCellStyle.ForeColor = this.dataGridView6.DefaultCellStyle.ForeColor = this.dataGridView1.DefaultCellStyle.ForeColor = this.dataGridView2.DefaultCellStyle.ForeColor = this.dataGridView3.DefaultCellStyle.ForeColor = this.dataGridView4.DefaultCellStyle.ForeColor = this.dataGridView5.DefaultCellStyle.ForeColor = Color.Black;
            // caricamenti
            stampaPren();
            fillComboVaccini();
            stampaTabellaVaccini();
            stampaListaMedici();
            checkMedico();
            stampaStrutture();
            fillCombo();
            stampaCovid();
            stampaAccertamenti();
        }

        private void fillCombo() {
            for (int i = 0; i <= 23; i++)
            {
                comboBox2.Items.Add(Convert.ToString(i));
            }
        }


        private void button1_Click(object sender, EventArgs e) {
            if (txtNome.Text != "" && txtCognome.Text != "" && textCF.Text != "" && txtLuogoDN.Text != "" && txtIndirizzo.Text != "" && txtTel.Text != "" && txtCel.Text != "" && txtEmail.Text != "")
            {
                db.esegui(string.Format(@"UPDATE pazienti SET nome = '{0}', cognome = '{1}', codiceFiscale = '{2}',luogodinascita = '{3}',indirizzo = '{4}',telefono = '{5}', cellulare = '{6}', email = '{7}' WHERE idp = '{8}'", txtNome.Text, txtCognome.Text, textCF.Text, txtLuogoDN.Text, txtIndirizzo.Text, txtTel.Text, txtCel.Text, txtEmail.Text, idp));
            }
            else
            {
                MessageBox.Show("Non sono stati compilati tutti i campi", "informazioni");
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
            string comandosql = "SELECT vacciniPazienti.idrVP as ID, vacciniPazienti.data, vacciniPazienti.dose, vaccini.tipo, vaccini.malattia FROM vaccini,vacciniPazienti WHERE vacciniPazienti.idp = '" + idp + "' AND vacciniPazienti.idv = vaccini.idv";
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

        // selettori struttura e operatore 
        // selettori condivi sia per l'accertamento sia per il vaccino

        // inserimento vaccino covid
        private void button3_Click(object sender, EventArgs e) {
            if (idop != "" && idst != "")
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    if (db.rowCount(string.Format("SELECT COUNT(*) FROM vaccinoCovid WHERE idp = '{0}' AND data = '{1}'", idp, db.converter(dateTimePicker1.Text))) == 0)
                    {
                        int count = Convert.ToInt32(db.getData(string.Format("SELECT quantita FROM scorte WHERE ids = '{0}'", idst)));
                        if (count > 0)
                        {
                            db.esegui(string.Format("INSERT INTO vaccinoCovid(idvc, idp, idop, ids, data, dataproduzione, lotto, dose) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}','{7}')", db.UUID(19, 9, 15), idp, idop, idst, db.converter(dateTimePicker1.Text), db.converter(dateTimePicker4.Text), textBox2.Text, textBox1.Text));

                            db.esegui(string.Format("UPDATE scorte SET quantita = {0} WHERE ids = '{1}'", count - 1, idst));
                            stampaCovid();
                            dataGridView2.Enabled = true;
                            button9.Visible = false;
                            stampaOperatori("");
                            stampaStrutture();
                        }
                        else
                        {
                            MessageBox.Show("Questa struttura è sprovvista di vaccini", "informazioni");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Non è possibile inserire un altro vaccino in questa data", "informazioni");
                    }
                }
                else
                {
                    MessageBox.Show("Non sono stati compilati tutti i campi", "informazioni");
                }
            }
            else
            {
                MessageBox.Show("Per poter inserire la vaccinazione è necessario selezionare struttura e operatore", "informazioni");
            }
        }

        // tabella vaccinazioni covid 
        private void stampaCovid() {
            dataGridView4.Rows.Clear();
            string comandosql = "SELECT idvc,data,lotto,dataproduzione,dose FROM vaccinoCovid WHERE idp = '"+ idp +"'";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView4.Rows.Add(db.inverter(dr["data"].ToString()), dr["lotto"].ToString(), db.inverter(dr["dataproduzione"].ToString()), dr["dose"].ToString(), dr["idvc"].ToString());
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }

        // Tabella covid 
        private void dataGridView4_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView4[4, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi eliminare questa riga", "informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("DELETE FROM vaccinoCovid WHERE idvc = '{0}'", id));
                        stampaCovid();
                    }
                }
            }
        }

        // tabella accertamenti
        private void stampaAccertamenti() {
            dataGridView5.Rows.Clear();
            string comandosql = "SELECT data,idop,ids,idac FROM Accertamento WHERE idp = '" + idp + "'";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView5.Rows.Add(db.inverter(dr["data"].ToString()), dr["idop"].ToString(), dr["ids"].ToString(), dr["idac"].ToString());
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }

        private void button7_Click(object sender, EventArgs e) {
            addAcc();
        }

        private void dataGridView5_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Right)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView5[3, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Sei sicuro di voler eliminare questa riga?", "eliminazione", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("DELETE FROM Accertamento WHERE idac = '{0}'", id));
                        db.esegui(string.Format("DELETE FROM effettiCollaterali WHERE idac = '{0}'", id));
                        stampaAccertamenti();
                    }
                }
            }
            else
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView5[3, e.RowIndex].Value.ToString();
                    string note = Convert.ToString(db.getData(string.Format("SELECT descrizione FROM effettiCollaterali WHERE idac = '{0}'", id)));
                    MessageBox.Show("Effetti collaterali: " + note, "Note accertamento");
                }
                
            }
        }

        private void checkEFF_CheckedChanged(object sender, EventArgs e) {
            if (checkEFF.Checked)
            {
                textBox3.Enabled = true;
            }
            else
            {
                textBox3.Enabled = false;
            }
        }

        private void addAcc() {
            if (idop != "" && idst != "")
            {
                if (checkEFF.Checked)
                {
                    if (textBox3.Text != "")
                    {
                        string id = db.UUID(20,8, 16);
                        db.esegui(string.Format("INSERT INTO Accertamento(idac, idop, idp, ids, data) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", id, idop, idp, idst, db.converter(dateTimePicker2.Text)));
                        db.esegui(string.Format("INSERT INTO effettiCollaterali(idec, idac, descrizione) VALUES('{0}', '{1}',  '{2}')", db.UUID(20, 7, 17), id, textBox3.Text));
                        stampaAccertamenti();
                    }
                    else
                    {
                        MessageBox.Show("Il campo testo non può essere vuoto", "informazioni");
                    }
                }
                else
                {
                    string id = db.UUID(20, 8, 16);
                    db.esegui(string.Format("INSERT INTO Accertamento(idac, idop, idp, ids, data) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", id, idop, idp, idst, db.converter(dateTimePicker2.Text)));
                    stampaAccertamenti();
                }
            }
            else
            {
                MessageBox.Show("Per poter inserire la vaccinazione è necessario selezionare struttura e operatore", "informazioni");
            }
        }

        // tabella strutture

        private void stampaStrutture() {
            dataGridView2.Rows.Clear();
            string comandosql = "SELECT indirizzo,email,ids FROM strutture";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        string id = dr["ids"].ToString();
                        int qt = Convert.ToInt32(db.getData(string.Format("SELECT quantita FROM scorte WHERE ids = '{0}'", id)));
                        if (qt > 0)
                        {
                            dataGridView2.Rows.Add(dr["indirizzo"].ToString(), dr["email"].ToString(), qt, id);
                        }
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView2[3, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi selezionare questa struttura?", "informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        idst = id;
                        stampaOperatori(id);
                        dataGridView2.Enabled = false;
                        button9.Visible = true;
                    }
                }
            }
        }

        // deselecta struttura
        private void button9_Click(object sender, EventArgs e) {
            dataGridView2.Enabled = true;
            button9.Visible = false;
            stampaOperatori("");
        }

        // tabella operatori
        // dipende da strutture 
        // non caricare prima di aver caricato la struttura
        private void stampaOperatori(string ids) {
            string comandosql = "SELECT personale.cognome,personale.nome,operatori.idop FROM operatori,personale WHERE operatori.idop IN (SELECT idop FROM turni WHERE ids = '" + ids + "') AND operatori.idpe = personale.idpe";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(comando);
                    DataSet ds = new DataSet("tabelle");
                    da.Fill(ds, "tabella");
                    dataGridView3.DataSource = ds.Tables["tabella"];
                    dataGridView3.Refresh();
                }
                connessione.Close();
            }
        }
        // click tabella operatori
        private void dataGridView3_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView3[2, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi selezionare questo operatore?", "informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        idop = id;
                        label23.Text = this.dataGridView3[0, e.RowIndex].Value.ToString() + " " + this.dataGridView3[1, e.RowIndex].Value.ToString();
                    }
                }
            }
        }

        // aggiunta prenotazioni
        private void button8_Click(object sender, EventArgs e) {
            addPren();
        }
        private void addPren() {
            string data = db.converter(dateTimePicker3.Text);
            string ora = comboBox2.Text;
            if (ora != "")
            {
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM prenotazioniCovid WHERE data = '{0}' AND idp = '{1}'", data, idp)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO prenotazioniCovid(idpr, idp, data, ora) VALUES('{0}', '{1}', '{2}', '{3}')", db.UUID(15, 5, 10), idp, data, ora));
                    stampaPren();
                }
                else
                {
                    MessageBox.Show("Prenotazione già esistente", "informazioni");
                }
            }
            else{
                MessageBox.Show("Non è stato selezionato l'orario", "informazioni");
            }
        }
        // stampa prenotazioni
        private void stampaPren() {
            dataGridView6.Rows.Clear();
            string comandosql = string.Format("SELECT data,ora,idpr FROM prenotazioniCovid WHERE idp = '{0}'", idp);
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView6.Rows.Add(db.inverter(dr["data"].ToString()), dr["ora"].ToString(), dr["idpr"].ToString());
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }
        private void dataGridView6_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    string id = this.dataGridView6[2, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi eliminare questa prenotazione?", "informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("DELETE FROM prenotazioniCovid WHERE idpr = '{0}'", id));
                        stampaPren();
                    }
                }
            }
        }
    }
}
