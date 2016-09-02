using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace printer
{
    public partial class AddError : Form
    {
        private printersDataSetTableAdapters.ErrorListTableAdapter Eadapter = new printersDataSetTableAdapters.ErrorListTableAdapter();
        int id;

        public AddError(int ID, string comment)
        {
            InitializeComponent();
            Text = comment;
            id = ID;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Eadapter.Insert(id, textBox1.Text);
            Close();
        }
        
    }
}
