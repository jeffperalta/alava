using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Reports.Product
{
    public partial class fForReplenishmentReport : Form
    {
        public fForReplenishmentReport()
        {
            InitializeComponent();
        }

        private void fForReplenishmentReport_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "ProductForReorder.rpt";
            crystalReportViewer1.RefreshReport();
        }

    }
}
