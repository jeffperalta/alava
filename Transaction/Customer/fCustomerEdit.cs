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
    public partial class fCustomerEdit : Form
    {
        public fCustomerEdit()
        {
            InitializeComponent();
        }

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        private int iCustomerID = 0;

        public int getCustomerID()
        {
            return this.iCustomerID;
        }

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public void reset()
        {
            iCustomerID = 0;
            lblCustomerID.Text = "CustomerID";
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

        private void resetData()
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showList(dgvCustomer, "SELECT CustomerID AS [ID], CompanyName AS [Customer Name], CompanyAddress AS [Address], DeliveryAddress AS [Delivery Address], Terms AS [Terms(days)], Discount AS [Discount(%)], ContactTitle AS [Contact Title], ContactName AS [Contact Name], ContactPosition AS [Contact Position], Contact1, Contact2, Contact3, Remarks FROM a_customer");
            dgvCustomer.Select();
        }

        private void fCustomerEdit_Load(object sender, EventArgs e)
        {
            resetData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (dgvCustomer.SelectedRows.Count == 1)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editCustomer(this) > 0) { reset(); resetData(); }
                myMessage.showMessage();
            }
            else
            {
                AlavaSoft.Message.fMessage myfMessage = new AlavaSoft.Message.fMessage("Information", "There is no customer account to edit.");
                myfMessage.ShowDialog();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (dgvCustomer.SelectedRows.Count == 1)
            {
                myMessage.reset();
                myMessage.setMessageNumber(10);
                if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
                {
                    sqlHelper mySqlHelper = new sqlHelper();
                    if (mySqlHelper.deleteCustomer(this) > 0) { reset(); resetData(); }
                    myMessage.showMessage();
                }
            }
            else
            {
                this.txtMessage.Text = "To correct this message, please select a valid record at the data grid.";
                this.myMessage.reset();
                this.myMessage.setMessageNumber(4);
                this.myMessage.addParameter("delete");
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void dgvCustomer_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCustomer.SelectedRows.Count == 1) 
            {
                iCustomerID = int.Parse(dgvCustomer.CurrentRow.Cells[0].Value.ToString());
                lblCustomerID.Text = iCustomerID.ToString();
                txtCustomerName.Text = dgvCustomer.CurrentRow.Cells[1].Value.ToString();
                txtCompanyAddress.Text = dgvCustomer.CurrentRow.Cells[2].Value.ToString();
                txtDeliveryAddress.Text = dgvCustomer.CurrentRow.Cells[3].Value.ToString();
                txtTerms.Text = dgvCustomer.CurrentRow.Cells[4].Value.ToString();
                txtDiscount.Text = dgvCustomer.CurrentRow.Cells[5].Value.ToString();
                txtRemarks.Text = dgvCustomer.CurrentRow.Cells[12].Value.ToString();
                txtContactTitle.Text = dgvCustomer.CurrentRow.Cells[6].Value.ToString();
                txtContactName.Text = dgvCustomer.CurrentRow.Cells[7].Value.ToString();
                txtContactPosition.Text = dgvCustomer.CurrentRow.Cells[8].Value.ToString();
                txtContact1.Text = dgvCustomer.CurrentRow.Cells[9].Value.ToString();
                txtContact2.Text = dgvCustomer.CurrentRow.Cells[10].Value.ToString();
                txtContact3.Text = dgvCustomer.CurrentRow.Cells[11].Value.ToString();
            }
        }

    }
}
