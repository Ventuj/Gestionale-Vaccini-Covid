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
            groupBox1.ForeColor = groupBox2.ForeColor = Color.White;
            caricaDati();
            fillCombo();
            tableOperatori();
            stampaTurni();
        }

        private void fillCombo() {
            for (int i = 0; i <= 23; i++)
            {
                comboBox2.Items.Add(Convert.ToString(i));
                comboBox3.Items.Add(Convert.ToString(i));
            }
        }

        private void caricaDati() {
            indirizo = txtIndirizzo.Text = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM strutture WHERE ids = '{0}'", ids)));
            email = txtEmail.Text = Convert.ToString(db.getData(string.Format("SELECT email FROM strutture WHERE ids = '{0}'", ids)));
            massimali = Convert.ToInt32(db.getData(string.Format("SELECT massimali FROM strutture WHERE ids = '{0}'", ids)));
            txtMassimali.Text = Convert.ToString(massimali);
        }

        // Update
        private void button1_Click(object sender, EventArgs e) {
            if (txtEmail.Text != "" && txtIndirizzo.Text != "" && txtMassimali.Text != "")
            {
                db.esegui(string.Format("UPDATE strutture SET email = '{0}', massimali = {1}, email = '{2}' WHERE ids = '{3}'", txtEmail.Text, Convert.ToInt32(txtMassimali.Text), txtIndirizzo.Text, ids));
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
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM turni WHERE ids = '{0}' AND idop = '{1}' AND giorno = '{2}' AND (('{3}' <= start AND '{3}' >= start) OR ('{4}' <= end AND '{4}' >= start))", ids , idop, giorno, start, end)) == 0)
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

        // view operatori legati alla struttura
        public void operatoriStruttura() {

        }

        // lista turni
        public void stampaTurni() {
            string comandosql = @"
                                SELECT 
                                    operatori.idop,
                                    personale.cognome,
                                    personale.nome,
                                    personale.tipo,
                                    turni.giorno,
                                    turni.start AS inizio,
                                    turni.end AS fine
                                FROM personale, operatori, turni WHERE personale.idpe = operatori.idpe AND turni.idop = operatori.idop AND turni.ids = '"+ ids +"'";
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
