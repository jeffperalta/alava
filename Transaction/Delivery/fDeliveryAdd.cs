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
    public partial class fDeliveryAdd : Form
    {
        String sSupplierName = "";

        private int iDeliveryID = 0;

        public void setDeliveryID(int DeliveryID)
        {
            iDeliveryID = DeliveryID;
        }

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public String getSupplierName()
        {
            return this.sSupplierName;
        }

        public fDeliveryAdd()
        {
            InitializeComponent();
        }

        public void setMessageNumber(int i)
        {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public void reset() 
        {
            txtDeliveryReceiptNo.Text = "";
            cmbStatus.Text = "For Request";
            txtRemarks.Text = "";
            txtMessage.Text = "Fields with * are required.";
            lblSupplier.Text = "<Supplier>";
            sSupplierName = "";
            iDeliveryID = 0;
        }

        private void resetData() 
        {
            dgvDelivery.Rows.Clear();
        }

        private void lnkAddProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //--Allow search of similar product supplier only--
            AlavaSoft.Search.fProductSearch myPage = new AlavaSoft.Search.fProductSearch(true, this.sSupplierName);
            myPage.ShowDialog();
        }

        public void Parameter(String ProductID, String ProductName, String CaseSize, String Category, String SupplierName) 
        {
            Boolean blnAllowSave = true;

            if (this.sSupplierName.Trim().Length == 0) { 
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
                fDeliveryDetail myPage = new fDeliveryDetail(ProductID, ProductName, CaseSize, Category, false, false);
                myPage.ShowDialog();
            }
            
        }

        private void lnkDeleteProduct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (dgvDelivery.SelectedRows.Count != 0)
            {
                if (dgvDelivery.SelectedRows.Count != 0 && dgvDelivery.CurrentRow != null)
                {
                    dgvDelivery.CurrentRow.Cells[4].Value = 0;
                    dgvDelivery.CurrentRow.Cells[5].Value = 0;
                    dgvDelivery.CurrentRow.Visible = false;
                    dgvDelivery.Focus();
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

        }

        private void fDeliveryAdd_Load(object sender, EventArgs e)
        {
            reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addDelivery(this, AlavaSoft.Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
            {
                myMessage.showMessage();

                AlavaSoft.Reports.Delivery.fDeliveryReceipt myPage = new AlavaSoft.Reports.Delivery.fDeliveryReceipt();
                myPage.setDeliveryID(iDeliveryID);
                myPage.ShowDialog();

                this.reset();
                this.resetData();
            }
            else
            {
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void dgvDelivery_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDelivery.SelectedRows.Count > 0) 
            {
                fDeliveryDetail myPage = new fDeliveryDetail(dgvDelivery.CurrentRow.Cells[0].Value.ToString(),
                                                             dgvDelivery.CurrentRow.Cells[2].Value.ToString(),
                                                             dgvDelivery.CurrentRow.Cells[3].Value.ToString(),
                                                             dgvDelivery.CurrentRow.Cells[1].Value.ToString(),
                                                             false, true);
                myPage.ShowDialog();
            }
        }

    }
}
