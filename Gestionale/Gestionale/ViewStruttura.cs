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
        string ids = "";
        public ViewStruttura(string id) {
            InitializeComponent();
            ids = id;
        }
    }
}
