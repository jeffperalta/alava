using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AlavaSoft.Transaction.Refund
{
    class sqlHelper
    {
        private SqlConnection myConnection = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataReader myDataReader = null;

        public int addRefund(fRefundAdd myPage, int EmployeeID)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
            double dRefundAmount = 0.00, dAmountReleased = 0.00;

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

                //--Required Field: Sales Order No--
                if (myPage.getSalesOrderNo().Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Sales Order No");
                    }
                    myPage.txtMessage.Text += "Provide a value for SALES ORDER NO field; \n\r";
                    hasError = true;
                }

                //--Required Field: Refund Amount--
                if (myPage.txtRefundAmount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Refund Amount");
                    }
                    myPage.txtMessage.Text += "Provide a value for REFUND AMOUNT field; \n\r";
                    hasError = true;
                }

                //--Field Must Be Numeric: Refund Amount--
                if (!double.TryParse(myPage.txtRefundAmount.Text.Trim(), out dRefundAmount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(15);
                        myPage.addMessageParameter("Refund Amount");
                    }
                    myPage.txtMessage.Text += "Change the REFUND AMOUNT to a positive number; \n\r";
                    hasError = true;
                }
                else
                {
                    //--Field Valid Range: Refund Amount > 0 AND RefundAmount <= TotalPayment - Previous Refund
                    if (!(dRefundAmount > 0 && dRefundAmount <= (myPage.getPaymentAmount() - myPage.getPreviousRefund())))
                    {
                        if (!hasError)
                        {     
                            myPage.setMessageNumber(11);
                            myPage.addMessageParameter("Refund Amount");
                            myPage.addMessageParameter(" less than or equal to " + (myPage.getPaymentAmount() - myPage.getPreviousRefund()).ToString("n"));
                        }
                        myPage.txtMessage.Text += "Value for REFUND AMOUNT must be BETWEEN 0 and " + (myPage.getPaymentAmount() - myPage.getPreviousRefund()).ToString("n") + "; \n\r";
                        hasError = true;
                    }
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

                //--Required Field if Status 'Released': Amount Released--
                if (myPage.cmbStatus.Text.Trim().ToLower() == "released" &&
                    myPage.txtAmountReleased.Text.Trim().Length == 0) 
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Amount Released");
                    }
                    myPage.txtMessage.Text += "Provide a value for AMOUNT RELEASED field; \n\r";
                    hasError = true;
                }

                //--Field Must Be Numeric: Amount Released--
                if (!double.TryParse(myPage.txtAmountReleased.Text.Trim(), out dAmountReleased))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(15);
                        myPage.addMessageParameter("Amount Released");
                    }
                    myPage.txtMessage.Text += "Change the AMOUNT RELEASED to a positive number; \n\r";
                    hasError = true;
                }
                else
                {
                    if (!(dAmountReleased >= 0 && dAmountReleased <= dRefundAmount)) 
                    {
                        if (!hasError)
                        {//"%0 is invalid; Entry must be%1",  //Message#11        
                            myPage.setMessageNumber(11);
                            myPage.addMessageParameter("Amount Released");
                            myPage.addMessageParameter(" less than or equal to the Refund Amount");
                        }
                        myPage.txtMessage.Text += "Value for AMOUNT RELEASED must be less or equal to the Refund Amount; \n\r"; 
                        hasError = true;
                    }
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Next Refund ID--
                    myCommand.CommandText = "SELECT MAX(RefundID) + 1 FROM a_refund";
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

                    //--Generate refund slip no--
                    myCommand.CommandText = "SELECT cnt FROM c_key_gen WHERE Module='REFUND'";
                    int iRefundSlipNo = int.Parse(myCommand.ExecuteScalar().ToString());
                    myCommand.CommandText = "UPDATE c_key_gen SET cnt=cnt+1 WHERE Module='REFUND'";
                    myCommand.ExecuteNonQuery();

                    //--Update Interface with Refund Slip No--
                    myPage.txtRefundSlipNo.Text = "REFUND-" + iRefundSlipNo;

                    //--Insert New Refund Information--
                    myCommand.CommandText = "INSERT INTO a_refund (RefundID, RefundDate, Amount, AmountReleased, ReceivedBy, EmployeeID, Remarks, SalesOrderNo, RefundSlipNo, Status) " +
                                            "VALUES (@RefundID, @RefundDate, @Amount, @AmountReleased, @ReceivedBy, @EmployeeID, @Remarks, @SalesOrderNo, @RefundSlipNo, @Status) ";
                    myCommand.Parameters.AddWithValue("@RefundID", MaxID);
                    myCommand.Parameters.AddWithValue("@RefundDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@Amount", double.Parse(myPage.txtRefundAmount.Text.Trim()));
                    myCommand.Parameters.AddWithValue("@AmountReleased", double.Parse(myPage.txtAmountReleased.Text.Trim()));
                    myCommand.Parameters.AddWithValue("@ReceivedBy", myPage.txtReceivedBy.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.getSalesOrderNo());
                    myCommand.Parameters.AddWithValue("@RefundSlipNo", myPage.txtRefundSlipNo.Text.Trim());
                    if (myPage.cmbStatus.Text.Trim().ToLower() == "released")
                    {
                        myCommand.Parameters.AddWithValue("@Status", 1);
                    }
                    else if (myPage.cmbStatus.Text.Trim().ToLower() == "unreleased")
                    {
                        myCommand.Parameters.AddWithValue("@Status", 0);
                    }
                    myResult = myCommand.ExecuteNonQuery();

                    //--Update Sales Order: RefundAmount--
                    myCommand.CommandText = "UPDATE a_sales_order SET RefundAmount = RefundAmount + @Amount WHERE SalesOrderNo=@SalesOrderNo";
                    myCommand.ExecuteNonQuery();

                    //--General Rule: Refund Amount Must Not Excede Total Payment--
                    myCommand.CommandText = "SELECT 'EXIST' FROM a_sales_order WHERE SalesOrderNo=@SalesOrderNo AND PaidAmount < RefundAmount";
                    myDataReader = myCommand.ExecuteReader();
                    myDataReader.Read();
                    if (myDataReader.HasRows)
                    {
                        myDataReader.Close();
                        myPage.setMessageNumber(18);
                        myPage.txtMessage.Text = "Concurrent transactions affect the Total Payment Amount (Void Payment, Payment). Reload refund info to refresh.";
                        myTransaction.Rollback();
                        myResult = 0;
                    }
                    else
                    {
                        myDataReader.Close();
                        if (myResult > 0)
                        {
                            myPage.setMessageNumber(7);
                            myPage.txtMessage.Text = "New Refund Information was SUCCESSFULLY SAVED.";
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

        public int editRefund(fRefundEdit myPage, int EmployeeID)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
            double dRefundAmount = 0.00, dAmountReleased = 0.00;

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

                //--Required Field: refund id--
                if (myPage.getRefundID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Refund Slip No");
                    }
                    myPage.txtMessage.Text += "Provide a value for REFUND SLIP NO field; \n\r";
                    hasError = true;
                }

                //--Required Field: Refund Amount--
                if (myPage.txtRefundAmount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Refund Amount");
                    }
                    myPage.txtMessage.Text += "Provide a value for REFUND AMOUNT field; \n\r";
                    hasError = true;
                }

                //--Field Must Be Numeric: Refund Amount--
                if (!double.TryParse(myPage.txtRefundAmount.Text.Trim(), out dRefundAmount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(15);
                        myPage.addMessageParameter("Refund Amount");
                    }
                    myPage.txtMessage.Text += "Change the REFUND AMOUNT to a positive number; \n\r";
                    hasError = true;
                }
                else
                {
                    //--Field Valid Range: Refund Amount > 0 AND RefundAmount <= (TotalPayment - SOamount - Allrefund) + MyRefund
                    if (!(dRefundAmount > 0 && dRefundAmount <= myPage.getExcess() + myPage.getInitialAmount()))
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(11);
                            myPage.addMessageParameter("Refund Amount");
                            myPage.addMessageParameter(" less than or equal to " + (myPage.getExcess() + myPage.getInitialAmount()).ToString("n"));
                        }
                        myPage.txtMessage.Text += "Value for REFUND AMOUNT must be BETWEEN 0 and " + (myPage.getExcess() + myPage.getInitialAmount()).ToString("n") + "; \n\r";
                        hasError = true;
                    }
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

                //--Required Field if Status 'Released': Amount Released--
                if (myPage.cmbStatus.Text.Trim().ToLower() == "released" &&
                    myPage.txtAmountReleased.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Amount Released");
                    }
                    myPage.txtMessage.Text += "Provide a value for AMOUNT RELEASED field; \n\r";
                    hasError = true;
                }

                //--Field Must Be Numeric: Amount Released--
                if (!double.TryParse(myPage.txtAmountReleased.Text.Trim(), out dAmountReleased))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(15);
                        myPage.addMessageParameter("Amount Released");
                    }
                    myPage.txtMessage.Text += "Change the AMOUNT RELEASED to a positive number; \n\r";
                    hasError = true;
                }
                else
                {
                    if (!(dAmountReleased >= 0 && dAmountReleased <= dRefundAmount))
                    {
                        if (!hasError)
                        {      
                            myPage.setMessageNumber(11);
                            myPage.addMessageParameter("Amount Released");
                            myPage.addMessageParameter(" less than or equal to the Refund Amount");
                        }
                        myPage.txtMessage.Text += "Value for AMOUNT RELEASED must be less or equal to the Refund Amount; \n\r";
                        hasError = true;
                    }
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Update Sales Order: Previous RefundAmount--
                    myCommand.CommandText = "UPDATE a_sales_order SET RefundAmount = RefundAmount - @Amount WHERE SalesOrderNo=@SalesOrderNo";
                    myCommand.Parameters.AddWithValue("@Amount", myPage.getInitialAmount());
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.lblSONo.Text.Trim());
                    myCommand.ExecuteNonQuery();

                    //--Insert New Refund Information--
                    myCommand.CommandText = "UPDATE a_refund SET " +
                                            "       RefundDate=@RefundDate, Amount=@Amount, " +
                                            "       AmountReleased=@AmountReleased, ReceivedBy=@ReceivedBy, EmployeeID=@EmployeeID, " +
                                            "       Remarks=@Remarks, Status=@Status " +
                                            "WHERE  RefundID=@RefundID";
                    myCommand.Parameters.AddWithValue("@RefundID", myPage.getRefundID());
                    myCommand.Parameters.AddWithValue("@RefundDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters["@Amount"].Value =  double.Parse(myPage.txtRefundAmount.Text.Trim());
                    myCommand.Parameters.AddWithValue("@AmountReleased", double.Parse(myPage.txtAmountReleased.Text.Trim()));
                    myCommand.Parameters.AddWithValue("@ReceivedBy", myPage.txtReceivedBy.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    if (myPage.cmbStatus.Text.Trim().ToLower() == "released")
                    {
                        myCommand.Parameters.AddWithValue("@Status", 1);
                    }
                    else if (myPage.cmbStatus.Text.Trim().ToLower() == "unreleased")
                    {
                        myCommand.Parameters.AddWithValue("@Status", 0);
                    }
                    myResult = myCommand.ExecuteNonQuery();

                    //--Update Sales Order: RefundAmount--
                    myCommand.CommandText = "UPDATE a_sales_order SET RefundAmount = RefundAmount + @Amount WHERE SalesOrderNo=@SalesOrderNo";
                    myCommand.ExecuteNonQuery();

                    //--General Rule: Refund Amount Must Not Excede Total Payment--
                    myCommand.CommandText = "SELECT 'EXIST' FROM a_sales_order WHERE SalesOrderNo=@SalesOrderNo AND PaidAmount < RefundAmount";
                    myDataReader = myCommand.ExecuteReader();
                    myDataReader.Read();
                    if (myDataReader.HasRows)
                    {
                        myDataReader.Close();
                        myPage.setMessageNumber(18);
                        myPage.txtMessage.Text = "Concurrent transactions affect the Total Payment Amount (Void Payment, Payment). Reload refund info to refresh.";
                        myTransaction.Rollback();
                        myResult = 0;
                    }
                    else
                    {
                        myDataReader.Close();
                        if (myResult > 0)
                        {
                            myPage.setMessageNumber(7);
                            myPage.txtMessage.Text = "Refund Information was SUCCESSFULLY EDITED.";
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

        public int deleteRefund(fRefundEdit myPage)
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

                //--Required Field: refund id--
                if (myPage.getRefundID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Refund Slip No");
                    }
                    myPage.txtMessage.Text += "Provide a value for REFUND SLIP NO field; \n\r";
                    hasError = true;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Update SO: RefundAmount--
                    myCommand.CommandText = "UPDATE a_sales_order SET RefundAmount=RefundAmount-@Amount WHERE SalesOrderNo=@SalesOrderNo";
                    myCommand.Parameters.AddWithValue("@Amount", myPage.getInitialAmount());
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", myPage.lblSONo.Text.Trim());
                    myCommand.ExecuteNonQuery();

                    //--DELETE Refund Information--
                    myCommand.CommandText = "DELETE FROM a_refund WHERE RefundID=@RefundID";
                    myCommand.Parameters.AddWithValue("@RefundID", myPage.getRefundID());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Refund Information was SUCCESSFULLY DELETED.";
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
