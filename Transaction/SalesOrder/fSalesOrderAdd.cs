using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.SalesOrder
{
    public partial class fSalesOrderAdd : Form
    {
        int iCustomerID = 0;
        double dDiscount = 0.00;
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setSONumber(String SONumber)
        {
            lblSONumber.Text = SONumber;
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
            iCustomerID = 0;
            lblCustomerName.Text = "";
        }

        public void setCustomer(int CustomerID, String CustomerName, String ShippingAddress, double Discount, int Terms)
        {
            iCustomerID = CustomerID;
            lblCustomerName.Text = CustomerName;
            dDiscount = Discount;
            txtShippingAddress.Text = ShippingAddress;
            txtTerms.Text = Terms.ToString();
        }

        public int getCustomerID()
        {
            return iCustomerID;
        }

        public fSalesOrderAdd()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fCustomerSearch(true);
            childForm.ShowDialog();
        }

        private void LinkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch(true);
            childForm.ShowDialog();
        }

        public void ParameterDetails(String ProductID, String ProductCategory, String ProductName, int CaseSize, double UnitPrice)
        {
            //--Check for duplicate product code--
            Boolean hasDuplicate = false;
            for (int iCtr = 0; iCtr < dgvProduct.Rows.Count; iCtr++)
            {
                if (dgvProduct.Rows[iCtr].Visible)
                {
                    if (dgvProduct.Rows[iCtr].Cells[0].Value.ToString().ToLower() == ProductID.ToLower())
                    {
                        hasDuplicate = true;
                        break;
                    }
                }
            }

            if (!hasDuplicate)
            {
                fSalesOrderDetail myPage = new fSalesOrderDetail(ProductID, ProductCategory, ProductName, CaseSize, UnitPrice, this.dDiscount, true);
                myPage.ShowDialog();
            }
            else
            {
                txtMessage.Text = "Product: " + ProductID + " is already in the list. Edit or delete this product to change SO details.";
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Information", "Product Code: " + ProductID + " is already in the list");
                mySimpleMessage.ShowDialog();
            }
        }

        private void fSalesOrderAdd_Load(object sender, EventArgs e)
        {

        }

        private void LinkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvProduct.SelectedRows.Count != 0)
            {
                if (dgvProduct.SelectedRows.Count != 0 && dgvProduct.CurrentRow != null)
                {
                    dgvProduct.CurrentRow.Cells[4].Value = 0;
                    dgvProduct.CurrentRow.Cells[5].Value = 0;
                    dgvProduct.CurrentRow.Cells[6].Value = 0;
                    dgvProduct.CurrentRow.Cells[7].Value = 0;
                    dgvProduct.CurrentRow.Cells[8].Value = 0;
                    dgvProduct.CurrentRow.Visible = false;
                    dgvProduct.Focus();

                    this.ComputeAmount();
                }
                else
                {
                    this.txtMessage.Text = "To correct this message, please select a valid record at the data grid.";
                    this.myMessage.reset();
                    this.myMessage.setMessageNumber(4);
                    this.myMessage.addParameter("delete");
                    myMessage.showMessage();
                }
            }
        }

        public void ComputeAmount()
        {
            double dGrossAmount = 0.00, dDiscount = 0.00, dNetAmount = 0.00;
            for (int iCtr = 0; iCtr < dgvProduct.Rows.Count; iCtr++)
            {
                if (dgvProduct.Rows[iCtr].Visible)
                {
                    dGrossAmount += int.Parse(dgvProduct.Rows[iCtr].Cells[4].Value.ToString()) * double.Parse(dgvProduct.Rows[iCtr].Cells[6].Value.ToString());
                    dDiscount += dGrossAmount * (double.Parse(dgvProduct.Rows[iCtr].Cells[7].Value.ToString()) / 100);
                }
            }
            dNetAmount = dGrossAmount - dDiscount;

            lblTotalAmount.Text = dGrossAmount.ToString("n");
            lblTotalDiscount.Text = dDiscount.ToString("n");
            txtBillingAmount.Text = dNetAmount.ToString("n");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            //--Retain the txtBillingAmount field --
            string strAmount = txtBillingAmount.Text.Trim();
            ComputeAmount();
            txtBillingAmount.Text = strAmount;

            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addSalesOrder(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
            {
                LinkLabel1.Enabled = false;
                LinkLabel7.Visible = false;
                LinkLabel8.Visible = false;
                txtShippingAddress.Enabled = false;
                txtTerms.Enabled = false;
                txtRemarks.Enabled = false;
                dtpTransactionDate.Enabled = false;
                dtpRequiredDate.Enabled = false;
                dtpShippingDate.Enabled = false;
                txtBillingAmount.Enabled = false;
                dgvProduct.Enabled = false;
                btnSave.Visible = false;

                myMessage.showMessage();

                AlavaSoft.Reports.fSalesOrderVoucher myPage = new AlavaSoft.Reports.fSalesOrderVoucher();
                myPage.setSalesOrderNo(lblSONumber.Text.Trim());
                myPage.ShowDialog();
            }
            else
            {
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0 && dgvProduct.SelectedRows.Count > 0)
            {
                fSalesOrderDetail myPage = new fSalesOrderDetail(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[1].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), int.Parse(dgvProduct.CurrentRow.Cells[3].Value.ToString()), double.Parse(dgvProduct.CurrentRow.Cells[6].Value.ToString()), double.Parse(dgvProduct.CurrentRow.Cells[7].Value.ToString()), false);
                myPage.ShowDialog();
            }
        }
    }
}
