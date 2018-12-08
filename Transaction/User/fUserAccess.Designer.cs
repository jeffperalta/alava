namespace AlavaSoft.Transaction.User
{
    partial class fUserAccess
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.gb = new System.Windows.Forms.GroupBox();
            this.chkCustomer = new System.Windows.Forms.CheckBox();
            this.chkDelivery = new System.Windows.Forms.CheckBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkAdministration = new System.Windows.Forms.CheckBox();
            this.chkQuery = new System.Windows.Forms.CheckBox();
            this.chkReports = new System.Windows.Forms.CheckBox();
            this.chkSupplier = new System.Windows.Forms.CheckBox();
            this.chkReturns = new System.Windows.Forms.CheckBox();
            this.chkRefund = new System.Windows.Forms.CheckBox();
            this.chkInventory = new System.Windows.Forms.CheckBox();
            this.chkProduct = new System.Windows.Forms.CheckBox();
            this.chkUser = new System.Windows.Forms.CheckBox();
            this.chkVoidPayment = new System.Windows.Forms.CheckBox();
            this.chkPayment = new System.Windows.Forms.CheckBox();
            this.chkSO = new System.Windows.Forms.CheckBox();
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.gb.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtMessage, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.gb, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgvUser, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 226F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(512, 529);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 595F));
            this.tableLayoutPanel9.Controls.Add(this.pictureBox2, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(497, 76);
            this.tableLayoutPanel9.TabIndex = 64;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AlavaSoft.Properties.Resources.UserAccess;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(83, 70);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Copperplate Gothic Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(92, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Edit User Access";
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.ForeColor = System.Drawing.Color.Black;
            this.txtMessage.Location = new System.Drawing.Point(3, 491);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(506, 33);
            this.txtMessage.TabIndex = 17;
            // 
            // gb
            // 
            this.gb.Controls.Add(this.chkCustomer);
            this.gb.Controls.Add(this.chkDelivery);
            this.gb.Controls.Add(this.lblName);
            this.gb.Controls.Add(this.btnSave);
            this.gb.Controls.Add(this.chkAdministration);
            this.gb.Controls.Add(this.chkQuery);
            this.gb.Controls.Add(this.chkReports);
            this.gb.Controls.Add(this.chkSupplier);
            this.gb.Controls.Add(this.chkReturns);
            this.gb.Controls.Add(this.chkRefund);
            this.gb.Controls.Add(this.chkInventory);
            this.gb.Controls.Add(this.chkProduct);
            this.gb.Controls.Add(this.chkUser);
            this.gb.Controls.Add(this.chkVoidPayment);
            this.gb.Controls.Add(this.chkPayment);
            this.gb.Controls.Add(this.chkSO);
            this.gb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gb.Location = new System.Drawing.Point(3, 263);
            this.gb.Name = "gb";
            this.gb.Size = new System.Drawing.Size(506, 220);
            this.gb.TabIndex = 8;
            this.gb.TabStop = false;
            this.gb.Text = "Allow User Access to the following System Modules";
            // 
            // chkCustomer
            // 
            this.chkCustomer.AutoSize = true;
            this.chkCustomer.Location = new System.Drawing.Point(384, 76);
            this.chkCustomer.Name = "chkCustomer";
            this.chkCustomer.Size = new System.Drawing.Size(82, 20);
            this.chkCustomer.TabIndex = 15;
            this.chkCustomer.Text = "Customer";
            this.chkCustomer.UseVisualStyleBackColor = true;
            // 
            // chkDelivery
            // 
            this.chkDelivery.AutoSize = true;
            this.chkDelivery.Location = new System.Drawing.Point(384, 50);
            this.chkDelivery.Name = "chkDelivery";
            this.chkDelivery.Size = new System.Drawing.Size(72, 20);
            this.chkDelivery.TabIndex = 14;
            this.chkDelivery.Text = "Delivery";
            this.chkDelivery.UseVisualStyleBackColor = true;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(34, 24);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(0, 16);
            this.lblName.TabIndex = 23;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(384, 175);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 16;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkAdministration
            // 
            this.chkAdministration.AutoSize = true;
            this.chkAdministration.Location = new System.Drawing.Point(211, 50);
            this.chkAdministration.Name = "chkAdministration";
            this.chkAdministration.Size = new System.Drawing.Size(109, 20);
            this.chkAdministration.TabIndex = 8;
            this.chkAdministration.Text = "Administration";
            this.chkAdministration.UseVisualStyleBackColor = true;
            // 
            // chkQuery
            // 
            this.chkQuery.AutoSize = true;
            this.chkQuery.Location = new System.Drawing.Point(211, 180);
            this.chkQuery.Name = "chkQuery";
            this.chkQuery.Size = new System.Drawing.Size(61, 20);
            this.chkQuery.TabIndex = 13;
            this.chkQuery.Text = "Query";
            this.chkQuery.UseVisualStyleBackColor = true;
            // 
            // chkReports
            // 
            this.chkReports.AutoSize = true;
            this.chkReports.Location = new System.Drawing.Point(211, 154);
            this.chkReports.Name = "chkReports";
            this.chkReports.Size = new System.Drawing.Size(71, 20);
            this.chkReports.TabIndex = 12;
            this.chkReports.Text = "Reports";
            this.chkReports.UseVisualStyleBackColor = true;
            // 
            // chkSupplier
            // 
            this.chkSupplier.AutoSize = true;
            this.chkSupplier.Location = new System.Drawing.Point(211, 128);
            this.chkSupplier.Name = "chkSupplier";
            this.chkSupplier.Size = new System.Drawing.Size(74, 20);
            this.chkSupplier.TabIndex = 11;
            this.chkSupplier.Text = "Supplier";
            this.chkSupplier.UseVisualStyleBackColor = true;
            // 
            // chkReturns
            // 
            this.chkReturns.AutoSize = true;
            this.chkReturns.Location = new System.Drawing.Point(211, 102);
            this.chkReturns.Name = "chkReturns";
            this.chkReturns.Size = new System.Drawing.Size(118, 20);
            this.chkReturns.TabIndex = 10;
            this.chkReturns.Text = "Product Returns";
            this.chkReturns.UseVisualStyleBackColor = true;
            // 
            // chkRefund
            // 
            this.chkRefund.AutoSize = true;
            this.chkRefund.Location = new System.Drawing.Point(211, 76);
            this.chkRefund.Name = "chkRefund";
            this.chkRefund.Size = new System.Drawing.Size(163, 20);
            this.chkRefund.TabIndex = 9;
            this.chkRefund.Text = "Refund/Excess Payment";
            this.chkRefund.UseVisualStyleBackColor = true;
            // 
            // chkInventory
            // 
            this.chkInventory.AutoSize = true;
            this.chkInventory.Location = new System.Drawing.Point(29, 180);
            this.chkInventory.Name = "chkInventory";
            this.chkInventory.Size = new System.Drawing.Size(80, 20);
            this.chkInventory.TabIndex = 7;
            this.chkInventory.Text = "Inventory";
            this.chkInventory.UseVisualStyleBackColor = true;
            // 
            // chkProduct
            // 
            this.chkProduct.AutoSize = true;
            this.chkProduct.Location = new System.Drawing.Point(29, 154);
            this.chkProduct.Name = "chkProduct";
            this.chkProduct.Size = new System.Drawing.Size(70, 20);
            this.chkProduct.TabIndex = 6;
            this.chkProduct.Text = "Product";
            this.chkProduct.UseVisualStyleBackColor = true;
            // 
            // chkUser
            // 
            this.chkUser.AutoSize = true;
            this.chkUser.Location = new System.Drawing.Point(29, 50);
            this.chkUser.Name = "chkUser";
            this.chkUser.Size = new System.Drawing.Size(163, 20);
            this.chkUser.TabIndex = 2;
            this.chkUser.Text = "Application User/Access";
            this.chkUser.UseVisualStyleBackColor = true;
            // 
            // chkVoidPayment
            // 
            this.chkVoidPayment.AutoSize = true;
            this.chkVoidPayment.Location = new System.Drawing.Point(29, 128);
            this.chkVoidPayment.Name = "chkVoidPayment";
            this.chkVoidPayment.Size = new System.Drawing.Size(156, 20);
            this.chkVoidPayment.TabIndex = 5;
            this.chkVoidPayment.Text = "Void Payment Voucher";
            this.chkVoidPayment.UseVisualStyleBackColor = true;
            // 
            // chkPayment
            // 
            this.chkPayment.AutoSize = true;
            this.chkPayment.Location = new System.Drawing.Point(29, 102);
            this.chkPayment.Name = "chkPayment";
            this.chkPayment.Size = new System.Drawing.Size(145, 20);
            this.chkPayment.TabIndex = 4;
            this.chkPayment.Text = "Payment/Remittance";
            this.chkPayment.UseVisualStyleBackColor = true;
            // 
            // chkSO
            // 
            this.chkSO.AutoSize = true;
            this.chkSO.Location = new System.Drawing.Point(29, 76);
            this.chkSO.Name = "chkSO";
            this.chkSO.Size = new System.Drawing.Size(95, 20);
            this.chkSO.TabIndex = 3;
            this.chkSO.Text = "Sales Order";
            this.chkSO.UseVisualStyleBackColor = true;
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.dgvUser.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUser.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUser.Location = new System.Drawing.Point(3, 90);
            this.dgvUser.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dgvUser.MultiSelect = false;
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.ReadOnly = true;
            this.dgvUser.RowHeadersVisible = false;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(506, 165);
            this.dgvUser.TabIndex = 1;
            this.dgvUser.Leave += new System.EventHandler(this.dgvUser_Leave);
            this.dgvUser.SelectionChanged += new System.EventHandler(this.dgvUser_SelectionChanged);
            // 
            // fUserAccess
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 529);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fUserAccess";
            this.Text = "User Access";
            this.Load += new System.EventHandler(this.fUserAccess_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.gb.ResumeLayout(false);
            this.gb.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox gb;
        private System.Windows.Forms.CheckBox chkQuery;
        private System.Windows.Forms.CheckBox chkReports;
        private System.Windows.Forms.CheckBox chkSupplier;
        private System.Windows.Forms.CheckBox chkReturns;
        private System.Windows.Forms.CheckBox chkRefund;
        private System.Windows.Forms.CheckBox chkInventory;
        private System.Windows.Forms.CheckBox chkProduct;
        private System.Windows.Forms.CheckBox chkUser;
        private System.Windows.Forms.CheckBox chkVoidPayment;
        private System.Windows.Forms.CheckBox chkPayment;
        private System.Windows.Forms.CheckBox chkSO;
        private System.Windows.Forms.CheckBox chkAdministration;
        internal System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkDelivery;
        private System.Windows.Forms.CheckBox chkCustomer;

    }
}