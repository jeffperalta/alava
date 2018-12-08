using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Search
{
    public partial class fSupplierSearch : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;

        public fSupplierSearch()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            tabControl2.SelectedTab = tabPage3;

            String sQuery = "SELECT SupplierID AS [Supplier ID], CompanyName AS [Supplier Name], ContactTitle AS [Contact Title], ContactName AS [Contact Name], TINNo, Address1, Address2, Address3, Contact1, Contact2, Contact3, Description, " +
                            "       (CASE WHEN Discontinue = 1 THEN 'Discontinue' ELSE 'Active' END) AS Status " +
                            "FROM   a_supplier " +
                            "WHERE  (0 = 0) ";
            
            if (txtCompanyName.Text.Trim().Length > 0)
            {
                sQuery += " AND CompanyName LIKE @CompanyName ";
            }

            if (txtCompanyAddress.Text.Trim().Length > 0)
            {
                sQuery += " AND (Address1 LIKE @Address OR Address2 LIKE @Address OR Address3 LIKE @Address) ";
            }

            if (txtContactNo.Text.Trim().Length > 0)
            {
                sQuery += " AND (Contact1 LIKE @Contact OR Contact2 LIKE @Contact OR Contact3 LIKE @Contact) ";
            }

            if (chkActive.Checked)
            {
                sQuery += " AND Discontinue = 0 ";
            }
            else
            {
                sQuery += " AND Discontinue = 1 ";
            }

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(sQuery, myConnection);
                mySqlCommand.Parameters.Clear();

                if (txtCompanyName.Text.Trim().Length < 2)
                {
                    mySqlCommand.Parameters.AddWithValue("@CompanyName", txtCompanyName.Text.Trim() + "%");
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@CompanyName", "%" + txtCompanyName.Text.Trim() + "%");
                }

                if (txtCompanyAddress.Text.Trim().Length < 2)
                {
                    mySqlCommand.Parameters.AddWithValue("@Address", txtCompanyAddress.Text.Trim() + "%");
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@Address", "%" + txtCompanyAddress.Text.Trim() + "%");
                }

                if (txtContactNo.Text.Trim().Length < 2)
                {
                    mySqlCommand.Parameters.AddWithValue("@Contact", txtContactNo.Text.Trim() + "%");
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@Contact", "%" + txtContactNo.Text.Trim() + "%");
                }
                
                //--YOU ARE HERE--

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvSupplier.DataSource = null;
                dgvSupplier.DataSource = myDataView;
                txtMessage.Text = "[RESULTS: " + dgvSupplier.RowCount + "] ";

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

        private void fSupplierSearch_Load(object sender, EventArgs e)
        {

        }

      
    }
}
