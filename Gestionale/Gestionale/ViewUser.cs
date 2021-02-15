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
        public ViewUser(string id) {
            InitializeComponent();
            idp = id;
            caricaDati();
            label1.Text = cognome + " " + nome;
        }

        private void caricaDati() {
            nome = Convert.ToString(db.getData(string.Format("SELECT nome FROM pazienti WHERE idp = '{0}'", idp)));
            cognome = Convert.ToString(db.getData(string.Format("SELECT cognome FROM pazienti WHERE idp = '{0}'", idp)));
            indirizzo = Convert.ToString(db.getData(string.Format("SELECT indirizzo FROM pazienti WHERE idp = '{0}'", idp)));
            luogodinascita = Convert.ToString(db.getData(string.Format("SELECT luogodinascita FROM pazienti WHERE idp = '{0}'", idp)));
            cellulare = Convert.ToString(db.getData(string.Format("SELECT cellulare FROM pazienti WHERE idp = '{0}'", idp)));
            telefono = Convert.ToString(db.getData(string.Format("SELECT telefono FROM pazienti WHERE idp = '{0}'", idp)));
            email = Convert.ToString(db.getData(string.Format("SELECT email FROM pazienti WHERE idp = '{0}'", idp)));
            codicefiscale = Convert.ToString(db.getData(string.Format("SELECT codiceFiscale FROM pazienti WHERE idp = '{0}'", idp)));
        }

        private void ViewUser_Load(object sender, EventArgs e) {

        }
    }
}
