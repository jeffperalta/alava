using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AlavaSoft.Transaction.Return
{
    class sqlHelper
    {
        private SqlConnection myConnection = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataReader myDataReader = null;

        public int addReturns(fReturnAdd myPage, int EmployeeID)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
            double dReturnAmount = 0.00;

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

                //--Required Field: Sales Order--
                if (myPage.getSalesOrderNo().Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Sales Order No");
                    }
                    myPage.txtMessage.Text += "Provide a value for SALES ORDER field; \n\r";
                    hasError = true;
                }

                //--Required Field: Reason--
                if (myPage.cmbReason.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Reason");
                    }
                    myPage.txtMessage.Text += "Provide a value for REASON field; \n\r";
                    hasError = true;
                }

                //--Required Field: Status--
                if (myPage.cmbStatus.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Status");
                    }
                    myPage.txtMessage.Text += "Provide a value for STATUS field; \n\r";
                    hasError = true;
                }

                //--Required Field: Sales Order Details Return Quantity--
                if (myPage.getReturnAmount() == 0) //--The Original Computation of return amount--
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("New Return Quantity");
                    }
                    myPage.txtMessage.Text += "No return details. Provide a value for NEW RETURN QUANTITY field; \n\r";
                    hasError = true;
                }

                //--Required Field: Return Amount--
                if (myPage.txtReturnAmount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Return Amount");
                    }
                    myPage.txtMessage.Text += "Provide a value for RETURN AMOUNT field; \n\r";
                    hasError = true;
                }

                //--Numeric Required: Return Amount--
                if (!double.TryParse(myPage.txtReturnAmount.Text.Trim(), out dReturnAmount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Return Amount");
                        myPage.addMessageParameter("numeric");
                    }
                    myPage.txtMessage.Text += "RETURN AMOUNT field must be numeric; \n\r";
                    hasError = true;
                }
                else
                {
                    //--Positive Number: Return Amount--
                    if (dReturnAmount < 0)
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(3);
                            myPage.addMessageParameter("Return Amount");
                            myPage.addMessageParameter("a positive number");
                        }
                        myPage.txtMessage.Text += "RETURN AMOUNT field must be a positive number; \n\r";
                        hasError = true;
                    }
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get ReasonID--
                    myCommand.CommandText = "SELECT ValueID FROM c_look_up WHERE LookUpDiv='ReturnReason' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@LookUpName", myPage.cmbReason.Text.Trim());
                    int iReason = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Get StatusID--
                    myCommand.CommandText = "SELECT ValueID FROM c_look_up WHERE LookUpDiv='Return' AND LookUpName=@LookUpName";
                    myCommand.Parameters["@LookUpName"].Value = myPage.cmbStatus.Text.Trim();
                    int iStatus = int.Parse(myCommand.ExecuteScalar().ToString());


                    //--Get Return ID--
                    myCommand.CommandText = "SELECT MAX(ReturnID) + 1 FROM a_return";
                    int MaxID;
                    object temp = myCommand.ExecuteScalar();
                    if (temp == System.DBNull.Value)
                    {
                        MaxID = 1000;
                    }
                    else
                    {
                        MaxID = (int)temp;
                    }

                    myPage.setReturnID(MaxID);
                    myPage.txtReturnSlipNo.Text = "RETURN-" + MaxID;

                    //--Insert Return Header Information--
                    myCommand.CommandText = "INSERT INTO a_return (ReturnID, TransactionDate, SalesOrderNo, Amount, Remarks, EmployeeID, Reason, ReturnSlipNo, Status) " +
                                            "VALUES (@ReturnID, @TransactionDate, @SalesOrderNo, @Amount, @Remarks, @EmployeeID, @Reason, @ReturnSlipNo, @Status)";
                    myCommand.Parameters.AddWithValue("@ReturnID", MaxID);
                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.getSalesOrderNo());
                    myCommand.Parameters.AddWithValue("@Amount", dReturnAmount);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@Reason", iReason);
                    myCommand.Parameters.AddWithValue("@ReturnSlipNo", myPage.txtReturnSlipNo.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Status", iStatus);
                    myResult = myCommand.ExecuteNonQuery();

                    //--Update Sales Order--
                    myCommand.CommandText = "UPDATE a_sales_order SET ReturnAmount = ReturnAmount + @Amount " +
                                            "WHERE SalesOrderNo=@SalesOrderNo ";
                    //--Amount and Sales Order No are already set--
                    myCommand.ExecuteNonQuery();

                    //--Insert To Details--
                    Boolean blnAddToInventory = false;
                    if (myPage.cmbStatus.Text.Trim().ToLower() == "accounted")
                    {
                        blnAddToInventory = true;
                        
                        //--Get Alteration ID--
                        myCommand.CommandText = "SELECT MAX(AlterID) + 1 FROM a_alteration";
                        temp = myCommand.ExecuteScalar();
                        if (temp == System.DBNull.Value)
                        {
                            MaxID = 1000;
                        }
                        else
                        {
                            MaxID = (int)temp;
                        }

                        //--Get alter reason--
                        myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='Inventory' AND LookUpName='Product Returns'";
                        int iAlterReasonID = int.Parse(myCommand.ExecuteScalar().ToString());

                        //--Insert Alteration Header--
                        myCommand.CommandText = "INSERT INTO a_alteration (AlterID, TransactionDate, ReferenceNo, ReferenceDate, AlterReason, Remarks, EmployeeID) " +
                                                "VALUES (@AlterID, @TransactionDateAlter, @ReferenceNo, @ReferenceDate, @AlterReason, @Remarks, @EmployeeID)";
                        myCommand.Parameters.AddWithValue("@AlterID", MaxID);
                        myCommand.Parameters.AddWithValue("@TransactionDateAlter", DateTime.Now);
                        myCommand.Parameters.AddWithValue("@ReferenceNo", myPage.txtReturnSlipNo.Text);
                        myCommand.Parameters.AddWithValue("@ReferenceDate", myPage.dtpTransactionDate.Value);
                        myCommand.Parameters.AddWithValue("@AlterReason", iAlterReasonID);
                        //--remarks is already set--
                        //--employee ID is already set--
                        myCommand.ExecuteNonQuery();
                    }

                    myCommand.Parameters.AddWithValue("@PID", "");
                    myCommand.Parameters.AddWithValue("@Quantity", 0);
                    int dPIDReturnQuantity = 0;
                    for (int iCtr = 0; iCtr < myPage.dgvSODetails.Rows.Count; iCtr++)
                    {
                        if(int.TryParse(myPage.dgvSODetails.Rows[iCtr].Cells[7].Value.ToString(), out dPIDReturnQuantity))
                        {
                            if (dPIDReturnQuantity > 0)
                            {
                                myCommand.CommandText = "INSERT INTO a_return_detail (ReturnID, PID, Quantity, Amount) " +
                                                        "VALUES (@ReturnID, @PID, @Quantity, @Amount) ";
                                //--Return ID is Already Set--
                                myCommand.Parameters["@PID"].Value = myPage.dgvSODetails.Rows[iCtr].Cells[0].Value.ToString();
                                myCommand.Parameters["@Quantity"].Value = dPIDReturnQuantity;
                                myCommand.Parameters["@Amount"].Value = double.Parse(myPage.dgvSODetails.Rows[iCtr].Cells[4].Value.ToString());
                                myCommand.ExecuteNonQuery();

                                //--Update Sales Order Details--
                                myCommand.CommandText = "UPDATE a_sales_order_detail SET ReturnQuantity = ReturnQuantity + @Quantity " +
                                                        "WHERE SalesOrderNo=@SalesOrderNo AND PID=@PID ";
                                //--Quantity, Sales Order No, PID are already set--
                                myCommand.ExecuteNonQuery();

                                if (blnAddToInventory)
                                {
                                    myCommand.CommandText = "INSERT INTO a_alteration_detail (AlterID, PID, Quantity) " +
                                                            "VALUES(@AlterID, @PID, @Quantity) ";
                                    //--Alter ID, PID, Quantity are already set--
                                    myCommand.ExecuteNonQuery();

                                    myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock + @Quantity WHERE PID=@PID ";
                                    //--Quantity and PID are already set--
                                    myCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Product Return Information was SUCCESSFULLY SAVED.";
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

        public int editReturns(fReturnEdit myPage, int EmployeeID)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
            double dReturnAmount = 0.00;

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

                //--Required Field: Slip No--
                if (myPage.getReturnID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Slip No");
                    }
                    myPage.txtMessage.Text += "Provide a value for Slip No field; \n\r";
                    hasError = true;
                }

                //--Required Field: Reason--
                if (myPage.cmbReason.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Reason");
                    }
                    myPage.txtMessage.Text += "Provide a value for REASON field; \n\r";
                    hasError = true;
                }

                //--Required Field: Status--
                if (myPage.cmbStatus.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Status");
                    }
                    myPage.txtMessage.Text += "Provide a value for STATUS field; \n\r";
                    hasError = true;
                }

                //--Required Field: Sales Order Details Return Quantity--
                if (myPage.getReturnAmount() == 0) //--The Original Computation of return amount--
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("New Return Quantity");
                    }
                    myPage.txtMessage.Text += "No return details. Provide a value for NEW RETURN QUANTITY field; \n\r";
                    hasError = true;
                }

                //--Required Field: Return Amount--
                if (myPage.txtReturnAmount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Return Amount");
                    }
                    myPage.txtMessage.Text += "Provide a value for RETURN AMOUNT field; \n\r";
                    hasError = true;
                }

                //--Numeric Required: Return Amount--
                if (!double.TryParse(myPage.txtReturnAmount.Text.Trim(), out dReturnAmount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Return Amount");
                        myPage.addMessageParameter("numeric");
                    }
                    myPage.txtMessage.Text += "RETURN AMOUNT field must be numeric; \n\r";
                    hasError = true;
                }
                else
                {
                    //--Positive Number: Return Amount--
                    if (dReturnAmount < 0)
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(3);
                            myPage.addMessageParameter("Return Amount");
                            myPage.addMessageParameter("a positive number");
                        }
                        myPage.txtMessage.Text += "RETURN AMOUNT field must be a positive number; \n\r";
                        hasError = true;
                    }
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Previous Status--
                    myCommand.CommandText = "SELECT LookUpName FROM c_look_up " +
                                            "WHERE LookUpDiv='Return' AND ValueId = (" +    
                                            "      SELECT status FROM a_return WHERE ReturnID=@ReturnID)";
                    myCommand.Parameters.AddWithValue("@ReturnID", myPage.getReturnID());
                    String sPreviousStatus = myCommand.ExecuteScalar().ToString();

                    //--Get ReasonID--
                    myCommand.CommandText = "SELECT ValueID FROM c_look_up WHERE LookUpDiv='ReturnReason' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@LookUpName", myPage.cmbReason.Text.Trim());
                    int iReason = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Get StatusID--
                    myCommand.CommandText = "SELECT ValueID FROM c_look_up WHERE LookUpDiv='Return' AND LookUpName=@LookUpName";
                    myCommand.Parameters["@LookUpName"].Value = myPage.cmbStatus.Text.Trim();
                    int iStatus = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Update Sales Order--
                    myCommand.CommandText = "UPDATE a_sales_order SET ReturnAmount = ReturnAmount - (SELECT Amount FROM a_return WHERE ReturnID=@ReturnID) " +
                                            "WHERE SalesOrderNo=@SalesOrderNo ";
                    //--ReturnID is already set--
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.lblSalesOrderNo.Text.Trim());
                    myCommand.ExecuteNonQuery();

                    //--Update Return Header Information--
                    myCommand.CommandText = "UPDATE a_return SET " + 
                                            "       TransactionDate=@TransactionDate, Amount=@Amount, " +
                                            "       Remarks=@Remarks, EmployeeID=@EmployeeID, Reason=@Reason, " +
                                            "       Status=@Status " +
                                            "WHERE ReturnID=@ReturnID ";
                    //--Return id is already set--
                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@Amount", dReturnAmount);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@Reason", iReason);
                    myCommand.Parameters.AddWithValue("@Status", iStatus);
                    myResult = myCommand.ExecuteNonQuery();

                    //--Update Sales Order--
                    myCommand.CommandText = "UPDATE a_sales_order SET ReturnAmount = ReturnAmount + @Amount " +
                                            "WHERE SalesOrderNo=@SalesOrderNo ";
                    //--Amount and SalesOrderNo is already set--
                    myCommand.ExecuteNonQuery();

                    //--Do not allow edit of detail when return is already finalized--
                    if (sPreviousStatus.ToLower() == "for request")
                    {
                        //--Update Details of Returns and SO--
                        myCommand.CommandText = "SELECT PID, Quantity FROM a_return_detail WHERE ReturnID=@ReturnID";
                        //--ReturnID is already set--
                        myDataReader = myCommand.ExecuteReader();
                        Queue<String> qPID = new Queue<string>();
                        Queue<int> qQuantity = new Queue<int>();
                        while (myDataReader.Read())
                        {
                            qPID.Enqueue(myDataReader.GetString(0));
                            qQuantity.Enqueue(myDataReader.GetInt32(1));
                        }
                        myDataReader.Close();

                        //--Update Sales order details--
                        myCommand.CommandText = "UPDATE a_sales_order_detail SET ReturnQuantity=ReturnQuantity-@Quantity " +
                                                "WHERE PID=@PID AND SalesOrderNo=@SalesOrderNo";
                        //--Sales Order No Is already set--
                        myCommand.Parameters.AddWithValue("@Quantity", 0);
                        myCommand.Parameters.AddWithValue("@PID", "");
                        while (qPID.Count > 0 && qQuantity.Count > 0)
                        {
                            myCommand.Parameters["@Quantity"].Value = qQuantity.Dequeue();
                            myCommand.Parameters["@PID"].Value = qPID.Dequeue();
                            myCommand.ExecuteNonQuery();
                        }

                        qPID = null;
                        qQuantity = null;

                        //--Delete all previous details--
                        myCommand.CommandText = "DELETE FROM a_return_detail WHERE ReturnID=@ReturnID";
                        //--ReturnId is already set--
                        myCommand.ExecuteNonQuery();

                        //--Add New Details--
                        Boolean blnAddToInventory = false;
                        if (myPage.cmbStatus.Text.Trim().ToLower() == "accounted")
                        {
                            blnAddToInventory = true;

                            //--Get Alteration ID--
                            myCommand.CommandText = "SELECT MAX(AlterID) + 1 FROM a_alteration";
                            object temp = myCommand.ExecuteScalar();
                            int MaxID;
                            if (temp == System.DBNull.Value)
                            {
                                MaxID = 1000;
                            }
                            else
                            {
                                MaxID = (int)temp;
                            }

                            //--Get alter reason--
                            myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='Inventory' AND LookUpName='Product Returns'";
                            int iAlterReasonID = int.Parse(myCommand.ExecuteScalar().ToString());

                            //--Insert Alteration Header--
                            myCommand.CommandText = "INSERT INTO a_alteration (AlterID, TransactionDate, ReferenceNo, ReferenceDate, AlterReason, Remarks, EmployeeID) " +
                                                    "VALUES (@AlterID, @TransactionDateAlter, @ReferenceNo, @ReferenceDate, @AlterReason, @Remarks, @EmployeeID)";
                            myCommand.Parameters.AddWithValue("@AlterID", MaxID);
                            myCommand.Parameters.AddWithValue("@TransactionDateAlter", DateTime.Now);
                            myCommand.Parameters.AddWithValue("@ReferenceNo", myPage.txtReturnSlipNo.Text);
                            myCommand.Parameters.AddWithValue("@ReferenceDate", myPage.dtpTransactionDate.Value);
                            myCommand.Parameters.AddWithValue("@AlterReason", iAlterReasonID);
                            //--remarks is already set--
                            //--employee ID is already set--
                            myCommand.ExecuteNonQuery();
                        }

                        int dPIDReturnQuantity = 0;
                        for (int iCtr = 0; iCtr < myPage.dgvSODetails.Rows.Count; iCtr++)
                        {
                            if (int.TryParse(myPage.dgvSODetails.Rows[iCtr].Cells[7].Value.ToString(), out dPIDReturnQuantity))
                            {
                                if (dPIDReturnQuantity > 0)
                                {
                                    myCommand.CommandText = "INSERT INTO a_return_detail (ReturnID, PID, Quantity, Amount) " +
                                                            "VALUES (@ReturnID, @PID, @Quantity, @Amount) ";
                                    //--Return ID is Already Set--
                                    myCommand.Parameters["@PID"].Value = myPage.dgvSODetails.Rows[iCtr].Cells[0].Value.ToString();
                                    myCommand.Parameters["@Quantity"].Value = dPIDReturnQuantity;
                                    myCommand.Parameters["@Amount"].Value = double.Parse(myPage.dgvSODetails.Rows[iCtr].Cells[4].Value.ToString());
                                    myCommand.ExecuteNonQuery();

                                    //--Update Sales Order Details--
                                    myCommand.CommandText = "UPDATE a_sales_order_detail SET ReturnQuantity = ReturnQuantity + @Quantity " +
                                                            "WHERE SalesOrderNo=@SalesOrderNo AND PID=@PID ";
                                    //--Quantity, Sales Order No, PID are already set--
                                    myCommand.ExecuteNonQuery();

                                    if (blnAddToInventory)
                                    {
                                        myCommand.CommandText = "INSERT INTO a_alteration_detail (AlterID, PID, Quantity) " +
                                                                "VALUES(@AlterID, @PID, @Quantity) ";
                                        //--Alter ID, PID, Quantity are already set--
                                        myCommand.ExecuteNonQuery();

                                        myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock + @Quantity WHERE PID=@PID ";
                                        //--Quantity and PID are already set--
                                        myCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                   
                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Product Return Information was SUCCESSFULLY EDITED.";
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

        public int deleteReturns(fReturnEdit myPage)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
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

                //--Required Field: Slip No--
                if (myPage.getReturnID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Slip No");
                    }
                    myPage.txtMessage.Text += "Provide a value for Slip No field; \n\r";
                    hasError = true;
                }

                //--Get Previous Status: Cannot Delete 'Accounted' Returns--
                myCommand.CommandText = "SELECT LookUpName FROM c_look_up " +
                                        "WHERE LookUpDiv='Return' AND ValueId = (" +
                                        "      SELECT status FROM a_return WHERE ReturnID=@ReturnID)";
                myCommand.Parameters.AddWithValue("@ReturnID", myPage.getReturnID());
                sPreviousStatus = myCommand.ExecuteScalar().ToString();
                if (sPreviousStatus.ToLower() == "accounted")
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(19);
                        myPage.addMessageParameter("return");
                        myPage.addMessageParameter("ACCOUNTED");
                    }
                    myPage.txtMessage.Text += "Can no longer delete an ACCOUNTED return; \n\r";
                    hasError = true;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Update Sales Order--
                    myCommand.CommandText = "UPDATE a_sales_order SET ReturnAmount = ReturnAmount - (SELECT Amount FROM a_return WHERE ReturnID=@ReturnID) " +
                                            "WHERE SalesOrderNo=@SalesOrderNo ";
                    myCommand.Parameters.AddWithValue("@ReturnID", myPage.getReturnID());
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.lblSalesOrderNo.Text.Trim());
                    myCommand.ExecuteNonQuery();

                    //--Update Details of Returns and SO--
                    myCommand.CommandText = "SELECT PID, Quantity FROM a_return_detail WHERE ReturnID=@ReturnID";
                    //--ReturnID is already set--
                    myDataReader = myCommand.ExecuteReader();
                    Queue<String> qPID = new Queue<string>();
                    Queue<int> qQuantity = new Queue<int>();
                    while (myDataReader.Read())
                    {
                        qPID.Enqueue(myDataReader.GetString(0));
                        qQuantity.Enqueue(myDataReader.GetInt32(1));
                    }
                    myDataReader.Close();

                    //--Update Sales order details--
                    myCommand.CommandText = "UPDATE a_sales_order_detail SET ReturnQuantity=ReturnQuantity-@Quantity " +
                                            "WHERE PID=@PID AND SalesOrderNo=@SalesOrderNo";
                    //--Sales Order No Is already set--
                    myCommand.Parameters.AddWithValue("@Quantity", 0);
                    myCommand.Parameters.AddWithValue("@PID", "");
                    while (qPID.Count > 0 && qQuantity.Count > 0)
                    {
                        myCommand.Parameters["@Quantity"].Value = qQuantity.Dequeue();
                        myCommand.Parameters["@PID"].Value = qPID.Dequeue();
                        myCommand.ExecuteNonQuery();
                    }

                    qPID = null;
                    qQuantity = null;

                    //--Delete all previous details--
                    myCommand.CommandText = "DELETE FROM a_return_detail WHERE ReturnID=@ReturnID";
                    //--ReturnId is already set--
                    myCommand.ExecuteNonQuery();

                    //--Delete Header--
                    myCommand.CommandText = "DELETE FROM a_return WHERE ReturnID=@ReturnID";
                    //--ReturnId is already set--
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Product Return Information was SUCCESSFULLY DELETED.";
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
