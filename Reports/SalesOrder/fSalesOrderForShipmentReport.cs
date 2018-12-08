using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Reports.SalesOrder
{
    public partial class fSalesOrderForShipmentReport : Form
    {
        public fSalesOrderForShipmentReport()
        {
            InitializeComponent();
        }

        private void fSalesOrderForShipmentReport_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "SalesOrderForShipmentReport.rpt";
            crystalReportViewer1.RefreshReport();
        }
    }
}
