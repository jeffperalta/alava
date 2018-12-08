using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.SalesOrder
{
    public partial class fSalesOrderEdit : Form
    {
        private String SONo = "";

        private String sPreviousStatus = "";

        private double dDiscount = 0.00;

        private int iCustomerID = 0;

        public String getSONumber()
        {
            return SONo;
        }

        public int getCustomerID() 
        {
            return iCustomerID;
        }

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fSalesOrderEdit()
        {
            InitializeComponent();
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSalesOrderSearch(true);
            childForm.ShowDialog();
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

        public void reset()
        {
            sPreviousStatus = "";
            lblSONumber.Text = "SO Number";
            SONo = "";
            dDiscount = 0.00;
            lblCustomerName.Text = "Customer Name";
            iCustomerID = 0;
            txtShippingAddress.Text = "";
            txtTerms.Text = "";
            txtRemarks.Text = "";
            cmbStatus.Items.Clear();
            cmbStatus.Enabled = true; //--Incase paid before--
            dtpRequiredDate.Checked = false;
            dtpShippingDate.Checked = false;
            txtBillingAmount.Text = "0,000.00";
            txtTotalPayment.Text = "0,000.00";
            txtTotalBalance.Text = "0,000.00";
            dgvProduct.Rows.Clear();
            lblTotalReturns.Text = "0,000.00";
            lblTotalRefund.Text = "0,000.00";
            lblTotalAmount.Text = "0,000.00";
            lblDiscount.Text = "0,000.00";
            txtMessage.Text = "Fields with * are required.";

            dgvProduct.Enabled = false;
            btnDelete.Visible = false;
            LinkLabel7.Visible = false;
            LinkLabel8.Visible = false;
        }
        
        public void ParameterHeader(String SalesOrderNo, String CustomerName, String ShippingAddress, int Terms, String Remarks, String TransactionDate, String RequiredDate, String ShippingDate, double BillingAmount, double PaymentAmount, double ReturnAmount, double RefundAmount, String Status)
        {
            this.reset();

            SONo = lblSONumber.Text = SalesOrderNo.Trim();
            lblCustomerName.Text = CustomerName.Trim();

            //--Get Default Discount--
            SqlConnection myConnection = null;
            SqlCommand myCommand = null;

            try
            {
                AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
                myConnection = myDatabase.getConnection();
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "SELECT Discount FROM a_customer WHERE CompanyName=@CompanyName";
                myCommand.Parameters.AddWithValue("@CompanyName", CustomerName.Trim());
                dDiscount = double.Parse(myCommand.ExecuteScalar().ToString());

                myCommand.CommandText = "SELECT CustomerID FROM a_customer WHERE CompanyName=@CompanyName";
                iCustomerID = int.Parse(myCommand.ExecuteScalar().ToString());
            }
            catch (Exception e)
            {
                txtMessage.Text = e.ToString();
            }
            finally
            {
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }

            //--Header Information--
            txtShippingAddress.Text = ShippingAddress.Trim();
            txtTerms.Text = Terms.ToString("d");
            txtRemarks.Text = Remarks.Trim();
            sPreviousStatus = Status;

            //--SetUp Status--
            if (Status.Trim().ToLower() == "for shipment")
            {
                cmbStatus.Items.Add("For Shipment");
                cmbStatus.Items.Add("For Collection");
                cmbStatus.Text = "For Shipment";

                dgvProduct.Enabled = true;
                btnDelete.Visible = true;
                LinkLabel7.Visible = true;
                LinkLabel8.Visible = true;
            }
            else if (Status.Trim().ToLower() == "for collection")
            {
                cmbStatus.Items.Add("For Collection");
                cmbStatus.Items.Add("Paid");
                cmbStatus.Items.Add("Uncollectible");
                cmbStatus.Text = "For Collection";
            }
            else if (Status.Trim().ToLower() == "paid")
            {
                cmbStatus.Items.Add("For Collection");
                cmbStatus.Items.Add("Paid");
                cmbStatus.Items.Add("Uncollectible");
                cmbStatus.Text = "Paid";
                //cmbStatus.Items.Add("Paid");
                //cmbStatus.Text = "Paid";
                //cmbStatus.Enabled = false;
            }
            else if (Status.Trim().ToLower() == "uncollectible")
            {
                cmbStatus.Items.Add("For Collection");
                cmbStatus.Items.Add("Paid");
                cmbStatus.Items.Add("Uncollectible");
                cmbStatus.Text = "Uncollectible";
            }
            else
            {
                cmbStatus.Enabled = false;
            }

            dtpTransactionDate.Text = TransactionDate;

            //--SetUp Required Date--
            if (RequiredDate == "1/1/1900 12:00:00 AM")
            {
                dtpRequiredDate.Checked = false;
            }
            else
            {
                dtpRequiredDate.Checked = true;
                dtpRequiredDate.Text = RequiredDate;
            }

            //--SetUp Shipping Date--
            if (ShippingDate == "1/1/1900 12:00:00 AM")
            {
                dtpShippingDate.Checked = false;
            }
            else
            {
                dtpShippingDate.Checked = true;
                dtpShippingDate.Text = ShippingDate;
            }

            //--Insert Details--
            sqlHelper mySqlHelper = new sqlHelper();
            mySqlHelper.showDetails(this);

            //--Compute Total Amount--
            this.ComputeAmount();

            //--Must be After Compute To Retain the Billing Amount text Field--
            txtBillingAmount.Text = BillingAmount.ToString("n");
            txtTotalPayment.Text = PaymentAmount.ToString("n");
            txtTotalBalance.Text = (BillingAmount - PaymentAmount - ReturnAmount).ToString("n");
            lblTotalReturns.Text = ReturnAmount.ToString("n");
            lblTotalRefund.Text = RefundAmount.ToString("n");
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
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Information", "Product Code: " + ProductID + " is already in the list");
                mySimpleMessage.ShowDialog();
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
            lblDiscount.Text = dDiscount.ToString("n");
            txtBillingAmount.Text = dNetAmount.ToString("n");
            txtTotalBalance.Text = (dNetAmount - double.Parse(txtTotalPayment.Text)).ToString("n");
        }

        private void fSalesOrderEdit_Load(object sender, EventArgs e)
        {

        }

        public void setCustomer(int CustomerID, String CustomerName, String ShippingAddress, double Discount, int Terms)
        {
            iCustomerID = CustomerID;
            lblCustomerName.Text = CustomerName;
            dDiscount = Discount;
            txtShippingAddress.Text = ShippingAddress; 
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

        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        {
            if (dgvProduct.Rows.Count > 0 && dgvProduct.SelectedRows.Count > 0)
            {
                fSalesOrderDetail myPage = new fSalesOrderDetail(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[1].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), int.Parse(dgvProduct.CurrentRow.Cells[3].Value.ToString()), double.Parse(dgvProduct.CurrentRow.Cells[6].Value.ToString()), double.Parse(dgvProduct.CurrentRow.Cells[7].Value.ToString()), false);
                myPage.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            Boolean blnAllowSave = true; Boolean blnToFinalize = false;
            if (sPreviousStatus.Trim().ToLower() == "for shipment" && cmbStatus.Text.Trim().ToLower() != "for shipment")
            {
                blnToFinalize = true;
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Question", "This will finalize the Sales Order, Continue?");
                mySimpleMessage.ShowDialog();
                if (mySimpleMessage.getClicked() == AlavaSoft.Message.fMessage.eButtonClicked.No) blnAllowSave = false;
            }

            if (blnAllowSave)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editSalesOrder(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
                {
                    myMessage.showMessage();
                    if (blnToFinalize)
                    {
                        AlavaSoft.Reports.fSalesOrderVoucher myPage = new AlavaSoft.Reports.fSalesOrderVoucher();
                        myPage.setSalesOrderNo(SONo);
                        myPage.ShowDialog();
                    }
                    this.reset();
                }
                else
                {
                    myMessage.showMessage();
                }
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            Boolean blnAllow = false;

            myMessage.reset();
            myMessage.setMessageNumber(10);
            if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes) { blnAllow = true; }

            if (blnAllow)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.deleteSalesOrder(this) > 0) { this.reset(); }
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

    }
}
