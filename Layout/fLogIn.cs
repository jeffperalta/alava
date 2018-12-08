using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using AlavaSoft.Class;

namespace AlavaSoft
{
    public partial class fLogIn : Form
    {
        private Boolean NewUser = true;
        public UserCredential myLogInCredential = null;
        public String ReportLocation = "";
        private AlavaSoft.Transaction.User.sqlHelper mySqlHelper = null;

        public fLogIn()
        {
            InitializeComponent();
            myLogInCredential = new UserCredential();
        }

        public fLogIn(Boolean NewUser) {
            this.NewUser = NewUser;
            InitializeComponent();
            myLogInCredential = new UserCredential();
        }

        private void fLogIn_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.PropertyReader myReader = new AlavaSoft.Class.PropertyReader();
            myReader.ReadFile();
            ReportLocation = myReader.getReportConnection();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            startProgress();
            Program.MainLogInPage.startProgress();
            mySqlHelper = new AlavaSoft.Transaction.User.sqlHelper();
            mySqlHelper.validate(this);

            if (myLogInCredential.isValidUser())
            {
                this.Close();
            }
            else {
                this.txtPassword.Text ="";
            }
            Program.MainLogInPage.endProgress();
            endProgress();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 2;
            }
            else
            {
                progressBar1.Value = 0;
            }
        }

        public void startProgress()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            timer1.Enabled = true;
        }

        public void endProgress()
        {
            progressBar1.Value = 100;
            progressBar1.Visible = false;
            timer1.Enabled = false;
        }

    }
}
