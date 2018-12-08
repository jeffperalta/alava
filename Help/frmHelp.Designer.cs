namespace AlavaSoft.Help
{
    partial class frmHelp
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Getting Started");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Login Page");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Menubar and Taskbar");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Workspace");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Query Page");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("AIS Environment", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Alavasoft Inventory System", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode6});
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Sales Order");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Payment");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Delivery");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Product Returns");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Refund");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Transaction", new System.Windows.Forms.TreeNode[] {
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(877, 574);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "Node0";
            treeNode1.Tag = "gettingstarted.htm";
            treeNode1.Text = "Getting Started";
            treeNode2.Name = "n1";
            treeNode2.Tag = "login.htm";
            treeNode2.Text = "Login Page";
            treeNode3.Name = "Node2";
            treeNode3.Tag = "taskbar_menubar.htm";
            treeNode3.Text = "Menubar and Taskbar";
            treeNode4.Name = "Node3";
            treeNode4.Tag = "workspace.htm";
            treeNode4.Text = "Workspace";
            treeNode5.Name = "Node4";
            treeNode5.Tag = "query.htm";
            treeNode5.Text = "Query Page";
            treeNode6.Name = "Node0";
            treeNode6.Tag = "aisenvironment.htm";
            treeNode6.Text = "AIS Environment";
            treeNode7.Name = "Node0";
            treeNode7.Tag = "index.htm";
            treeNode7.Text = "Alavasoft Inventory System";
            treeNode8.Name = "Node1";
            treeNode8.Tag = "salesorder.htm";
            treeNode8.Text = "Sales Order";
            treeNode9.Name = "Node2";
            treeNode9.Tag = "payment.htm";
            treeNode9.Text = "Payment";
            treeNode10.Name = "Node3";
            treeNode10.Tag = "delivery.htm";
            treeNode10.Text = "Delivery";
            treeNode11.Name = "Node4";
            treeNode11.Tag = "Returns.htm";
            treeNode11.Text = "Product Returns";
            treeNode12.Name = "Node5";
            treeNode12.Tag = "refund.htm";
            treeNode12.Text = "Refund";
            treeNode13.Name = "Node0";
            treeNode13.Tag = "mainpage.htm";
            treeNode13.Text = "Transaction";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode7,
            treeNode13});
            this.treeView1.Size = new System.Drawing.Size(230, 574);
            this.treeView1.TabIndex = 1;
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Size = new System.Drawing.Size(1111, 574);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 2;
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1111, 574);
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimizeBox = false;
            this.Name = "frmHelp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Help";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmHelp_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}