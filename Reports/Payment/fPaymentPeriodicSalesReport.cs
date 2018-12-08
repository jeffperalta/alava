using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace AlavaSoft.Reports.Payment
{
    public partial class fPaymentPeriodicSalesReport : Form
    {

        public fPaymentPeriodicSalesReport()
        {
            InitializeComponent();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            ParameterField ParameterDate1 = new ParameterField();
            ParameterField ParameterDate2 = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValueDate1 = new ParameterDiscreteValue();
            ParameterDiscreteValue ParamValueDate2 = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "PaymentPeriodicSalesReport.rpt";

            ParameterDate1.Name = "Date1";
            ParamValueDate1.Value = dtpTS.Value.Date;
            ParameterDate1.CurrentValues.Add(ParamValueDate1);
            Parameters.Add(ParameterDate1);

            ParameterDate2.Name = "Date2";
            ParamValueDate2.Value = dtpTE.Value.Date;
            ParameterDate2.CurrentValues.Add(ParamValueDate2);
            Parameters.Add(ParameterDate2);

            crystalReportViewer1.ParameterFieldInfo = Parameters;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ShowPrintButton = true;
            crystalReportViewer1.ShowGroupTreeButton = true;
            crystalReportViewer1.ShowNextPage();
            Program.ofAlavaSoft.endProgress();
        }


    }
}
