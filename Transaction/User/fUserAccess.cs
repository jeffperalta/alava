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
    public partial class fUserAccess : Form
    {

        private int iUserId = 0;

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fUserAccess()
        {
            InitializeComponent();
        }

        public int getUserID() {
            return iUserId;
        }

        private void fUserAccess_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.Database qa = new AlavaSoft.Class.Database();
            qa.showList(dgvUser, "SELECT " +        
                                "a_users.user_id AS [User ID], " + 
                                "a_users.title + ' ' + a_users.f_name + ' ' + a_users.l_name AS [Employee Name], " + 
                                "a_users.position AS Position, a_users.username AS [User Name], " + 
                                "(CASE WHEN a_users.status = 1 THEN 'Active' ELSE 'Inactive' END) AS Status, " + 
                                "SUM((CASE WHEN AccessID = 1 AND Active = 1 THEN 1 ELSE 0 END)) AS [User Module], " + 
                                "SUM((CASE WHEN AccessID = 2 AND Active = 1 THEN 1 ELSE 0 END)) AS [Sales Order], " + 
                                "SUM((CASE WHEN AccessID = 3 AND Active = 1 THEN 1 ELSE 0 END)) AS Payment, " + 
                                "SUM((CASE WHEN AccessID = 4 AND Active = 1 THEN 1 ELSE 0 END)) AS [Void Payment], " + 
                                "SUM((CASE WHEN AccessID = 5 AND Active = 1 THEN 1 ELSE 0 END)) AS Product, " + 
                                "SUM((CASE WHEN AccessID = 6 AND Active = 1 THEN 1 ELSE 0 END)) AS Inventory, " + 
                                "SUM((CASE WHEN AccessID = 7 AND Active = 1 THEN 1 ELSE 0 END)) AS Administration, " + 
                                "SUM((CASE WHEN AccessID = 8 AND Active = 1 THEN 1 ELSE 0 END)) AS Refund, " + 
                                "SUM((CASE WHEN AccessID = 9 AND Active = 1 THEN 1 ELSE 0 END)) AS [Returns], " + 
                                "SUM((CASE WHEN AccessID = 10 AND Active = 1 THEN 1 ELSE 0 END)) AS Supplier, " + 
                                "SUM((CASE WHEN AccessID = 11 AND Active = 1 THEN 1 ELSE 0 END)) AS Reports, " + 
                                "SUM((CASE WHEN AccessID = 12 AND Active = 1 THEN 1 ELSE 0 END)) AS Query, " +
                                "SUM((CASE WHEN AccessID = 13 AND Active = 1 THEN 1 ELSE 0 END)) AS Delivery, " +
                                "SUM((CASE WHEN AccessID = 14 AND Active = 1 THEN 1 ELSE 0 END)) AS Customer  " +
                                "FROM a_users LEFT OUTER JOIN a_access_detail ON a_users.user_id = a_access_detail.EmployeeID " + 
                                "WHERE (a_users.sys = 0) " +
                                "GROUP BY a_users.user_id, a_users.title + ' ' + a_users.f_name + ' ' + a_users.l_name, a_users.position, a_users.username, (CASE WHEN a_users.status = 1 THEN 'Active' ELSE 'Inactive' END) ");
                
             dgvUser.Select();
        }

        public void reset() 
        {
            chkUser.Checked = false;
            chkSO.Checked = false;
            chkPayment.Checked = false;
            chkVoidPayment.Checked = false;
            chkProduct.Checked = false;
            chkInventory.Checked = false;
            chkAdministration.Checked = false;
            chkRefund.Checked = false;
            chkReturns.Checked = false;
            chkSupplier.Checked = false;
            chkReports.Checked = false;
            chkQuery.Checked = false;
            chkDelivery.Checked = false;
            chkCustomer.Checked = false;
            txtMessage.Text = "";
            iUserId = 0;
            lblName.Text = "";
        }

        private void resetData() 
        {
            AlavaSoft.Class.Database qa = new AlavaSoft.Class.Database();
            qa.showList(dgvUser, "SELECT " +
                                "a_users.user_id AS [User ID], " +
                                "a_users.title + ' ' + a_users.f_name + ' ' + a_users.l_name AS [Employee Name], " +
                                "a_users.position AS Position, a_users.username AS [User Name], " +
                                "(CASE WHEN a_users.status = 1 THEN 'Active' ELSE 'Inactive' END) AS Status, " +
                                "SUM((CASE WHEN AccessID = 1 AND Active = 1 THEN 1 ELSE 0 END)) AS [User Module], " +
                                "SUM((CASE WHEN AccessID = 2 AND Active = 1 THEN 1 ELSE 0 END)) AS [Sales Order], " +
                                "SUM((CASE WHEN AccessID = 3 AND Active = 1 THEN 1 ELSE 0 END)) AS Payment, " +
                                "SUM((CASE WHEN AccessID = 4 AND Active = 1 THEN 1 ELSE 0 END)) AS [Void Payment], " +
                                "SUM((CASE WHEN AccessID = 5 AND Active = 1 THEN 1 ELSE 0 END)) AS Product, " +
                                "SUM((CASE WHEN AccessID = 6 AND Active = 1 THEN 1 ELSE 0 END)) AS Inventory, " +
                                "SUM((CASE WHEN AccessID = 7 AND Active = 1 THEN 1 ELSE 0 END)) AS Administration, " +
                                "SUM((CASE WHEN AccessID = 8 AND Active = 1 THEN 1 ELSE 0 END)) AS Refund, " +
                                "SUM((CASE WHEN AccessID = 9 AND Active = 1 THEN 1 ELSE 0 END)) AS [Returns], " +
                                "SUM((CASE WHEN AccessID = 10 AND Active = 1 THEN 1 ELSE 0 END)) AS Supplier, " +
                                "SUM((CASE WHEN AccessID = 11 AND Active = 1 THEN 1 ELSE 0 END)) AS Reports, " +
                                "SUM((CASE WHEN AccessID = 12 AND Active = 1 THEN 1 ELSE 0 END)) AS Query, " +
                                "SUM((CASE WHEN AccessID = 13 AND Active = 1 THEN 1 ELSE 0 END)) AS Delivery, " +
                                "SUM((CASE WHEN AccessID = 14 AND Active = 1 THEN 1 ELSE 0 END)) AS Customer  " +  
                                "FROM a_users LEFT OUTER JOIN a_access_detail ON a_users.user_id = a_access_detail.EmployeeID " +
                                "WHERE (a_users.sys = 0) " +
                                "GROUP BY a_users.user_id, a_users.title + ' ' + a_users.f_name + ' ' + a_users.l_name, a_users.position, a_users.username, (CASE WHEN a_users.status = 1 THEN 'Active' ELSE 'Inactive' END) ");

            dgvUser.Select();
        }

        public Boolean getUserAccess() {
            return chkUser.Checked;
        }

        public Boolean getSalesOrderAccess()
        {
            return chkSO.Checked;
        }

        public Boolean getPaymentAccess()
        {
            return chkPayment.Checked;
        }

        public Boolean getVoidPaymentAccess()
        {
            return chkVoidPayment.Checked;
        }

        public Boolean getProductAccess()
        {
            return chkProduct.Checked;
        }

        public Boolean getInventoryAccess()
        {
            return chkInventory.Checked;
        }

        public Boolean getAdministrationAccess()
        {
            return chkAdministration.Checked;
        }

        public Boolean getRefundAccess()
        {
            return chkRefund.Checked;
        }

        public Boolean getReturnsAccess()
        {
            return chkReturns.Checked;
        }

        public Boolean getSupplierAccess()
        {
            return chkSupplier.Checked;
        }

        public Boolean getReportsAccess()
        {
            return chkReports.Checked;
        }

        public Boolean getQueryAccess()
        {
            return chkQuery.Checked;
        }

        public Boolean getDeliveryAccess()
        {
            return chkDelivery.Checked;
        }

        public Boolean getCustomerAccess()
        {
            return chkCustomer.Checked;
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            String strMessage = txtMessage.Text;
            reset();
            txtMessage.Text = strMessage;

            iUserId = Convert.ToInt32(dgvUser.CurrentRow.Cells[0].Value.ToString());
            lblName.Text = dgvUser.CurrentRow.Cells[1].Value.ToString();
            
            if (dgvUser.CurrentRow.Cells[5].Value.ToString() == "1") 
            {
                this.chkUser.Checked = true;    
            }

            if (dgvUser.CurrentRow.Cells[6].Value.ToString() == "1")
            {
                this.chkSO.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[7].Value.ToString() == "1")
            {
                this.chkPayment.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[8].Value.ToString() == "1")
            {
                this.chkVoidPayment.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[9].Value.ToString() == "1")
            {
                this.chkProduct.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[10].Value.ToString() == "1")
            {
                this.chkInventory.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[11].Value.ToString() == "1")
            {
                this.chkAdministration.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[12].Value.ToString() == "1")
            {
                this.chkRefund.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[13].Value.ToString() == "1")
            {
                this.chkReturns.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[14].Value.ToString() == "1")
            {
                this.chkSupplier.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[15].Value.ToString() == "1")
            {
                this.chkReports.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[16].Value.ToString() == "1")
            {
                this.chkQuery.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[17].Value.ToString() == "1")
            {
                this.chkDelivery.Checked = true;
            }

            if (dgvUser.CurrentRow.Cells[18].Value.ToString() == "1")
            {
                this.chkCustomer.Checked = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            if (dgvUser.SelectedRows.Count == 1)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editAccess(this) > 0) { this.reset(); this.resetData(); }
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

        private void dgvUser_Leave(object sender, EventArgs e)
        {
            txtMessage.Text = "";
        }

    }
}
