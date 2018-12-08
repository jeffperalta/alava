using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace AlavaSoft.Reports.Refund
{
    public partial class fRefundVoucher : Form
    {
        public fRefundVoucher()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fSalesOrderSearch myPage = new AlavaSoft.Search.fSalesOrderSearch(true);
            myPage.ShowDialog();
        }

        public void setSalesOrderNo(String SalesOrderNo)
        {
            Program.ofAlavaSoft.startProgress();
            label1.Text = SalesOrderNo;

            ParameterField Parameter = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "RefundVoucher.rpt";

            Parameter.Name = "SalesOrderNo";
            ParamValue.Value = SalesOrderNo;
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
