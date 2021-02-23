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
    public partial class ViewStruttura : Form
    {
        string ids, indirizo, email = "";
        int massimali = 0;
        database db = new database();

        public ViewStruttura(string id) {
            InitializeComponent();
            ids = id;
            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = groupBox4.ForeColor = groupBox5.ForeColor = groupBox6.ForeColor = groupBox7.ForeColor = Color.White;
            caricaDati();
            fillCombo();
            tableOperatori();
            stampaTurni();
            stampaOrari();
        }

        private void fillCombo() {
            for (int i = 0; i <= 23; i++)
            {
                comboBox2.Items.Add(Convert.ToString(i));
                comboBox3.Items.Add(Convert.ToString(i));
                comboBox4.Items.Add(Convert.ToString(i));
                comboBox5.Items.Add(Convert.ToString(i));
            }
        }

        private void caricaDati() {
            indirizo = txtIndirizzo.Text = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM strutture WHERE ids = '{0}'", ids)));
            email = txtEmail.Text = Convert.ToString(db.getData(string.Format("SELECT email FROM strutture WHERE ids = '{0}'", ids)));
            massimali = Convert.ToInt32(db.getData(string.Format("SELECT massimali FROM strutture WHERE ids = '{0}'", ids)));
            lbldispo.Text = Convert.ToString(db.getData(string.Format("SELECT quantita FROM SCORTE WHERE ids = '{0}'", ids)));
        }

        // Update
        private void button1_Click(object sender, EventArgs e) {
            if (txtEmail.Text != "" && txtIndirizzo.Text != "")
            {
                db.esegui(string.Format("UPDATE strutture SET email = '{0}', indirizzo = '{1}' WHERE ids = '{2}'", txtEmail.Text, txtIndirizzo.Text, ids));
                caricaDati();
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");
            }
        }

        // add turni
        public void addTurno(string idop) {
            string start = comboBox2.Text;
            string end = comboBox3.Text;
            string giorno = comboBox1.Text;
            if (start != "" && end != "" && giorno != "")
            {
                // Se esiste già questo turno
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM turni WHERE ids = '{0}' AND idop = '{1}' AND giorno = '{2}'", ids , idop, giorno)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO turni(idtu, idop, ids, giorno, start, end) VALUES('{0}','{1}','{2}','{3}','{4}','{5}')", db.UUID(12, 2, 7), idop, ids, giorno, start, end));
                    MessageBox.Show("Turno inserito correttamente");
                }
                else
                {
                    MessageBox.Show("Turno già essitente");
                }
            }
            else
            {
                MessageBox.Show("Non sono stati selezionati tutti i parametri ");
            }
        }

        public void addOrario() {
            string orario = comboBox5.Text + " - " + comboBox4.Text;
            string giorno = comboBox6.Text;
            if (Convert.ToInt32(comboBox5.Text) < Convert.ToInt32(comboBox4.Text))
            {
                if (comboBox4.Text != "" && comboBox5.Text != "" && comboBox6.Text != "")
                {
                    // orario già inserito
                    if (db.rowCount(string.Format("SELECT COUNT(*) FROM orari WHERE id = '{0}' AND orario = '{1}' AND giorno = '{2}'", ids, orario, giorno)) == 0)
                    {
                        db.esegui(string.Format("INSERT INTO orari(ido, id, orario, giorno) VALUES('{0}', '{1}', '{2}', '{3}')", db.UUID(11, 2, 7), ids, orario, giorno));
                        stampaOrari();
                    }
                    else
                    {
                        MessageBox.Show("Orario già inserito");
                    }
                }
                else
                {
                    MessageBox.Show("Non sono stati selezionati tutti i parametri ");
                }
            }
        }

        private void datiPazienti_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
                if (e.Button == MouseButtons.Left)
                {
                    addTurno(id);
                    stampaTurni();
                }
            }
        }

        // orari 
        public void stampaOrari() {
            string comandosql = @"SELECT ido, orario, giorno FROM orari WHERE id = '" + ids + "'";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(comando);
                    DataSet ds = new DataSet("tabelle");
                    da.Fill(ds, "tabella");
                    dataGridView2.DataSource = ds.Tables["tabella"];
                    dataGridView2.Refresh();
                }
                connessione.Close();
            }
        }

        // view operatori legati alla struttura
        public void operatoriStruttura() {

        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.dataGridView1[7, e.RowIndex].Value.ToString();
                if (e.Button == MouseButtons.Left)
                {
                    DialogResult dialogResult = MessageBox.Show("Vuoi eliminare questo turno?", "Informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("DELETE FROM turni WHERE idtu = '{0}'", id));
                        stampaTurni();
                        tableOperatori();
                    }
                }
            }
        }

        // tabella orario
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.dataGridView2[0, e.RowIndex].Value.ToString();
                if (e.Button == MouseButtons.Left)
                {
                    DialogResult dialogResult = MessageBox.Show("Vuoi eliminare questo orario?", "Informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("DELETE FROM orari WHERE ido = '{0}'", id));
                        stampaOrari();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            addOrario();
        }

        // lista turni
        public void stampaTurni() {
            string comandosql = @"
                                SELECT 
                                    personale.cognome,
                                    personale.nome,
                                    personale.tipo,
                                    turni.giorno,
                                    turni.start AS inizio,
                                    turni.end AS fine,
                                    operatori.idop,
                                    turni.idtu AS id
                                FROM personale, operatori, turni WHERE personale.idpe = operatori.idpe AND turni.idop = operatori.idop AND turni.ids = '" + ids +"'";
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

        // view operatori disponibili per aggiunta turno

        public void tableOperatori() {
            string comandosql = @"
                                SELECT 
                                    operatori.idop,
                                    personale.cognome,
                                    personale.nome,
                                    personale.tipo
                                FROM personale, operatori WHERE personale.idpe = operatori.idpe AND operatori.idop NOT IN (SELECT idop FROM turni WHERE ids != '" + ids +"')";
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
