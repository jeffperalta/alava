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
    public partial class fDeliverySearch : Form
    {

        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;

        private Boolean boolAllowChoose = false;

        public fDeliverySearch(Boolean isSearch)
        {
            InitializeComponent();
            boolAllowChoose = isSearch;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            String strQuery = "SELECT a_delivery.DeliveryID AS [Delivery ID], a_supplier.CompanyName AS Supplier, a_delivery.DeliveryReceiptNo AS [Receipt No], " +
                              "       a_delivery.TransactionDate AS [Transaction Date], (CASE WHEN a_delivery.DeliveryDate = CONVERT(DATETIME, '1900-01-01 00:00:00', 102) THEN ' ' ELSE a_delivery.DeliveryDate END) AS [Delivery Date], c_look_up.LookUpName AS Status, a_delivery.Remarks, " +
                              "       a_users.f_name + ' ' + a_users.l_name AS [Modified By] " +
                              "FROM   a_delivery LEFT OUTER JOIN " +
                              "       c_look_up ON a_delivery.Status = c_look_up.ValueId LEFT OUTER JOIN " +
                              "       a_users ON a_delivery.EmployeeID = a_users.user_id LEFT OUTER JOIN " +
                              "       a_supplier ON a_delivery.SupplierID = a_supplier.SupplierID " +
                              "WHERE  (c_look_up.LookUpDiv = 'Delivery') ";
            if (chkTransDate.Checked)
            {
                strQuery += " AND a_delivery.TransactionDate BETWEEN @TransDateStart AND @TransDateEnd ";
            }

            if (chkDeliveryDate.Checked)
            {
                strQuery += " AND a_delivery.DeliveryDate BETWEEN @DeliveryStart AND @DeliveryEnd ";
            }

            if (cmbStatus.Text.Trim() != "" && cmbStatus.Text.Trim().ToLower() != "all")
            {
                strQuery += " AND c_look_up.LookUpName = @LookUpName ";
            }

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(strQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@TransDateStart", dtpTransDateStart.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@TransDateEnd", dtpTransDateEnd.Value.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@DeliveryStart", dtpDelDateStart.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@DeliveryEnd", dtpDelDateEnd.Value.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@LookUpName", cmbStatus.Text.Trim());

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvDelivery.DataSource = null;
                dgvDelivery.DataSource = myDataView;
                txtMessage.Text = "RESULTS: " + dgvDelivery.RowCount;

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
            //Delivery ID =0; 
            //SupplierName = 1; 
            //DeliveryReceiptNo = 2; 
            //TransactionDate = 3; 
            //DeliveryDate = 4; 
            //Status = 5; 
            //Remarks = 6; 
            //ModifiedBy =7
            if (boolAllowChoose && dgvDelivery.RowCount != 0 && dgvDelivery.SelectedRows.Count !=0)
            {
                this.Visible = false;
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fDeliveryEdit") 
                {
                    AlavaSoft.Transaction.Delivery.fDeliveryEdit myPage = (AlavaSoft.Transaction.Delivery.fDeliveryEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.ParameterHeader(dgvDelivery.CurrentRow.Cells[0].Value.ToString(),
                                     dgvDelivery.CurrentRow.Cells[2].Value.ToString(),
                                     dgvDelivery.CurrentRow.Cells[5].Value.ToString(),
                                     dgvDelivery.CurrentRow.Cells[6].Value.ToString(),
                                     dgvDelivery.CurrentRow.Cells[3].Value.ToString(),
                                     dgvDelivery.CurrentRow.Cells[4].Value.ToString(),
                                     dgvDelivery.CurrentRow.Cells[1].Value.ToString());
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fDeliveryReceipt")
                {
                    AlavaSoft.Reports.Delivery.fDeliveryReceipt myPage = (AlavaSoft.Reports.Delivery.fDeliveryReceipt)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setDeliveryID(int.Parse(dgvDelivery.CurrentRow.Cells[0].Value.ToString()));
                }

                //--Insert Interface Here--

                this.Close();
            }
        }

        private void fDeliverySearch_Load(object sender, EventArgs e)
        {
            btnChoose.Visible = boolAllowChoose;
        }

        private void dgvDelivery_DoubleClick(object sender, EventArgs e)
        {
            if (boolAllowChoose && dgvDelivery.SelectedRows.Count > 0) 
            {
                this.btnChoose_Click(sender, e);
            }
        }

    }
}
