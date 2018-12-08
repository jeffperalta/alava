using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace AlavaSoft.Search
{
    public partial class fProductSearch : Form
    {
        private Boolean blnAllowChoose = false;
        private String sSupplier = " ";
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;

        public fProductSearch(Boolean isSearch)
        {
            InitializeComponent();
            blnAllowChoose = isSearch;
        }

        public fProductSearch(Boolean isSearch, String Supplier)
        {
            InitializeComponent();
            blnAllowChoose = isSearch;
            sSupplier = Supplier;

        }

        private void fProductSearch_Load(object sender, EventArgs e)
        {
            btnChoose.Visible = blnAllowChoose;
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbCategory, "SELECT CategoryName FROM a_category UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='BLANK'", "CategoryName");
            myDatabase.showCombo(cmbSupplier, "SELECT CompanyName FROM a_supplier UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='BLANK'", "CompanyName");
            cmbCategory.Text = " ";
            cmbSupplier.Text = sSupplier;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            tabControl1.SelectedTab = tabPage1;
            
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            String sqlQuery = "", sWhere = "";
            sqlQuery += "SELECT a_product.PID AS [Product ID], a_category.CategoryName AS [Category], a_product.ProductName AS [Name], a_product.QuantityPerUnit AS [Case Size], a_product.UnitPrice AS [Unit Price], " +
                        "       a_product.BulkPrice AS [Bulk Price], a_product.UnitInStock AS [Stock (PC)], a_product.UnitInStock/a_product.QuantityPerUnit AS [Stock (CS)], a_product.UnitOnOrder AS [Reserved], a_product.UnitOnDelivery AS [Delivery], a_product.ReorderLevel AS [Reorder Level], a_supplier.CompanyName AS [Supplier], a_product.ProductDesc AS [Product Description]" +
                        "FROM a_product INNER JOIN a_category ON a_product.CategoryID = a_category.CategoryID INNER JOIN a_supplier ON a_product.SupplierID = a_supplier.SupplierID ";
            
            //--Product Name--
            if (txtProductName.Text.Trim().Length != 0) 
            { 
                if (sWhere.Trim().Length != 0) sWhere += " AND "; 
                sWhere += " a_product.ProductName LIKE @ProductName "; 
            }

            //--Product Code--
            if (txtProductCode.Text.Trim().Length != 0)
            {
                if (sWhere.Trim().Length != 0) sWhere += " AND ";
                sWhere += " a_product.PID = @PID "; 
            }

            //--Product Category--
            if (cmbCategory.Text.Trim().Length != 0)
            {
                if (sWhere.Trim().Length != 0) sWhere += " AND ";
                sWhere += " a_category.CategoryName = @CategoryName "; 
            }

            //--Supplier--
            if (cmbSupplier.Text.Trim().Length != 0)
            {
                if (sWhere.Trim().Length != 0) sWhere += " AND ";
                sWhere += " a_supplier.CompanyName = @CompanyName "; 
            }

            //--Status--
            if (cmbStatus.Text.Trim().Length != 0)
            {
                if (cmbStatus.Text.Trim().ToLower() == "active") 
                {
                    if (sWhere.Trim().Length != 0) sWhere += " AND ";
                    sWhere += " a_product.Discontinued = 0 "; 
                }

                if (cmbStatus.Text.Trim().ToLower() == "discontinued") 
                {
                    if (sWhere.Trim().Length != 0) sWhere += " AND ";
                    sWhere += " a_product.Discontinued = 1 "; 
                }
            }

            //--REORDER Level--
            if (chkReorder.Checked)
            {
                if (sWhere.Trim().Length != 0) sWhere += " AND ";
                sWhere += " ((a_product.UnitInStock - a_product.UnitOnOrder) < a_product.ReorderLevel) "; 
            }

            //--Below the REQUIRED SALES QUANTITY Level--
            if (chkSO.Checked)
            {
                if (sWhere.Trim().Length != 0) sWhere += " AND ";
                sWhere += " (a_product.UnitInStock < a_product.UnitOnOrder) "; 
            }

            if (sWhere.Trim().Length != 0) { sWhere = " WHERE " + sWhere; }


            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(sqlQuery + sWhere, myConnection);
                mySqlCommand.Parameters.Clear();
                
                if (txtProductName.Text.Trim().Length <= 2) mySqlCommand.Parameters.AddWithValue("@ProductName", txtProductName.Text.Trim() + "%");
                else mySqlCommand.Parameters.AddWithValue("@ProductName", "%" + txtProductName.Text.Trim() + "%");
                
                mySqlCommand.Parameters.AddWithValue("@PID", txtProductCode.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@CategoryName", cmbCategory.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@CompanyName", cmbSupplier.Text.Trim());
                
                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvProduct.DataSource = null;
                dgvProduct.DataSource = myDataView;
                txtMessage.Text = "RESULTS: " + dgvProduct.RowCount;
                
                dgvProduct.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[7].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvProduct.Columns[10].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }
            }
            Program.ofAlavaSoft.endProgress();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            //Product ID            0
            //Category              1
            //Name                  2
            //Case Size             3   
            //Unit Price            4
            //Bulk Price            5
            //Stock PC              6
            //Stock CS              7
            //IN order (reserved)   8
            //In Delivery           9
            //Reorder               10
            //Supplier              11
            //Description           12
            if (blnAllowChoose && dgvProduct.RowCount != 0 && dgvProduct.SelectedRows.Count !=0)
            {
                this.Visible = false;

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fProductEdit")
                {
                    AlavaSoft.Transaction.Product.fProductEdit myPage = (AlavaSoft.Transaction.Product.fProductEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setProductID(dgvProduct.CurrentRow.Cells[0].Value.ToString());
                    
                }
                
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fInventoryEdit") 
                {
                    AlavaSoft.Transaction.Product.fInventoryEdit myPage = (AlavaSoft.Transaction.Product.fInventoryEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    int iPC = int.Parse(dgvProduct.CurrentRow.Cells[6].Value.ToString());
                    int iCaseSize = int.Parse(dgvProduct.CurrentRow.Cells[3].Value.ToString());
                    myPage.Parameter(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), iPC, iCaseSize, dgvProduct.CurrentRow.Cells[1].Value.ToString());
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fDeliveryAdd") 
                {
                    AlavaSoft.Transaction.Delivery.fDeliveryAdd myPage = (AlavaSoft.Transaction.Delivery.fDeliveryAdd)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.Parameter(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), dgvProduct.CurrentRow.Cells[3].Value.ToString(), dgvProduct.CurrentRow.Cells[1].Value.ToString(), dgvProduct.CurrentRow.Cells[11].Value.ToString());
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fDeliveryEdit")
                {
                    AlavaSoft.Transaction.Delivery.fDeliveryEdit myPage = (AlavaSoft.Transaction.Delivery.fDeliveryEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.ParameterDetail(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[1].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), int.Parse(dgvProduct.CurrentRow.Cells[3].Value.ToString()), dgvProduct.CurrentRow.Cells[11].Value.ToString());
                }
                
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderAdd")
                {
                    AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd myPage = (AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.ParameterDetails(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[1].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), int.Parse(dgvProduct.CurrentRow.Cells[3].Value.ToString()), double.Parse(dgvProduct.CurrentRow.Cells[4].Value.ToString())); 
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderEdit")
                {
                    AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit myPage = (AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.ParameterDetails(dgvProduct.CurrentRow.Cells[0].Value.ToString(), dgvProduct.CurrentRow.Cells[1].Value.ToString(), dgvProduct.CurrentRow.Cells[2].Value.ToString(), int.Parse(dgvProduct.CurrentRow.Cells[3].Value.ToString()), double.Parse(dgvProduct.CurrentRow.Cells[4].Value.ToString())); 
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fProductInventory")
                {
                    AlavaSoft.Reports.Product.fProductInventory myPage = (AlavaSoft.Reports.Product.fProductInventory)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.txtProductCode.Text = dgvProduct.CurrentRow.Cells[0].Value.ToString();
                }

                //--Insert new form here--
                this.Close();
            }
        }

        private void dgvProduct_DoubleClick(object sender, EventArgs e)
        {
            btnChoose_Click(sender, e);
        }

    }
}
