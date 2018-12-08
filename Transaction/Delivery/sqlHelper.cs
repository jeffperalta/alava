using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AlavaSoft.Transaction.Delivery
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

        public int addDelivery(fDeliveryAdd myPage, int UserID) 
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

                //--Required Field: Status--
                if (myPage.cmbStatus.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Delivery STATUS");
                    }
                    myPage.txtMessage.Text += "Provide a value for DELIVERY STATUS field; \n\r";
                    hasError = true;
                }

                //--Check Status Validity--
                if (myPage.cmbStatus.Text.Trim().ToLower() != "accounted" && myPage.cmbStatus.Text.Trim().ToLower() != "confirmed" && myPage.cmbStatus.Text.Trim().ToLower() != "for request")
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(11);
                        myPage.addMessageParameter("STATUS field");
                        myPage.addMessageParameter("'Accounted', 'Confirmed', and 'For Request' ONLY.");
                    }

                    myPage.txtMessage.Text += "Invalid Delivery STATUS value; \n\r";
                    hasError = true;
                }

                //--Required Field: Delivery Details--
                Boolean blnHasDetail = false;
                for (int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                {
                    if (myPage.dgvDelivery.Rows[iCtr].Visible)
                    {
                        blnHasDetail = true;
                        break;
                    }
                }

                if (!blnHasDetail) 
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Delivery DETAILS");
                    }
                    myPage.txtMessage.Text += "Identify the PRODUCTS for delivery; \n\r";
                    hasError = true;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Delivery ID--
                    myCommand.CommandText = "SELECT MAX(DeliveryID) + 1 FROM a_delivery";
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

                    myPage.setDeliveryID(MaxID);

                    //--Get Status ID--
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='Delivery' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@LookUpName", myPage.cmbStatus.Text.Trim());
                    int iStatus = int.Parse(myCommand.ExecuteScalar().ToString());
                     
                    //--Get Supplier ID--
                    myCommand.CommandText = "SELECT SupplierID FROM a_supplier WHERE CompanyName=@CompanyName";
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.getSupplierName());
                    int iSupplier = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Insert New Delivery Information--
                    myCommand.CommandText = "INSERT INTO a_delivery (DeliveryID, DeliveryDate, TransactionDate, DeliveryReceiptNo, Status, Remarks, EmployeeID, SupplierID) " +
                                            "VALUES(@DeliveryID, @DeliveryDate, @TransactionDate, @DeliveryReceiptNo, @Status, @Remarks, @EmployeeID, @SupplierID)";
                    myCommand.Parameters.AddWithValue("@DeliveryID", MaxID);
                    if (myPage.dtpDeliveryDate.Checked)
                    {
                        myCommand.Parameters.AddWithValue("@DeliveryDate", myPage.dtpDeliveryDate.Value);
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@DeliveryDate", String.Empty);
                    }

                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@DeliveryReceiptNo", myPage.txtDeliveryReceiptNo.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Status", iStatus);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", UserID);
                    myCommand.Parameters.AddWithValue("@SupplierID", iSupplier);
                    myResult = myCommand.ExecuteNonQuery();

                    
                    //--Delivery ID Set--
                    myCommand.Parameters.AddWithValue("@PID", "");
                    myCommand.Parameters.AddWithValue("@Quantity", 0);
                    for (int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                    {
                        if (myPage.dgvDelivery.Rows[iCtr].Visible) 
                        {
                            //--Insert Details--
                            myCommand.CommandText = "INSERT INTO a_delivery_detail(DeliveryID, PID, Quantity) " +
                                                    "VALUES(@DeliveryID, @PID, @Quantity)";
                            myCommand.Parameters["@PID"].Value = myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString();
                            myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvDelivery.Rows[iCtr].Cells[4].Value.ToString());
                            myResult += myCommand.ExecuteNonQuery();

                            //--Items Count: UnitInStock--
                            if (myPage.cmbStatus.Text.Trim().ToLower() == "accounted") 
                            {
                                myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock + @Quantity WHERE PID=@PID";
                                myCommand.ExecuteNonQuery();
                            }

                            //--Items Count: UnitOnOrder--
                            if (myPage.cmbStatus.Text.Trim().ToLower() == "confirmed")
                            {
                                myCommand.CommandText = "UPDATE a_product SET UnitOnDelivery = UnitOnDelivery + @Quantity WHERE PID=@PID";
                                myCommand.ExecuteNonQuery();
                            }
                        }//--Next Visible Row--
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Delivery Information was SUCCESSFULLY SAVED.";
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

        public int showDetails(fDeliveryEdit myPage) 
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;

            try
            {
                #region Instantion Section
                Boolean hasError = false;
                myCommand = myConnection.CreateCommand();
                #endregion

                #region Check Input Section
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Retrieve Supplier Information--
                    myCommand.CommandText = "SELECT a_product.PID, a_category.CategoryName, a_product.ProductName, a_product.QuantityPerUnit, a_delivery_detail.Quantity, a_delivery_detail.Quantity/a_product.QuantityPerUnit " + 
                                            "FROM a_category JOIN (a_delivery_detail JOIN a_product ON a_delivery_detail.PID = a_product.PID) ON a_category.CategoryID = a_product.CategoryID " + 
                                            "WHERE a_delivery_detail.DeliveryID=@DeliveryID";
                    myCommand.Parameters.AddWithValue("@DeliveryID", myPage.getDeliveryID());
                    myDataReader = myCommand.ExecuteReader();
                    while (myDataReader.Read())
                    {
                        myPage.dgvDelivery.Rows.Add(myDataReader.GetString(0), myDataReader.GetString(1), myDataReader.GetString(2), myDataReader.GetInt32(3), myDataReader.GetInt32(4), myDataReader.GetInt32(5));
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

            return myResult;
        }

        public int editDelivery(fDeliveryEdit myPage, int UserID) 
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

                //--Required Field: Delivery ID--
                if (myPage.getDeliveryID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Delivery ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for Delivery ID field; \n\r";
                    hasError = true;
                }

                //--Required Field: Status--
                if (myPage.cmbStatus.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Delivery STATUS");
                    }
                    myPage.txtMessage.Text += "Provide a value for DELIVERY STATUS field; \n\r";
                    hasError = true;
                }

                //--Check Status Validity--
                if (myPage.cmbStatus.Text.Trim().ToLower() != "accounted" && myPage.cmbStatus.Text.Trim().ToLower() != "confirmed" && myPage.cmbStatus.Text.Trim().ToLower() != "for request")
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(11);
                        myPage.addMessageParameter("STATUS field");
                        myPage.addMessageParameter("s are 'Accounted', 'Confirmed', and 'For Request' ONLY.");
                    }

                    myPage.txtMessage.Text += "Invalid Delivery STATUS value; \n\r";
                    hasError = true;
                }

                //--Required Field: Delivery Details--
                Boolean blnHasDetail = false;
                for (int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                {
                    if (myPage.dgvDelivery.Rows[iCtr].Visible)
                    {
                        blnHasDetail = true;
                        break;
                    }
                }

                if (!blnHasDetail)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Delivery DETAILS");
                    }
                    myPage.txtMessage.Text += "Identify the PRODUCTS for delivery; \n\r";
                    hasError = true;
                }

                //--Check Current Status: Cannot Edit details infomation--
                //--Check Delivery Date If Status Is Set To Accounted--

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();
    
                    //--Get Current Status--
                    String sCurrentStatus = "";
                    myCommand.CommandText = "SELECT  c_look_up.LookUpName FROM  a_delivery INNER JOIN c_look_up ON a_delivery.Status = c_look_up.ValueId " +
                                            "WHERE (c_look_up.LookUpDiv = 'delivery') AND (a_delivery.DeliveryID = @DeliveryID)";
                    myCommand.Parameters.AddWithValue("@DeliveryID", myPage.getDeliveryID());
                    sCurrentStatus = myCommand.ExecuteScalar().ToString();
                    
                    //--Get Status ID--
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='Delivery' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@LookUpName", myPage.cmbStatus.Text.Trim());
                    int iStatus = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Get Supplier ID--
                    myCommand.CommandText = "SELECT SupplierID FROM a_supplier WHERE CompanyName=@CompanyName";
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.getSupplierName());
                    int iSupplier = int.Parse(myCommand.ExecuteScalar().ToString());

                    
                    //--Edit Delivery Header Information--
                    myCommand.CommandText = "UPDATE a_delivery SET DeliveryDate=@DeliveryDate, TransactionDate=@TransactionDate, DeliveryReceiptNo=@DeliveryReceiptNo, Status=@Status, Remarks=@Remarks, EmployeeID=@EmployeeID, SupplierID=@SupplierID " +
                                            "WHERE DeliveryID=@DeliveryID ";
                    //--Delivery ID is already Set--
                    if (myPage.dtpDeliveryDate.Checked)
                    {
                        myCommand.Parameters.AddWithValue("@DeliveryDate", myPage.dtpDeliveryDate.Value);
                    }
                    else
                    {
                        myCommand.Parameters.AddWithValue("@DeliveryDate", String.Empty);
                    }

                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@DeliveryReceiptNo", myPage.txtDeliveryReceiptNo.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Status", iStatus);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", UserID);
                    myCommand.Parameters.AddWithValue("@SupplierID", iSupplier);
                    myResult = myCommand.ExecuteNonQuery();

                    //--Update Delivery Details Depending on the Status Set On the Calling Interface--
                    /**
                     * Delivery Status:
                     *  FROM 'for request' TO 'confirmed' DETAILS are added to UnitsOnDelivery
                     *  FROM 'for request' TO 'accounted' DETAILS are added to UnitInStock (actual warehouse count)
                     *  FROM 'confirmed' TO 'accounted' DETAILS are added to UnitInStock and UnitsOnDelivery is Updated
                     */

                    myCommand.Parameters.AddWithValue("@PID", "");
                    myCommand.Parameters.AddWithValue("@Quantity", 0);
                    if(myPage.cmbStatus.Text.Trim().ToLower() == "confirmed")
                    {
                        if(sCurrentStatus.Trim().ToLower() == "for request")
                        {
                            myCommand.CommandText = "DELETE FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                            //--Delivery ID is already SET--
                            myCommand.ExecuteNonQuery();

                            //--UPDATE Product table: Unit On Delivery--
                            for(int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                            {
                                if(myPage.dgvDelivery.Rows[iCtr].Visible)
                                {
                                    myCommand.CommandText = "INSERT INTO a_delivery_detail(DeliveryID, PID, Quantity) " + 
                                                            "VALUES(@DeliveryID, @PID, @Quantity) ";
                                    //--Delivery ID is already SET--
                                    myCommand.Parameters["@PID"].Value = myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString();
                                    myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvDelivery.Rows[iCtr].Cells[4].Value.ToString());
                                    myResult += myCommand.ExecuteNonQuery();

                                    myCommand.CommandText = "UPDATE a_product SET UnitOnDelivery = UnitOnDelivery + @Quantity WHERE PID=@PID";
                                    myCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        else //--Same Status BUT might change the details information--
                        {
                            myCommand.CommandText = "SELECT PID, Quantity FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                            //--DeliveryID is Set--
                            myDataReader = myCommand.ExecuteReader();
                            Queue<string> qProductID = new Queue<string>(); //Holds the Products to Update
                            Queue<int> qQuantity = new Queue<int>();        //The quantity of products to Update
                            while(myDataReader.Read()) {
                                qProductID.Enqueue(myDataReader.GetString(0));
                                qQuantity.Enqueue(myDataReader.GetInt32(1));
                            }
                            myDataReader.Close();

                            //--Update existing delivery Details--
                            myCommand.CommandText = "UPDATE a_product SET UnitOnDelivery = UnitOnDelivery - @Quantity WHERE PID=@PID";
                            while(qProductID.Count > 0 && qQuantity.Count > 0) 
                            {
                                myCommand.Parameters["@PID"].Value = qProductID.Dequeue();
                                myCommand.Parameters["@Quantity"].Value = qQuantity.Dequeue();
                                myResult += myCommand.ExecuteNonQuery();
                            }
                            qProductID = null;
                            qQuantity = null;

                            //--Delete Previous Delivery Details
                            myCommand.CommandText = "DELETE FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                            //--DeliveryID is already Set--
                            myCommand.ExecuteNonQuery();

                            //--Insert New Delivery Details--
                            for (int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                            {
                                if (myPage.dgvDelivery.Rows[iCtr].Visible)
                                {
                                    myCommand.CommandText = "INSERT INTO a_delivery_detail(DeliveryID, PID, Quantity) " +
                                                            "VALUES(@DeliveryID, @PID, @Quantity) ";
                                    //--Delivery ID is already SET--
                                    myCommand.Parameters["@PID"].Value = myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString();
                                    myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvDelivery.Rows[iCtr].Cells[4].Value.ToString());
                                    myResult += myCommand.ExecuteNonQuery();

                                    myCommand.CommandText = "UPDATE a_product SET UnitOnDelivery = UnitOnDelivery + @Quantity WHERE PID=@PID";
                                    myCommand.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    if(myPage.cmbStatus.Text.Trim().ToLower() == "accounted")
                    {
                        if (sCurrentStatus.Trim().ToLower() == "for request")
                        {
                            //--Delete Previous Delivery Details
                            myCommand.CommandText = "DELETE FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                            //--DeliveryID is already Set--
                            myCommand.ExecuteNonQuery();

                            //--UPDATE Product Table: Unit In Stock; INSERT NEW DETAILS--
                            for(int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                            {
                                if(myPage.dgvDelivery.Rows[iCtr].Visible)
                                {
                                    myCommand.CommandText = "INSERT INTO a_delivery_detail(DeliveryID, PID, Quantity) " + 
                                                            "VALUES(@DeliveryID, @PID, @Quantity) ";
                                    //--Delivery ID is already SET--
                                    myCommand.Parameters["@PID"].Value = myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString();
                                    myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvDelivery.Rows[iCtr].Cells[4].Value.ToString());
                                    myResult += myCommand.ExecuteNonQuery();

                                    myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock + @Quantity WHERE PID=@PID";
                                    myCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        if (sCurrentStatus.Trim().ToLower() == "confirmed")
                        {
                            //--UPDATE Product Table: Unit On Order, Unit On Stock
                            myCommand.CommandText = "SELECT PID, Quantity FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                            //--DeliveryID is Set--
                            myDataReader = myCommand.ExecuteReader();
                            Queue<string> qProductID = new Queue<string>(); //Holds the Products to Update
                            Queue<int> qQuantity = new Queue<int>();        //The quantity of products to Update
                            while(myDataReader.Read()) {
                                qProductID.Enqueue(myDataReader.GetString(0));
                                qQuantity.Enqueue(myDataReader.GetInt32(1));
                            }
                            myDataReader.Close();

                            //--Update existing delivery Details--
                            myCommand.CommandText = "UPDATE a_product SET UnitOnDelivery = UnitOnDelivery - @Quantity WHERE PID=@PID";
                            while(qProductID.Count > 0 && qQuantity.Count > 0) 
                            {
                                myCommand.Parameters["@PID"].Value = qProductID.Dequeue();
                                myCommand.Parameters["@Quantity"].Value = qQuantity.Dequeue();
                                myResult += myCommand.ExecuteNonQuery();
                            }
                            qProductID = null;
                            qQuantity = null;

                            //--Delete Previous Delivery Details
                            myCommand.CommandText = "DELETE FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                            //--DeliveryID is already Set--
                            myCommand.ExecuteNonQuery();

                            //--Insert New Delivery Details--
                            for(int iCtr = 0; iCtr < myPage.dgvDelivery.Rows.Count; iCtr++)
                            {
                                if(myPage.dgvDelivery.Rows[iCtr].Visible)
                                {
                                    myCommand.CommandText = "INSERT INTO a_delivery_detail(DeliveryID, PID, Quantity) " + 
                                                            "VALUES(@DeliveryID, @PID, @Quantity) ";
                                    //--Delivery ID is already SET--
                                    myCommand.Parameters["@PID"].Value = myPage.dgvDelivery.Rows[iCtr].Cells[0].Value.ToString();
                                    myCommand.Parameters["@Quantity"].Value = int.Parse(myPage.dgvDelivery.Rows[iCtr].Cells[4].Value.ToString());
                                    myResult += myCommand.ExecuteNonQuery();

                                    myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock + @Quantity WHERE PID=@PID";
                                    myCommand.ExecuteNonQuery();
                                }
                            }
                        }
                        //--There Will be no changes to the delivery Details when the status is set to 'Accounted'--
                        //--Ensure that the interface will disable such attempts--
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Delivery Information was SUCCESSFULLY SAVED.";
                    }
                    else
                    {
                        myPage.setMessageNumber(5);
                        myPage.txtMessage.Text = "The process succeeded but NO records where affected. POSSIBLE CAUSE: No data corresponds to your input. ";
                    }

                    myTransaction.Commit();
                }
                else //--There was an ERROR at the Check Input Section
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

        public int deleteDelivery(fDeliveryEdit myPage) 
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0;
            String sCurrentStatus = "";

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

                //--Required Field: Delivery ID--
                if (myPage.getDeliveryID() == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Delivery ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for Delivery ID field; \n\r";
                    hasError = true;
                }

                //--Instantiate the current status of the delivery--
                //--Can only delete Delivery with status of 'for request' and 'confirmed'
                if (myPage.getDeliveryID() != 0)
                {
                    myCommand.CommandText = "SELECT  c_look_up.LookUpName FROM  a_delivery INNER JOIN c_look_up ON a_delivery.Status = c_look_up.ValueId " +
                                            "WHERE (c_look_up.LookUpDiv = 'delivery') AND (a_delivery.DeliveryID = @DeliveryID)";
                    myCommand.Parameters.AddWithValue("@DeliveryID", myPage.getDeliveryID());
                    sCurrentStatus = myCommand.ExecuteScalar().ToString();

                    if (sCurrentStatus.Trim().ToLower() != "for request" && sCurrentStatus.Trim().ToLower() != "confirmed") 
                    {
                        if (!hasError)
                        {
                            myPage.setMessageNumber(12);
                            myPage.addMessageParameter("a '" + sCurrentStatus.ToUpper() + "' Delivery.");
                        }
                        myPage.txtMessage.Text += "The delivery record can no longer be cancelled; \n\r";
                        hasError = true;
                    }
                }
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();
                    myCommand.Parameters.AddWithValue("@DeliveryID", myPage.getDeliveryID());

                    //--Additional Step For Deliveries with status of 'confirmed'--
                    if (sCurrentStatus.Trim().ToLower() == "confirmed")
                    {
                        //--Get Product Quantity--
                        myCommand.CommandText = "SELECT PID, Quantity FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                        myDataReader = myCommand.ExecuteReader();
                        Stack<string> stkProduct = new Stack<string>();
                        Stack<int> stkQuantity = new Stack<int>();
                        while (myDataReader.Read())
                        {
                            stkProduct.Push(myDataReader.GetString(0));
                            stkQuantity.Push(myDataReader.GetInt32(1));
                        }
                        myDataReader.Close();

                        //--UPDATE product table EDIT UnitOnDelivery
                        myCommand.CommandText = "UPDATE a_product SET UnitOnDelivery = UnitOnDelivery - @Quantity WHERE PID=@PID";
                        myCommand.Parameters.AddWithValue("@PID", "");
                        myCommand.Parameters.AddWithValue("@Quantity", 0);   
                        while (stkProduct.Count > 0 && stkQuantity.Count > 0)
                        {
                            myCommand.Parameters["@PID"].Value = stkProduct.Pop();
                            myCommand.Parameters["@Quantity"].Value = stkQuantity.Pop();
                            myCommand.ExecuteNonQuery();
                        }

                        stkProduct = null;
                        stkQuantity = null;

                    }

                    //--Delete The Actual Delivery With status of 'For Request' OR 'Confirmed'
                    myCommand.CommandText = "DELETE FROM a_delivery_detail WHERE DeliveryID=@DeliveryID";
                    myResult = myCommand.ExecuteNonQuery();

                    myCommand.CommandText = "DELETE FROM a_delivery WHERE DeliveryID=@DeliveryID";
                    myResult += myCommand.ExecuteNonQuery();
                    
                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Delivery Information was SUCCESSFULLY DELETED.";
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
