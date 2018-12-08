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
    public partial class fPaymentReciept : Form
    {

        int iPaymentID = 0;

        public void setPaymentID(int PaymentID, String ReceiptNo)
        {
            Program.ofAlavaSoft.startProgress();
            iPaymentID = PaymentID;
            lblReceiptNo.Text = ReceiptNo + " PaymentID: " + PaymentID;

            if (iPaymentID == 0)
            {
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Exclamation", "Receipt No is required.");
                mySimpleMessage.ShowDialog();
            }
            else
            {
                ParameterField Parameter = new ParameterField();
                ParameterFields Parameters = new ParameterFields();
                ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

                crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "PaymentReceipt.rpt";

                Parameter.Name = "PaymentID";
                ParamValue.Value = iPaymentID;
                Parameter.CurrentValues.Add(ParamValue);

                Parameters.Add(Parameter);

                crystalReportViewer1.ParameterFieldInfo = Parameters;
                crystalReportViewer1.Refresh();
                crystalReportViewer1.ShowPrintButton = true;
                crystalReportViewer1.ShowNextPage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        public fPaymentReciept()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fReceiptSearch myPage = new AlavaSoft.Search.fReceiptSearch(true);
            myPage.ShowDialog();
        }

    }
}
