using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Customer
{
    public partial class fCustomerAdd : Form
    {
        public fCustomerAdd()
        {
            InitializeComponent();
        }

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value) {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value) {
            myMessage.addParameter(value);
        }

        public void reset() {
            txtCustomerName.Text = "";
            txtCompanyAddress.Text = "";
            txtDeliveryAddress.Text = "";
            txtTerms.Text = "";
            txtDiscount.Text = "0";
            txtRemarks.Text = "";
            txtContactTitle.Text = "";
            txtContactName.Text = "";
            txtContactPosition.Text = "";
            txtContact1.Text = "";
            txtContact2.Text = "";
            txtContact3.Text = "";
            txtMessage.Text = "Fields with * are required.";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addCustomer(this) > 0) reset();
            myMessage.showMessage();
            Program.ofAlavaSoft.endProgress();
        }


    }
}
