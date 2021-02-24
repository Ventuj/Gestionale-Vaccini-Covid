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
    public partial class viewStudioMedico : Form {
        string idst, telefono, email, indirizzo = "";
        database db = new database();

        public viewStudioMedico(string id) {
            InitializeComponent();
            idst = id;
            caricaDati();
            caricaListaMediciCompleta();
            caricaListaMediciStudio();
            fillCombo();
            stampaOrari();
            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = Color.White;
            this.datiPazienti.DefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
        }

        private void caricaDati() {
            indirizzo = txtIndirizzo.Text = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM studiomedico WHERE idst = '{0}'", idst)));
            telefono = txtTelefono.Text = Convert.ToString(db.getData(string.Format("SELECT telefono FROM studiomedico WHERE idst = '{0}'", idst)));
            email = txtEmail.Text = Convert.ToString(db.getData(string.Format("SELECT email FROM studiomedico WHERE idst = '{0}'", idst)));
        }

        private void button1_Click(object sender, EventArgs e) {

        }

        private void caricaListaMediciCompleta() {
            string comandosql = "SELECT cognome, nome, idpe FROM personale WHERE idpe NOT IN (SELECT idpe FROM studioPersonale) AND idpe NOT IN (SELECT idpe FROM operatori)";
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

        private void button2_Click(object sender, EventArgs e) {
            MessageBox.Show("Doppio click destro per spostare una persona tra una lista e l'altra \nDoppio click destro per vedere le informazioni della persona", "Informazioni", MessageBoxButtons.OK);
        }

        private void button3_Click(object sender, EventArgs e) {
            MessageBox.Show("Doppio click destro per spostare una persona tra una lista e l'altra \nDoppio click destro per vedere le informazioni della persona", "Informazioni", MessageBoxButtons.OK);
        }

        private void caricaListaMediciStudio() {
            string comandosql = "SELECT personale.cognome, personale.nome, personale.idpe FROM personale,studioPersonale WHERE studioPersonale.idst = '"+ idst +"' AND studioPersonale.idpe = personale.idpe";
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


        // Lista medici completa
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                string id = this.dataGridView1[2, e.RowIndex].Value.ToString();
                if (e.Button == MouseButtons.Left)
                {
                    DialogResult dialogResult = MessageBox.Show("Vuoi aggiungere questa persona allo studio medico?", "Informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("INSERT INTO studioPersonale(idst, idpe) VALUES('{0}', '{1}')", idst, id));
                        caricaListaMediciCompleta();
                        caricaListaMediciStudio();
                    }
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Vuoi vedere i dati di questa persona?", "Informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ViewMedico view = new ViewMedico(id);
                        this.Hide();
                        view.ShowDialog();
                        caricaListaMediciCompleta();
                        caricaListaMediciStudio();
                        this.Show();
                    }
                }
            }
        }

        // Lista Personale nello studio
        private void datiPazienti_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e) {
            string id = this.datiPazienti[2, e.RowIndex].Value.ToString();
            if (e.Button == MouseButtons.Left)
            {
                DialogResult dialogResult = MessageBox.Show("Vuoi rimuovere questo medico da questo studio?", "Informazioni", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    db.esegui(string.Format("DELETE FROM studioPersonale WHERE idst = '{0}' AND idpe = '{1}'", idst, id));
                    caricaListaMediciCompleta();
                    caricaListaMediciStudio();
                }
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Vuoi vedere i dati di questa persona?", "Informazioni", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ViewMedico view = new ViewMedico(id);
                    this.Hide();
                    view.ShowDialog();
                    caricaListaMediciCompleta();
                    caricaListaMediciStudio();
                    this.Show();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) {
            addOrario();
        }

        private void fillCombo() {
            for (int i = 0; i <= 23; i++)
            {
                comboBox4.Items.Add(Convert.ToString(i));
                comboBox5.Items.Add(Convert.ToString(i));
            }
        }

        // orari
        private void dataGridView2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
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

        private void stampaOrari() {
            string comandosql = @"SELECT ido, orario, giorno FROM orari WHERE id = '" + idst + "'";
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

        private void addOrario() {
            string orario = comboBox5.Text + " - " + comboBox4.Text;
            string giorno = comboBox6.Text;
            if (Convert.ToInt32(comboBox5.Text) < Convert.ToInt32(comboBox4.Text))
            {
                if (comboBox4.Text != "" && comboBox5.Text != "" && comboBox6.Text != "")
                {
                    // orario già inserito
                    if (db.rowCount(string.Format("SELECT COUNT(*) FROM orari WHERE id = '{0}' AND orario = '{1}' AND giorno = '{2}'", idst, orario, giorno)) == 0)
                    {
                        db.esegui(string.Format("INSERT INTO orari(ido, id, orario, giorno) VALUES('{0}', '{1}', '{2}', '{3}')", db.UUID(11, 2, 7), idst, orario, giorno));
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
    }
}
