using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Refund
{
    public partial class fRefundAdd : Form
    {
        public fRefundAdd()
        {
            InitializeComponent();
        }

        private double dBillingAmount = 0.00;
        private double dPaymentAmount = 0.00;
        private double dPreviousRefund = 0.00;
        private string sSalesOrderNo = "";

        public void setBillingAmount(double value)
        {
            dBillingAmount = value;
        }

        public void setPaymentAmount(double value)
        {
            dPaymentAmount = value;
        }

        public void setPreviousRefund(double value)
        {
            dPreviousRefund = value;
        }

        public void setSalesOrderNo(string value)
        {
            sSalesOrderNo = value;
        }

        public double getBillingAmount()
        {
            return dBillingAmount;
        }

        public double getPaymentAmount()
        {
            return dPaymentAmount;
        }

        public double getPreviousRefund()
        {
            return dPreviousRefund;
        }

        public string getSalesOrderNo()
        {
            return sSalesOrderNo;
        }

        public void reset()
        {
            dBillingAmount = 0.00;
            dPaymentAmount = 0.00;
            dPreviousRefund = 0.00;
            sSalesOrderNo = "";
            lblSalesOrder.Text = "<Sales Order No>";
            txtRefundSlipNo.Text = "";
            txtReceivedBy.Text = "";
            txtRemarks.Text = "";
            txtRefundAmount.Text = "";
            txtAmountReleased.Text = "";
            cmbStatus.Text = "";
            Label8.Text = "Amount Released";
            Label8.ForeColor = Color.Black;
            txtMessage.Text = "Fields with * are required.";
        }

        public void disableFields()
        {
            LinkLabel1.Enabled = false;
            txtReceivedBy.ReadOnly = true;
            txtRemarks.ReadOnly = true;
            dtpTransactionDate.Enabled = false;
            txtRefundAmount.ReadOnly = true;
            txtAmountReleased.ReadOnly = true;
            cmbStatus.Enabled = false;
            btnSave.Visible = false;
            btnSave.Enabled = false;
        }

        public void HeaderParameter(string SalesOrderNo, double BillingAmount, double PaymentAmount, double PreviousRefund, String CompanyName)
        {
            reset();

            sSalesOrderNo = SalesOrderNo;
            dBillingAmount = BillingAmount;
            dPaymentAmount = PaymentAmount;
            dPreviousRefund = PreviousRefund;
            Boolean hasError = false;

            //--Check if there is an excess payment-
            if (dBillingAmount  >= dPaymentAmount - dPreviousRefund)
            {
                if (dPaymentAmount == 0)
                {
                    myMessage.setMessageNumber(17);
                    myMessage.showMessage();
                    hasError = true;
                }

                if (!hasError)
                {
                    myMessage.setMessageNumber(16);
                    if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.No)
                    {
                        hasError = true;
                    }
                }
            }

            if (!hasError)
            {
                lblSalesOrder.Text = sSalesOrderNo;
                txtReceivedBy.Text = CompanyName;
                if (dPaymentAmount - dBillingAmount - dPreviousRefund > 0)
                {
                    txtRefundAmount.Text = (dPaymentAmount - dBillingAmount - dPreviousRefund).ToString("n");
                }
                else
                {
                    txtRefundAmount.Text = "0,000.00";
                }
                cmbStatus.Text = "Unreleased";
                txtAmountReleased.Text = "0,000.00";

                txtMessage.Text = "[SO Billing Amount: " + dBillingAmount.ToString("n") + "] [Total Payment: " + dPaymentAmount.ToString("n") + "] [Previous Refund: " + dPreviousRefund.ToString("n") + "]";
            }
            
        }

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fSalesOrderSearch childForm = new AlavaSoft.Search.fSalesOrderSearch(true);
            childForm.chkExcessPayment.Checked = true;
            childForm.ShowDialog();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.Text.Trim().ToLower() == "released")
            {
                Label8.Text = "*Amount Released";
                Label8.ForeColor = Color.Maroon;
                double dAmountRelease = 0.00;
                if (!double.TryParse(txtRefundAmount.Text.Trim(), out dAmountRelease))
                {
                    txtAmountReleased.Text = "0,000.00";
                }
                else
                {
                    txtAmountReleased.Text = dAmountRelease.ToString("n");
                }
            }
            else
            {
                Label8.Text = "Amount Released";
                Label8.ForeColor = Color.Black;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (txtAmountReleased.Text.Trim().Length == 0) { txtAmountReleased.Text = "0,000.00"; }
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addRefund(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
            {
                myMessage.showMessage();

                AlavaSoft.Reports.Refund.fRefundVoucher myPage = new AlavaSoft.Reports.Refund.fRefundVoucher();
                myPage.setSalesOrderNo(lblSalesOrder.Text.Trim());
                myPage.ShowDialog();

                disableFields();
            }
            else
            {
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

      
    }
}
