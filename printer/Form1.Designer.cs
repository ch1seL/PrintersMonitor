namespace printer
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataSet1 = new System.Data.DataSet();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.printersDataSet = new printersDataSet();
            this.printersDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.printersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.printersTableAdapter = new printersDataSetTableAdapters.PrintersTableAdapter();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.invnomDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.markidDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.comentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ipDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printersDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printersDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.printersBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(612, 30);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(278, 354);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 389);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300000;
            this.timer1.Tick += new System.EventHandler(this.Time_Tick);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idDataGridViewTextBoxColumn,
            this.invnomDataGridViewTextBoxColumn,
            this.markidDataGridViewTextBoxColumn,
            this.comentDataGridViewTextBoxColumn,
            this.ipDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.printersBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 30);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(594, 353);
            this.dataGridView1.TabIndex = 4;
            // 
            // printersDataSet
            // 
            this.printersDataSet.DataSetName = "printersDataSet";
            this.printersDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // printersDataSetBindingSource
            // 
            this.printersDataSetBindingSource.DataSource = this.printersDataSet;
            this.printersDataSetBindingSource.Position = 0;
            // 
            // printersBindingSource
            // 
            this.printersBindingSource.DataMember = "Printers";
            this.printersBindingSource.DataSource = this.printersDataSetBindingSource;
            // 
            // printersTableAdapter
            // 
            this.printersTableAdapter.ClearBeforeFill = true;
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "id";
            this.idDataGridViewTextBoxColumn.HeaderText = "id";
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            // 
            // invnomDataGridViewTextBoxColumn
            // 
            this.invnomDataGridViewTextBoxColumn.DataPropertyName = "invnom";
            this.invnomDataGridViewTextBoxColumn.HeaderText = "invnom";
            this.invnomDataGridViewTextBoxColumn.Name = "invnomDataGridViewTextBoxColumn";
            // 
            // markidDataGridViewTextBoxColumn
            // 
            this.markidDataGridViewTextBoxColumn.DataPropertyName = "mark_id";
            this.markidDataGridViewTextBoxColumn.HeaderText = "mark_id";
            this.markidDataGridViewTextBoxColumn.Name = "markidDataGridViewTextBoxColumn";
            // 
            // comentDataGridViewTextBoxColumn
            // 
            this.comentDataGridViewTextBoxColumn.DataPropertyName = "coment";
            this.comentDataGridViewTextBoxColumn.HeaderText = "coment";
            this.comentDataGridViewTextBoxColumn.Name = "comentDataGridViewTextBoxColumn";
            // 
            // ipDataGridViewTextBoxColumn
            // 
            this.ipDataGridViewTextBoxColumn.DataPropertyName = "ip";
            this.ipDataGridViewTextBoxColumn.HeaderText = "ip";
            this.ipDataGridViewTextBoxColumn.Name = "ipDataGridViewTextBoxColumn";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 424);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printersDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printersDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.printersBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource printersDataSetBindingSource;
        private printersDataSet printersDataSet;
        private System.Windows.Forms.BindingSource printersBindingSource;
        private printersDataSetTableAdapters.PrintersTableAdapter printersTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn invnomDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn markidDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn comentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ipDataGridViewTextBoxColumn;
    }
}

