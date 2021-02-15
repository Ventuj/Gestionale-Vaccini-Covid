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
        string nome, cognome, idpe, indirizzo, luogodinascita, cellulare, email, codicefiscale = "";

        public ViewMedico(string id) {
            InitializeComponent();
            idpe = id;
            caricaDati();
        }

        private void caricaDati() {
            nome = Convert.ToString(db.getData(string.Format("SELECT nome FROM personale WHERE idpe = '{0}'", idpe)));
            cognome = Convert.ToString(db.getData(string.Format("SELECT cognome FROM personale WHERE idpe = '{0}'", idpe)));
            indirizzo = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM personale WHERE idpe = '{0}'", idpe)));
            luogodinascita = Convert.ToString(db.getData(string.Format("SELECT luogodinascita FROM personale WHERE idpe = '{0}'", idpe)));
            cellulare = Convert.ToString(db.getData(string.Format("SELECT cellulare FROM personale WHERE idpe = '{0}'", idpe)));
            email = Convert.ToString(db.getData(string.Format("SELECT email FROM personale WHERE idpe = '{0}'", idpe)));
            codicefiscale = Convert.ToString(db.getData(string.Format("SELECT codiceFiscale FROM personale WHERE idpe = '{0}'", idpe)));
        }
        private void button1_Click(object sender, EventArgs e) {

        }

        private void ViewMedico_Load(object sender, EventArgs e) {

        }
    }
}
