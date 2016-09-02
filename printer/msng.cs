using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace printer
{
    public class msngcentr : TableLayoutPanel
    {
        public msngcentr()
        {
            ColumnCount = 1;
            ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            RowCount = 0;
        }

        public void Add(Form1.cprinter p, string name, string error)
        {
            RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            RowCount++;
            var msng3 = new msng(p, name, error);
            Controls.Add(msng3, 0, RowCount - 1);

            //var q = new msng(name, error) {Location = new System.Drawing.Point( };
            //msngs.Add(q);
        }

        public void Remove(int index)
        {
            RowStyles.RemoveAt(index);
            Controls.RemoveAt(index);
            var c = new Control[Controls.Count];
            Controls.CopyTo(c, 0);
            var C = Controls.Count;
            RowCount = Controls.Count;
            for (int i = index; i < C; i++)
            {
                RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
                Controls.Add(c[i], 0, i);
            }
        }
    }

    public class msng : GroupBox
    {
        private Button Ok = new Button() { Text = "Ok" },
            Cancel = new Button() { Text = "Cancel" };

        private TextBox Summ = new TextBox() { };
        private string Name, Error;
        public Form1.cprinter print;

        public msng(Form1.cprinter p, string name, string error)
        {
            print = p;
            Name = name;
            Error = error;
            Visible = true;
            Dock = DockStyle.Fill;
            Text = name + " " + error;
            Summ.Parent = this;
            Summ.Dock = DockStyle.Top;
            Summ.KeyPress += Summ_KeyPress;
            Ok.Parent = this;
            Ok.Location = new System.Drawing.Point(0, Summ.Bottom);
            Ok.Size = new System.Drawing.Size(55, 22);
            Ok.Click += Ok_Click;
            Cancel.Parent = this;
            Cancel.Location = new System.Drawing.Point(Ok.Right, Summ.Bottom);
            Cancel.Size = new System.Drawing.Size(55, 22);
            Cancel.Click += Cancel_Click;
        }

        private void Summ_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            (Parent as msngcentr).Remove((Parent as msngcentr).Controls.GetChildIndex(this));
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            Form1.CEadapter.Delete(print.id);
            Form1.CEdt = Form1.CEadapter.GetData();
            //this.currentErorrsTableAdapter.Fill(this.printersDataSet.CurrentErorrs);
            if (Summ.Text != "")
            {
                Form1.ELadapter.Insert(print.id, DateTime.Now, Convert.ToInt32(print.count), print.error, Convert.ToInt32(Summ.Text));
            }
            else
            {
                Form1.ELadapter.Insert(print.id, DateTime.Now, Convert.ToInt32(print.count), print.error, 0);
            }
            (Parent as msngcentr).Remove((Parent as msngcentr).Controls.GetChildIndex(this));
        }
    }
}