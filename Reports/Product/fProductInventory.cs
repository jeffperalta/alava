using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace AlavaSoft.Reports.Product
{
    public partial class fProductInventory : Form
    {
        public fProductInventory()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fProductSearch myPage = new AlavaSoft.Search.fProductSearch(true);
            myPage.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            ParameterField Parameter = new ParameterField();
            ParameterField Parameter2 = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();
            ParameterDiscreteValue ParamValue2 = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "PeriodicInventoryReport.rpt";

            Parameter.Name = "ProductCode";
            if (txtProductCode.Text.Trim().Length == 0)
            {
                ParamValue.Value = "%";
            }
            else
            {
                ParamValue.Value = txtProductCode.Text.Trim();
            }

            Parameter.CurrentValues.Add(ParamValue);
            Parameters.Add(Parameter);


            Parameter2.Name = "Date1";
            ParamValue2.Value = dtpTransactionDate.Value.Date;
            Parameter2.CurrentValues.Add(ParamValue2);
            Parameters.Add(Parameter2);

            crystalReportViewer1.ParameterFieldInfo = Parameters;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ShowPrintButton = true;
            crystalReportViewer1.ShowGroupTreeButton = true;
            crystalReportViewer1.ShowNextPage();
            Program.ofAlavaSoft.endProgress();
        }
    }
}
