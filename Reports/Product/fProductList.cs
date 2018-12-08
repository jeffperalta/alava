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
    public partial class fProductList : Form
    {
        public fProductList()
        {
            InitializeComponent();
        }

        private void fProductList_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbCategory, "SELECT CategoryName FROM a_category", "CategoryName");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            ParameterField Parameter = new ParameterField();
            ParameterFields Parameters = new ParameterFields();
            ParameterDiscreteValue ParamValue = new ParameterDiscreteValue();

            crystalReportViewer1.ReportSource = Program.MainLogInPage.ReportLocation + "ProductList.rpt";

            Parameter.Name = "CategoryName";
            if (cmbCategory.Text.Trim().Length == 0)
            {
                ParamValue.Value = "%";
            }
            else
            {
                ParamValue.Value = cmbCategory.Text.Trim();
            }
            
            Parameter.CurrentValues.Add(ParamValue);

            Parameters.Add(Parameter);

            crystalReportViewer1.ParameterFieldInfo = Parameters;
            crystalReportViewer1.Refresh();
            crystalReportViewer1.ShowPrintButton = true;
            crystalReportViewer1.ShowGroupTreeButton = true;
            crystalReportViewer1.ShowNextPage();
            Program.ofAlavaSoft.endProgress();
        }
    }
}
