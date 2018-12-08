using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.SqlClient;
using System.Data;

namespace AlavaSoft.Transaction.User
{
    class sqlHelper
    {
        private SqlConnection myConnection = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataReader myDataReader = null;

        public sqlHelper() { 
        
        }

        public Boolean validate(AlavaSoft.fLogIn myLogInPage) {
            Boolean myResult = false;
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();

            try
            {
                myConnection = myDatabase.getConnection();
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "SELECT f_name, l_name, user_id FROM a_users WHERE username=@username AND pass=@pass AND status=1";
                myCommand.Parameters.AddWithValue("@username", myLogInPage.txtUserName.Text.Trim());
                myCommand.Parameters.AddWithValue("@pass", myLogInPage.txtPassword.Text.Trim());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    myLogInPage.myLogInCredential.setValidUser(true);

                    //--Set UserCredential Information--
                    myLogInPage.myLogInCredential.setFullName(myDataReader.GetString(0) + " " + myDataReader.GetString(1));
                    myLogInPage.myLogInCredential.setUserID(myDataReader.GetInt32(2));

                    //--Set Access Type--
                    myDataReader.Close();
                    myCommand.CommandText = "SELECT a_access.AccessName FROM a_access JOIN a_access_detail ON a_access.AccessID=a_access_detail.AccessID " +
                                            "WHERE a_access_detail.EmployeeID=@EmployeeID AND a_access_detail.Active=1 ";
                    myCommand.Parameters.AddWithValue("@EmployeeID", myLogInPage.myLogInCredential.getUserID());
                    myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read()) {
                        myLogInPage.myLogInCredential.addUserAccess(myDataReader.GetString(0));
                    }
                }
                else 
                {
                    myLogInPage.myLogInCredential.setValidUser(false);
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            finally 
            {
                if (myConnection != null) {
                    myConnection.Close();
                }

                if(myDataReader != null) {
                    myDataReader.Close();
                }
            }


            return myResult;
        }

        public int addUser(fUserAdd myPage) {
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
                //Erase Previous Message
                myPage.txtMessage.Text = "";

                //--Required Field: FirstName--
                if (myPage.txtFirstName.Text.Trim().Length == 0) {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("First name");
                    }
                    myPage.txtMessage.Text += "Provide value for FIRST NAME field; \n\r";
                    hasError = true;
                }

                //--Required Field: UserName--
                //--Input Format: UserName (At Least 6 Characters)--
                if (myPage.txtUserName.Text.Trim().Length < 6) {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("User Name");
                        myPage.addMessageParameter("at least 6 characters");
                    }
                    myPage.txtMessage.Text += "Provide a value for the USERNAME and make it at least 6 characters long; \n\r";
                    hasError = true;
                }

