﻿using System;
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
            label1.Text = cognome + " " + nome;
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

        private void ViewMedico_Load(object sender, EventArgs e) {

        }
    }
}
