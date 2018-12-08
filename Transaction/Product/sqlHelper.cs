using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace AlavaSoft.Transaction.Product
{

    class sqlHelper
    {

        private SqlConnection myConnection = null;
        private SqlCommand myCommand = null;
        private SqlTransaction myTransaction = null;
        private SqlDataReader myDataReader = null;

        public sqlHelper() { 
        
        }

        public int addCategory(AlavaSoft.Transaction.Product.fCategoryAdd myPage) { 

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

                //--Required Field: Category Name--
                if (myPage.txtCategoryName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Category name");
                    }
                    myPage.txtMessage.Text += "Provide value for CATEGORY NAME field; \n\r";
                    hasError = true;
                }

                //--Check for Duplicate: Category Name--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_category WHERE CategoryName = @CName ";
                myCommand.Parameters.AddWithValue("@CName", myPage.txtCategoryName.Text.Trim());
                myDataReader = myCommand.ExecuteReader();

                myDataReader.Read();
                if (myDataReader.HasRows) {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Category name");
                    }
                    myPage.txtMessage.Text += "Provide a unique value for the CATEGORY NAME field; \n\r";
                    hasError = true;
                }
                myDataReader.Close();


                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    //Get Next UserID
                    myCommand.CommandText = "SELECT MAX(CategoryID) + 1 FROM a_category ";
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

                    myCommand.CommandText = "INSERT INTO a_category (CategoryID, CategoryName, Description) " +
                                            "VALUES(@CategoryID, @CategoryName, @Description) ";
                    myCommand.Parameters.AddWithValue("@CategoryID", MaxID);
                    myCommand.Parameters.AddWithValue("@CategoryName", myPage.txtCategoryName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Description", myPage.txtDescription.Text.Trim());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Product Category Information was SUCCESSFULLY SAVED.";
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

        public int editCategory(AlavaSoft.Transaction.Product.fCategoryEdit myPage) {

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

                //--Required Field: Category Name--
                if (myPage.txtCategoryName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Category name");
                    }
                    myPage.txtMessage.Text += "Provide value for CATEGORY NAME field; \n\r";
                    hasError = true;
                }

                //--Check for Duplicate: Category Name--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_category WHERE CategoryName = @CName AND CategoryID != @CatID";
                myCommand.Parameters.AddWithValue("@CName", myPage.txtCategoryName.Text.Trim());
                myCommand.Parameters.AddWithValue("@CatID", myPage.getCategoryID());
                myDataReader = myCommand.ExecuteReader();

                myDataReader.Read();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Category name");
                    }
                    myPage.txtMessage.Text += "Provide a unique value for the CATEGORY NAME field; \n\r";
                    hasError = true;
                }
                myDataReader.Close();
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();
                    myCommand.CommandText = "UPDATE a_category SET CategoryName=@CategoryName, Description=@Description WHERE CategoryID=@CategoryID";
                    myCommand.Parameters.AddWithValue("@CategoryName", myPage.txtCategoryName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@Description", myPage.txtDescription.Text.Trim());
                    myCommand.Parameters.AddWithValue("@CategoryID", myPage.getCategoryID());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Product Category Information was SUCCESSFULLY EDITED.";
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

        public int deleteCategory(AlavaSoft.Transaction.Product.fCategoryEdit myPage) {

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

                //--Referenced Item--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_product WHERE CategoryID=@CategoryID; ";
                myCommand.Parameters.AddWithValue("@CategoryID", myPage.getCategoryID());
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Some products belong to this category and cannot be deleted; \n\r";
                    hasError = true;
                }
                myDataReader.Close();
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();
                    myCommand.CommandText ="DELETE FROM a_category WHERE CategoryID=@CategoryID";
                    myCommand.Parameters.AddWithValue("@CategoryID", myPage.getCategoryID());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Product Category Information was SUCCESSFULLY DELETED.";
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

        public int addProduct(fProductAdd myPage) 
        {

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0, iReorderLevel = 0, iCaseSize = 1;
            double dUnitPrice = 0.00, dBulkPrice = 0.00;

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

                //--Required Field: Product ID--
                if (myPage.txtProductID.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Product ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT ID field; \n\r";
                    hasError = true;
                }

                //--Unique Field: Product ID--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_product WHERE PID=@PID";
                myCommand.Parameters.AddWithValue("@PID", myPage.txtProductID.Text.Trim());
                myDataReader = myCommand.ExecuteReader();
                myDataReader.Read();
                if (myDataReader.HasRows) 
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(2);
                        myPage.addMessageParameter("Product ID");
                    }
                    myPage.txtMessage.Text += "Change the value of the PRODUCT ID to make it unique; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                //--Required Field: Product Name--
                if (myPage.txtProductName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Product Name");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT Name field; \n\r";
                    hasError = true;
                }

                //--Required Field: Category--
                if (myPage.cmbCategory.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Category");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT CATEGORY field; \n\r";
                    hasError = true;
                }

                //--Required Field: Supplier--
                if (myPage.cmbSupplier.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Supplier");
                    }
                    myPage.txtMessage.Text += "Provide a value for SUPPLIER field; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Reorder level--
                iReorderLevel = 0;
                if (myPage.txtReorderLevel.Text.Trim().Length !=0 && !int.TryParse(myPage.txtReorderLevel.Text.Trim(), out iReorderLevel)) 
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("reorder level");
                        myPage.addMessageParameter("a whole number");
                    }
                    myPage.txtMessage.Text += "The REORDER LEVEL must be a valid integer number; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Case Size--
                iCaseSize = 1;
                if (myPage.txtCaseSize.Text.Trim().Length != 0 && !int.TryParse(myPage.txtCaseSize.Text.Trim(), out iCaseSize))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("case size");
                        myPage.addMessageParameter("a whole number");
                    }
                    myPage.txtMessage.Text += "The CASE SIZE must be a valid integer number; \n\r";
                    hasError = true;
                }

                //--Required Field: Unit Price--
                if (myPage.txtUnitPrice.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Unit price");
                    }
                    myPage.txtMessage.Text += "Provide a value for the UNIT PRICE field; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Unit Price--
                dUnitPrice = 0;
                if (myPage.txtUnitPrice.Text.Trim().Length != 0 && !double.TryParse(myPage.txtUnitPrice.Text.Trim(), out dUnitPrice))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("unit price");
                        myPage.addMessageParameter("a monetary value");
                    }
                    myPage.txtMessage.Text += "The UNIT PRICE must be a valid monetary value; \n\r";
                    hasError = true;
                }

                //--Required Field: Bulk Price--
                if (myPage.txtBulkPrice.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Bulk price");
                    }
                    myPage.txtMessage.Text += "Provide a value for the BULK PRICE field; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Bulk Price--
                dBulkPrice = 0;
                if (myPage.txtBulkPrice.Text.Trim().Length != 0 && !double.TryParse(myPage.txtBulkPrice.Text.Trim(), out dBulkPrice))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("bulk price");
                        myPage.addMessageParameter("a monetary value");
                    }
                    myPage.txtMessage.Text += "The BULK PRICE must be a valid monetary value; \n\r";
                    hasError = true;
                }

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Supplier ID--
                    myCommand.CommandText = "SELECT SupplierID FROM a_supplier WHERE CompanyName=@CompanyName";
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.cmbSupplier.Text.Trim());
                    int iSupplierID = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Get Category ID--
                    myCommand.CommandText = "SELECT CategoryID FROM a_category WHERE CategoryName=@CategoryName";
                    myCommand.Parameters.AddWithValue("@CategoryName", myPage.cmbCategory.Text.Trim());
                    int iCategoryID = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Insert new product information--
                    myCommand.CommandText = "INSERT INTO a_product (PID, ProductName, ProductDesc, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, BulkPrice, UnitInStock, UnitOnOrder, UnitOnDelivery, ReorderLevel, Discontinued) " +
                                            "VALUES(@PID, @ProductName, @ProductDesc, @SupplierID, @CategoryID, @QuantityPerUnit, @UnitPrice, @BulkPrice, @UnitInStock, @UnitOnOrder, @UnitOnDelivery, @ReorderLevel, @Discontinued)";
                    myCommand.Parameters.AddWithValue("@PID", myPage.txtProductID.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ProductName", myPage.txtProductName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ProductDesc", myPage.txtDescription.Text.Trim());
                    myCommand.Parameters.AddWithValue("@SupplierID", iSupplierID);
                    myCommand.Parameters.AddWithValue("@CategoryID", iCategoryID);
                    if (iCaseSize == 0) iCaseSize = 1; // Case by itself
                    myCommand.Parameters.AddWithValue("@QuantityPerUnit", iCaseSize);
                    myCommand.Parameters.AddWithValue("@UnitPrice", dUnitPrice);
                    myCommand.Parameters.AddWithValue("@BulkPrice", dBulkPrice);
                    myCommand.Parameters.AddWithValue("@UnitInStock", 0); //Initial Value -- change in Inventory module
                    myCommand.Parameters.AddWithValue("@UnitOnOrder", 0); //Initial Value -- change in SO module
                    myCommand.Parameters.AddWithValue("@UnitOnDelivery", 0); //Initial Value -- change in Delivery module
                    myCommand.Parameters.AddWithValue("@ReorderLevel", iReorderLevel);
                    myCommand.Parameters.AddWithValue("@Discontinued", 0); //Not Discontinued;
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "New Product Category Information was SUCCESSFULLY SAVED.";
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

        public int editProduct(fProductEdit myPage) 
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myConnection = myDatabase.getConnection();
            int myResult = 0, iReorderLevel = 0, iCaseSize = 1;
            double dUnitPrice = 0.00, dBulkPrice = 0.00;

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

                //--Required Field: Product ID--
                if (myPage.getProductID().Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Product ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT ID field; \n\r";
                    hasError = true;
                }

                //--Required Field: Product Name--
                if (myPage.txtProductName.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Product Name");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT Name field; \n\r";
                    hasError = true;
                }

                //--Required Field: Category--
                if (myPage.cmbCategory.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Category");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT CATEGORY field; \n\r";
                    hasError = true;
                }

                //--Required Field: Supplier--
                if (myPage.cmbSupplier.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Supplier");
                    }
                    myPage.txtMessage.Text += "Provide a value for SUPPLIER field; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Reorder level--
                iReorderLevel = 0;
                if (myPage.txtReorderLevel.Text.Trim().Length != 0 && !int.TryParse(myPage.txtReorderLevel.Text.Trim(), out iReorderLevel))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("reorder level");
                        myPage.addMessageParameter("a whole number");
                    }
                    myPage.txtMessage.Text += "The REORDER LEVEL must be a valid integer number; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Case Size--
                iCaseSize = 1;
                if (myPage.txtCaseSize.Text.Trim().Length != 0 && !int.TryParse(myPage.txtCaseSize.Text.Trim(), out iCaseSize))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("case size");
                        myPage.addMessageParameter("a whole number");
                    }
                    myPage.txtMessage.Text += "The CASE SIZE must be a valid integer number; \n\r";
                    hasError = true;
                }

                //--Required Field: Unit Price--
                if (myPage.txtUnitPrice.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Unit price");
                    }
                    myPage.txtMessage.Text += "Provide a value for the UNIT PRICE field; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Unit Price--
                dUnitPrice = 0;
                if (myPage.txtUnitPrice.Text.Trim().Length != 0 && !double.TryParse(myPage.txtUnitPrice.Text.Trim(), out dUnitPrice))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("unit price");
                        myPage.addMessageParameter("a monetary value");
                    }
                    myPage.txtMessage.Text += "The UNIT PRICE must be a valid monetary value; \n\r";
                    hasError = true;
                }

                //--Required Field: Bulk Price--
                if (myPage.txtBulkPrice.Text.Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Bulk price");
                    }
                    myPage.txtMessage.Text += "Provide a value for the BULK PRICE field; \n\r";
                    hasError = true;
                }

                //--Format must be numeric: Bulk Price--
                dBulkPrice = 0;
                if (myPage.txtBulkPrice.Text.Trim().Length != 0 && !double.TryParse(myPage.txtBulkPrice.Text.Trim(), out dBulkPrice))
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(8);
                        myPage.addMessageParameter("bulk price");
                        myPage.addMessageParameter("a monetary value");
                    }
                    myPage.txtMessage.Text += "The BULK PRICE must be a valid monetary value; \n\r";
                    hasError = true;
                }
                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--Get Supplier ID--
                    myCommand.CommandText = "SELECT SupplierID FROM a_supplier WHERE CompanyName=@CompanyName";
                    myCommand.Parameters.AddWithValue("@CompanyName", myPage.cmbSupplier.Text.Trim());
                    int iSupplierID = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Get Category ID--
                    myCommand.CommandText = "SELECT CategoryID FROM a_category WHERE CategoryName=@CategoryName";
                    myCommand.Parameters.AddWithValue("@CategoryName", myPage.cmbCategory.Text.Trim());
                    int iCategoryID = int.Parse(myCommand.ExecuteScalar().ToString());

                    //--Insert new product information--
                    myCommand.CommandText = "UPDATE a_product SET ProductName=@ProductName, ProductDesc=@ProductDesc, SupplierID=@SupplierID, CategoryID = @CategoryID, QuantityPerUnit=@QuantityPerUnit, UnitPrice=@UnitPrice, BulkPrice=@BulkPrice, ReorderLevel=@ReorderLevel, Discontinued=@Discontinued " + 
                                            "WHERE PID=@PID";
                    myCommand.Parameters.AddWithValue("@PID", myPage.getProductID());
                    myCommand.Parameters.AddWithValue("@ProductName", myPage.txtProductName.Text.Trim());
                    myCommand.Parameters.AddWithValue("@ProductDesc", myPage.txtDescription.Text.Trim());
                    myCommand.Parameters.AddWithValue("@SupplierID", iSupplierID);
                    myCommand.Parameters.AddWithValue("@CategoryID", iCategoryID);
                    myCommand.Parameters.AddWithValue("@QuantityPerUnit", iCaseSize);
                    myCommand.Parameters.AddWithValue("@UnitPrice", dUnitPrice);
                    myCommand.Parameters.AddWithValue("@BulkPrice", dBulkPrice);
                    myCommand.Parameters.AddWithValue("@ReorderLevel", iReorderLevel);
                    if (myPage.chkDiscontinue.Checked) myCommand.Parameters.AddWithValue("@Discontinued", 1);
                    else myCommand.Parameters.AddWithValue("@Discontinued", 0);
                    
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        //myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Product Information was SUCCESSFULLY EDITED.";
                    }
                    else
                    {
                        //myPage.setMessageNumber(5);
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

        public int deleteProduct(fProductEdit myPage) 
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

                //--Required Field: Product ID--
                if (myPage.getProductID().Trim().Length == 0)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(1);
                        myPage.addMessageParameter("Product ID");
                    }
                    myPage.txtMessage.Text += "Provide a value for PRODUCT ID field; \n\r";
                    hasError = true;
                }

                //--Referenced Item--
                myCommand.CommandText = "SELECT 'EXIST' FROM a_alteration_detail WHERE PID=@PID; ";
                myCommand.Parameters.AddWithValue("@PID", myPage.getProductID());
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Alteration details; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_delivery_detail WHERE PID=@PID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Delivery details; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                myCommand.CommandText = "SELECT 'EXIST' FROM a_return_detail WHERE PID=@PID; ";
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

                myCommand.CommandText = "SELECT 'EXIST' FROM a_sales_order_detail WHERE PID=@PID; ";
                myDataReader = myCommand.ExecuteReader();
                if (myDataReader.HasRows)
                {
                    if (!hasError)
                    {
                        myPage.setMessageNumber(6);
                    }
                    myPage.txtMessage.Text += "Sales order details; \n\r";
                    hasError = true;
                }
                myDataReader.Close();

                if (myPage.txtMessage.Text.Trim().Length > 0)
                {
                    myPage.txtMessage.Text = "CANNOT DELETE a referenced record. DISCONTINUE the product instead. THIS PRODUCT HAS: " + myPage.txtMessage.Text;
                }

                #endregion

                #region Process Section
                if (!hasError)
                {
                    myCommand.Parameters.Clear();
                    myCommand.CommandText = "DELETE FROM a_product WHERE PID=@PID";
                    myCommand.Parameters.AddWithValue("@PID", myPage.getProductID());
                    myResult = myCommand.ExecuteNonQuery();

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Product Information was SUCCESSFULLY DELETED.";
                    }
                    else
                    {
                        myPage.setMessageNumber(5);
                        myPage.txtMessage.Text = "The process succeeded but NO records where affected. POSSIBLE CAUSE: No data corresponds to your input. ";
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

        public void getProductInfo(String ProductID, fProductEdit myPage)
        {
            try
            {
                AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
                myConnection = myDatabase.getConnection();
                myCommand = myConnection.CreateCommand();
                myCommand.CommandText = "SELECT a_product.PID, a_product.ProductName, a_product.ProductDesc, a_supplier.CompanyName, a_category.CategoryName, a_product.QuantityPerUnit, a_product.UnitPrice, a_product.BulkPrice, a_product.UnitInStock, a_product.UnitOnOrder, a_product.UnitOnDelivery, a_product.ReorderLevel, a_product.Discontinued " +
                                           "FROM a_product INNER JOIN a_category ON a_product.CategoryID = a_category.CategoryID INNER JOIN a_supplier ON a_product.SupplierID = a_supplier.SupplierID " +
                                           "WHERE a_product.PID=@PID";
                myCommand.Parameters.AddWithValue("@PID", ProductID);
                myDataReader = myCommand.ExecuteReader();
                while (myDataReader.Read())
                {
                    myPage.txtProductName.Text = myDataReader.GetString(1);
                    myPage.txtDescription.Text = myDataReader.GetString(2);
                    myPage.cmbSupplier.Text = myDataReader.GetString(3);
                    myPage.cmbCategory.Text = myDataReader.GetString(4);
                    myPage.txtCaseSize.Text = myDataReader.GetInt32(5).ToString();
                    myPage.txtUnitPrice.Text = myDataReader.GetDecimal(6).ToString();
                    myPage.txtBulkPrice.Text = myDataReader.GetDecimal(7).ToString();
                    myPage.txtUnitsStock.Text = myDataReader.GetInt32(8).ToString();
                    myPage.txtUnitsOrder.Text = myDataReader.GetInt32(9).ToString();
                    myPage.txtUnitsDelivery.Text = myDataReader.GetInt32(10).ToString();
                    myPage.txtReorderLevel.Text = myDataReader.GetInt32(11).ToString();
                    if (myDataReader.GetInt32(12) == 0) myPage.chkDiscontinue.Checked = false;
                    else myPage.chkDiscontinue.Checked = true;
                    break;
                }
                myDataReader.Close();
            }
            catch (Exception e)
            {
                e.ToString();
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

        public int addInventory(fInventoryEdit myPage, int EmployeeID) 
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
                

                #endregion

                #region ProcessSection
                if (!hasError)
                {
                    myCommand.Parameters.Clear();

                    //--SELECT Alter Reason--
                    String sAlterReason = "";
                    if (myPage.cmbAlterReason.Text.Trim().Length == 0) sAlterReason = "Others";
                    else sAlterReason = myPage.cmbAlterReason.Text.Trim();
                    myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='Inventory' AND LookUpName=@LookUpName";
                    myCommand.Parameters.AddWithValue("@LookUpName", sAlterReason);
                    int iAlterReason;
                    object temp = myCommand.ExecuteScalar();
                    if (temp == System.DBNull.Value)
                    {
                        myCommand.CommandText = "SELECT ValueId FROM c_look_up WHERE LookUpDiv='Inventory' AND LookUpName='Others'";
                        iAlterReason = (int)myCommand.ExecuteScalar();
                    }
                    else
                    {
                        iAlterReason = (int)temp;
                    }


                    //--Insert new product information--
                    myCommand.CommandText = "SELECT MAX(AlterID) + 1 FROM a_alteration";
                    int iMaxID;
                    object temp1 = myCommand.ExecuteScalar();
                    if (temp1 == System.DBNull.Value)
                    {
                        iMaxID = 1000;
                    }
                    else
                    {
                        iMaxID = (int)temp1;
                    }

                    myCommand.CommandText = "INSERT INTO a_alteration (AlterID, TransactionDate, ReferenceNo, ReferenceDate, AlterReason, Remarks, EmployeeID) " +
                                            "VALUES (@AlterID, @TransactionDate, @ReferenceNo, @ReferenceDate, @AlterReason, @Remarks, @EmployeeID) ";
                    myCommand.Parameters.AddWithValue("@AlterID", iMaxID);
                    myCommand.Parameters.AddWithValue("@TransactionDate", myPage.dtpTransactionDate.Value);
                    myCommand.Parameters.AddWithValue("@ReferenceNo", myPage.txtReferenceNo.Text.Trim());
                    if (myPage.dtpReferenceDate.Checked) myCommand.Parameters.AddWithValue("@ReferenceDate", myPage.dtpReferenceDate.Value);
                    else myCommand.Parameters.AddWithValue("@ReferenceDate", String.Empty);
                    myCommand.Parameters.AddWithValue("@AlterReason", iAlterReason);
                    myCommand.Parameters.AddWithValue("@Remarks", myPage.txtRemarks.Text.Trim());
                    myCommand.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                    myResult += myCommand.ExecuteNonQuery();

                    //--Insert Details--
                    myCommand.Parameters.AddWithValue("@PID", "");
                    myCommand.Parameters.AddWithValue("@Quantity", 0);
                    int iAdd = 0, iSubtract = 0;
                    for (int iCtr = 0; iCtr < myPage.dgvProduct.Rows.Count; iCtr++)
                    {
                        if (myPage.dgvProduct.Rows[iCtr].Displayed) { 
                            myCommand.CommandText = "INSERT INTO a_alteration_detail (AlterID, PID, Quantity) VALUES (@AlterID, @PID, @Quantity)";
                            myCommand.Parameters["@PID"].Value = myPage.dgvProduct.Rows[iCtr].Cells[0].Value.ToString();
                            
                            iAdd = Int32.Parse(myPage.dgvProduct.Rows[iCtr].Cells[4].Value.ToString());
                            iSubtract = Int32.Parse(myPage.dgvProduct.Rows[iCtr].Cells[5].Value.ToString());

                            myCommand.Parameters["@Quantity"].Value = (iAdd - iSubtract);
                            myResult += myCommand.ExecuteNonQuery();

                            myCommand.CommandText = "UPDATE a_product SET UnitInStock = UnitInStock + @Quantity WHERE PID = @PID";
                            myResult += myCommand.ExecuteNonQuery();
                        }
                    }

                    if (myResult > 0)
                    {
                        myPage.setMessageNumber(7);
                        myPage.txtMessage.Text = "Inventory Information was SUCCESSFULLY SAVED.";
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
                myResult = 0;
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
