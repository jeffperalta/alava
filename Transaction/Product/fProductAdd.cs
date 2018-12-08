using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Product
{
    public partial class fProductAdd : Form
    {
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public fProductAdd()
        {
            InitializeComponent();
        }

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public void reset() 
        {
            txtBulkPrice.Text = "";
            txtCaseSize.Text = "";
            txtDescription.Text = "";
            txtMessage.Text = "Fields with * are required.";
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtReorderLevel.Text = "";
            txtUnitPrice.Text = "";
            cmbCategory.Text = "";
            cmbSupplier.Text = "";
        }

        private void resetData() 
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbCategory, "SELECT CategoryName FROM a_category", "CategoryName");
            myDatabase.showCombo(cmbSupplier, "SELECT CompanyName FROM a_supplier", "CompanyName");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addProduct(this) > 0) { reset(); }
            myMessage.showMessage();
            Program.ofAlavaSoft.endProgress();
        }

        private void fProductAdd_Load(object sender, EventArgs e)
        {
            resetData();

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Inventory"))
            {
                LinkLabel2.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("SalesOrder"))
            {
                LinkLabel3.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Delivery"))
            {
                LinkLabel4.Visible = false;
            }

        }

        private void btnCompute_Click(object sender, EventArgs e)
        {
            int iCaseSize = 0;
            if (!int.TryParse(txtCaseSize.Text.Trim(), out iCaseSize)) iCaseSize = 1;

            double dUnitPrice = 0.00;
            if (!double.TryParse(txtUnitPrice.Text.Trim(), out dUnitPrice)) dUnitPrice = 0.00;

            txtBulkPrice.Text = (dUnitPrice * iCaseSize).ToString();
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fInventoryEdit myPage = new fInventoryEdit();
            myPage.MdiParent = Program.ofAlavaSoft;
            myPage.Show();
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd myPage = new AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd();
            myPage.MdiParent = Program.ofAlavaSoft;
            myPage.Show();
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Transaction.Delivery.fDeliveryAdd myPage = new AlavaSoft.Transaction.Delivery.fDeliveryAdd();
            myPage.MdiParent = Program.ofAlavaSoft;
            myPage.Show();
        }
    }
}
