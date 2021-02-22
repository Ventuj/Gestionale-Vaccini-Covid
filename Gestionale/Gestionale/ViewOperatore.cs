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
    public partial class ViewOperatore : Form
    {
        database db = new database();
        string idop = "";
        public ViewOperatore(string id) {
            InitializeComponent();
            idop = id;
        }
    }
}
