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
    public partial class fRefundEdit : Form
    {
        int iRefundID = 0;
        double dTotalSOPayment = 0.00, dAllRefundAmount = 0.00, dSOAmount =0.00, dExcess=0.00, dInitialAmount = 0.00;

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fRefundEdit()
        {
            InitializeComponent();
        }

        public void reset()
        {
            iRefundID = 0;
            dTotalSOPayment = 0.00;
            dAllRefundAmount = 0.00;
            dSOAmount = 0.00;
            dExcess = 0.00;
            dInitialAmount = 0.00;
            lblSlipNo.Text = "<Slip No>";
            lblSONo.Text = "<SO No>";
            txtReceivedBy.Text = "";
            txtRemarks.Text = "";
            txtRefundAmount.Text = "";
            txtAmountReleased.Text = "";
            cmbStatus.Text = "";
            txtMessage.Text = "Fields with * are required.";
        }

        public void ParameterHeader(int RefundID, string RefundSlipNo, string SONo, string ReceivedBy, string Remarks, string TransactionDate, double RefundAmount, double AmountReleased, string Status, double SOPayment, double AllRefundAmount, double SOAmount)
        {
            reset();
            iRefundID = RefundID;
            lblSlipNo.Text = RefundSlipNo;
            lblSONo.Text = SONo;
            txtReceivedBy.Text = ReceivedBy;
            txtRemarks.Text = Remarks;
            dtpTransactionDate.Text = TransactionDate;
            dInitialAmount = RefundAmount;
            txtRefundAmount.Text = RefundAmount.ToString("n");
            txtAmountReleased.Text = AmountReleased.ToString("n");
            cmbStatus.Text = Status;
            dTotalSOPayment = SOPayment;
            dAllRefundAmount = AllRefundAmount;
            dSOAmount = SOAmount;
            dExcess = dTotalSOPayment - dSOAmount - dAllRefundAmount;
            if (dExcess < 0) dExcess = 0.00; //--Possible cause: SO amount was changed to a Higher value eventhough there was a refund--

            txtMessage.Text = "[SO Billing Amount: " + dSOAmount.ToString("n") + "] [Total Payment: " + dTotalSOPayment.ToString("n") + "] [Total Refund: " + dAllRefundAmount.ToString("n") + "] [Refundable: " + dExcess.ToString("n") + "]";
            
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fRefundSearch myPage = new AlavaSoft.Search.fRefundSearch(true);
            myPage.cmbStatus.Text = "Unreleased";
            myPage.ShowDialog();
        }

        public double getTotalSOPayment()
        {
            return dTotalSOPayment;
        }

        public double getAllRefundAmount()
        {
            return dAllRefundAmount;
        }

        public double getSOAmount()
        {
            return dSOAmount;
        }

        public double getExcess()
        {
            return dExcess;
        }

        public double getInitialAmount()
        {
            return dInitialAmount;
        }

        public int getRefundID()
        {
            return iRefundID;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (txtAmountReleased.Text.Trim().Length == 0)
            {
                txtAmountReleased.Text = "0,000.00";
            }
            
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.editRefund(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
            {
                myMessage.showMessage();

                AlavaSoft.Reports.Refund.fRefundVoucher myPage = new AlavaSoft.Reports.Refund.fRefundVoucher();
                myPage.setSalesOrderNo(lblSONo.Text.Trim());
                myPage.ShowDialog();

                this.reset();
            }
            else
            {
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            myMessage.setMessageNumber(10);
            if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.deleteRefund(this) > 0) { this.reset(); }
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }
    }
}
