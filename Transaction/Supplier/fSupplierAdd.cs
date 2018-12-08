using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Supplier
{
    public partial class fSupplierAdd : Form
    {
        public fSupplierAdd()
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

        public void reset()
        {
            txtSupplierName.Text = "";
            txtTIN.Text = "";
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtRemarks.Text = "";
            txtContactTitle.Text = "";
            txtContactName.Text = "";
            txtContact1.Text = "";
            txtContact2.Text = "";
            txtContact3.Text = "";
            txtTIN.Text = "";
            txtRemarks.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addSupplier(this) > 0) { this.reset(); }
            myMessage.showMessage();
            Program.ofAlavaSoft.endProgress();
        }

    }
}
