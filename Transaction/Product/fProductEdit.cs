using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Product
{
    public partial class fProductEdit : Form
    {
        private String ProductID = "";
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fProductEdit()
        {
            InitializeComponent();
        }

        private void resetData()
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbCategory, "SELECT CategoryName FROM a_category", "CategoryName");
            myDatabase.showCombo(cmbSupplier, "SELECT CompanyName FROM a_supplier", "CompanyName");
        }

        public void setProductID(String value) 
        {
            ProductID = value;
            lblProductID.Text = ProductID;
            sqlHelper mySqlHelper = new sqlHelper();
            mySqlHelper.getProductInfo(ProductID, this);
        }

        public String getProductID()
        {
            return ProductID;
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch(true);
            childForm.ShowDialog();
        }

        private void txtBulkPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void fProductEdit_Load(object sender, EventArgs e)
        {
            resetData();

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Inventory"))
            {
                lnkUnitsStock.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("SalesOrder"))
            {
                lnkUnitSalesOrder.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Delivery"))
            {
                lnkUnitDelivery.Visible = false;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (ProductID.Length != 0)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editProduct(this) > 0) { reset(); }
                myMessage.showMessage();
            }
            else
            {
                AlavaSoft.Message.fMessage myfMessage = new AlavaSoft.Message.fMessage("Information", "Search the product that you want to edit.");
                myfMessage.ShowDialog();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            myMessage.reset();
            myMessage.setMessageNumber(10);
            if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                if (mySqlHelper.deleteProduct(this) > 0) { reset();}
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        public void reset()
        {
            ProductID = "";
            lblProductID.Text = "Product ID";
            txtProductName.Text = "";
            txtDescription.Text = "";
            txtReorderLevel.Text = "";
            chkDiscontinue.Checked = false;
            txtUnitsStock.Text = "";
            txtUnitsOrder.Text = "";
            txtUnitsDelivery.Text = "";
            txtCaseSize.Text = "";
            txtUnitPrice.Text = "";
            txtBulkPrice.Text = "";
        }

        private void lnkUnitsStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            fInventoryEdit myPage = new fInventoryEdit();
            myPage.MdiParent = Program.ofAlavaSoft;
            myPage.Show();
        }

        private void lnkUnitSalesOrder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd myPage = new AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd();
            myPage.MdiParent = Program.ofAlavaSoft;
            myPage.Show();
        }

        private void lnkUnitDelivery_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Transaction.Delivery.fDeliveryAdd myPage = new AlavaSoft.Transaction.Delivery.fDeliveryAdd();
            myPage.MdiParent = Program.ofAlavaSoft;
            myPage.Show();
        }

    }
}
