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
    public partial class fSupplierEdit : Form
    {
        private int iSupplierID = 0;

        public fSupplierEdit()
        {
            InitializeComponent();
        }

        public int getSupplierID()
        {
            return iSupplierID;
        }

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

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
            txtRemarks.Text = "";
            chkDiscontinue.Checked = false;
            lblSupplierID.Text = "Supplier ID";
        }

        private void resetData()
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showList(dgvSupplier, "SELECT SupplierID AS [Supplier ID], CompanyName AS [SupplierName], ContactTitle AS [Contact Title], ContactName AS [Contact Person], Address1, Address2, Address3, Contact1, Contact2, Contact3, TINNo, Description, (CASE WHEN Discontinue = 1 THEN 'Discontinue' ELSE 'Active' END) AS [Status] FROM a_supplier ");
            dgvSupplier.Select();
        }

        private void fSupplierEdit_Load(object sender, EventArgs e)
        {
            resetData();
        }

        private void dgvSupplier_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSupplier.SelectedRows.Count == 1) 
            {
                iSupplierID = Convert.ToInt32(dgvSupplier.CurrentRow.Cells[0].Value.ToString());
                lblSupplierID.Text = iSupplierID.ToString();
                this.txtSupplierName.Text = dgvSupplier.CurrentRow.Cells[1].Value.ToString();
                this.txtContactTitle.Text = dgvSupplier.CurrentRow.Cells[2].Value.ToString();
                this.txtContactName.Text = dgvSupplier.CurrentRow.Cells[3].Value.ToString();
                this.txtAddress1.Text = dgvSupplier.CurrentRow.Cells[4].Value.ToString();
                this.txtAddress2.Text = dgvSupplier.CurrentRow.Cells[5].Value.ToString();
                this.txtAddress3.Text = dgvSupplier.CurrentRow.Cells[6].Value.ToString();
                this.txtContact1.Text = dgvSupplier.CurrentRow.Cells[7].Value.ToString();
                this.txtContact2.Text = dgvSupplier.CurrentRow.Cells[8].Value.ToString();
                this.txtContact3.Text = dgvSupplier.CurrentRow.Cells[9].Value.ToString();
                this.txtTIN.Text = dgvSupplier.CurrentRow.Cells[10].Value.ToString();
                this.txtRemarks.Text = dgvSupplier.CurrentRow.Cells[11].Value.ToString();
                if (dgvSupplier.CurrentRow.Cells[12].Value.ToString().ToLower() == "discontinue") chkDiscontinue.Checked = true;
                else chkDiscontinue.Checked = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (dgvSupplier.SelectedRows.Count == 1)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editSupplier(this) > 0) { reset(); resetData(); }
                myMessage.showMessage();
            }
            else
            {
                this.txtMessage.Text = "To correct this message, please select a valid record at the data grid.";
                this.myMessage.reset();
                this.myMessage.setMessageNumber(4);
                this.myMessage.addParameter("edit");
                this.myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (dgvSupplier.SelectedRows.Count == 1)
            {
                myMessage.reset();
                myMessage.setMessageNumber(10);
                if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
                {
                    sqlHelper mySqlHelper = new sqlHelper();
                    if (mySqlHelper.deleteSupplier(this) > 0) { reset(); resetData(); }
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

    }
}
