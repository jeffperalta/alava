using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AlavaSoft.Search
{
    public partial class fCustomerSearch : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;

        private Boolean blnAllowChoose = false;

        public fCustomerSearch(Boolean AllowSearch)
        {
            InitializeComponent();
            blnAllowChoose = AllowSearch;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            double dDiscount = 0.00;
            int iTerms = 0;

            tabControl2.SelectedTab  = tabPage3;
            String strQuery = "SELECT CustomerID AS [Customer ID], "  + //0
                              "       CompanyName AS [Customer Name], " + //1
                              "       CompanyAddress AS [Address], " + //2
                              "       Terms AS [Terms (days)], " + //3
                              "       Discount AS [Discount (%)], " + //4
                              "       ContactTitle AS [Contact Title], " +//5 
                              "       ContactName AS [Contact Person], " + //6
                              "       ContactPosition AS [Position], " + //7
                              "       DeliveryAddress AS [Delivery Address], " +//8 
                              "       Contact1 AS Contact1, Contact2 AS Contact2, Contact3 AS Contact3, " +
                              "       Remarks AS Remarks " +
                              "FROM a_customer ";
            
            String strWhereClause = "";
            if (txtCustomerName.Text.Trim().Length > 0)
            {
                if (strWhereClause.Trim().Length != 0) { strWhereClause += " AND "; }
                if (txtCustomerName.Text.Trim().Length < 2)
                {
                    strWhereClause += " a_customer.CompanyName LIKE @NameFirstLetter "; 
                }
                else
                {
                    strWhereClause += " a_customer.CompanyName LIKE @NameWholeWord ";
                }
            }

            if (txtAddress.Text.Trim().Length > 0)
            {
                if (strWhereClause.Trim().Length != 0) { strWhereClause += " AND "; }
                if (txtAddress.Text.Trim().Length < 2)
                {
                    strWhereClause += " (a_customer.CompanyAddress LIKE @AddressFirstLetter OR a_customer.DeliveryAddress LIKE @AddressFirstLetter) ";
                }
                else
                {
                    strWhereClause += " (a_customer.CompanyAddress LIKE @AddressWholeWord OR a_customer.DeliveryAddress LIKE @AddressWholeWord) "; 
                }
            }

            if (txtDiscount.Text.Trim().Length > 0 && double.TryParse(txtDiscount.Text.Trim(),out dDiscount))
            {
                if (strWhereClause.Trim().Length != 0) { strWhereClause += " AND "; }
                strWhereClause += " a_customer.Discount=@Discount ";
            }

            if (txtTerms.Text.Trim().Length > 0 && int.TryParse(txtTerms.Text.Trim(), out iTerms))
            {
                if (strWhereClause.Trim().Length != 0) { strWhereClause += " AND "; }
                strWhereClause += " a_customer.Terms=@Terms ";
            }

            if (strWhereClause.Trim().Length > 0)
            {
                strQuery += " WHERE " + strWhereClause;
            }

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(strQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@NameFirstLetter", txtCustomerName.Text.Trim() + "%");
                mySqlCommand.Parameters.AddWithValue("@NameWholeWord", "%" + txtCustomerName.Text.Trim() + "%");
                mySqlCommand.Parameters.AddWithValue("@AddressFirstLetter", txtAddress.Text.Trim() + "%");
                mySqlCommand.Parameters.AddWithValue("@AddressWholeWord", "%" + txtAddress.Text.Trim() + "%");
                mySqlCommand.Parameters.AddWithValue("@Discount", dDiscount);
                mySqlCommand.Parameters.AddWithValue("@Terms", iTerms);

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvCustomer.DataSource = null;
                dgvCustomer.DataSource = myDataView;
                txtMessage.Text = "RESULTS: " + dgvCustomer.RowCount;

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

        public void reset()
        {
            btnChoose.Visible = blnAllowChoose;
            txtCustomerName.Text = "";
            txtAddress.Text = "";
            txtDiscount.Text = "";
            txtTerms.Text = "";
            txtMessage.Text = "";
        }

        private void fCustomerSearch_Load(object sender, EventArgs e)
        {
            this.reset();
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            //--CustomerID          0--
            //--CompanyName         1--
            //--CompanyAddress      2--
            //--Terms               3--
            //--Discount            4--
            //--ContactTitle        5--
            //--ContactName         6--
            //--ContactPosition     7--
            //--DeliveryAddress     8--
            //--Contact1            9--
            //--Contact2            10--
            //--Contact3            11--
            //--Remarks             12--
            if (blnAllowChoose && dgvCustomer.Rows.Count !=0 && dgvCustomer.SelectedRows.Count != 0)
            {
                this.Visible = false;
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderAdd")
                {
                    AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd myPage = (AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setCustomer(int.Parse(dgvCustomer.CurrentRow.Cells[0].Value.ToString()), dgvCustomer.CurrentRow.Cells[1].Value.ToString(), dgvCustomer.CurrentRow.Cells[8].Value.ToString(), double.Parse(dgvCustomer.CurrentRow.Cells[4].Value.ToString()), int.Parse(dgvCustomer.CurrentRow.Cells[3].Value.ToString()));
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderEdit")
                {
                    AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit myPage = (AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setCustomer(int.Parse(dgvCustomer.CurrentRow.Cells[0].Value.ToString()), dgvCustomer.CurrentRow.Cells[1].Value.ToString(), dgvCustomer.CurrentRow.Cells[8].Value.ToString(), double.Parse(dgvCustomer.CurrentRow.Cells[4].Value.ToString()), int.Parse(dgvCustomer.CurrentRow.Cells[3].Value.ToString()));
                
                }

                //--Insert Form Here--
                this.Close();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            btnChoose_Click(sender, e);
        }

    }
}
