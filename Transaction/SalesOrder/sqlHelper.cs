using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AlavaSoft.Transaction.SalesOrder
{
    class sqlHelper
    {
        private SqlConnection myConnection = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataReader myDataReader = null;

        public sqlHelper()
        {
        }

        public int addSalesOrder(fSalesOrderAdd myPage, int EmployeeID)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0, iTerms = 0;
            double dBillingAmount = 0.00;
            
            try
            {
                #region Instantion Section
                Boolean hasError = false;
                myTransaction = myConnection.BeginTransaction();
                myCommand = myConnection.CreateCommand();
                myCommand.Transaction = myTransaction;
                #endregion

                #region Check Input Section
                //--Erase Previous Message--
                myPage.txtMessage.Text = "";

                //--Customer ID is required--
                if (myPage.getCustomerID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Customer ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for CUSTOMER ID field; \n\r";
                    hasError = true;
                }

                //--Terms (days) is required--
                if (myPage.txtTerms.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Terms (days)");
                    }
                    myPage.txtMessage.Text += "Provide a value for TERMS field; \n\r";
                    hasError = true;
                }

                //--Terms must be numeric--
                if (!int.TryParse(myPage.txtTerms.Text.Trim(), out iTerms))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Terms (days)");
                        myPage.addMessageParameter("numeric");
                    }
                    myPage.txtMessage.Text += "TERMS field must be numeric; \n\r";
                    hasError = true;
                }

                //--Billing Amount is required--
                if (myPage.txtBillingAmount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Billing Amount");
                    }
                    myPage.txtMessage.Text += "Provide a value for BILLING AMOUNT field; \n\r";
                    hasError = true;
                }

                //--Billing Amount must be numeric--
                if (!double.TryParse(myPage.txtBillingAmount.Text.Trim(), out dBillingAmount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Billing amount");
                        myPage.addMessageParameter("numeric");
                    }
                    myPage.txtMessage.Text += "BILLING AMOUNT must be numeric; \n\r";
                    hasError = true;
                }
                else 
                {
                    //--Billing amount must be a positive number greater than 0--
                    if (dBillingAmount <= 0)
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(3);
                            myPage.addMessageParameter("Billing amount");
                            myPage.addMessageParameter("greater than zero");
                        }
                        myPage.txtMessage.Text += "BILLING AMOUNT must be a positive number; \n\r";
                        hasError = true;
                    }
                }

                //--SO Details is required--
                Boolean hasDetails = false;
                for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                {
                    if (myPage.dgvProduct.Rows[iCtr].Visible)
                    {
                        hasDetails = true;
                        break;
                    }
                }

                if (!hasDetails)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("SO Details");
                    }
                    myPage.txtMessage.Text += "Include a product at SO Details list; \n\r";
                    hasError = true;
                }
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get SO Number--
                    myCommand.CommandText = "SELECT cnt FROM c_key_gen WHERE Module='SO'";
                    int iSONumber = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Update SO Counter--
                    myCommand.CommandText = "UPDATE c_key_gen SET cnt=cnt+1 WHERE Module='SO'";
                    myCommand.ExecuteNonQuery();

                    //--Insert SO Header--
                    myCommand.CommandText = "INSERT INTO a_sales_order(SalesOrderNo, CustomerID, EmployeeID, TransactionDate, RequiredDate, ShippedDate, Terms, Discount, Amount, PaidAmount, ReturnAmount, RefundAmount, Status, Remarks, DestinationAddress) " +
                                            "VALUES(@SalesOrderNo, @CustomerID, @EmployeeID, @TransactionDate, @RequiredDate, @ShippedDate, @Terms, @Discount, @Amount, @PaidAmount, @ReturnAmount, @RefundAmount, @Status, @Remarks, @DestinationAddress)";
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", "SO-" + iSONumber);
                    myCommand.Parameters.AddWithValue("@CustomerID", myPage.getCustomerID());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value.ToString());
                    if (myPage.dtpRequiredDate.Checked)
                    {
                        myCommand.Parameters.AddWithValue("@RequiredDate", myPage.dtpRequiredDate.Value.ToString());
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@RequiredDate", string.Empty);
                    }

                    if (myPage.dtpShippingDate.Checked)
                    {
                        myCommand.Parameters.AddWithValue("@ShippedDate", myPage.dtpShippingDate.Value.ToString());
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@ShippedDate", String.Empty);
                    }

                    myCommand.Parameters.AddWithValue("@Terms", myPage.txtTerms.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Discount", double.Parse(myPage.lblTotalAmount.Text.Trim()) - dBillingAmount);
                    myCommand.Parameters.AddWithValue("@Amount", dBillingAmount);
                    myCommand.Parameters.AddWithValue("@PaidAmount", 0.00);
                    myCommand.Parameters.AddWithValue("@ReturnAmount", 0.00);
                    myCommand.Parameters.AddWithValue("@RefundAmount", 0.00);
                    myCommand.Parameters.AddWithValue("@Status", 1); //--For Shipment--
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@DestinationAddress", myPage.txtShippingAddress.Text.Trim());
                    myResult += myCommand.ExecuteNonQuery();

                    //--Insert SO Details--
                    myCommand.Parameters.AddWithValue("@PID", "");
                    myCommand.Parameters.AddWithValue("@Quantity", 0);
                    myCommand.Parameters.AddWithValue("@UnitPrice", 0.00);
                    myCommand.Parameters.AddWithValue("@DiscountDetails", 0.00);
                    myCommand.Parameters.AddWithValue("@ReturnQuantity", 0);
                    myCommand.Parameters.AddWithValue("@CaseSize", 0);
                    for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                    {
                        if (myPage.dgvProduct.Rows[iCtr].Visible)
                        {
                            myCommand.CommandText = "INSERT INTO a_sales_order_detail(SalesOrderNo, PID, Quantity, UnitPrice, Discount, ReturnQuantity, CaseSize) " +
                                                    "VALUES(@SalesOrderNo, @PID, @Quantity, @UnitPrice, @DiscountDetails, @ReturnQuantity, @CaseSize) ";
                            //--Sales Order No is Already Set--
                            myCommand.Parameters["@PID"].Value = myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString();
                            myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvProduct.Rows[iCtr].Cells[4].Value.ToString());
                            myCommand.Parameters["@UnitPrice"].Value = double.Parse(myPage.dgvProduct.Rows[iCtr].Cells[6].Value.ToString());
                            myCommand.Parameters["@DiscountDetails"].Value = double.Parse(myPage.dgvProduct.Rows[iCtr].Cells[7].Value.ToString());
                            myCommand.Parameters["@CaseSize"].Value = int.Parse(myPage.dgvProduct.Rows[iCtr].Cells[3].Value.ToString());
                            myResult += myCommand.ExecuteNonQuery();

                            myCommand.CommandText = "UPDATE a_product SET UnitOnOrder = UnitOnOrder + @Quantity WHERE PID=@PID";
                            myResult += myCommand.ExecuteNonQuery();
                        }
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Sales Order(" + "SO-" + iSONumber + ") was SUCCESSFULLY SAVED.";
                        myPage.setSONumber("SO-" + iSONumber);
                    }
                    else
                    {
                        myPage.setMessageNumber(5);
                        myPage.txtMessage.Text = "The process succeeded but NO records where affected. POSSIBLE CAUSE: No data corresponds to your input. ";
                    }

                    myTransaction.Commit();
                }
                else
                {
                    myTransaction.Rollback();
                    myResult = 0;
                }

                #endregion
            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
                myResult = 0;
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }

                if (this.myDataReader != null)
                {
                    this.myDataReader.Close();
                }
            }

            return myResult;
        }

        public void showDetails(fSalesOrderEdit myPage)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();

            try
            {
                #region Instantion Section
                Boolean hasError = false;
                myCommand = myConnection.CreateCommand();
                #endregion

                #region Check Input Section
                myPage.txtMessage.Text ="";
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Retrieve Supplier Information--
                    myCommand.CommandText = "SELECT a_product.PID AS [Product Code], a_category.CategoryName AS Category, a_product.ProductName AS [Product Name], " +
                                            "       a_sales_order_detail.CaseSize AS [Pack Size], a_sales_order_detail.Quantity - a_sales_order_detail.ReturnQuantity AS [Quantity (PC)],  " +
                                            "       (a_sales_order_detail.Quantity - a_sales_order_detail.ReturnQuantity) / a_sales_order_detail.CaseSize AS [Quantity (CS)],  " +
                                            "       a_sales_order_detail.UnitPrice AS [Unit Price], a_sales_order_detail.Discount, (a_sales_order_detail.Quantity - a_sales_order_detail.ReturnQuantity)  " +
                                            "       * a_sales_order_detail.UnitPrice - ((a_sales_order_detail.Quantity - a_sales_order_detail.ReturnQuantity) * a_sales_order_detail.UnitPrice)  " +
                                            "       * (a_sales_order_detail.Discount / 100) AS [Total Price], a_product.UnitInStock " +
                                            "FROM   a_sales_order_detail INNER JOIN " +
                                            "       a_product ON a_sales_order_detail.PID = a_product.PID INNER JOIN " +
                                            "       a_category ON a_product.CategoryID = a_category.CategoryID " +
                                            "WHERE  (a_sales_order_detail.SalesOrderNo = @SONo)";
                    myCommand.Parameters.AddWithValue("@SONo", myPage.getSONumber());
                    myDataReader = myCommand.ExecuteReader();
                    int iCtr = 0;
                    while (myDataReader.Read())
                    {
                        myPage.dgvProduct.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetInt32(3), myDataReader.GetInt32(4), myDataReader.GetInt32(5), myDataReader.GetDecimal(6), myDataReader.GetDecimal(7), myDataReader.GetDecimal(8) );
                        if (myDataReader.GetInt32(9) < myDataReader.GetInt32(4) && myPage.cmbStatus.Text.Trim().ToLower() == "for shipment") //--cmbStatus contains the initial status of SO--
                        {
                            myPage.dgvProduct.Rows[iCtr].DefaultCellStyle.BackColor = System.Drawing.Color.Red;
                            myPage.txtMessage.Text += "PRODUCT: " + myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString() + " only have " + myDataReader.GetInt32(9).ToString("d") + " PC available in stock; "; 
                        }

                        iCtr++;
                    }
                    myDataReader.Close();
                }
                #endregion
            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT COMPLETED: " + e.ToString();
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }

                if (this.myDataReader != null)
                {
                    this.myDataReader.Close();
                }
            }
        }

        public int editSalesOrder(fSalesOrderEdit myPage, int EmployeeID)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0, iTerms = 0;
            double dBillingAmount = 0.00;
            Boolean bReadyToCommit = true;
            String sPreviousStatus = "";

            try
            {
                #region Instantion Section
                Boolean hasError = false;
                myTransaction = myConnection.BeginTransaction();
                myCommand = myConnection.CreateCommand();
                myCommand.Transaction = myTransaction;
                #endregion

                #region Check Input Section
                //--Erase Previous Message--
                myPage.txtMessage.Text = "";

                //--Sales Order No is required--
                if (myPage.getSONumber().Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Sales Order No");
                    }
                    myPage.txtMessage.Text += "Provide a value for SALES ORDER NO field; \n\r";
                    hasError = true;
                }

                //--Customer ID is required--
                if (myPage.getCustomerID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Customer ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for CUSTOMER ID field; \n\r";
                    hasError = true;
                }

                //--Terms (days) is required--
                if (myPage.txtTerms.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Terms (days)");
                    }
                    myPage.txtMessage.Text += "Provide a value for TERMS field; \n\r";
                    hasError = true;
                }

                //--Terms must be numeric--
                if (!int.TryParse(myPage.txtTerms.Text.Trim(), out iTerms))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Terms (days)");
                        myPage.addMessageParameter("numeric");
                    }
                    myPage.txtMessage.Text += "TERMS field must be numeric; \n\r";
                    hasError = true;
                }

                //--Billing Amount is required--
                if (myPage.txtBillingAmount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Billing Amount");
                    }
                    myPage.txtMessage.Text += "Provide a value for BILLING AMOUNT field; \n\r";
                    hasError = true;
                }

                //--Billing Amount must be numeric--
                if (!double.TryParse(myPage.txtBillingAmount.Text.Trim(), out dBillingAmount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Billing amount");
                        myPage.addMessageParameter("numeric");
                    }
                    myPage.txtMessage.Text += "BILLING AMOUNT must be numeric; \n\r";
                    hasError = true;
                }
                else
                {
                    //--Billing amount must be a positive number greater than 0--
                    if (dBillingAmount <= 0)
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(3);
                            myPage.addMessageParameter("Billing amount");
                            myPage.addMessageParameter("greater than zero");
                        }
                        myPage.txtMessage.Text += "BILLING AMOUNT must be a positive number; \n\r";
                        hasError = true;
                    }
                }

                //--SO Details is required--
                Boolean hasDetails = false;
                for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                {
                    if (myPage.dgvProduct.Rows[iCtr].Visible)
                    {
                        hasDetails = true;
                        break;
                    }
                }

                if (!hasDetails)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("SO Details");
                    }
                    myPage.txtMessage.Text += "Include a product at SO Details list; \n\r";
                    hasError = true;
                }

                //--Check status--
                //--Get Previous Status--
                myCommand.CommandText = "SELECT LookUpName FROM c_look_up WHERE LookUpDiv='SalesOrder' AND ValueID = " +
                                        "(SELECT Status FROM a_sales_order WHERE SalesOrderNo=@SalesOrderNo)";
                myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.getSONumber());
                sPreviousStatus = myCommand.ExecuteScalar().ToString();

                if (sPreviousStatus.Trim().ToLower() == "for shipment" && myPage.cmbStatus.Text.Trim().ToLower() == "for collection")
                {
                    //--Check for negative Inventory--
                    if (!hasError) //--Sole View--
                    {
                        int iCheckQuantity = 0;
                        myCommand.CommandText = "SELECT UnitInStock FROM a_product WHERE PID=@PID";
                        myCommand.Parameters.AddWithValue("@PID", "");
                        for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                        {
                            if (myPage.dgvProduct.Rows[iCtr].Visible)
                            {
                                myCommand.Parameters["@PID"].Value = myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString();
                                iCheckQuantity = int.Parse(myCommand.ExecuteScalar().ToString());
                                if (iCheckQuantity < int.Parse(myPage.dgvProduct.Rows[iCtr].Cells[4].Value.ToString()))
                                {
                                    if (!hasError)
                                    {
                                        myPage.setMessageNumber(13);
                                    }
                                    hasError = true;
                                    myPage.txtMessage.Text +=  "PRODUCT: " + myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString() + " only have " + iCheckQuantity.ToString("d") + " PC available in stock; ";
                                }
                            }
                        }
                    }
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get New Status ID--
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='SalesOrder' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.getSONumber());
                    myCommand.Parameters.AddWithValue("@LookUpName", myPage.cmbStatus.Text.Trim());
                    int iStatus = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Update SO Header--
                    myCommand.CommandText = "UPDATE a_sales_order SET CustomerID=@CustomerID, EmployeeID=@EmployeeID, " +
                                            "                         TransactionDate=@TransactionDate, RequiredDate=@RequiredDate, " +
                                            "                         ShippedDate=@ShippedDate, Terms=@Terms, Discount=@Discount, " +
                                            "                         Amount=@Amount, Status=@Status, Remarks=@Remarks, " +
                                            "                         DestinationAddress=@DestinationAddress " +
                                            "WHERE SalesOrderNo=@SalesOrderNo";
                    //--Sales Order No is already set--
                    myCommand.Parameters.AddWithValue("@CustomerID", myPage.getCustomerID());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value.ToString());
                    if (myPage.dtpRequiredDate.Checked)
                    {
                        myCommand.Parameters.AddWithValue("@RequiredDate", myPage.dtpRequiredDate.Value.ToString());
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@RequiredDate", string.Empty);
                    }

                    if (myPage.dtpShippingDate.Checked)
                    {
                        myCommand.Parameters.AddWithValue("@ShippedDate", myPage.dtpShippingDate.Value.ToString());
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@ShippedDate", String.Empty);
                    }

                    myCommand.Parameters.AddWithValue("@Terms", myPage.txtTerms.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Discount", double.Parse(myPage.lblTotalAmount.Text.Trim()) - dBillingAmount);
                    myCommand.Parameters.AddWithValue("@Amount", dBillingAmount);
                    myCommand.Parameters.AddWithValue("@Status", iStatus);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@DestinationAddress", myPage.txtShippingAddress.Text.Trim());
                    myResult += myCommand.ExecuteNonQuery();

                    //--Insert SO Details For Shipment--
                    //--No Product Returns YET for SO with 'for shipment' status--
                    if (sPreviousStatus.Trim().ToLower() == "for shipment")
                    {
                        myCommand.Parameters.AddWithValue("@PID", "");
                        myCommand.Parameters.AddWithValue("@Quantity", 0);
                        myCommand.Parameters.AddWithValue("@UnitPrice", 0.00);
                        myCommand.Parameters.AddWithValue("@DiscountDetails", 0.00);
                        myCommand.Parameters.AddWithValue("@ReturnQuantity", 0);
                        myCommand.Parameters.AddWithValue("@CaseSize", 0);

                        //--Get Previous SO Details--
                        myCommand.CommandText = "SELECT PID, Quantity FROM a_sales_order_detail WHERE SalesOrderNo=@SalesOrderNo ";
                        //--Sales Order Is Set--
                        myDataReader = myCommand.ExecuteReader();
                        Queue<String> qPID = new Queue<String>();
                        Queue<int> qQuantity = new Queue<int>();
                        while (myDataReader.Read())
                        {
                            qPID.Enqueue(myDataReader.GetString(0));
                            qQuantity.Enqueue(myDataReader.GetInt32(1));
                        }
                        myDataReader.Close();

                        //--Subtract from reserved Products--
                        myCommand.CommandText = "UPDATE a_product SET UnitOnOrder=UnitOnOrder-@Quantity WHERE PID=@PID";
                        while (qPID.Count > 0 && qQuantity.Count > 0)
                        {
                            myCommand.Parameters["@Quantity"].Value = qQuantity.Dequeue();
                            myCommand.Parameters["@PID"].Value = qPID.Dequeue();
                            myCommand.ExecuteNonQuery();
                        }
                        qPID = null;
                        qQuantity = null;

                        //--Delete Previous Sales Order details--
                        myCommand.CommandText = "DELETE FROM a_sales_order_detail WHERE SalesOrderNo=@SalesOrderNo";
                        //--Sales Order No is Set--
                        myCommand.ExecuteNonQuery();

                        //--Insert New Sales Order Details--
                        for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                        {
                            if (myPage.dgvProduct.Rows[iCtr].Visible)
                            {
                                myCommand.CommandText = "INSERT INTO a_sales_order_detail(SalesOrderNo, PID, Quantity, UnitPrice, Discount, ReturnQuantity, CaseSize) " +
                                                        "VALUES(@SalesOrderNo, @PID, @Quantity, @UnitPrice, @DiscountDetails, @ReturnQuantity, @CaseSize) ";
                                //--Sales Order No is Already Set--
                                myCommand.Parameters["@PID"].Value = myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString();
                                myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvProduct.Rows[iCtr].Cells[4].Value.ToString());
                                myCommand.Parameters["@UnitPrice"].Value = double.Parse(myPage.dgvProduct.Rows[iCtr].Cells[6].Value.ToString());
                                myCommand.Parameters["@DiscountDetails"].Value = double.Parse(myPage.dgvProduct.Rows[iCtr].Cells[7].Value.ToString());
                                myCommand.Parameters["@CaseSize"].Value = int.Parse(myPage.dgvProduct.Rows[iCtr].Cells[3].Value.ToString());
                                myResult += myCommand.ExecuteNonQuery();

                                if (myPage.cmbStatus.Text.Trim().ToLower() == "for collection") //--There was a change of status from 'for shipment' TO 'for collection'--
                                {
                                    myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock - @Quantity WHERE PID=@PID";
                                    myResult += myCommand.ExecuteNonQuery();
                                }
                                else if (myPage.cmbStatus.Text.Trim().ToLower() == "for shipment") //--No Change in Status--
                                {
                                    myCommand.CommandText = "UPDATE a_product SET UnitOnOrder = UnitOnOrder + @Quantity WHERE PID=@PID";
                                    myResult += myCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        if (myPage.cmbStatus.Text.Trim().ToLower() == "for collection") //--There was a change of status from 'for shipment' TO 'for collection'--
                        {
                            //--Recheck for Negative Inventory--
                            myCommand.CommandText = "SELECT 'EXIST' FROM a_product WHERE UnitInStock<0";
                            myDataReader = myCommand.ExecuteReader();
                            myDataReader.Read();
                            if (myDataReader.HasRows)
                            {
                                myPage.txtMessage.Text = "Other concurrent users of the application have changed the total stock quantity which makes the details of this SO INSUFFICIENT.";
                                myResult = 0;
                                myPage.setMessageNumber(13);
                                myTransaction.Rollback();
                                bReadyToCommit = false;
                            }
                            myDataReader.Close();
                        }
                    } 

                    if (bReadyToCommit)
                    {
                        if (myResult > 0)
                        {
                            myPage.setMessageNumber(7);
                            myPage.txtMessage.Text = "Sales Order(" + myPage.getSONumber() + ") was SUCCESSFULLY SAVED.";
                        }
                        else
                        {
                            myPage.setMessageNumber(5);
                            myPage.txtMessage.Text = "The process succeeded but NO records where affected. POSSIBLE CAUSE: No data corresponds to your input. ";
                        }
                        myTransaction.Commit();
                    }
                }
                else
                {
                    myTransaction.Rollback();
                    myResult = 0;
                }

                #endregion

            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
                myResult = 0;
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }

                if (this.myDataReader != null)
                {
                    this.myDataReader.Close();
                }
            }

            return myResult;
        }

        public int deleteSalesOrder(fSalesOrderEdit myPage)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;

            try
            {
                #region Instantion Section
                Boolean hasError = false;
                myTransaction = myConnection.BeginTransaction();
                myCommand = myConnection.CreateCommand();
                myCommand.Transaction = myTransaction;
                #endregion

                #region Check Input Section
                //--Erase Previous Message--
                myPage.txtMessage.Text = "";

                //--Sales Order No is required--
                if (myPage.getSONumber().Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Sales Order No");
                    }
                    myPage.txtMessage.Text += "Provide a value for SALES ORDER NO field; \n\r";
                    hasError = true;
                }

                if (!hasError) //--Because of txtMessage must only check this criteria the last--
                {
                    //--Referenced Item: Payments--
                    myCommand.CommandText = "SELECT 'EXIST' FROM a_payment_detail WHERE SalesOrderNo=@SalesOrderNo; ";
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.getSONumber());
                    myDataReader = myCommand.ExecuteReader();
                    if (myDataReader.HasRows)
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(6);
                        }
                        myPage.txtMessage.Text += "Payment details; \n\r";
                        hasError = true;
                    }
                    myDataReader.Close();

                    //--Referenced Item: Refund--
                    myCommand.CommandText = "SELECT 'EXIST' FROM a_refund WHERE SalesOrderNo=@SalesOrderNo; ";
                    //--Sales Order No is set--
                    myDataReader = myCommand.ExecuteReader();
                    if (myDataReader.HasRows)
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(6);
                        }
                        myPage.txtMessage.Text += "Refund details; \n\r";
                        hasError = true;
                    }
                    myDataReader.Close();

                    if (myPage.txtMessage.Text.Trim().Length > 0)
                    {
                        myPage.txtMessage.Text = "CANNOT DELETE a referenced record. THIS SALES ORDER HAS: " + myPage.txtMessage.Text;
                    }
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.getSONumber());
                    myCommand.Parameters.AddWithValue("@PID", "");
                    myCommand.Parameters.AddWithValue("@Quantity", 0);

                    //--Get Previous SO Details--
                    myCommand.CommandText = "SELECT PID, Quantity FROM a_sales_order_detail WHERE SalesOrderNo=@SalesOrderNo ";
                    //--Sales Order Is Set--
                    myDataReader = myCommand.ExecuteReader();
                    Queue<String> qPID = new Queue<String>();
                    Queue<int> qQuantity = new Queue<int>();
                    while (myDataReader.Read())
                    {
                        qPID.Enqueue(myDataReader.GetString(0));
                        qQuantity.Enqueue(myDataReader.GetInt32(1));
                    }
                    myDataReader.Close();

                    //--Subtract from reserved Products--
                    myCommand.CommandText = "UPDATE a_product SET UnitOnOrder=UnitOnOrder-@Quantity WHERE PID=@PID";
                    while (qPID.Count > 0 && qQuantity.Count > 0)
                    {
                        myCommand.Parameters["@Quantity"].Value = qQuantity.Dequeue();
                        myCommand.Parameters["@PID"].Value = qPID.Dequeue();
                        myCommand.ExecuteNonQuery();
                    }
                    qPID = null;
                    qQuantity = null;

                    //--Delete Previous Sales Order details--
                    myCommand.CommandText = "DELETE FROM a_sales_order_detail WHERE SalesOrderNo=@SalesOrderNo";
                    //--Sales Order No is Set--
                    myResult += myCommand.ExecuteNonQuery();

                    myCommand.CommandText = "DELETE FROM a_sales_order WHERE SalesOrderNo=@SalesOrderNo";
                    myResult += myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Sales Order Information was SUCCESSFULLY DELETED.";
                    }
                    else
                    {
                        myPage.setMessageNumber(5);
                        myPage.txtMessage.Text = "The process succeeded but NO records where affected. POSSIBLE CAUSE: No data corresponds to your input. ";
                    }

                    myTransaction.Commit();
                }
                else
                {
                    myTransaction.Rollback();
                    myResult = 0;
                }

                #endregion
            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
                myResult = 0;
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }

                if (this.myDataReader != null)
                {
                    this.myDataReader.Close();
                }
            }

            return myResult;
        }

    }
}
