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
    public partial class Medici : Form
    {
        database db = new database();

        public string[] valori = new string[36];
        public Medici() {
            InitializeComponent();
            carica();
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
    }
}
