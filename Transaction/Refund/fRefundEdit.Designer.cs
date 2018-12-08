namespace AlavaSoft.Transaction.Refund
{
    partial class fRefundEdit
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
            this.Label6 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblSlipNo = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.txtReceivedBy = new System.Windows.Forms.TextBox();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.TableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtAmountReleased = new System.Windows.Forms.TextBox();
            this.txtRefundAmount = new System.Windows.Forms.TextBox();
            this.dtpTransactionDate = new System.Windows.Forms.DateTimePicker();
            this.label17 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.TableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.LinkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblSONo = new System.Windows.Forms.Label();
            this.TableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.TableLayoutPanel2.SuspendLayout();
            this.TableLayoutPanel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(301, 31);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(117, 16);
            this.Label6.TabIndex = 11;
            this.Label6.Text = "*Refund Amount";
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(301, 61);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(108, 16);
            this.Label8.TabIndex = 13;
            this.Label8.Text = "Amount Released";
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(3, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(74, 26);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblSlipNo
            // 
            this.lblSlipNo.AutoSize = true;
            this.lblSlipNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSlipNo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSlipNo.Location = new System.Drawing.Point(125, 0);
            this.lblSlipNo.Name = "lblSlipNo";
            this.lblSlipNo.Size = new System.Drawing.Size(170, 31);
            this.lblSlipNo.TabIndex = 2;
            this.lblSlipNo.Text = "<Slip No>";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.ForeColor = System.Drawing.Color.Maroon;
            this.Label1.Location = new System.Drawing.Point(301, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(113, 16);
            this.Label1.TabIndex = 19;
            this.Label1.Text = "*Transaction Date";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(3, 61);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(76, 16);
            this.Label11.TabIndex = 5;
            this.Label11.Text = "Received By";
            // 
            // txtReceivedBy
            // 
            this.txtReceivedBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtReceivedBy.Location = new System.Drawing.Point(125, 64);
            this.txtReceivedBy.MaxLength = 50;
            this.txtReceivedBy.Name = "txtReceivedBy";
            this.txtReceivedBy.Size = new System.Drawing.Size(170, 23);
            this.txtReceivedBy.TabIndex = 6;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRemarks.Location = new System.Drawing.Point(125, 93);
            this.txtRemarks.MaxLength = 100;
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(170, 49);
            this.txtRemarks.TabIndex = 8;
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(3, 90);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(58, 16);
            this.Label12.TabIndex = 7;
            this.Label12.Text = "Remarks";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.ColumnCount = 1;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.Controls.Add(this.tableLayoutPanel9, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.txtMessage, 0, 2);
            this.TableLayoutPanel1.Controls.Add(this.TableLayoutPanel2, 0, 1);
            this.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.TableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 3;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 84F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(610, 322);
            this.TableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel9
            // 
            this.tableLayoutPanel9.ColumnCount = 2;
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 89F));
            this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 595F));
            this.tableLayoutPanel9.Controls.Add(this.pictureBox2, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.label14, 1, 0);
            this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.tableLayoutPanel9.RowCount = 1;
            this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel9.Size = new System.Drawing.Size(484, 76);
            this.tableLayoutPanel9.TabIndex = 60;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AlavaSoft.Properties.Resources.Refund;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(83, 70);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // label14
            // 
            this.label14.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Copperplate Gothic Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Navy;
            this.label14.Location = new System.Drawing.Point(92, 26);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(303, 24);
            this.label14.TabIndex = 6;
            this.label14.Text = "Edit Refund Information";
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtMessage.Location = new System.Drawing.Point(3, 280);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ReadOnly = true;
            this.txtMessage.Size = new System.Drawing.Size(604, 39);
            this.txtMessage.TabIndex = 19;
            this.txtMessage.Text = "Fields with * are required.";
            // 
            // TableLayoutPanel2
            // 
            this.TableLayoutPanel2.ColumnCount = 4;
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 122F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 129F));
            this.TableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel2.Controls.Add(this.txtAmountReleased, 3, 2);
            this.TableLayoutPanel2.Controls.Add(this.txtRefundAmount, 3, 1);
            this.TableLayoutPanel2.Controls.Add(this.Label6, 2, 1);
            this.TableLayoutPanel2.Controls.Add(this.dtpTransactionDate, 3, 0);
            this.TableLayoutPanel2.Controls.Add(this.lblSlipNo, 1, 0);
            this.TableLayoutPanel2.Controls.Add(this.Label1, 2, 0);
            this.TableLayoutPanel2.Controls.Add(this.Label8, 2, 2);
            this.TableLayoutPanel2.Controls.Add(this.label17, 2, 3);
            this.TableLayoutPanel2.Controls.Add(this.cmbStatus, 3, 3);
            this.TableLayoutPanel2.Controls.Add(this.TableLayoutPanel7, 1, 4);
            this.TableLayoutPanel2.Controls.Add(this.label2, 0, 1);
            this.TableLayoutPanel2.Controls.Add(this.Label11, 0, 2);
            this.TableLayoutPanel2.Controls.Add(this.Label12, 0, 3);
            this.TableLayoutPanel2.Controls.Add(this.txtReceivedBy, 1, 2);
            this.TableLayoutPanel2.Controls.Add(this.txtRemarks, 1, 3);
            this.TableLayoutPanel2.Controls.Add(this.LinkLabel1, 0, 0);
            this.TableLayoutPanel2.Controls.Add(this.lblSONo, 1, 1);
            this.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TableLayoutPanel2.Location = new System.Drawing.Point(3, 88);
            this.TableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.TableLayoutPanel2.Name = "TableLayoutPanel2";
            this.TableLayoutPanel2.RowCount = 5;
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.TableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.TableLayoutPanel2.Size = new System.Drawing.Size(604, 185);
            this.TableLayoutPanel2.TabIndex = 7;
            // 
            // txtAmountReleased
            // 
            this.txtAmountReleased.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAmountReleased.Location = new System.Drawing.Point(430, 64);
            this.txtAmountReleased.MaxLength = 15;
            this.txtAmountReleased.Name = "txtAmountReleased";
            this.txtAmountReleased.Size = new System.Drawing.Size(171, 23);
            this.txtAmountReleased.TabIndex = 14;
            this.txtAmountReleased.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRefundAmount
            // 
            this.txtRefundAmount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRefundAmount.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRefundAmount.Location = new System.Drawing.Point(430, 34);
            this.txtRefundAmount.MaxLength = 15;
            this.txtRefundAmount.Name = "txtRefundAmount";
            this.txtRefundAmount.Size = new System.Drawing.Size(171, 23);
            this.txtRefundAmount.TabIndex = 12;
            this.txtRefundAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // dtpTransactionDate
            // 
            this.dtpTransactionDate.CustomFormat = "MMM-dd-yyyy";
            this.dtpTransactionDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dtpTransactionDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTransactionDate.Location = new System.Drawing.Point(430, 5);
            this.dtpTransactionDate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dtpTransactionDate.Name = "dtpTransactionDate";
            this.dtpTransactionDate.Size = new System.Drawing.Size(171, 23);
            this.dtpTransactionDate.TabIndex = 10;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Maroon;
            this.label17.Location = new System.Drawing.Point(301, 90);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(52, 16);
            this.label17.TabIndex = 15;
            this.label17.Text = "*Status";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "Unreleased",
            "Released"});
            this.cmbStatus.Location = new System.Drawing.Point(430, 93);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(171, 24);
            this.cmbStatus.TabIndex = 16;
            this.cmbStatus.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_SelectedIndexChanged);
            // 
            // TableLayoutPanel7
            // 
            this.TableLayoutPanel7.ColumnCount = 2;
            this.TableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.TableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 83F));
            this.TableLayoutPanel7.Controls.Add(this.btnDelete, 0, 0);
            this.TableLayoutPanel7.Controls.Add(this.btnSave, 0, 0);
            this.TableLayoutPanel7.Location = new System.Drawing.Point(125, 148);
            this.TableLayoutPanel7.Name = "TableLayoutPanel7";
            this.TableLayoutPanel7.RowCount = 1;
            this.TableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TableLayoutPanel7.Size = new System.Drawing.Size(163, 33);
            this.TableLayoutPanel7.TabIndex = 70;
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(83, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 26);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Sales Order No";
            // 
            // LinkLabel1
            // 
            this.LinkLabel1.AutoSize = true;
            this.LinkLabel1.Location = new System.Drawing.Point(3, 0);
            this.LinkLabel1.Name = "LinkLabel1";
            this.LinkLabel1.Size = new System.Drawing.Size(92, 16);
            this.LinkLabel1.TabIndex = 1;
            this.LinkLabel1.TabStop = true;
            this.LinkLabel1.Text = "Refund Slip No";
            this.LinkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // lblSONo
            // 
            this.lblSONo.AutoSize = true;
            this.lblSONo.Location = new System.Drawing.Point(125, 31);
            this.lblSONo.Name = "lblSONo";
            this.lblSONo.Size = new System.Drawing.Size(62, 16);
            this.lblSONo.TabIndex = 4;
            this.lblSONo.Text = "<SO No>";
            // 
            // fRefundEdit
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(610, 322);
            this.Controls.Add(this.TableLayoutPanel1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "fRefundEdit";
            this.Text = "Sales Refund";
            this.TableLayoutPanel1.ResumeLayout(false);
            this.TableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.TableLayoutPanel2.ResumeLayout(false);
            this.TableLayoutPanel2.PerformLayout();
            this.TableLayoutPanel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Label lblSlipNo;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.TextBox txtReceivedBy;
        internal System.Windows.Forms.TextBox txtRemarks;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel1;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel2;
        internal System.Windows.Forms.TableLayoutPanel TableLayoutPanel7;
        internal System.Windows.Forms.Label label17;
        public System.Windows.Forms.TextBox txtMessage;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.TextBox txtAmountReleased;
        internal System.Windows.Forms.TextBox txtRefundAmount;
        internal System.Windows.Forms.DateTimePicker dtpTransactionDate;
        internal System.Windows.Forms.ComboBox cmbStatus;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.LinkLabel LinkLabel1;
        internal System.Windows.Forms.Label lblSONo;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
        private System.Windows.Forms.PictureBox pictureBox2;
        internal System.Windows.Forms.Label label14;
    }
}