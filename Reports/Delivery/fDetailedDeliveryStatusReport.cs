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
    public partial class fDetailedDeliveryStatusReport : Form
    {
        public fDetailedDeliveryStatusReport()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            ParameterField Parameter = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

            ParameterField Parameter2 = new ParameterField();
            ParameterDiscreteValue ParamValue2 = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "DeliveryStatusReport.rpt";

            Parameter.Name = "status";
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


            Parameter2.Name = "SupplierName";
            if (cmbSupplier.Text.Trim().Length == 0 || cmbSupplier.Text.Trim().ToLower() == "all")
            {
                ParamValue2.Value = "%";
            }
            else
            {
                ParamValue2.Value = cmbSupplier.Text.Trim();
            }
            Parameter2.CurrentValues.Add(ParamValue2);
            Parameters.Add(Parameter2);

            crystalReportViewer1.ParameterFieldInfo = Parameters;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ShowPrintButton = true;
            crystalReportViewer1.ShowGroupTreeButton = true;
            crystalReportViewer1.ShowNextPage();
            Program.ofAlavaSoft.endProgress();
        }

        private void fDetailedDeliveryStatusReport_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbSupplier, "SELECT CompanyName FROM a_supplier UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='BLANK'", "CompanyName");
            cmbSupplier.Text = " ";
        }
    }
}
