using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Return
{
    public partial class fReturnEdit : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private SqlDataReader myDataReader = null;
        private int iReturnID = 0;
        private double dReturnAmount = 0.00; //--Original Computation--
        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public double getReturnAmount()
        {
            return dReturnAmount;
        }

        public int getReturnID()
        {
            return iReturnID;
        }

        public fReturnEdit()
        {
            InitializeComponent();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReturnSearch(true);
            childForm.ShowDialog();
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fReturnSearch myPage = new AlavaSoft.Search.fReturnSearch(true);
            myPage.ShowDialog();
        }

        public void ParameterHeader(int iReturnID, String SlipNo, String SalesOrderNo, String Remarks, String Reason, String TransactionDate, String Status, double ReturnAmount)
        {
            dgvSODetails.Rows.Clear();
            dReturnAmount = 0.00;
            this.iReturnID = iReturnID;
            txtReturnSlipNo.Text = SlipNo;
            lblSalesOrderNo.Text = SalesOrderNo;
            txtRemarks.Text = Remarks;
            cmbReason.Text = Reason;
            dtpTransactionDate.Text = TransactionDate;
            cmbStatus.Text = Status;

            if (Status.Trim().ToLower() == "accounted")
            {
                dgvSODetails.Enabled = false;
                cmbStatus.Enabled = false;
                btnDelete.Visible = false;
            }
            else if (Status.Trim().ToLower() == "for request")
            {
                dgvSODetails.Enabled = true;
                cmbStatus.Enabled = true;
                btnDelete.Visible = true;
            }

            showSODetails(SalesOrderNo, iReturnID);

            //--Must be after the showSODetails() to retain the original amount of return--
            txtReturnAmount.Text = ReturnAmount.ToString("n");
        }

        private void showSODetails(String SONo, int ReturnID)
        {
            //--Select ALL SO Details--
            String sQuery = "SELECT a_product.PID, a_category.CategoryName, a_product.ProductName, a_sales_order_detail.CaseSize, " +
                            "       a_sales_order_detail.UnitPrice - a_sales_order_detail.UnitPrice * a_sales_order_detail.Discount / 100 AS [Unit Price], a_sales_order_detail.Quantity, " + 
                            "       a_sales_order_detail.ReturnQuantity " +
                            "FROM   a_product INNER JOIN " +
                            "       a_sales_order_detail ON a_product.PID = a_sales_order_detail.PID INNER JOIN " +
                            "       a_category ON a_product.CategoryID = a_category.CategoryID " +
                            "WHERE  (a_sales_order_detail.SalesOrderNo = @SalesOrderNo) ";
            try
            {
                AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(sQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@SalesOrderNo", SONo.Trim());
                myDataReader = mySqlCommand.ExecuteReader();
                while (myDataReader.Read())
                {
                    dgvSODetails.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(1),
                                          myDataReader.GetString(2), myDataReader.GetInt32(3).ToString("d"),
                                          myDataReader.GetDecimal(4).ToString("n"), myDataReader.GetInt32(5).ToString("d"),
                                          myDataReader.GetInt32(6).ToString("d"), "");
                }
                myDataReader.Close();


                //--Select Number of Products returned for this particular return--
                int iReturnedQuantity = 0, iTotalReturnedQuantity = 0;
                double dTotalAmount = 0.00;

                sQuery = "SELECT a_return_detail.Quantity FROM a_return_detail " +
                         "WHERE a_return_detail.PID=@PID AND a_return_detail.ReturnID=@ReturnID ";
                mySqlCommand.CommandText = sQuery;
                mySqlCommand.Parameters.AddWithValue("@PID", "");
                mySqlCommand.Parameters.AddWithValue("@ReturnID", iReturnID);

                for (int iCtr = 0; iCtr < dgvSODetails.Rows.Count; iCtr++)
                {
                    mySqlCommand.Parameters["@PID"].Value = dgvSODetails.Rows[iCtr].Cells[0].Value.ToString();
                    if (mySqlCommand.ExecuteScalar() != null) //--Products in a SO with no returns will make it null--
                    {
                        iReturnedQuantity = int.Parse(mySqlCommand.ExecuteScalar().ToString());
                        iTotalReturnedQuantity += iReturnedQuantity;
                        dgvSODetails.Rows[iCtr].Cells[6].Value = (int.Parse(dgvSODetails.Rows[iCtr].Cells[6].Value.ToString()) - iReturnedQuantity).ToString("d");
                        dgvSODetails.Rows[iCtr].Cells[7].Value = iReturnedQuantity.ToString("d");
                        dTotalAmount += iReturnedQuantity * double.Parse(dgvSODetails.Rows[iCtr].Cells[4].Value.ToString());
                    }
                }
                this.dReturnAmount = dTotalAmount;
                lblItems.Text = iTotalReturnedQuantity.ToString("d");
                lblTotalReturns.Text = dTotalAmount.ToString("n");
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }
            }
        }

        private void dgvSODetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            Boolean blnAllowSave = true;
            Boolean blnPrint = false;
            if (cmbStatus.Text.Trim().ToLower() == "accounted" && cmbStatus.Enabled) //--There was no change in cmbStatus--
            {
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Question", "This will finalize the returns. All products will now be added to the INVENTORY. Continue?");
                mySimpleMessage.ShowDialog();
                if (mySimpleMessage.getClicked() == AlavaSoft.Message.fMessage.eButtonClicked.No)
                {
                    blnAllowSave = false;
                }
                else
                {
                    blnPrint = true;
                }
            }

            if (blnAllowSave)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.editReturns(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
                {
                    myMessage.showMessage();

                    if (blnPrint)
                    {
                        AlavaSoft.Reports.Returns.fProductReturnVoucher myPage = new AlavaSoft.Reports.Returns.fProductReturnVoucher();
                        myPage.setReturnID(iReturnID);
                        myPage.ShowDialog();
                    }

                    reset();
                }
                else
                {
                    myMessage.showMessage();
                }
            }
            Program.ofAlavaSoft.endProgress();
        }

        public void reset()
        {
            iReturnID = 0;
            dReturnAmount = 0;
            txtReturnSlipNo.Text = "";
            lblSalesOrderNo.Text = "<Sales Order No>";
            txtRemarks.Text = "";
            cmbReason.Text = "";
            cmbStatus.Text = "";
            txtReturnAmount.Text = "";
            dgvSODetails.Rows.Clear();
            lblItems.Text = "0";
            lblTotalReturns.Text = "0,000.00";
            txtMessage.Text = "Fields with * are required.";
            btnDelete.Visible = false;
            cmbStatus.Enabled = true;
        }

        private void fReturnEdit_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbReason, "SELECT LookUpName FROM c_look_up WHERE LookUpDiv='ReturnReason' OR LookUpDiv='Blank'", "LookUpName");
            cmbReason.Text = " ";
        }

        private void dgvSODetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //--Product ID          0--
                //--Category            1--
                //--Product Name        2--
                //--Case Size           3--
                //--Unit Price          4--
                //--Quantity            5--
                //--Return Quantity     6--
                if (dgvSODetails.CurrentCell != null && dgvSODetails.CurrentRow != null)
                {
                    txtMessage.Text = "";
                    //--Check Input: Return amount must be less than the remaining product quantity--
                    int dReturnAmount = 0;

                    //--If Not Numeric value will be zero--
                    if (dgvSODetails.CurrentCell.Value != null)
                    {
                        if (!int.TryParse(dgvSODetails.CurrentCell.Value.ToString(), out dReturnAmount)) { dgvSODetails.CurrentCell.Value = 0; }
                    }

                    //--Incase a negative value is entered: Update--
                    dgvSODetails.CurrentCell.Value = Math.Abs(dReturnAmount);

                    if (Math.Abs(dReturnAmount) > int.Parse(dgvSODetails.CurrentRow.Cells[5].Value.ToString()) - int.Parse(dgvSODetails.CurrentRow.Cells[6].Value.ToString()))
                    {
                        //--Update to Max Product--
                        dgvSODetails.CurrentCell.Value = (int.Parse(dgvSODetails.CurrentRow.Cells[5].Value.ToString()) - int.Parse(dgvSODetails.CurrentRow.Cells[6].Value.ToString())).ToString("d");
                        txtMessage.Text = "Max return for product (" + dgvSODetails.CurrentRow.Cells[0].Value.ToString() + "): " + (int.Parse(dgvSODetails.CurrentRow.Cells[5].Value.ToString()) - int.Parse(dgvSODetails.CurrentRow.Cells[6].Value.ToString())).ToString("d") + " PC";
                    }

                    //--Update total Return amount--
                    double dTotalRefundAmount = 0.00;
                    int iTemp = 0, iItems = 0;


                    for (int iCtr = 0; iCtr < dgvSODetails.Rows.Count; iCtr++)
                    {
                        //--Updated to a positive number only--
                        //--Negative value will be turned to positive--
                        //--Non numeric will be zero--
                        //--More than the SO details will be updated to the MAX number of product--
                        if (!int.TryParse(dgvSODetails.Rows[iCtr].Cells[7].Value.ToString(), out iTemp))
                        {
                            iTemp = 0;
                        }
                        iItems += iTemp;
                        dTotalRefundAmount += double.Parse(dgvSODetails.Rows[iCtr].Cells[4].Value.ToString()) * iTemp;
                    }

                    this.dReturnAmount = dTotalRefundAmount;
                    txtReturnAmount.Text = dTotalRefundAmount.ToString("n");
                    lblTotalReturns.Text = dTotalRefundAmount.ToString("n");
                    lblItems.Text = iItems.ToString("d");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            myMessage.setMessageNumber(10);
            if (myMessage.showMessage() == AlavaSoft.Message.fMessage.eButtonClicked.Yes)
            {
                sqlHelper mySqlHelper = new sqlHelper();
                if (mySqlHelper.deleteReturns(this) > 0) { this.reset(); }
                myMessage.showMessage();
            }
            Program.ofAlavaSoft.endProgress();
        }

    }
}
