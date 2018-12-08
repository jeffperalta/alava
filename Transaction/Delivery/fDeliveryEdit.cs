using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Delivery
{
    public partial class fDeliveryEdit : Form
    {
        int iDeliveryID = 0;
        String sSupplierName = "";
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();
        private Boolean isFinal = false; //Used when status is already set to 'Accounted', where changes to the details are no longer allowed.

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public String getSupplierName() 
        {
            return sSupplierName;
        }

        public int getDeliveryID()
        {
            return iDeliveryID;
        }

        public fDeliveryEdit()
        {
            InitializeComponent();
        }

        public void ParameterHeader(String DeliveryID, String DeliveryReceiptNo, String Status, String Remarks, String TransactionDate, String DeliveryDate, String SupplierName) 
        {
            //--Clear Previous Transaction--
            this.reset();

            lblDeliveryID.Text = DeliveryID;
            if (!int.TryParse(DeliveryID, out iDeliveryID)) { iDeliveryID = 0; }

            txtDeliveryReceiptNo.Text = DeliveryReceiptNo;
            //--Determine Value of Status Combo box--
            if (Status.ToLower() == "for request") 
            {
                LinkLabel7.Visible = true;
                LinkLabel8.Visible = true;
                cmbStatus.Enabled = true;
                dgvDelivery.Enabled = true;
                btnDelete.Visible = true;

                cmbStatus.Items.Add("For Request");
                cmbStatus.Items.Add("Confirmed");
                cmbStatus.Items.Add("Accounted");

                isFinal = false;
            }
            else if(Status.ToLower() == "confirmed") 
            {
                LinkLabel7.Visible = true;
                LinkLabel8.Visible = true;
                cmbStatus.Enabled = true;
                dgvDelivery.Enabled = true;
                btnDelete.Visible = true;

                cmbStatus.Items.Add("Confirmed");
                cmbStatus.Items.Add("Accounted");

                isFinal = false;
            }
            else if (Status.ToLower() == "accounted")
            {
                cmbStatus.Items.Clear();

                //--Accounted Delivery Details will no longer be edited--
                cmbStatus.Items.Add("Accounted"); //--Because status is required--
                cmbStatus.Text = "Accounted";

                LinkLabel7.Visible = false;
                LinkLabel8.Visible = false;
                cmbStatus.Enabled = false;
                dgvDelivery.Enabled = false;
                btnDelete.Visible = false;

                isFinal = true;
            }
            else
            {
                cmbStatus.Items.Clear();
                isFinal = true;
            }


            cmbStatus.Text = Status;
            txtRemarks.Text = Remarks;
            dtpTransactionDate.Text = TransactionDate;

            //--Check Delivery Date--
            if (DeliveryDate == "1/1/1900 12:00:00 AM")
            {
                dtpDeliveryDate.Checked = false;
            }
            else
            {
                dtpDeliveryDate.Checked = true;
                dtpDeliveryDate.Text = DeliveryDate;
            }
            
            sSupplierName = lblSupplier.Text = SupplierName;

            //--Feel In Details--
            sqlHelper mySqlHelper = new sqlHelper();
            mySqlHelper.showDetails(this);
        }

        public void ParameterDetail(String ProductID, String Category, String ProductName, int CaseSize, String SupplierName) 
        {
            Boolean blnAllowSave = true;

            if (this.sSupplierName.Trim().Length == 0)
            {
                this.sSupplierName = SupplierName.Trim();
                this.lblSupplier.Text = this.sSupplierName;
            }

            if (this.sSupplierName.Trim() != SupplierName.Trim())
            {
                AlavaSoft.Message.fMessage myMessage = new AlavaSoft.Message.fMessage("Question", "This product belongs to a different SUPPLIER. Would you like to continue and ADD this product?");
                myMessage.ShowDialog();
                if (myMessage.getClicked() != AlavaSoft.Message.fMessage.eButtonClicked.Yes)
                {
                    blnAllowSave = false;
                }
            }

            if (blnAllowSave)
            {
                //--Not Edit Delivery Page--
                //--Add New Product Page--
                fDeliveryDetail myPage = new fDeliveryDetail(ProductID, ProductName, CaseSize.ToString(), Category, true, false);
                myPage.ShowDialog();
            }
        }

        private void LinkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //--Allow search of similar product supplier only--
            AlavaSoft.Search.fProductSearch myPage = new AlavaSoft.Search.fProductSearch(true, this.sSupplierName);
            myPage.ShowDialog();
        }

        private void fDeliveryEdit_Load(object sender, EventArgs e)
        {
            reset();
        }

        private void lblSearchDeliveryID_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fDeliverySearch myPage = new AlavaSoft.Search.fDeliverySearch(true);
            myPage.ShowDialog();
        }

        private void dgvDelivery_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDelivery.SelectedRows.Count ==1) 
            {
                fDeliveryDetail myPage = new fDeliveryDetail(dgvDelivery.CurrentRow.Cells[0].Value.ToString(), 
                                                             dgvDelivery.CurrentRow.Cells[2].Value.ToString(), 
                                                             dgvDelivery.CurrentRow.Cells[3].Value.ToString(), 
                                                             dgvDelivery.CurrentRow.Cells[1].Value.ToString(), 
                                                             true, true);
                myPage.ShowDialog();
            }
        }

        private void LinkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //--Delete Product--
            if (dgvDelivery.Rows.Count > 0 && dgvDelivery.SelectedRows.Count == 1)
            {
                dgvDelivery.CurrentRow.Cells[4].Value = 0;
                dgvDelivery.CurrentRow.Cells[5].Value = 0;
                dgvDelivery.CurrentRow.Visible = false;
            }
        }

        public void reset() 
        {
            sSupplierName = "";
            lblSupplier.Text = "<Supplier>";
            lblDeliveryID.Text = "Delivery ID";
            iDeliveryID = 0;
            cmbStatus.Items.Clear();
            txtRemarks.Clear();
            dtpDeliveryDate.Checked = false;
            txtMessage.Text = "Fields with * are required.";
            txtDeliveryReceiptNo.Text = "";

            LinkLabel7.Visible = false;
            LinkLabel8.Visible = false;
            cmbStatus.Enabled = false;
            dgvDelivery.Enabled = false;
            btnDelete.Visible = false;

            dgvDelivery.Rows.Clear();
            isFinal = false;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            myMessage.reset();
            myMessage.setMessageNumber(10);
            if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.deleteDelivery(this) > 0) this.reset();
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            Boolean blnAllowed = true;

            if (cmbStatus.Text.Trim().ToLower() == "accounted" && !isFinal)
            {
                AlavaSoft.Message.fMessage myPage = new AlavaSoft.Message.fMessage("question", "This will now ADD the products to the inventory. Would you like to continue?");
                myPage.ShowDialog();
                if (myPage.getClicked() != AlavaSoft.Message.fMessage.eButtonClicked.Yes) blnAllowed = false;
            }

            if (blnAllowed)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editDelivery(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0) this.reset();
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }
    }
}
