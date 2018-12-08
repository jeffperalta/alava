using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Help
{
    public partial class frmHelp : Form
    {
        private String ReportLocation = "";

        public frmHelp()
        {
            InitializeComponent();
            AlavaSoft.Class.PropertyReader myReader = new AlavaSoft.Class.PropertyReader();
            myReader.ReadFile();
            ReportLocation = myReader.getHelpFile();
        }

        private void frmHelp_Load(object sender, EventArgs e)
        {
            treeView1.ExpandAll();

            Uri myUri = new Uri(ReportLocation + "index.htm");
            this.webBrowser1.Url = myUri;
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Tag.ToString().Trim().Length > 0)
            {
                Uri myUri = new Uri(ReportLocation + e.Node.Tag.ToString().Trim());
                this.webBrowser1.Url = myUri;
            }
        }
           
    }
}
