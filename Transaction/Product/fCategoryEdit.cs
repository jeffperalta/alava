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
    public partial class fCategoryEdit : Form
    {
        private int iCategoryID = 0;
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public fCategoryEdit()
        {
            InitializeComponent();
        }

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public int getCategoryID() {
            return this.iCategoryID;
        }

        public void reset() {
            txtCategoryName.Text = "";
            lblCategoryID.Text = "<Category ID>";
            txtDescription.Text = "";
            txtMessage.Text = "Fields with * are required.";
            iCategoryID = 0;
            txtCategoryName.Focus();
        }

        private void resetData() {
            AlavaSoft.Class.Database qa = new AlavaSoft.Class.Database();
            qa.showList(this.dgvProductCategory, "SELECT CategoryID AS [Category ID],  CategoryName as [Category Name], Description AS [Description] FROM a_category");
            dgvProductCategory.Select();
        }

        private void fCategoryEdit_Load(object sender, EventArgs e)
        {
            this.reset();
            this.resetData();
        }

        private void dgvProductCategory_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProductCategory.SelectedRows.Count == 1)
            {
                iCategoryID = Convert.ToInt32(this.dgvProductCategory.CurrentRow.Cells[0].Value.ToString());
                lblCategoryID.Text = iCategoryID.ToString();
                txtCategoryName.Text = dgvProductCategory.CurrentRow.Cells[1].Value.ToString();
                txtDescription.Text = dgvProductCategory.CurrentRow.Cells[2].Value.ToString();
            }
            else 
            {
                reset();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();

            if (dgvProductCategory.SelectedRows.Count == 0)
            {
                this.txtMessage.Text = "To correct this message, please select a valid record at the data grid.";
                this.myMessage.reset();
                this.myMessage.setMessageNumber(4);
                this.myMessage.addParameter("edit");
                this.myMessage.showMessage();
            }
            else 
            { 
                if (mySqlHelper.editCategory(this) > 0) {reset(); resetData();}
                this.myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();

            if (dgvProductCategory.SelectedRows.Count > 0) 
            {
                myMessage.reset();
                myMessage.setMessageNumber(10);
                if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
                {
                    if (mySqlHelper.deleteCategory(this) > 0) { reset(); resetData(); }
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
