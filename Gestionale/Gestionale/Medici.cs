﻿using System;
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
    public partial class Medici : Form
    {
        database db = new database();

        public string[] valori = new string[36];
        public Medici() {
            InitializeComponent();
            carica();
            stampaLista();
        }

        public void carica() {
            for (int i = 0; i < 36; i++)
            {
                if (i < 10)
                {
                    valori[i] = Convert.ToString(i);
                }
                else
                {
                    valori[i] = Convert.ToString((char)('a' + i - 10));
                }
            }
        }

        public string UUID() {
            string cu = "";
            Random rnd = new Random();
            for (int i = 0; i < 15; i++)
            {
                if (cu.Length == 3 || cu.Length == 12)
                {
                    cu += "-";
                }
                else
                {
                    cu += valori[rnd.Next(0, 36)];
                }
            }
            return cu.ToUpper();
        }

        private void Medici_Load(object sender, EventArgs e) {

        }


        private void stampaLista() {
            string comandosql = "SELECT idpe,cognome,nome,codiceFiscale,tipo FROM personale";
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

        // 0 Medico 
        // 1 Infermiere

        private void button1_Click_1(object sender, EventArgs e) {
            if (txtNome.Text != "" && comboBox1.Text != "" && txtCognome.Text != "" && textCF.Text != "" && txtDataDiNascita.Text != "" && txtLuogoDN.Text != "" && txtIndirizzo.Text != "" && txtCel.Text != "" && txtEmail.Text != "")
            {
                if (db.rowCount(string.Format("SELECT COUNT(*) FROM personale WHERE codiceFiscale = '{0}'", textCF.Text)) == 0)
                {
                    db.esegui(string.Format("INSERT INTO personale(idpe, tipo, nome, cognome, codiceFiscale, datadinascita, luogodinascita, indirizzo, cellulare, email) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')", UUID(), comboBox1.Text, txtNome.Text, txtCognome.Text, textCF.Text, txtDataDiNascita.Text, txtLuogoDN.Text, txtIndirizzo.Text, txtCel.Text, txtEmail.Text));
                    stampaLista();
                    txtNome.Text = txtCognome.Text = textCF.Text = txtDataDiNascita.Text = txtLuogoDN.Text = txtIndirizzo.Text = txtCel.Text = txtEmail.Text = "";
                }
                else
                {
                    MessageBox.Show("Medico già inserito già inserito");
                }
            }
            else
            {
                MessageBox.Show("Alcuni campi risultano vuoti");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }
}