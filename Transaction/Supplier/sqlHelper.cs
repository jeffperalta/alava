using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AlavaSoft.Transaction.Supplier
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

        public int addSupplier(fSupplierAdd myPage)
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

                //--Required Field: Supplier Name--
                if (myPage.txtSupplierName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Supplier Name");
                    }
                    myPage.txtMessage.Text += "Provide a value for SUPPLIER NAME field; \n\r";
                    hasError = true;
                }

                //--Unique Field: Supplier Name--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_supplier WHERE CompanyName=@CompanyName; ";
                myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtSupplierName.Text.Trim());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Supplier Name");
                    }
                    myPage.txtMessage.Text += "Change the value of the SUPPLIER NAME to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Insert new product information--
                    myCommand.CommandText = "SELECT MAX(SupplierID)+1 FROM a_supplier ";
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

                    myCommand.CommandText = "INSERT INTO a_supplier (SupplierID, CompanyName, ContactName, ContactTitle, Address1, Address2, Address3, Contact1, Contact2, Contact3, TINNo, Description, Discontinue) " +
                                            "VALUES (@SupplierID, @CompanyName, @ContactName, @ContactTitle, @Address1, @Address2, @Address3, @Contact1, @Contact2, @Contact3, @TINNo, @Description, @Discontinue)";
                    myCommand.Parameters.AddWithValue("@SupplierID", MaxID);
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtSupplierName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactName", myPage.txtContactName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactTitle", myPage.txtContactTitle.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Address1", myPage.txtAddress1.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Address2", myPage.txtAddress2.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Address3", myPage.txtAddress3.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact1", myPage.txtContact1.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact2", myPage.txtContact2.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact3", myPage.txtContact3.Text.Trim());
                    myCommand.Parameters.AddWithValue("@TINNo", myPage.txtTIN.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Description", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Discontinue",0);
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Supplier Information was SUCCESSFULLY SAVED.";
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

        public int editSupplier(fSupplierEdit myPage)
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

                //--Required Field: Supplier Name--
                if (myPage.txtSupplierName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Supplier Name");
                    }
                    myPage.txtMessage.Text += "Provide a value for SUPPLIER NAME field; \n\r";
                    hasError = true;
                }

                //--Unique Field: Supplier Name--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_supplier WHERE CompanyName=@CompanyName AND SupplierID!=@SupplierID; ";
                myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtSupplierName.Text.Trim());
                myCommand.Parameters.AddWithValue("@SupplierID", myPage.getSupplierID());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Supplier Name");
                    }
                    myPage.txtMessage.Text += "Change the value of the SUPPLIER NAME to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Insert new product information--
                    myCommand.CommandText = "UPDATE a_supplier SET CompanyName=@CompanyName, ContactName=@ContactName, ContactTitle=@ContactTitle, Address1=@Address1, Address2=@Address2, Address3=@Address3, Contact1=@Contact1, Contact2=@Contact2, Contact3=@Contact3, TINNo=@TINNo, Description=@Description, Discontinue=@Discontinue " +
                                            "WHERE SupplierID = @SupplierID";
                    myCommand.Parameters.AddWithValue("@SupplierID", myPage.getSupplierID());
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.txtSupplierName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactName", myPage.txtContactName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ContactTitle", myPage.txtContactTitle.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Address1", myPage.txtAddress1.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Address2", myPage.txtAddress2.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Address3", myPage.txtAddress3.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact1", myPage.txtContact1.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact2", myPage.txtContact2.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Contact3", myPage.txtContact3.Text.Trim());
                    myCommand.Parameters.AddWithValue("@TINNo", myPage.txtTIN.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Description", myPage.txtRemarks.Text.Trim());
                    if(myPage.chkDiscontinue.Checked) myCommand.Parameters.AddWithValue("@Discontinue", 1);
                    else myCommand.Parameters.AddWithValue("@Discontinue", 0);
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Supplier Information was SUCCESSFULLY EDITED.";
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

        public int deleteSupplier(fSupplierEdit myPage)
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

                //--Referenced Item--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_delivery WHERE SupplierID=@SupplierID; ";
                myCommand.Parameters.AddWithValue("@SupplierID", myPage.getSupplierID());
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Product deliveries; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_product WHERE SupplierID=@SupplierID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Product(s) for sale; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                if (myPage.txtMessage.Text.Trim().Length > 0)
                {
                    myPage.txtMessage.Text = "CANNOT DELETE a referenced record. CHANGE THE STATUS to discontinue instead. THIS SUPPLIER HAS: " + myPage.txtMessage.Text;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Insert new product information--
                    myCommand.CommandText = "DELETE FROM a_supplier WHERE SupplierID=@SupplierID";
                    myCommand.Parameters.AddWithValue("@SupplierID", myPage.getSupplierID());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Supplier information was SUCCESSFULLY DELETED.";
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
    }
}
