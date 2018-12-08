using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Payment
{
    public partial class fPaymentVoid : Form
    {
        private int iPaymentID = 0;

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public void setPaymentID(int value)
        {
            reset();
            iPaymentID = value;
            sqlHelper mySqlHelper = new sqlHelper();
            mySqlHelper.showPayment(this);
        }

        public int getPaymentID()
        {
            return iPaymentID;
        }

        public fPaymentVoid()
        {
            InitializeComponent();
        }

        public void addSalesOrder(String SalesOrder, String Customer, String TransactionDate, double BillingAmount, double Balance, double AmountApplied)
        {
            dgvSalesOrder.Rows.Add(SalesOrder, Customer, TransactionDate, BillingAmount, Balance, AmountApplied);
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReceiptSearch(true);
            childForm.ShowDialog();
        }

        private void reset()
        {
            iPaymentID = 0;
            lblReceiptNo.Text = "<Receipt No>";
            lblPaidBy.Text = "<Paid By>";
            lblTransactionDate.Text = "<Date>";
            lblReceivedBy.Text = "<Received By>";
            dgvSalesOrder.Rows.Clear();
            lblTotalSOAmount.Text = "0,000.00";
            lblTotalBalance.Text = "0,000.00";
            lblTotalApplied.Text = "0,000.00";
            lblTotalPayment.Text = "0,000.00";
            txtMessage.Text = "";
            lblCash.Text = "<Cash Amount>";
            dgvPaymentType.Rows.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            Boolean isAllow = false;
            AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Question", "This will cancel the payment transaction. Continue?");
            mySimpleMessage.ShowDialog();
            if (mySimpleMessage.getClicked() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                isAllow = true;
            }

            if (isAllow)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.voidPayment(this) > 0) { reset(); }
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }
    }
}
