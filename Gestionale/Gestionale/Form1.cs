using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Gestionale
{
    public partial class Form1 : Form
    {

        public Form1() {
            InitializeComponent();
        }

        private void createTable_Click(object sender, EventArgs e) {
            database d = new database();
            d.createTable();
        }

        private void zonaPazienti_Click(object sender, EventArgs e) {
            users u = new users();
            this.Hide();
            u.ShowDialog();
            this.Show();
        }

        // Vaccini normali
        private void button2_Click(object sender, EventArgs e) {
            VacciniN v = new VacciniN();
            this.Hide();
            v.ShowDialog();
            this.Show();
        }

        //medici
        private void button1_Click(object sender, EventArgs e) {
            Medici m = new Medici();
            this.Hide();
            m.ShowDialog();
            this.Show();
        }

        // vaccini covid
        private void button3_Click(object sender, EventArgs e) {
            VacciniC m = new VacciniC();
            this.Hide();
            m.ShowDialog();
            this.Show();
        }
        // strutture
        private void button5_Click(object sender, EventArgs e) {
            Strutture s = new Strutture();
            this.Hide();
            s.ShowDialog();
            this.Show();
        }
        // studi medici
        private void button6_Click(object sender, EventArgs e) {
            StudiMedici s = new StudiMedici();
            this.Hide();
            s.ShowDialog();
            this.Show();
        }
    }
}
