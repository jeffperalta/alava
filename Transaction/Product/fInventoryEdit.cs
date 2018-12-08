using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Product
{
    public partial class fInventoryEdit : Form
    {

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value) {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value) {
            myMessage.addParameter(value);
        }

        public void reset() 
        {
            txtReferenceNo.Text = "";
            txtRemarks.Text = "";
            dgvProduct.Rows.Clear();
        }

        private void resetData()
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbAlterReason, "SELECT LookUpName FROM c_look_up WHERE LookUpDiv='Inventory' UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='Blank'", "LookUpName");
            cmbAlterReason.Text = "Others";
        }

        public fInventoryEdit()
        {
            InitializeComponent();
        }

        public void Parameter(String ProductID, String ProductName, int PC, int CaseSize, String Category)
        {
            fInventoryDetail myPage = new fInventoryDetail(ProductID, ProductName, PC, CaseSize, Category);
            myPage.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch(true);
            childForm.ShowDialog();
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dgvProduct.SelectedRows.Count != 0 && dgvProduct.CurrentRow != null)
            {
                dgvProduct.CurrentRow.Cells[2].Value = 0;
                dgvProduct.CurrentRow.Cells[3].Value = 0;
                dgvProduct.CurrentRow.Cells[4].Value = 0;
                dgvProduct.CurrentRow.Visible = false;
                dgvProduct.Focus();
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

        private void fInventoryEdit_Load(object sender, EventArgs e)
        {
            resetData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            AlavaSoft.Message.fMessage myPage = new AlavaSoft.Message.fMessage("Question", "Would you like to contine and save this changes?");
            myPage.ShowDialog();
            if (myPage.getClicked() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                Boolean hasError = false;

                //--Check Required Field: Inventory Details--
                this.txtMessage.Text = "";
                if (dgvProduct.DisplayedRowCount(false) <= 0)
                {
                    //--Required Field: Details field --
                    this.txtMessage.Text += "Provide a value for DETAILS LIST; \n\r";
                    if (!hasError)
                    {
                        this.setMessageNumber(1);
                        this.addMessageParameter("Details");
                        myMessage.showMessage();
                    }
                    hasError = true;
                }

                //--Check Negative: Inventory Count--
                int iVisibleCtr = 0;
                for (int iCtr = 0; iCtr < dgvProduct.Rows.Count; iCtr++)
                {
                    if (dgvProduct.Rows[iCtr].Visible)
                    {
                        ++iVisibleCtr;
                        if (int.Parse(dgvProduct.Rows[iCtr].Cells[6].Value.ToString()) < 0)
                        {
                            this.txtMessage.Text += "A NEGATIVE inventory is found at item #" + iVisibleCtr + ". CHANGE or DELETE this item to continue; \n\r";
                            if (!hasError)
                            {
                                myPage = new AlavaSoft.Message.fMessage("Exclamation", "Item #" + iVisibleCtr + " is INVALID. It will cause a NEGATIVE Inventory Count.");
                                myPage.ShowDialog();
                            }
                            hasError = true;
                            break;
                        }
                    }
                }

                if (!hasError)
                {
                    sqlHelper mySqlHelper = new sqlHelper();
                    if (mySqlHelper.addInventory(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0) reset();
                    myMessage.showMessage();
                }
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnProductSearch_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch(true);
            childForm.ShowDialog();
        }

    }
}
