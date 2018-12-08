using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Transaction.Delivery
{
    public partial class fDeliveryDetail : Form
    {
        private Boolean boolEditDelivery = false;
        private Boolean boolEditProduct = false;

        private AlavaSoft.Class.Message myMessage = new AlavaSoft.Class.Message();

        public void setMessageNumber(int value)
        {
            myMessage.setMessageNumber(value);
        }

        public void addMessageParameter(String value)
        {
            myMessage.addParameter(value);
        }

        public fDeliveryDetail(String ProductID, String ProductName, String CaseSize, String Category, Boolean EditInterface, Boolean EditProduct)
        {
            InitializeComponent();
            txtProductID.Text = ProductID;
            txtProductName.Text = ProductName;
            txtCaseSize.Text = CaseSize;
            txtCategory.Text = Category;
            boolEditDelivery = EditInterface;
            boolEditProduct = EditProduct;
        }

        private void fDeliveryDetail_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean hasError = false;

            #region Check Input
            //--Required Field: Quantity--
            if (this.txtPC.Text.Trim().Length == 0 && this.txtCS.Text.Trim().Length == 0) 
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
            
            //--Numeric Field: Quantity--
            int iPC = 0, iCS = 0;
            if (this.txtPC.Text.Trim().Length != 0 && !int.TryParse(this.txtPC.Text.Trim(), out iPC))
            {
                this.txtMessage.Text += "Change the value of QUANTITY(PC) to make it numeric; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("QUANTITY (PC)");
                    this.addMessageParameter("numeric");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            if (this.txtCS.Text.Trim().Length != 0 && !int.TryParse(this.txtCS.Text.Trim(), out iCS))
            {
                this.txtMessage.Text += "Change the value of QUANTITY(CS) to make it numeric; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("QUANTITY (CS)");
                    this.addMessageParameter("numeric");
                    myMessage.showMessage();
                }
                hasError = true;
            }

            //--Required Field: Quantity (Both must not be zero)
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

            //--Positive Value: Quantity--
            if (iPC < 0 || iCS < 0)
            {
                this.txtMessage.Text += "Change the value of QUANTITY to make it a positive number; \n\r";
                if (!hasError)
                {
                    this.setMessageNumber(3);
                    this.addMessageParameter("QUANTITY");
                    this.addMessageParameter("a positive number");
                    myMessage.showMessage();
                }
                hasError = true;
            }
            #endregion

            #region Process Section
            if (!boolEditDelivery) //--Triggered At Add Delivery Page--
            {
                if (!hasError)
                {
                    //--Calculate Total Piece--
                    iPC += (iCS * int.Parse(txtCaseSize.Text.Trim()));

                    //--Search for Similar Product--
                    Boolean blnFound = false;
                    int iAtIndex = 0;
                    AlavaSoft.Transaction.Delivery.fDeliveryAdd myPage = (AlavaSoft.Transaction.Delivery.fDeliveryAdd)Program.ofAlavaSoft.ActiveMdiChild;
                    for (int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                    {
                        if (txtProductID.Text.Trim() == myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString() && myPage.dgvDelivery.Rows[iCtr].Displayed)
                        {
                            blnFound = true;
                            iAtIndex = iCtr;
                        }
                    }

                    if (blnFound)
                    {
                        if (!boolEditProduct) //--From Add New Product--
                        {
                            myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value = int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value.ToString()) + iPC;
                            myPage.dgvDelivery.Rows[iAtIndex].Cells[5].Value = int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value.ToString()) / int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[3].Value.ToString());
                            myPage.dgvDelivery.Rows[iAtIndex].Visible = true;
                        }
                        else //--Modify Existing Product Detail Information--
                        {
                            myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value = iPC;
                            myPage.dgvDelivery.Rows[iAtIndex].Cells[5].Value = int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value.ToString()) / int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[3].Value.ToString());
                            myPage.dgvDelivery.Rows[iAtIndex].Visible = true;
                        }
                    }
                    else
                    {
                        //--Add New Row--
                        myPage.dgvDelivery.Rows.Add(txtProductID.Text, txtCategory.Text, txtProductName.Text, txtCaseSize.Text, iPC, iPC / int.Parse(txtCaseSize.Text));
                    }
                }
            }
            else //--Trigerred at Edit Delivery Page-- 
            {
                //--Calculate Total Piece--
                iPC += (iCS * int.Parse(txtCaseSize.Text.Trim()));

                //--Search for Similar Product--
                Boolean blnFound = false;
                int iAtIndex = 0;
                AlavaSoft.Transaction.Delivery.fDeliveryEdit myPage = (AlavaSoft.Transaction.Delivery.fDeliveryEdit)Program.ofAlavaSoft.ActiveMdiChild;
                for (int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                {
                    if (txtProductID.Text.Trim() == myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString() && myPage.dgvDelivery.Rows[iCtr].Displayed)
                    {
                        blnFound = true;
                        iAtIndex = iCtr;
                    }
                }

                if (blnFound)
                {
                    if (!boolEditProduct) //--From Add New Product--
                    {
                        myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value = int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value.ToString()) + iPC;
                        myPage.dgvDelivery.Rows[iAtIndex].Cells[5].Value = int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value.ToString()) / int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[3].Value.ToString());
                        myPage.dgvDelivery.Rows[iAtIndex].Visible = true;
                    }
                    else //--Modify Existing Product Detail Information--
                    {
                        myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value = iPC;
                        myPage.dgvDelivery.Rows[iAtIndex].Cells[5].Value = int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[4].Value.ToString()) / int.Parse(myPage.dgvDelivery.Rows[iAtIndex].Cells[3].Value.ToString());
                        myPage.dgvDelivery.Rows[iAtIndex].Visible = true;
                    }
                }
                else
                {
                    //--Add New Row--
                    myPage.dgvDelivery.Rows.Add(txtProductID.Text, txtCategory.Text, txtProductName.Text, txtCaseSize.Text, iPC, iPC / int.Parse(txtCaseSize.Text));
                }

            }

            #endregion

            this.Close();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
