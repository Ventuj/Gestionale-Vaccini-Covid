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
    public partial class ViewMedico : Form
    {
        database db = new database();
        string nome, cognome, idpe, idop, idst, indirizzo, luogodinascita, cellulare, email, codicefiscale = "";

        public ViewMedico(string id) {
            InitializeComponent();
            idpe = id;
            caricaDati();
            label1.Text = cognome + " " + nome;
            checkST();
            controlloOP();
            stampaPazienti();
            stampaOreLav();
            groupBox1.ForeColor = groupBox2.ForeColor = groupBox3.ForeColor = groupBox4.ForeColor = groupBox5.ForeColor = groupBox6.ForeColor = Color.White;
            this.datiPazienti.DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns.Add("1", "Data"); dataGridView1.Columns.Add("1", "Ora"); dataGridView1.Columns.Add("1", "ido"); dataGridView1.Columns.Add("1", "idst");

        }

        private void button3_Click(object sender, EventArgs e) {
            viewStudioMedico v = new viewStudioMedico(idst);
            this.Hide();
            v.ShowDialog();
            checkStudio();
            this.Show();
        }

        private void caricaDati() {
            nome = txtNome.Text = Convert.ToString(db.getData(string.Format("SELECT nome FROM personale WHERE idpe = '{0}'", idpe)));
            cognome = txtCognome.Text = Convert.ToString(db.getData(string.Format("SELECT cognome FROM personale WHERE idpe = '{0}'", idpe)));
            indirizzo = txtIndirizzo.Text = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM personale WHERE idpe = '{0}'", idpe)));
            luogodinascita = txtLuogoDN.Text = Convert.ToString(db.getData(string.Format("SELECT luogodinascita FROM personale WHERE idpe = '{0}'", idpe)));
            cellulare = txtCel.Text = Convert.ToString(db.getData(string.Format("SELECT cellulare FROM personale WHERE idpe = '{0}'", idpe)));
            email = txtEmail.Text = Convert.ToString(db.getData(string.Format("SELECT email FROM personale WHERE idpe = '{0}'", idpe)));
            codicefiscale = textCF.Text = Convert.ToString(db.getData(string.Format("SELECT codiceFiscale FROM personale WHERE idpe = '{0}'", idpe)));
        }
        private void button1_Click(object sender, EventArgs e) {
            if (txtNome.Text != "" && txtCognome.Text != "" && textCF.Text != "" && txtLuogoDN.Text != "" && txtIndirizzo.Text != "" && txtCel.Text != "" && txtEmail.Text != "")
            {
                db.esegui(string.Format("UPDATE personale SET nome = '{0}', cognome = '{1}', codiceFiscale = '{2}',luogodinascita = '{3}',indirizzo = '{4}', cellulare = '{5}', email = '{6}' WHERE idpe = '{7}'", txtNome.Text, txtCognome.Text, textCF.Text, txtLuogoDN.Text, txtIndirizzo.Text, txtCel.Text, txtEmail.Text, idpe));
            }
        }


        private bool checkOP() {
            if (db.rowCount(string.Format("SELECT COUNT(*) FROM operatori WHERE idpe = '{0}'", idpe)) > 0)
            {
                idop = Convert.ToString(db.getData(string.Format("SELECT idop FROM operatori WHERE idpe = '{0}'", idpe)));
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkStudio() {
            if (db.rowCount(string.Format("SELECT COUNT(*) FROM studioPersonale WHERE idpe = '{0}'", idpe)) > 0)
            {
                idst = Convert.ToString(db.getData(string.Format("SELECT idst FROM studioPersonale WHERE idpe = '{0}'", idpe)));
                return true;
            }
            else
            {
                return false;
            }
        }

        private void controlloOP() {
            if (checkOP())
            {
                button2.Visible = false;
                button4.Visible = true;
                if (getIDST() != "")
                {
                    groupBox5.Enabled = true;
                    groupBox6.Enabled = true;
                }
            }
            else
            {
                groupBox5.Enabled = false;
                groupBox6.Enabled = false;
                if (checkStudio())
                {
                    button2.Visible = button4.Visible = false;
                }
                else
                {
                    button4.Visible = false;
                }
            }
        }

        private void checkST() {
            if (checkStudio())
            {
                label4.Text = "Questa persona è associata ad uno studio medico";
                button3.Visible = true;
            }
            else
            {
                if (checkOP())
                {
                    label4.Text = "Questa persona è un operatore Covid";
                    button3.Visible = false;
                }
                else
                {
                    label4.Text = "Studio medico non trovato";
                    button3.Visible = false;
                }
            }
        }

        // Visualizza operatori Covid
        private void button4_Click(object sender, EventArgs e) {
            Operatori o = new Operatori();
            this.Hide();
            o.ShowDialog();
            checkST();
            controlloOP();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e) {
            db.esegui(string.Format("INSERT INTO operatori(idop, idpe) VALUES('{0}', '{1}') ",db.UUID(15, 7,10), idpe));
            checkST();
            controlloOP();
        }

        private void stampaPazienti() {
            string comandosql = "SELECT idp, cognome, nome FROM pazienti WHERE idpe = '"+ idpe + "'";
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
            string id = this.datiPazienti[0, e.RowIndex].Value.ToString();
            if (e.Button == MouseButtons.Left)
            {
                ViewUser view = new ViewUser(id);
                this.Hide();
                view.ShowDialog();
                stampaPazienti();
                this.Show();
            }
        }

        private string getIDST() {
            return  Convert.ToString(db.getData(string.Format("SELECT ids FROM turni WHERE idop = (SELECT idop FROM operatori WHERE idpe = '{0}') LIMIT 1", idpe)));
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e) {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void addOra() {
            if (textBox1.Text != "" && Convert.ToInt32(textBox1.Text) <= 12)
            {
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM orelavorate WHERE idop = '{0}' AND data = '{1}'", idop, db.converter(dateTimePicker1.Text))) == 0)
                {
                    db.esegui(string.Format("INSERT INTO orelavorate(ido, idop, ids, data, ora) VALUES('{0}', '{1}', '{2}', '{3}', '{4}')", db.UUID(19, 6, 16), idop, getIDST(), db.converter(dateTimePicker1.Text), textBox1.Text));
                    stampaOreLav();
                }
                else
                {
                    MessageBox.Show("Sono già state inserite delle ore per questa data", "informazioni");
                }
            }
            else
            {
                MessageBox.Show("Informazioni mancanti o errate", "informazioni");
            }
        }

        private void button5_Click(object sender, EventArgs e) {
            addOra();
        }
        
        private void stampaOreLav() {
            dataGridView1.Rows.Clear();
            string comandosql = "SELECT * FROM orelavorate WHERE idop = '"+ idop +"'";
            using (SQLiteConnection connessione = new SQLiteConnection(db.stringaConnessione))
            {
                connessione.Open();
                using (SQLiteCommand comando = new SQLiteCommand(comandosql, connessione))
                {
                    SQLiteDataReader dr = comando.ExecuteReader();
                    while (dr.Read())
                    {
                        dataGridView1.Rows.Add(db.inverter(dr["data"].ToString()), dr["ora"].ToString(), dr["ido"].ToString(), dr["ids"].ToString());
                    }
                    dr.Close();
                }
                connessione.Close();
            }
        }
        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e) {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                
                if (e.Button == MouseButtons.Right)
                {
                    string id = this.dataGridView1[2, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi rimuovere questa riga?", "Informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        db.esegui(string.Format("DELETE FROM orelavorate WHERE ido = '{0}'", id));
                        stampaOreLav();
                    }
                }
                else
                {
                    string id = this.dataGridView1[3, e.RowIndex].Value.ToString();
                    DialogResult dialogResult = MessageBox.Show("Vuoi vedere i dati di questa struttura?", "Informazioni", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        ViewStruttura view = new ViewStruttura(id);
                        this.Hide();
                        view.ShowDialog();
                        stampaOreLav();
                        this.Show();
                    }
                }
            }
        }
    }
}
