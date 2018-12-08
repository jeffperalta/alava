using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace AlavaSoft.Reports
{
    public partial class fSalesOrderStatusReport : Form
    {
        public fSalesOrderStatusReport()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            ParameterField Parameter = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "SalesOrderStatusReport.rpt";

            Parameter.Name = "SOStatus";
            if (cmbStatus.Text.Trim().Length == 0 || cmbStatus.Text.Trim().ToLower() == "all")
            {
                ParamValue.Value = "%";
            }
            else
            {
                ParamValue.Value = cmbStatus.Text.Trim();
            }
            Parameter.CurrentValues.Add(ParamValue);
            
            Parameters.Add(Parameter);

            crystalReportViewer1.ParameterFieldInfo = Parameters;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ShowPrintButton = true;
            crystalReportViewer1.ShowNextPage();
            Program.ofAlavaSoft.endProgress();
        }

     
    }
}
