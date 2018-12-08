using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.User
{
    public partial class fUserEdit : Form
    {
        private int iUserID = 0;
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public fUserEdit()
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

        public int getUserID() {
            return this.iUserID;
        }

        private void LinkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserAccess();
            childForm.ShowDialog();
        }

        private void fUserEdit_Load(object sender, EventArgs e)
        {
            resetData();
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count == 1)
            {
                iUserID = Convert.ToInt32(dgvUser.CurrentRow.Cells[0].Value.ToString());
                txtTitle.Text = dgvUser.CurrentRow.Cells[1].Value.ToString();
                txtFirstName.Text = dgvUser.CurrentRow.Cells[2].Value.ToString();
                txtLastName.Text = dgvUser.CurrentRow.Cells[3].Value.ToString();
                txtPosition.Text = dgvUser.CurrentRow.Cells[4].Value.ToString();
                txtUserName.Text = dgvUser.CurrentRow.Cells[5].Value.ToString();
                cmbStatus.Text = dgvUser.CurrentRow.Cells[6].Value.ToString();
            }
        }

        private void dgvUser_Leave(object sender, EventArgs e)
        {
            if (dgvUser.SelectedRows.Count == 1) 
            {
                List<String> resultList = new List<String>();
                List<String> fieldList = new List<String>();
                AlavaSoft.Class.Database db = new AlavaSoft.Class.Database();

                resultList.Clear();
                fieldList.Clear();

                fieldList.Add("pass");
                resultList = db.getElement("a_users", fieldList, "user_id=" + iUserID);

                if (resultList.Count > 0) {
                    txtConfirm.Text = txtPassword.Text = resultList.ElementAt<String>(0);
                }

            }
        }

        private void dgvUser_Enter(object sender, EventArgs e)
        {
            txtConfirm.Text = txtPassword.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (dgvUser.SelectedRows.Count == 1)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editUser(this) > 0) { reset(); resetData(); }
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

        public void reset() {
            iUserID = 0;
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtTitle.Text = "";
            txtPosition.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirm.Text = "";
            txtMessage.Text = "Fields with * are required.";
        }

        private void resetData() 
        {
            AlavaSoft.Class.Database qa = new AlavaSoft.Class.Database();
            qa.showList(dgvUser, "SELECT user_id AS [User ID], title AS [Title], f_name AS [First Name], l_name AS [Last Name], position as [Position], username AS [User Name], (CASE WHEN status = 1 THEN 'Active' ELSE 'Inactive' END) AS [Status] FROM a_users WHERE sys=0");
            qa.showCombo(cmbStatus, "SELECT LookUpName FROM c_look_up WHERE LookUpDiv='UserStatus'", "LookUpName");
            dgvUser.Select();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (Program.MainLogInPage.myLogInCredential.getUserID() == this.iUserID)
            {
                this.txtMessage.Text = "CANNOT DELETE a currently log-in account. To delete this record, LOG IN with a different administration account.";
                myMessage.reset();
                myMessage.setMessageNumber(9);
                myMessage.showMessage();
            }
            else
            {
                if (dgvUser.SelectedRows.Count == 1)
                {  
                    myMessage.reset();
                    myMessage.setMessageNumber(10);
                    if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
                    {
                        sqlHelper mySqlHelper = new sqlHelper();
                        if (mySqlHelper.deleteUser(this) > 0) { reset(); resetData(); }
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
            }
            Program.ofAlavaSoft.endProgress();
        }

    }
}
