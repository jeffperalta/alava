using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;


namespace AlavaSoft.Transaction.Payment
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

        public int addPayment(fPayment myPage, int EmployeeID) 
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
            double dTotalApplied = 0.00, dTotalPayment = 0.00, dCash = 0.00;

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

                //--Required Field: ReceiptNo--
                if (myPage.txtReceiptNo.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Receipt No");
                    }
                    myPage.txtMessage.Text += "Provide a value for RECEIPT NO field; \n\r";
                    hasError = true;
                }

                //--Sales Order List Is Required--
                Boolean hasSalesOrder = false;
                for (int iCtr = 0; iCtr < myPage.dgvSalesOrder.Rows.Count; iCtr++)
                {
                    if (myPage.dgvSalesOrder.Rows[iCtr].Visible)
                    {
                        hasSalesOrder = true;
                        break;
                    }
                }

                if (!hasSalesOrder)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Sales Order");
                    }
                    myPage.txtMessage.Text += "Include a SALES ORDER for payment; \n\r";
                    hasError = true;
                }

                //--Cash amount must be numeric--
                dCash = 0.00;
                if (myPage.txtCash.Text.Trim().Length != 0 && !double.TryParse(myPage.txtCash.Text.Trim(), out dCash))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(15);
                        myPage.addMessageParameter("Cash");
                    }
                    myPage.txtMessage.Text += "Cash field must be a POSITIVE NUMBER; \n\r";
                    hasError = true;
                }

                //--Cash amount should be greater than zero--
                if (dCash < 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(15);
                        myPage.addMessageParameter("Cash");
                    }
                    myPage.txtMessage.Text += "Cash field must be a POSITIVE NUMBER; \n\r";
                    hasError = true;
                }

                //--SO Applied amount must be equal to total Payment--
                double.TryParse(myPage.lblTotalApplied.Text.Trim(), out dTotalApplied);
                double.TryParse(myPage.lblTotalPayment.Text.Trim(), out dTotalPayment);
                if (dTotalPayment != dTotalApplied)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(14);
                    }

                    if (dTotalPayment > dTotalApplied) myPage.txtMessage.Text += "Exact payment is needed, CHANGE is not entered in the system; \n\r";
                    else myPage.txtMessage.Text += "Additional payment details is needed to tally with the total SO amount; \n\r";
                    
                    hasError = true;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Customer ID--
                    myCommand.CommandText = "SELECT MAX(PaymentID) + 1 FROM a_payment";
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

                    //--Insert PAYMENT information--
                    myCommand.CommandText = "INSERT INTO a_payment (PaymentID, Amount, TransactionDate, PaidBy, ReceivedBy, EmployeeID, ReceiptNo, Void) " +
                                            "VALUES(@PaymentID, @Amount, @TransactionDate, @PaidBy, @ReceivedBy, @EmployeeID, @ReceiptNo, @Void) ";
                    myCommand.Parameters.AddWithValue("@PaymentID", MaxID);
                    myCommand.Parameters.AddWithValue("@Amount", double.Parse(myPage.lblTotalPayment.Text.Trim()));
                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@PaidBy", myPage.txtPaidBy.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ReceivedBy", myPage.txtReceivedBy.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myCommand.Parameters.AddWithValue("@ReceiptNo", myPage.txtReceiptNo.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Void", 0);
                    myResult += myCommand.ExecuteNonQuery();

                    //--Set Interface Payment ID: Will be used in the payment receipt printout--
                    myPage.setPaymentID(MaxID);

                    //--Select SO paid status index number--
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='SalesOrder' AND LookUpName='Paid'";
                    int iStatusPaid = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Insert Payment Details--
                    myCommand.Parameters.AddWithValue("@SalesOrderNo","");
                    myCommand.Parameters.AddWithValue("@Status", iStatusPaid);
                    for (int iCtr = 0; iCtr < myPage.dgvSalesOrder.Rows.Count; iCtr++)
                    {
                        if (myPage.dgvSalesOrder.Rows[iCtr].Visible)
                        {
                            myCommand.CommandText = "INSERT INTO a_payment_detail (PaymentID, SalesOrderNo, Amount) " +
                                                    "VALUES(@PaymentID, @SalesOrderNo, @Amount)";
                            //--PaymentID is already set--
                            myCommand.Parameters["@SalesOrderNo"].Value = myPage.dgvSalesOrder.Rows[iCtr].Cells[0].Value.ToString();
                            myCommand.Parameters["@Amount"].Value = double.Parse(myPage.dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString());
                            myResult += myCommand.ExecuteNonQuery();

                            //--Update Sales Order No--
                            myCommand.CommandText = "UPDATE a_sales_order SET PaidAmount = PaidAmount + @Amount " +
                                                    "WHERE SalesOrderNo=@SalesOrderNo";
                            myCommand.ExecuteNonQuery();

                            //--Update Status of Sales Order To Paid--
                            myCommand.CommandText = "UPDATE a_sales_order SET Status=@Status WHERE SalesOrderNo=@SalesOrderNo AND PaidAmount > (Amount - ReturnAmount)";
                            myCommand.ExecuteNonQuery();
                        }
                    }

                    //--Insert Payment Type Details: Check--
                    myCommand.Parameters.AddWithValue("@PayTypeDetailID", 0);
                    myCommand.Parameters.AddWithValue("@PaymentType", "");

                    myCommand.Parameters.AddWithValue("@CheckID", 0);
                    myCommand.Parameters.AddWithValue("@CheckNo", "");
                    myCommand.Parameters.AddWithValue("@CheckBank", "");
                    myCommand.Parameters.AddWithValue("@MaturityDate", "");
                    myCommand.Parameters.AddWithValue("@CheckType", "");

                    for (int iCtr=0; iCtr < myPage.dgvPaymentType.Rows.Count; iCtr++)
                    {
                        myCommand.CommandText = "SELECT MAX(PayTypeDetailID) + 1 FROM a_payment_type_detail";
                        temp = myCommand.ExecuteScalar();
                        if (temp == System.DBNull.Value)
                        {
                            MaxID = 1000;
                        }
                        else
                        {
                            MaxID = (int)temp;
                        }

                        myCommand.CommandText = "INSERT INTO a_payment_type_detail (PayTypeDetailID, PaymentID, Amount, PaymentType) " +
                                                "VALUES(@PayTypeDetailID, @PaymentID, @Amount, @PaymentType) ";
                        //--PaymentID is already set--
                        myCommand.Parameters["@PayTypeDetailID"].Value = MaxID;
                        myCommand.Parameters["@Amount"].Value = double.Parse(myPage.dgvPaymentType.Rows[iCtr].Cells[4].Value.ToString());
                        myCommand.Parameters["@PaymentType"].Value = "CHECK";
                        myResult += myCommand.ExecuteNonQuery();

                        myCommand.CommandText = "SELECT MAX(CheckID) + 1 FROM a_check_payment";
                        temp = myCommand.ExecuteScalar();
                        if (temp == System.DBNull.Value)
                        {
                            MaxID = 1000;
                        }
                        else
                        {
                            MaxID = (int)temp;
                        }

                        myCommand.CommandText = "INSERT INTO a_check_payment (CheckID, PayTypeID, CheckNo, CheckBank, MaturityDate, CheckType) " +
                                                "VALUES(@CheckID, @PayTypeDetailID, @CheckNo, @CheckBank, @MaturityDate, @CheckType)";
                        myCommand.Parameters["@CheckID"].Value = MaxID;
                        //--PayTypeDetailID is already set--
                        myCommand.Parameters["@CheckNo"].Value = myPage.dgvPaymentType.Rows[iCtr].Cells[0].Value.ToString();
                        myCommand.Parameters["@CheckBank"].Value = myPage.dgvPaymentType.Rows[iCtr].Cells[1].Value.ToString();
                        myCommand.Parameters["@MaturityDate"].Value = myPage.dgvPaymentType.Rows[iCtr].Cells[3].Value.ToString();
                        myCommand.Parameters["@CheckType"].Value = myPage.dgvPaymentType.Rows[iCtr].Cells[2].Value.ToString();
                        myResult += myCommand.ExecuteNonQuery();

                    }

                    //--Insert Payment: CASH--
                    if (dCash > 0)
                    {
                        myCommand.CommandText = "SELECT MAX(PayTypeDetailID) + 1 FROM a_payment_type_detail";
                        temp = myCommand.ExecuteScalar();
                        if (temp == System.DBNull.Value)
                        {
                            MaxID = 1000;
                        }
                        else
                        {
                            MaxID = (int)temp;
                        }

                        myCommand.CommandText = "INSERT INTO a_payment_type_detail (PayTypeDetailID, PaymentID, Amount, PaymentType) " +
                                                "VALUES(@PayTypeDetailID, @PaymentID, @Amount, @PaymentType) ";
                        //--PaymentID is already set--
                        myCommand.Parameters["@PayTypeDetailID"].Value = MaxID;
                        myCommand.Parameters["@Amount"].Value = dCash;
                        myCommand.Parameters["@PaymentType"].Value = "CASH";
                        myResult += myCommand.ExecuteNonQuery();
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Payment Information was SUCCESSFULLY SAVED.";
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
                    myPage.setPaymentID(0);
                }

                #endregion

            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
                myResult = 0;
                myPage.setPaymentID(0);
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

        public void showPayment(fPaymentVoid myPage)
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
                myPage.txtMessage.Text = "";
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Retrieve Payment Header Information--
                    myCommand.CommandText = "SELECT ReceiptNo, TransactionDate, PaidBy, ReceivedBy " + 
                                            "FROM a_payment " +
                                            "WHERE PaymentID=@PaymentID ";
                    myCommand.Parameters.AddWithValue("@PaymentID", myPage.getPaymentID());
                    myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read())
                    {
                        myPage.lblReceiptNo.Text = myDataReader.GetString(0);
                        myPage.lblTransactionDate.Text = myDataReader.GetDateTime(1).ToString();
                        myPage.lblPaidBy.Text = myDataReader.GetString(2);
                        myPage.lblReceivedBy.Text = myDataReader.GetString(3);
                    }
                    myDataReader.Close();

                    //--Get SO details--
                    myCommand.CommandText = "SELECT a_sales_order.SalesOrderNo, a_customer.CompanyName, a_sales_order.TransactionDate, a_sales_order.Amount-a_sales_order.ReturnAmount, a_sales_order.Amount-a_sales_order.ReturnAmount-a_sales_order.PaidAmount, a_payment_detail.Amount " +
                                            "FROM   a_payment_detail INNER JOIN a_sales_order ON a_payment_detail.SalesOrderNo = a_sales_order.SalesOrderNo INNER JOIN a_customer ON a_sales_order.CustomerID = a_customer.CustomerID " +
                                            "WHERE  a_payment_detail.PaymentID=@PaymentID";
                    myDataReader = myCommand.ExecuteReader();
                    double dBillingAmount = 0, dBalance = 0, dAmountApplied = 0;
                    while (myDataReader.Read())
                    {
                        myPage.addSalesOrder(myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetDateTime(2).ToString(), double.Parse(myDataReader.GetDecimal(3).ToString()), double.Parse(myDataReader.GetDecimal(4).ToString()), double.Parse(myDataReader.GetDecimal(5).ToString()));
                        dBillingAmount += double.Parse(myDataReader.GetDecimal(3).ToString());
                        dBalance += double.Parse(myDataReader.GetDecimal(4).ToString());
                        dAmountApplied += double.Parse(myDataReader.GetDecimal(5).ToString());
                    }
                    myPage.lblTotalSOAmount.Text = dBillingAmount.ToString("n");
                    myPage.lblTotalBalance.Text = dBalance.ToString("n");
                    myPage.lblTotalApplied.Text = dAmountApplied.ToString("n");
                    myDataReader.Close();

                    //--Get SO Payment Details: CHECK--
                    myCommand.CommandText = "SELECT a_check_payment.CheckNo, a_check_payment.CheckBank, a_check_payment.CheckType, a_check_payment.MaturityDate, " +
                                            "       a_payment_type_detail.Amount " +
                                            "FROM   a_payment_type_detail INNER JOIN " +
                                            "       a_check_payment ON a_payment_type_detail.PayTypeDetailID = a_check_payment.PayTypeID " +
                                            "WHERE  (a_payment_type_detail.PaymentType = 'CHECK') AND (a_payment_type_detail.PaymentID = @PaymentID) ";
                    myDataReader = myCommand.ExecuteReader();
                    double dPayment = 0.00;
                    while (myDataReader.Read())
                    {
                        myPage.dgvPaymentType.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetDateTime(3).ToString(), myDataReader.GetDecimal(4));
                        dPayment += double.Parse(myDataReader.GetDecimal(4).ToString());
                    }
                    myDataReader.Close();

                    //--Get SO Payment Details: CASH--
                    myCommand.CommandText = "SELECT Amount " + 
                                            "FROM a_payment_type_detail " +
                                            "WHERE a_payment_type_detail.PaymentType = 'CASH' AND a_payment_type_detail.PaymentID = @PaymentID";
                    myDataReader = myCommand.ExecuteReader();
                    myDataReader.Read();
                    myPage.lblCash.Text = double.Parse(myDataReader.GetDecimal(0).ToString()).ToString("n");
                    dPayment += double.Parse(myDataReader.GetDecimal(0).ToString());
                    myPage.lblTotalPayment.Text = dPayment.ToString("n");
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

        public int voidPayment(fPaymentVoid myPage)
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

                //--Required Field: Receipt No--
                if (myPage.getPaymentID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Receipt No");
                    }
                    myPage.txtMessage.Text += "Provide a value for RECEIPT NO field; \n\r";
                    hasError = true;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Void Payment Transaction--
                    myCommand.CommandText = "UPDATE a_payment SET Void=1 WHERE PaymentID=@PaymentID";
                    myCommand.Parameters.AddWithValue("@PaymentID", myPage.getPaymentID());
                    myResult = myCommand.ExecuteNonQuery();

                    myCommand.CommandText = "SELECT SalesOrderNo, Amount FROM a_payment_detail WHERE PaymentID=@PaymentID";
                    Queue<string> qSalesOrder = new Queue<string>();
                    Queue<double> qAmount = new Queue<double>();
                    myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read())
                    {
                        qSalesOrder.Enqueue(myDataReader.GetString(0));
                        qAmount.Enqueue(double.Parse(myDataReader.GetDecimal(1).ToString()));
                    }
                    myDataReader.Close();

                    //--Get Status ID: for collection--
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpName='For Collection' AND LookUpDiv='SalesOrder'";
                    int iForCollection = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Get Status ID: paid--
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpName='Paid' AND LookUpDiv='SalesOrder'";
                    int iPaid = int.Parse(myCommand.ExecuteScalar().ToString());


                    //--Update Paid Amount and Status of Sales Orders affected--
                    myCommand.Parameters.AddWithValue("@Amount", 0.00);
                    myCommand.Parameters.AddWithValue("@SalesOrderNo", "");
                    myCommand.Parameters.AddWithValue("@StatusForCollection", iForCollection);
                    myCommand.Parameters.AddWithValue("@StatusPaid", iPaid);
                    while (qSalesOrder.Count > 0 && qAmount.Count > 0)
                    {
                        myCommand.CommandText = "UPDATE a_sales_order SET PaidAmount=PaidAmount-@Amount " + 
                                                "WHERE SalesOrderNo=@SalesOrderNo";
                        myCommand.Parameters["@Amount"].Value = qAmount.Dequeue();
                        myCommand.Parameters["@SalesOrderNo"].Value = qSalesOrder.Dequeue();
                        myCommand.ExecuteNonQuery();

                        myCommand.CommandText = "UPDATE a_sales_order SET Status=@StatusForCollection " + 
                                                "WHERE SalesOrderNo=@SalesOrderNo AND (Amount-ReturnAmount) > PaidAmount AND Status=@StatusPaid";
                        myCommand.ExecuteNonQuery();
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Payment was SUCCESSFULLY VOID.";
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