                //--Unique Field: UserName--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_users WHERE username=@username; ";
                myCommand.Parameters.AddWithValue("@username", myPage.txtUserName.Text.Trim());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows) {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("User Name");
                    }
                    myPage.txtMessage.Text += "Change the value of the USERNAME to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                //--Required Field: Password--
                //--Input Format: Password (At Least 6 Characters)--
                if (myPage.txtPassword.Text.Trim().Length < 6) {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Password");
                        myPage.addMessageParameter("at least 6 characters");
                    }
                    myPage.txtMessage.Text += "Provide a value for the PASSWORD and make it at least 6 characters long; \n\r";
                    hasError = true;
                }
                
                //--Input Misc: Password and confirmation must be equal--
                if (myPage.txtPassword.Text.Trim() != myPage.txtConfirm.Text.Trim()) {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Confirmation");
                        myPage.addMessageParameter("same with the password you have entered");
                    }
                    myPage.txtMessage.Text += "Provide a value for the PASSWORD CONFIRM field; \n\r";
                    hasError = true;
                }

                #endregion

                #region Process Section

                if (!hasError)
                {
                    //Get Next UserID
                    myCommand.CommandText = "SELECT MAX(user_id) + 1 FROM a_users ";
                    int MaxID;
                    object temp = myCommand.ExecuteScalar();
                    if (temp == System.DBNull.Value) { 
                        MaxID = 1000; 
                    }
                    else
                    {
                        MaxID = (int)temp;
                    }

                    myCommand.CommandText = "INSERT INTO a_users " +
                        "(user_id, f_name, l_name, username, pass, title, position, status) " +
                        "VALUES (@user_id, @f_name, @l_name, @username, @pass, @title, @position, @status) ";
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("@user_id", MaxID).SqlDbType = SqlDbType.Int;
                    myCommand.Parameters.AddWithValue("@f_name", myPage.txtFirstName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@l_name", myPage.txtLastName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@username", myPage.txtUserName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@pass", myPage.txtPassword.Text.Trim());
                    myCommand.Parameters.AddWithValue("@title", myPage.txtTitle.Text.Trim());
                    myCommand.Parameters.AddWithValue("@position", myPage.txtPosition.Text.Trim());
                    myCommand.Parameters.AddWithValue("@status", 1).SqlDbType = SqlDbType.Int; //Active
                    myResult = myCommand.ExecuteNonQuery();

                    myCommand.CommandText = "INSERT INTO a_access_detail (AccessID, EmployeeID, Active) " +
                                            "SELECT AccessID, @user_id, 1 FROM a_access ";
                    myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New User Account Information was SUCCESSFULLY SAVED.";
                    }
                    else
                    {
                        myPage.setMessageNumber(5);
                        myPage.txtMessage.Text = "This event was caused by the fact that there is no record that correspond to your input. To correct this, please select a valid record at the data grid.";
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

        public int editUser(fUserEdit myPage) {
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
                //Erase Previous Message
                myPage.txtMessage.Text = "";

                //--Required Field: First Name --
                if (myPage.txtFirstName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("First name");
                    }
                    myPage.txtMessage.Text += "Provide value for FIRST NAME field; \n\r";
                    hasError = true;
                }

                //--Required Field: UserName--
                //--Input Format: UserName (At Least 6 Characters)--
                if (myPage.txtUserName.Text.Trim().Length < 6)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("User Name");
                        myPage.addMessageParameter("at least 6 characters");
                    }
                    myPage.txtMessage.Text += "Provide a value for the USERNAME and make it at least 6 characters long; \n\r";
                    hasError = true;
                }

                //--Unique Field: UserName--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_users WHERE username=@username AND user_id!=@user_id; ";
                myCommand.Parameters.AddWithValue("@username", myPage.txtUserName.Text.Trim());
                myCommand.Parameters.AddWithValue("@user_id", myPage.getUserID());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("User Name");
                    }
                    myPage.txtMessage.Text += "Change the value of the USERNAME to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                //--Required Field: Password--
                //--Input Format: Password (At Least 6 Characters)--
                if (myPage.txtPassword.Text.Trim().Length < 6)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Password");
                        myPage.addMessageParameter("at least 6 characters");
                    }
                    myPage.txtMessage.Text += "Provide a value for the PASSWORD and make it at least 6 characters long; \n\r";
                    hasError = true;
                }

                //--Input Misc: Password and confirmation must be equal--
                if (myPage.txtPassword.Text.Trim() != myPage.txtConfirm.Text.Trim())
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(3);
                        myPage.addMessageParameter("Confirmation");
                        myPage.addMessageParameter("same with the password you have entered");
                    }
                    myPage.txtMessage.Text += "Provide a value for the PASSWORD CONFIRM field; \n\r";
                    hasError = true;
                }
                #endregion

                #region Process Section

                if (!hasError) {
                    myCommand.Parameters.Clear();
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='UserStatus' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@LookUpName", myPage.cmbStatus.Text);
                    int iStatus = (int)myCommand.ExecuteScalar();

                    myCommand.CommandText = "UPDATE a_users SET f_name=@f_name, l_name=@l_name, username=@username, pass=@pass, title=@title, position=@position, status=@status WHERE user_id=@user_id";
                    myCommand.Parameters.AddWithValue("@user_id", myPage.getUserID()).SqlDbType = SqlDbType.Int;
                    myCommand.Parameters.AddWithValue("@f_name", myPage.txtFirstName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@l_name", myPage.txtLastName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@username", myPage.txtUserName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@pass", myPage.txtPassword.Text.Trim());
                    myCommand.Parameters.AddWithValue("@title", myPage.txtTitle.Text.Trim());
                    myCommand.Parameters.AddWithValue("@position", myPage.txtPosition.Text.Trim());
                    myCommand.Parameters.AddWithValue("@status", iStatus).SqlDbType = SqlDbType.Int; //Active
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "User Account Information was SUCCESSFULLY EDITED.";
                    }
                    else
                    {
                        myPage.setMessageNumber(5);
                        myPage.txtMessage.Text = "This event was caused by the fact that there is no record that correspond to your input. To correct this, please select a valid record at the data grid.";
                    }
                    myTransaction.Commit();
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

        public int deleteUser(fUserEdit myPage) {
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

                myCommand.CommandText = "SELECT 'EXIST' FROM a_alteration WHERE EmployeeID=@EmployeeID; ";
                myCommand.Parameters.AddWithValue("@EmployeeID", myPage.getUserID());
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows) 
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Altered the product inventory; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_delivery WHERE EmployeeID=@EmployeeID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Requested for a delivery; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_payment WHERE EmployeeID=@EmployeeID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Received payment; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_refund WHERE EmployeeID=@EmployeeID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Created a refund for excess payment; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_return WHERE EmployeeID=@EmployeeID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Created a product refund; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_sales_order WHERE EmployeeID=@EmployeeID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Created a sales order; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                if (myPage.txtMessage.Text.Trim().Length > 0) {
                    myPage.txtMessage.Text = "CANNOT DELETE a referenced record. If you really like to 'remove' this information, edit the user account and change its status to inactive. In this way all his previous transactions will be retained but will disallow him to access the application. THIS EMPLOYEE HAS ALREADY: " + myPage.txtMessage.Text;
                }

                #endregion

                #region Process Section
                myCommand.Parameters.Clear();
                myCommand.CommandText="DELETE FROM a_access_detail WHERE EmployeeID=@EmployeeID";
                myCommand.Parameters.AddWithValue("@EmployeeID", myPage.getUserID());
                myCommand.ExecuteNonQuery();
                myCommand.CommandText="DELETE FROM a_users WHERE user_id=@EmployeeID";
                myResult = myCommand.ExecuteNonQuery();

                if (myResult > 0)
                {
                    myPage.setMessageNumber(7);
                    myPage.txtMessage.Text = "User Account Information was SUCCESSFULLY DELETED.";
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

        public int editAccess(fUserAccess myPage) {
            int myResult = 0;
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();

            try
            {
                #region Instantion Section
                //Boolean hasError = false;
                myTransaction = myConnection.BeginTransaction();
                myCommand = myConnection.CreateCommand();
                myCommand.Transaction = myTransaction;
                #endregion

                #region Check Input Section
                #endregion

                #region Process Section

                myCommand.CommandText = "UPDATE a_access_detail SET Active=@Active WHERE AccessID=@AccessID AND EmployeeID=@EmployeeID ";
                myCommand.Parameters.AddWithValue("@EmployeeID", myPage.getUserID());

                //--User Module--
                myCommand.Parameters.AddWithValue("@AccessID", 1);
                if (myPage.getUserAccess())myCommand.Parameters.AddWithValue("@Active", 1); else myCommand.Parameters.AddWithValue("@Active", 0);
                myResult += myCommand.ExecuteNonQuery();

                //--Sales Order Module--
                myCommand.Parameters["@AccessID"].Value = 2;
                if (myPage.getSalesOrderAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Payment Module--
                myCommand.Parameters["@AccessID"].Value = 3;
                if (myPage.getPaymentAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Void Payment Module--
                myCommand.Parameters["@AccessID"].Value = 4;
                if (myPage.getVoidPaymentAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Product Module--
                myCommand.Parameters["@AccessID"].Value = 5;
                if (myPage.getProductAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Inventory Module--
                myCommand.Parameters["@AccessID"].Value = 6;
                if (myPage.getInventoryAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Administration Module--
                myCommand.Parameters["@AccessID"].Value = 7;
                if (myPage.getAdministrationAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Refund Module--
                myCommand.Parameters["@AccessID"].Value = 8;
                if (myPage.getRefundAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Returns Module--
                myCommand.Parameters["@AccessID"].Value = 9;
                if (myPage.getReturnsAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Supplier Module--
                myCommand.Parameters["@AccessID"].Value = 10;
                if (myPage.getSupplierAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Reports Module--
                myCommand.Parameters["@AccessID"].Value = 11;
                if (myPage.getReportsAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Query Module--
                myCommand.Parameters["@AccessID"].Value = 12;
                if (myPage.getQueryAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Delivery Module--
                myCommand.Parameters["@AccessID"].Value = 13;
                if (myPage.getDeliveryAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                //--Customer Module--
                myCommand.Parameters["@AccessID"].Value = 14;
                if (myPage.getCustomerAccess()) myCommand.Parameters["@Active"].Value = 1; else myCommand.Parameters["@Active"].Value = 0;
                myResult += myCommand.ExecuteNonQuery();

                if (myResult > 0)
                {
                    myPage.setMessageNumber(7);
                    myPage.txtMessage.Text = "User Account Access Information was SUCCESSFULLY EDITED.";
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
