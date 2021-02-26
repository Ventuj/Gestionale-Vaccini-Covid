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
    public partial class viewVaccino : Form
    {
        database db = new database();
        string idv, tipo, casafarmaceutica, malattia = "";

        public viewVaccino(string id) {
            InitializeComponent();
            idv = id;
            caricaDati();
            groupBox1.ForeColor = Color.White;
        }

        private void caricaDati() {
            tipo = txtTipo.Text = Convert.ToString(db.getData(string.Format("SELECT tipo FROM vaccini WHERE idv = '{0}'", idv)));
            malattia = txtMalattia.Text = Convert.ToString(db.getData(string.Format("SELECT malattia FROM vaccini WHERE idv = '{0}'", idv)));
            casafarmaceutica = txtCasa.Text = Convert.ToString(db.getData(string.Format("SELECT casaFarmaceutica FROM vaccini WHERE idv = '{0}'", idv)));
        }
        private void button1_Click(object sender, EventArgs e) {
            if (txtCasa.Text != "" && txtMalattia.Text != "" && txtTipo.Text != "")
            {
                db.esegui(string.Format("UPDATE vaccini SET tipo = '{0}', malattia = '{1}', casaFarmaceutica = '{2}' WHERE idv = '{3}'", txtTipo.Text, txtMalattia.Text, txtCasa.Text,idv));
            }
            else
            {
                MessageBox.Show("Non sono stati compilati tutti i campi", "informazioni");
            }
        }
    }
}
