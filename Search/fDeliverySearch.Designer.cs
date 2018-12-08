namespace AlavaSoft.Search
{
    partial class fDeliverySearch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dtpTransDateStart = new System.Windows.Forms.DateTimePicker();
            this.chkTransDate = new System.Windows.Forms.CheckBox();
            this.dtpDelDateStart = new System.Windows.Forms.DateTimePicker();
            this.btnChoose = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.TableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpTransDateEnd = new System.Windows.Forms.DateTimePicker();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.dtpDelDateEnd = new System.Windows.Forms.DateTimePicker();
            this.Label1 = new System.Windows.Forms.Label();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.chkDeliveryDate = new System.Windows.Forms.CheckBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.dgvDelivery = new System.Windows.Forms.DataGridView();
            this.TableLayoutPanel4.SuspendLayout();
            this.TableLayoutPanel2.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel15.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDelivery)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpTransDateStart
            // 
            this.dtpTransDateStart.CustomFormat = "MMM-dd-yyyy";
            this.dtpTransDateStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTransDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTransDateStart.Location = new System.Drawing.Point(174, 4);
            this.dtpTransDateStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTransDateStart.Name = "dtpTransDateStart";
            this.dtpTransDateStart.Size = new System.Drawing.Size(229, 23);
            this.dtpTransDateStart.TabIndex = 2;
            // 
            // chkTransDate
            // 
            this.chkTransDate.AutoSize = true;
            this.chkTransDate.Location = new System.Drawing.Point(3, 3);
            this.chkTransDate.Name = "chkTransDate";
            this.chkTransDate.Size = new System.Drawing.Size(164, 20);
            this.chkTransDate.TabIndex = 1;
            this.chkTransDate.Text = "Transaction Date Range";
            this.chkTransDate.UseVisualStyleBackColor = true;
            // 
            // dtpDelDateStart
            // 
            this.dtpDelDateStart.CustomFormat = "MMM-dd-yyyy";
            this.dtpDelDateStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDelDateStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDelDateStart.Location = new System.Drawing.Point(174, 34);
            this.dtpDelDateStart.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpDelDateStart.Name = "dtpDelDateStart";
            this.dtpDelDateStart.Size = new System.Drawing.Size(229, 23);
            this.dtpDelDateStart.TabIndex = 5;
            // 
            // btnChoose
            // 
            this.btnChoose.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChoose.Location = new System.Drawing.Point(88, 3);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(76, 26);
            this.btnChoose.TabIndex = 10;
            this.btnChoose.Text = "&Choose";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.Location = new System.Drawing.Point(3, 3);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(79, 26);
            this.btnSearch.TabIndex = 9;
            this.btnSearch.Text = "&Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // TableLayoutPanel4
            // 
            this.TableLayoutPanel4.ColumnCount = 2;
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.TableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 109F));
            this.TableLayoutPanel4.Controls.Add(this.btnChoose, 0, 0);
            this.TableLayoutPanel4.Controls.Add(this.btnSearch, 0, 0);
            this.TableLayoutPanel4.Location = new System.Drawing.Point(174, 95);
            this.TableLayoutPanel4.Name = "TableLayoutPanel4";
            this.TableLayoutPanel4.RowCount = 1;
            this.TableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel4.Size = new System.Drawing.Size(176, 32);
            this.TableLayoutPanel4.TabIndex = 33;
            // 
            // dtpTransDateEnd
            // 
            this.dtpTransDateEnd.CustomFormat = "MMM-dd-yyyy";
            this.dtpTransDateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTransDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTransDateEnd.Location = new System.Drawing.Point(409, 4);
            this.dtpTransDateEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpTransDateEnd.Name = "dtpTransDateEnd";
            this.dtpTransDateEnd.Size = new System.Drawing.Size(216, 23);
            this.dtpTransDateEnd.TabIndex = 3;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "ALL",
            "For Request",
            "Confirmed",
            "Accounted"});
            this.cmbStatus.Location = new System.Drawing.Point(174, 64);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(229, 24);
            this.cmbStatus.TabIndex = 8;
            // 
            // dtpDelDateEnd
            // 
            this.dtpDelDateEnd.CustomFormat = "MMM-dd-yyyy";
            this.dtpDelDateEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpDelDateEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpDelDateEnd.Location = new System.Drawing.Point(409, 34);
            this.dtpDelDateEnd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpDelDateEnd.Name = "dtpDelDateEnd";
            this.dtpDelDateEnd.Size = new System.Drawing.Size(216, 23);
            this.dtpDelDateEnd.TabIndex = 6;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(3, 60);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(44, 16);
            this.Label1.TabIndex = 7;
            this.Label1.Text = "Status";
            // 
            // TableLayoutPanel2
            // 
            this.TableLayoutPanel2.ColumnCount = 4;
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 171F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 235F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 222F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.TableLayoutPanel2.Controls.Add(this.dtpTransDateEnd, 2, 0);
            this.TableLayoutPanel2.Controls.Add(this.dtpTransDateStart, 1, 0);
            this.TableLayoutPanel2.Controls.Add(this.chkTransDate, 0, 0);
            this.TableLayoutPanel2.Controls.Add(this.cmbStatus, 1, 2);
            this.TableLayoutPanel2.Controls.Add(this.Label1, 0, 2);
            this.TableLayoutPanel2.Controls.Add(this.dtpDelDateEnd, 2, 1);
            this.TableLayoutPanel2.Controls.Add(this.dtpDelDateStart, 1, 1);
            this.TableLayoutPanel2.Controls.Add(this.TableLayoutPanel4, 1, 3);
            this.TableLayoutPanel2.Controls.Add(this.chkDeliveryDate, 0, 1);
            this.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel2.Location = new System.Drawing.Point(3, 88);
            this.TableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 4;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 8F));
            this.TableLayoutPanel2.Size = new System.Drawing.Size(639, 132);
            this.TableLayoutPanel2.TabIndex = 0;
            // 
            // chkDeliveryDate
            // 
            this.chkDeliveryDate.AutoSize = true;
            this.chkDeliveryDate.Location = new System.Drawing.Point(3, 33);
            this.chkDeliveryDate.Name = "chkDeliveryDate";
            this.chkDeliveryDate.Size = new System.Drawing.Size(142, 20);
            this.chkDeliveryDate.TabIndex = 4;
            this.chkDeliveryDate.Text = "Delivery Date Range";
            this.chkDeliveryDate.UseVisualStyleBackColor = true;
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 1;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 645F));
            this.TableLayoutPanel1.Controls.Add(this.tableLayoutPanel15, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.txtMessage, 0, 3);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel2, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.dgvDelivery, 0, 2);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 4;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(645, 554);
            this.TableLayoutPanel1.TabIndex = 15;
            // 
            // tableLayoutPanel15
            // 
            this.tableLayoutPanel15.ColumnCount = 2;
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 595F));
            this.tableLayoutPanel15.Controls.Add(this.pictureBox2, 0, 0);
            this.tableLayoutPanel15.Controls.Add(this.label9, 1, 0);
            this.tableLayoutPanel15.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel15.Name = "tableLayoutPanel15";
            this.tableLayoutPanel15.RowCount = 1;
            this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel15.Size = new System.Drawing.Size(548, 76);
            this.tableLayoutPanel15.TabIndex = 65;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AlavaSoft.Properties.Resources.DeliverySearch;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(83, 70);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Copperplate Gothic Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Maroon;
            this.label9.Location = new System.Drawing.Point(92, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(205, 24);
            this.label9.TabIndex = 6;
            this.label9.Text = "Delivery Search";
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(3, 517);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(639, 32);
            this.txtMessage.TabIndex = 56;
            // 
            // dgvDelivery
            // 
            this.dgvDelivery.AllowUserToAddRows = false;
            this.dgvDelivery.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvDelivery.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvDelivery.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvDelivery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDelivery.Location = new System.Drawing.Point(3, 229);
            this.dgvDelivery.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvDelivery.MultiSelect = false;
            this.dgvDelivery.Name = "dgvDelivery";
            this.dgvDelivery.ReadOnly = true;
            this.dgvDelivery.RowHeadersVisible = false;
            this.dgvDelivery.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDelivery.Size = new System.Drawing.Size(639, 278);
            this.dgvDelivery.TabIndex = 11;
            this.dgvDelivery.DoubleClick += new System.EventHandler(this.dgvDelivery_DoubleClick);
            // 
            // fDeliverySearch
            // 
            this.AcceptButton = this.btnSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 554);
            this.Controls.Add(this.TableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fDeliverySearch";
            this.Text = "Query Page";
            this.Load += new System.EventHandler(this.fDeliverySearch_Load);
            this.TableLayoutPanel4.ResumeLayout(false);
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel2.PerformLayout();
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel15.ResumeLayout(false);
            this.tableLayoutPanel15.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDelivery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.DateTimePicker dtpTransDateStart;
        internal System.Windows.Forms.CheckBox chkTransDate;
        internal System.Windows.Forms.DateTimePicker dtpDelDateStart;
        internal System.Windows.Forms.Button btnChoose;
        internal System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel4;
        internal System.Windows.Forms.DateTimePicker dtpTransDateEnd;
        internal System.Windows.Forms.ComboBox cmbStatus;
        internal System.Windows.Forms.DateTimePicker dtpDelDateEnd;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        internal System.Windows.Forms.CheckBox chkDeliveryDate;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.DataGridView dgvDelivery;
        internal System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel15;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.Label label9;
    }
}