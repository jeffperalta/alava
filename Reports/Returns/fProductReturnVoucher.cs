using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace AlavaSoft.Reports.Returns
{
    public partial class fProductReturnVoucher : Form
    {
        public fProductReturnVoucher()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fReturnSearch myPage = new AlavaSoft.Search.fReturnSearch(true);
            myPage.ShowDialog();
        }

        public void setReturnID(int ReturnID)
        {
            Program.ofAlavaSoft.startProgress();
            label1.Text = ReturnID.ToString();

            ParameterField Parameter = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "ReturnSlipVoucher.rpt";

            Parameter.Name = "ReturnID";
            ParamValue.Value = ReturnID;
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
