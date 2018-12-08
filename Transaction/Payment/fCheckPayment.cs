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
    public partial class fCheckPayment : Form
    {
        Boolean addCheck = false;

        public fCheckPayment(Boolean addCheck)
        {
            InitializeComponent();
            this.addCheck = addCheck;
        }

        public void setMaturityDate(String value)
        {
            dtpMaturityDate.Text = value;
        }

        public void setCheckType(String value)
        {
            cmbCheckType.Text = value;
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;
            double dAmount = 0.00;

            #region Instantiation Section
            #endregion

            #region Check Input Section
            //--CheckNo is required--
            if (!hasError && this.txtCheckNo.Text.Trim().Length == 0)
            {
                if (!hasError)
                {
                    setMessageNumber(1);
                    addMessageParameter("Check No");
                }
                hasError = true;
            }

            //--Bank Field is required--
            if (!hasError && this.txtBank.Text.Trim().Length == 0)
            {
                if (!hasError)
                {
                    setMessageNumber(1);
                    addMessageParameter("Bank");
                }
                hasError = true;
            }

            //--CheckType is required--
            if (!hasError && this.cmbCheckType.Text.Trim().Length == 0)
            {
                if (!hasError)
                {
                    setMessageNumber(1);
                    addMessageParameter("Check type");
                }
                hasError = true;
            }

            //--Amount field is required--
            if (!hasError && this.txtAmount.Text.Trim().Length == 0)
            {
                if (!hasError)
                {
                    setMessageNumber(1);
                    addMessageParameter("Amount");
                }
                hasError = true;
            }

            //--Amount must be numeric--
            if (!double.TryParse(txtAmount.Text.Trim(), out dAmount))
            {
                setMessageNumber(3);
                addMessageParameter("Amount");
                addMessageParameter("numeric");
                hasError = true;
            }

            //--Amount must be greater than zero--
            if (dAmount <= 0)
            {
                setMessageNumber(3);
                addMessageParameter("Amount");
                addMessageParameter("a positive number");
                hasError = true;
            }

            #endregion

            #region Process Section
            if (!hasError)
            {
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fPayment")
                {
                    fPayment myPage = (fPayment)Program.ofAlavaSoft.ActiveMdiChild;
                    if (addCheck)
                    {
                        myPage.dgvPaymentType.Rows.Add(txtCheckNo.Text.Trim(), txtBank.Text.Trim(), cmbCheckType.Text.Trim(), dtpMaturityDate.Value.ToString(), dAmount.ToString("n"));
                        myPage.computePayment();
                    }
                    else
                    {
                        myPage.dgvPaymentType.CurrentRow.Cells[0].Value = txtCheckNo.Text.Trim();
                        myPage.dgvPaymentType.CurrentRow.Cells[1].Value = txtBank.Text.Trim();
                        myPage.dgvPaymentType.CurrentRow.Cells[2].Value = cmbCheckType.Text.Trim();
                        myPage.dgvPaymentType.CurrentRow.Cells[3].Value = dtpMaturityDate.Value.ToString();
                        myPage.dgvPaymentType.CurrentRow.Cells[4].Value = dAmount.ToString("n");
                        myPage.computePayment();
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

        private void fCheckPayment_Load(object sender, EventArgs e)
        {

        }
    }
}
