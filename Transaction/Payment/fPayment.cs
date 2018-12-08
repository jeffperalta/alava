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
    public partial class fPayment : Form
    {
        public fPayment()
        {
            InitializeComponent();
        }

        private int iPaymentID = 0;

        public void setPaymentID(int PaymentID)
        {
            iPaymentID = PaymentID;
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

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSalesOrderSearch(true);
            childForm.ShowDialog();
        }

        public void addSalesOrder(String SONo, String CustomerName, String TransactionDate, double BillingAmount, double Balance, double AmountApplied, String Status)
        {
            if (Status.Trim().ToLower() == "paid")
            {
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Exclamation", "Cannot accept payment. This Sales Order is already PAID.");
                mySimpleMessage.ShowDialog();
            }
            else
            {
                Boolean hasDuplicate = false;
                for (int iCtr = 0; iCtr < dgvSalesOrder.Rows.Count; iCtr++)
                {
                    if (dgvSalesOrder.Rows[iCtr].Visible && dgvSalesOrder.Rows[iCtr].Cells[0].Value.ToString().ToLower() == SONo.ToLower())
                    {
                        hasDuplicate = true;
                        break;
                    }
                }

                if (!hasDuplicate)
                {
                    fSalesOrderPayment mySOPage = new fSalesOrderPayment(SONo, CustomerName, TransactionDate, BillingAmount, Balance, AmountApplied, true);
                    mySOPage.ShowDialog();
                }
                else
                {
                    txtMessage.Text = "Cannot have DUPLICATE entry. SO NO: " + SONo + " is already in the list.";
                    AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Information", "SO NO: " + SONo + " is already in the list");
                    mySimpleMessage.ShowDialog();
                }
            }
        }

        public void computeSalesOrder()
        {
            double dTotalSOAmount = 0.00, dTotalBalance =0.00, dTotalApplied =0.00;
            for (int iCtr = 0; iCtr < dgvSalesOrder.Rows.Count; iCtr++)
            {
                if (dgvSalesOrder.Rows[iCtr].Visible)
                {
                    dTotalSOAmount += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[3].Value.ToString());
                    dTotalBalance += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[4].Value.ToString());
                    dTotalApplied += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString());
                }
            }

            lblTotalSOAmount.Text = dTotalSOAmount.ToString("n");
            lblTotalBalance.Text = dTotalBalance.ToString("n");
            lblTotalApplied.Text = dTotalApplied.ToString("n");
        }

        public void addCheck(String CheckNo, String Bank, String CheckType, String MaturityDate, double Amount)
        {
            dgvPaymentType.Rows.Add(CheckNo, Bank, CheckType, MaturityDate, Amount.ToString("n"));
            computePayment();
        }

        public void computePayment()
        {
            double dTotalPayment = 0.00;
            if (!double.TryParse(txtCash.Text.Trim(), out dTotalPayment)) { dTotalPayment = 0.00; }

            for (int iCtr = 0; iCtr < dgvPaymentType.Rows.Count; iCtr++)
            {
                dTotalPayment += double.Parse(dgvPaymentType.Rows[iCtr].Cells[4].Value.ToString());
            }
            lblTotalPayment.Text = dTotalPayment.ToString("n");
        }

        private void txtCash_Leave(object sender, EventArgs e)
        {
            computePayment();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtCash.Text = "0.00";
            computePayment();
            double dRemaining = double.Parse(lblTotalApplied.Text) - double.Parse(lblTotalPayment.Text);
            if (dRemaining > 0)
            {
                txtCash.Text = dRemaining.ToString("n");
                computePayment();
            }
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fSalesOrderSearch myPage = new AlavaSoft.Search.fSalesOrderSearch(true);
            myPage.chkPaid.Checked = false;
            myPage.ShowDialog();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvSalesOrder.Rows.Count > 0 && dgvSalesOrder.SelectedRows.Count > 0 && dgvSalesOrder.CurrentRow != null)
            {
                dgvSalesOrder.CurrentRow.Cells[5].Value = "0.00";
                dgvSalesOrder.CurrentRow.Visible = false;
                computeSalesOrder();
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

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fCheckPayment(true);
            childForm.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvPaymentType.Rows.Count > 0 && dgvPaymentType.SelectedRows.Count > 0 && dgvPaymentType.CurrentRow != null)
            {
                dgvPaymentType.CurrentRow.Cells[4].Value = "0.00";
                dgvPaymentType.CurrentRow.Visible = false;
                computePayment();
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

        private void dgvSalesOrder_DoubleClick(object sender, EventArgs e)
        {
            if (dgvSalesOrder.Rows.Count > 0 && dgvSalesOrder.SelectedRows.Count > 0 && dgvSalesOrder.CurrentRow != null)
            {
                fSalesOrderPayment myPage = new fSalesOrderPayment(dgvSalesOrder.CurrentRow.Cells[0].Value.ToString(),
                    dgvSalesOrder.CurrentRow.Cells[1].Value.ToString(), dgvSalesOrder.CurrentRow.Cells[2].Value.ToString(),
                    double.Parse(dgvSalesOrder.CurrentRow.Cells[3].Value.ToString()), double.Parse(dgvSalesOrder.CurrentRow.Cells[4].Value.ToString()),
                    double.Parse(dgvSalesOrder.CurrentRow.Cells[5].Value.ToString()), false);
                myPage.ShowDialog();
            }
        }

        private void dgvPaymentType_DoubleClick(object sender, EventArgs e)
        {
            if (dgvPaymentType.Rows.Count > 0 && dgvPaymentType.SelectedRows.Count > 0 && dgvPaymentType.CurrentRow != null)
            {
                fCheckPayment myPage = new fCheckPayment(false);
                myPage.txtCheckNo.Text = dgvPaymentType.CurrentRow.Cells[0].Value.ToString();
                myPage.txtBank.Text = dgvPaymentType.CurrentRow.Cells[1].Value.ToString();
                myPage.setCheckType(dgvPaymentType.CurrentRow.Cells[2].Value.ToString());
                myPage.setMaturityDate(dgvPaymentType.CurrentRow.Cells[3].Value.ToString());
                myPage.txtAmount.Text = double.Parse(dgvPaymentType.CurrentRow.Cells[4].Value.ToString()).ToString("n");
                myPage.ShowDialog();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Question", "Correct payment details?");
            mySimpleMessage.ShowDialog();
            if (mySimpleMessage.getClicked() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.addPayment(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
                {
                    myMessage.showMessage();

                    //--Receipt Printout--
                    AlavaSoft.Reports.Payment.fPaymentReciept myPage = new AlavaSoft.Reports.Payment.fPaymentReciept();
                    myPage.setPaymentID(iPaymentID, txtReceiptNo.Text.Trim());
                    myPage.ShowDialog();

                    reset();
                }
                else
                {
                    myMessage.showMessage();
                }
            }
            Program.ofAlavaSoft.endProgress();
        }

        public void reset()
        {
            txtReceiptNo.Text = "";
            txtPaidBy.Text = "";
            txtReceivedBy.Text = Program.MainLogInPage.myLogInCredential.getFullName();
            dgvSalesOrder.Rows.Clear();
            lblTotalSOAmount.Text = "0,000.00";
            lblTotalBalance.Text = "0,000.00";
            lblTotalApplied.Text = "0,000.00";
            lblTotalPayment.Text = "0,000.00";
            txtMessage.Text = "Fields with * are required.";
            txtCash.Text = "";
            dgvPaymentType.Rows.Clear();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fPayment_Load(object sender, EventArgs e)
        {
            reset();
        }
    }
}
