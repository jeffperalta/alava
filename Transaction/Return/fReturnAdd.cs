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
    public partial class fReturnAdd : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private SqlDataReader myDataReader = null;

        private String sSalesOrderNo = "";
        private double dReturnAmount = 0.00;
        private int iReturnID = 0;

        public void setReturnID(int ReturnID)
        {
            iReturnID = ReturnID;
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

        public String getSalesOrderNo()
        {
            return sSalesOrderNo;
        }

        public double getReturnAmount()
        {
            return dReturnAmount;
        }

        public fReturnAdd()
        {
            InitializeComponent();
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AlavaSoft.Search.fSalesOrderSearch childForm = new AlavaSoft.Search.fSalesOrderSearch(true);
            childForm.chkForShipment.Checked = false;
            childForm.ShowDialog();
        }

        public void showSODetails(string SONo, string Status)
        {
            dgvSODetails.Rows.Clear();
            sSalesOrderNo = "";
            dReturnAmount = 0.00;

            //--Product ID          0--
            //--Category            1--
            //--Product Name        2--
            //--Case Size           3--
            //--Unit Price          4--
            //--Quantity            5--
            //--Return Quantity     6--
            if (Status.Trim().ToLower() == "for shipment")
            {
                AlavaSoft.Message.fMessage mySimpleMessage = new AlavaSoft.Message.fMessage("Exclamation", "Cannot accept product returns for this SO. Go to Sales Order edit to modify the product details.");
                mySimpleMessage.ShowDialog();
            }
            else
            {
                lblSalesOrderNo.Text = SONo.Trim();
                sSalesOrderNo = SONo.Trim();
                string sQuery = "SELECT  a_product.PID AS [Product Code], a_category.CategoryName AS Category, a_product.ProductName AS [Product Name], " +
                                "        a_sales_order_detail.CaseSize AS [Case Size],  " +
                                "        a_sales_order_detail.UnitPrice - a_sales_order_detail.UnitPrice * a_sales_order_detail.Discount / 100 AS [Unit Price], " +
                                "        a_sales_order_detail.Quantity AS [Ordered(PC)], a_sales_order_detail.ReturnQuantity AS [Returns (PC)]  " +
                                "FROM    a_sales_order_detail INNER JOIN a_product ON a_sales_order_detail.PID = a_product.PID INNER JOIN a_category ON a_product.CategoryID = a_category.CategoryID " +
                                "WHERE 	 a_sales_order_detail.SalesOrderNo=@SalesOrderNo ";

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
        }

        private void fReturnAdd_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbReason, "SELECT LookUpName FROM c_look_up WHERE LookUpDiv='ReturnReason' OR LookUpDiv='Blank'", "LookUpName");
            cmbReason.Text = " ";
        }

        private void dgvSODetails_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            Boolean blnAllowSave = true;
            Boolean blnPrint = false;
            if (cmbStatus.Text.Trim().ToLower() == "accounted")
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
                if (mySqlHelper.addReturns(this, Program.MainLogInPage.myLogInCredential.getUserID()) > 0)
                {
                    myMessage.showMessage();
                    
                    if (blnPrint)
                    {
                        AlavaSoft.Reports.Returns.fProductReturnVoucher myPage = new AlavaSoft.Reports.Returns.fProductReturnVoucher();
                        myPage.setReturnID(iReturnID);
                        myPage.ShowDialog();
                    }

                    disableFields();
                }
                else
                {
                    myMessage.showMessage();
                }
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void disableFields()
        {
            btnSave.Visible = false;
            LinkLabel3.Enabled = false;
            txtRemarks.ReadOnly = true;
            cmbReason.Enabled = false;
            dtpTransactionDate.Enabled = false;
            cmbStatus.Enabled = false;
            txtReturnAmount.ReadOnly = true;
            dgvSODetails.Enabled = false;
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

    }
}
