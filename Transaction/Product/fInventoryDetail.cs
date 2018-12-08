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
    public partial class fInventoryDetail : Form
    {
        public fInventoryDetail(String ProductID, String ProductName, int PC, int CaseSize, String Category)
        {
            InitializeComponent();
            txtProductID.Text = ProductID;
            txtProductName.Text = ProductName;
            txtUnitInStockPC.Text = PC.ToString();
            txtCaseSize.Text = CaseSize.ToString();
            txtStockCS.Text = (PC / CaseSize).ToString();
            txtCategory.Text = Category;
        }

        private void rdbAdd_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAdd.Checked)
            {
                btnSave.Text = "Add";
                txtSubtractCS.Text = "0";
                txtSubtractPC.Text = "0";
            }
            else
            {
                txtAddPC.Focus();   
            }
        }

        private void rdbSubtract_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbSubtract.Checked)
            {
                btnSave.Text = "Subtract";
                txtAddCS.Text = "0";
                txtAddPC.Text = "0";
            }
            else
            {
                txtSubtractPC.Focus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int iCaseSize = int.Parse(txtCaseSize.Text);
            int iPC = 0, iCS = 0, iAtIndex=0; Boolean blnFound = false;

            if (txtAddPC.Text.Trim().Length == 0) txtAddPC.Text = "0";
            if (txtAddCS.Text.Trim().Length == 0) txtAddCS.Text = "0";
            if (txtSubtractCS.Text.Trim().Length == 0) txtSubtractCS.Text = "0";
            if (txtSubtractPC.Text.Trim().Length == 0) txtSubtractPC.Text = "0";

            if (btnSave.Text.ToLower() == "add")
            {
                if ((txtAddPC.Text.Trim().Length != 0 && int.TryParse(txtAddPC.Text, out iPC)) &&
                    (txtAddCS.Text.Trim().Length != 0 && int.TryParse(txtAddCS.Text, out iCS)))
                {
                    if ((iPC < 0 || iCS < 0) || (iPC == 0 && iCS == 0))
                    {
                        txtMessage.Text = "A positive whole number for (Add) PC/CS field is required.";
                        AlavaSoft.Message.fMessage myMessage = new AlavaSoft.Message.fMessage("Exclamation", "A valid input for (ADD)PC/CS field is required.");
                        myMessage.ShowDialog();
                    }
                    else 
                    {
                        //--Calculate Total Piece--
                        iPC += (iCS * int.Parse(txtCaseSize.Text.Trim()));

                        //--Search for Similar Product--
                        AlavaSoft.Transaction.Product.fInventoryEdit myPage = (AlavaSoft.Transaction.Product.fInventoryEdit)Program.ofAlavaSoft.ActiveMdiChild;
                        for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++) 
                        {
                            if (txtProductID.Text.Trim() == myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString() && myPage.dgvProduct.Rows[iCtr].Displayed)
                            {
                                blnFound = true;
                                iAtIndex = iCtr;
                            }
                        }

                        if (blnFound)
                        {
                            int iTotalAdd = int.Parse(myPage.dgvProduct.Rows[iAtIndex].Cells[4].Value.ToString());
                            int iTotalInventory = int.Parse(myPage.dgvProduct.Rows[iAtIndex].Cells[6].Value.ToString());
                            iTotalAdd += iPC; //PC - Contains Case Computation
                            iTotalInventory += iPC;

                            myPage.dgvProduct.Rows[iAtIndex].Cells[4].Value = iTotalAdd;
                            myPage.dgvProduct.Rows[iAtIndex].Cells[6].Value = iTotalInventory;
                            myPage.dgvProduct.Rows[iAtIndex].Visible = true; //Incase of deleted row;
                        }
                        else
                        {
                            //--Add New Row--
                            myPage.dgvProduct.Rows.Add(txtProductID.Text, txtCategory.Text, txtProductName.Text, txtCaseSize.Text, iPC, 0, (iPC + int.Parse(txtUnitInStockPC.Text)));
                        }

                        myPage.dgvProduct.Focus();
                        this.Close();
                    }
                }
                else 
                {
                    txtMessage.Text = "A positive whole number for (Add) PC/CS field is required.";
                    AlavaSoft.Message.fMessage myMessage = new AlavaSoft.Message.fMessage("Exclamation", "A valid input for (ADD)PC/CS field is required.");
                    myMessage.ShowDialog();
                }
            }
            else //--Subtraction--
            {
                if ((txtSubtractPC.Text.Trim().Length != 0 && int.TryParse(txtSubtractPC.Text, out iPC)) &&
                    (txtSubtractCS.Text.Trim().Length != 0 && int.TryParse(txtSubtractCS.Text, out iCS)))
                {
                    if ((iPC < 0 || iCS < 0) || (iPC == 0 && iCS == 0))
                    {
                        txtMessage.Text = "A positive whole number for (Add) PC/CS field is required.";
                        AlavaSoft.Message.fMessage myMessage = new AlavaSoft.Message.fMessage("Exclamation", "A valid input for (ADD)PC/CS field is required.");
                        myMessage.ShowDialog();
                    }
                    else 
                    {
                        //--Calculate Total Piece--
                        iPC += (iCS * int.Parse(txtCaseSize.Text.Trim()));

                        //--Search for Similar Product--
                        AlavaSoft.Transaction.Product.fInventoryEdit myPage = (AlavaSoft.Transaction.Product.fInventoryEdit)Program.ofAlavaSoft.ActiveMdiChild;
                        for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                        {
                            if (txtProductID.Text.Trim() == myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString() && myPage.dgvProduct.Rows[iCtr].Displayed )
                            {
                                blnFound = true;
                                iAtIndex = iCtr;
                            }
                        }

                        if (blnFound)
                        {
                            int iTotalSubtract = int.Parse(myPage.dgvProduct.Rows[iAtIndex].Cells[5].Value.ToString());
                            int iTotalInventory = int.Parse(myPage.dgvProduct.Rows[iAtIndex].Cells[6].Value.ToString());
                            iTotalSubtract += iPC; //PC - Contains Case Computation
                            iTotalInventory -= iPC;

                            myPage.dgvProduct.Rows[iAtIndex].Cells[5].Value = iTotalSubtract;
                            myPage.dgvProduct.Rows[iAtIndex].Cells[6].Value = iTotalInventory;
                            myPage.dgvProduct.Rows[iAtIndex].Visible = true; //Incase of deleted row;
                        }
                        else
                        {
                            //--Add New Row--
                            myPage.dgvProduct.Rows.Add(txtProductID.Text, txtCategory.Text, txtProductName.Text, txtCaseSize.Text, 0, iPC, (int.Parse(txtUnitInStockPC.Text) - iPC));
                        }

                        myPage.dgvProduct.Focus();
                        this.Close();
                    }
                }
                else
                {
                    txtMessage.Text = "A positive whole number for (SUBTRACT) PC/CS field is required.";
                    AlavaSoft.Message.fMessage myMessage = new AlavaSoft.Message.fMessage("Exclamation", "A valid input for (SUBTRACT)PC/CS field is required.");
                    myMessage.ShowDialog();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
