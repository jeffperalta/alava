using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace AlavaSoft.Transaction.Customer
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

        public int addCustomer(fCustomerAdd myPage) 
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0, iTerms =0;
            double dDiscount = 0;

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

                //--Required Field: Customer Name--
                if (myPage.txtCustomerName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Customer name");
                    }
                    myPage.txtMessage.Text += "Provide a value for CUSTOMER/COMPANY NAME field; \n\r";
                    hasError = true;
                }

                //--Unique Field: UserName--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_customer WHERE CompanyName=@CompanyName; ";
                myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtCustomerName.Text.Trim());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Customer name");
                    }
                    myPage.txtMessage.Text += "Change the value of the CUSTOMER/COMPANY Name to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                //--Required Field: Terms(days)--
                if (myPage.txtTerms.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Terms(days)");
                    }
                    myPage.txtMessage.Text += "Provide a numeric value for TERMS(DAYS) field; \n\r";
                    hasError = true;
                }

                //--Positive Numeric Value: Terms(days)--
                if (myPage.txtTerms.Text.Trim().Length != 0 && !int.TryParse(myPage.txtTerms.Text.Trim(), out iTerms))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Terms(days)");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for TERMS(DAYS) field; \n\r";
                    hasError = true;
                }
                else if (iTerms < 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Terms(days)");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for TERMS(DAYS) field; \n\r";
                    hasError = true;
                }

                //--Required Field: Discount(%)--
                if (myPage.txtDiscount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Discount");
                    }
                    myPage.txtMessage.Text += "Provide a numeric value for DISCOUNT field; \n\r";
                    hasError = true;
                }

                //--Positive Numeric Value: Discount--
                if (myPage.txtDiscount.Text.Trim().Length != 0 && !double.TryParse(myPage.txtDiscount.Text.Trim(), out dDiscount)) 
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Discount");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for DISCOUNT field; \n\r";
                    hasError = true;
                }
                else if (dDiscount < 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Discount)");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for DISCOUNT field; \n\r";
                    hasError = true;
                }


                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Customer ID--
                    myCommand.CommandText = "SELECT MAX(CustomerID) + 1 FROM a_customer";
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

                    //--Insert New Customer information--
                    myCommand.CommandText = "INSERT INTO a_customer (CustomerID, CompanyName, CompanyAddress, ContactName, ContactTitle, ContactPosition, Contact1, Contact2, Contact3, Terms, Discount, DeliveryAddress, Remarks) " +
                                            "VALUES (@CustomerID, @CompanyName, @CompanyAddress, @ContactName, @ContactTitle, @ContactPosition, @Contact1, @Contact2, @Contact3, @Terms, @Discount, @DeliveryAddress, @Remarks) ";
                    myCommand.Parameters.AddWithValue("@CustomerID", MaxID);
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtCustomerName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@CompanyAddress", myPage.txtCompanyAddress.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactName", myPage.txtContactName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactTitle", myPage.txtContactTitle.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactPosition", myPage.txtContactPosition.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact1", myPage.txtContact1.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact2", myPage.txtContact2.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact3", myPage.txtContact3.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Terms", iTerms);
                    myCommand.Parameters.AddWithValue("@Discount", dDiscount);
                    myCommand.Parameters.AddWithValue("@DeliveryAddress", myPage.txtDeliveryAddress.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Customer Information was SUCCESSFULLY SAVED.";
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
                }

                #endregion
            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
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

        public int editCustomer(fCustomerEdit myPage)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0, iTerms = 0;
            double dDiscount = 0;

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

                //--Required Field: Customer Name--
                if (myPage.txtCustomerName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Customer name");
                    }
                    myPage.txtMessage.Text += "Provide a value for CUSTOMER/COMPANY NAME field; \n\r";
                    hasError = true;
                }

                //--Unique Field: UserName--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_customer WHERE CompanyName=@CompanyName AND CustomerID != @CustomerID; ";
                myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtCustomerName.Text.Trim());
                myCommand.Parameters.AddWithValue("@CustomerID", myPage.getCustomerID());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Customer name");
                    }
                    myPage.txtMessage.Text += "Change the value of the CUSTOMER/COMPANY Name to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                //--Required Field: Terms(days)--
                if (myPage.txtTerms.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Terms(days)");
                    }
                    myPage.txtMessage.Text += "Provide a numeric value for TERMS(DAYS) field; \n\r";
                    hasError = true;
                }

                //--Positive Numeric Value: Terms(days)--
                if (myPage.txtTerms.Text.Trim().Length != 0 && !int.TryParse(myPage.txtTerms.Text.Trim(), out iTerms))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Terms(days)");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for TERMS(DAYS) field; \n\r";
                    hasError = true;
                }
                else if (iTerms < 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Terms(days)");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for TERMS(DAYS) field; \n\r";
                    hasError = true;
                }

                //--Required Field: Discount(%)--
                if (myPage.txtDiscount.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Discount");
                    }
                    myPage.txtMessage.Text += "Provide a numeric value for DISCOUNT field; \n\r";
                    hasError = true;
                }

                //--Positive Numeric Value: Discount--
                if (myPage.txtDiscount.Text.Trim().Length != 0 && !double.TryParse(myPage.txtDiscount.Text.Trim(), out dDiscount))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Discount");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for DISCOUNT field; \n\r";
                    hasError = true;
                }
                else if (dDiscount < 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Discount)");
                        myPage.addMessageParameter("a positive numeric value");
                    }
                    myPage.txtMessage.Text += "Provide a positive numeric value value for DISCOUNT field; \n\r";
                    hasError = true;
                }


                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--UPDATE Customer information--
                    myCommand.CommandText = "UPDATE a_customer SET CompanyName=@CompanyName, CompanyAddress=@CompanyAddress, ContactName=@ContactName, ContactTitle=@ContactTitle, ContactPosition=@ContactPosition, Contact1=@Contact1, Contact2=@Contact2, Contact3=@Contact3, Terms=@Terms, Discount=@Discount, DeliveryAddress=@DeliveryAddress, Remarks=@Remarks WHERE CustomerID=@CustomerID ";
                    myCommand.Parameters.AddWithValue("@CustomerID", myPage.getCustomerID());
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtCustomerName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@CompanyAddress", myPage.txtCompanyAddress.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactName", myPage.txtContactName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactTitle", myPage.txtContactTitle.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactPosition", myPage.txtContactPosition.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact1", myPage.txtContact1.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact2", myPage.txtContact2.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact3", myPage.txtContact3.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Terms", iTerms);
                    myCommand.Parameters.AddWithValue("@Discount", dDiscount);
                    myCommand.Parameters.AddWithValue("@DeliveryAddress", myPage.txtDeliveryAddress.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Customer Information was SUCCESSFULLY EDITED.";
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
                }

                #endregion
            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
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

        public int deleteCustomer(fCustomerEdit myPage)
        {
            int myResult = 0;
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();

            try
            {
                #region Instantiation Section
                Boolean hasError = false;
                myTransaction = myConnection.BeginTransaction();
                myCommand = myConnection.CreateCommand();
                myCommand.Transaction = myTransaction;
                #endregion

                #region Check Input Section
                myPage.txtMessage.Text = "";
                //--Referenced Item--

                myCommand.CommandText = "SELECT 'EXIST' FROM a_sales_order WHERE CustomerID=@CustomerID; ";
                myCommand.Parameters.AddWithValue("@CustomerID", myPage.getCustomerID());
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Sales order; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                if (myPage.txtMessage.Text.Trim().Length > 0)
                {
                    myPage.txtMessage.Text = "CANNOT DELETE a referenced record. THIS CUSTOMER HAS: " + myPage.txtMessage.Text;
                }

                #endregion

                #region Process Section
                myCommand.Parameters.Clear();
                myCommand.CommandText = "DELETE FROM a_customer WHERE CustomerID=@CustomerID";
                myCommand.Parameters.AddWithValue("@CustomerID", myPage.getCustomerID());
                myResult = myCommand.ExecuteNonQuery();

                if (myResult > 0)
                {
                    myPage.setMessageNumber(7);
                    myPage.txtMessage.Text = "Customer Account Information was SUCCESSFULLY DELETED.";
                }
                else
                {
                    myPage.setMessageNumber(5);
                    myPage.txtMessage.Text = "This event was caused by the fact that there is no record that correspond to your input. To correct this, please select a valid record at the data grid.";
                }

                myTransaction.Commit();
                #endregion
            }
            catch (Exception e)
            {
                myPage.txtMessage.Text = "TRANSACTION WAS NOT SAVED: " + e.ToString();
                myTransaction.Rollback();
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
