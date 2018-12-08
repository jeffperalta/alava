using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.SalesOrder
{
    public partial class fSalesOrderDetail : Form
    {
        private int iPC = 0, iCS = 0, iTotalItem = 0;
        private double dUnitPrice = 0.00, dDiscount = 0.00;
        private double dPrice = 0.00;
        Boolean isNumeric = true;
        private Boolean isAddDetails = false;

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fSalesOrderDetail(String ProductID, String ProductCategory, String ProductName, int CaseSize, double UnitPrice, double Discount, Boolean isAddDetails)
        {
            InitializeComponent();
            txtProductID.Text = ProductID;
            txtCategory.Text = ProductCategory;
            txtProductName.Text = ProductName;
            txtCaseSize.Text = CaseSize.ToString();
            txtUnitPrice.Text = UnitPrice.ToString();
            txtDiscount.Text = Discount.ToString();
            this.isAddDetails = isAddDetails;
        }

        private void fSalesOrderDetail_Load(object sender, EventArgs e)
        {
            computeAmount();
        }

        private void txtPC_Leave(object sender, EventArgs e)
        {
            computeAmount();
        }

        private void txtCS_Leave(object sender, EventArgs e)
        {
            computeAmount();
        }

        private void txtUnitPrice_Leave(object sender, EventArgs e)
        {
            computeAmount();
        }

        private void txtDiscount_Leave(object sender, EventArgs e)
        {
            computeAmount();
        }

        private void computeAmount()
        {
            iPC = 0; iCS = 0; dUnitPrice = 0; dDiscount = 0; dPrice = 0; isNumeric = true;

            txtMessage.Text ="";
            if (txtPC.Text.Trim().Length > 0 && !int.TryParse(txtPC.Text.Trim(), out iPC)) { iPC = 0; txtMessage.Text += "Quantity(PC) must be numeric; "; isNumeric = false; }
            if (txtCS.Text.Trim().Length > 0 && !int.TryParse(txtCS.Text.Trim(), out iCS)) { iCS = 0; txtMessage.Text += "Quantity(CS) must be numeric; "; isNumeric = false; }
            iTotalItem = iPC + (iCS * int.Parse(txtCaseSize.Text.Trim()));

            if (txtUnitPrice.Text.Trim().Length > 0 && !double.TryParse(txtUnitPrice.Text.Trim(), out dUnitPrice)) { dUnitPrice = 0.00; txtMessage.Text += "Unit Price must be numeric; "; isNumeric = false; }
            if (txtDiscount.Text.Trim().Length > 0 && !double.TryParse(txtDiscount.Text.Trim(), out dDiscount)) { dDiscount = 0.00; txtMessage.Text += "Discount must be numeric; "; isNumeric = false; }

            dPrice = (iTotalItem * dUnitPrice) - ((iTotalItem * dUnitPrice) * (dDiscount / 100));
            lblTotalPrice.Text = dPrice.ToString("n");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            #region Instantiation Section
            computeAmount();

            #endregion

            #region Check Input Section
            this.txtMessage.Text = "";

            //--Entry must be numeric--
            if (!isNumeric)
            {
                this.txtMessage.Text += "Please check all fields, entries must be numeric; \n\r";
                if (!hasError)
                {
                    AlavaSoft.Message.fMessage mySimpleError = new AlavaSoft.Message.fMessage("Exclamation", "All entries must be numeric.");
                    mySimpleError.ShowDialog();
                }
                hasError = true;
            }

            //--Quantity field is required--
            if (iPC == 0 && iCS == 0)
            {
                this.txtMessage.Text += "Provide a value for QUANTITY field; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(1);
                    this.addMessageParameter("QUANTITY");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            //--Quantity(PC) must be a positive number--
            if (iPC < 0)
            {
                this.txtMessage.Text += "A positive numeric value is required for the QUANTITY(PC); \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("QUANTITY(PC)");
                    this.addMessageParameter("a positive number");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            //--Quantity(CS) must be a positive number--
            if (iCS < 0)
            {
                this.txtMessage.Text += "A positive numeric value is required for the QUANTITY(CS); \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("QUANTITY(CS)");
                    this.addMessageParameter("a positive number");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            //--Unit Price field is required--
            if (dUnitPrice == 0)
            {
                this.txtMessage.Text += "Provide a value for UNIT PRICE field; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(1);
                    this.addMessageParameter("UNIT PRICE");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            //--Unit Price must be a positive number--
            if (dUnitPrice < 0)
            {
                this.txtMessage.Text += "A positive numeric value is required for the UNIT PRICE; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("UNIT PRICE");
                    this.addMessageParameter("a positive number");
                    myMessage.showMessage();
                }
                hasError = true;
            }


            //--Discount must be a positive number--
            if (dDiscount < 0)
            {
                this.txtMessage.Text += "A positive numeric value is required for the DISCOUNT; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("DISCOUNT");
                    this.addMessageParameter("a positive number");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            //--Total Price must be positive--
            if (dPrice < 0)
            {
                this.txtMessage.Text += "A positive numeric value is required for the TOTAL PRICE; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("DISCOUNT");
                    this.addMessageParameter("a positive number");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            #endregion

            #region Process Section
            if (!hasError)
            {
               
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderAdd")
                {
                    fSalesOrderAdd myPage = (fSalesOrderAdd)Program.ofAlavaSoft.ActiveMdiChild;
                    if (isAddDetails)
                    {
                        //--Duplicate entry is filtered before fSalesOrderDetail is called--
                        myPage.dgvProduct.Rows.Add(txtProductID.Text, txtCategory.Text, txtProductName.Text, txtCaseSize.Text, iTotalItem, iTotalItem / int.Parse(txtCaseSize.Text.Trim()), dUnitPrice, dDiscount, dPrice);
                        myPage.ComputeAmount();
                    }
                    else //--Edit Existing Details--
                    {
                        myPage.dgvProduct.CurrentRow.Cells[4].Value = iTotalItem;
                        myPage.dgvProduct.CurrentRow.Cells[5].Value = iTotalItem / int.Parse(txtCaseSize.Text.Trim());
                        myPage.dgvProduct.CurrentRow.Cells[6].Value = dUnitPrice; 
                        myPage.dgvProduct.CurrentRow.Cells[7].Value = dDiscount;
                        myPage.dgvProduct.CurrentRow.Cells[8].Value = dPrice;
                        myPage.ComputeAmount();
                    }
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderEdit")
                {
                    fSalesOrderEdit myPage = (fSalesOrderEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    if (isAddDetails)
                    {
                        //--Duplicate entry is filtered before fSalesOrderDetail is called--
                        myPage.dgvProduct.Rows.Add(txtProductID.Text, txtCategory.Text, txtProductName.Text, txtCaseSize.Text, iTotalItem, iTotalItem / int.Parse(txtCaseSize.Text.Trim()), dUnitPrice, dDiscount, dPrice);
                        myPage.ComputeAmount();
                    }
                    else //--Edit Existing Details--
                    {
                        myPage.dgvProduct.CurrentRow.Cells[4].Value = iTotalItem;
                        myPage.dgvProduct.CurrentRow.Cells[5].Value = iTotalItem / int.Parse(txtCaseSize.Text.Trim());
                        myPage.dgvProduct.CurrentRow.Cells[6].Value = dUnitPrice;
                        myPage.dgvProduct.CurrentRow.Cells[7].Value = dDiscount;
                        myPage.dgvProduct.CurrentRow.Cells[8].Value = dPrice;
                        myPage.ComputeAmount();
                    }

                    //--Change Color if quantity is greater than the unit in stock--
                    //--Interface disallows add of product if SO status is already 'for collection', 'paid', and 'uncollectible'--
                    AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
                    SqlConnection myConnection = myDatabase.getConnection();
                    SqlCommand myCommand = myConnection.CreateCommand();
                    myCommand.CommandText = "SELECT UnitInStock FROM a_product WHERE PID=@PID";
                    myCommand.Parameters.AddWithValue("@PID", this.txtProductID.Text.Trim());
                    int iUnitInStock = int.Parse(myCommand.ExecuteScalar().ToString());
                    for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                    {
                        if (myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString().ToLower() == this.txtProductID.Text.Trim().ToLower())
                        {
                            if (iTotalItem > iUnitInStock)
                            {
                                myPage.dgvProduct.Rows[iCtr].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                                myPage.txtMessage.Text += "PRODUCT: " + myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString() + " only have " + iUnitInStock.ToString("d") + " PC available in stock; ";
                            }
                            else 
                            {
                                myPage.dgvProduct.Rows[iCtr].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                            }
                        }
                    }

                }
            
                this.Close();
            }
            #endregion

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
