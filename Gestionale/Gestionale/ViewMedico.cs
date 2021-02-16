using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            }
            else
            {
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

        }

        private void button2_Click(object sender, EventArgs e) {
            db.esegui(string.Format("INSERT INTO operatori(idop, idpe) VALUES('{0}', '{1}') ",db.UUID(15, 7,10), idpe));
            checkST();
            controlloOP();
        }
    }
}
