namespace AlavaSoft.Transaction.Product
{
    partial class fInventoryEdit
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
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.Label7 = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvProduct = new System.Windows.Forms.DataGridView();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Add = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Subtract = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InventoryCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.dtpReferenceDate = new System.Windows.Forms.DateTimePicker();
            this.dtpTransactionDate = new System.Windows.Forms.DateTimePicker();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtReferenceNo = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.cmbAlterReason = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.btnProductSearch = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.TableLayoutPanel3.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).BeginInit();
            this.TableLayoutPanel2.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TextBox2
            // 
            this.TextBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBox2.Location = new System.Drawing.Point(172, 45);
            this.TextBox2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.TextBox2.Multiline = true;
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(1, 50);
            this.TextBox2.TabIndex = 44;
            // 
            // TableLayoutPanel3
            // 
            this.TableLayoutPanel3.ColumnCount = 4;
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 169F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 184F));
            this.TableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel3.Controls.Add(this.TextBox2, 1, 2);
            this.TableLayoutPanel3.Controls.Add(this.DateTimePicker2, 3, 0);
            this.TableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel3.Name = "TableLayoutPanel3";
            this.TableLayoutPanel3.RowCount = 3;
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel3.Size = new System.Drawing.Size(200, 100);
            this.TableLayoutPanel3.TabIndex = 2;
            // 
            // DateTimePicker2
            // 
            this.DateTimePicker2.CustomFormat = "MMM-dd-yyyy";
            this.DateTimePicker2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DateTimePicker2.Location = new System.Drawing.Point(280, 5);
            this.DateTimePicker2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.DateTimePicker2.Name = "DateTimePicker2";
            this.DateTimePicker2.Size = new System.Drawing.Size(1, 23);
            this.DateTimePicker2.TabIndex = 39;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(3, 0);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(45, 16);
            this.Label7.TabIndex = 1;
            this.Label7.Text = "Label7";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.dgvProduct);
            this.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GroupBox2.Location = new System.Drawing.Point(3, 245);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(720, 204);
            this.GroupBox2.TabIndex = 53;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Details";
            // 
            // dgvProduct
            // 
            this.dgvProduct.AllowUserToAddRows = false;
            this.dgvProduct.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvProduct.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProduct.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductCode,
            this.Column1,
            this.ProductName,
            this.Column2,
            this.Add,
            this.Subtract,
            this.InventoryCount});
            this.dgvProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvProduct.Location = new System.Drawing.Point(3, 19);
            this.dgvProduct.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvProduct.MultiSelect = false;
            this.dgvProduct.Name = "dgvProduct";
            this.dgvProduct.ReadOnly = true;
            this.dgvProduct.RowHeadersVisible = false;
            this.dgvProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProduct.Size = new System.Drawing.Size(714, 182);
            this.dgvProduct.TabIndex = 12;
            // 
            // ProductCode
            // 
            this.ProductCode.HeaderText = "Product Code";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.ReadOnly = true;
            this.ProductCode.Width = 109;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Category";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 84;
            // 
            // ProductName
            // 
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 113;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Pack Size";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 87;
            // 
            // Add
            // 
            this.Add.HeaderText = "Add (PC)";
            this.Add.Name = "Add";
            this.Add.ReadOnly = true;
            this.Add.Width = 84;
            // 
            // Subtract
            // 
            this.Subtract.HeaderText = "Subtract (PC)";
            this.Subtract.Name = "Subtract";
            this.Subtract.ReadOnly = true;
            this.Subtract.Width = 110;
            // 
            // InventoryCount
            // 
            this.InventoryCount.HeaderText = "Inventory Count";
            this.InventoryCount.Name = "InventoryCount";
            this.InventoryCount.ReadOnly = true;
            this.InventoryCount.Width = 123;
            // 
            // TableLayoutPanel2
            // 
            this.TableLayoutPanel2.ColumnCount = 4;
            this.TableLayoutPanel1.SetColumnSpan(this.TableLayoutPanel2, 4);
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 123F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.Controls.Add(this.dtpReferenceDate, 3, 1);
            this.TableLayoutPanel2.Controls.Add(this.dtpTransactionDate, 3, 0);
            this.TableLayoutPanel2.Controls.Add(this.Label5, 2, 0);
            this.TableLayoutPanel2.Controls.Add(this.Label3, 0, 0);
            this.TableLayoutPanel2.Controls.Add(this.txtReferenceNo, 1, 0);
            this.TableLayoutPanel2.Controls.Add(this.Label6, 0, 1);
            this.TableLayoutPanel2.Controls.Add(this.Label4, 0, 2);
            this.TableLayoutPanel2.Controls.Add(this.txtRemarks, 1, 2);
            this.TableLayoutPanel2.Controls.Add(this.cmbAlterReason, 1, 1);
            this.TableLayoutPanel2.Controls.Add(this.label2, 2, 1);
            this.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel2.Location = new System.Drawing.Point(3, 86);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 3;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.TableLayoutPanel2.Size = new System.Drawing.Size(720, 120);
            this.TableLayoutPanel2.TabIndex = 3;
            // 
            // dtpReferenceDate
            // 
            this.dtpReferenceDate.Checked = false;
            this.dtpReferenceDate.CustomFormat = "MMM-dd-yyyy";
            this.dtpReferenceDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpReferenceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpReferenceDate.Location = new System.Drawing.Point(476, 37);
            this.dtpReferenceDate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dtpReferenceDate.Name = "dtpReferenceDate";
            this.dtpReferenceDate.ShowCheckBox = true;
            this.dtpReferenceDate.Size = new System.Drawing.Size(241, 23);
            this.dtpReferenceDate.TabIndex = 10;
            // 
            // dtpTransactionDate
            // 
            this.dtpTransactionDate.CustomFormat = "MMM-dd-yyyy";
            this.dtpTransactionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTransactionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTransactionDate.Location = new System.Drawing.Point(476, 5);
            this.dtpTransactionDate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dtpTransactionDate.Name = "dtpTransactionDate";
            this.dtpTransactionDate.Size = new System.Drawing.Size(241, 23);
            this.dtpTransactionDate.TabIndex = 8;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.ForeColor = System.Drawing.Color.Maroon;
            this.Label5.Location = new System.Drawing.Point(353, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(113, 16);
            this.Label5.TabIndex = 7;
            this.Label5.Text = "*Transaction Date";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(3, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(85, 16);
            this.Label3.TabIndex = 1;
            this.Label3.Text = "Reference No";
            // 
            // txtReferenceNo
            // 
            this.txtReferenceNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReferenceNo.Location = new System.Drawing.Point(107, 3);
            this.txtReferenceNo.MaxLength = 50;
            this.txtReferenceNo.Name = "txtReferenceNo";
            this.txtReferenceNo.Size = new System.Drawing.Size(240, 23);
            this.txtReferenceNo.TabIndex = 2;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(3, 32);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(81, 16);
            this.Label6.TabIndex = 3;
            this.Label6.Text = "Alter Reason";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(3, 62);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(58, 16);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "Remarks";
            // 
            // txtRemarks
            // 
            this.txtRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemarks.Location = new System.Drawing.Point(107, 67);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtRemarks.MaxLength = 100;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(240, 48);
            this.txtRemarks.TabIndex = 6;
            // 
            // cmbAlterReason
            // 
            this.cmbAlterReason.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbAlterReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlterReason.FormattingEnabled = true;
            this.cmbAlterReason.Location = new System.Drawing.Point(107, 35);
            this.cmbAlterReason.Name = "cmbAlterReason";
            this.cmbAlterReason.Size = new System.Drawing.Size(240, 24);
            this.cmbAlterReason.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(353, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Reference Date";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 1;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.txtMessage, 0, 5);
            this.TableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 4);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel2, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.GroupBox2, 0, 3);
            this.TableLayoutPanel1.Controls.Add(this.linkLabel1, 0, 2);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 6;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(726, 540);
            this.TableLayoutPanel1.TabIndex = 3;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 595F));
            this.tableLayoutPanel9.Controls.Add(this.pictureBox2, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(484, 76);
            this.tableLayoutPanel9.TabIndex = 57;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AlavaSoft.Properties.Resources.Inventory;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(83, 70);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Copperplate Gothic Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Navy;
            this.label8.Location = new System.Drawing.Point(92, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(260, 24);
            this.label8.TabIndex = 6;
            this.label8.Text = "Edit Inventory Stock";
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(3, 499);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(720, 36);
            this.txtMessage.TabIndex = 16;
            this.txtMessage.Text = "Fields with * are required.";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnSave);
            this.flowLayoutPanel1.Controls.Add(this.btnDeleteProduct);
            this.flowLayoutPanel1.Controls.Add(this.btnProductSearch);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 455);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(720, 36);
            this.flowLayoutPanel1.TabIndex = 54;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(630, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(87, 26);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDeleteProduct
            // 
            this.btnDeleteProduct.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteProduct.Location = new System.Drawing.Point(558, 3);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(66, 26);
            this.btnDeleteProduct.TabIndex = 14;
            this.btnDeleteProduct.Text = "Remove";
            this.btnDeleteProduct.UseVisualStyleBackColor = true;
            this.btnDeleteProduct.Click += new System.EventHandler(this.btnDeleteProduct_Click);
            // 
            // btnProductSearch
            // 
            this.btnProductSearch.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductSearch.Location = new System.Drawing.Point(465, 3);
            this.btnProductSearch.Name = "btnProductSearch";
            this.btnProductSearch.Size = new System.Drawing.Size(87, 26);
            this.btnProductSearch.TabIndex = 13;
            this.btnProductSearch.Text = "Add Product";
            this.btnProductSearch.UseVisualStyleBackColor = true;
            this.btnProductSearch.Click += new System.EventHandler(this.btnProductSearch_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(3, 209);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(77, 16);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Add Product";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // fInventoryEdit
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 540);
            this.Controls.Add(this.TableLayoutPanel1);
            this.Controls.Add(this.TableLayoutPanel3);
            this.Controls.Add(this.Label7);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fInventoryEdit";
            this.Text = "Edit Inventory";
            this.Load += new System.EventHandler(this.fInventoryEdit_Load);
            this.TableLayoutPanel3.ResumeLayout(false);
            this.TableLayoutPanel3.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvProduct)).EndInit();
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel2.PerformLayout();
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox TextBox2;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel3;
        internal System.Windows.Forms.DateTimePicker DateTimePicker2;
        internal System.Windows.Forms.Label Label7;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.DataGridView dgvProduct;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        internal System.Windows.Forms.DateTimePicker dtpReferenceDate;
        internal System.Windows.Forms.DateTimePicker dtpTransactionDate;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtReferenceNo;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtRemarks;
        internal System.Windows.Forms.ComboBox cmbAlterReason;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        internal System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.LinkLabel linkLabel1;
        internal System.Windows.Forms.Button btnDeleteProduct;
        internal System.Windows.Forms.TextBox txtMessage;
        internal System.Windows.Forms.Button btnProductSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Add;
        private System.Windows.Forms.DataGridViewTextBoxColumn Subtract;
        private System.Windows.Forms.DataGridViewTextBoxColumn InventoryCount;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.Label label8;
    }
}