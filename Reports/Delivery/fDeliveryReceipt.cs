using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.Shared;

namespace AlavaSoft.Reports.Delivery
{
    public partial class fDeliveryReceipt : Form
    {

        public fDeliveryReceipt()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fDeliverySearch myPage = new AlavaSoft.Search.fDeliverySearch(true);
            myPage.ShowDialog();
        }

        public void setDeliveryID(int DeliveryID)
        {
            Program.ofAlavaSoft.startProgress();
            lblDeliveryID.Text = DeliveryID.ToString();
            ParameterField Parameter = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "DeliveryVoucher.rpt";

            Parameter.Name = "DeliveryID";
            ParamValue.Value = DeliveryID;
            Parameter.CurrentValues.Add(ParamValue);

            Parameters.Add(Parameter);

            crystalReportViewer1.ParameterFieldInfo = Parameters;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ShowPrintButton = true;
            crystalReportViewer1.ShowNextPage();
            Program.ofAlavaSoft.endProgress();
        }

        private void fDeliveryReceipt_Load(object sender, EventArgs e)
        {
            
        }
    }
}
