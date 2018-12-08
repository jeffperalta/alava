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
    public partial class fUserAdd : Form
    {
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public fUserAdd()
        {
            InitializeComponent();
        }

        public void setMessageNumber(int value) {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value) {
            myMessage.addParameter(value);
        }

        public void reset() {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtTitle.Text = "";
            txtPosition.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirm.Text = "";
            txtMessage.Text = "Fields with * are required.";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addUser(this) > 0) reset();
            myMessage.showMessage();
            Program.ofAlavaSoft.endProgress();
        }

    }
}
