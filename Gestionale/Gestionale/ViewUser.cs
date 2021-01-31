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
    public partial class ViewUser : Form
    {
        string idp = "";
        public ViewUser(string id) {
            InitializeComponent();
            idp = id;
            label1.Text = Convert.ToString(idp);
        }

        private void ViewUser_Load(object sender, EventArgs e) {

        }
    }
}
