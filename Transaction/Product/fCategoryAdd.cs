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
    public partial class fCategoryAdd : Form
    {
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public fCategoryAdd()
        {
            InitializeComponent();
        }

        public void setMessageNumber(int i) {
            myMessage.setMessageNumber(i);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public void reset()
        {
            this.txtCategoryName.Text = "";
            this.txtDescription.Text = "";
            txtMessage.Text = "Fields with * are required.";
            txtCategoryName.Focus();
        }

        private void fCategoryAdd_Load(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            sqlHelper mySqlHelper = new sqlHelper();
            if (mySqlHelper.addCategory(this) > 0) { reset(); }
            myMessage.showMessage();
            Program.ofAlavaSoft.endProgress();
        }


    }
}
