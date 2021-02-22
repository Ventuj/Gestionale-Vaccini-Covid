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
        public void addTurno() {

        }

        // orari 

        // view operatori legati alla struttura
        public void operatoriStruttura() {

        }

        // view operatori disponibili per aggiunta turno

        public void tableOperatori() {

        }
    }
}
