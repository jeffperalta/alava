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
    public partial class fSalesOrderPayment : Form
    {
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();
        Boolean blnAddSO = false;

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fSalesOrderPayment(String SONo, String CustomerName, String TransactionDate, double BillingAmount, double Balance, double AmountApplied, Boolean addSO)
        {
            //--Billing amount is less the returns--
            InitializeComponent();
            txtSalesOrderNo.Text = SONo;
            txtCustomer.Text = CustomerName;
            txtTransactionDate.Text = TransactionDate;
            txtBillingAmount.Text = BillingAmount.ToString("n");
            txtBalance.Text = Balance.ToString("n");
            txtAmountApplied.Text = AmountApplied.ToString("n");
            blnAddSO = addSO;
        }

        private void fSalesOrderPayment_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            double dBalance = 0.00;
            double dAmountApplied = 0.00;
            Boolean hasError = false;
           
            #region Instantiation Section
            dBalance = double.Parse(txtBalance.Text.Trim());
            #endregion

            #region Check Input Section
            if (!double.TryParse(txtAmountApplied.Text.Trim(), out dAmountApplied))
            {
                if (!hasError)
                {
                    myMessage.setMessageNumber(3);
                    myMessage.addParameter("Amount applied");
                    myMessage.addParameter("numeric");
                }
                hasError = true;
            }

            if (dAmountApplied <= 0) 
            {
                if (!hasError)
                {
                    myMessage.setMessageNumber(3);
                    myMessage.addParameter("Amount applied");
                    myMessage.addParameter("a positive number");
                }
                hasError = true;
            }

            if (dAmountApplied > dBalance)
            {
                if (!hasError)
                {
                    AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Question", "The AMOUNT APPLIED exceeds the SO balance, would you allow this?");
                    mySimpleMessage.ShowDialog();
                    if (mySimpleMessage.getClicked() == AlavaSoft.Message.fMessage.eButtonClicked.No)
                    {
                        hasError = true;
                    }
                }
            }
            #endregion

            #region Process Section
            if (!hasError)
            {
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fPayment")
                {
                    if (blnAddSO)
                    {
                        fPayment myPage = (fPayment)Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.dgvSalesOrder.Rows.Add(txtSalesOrderNo.Text, txtCustomer.Text, txtTransactionDate.Text, txtBillingAmount.Text, txtBalance.Text, dAmountApplied.ToString("n"));
                        myPage.computeSalesOrder();
                        if (myPage.txtPaidBy.Text.Trim().Length == 0){ myPage.txtPaidBy.Text = txtCustomer.Text; }
                    }
                    else
                    {
                        fPayment myPage = (fPayment)Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.dgvSalesOrder.CurrentRow.Cells[5].Value = dAmountApplied.ToString("n");
                        myPage.computeSalesOrder();
                    }
                }
                this.Close();
            }
            else
            {
                myMessage.showMessage();
            }
            #endregion

        }
    }
}
