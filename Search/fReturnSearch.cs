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
    public partial class fReturnSearch : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;
        Boolean allowChoose = false;

        public fReturnSearch(Boolean allowChoose)
        {
            InitializeComponent();
            this.allowChoose = allowChoose;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            tabControl1.SelectedTab = tabPage1;

            String sQuery = "SELECT a_return.ReturnID AS [Return ID], a_return.ReturnSlipNo AS [Return Slip No], a_return.TransactionDate AS [Transaction Date], " +
                            "       a_return.SalesOrderNo AS [Sales Order No], a_return.Amount, c_look_up_1.LookUpName AS Reason, c_look_up.LookUpName AS Status, a_return.Remarks, " +
                            "       a_customer.CompanyName AS Customer, a_users.f_name + ' ' + a_users.l_name AS [Last Edit By] " +
                            "FROM   a_return INNER JOIN " +
                            "       a_sales_order ON a_return.SalesOrderNo = a_sales_order.SalesOrderNo INNER JOIN " +
                            "       a_customer ON a_sales_order.CustomerID = a_customer.CustomerID INNER JOIN " +
                            "       a_users ON a_return.EmployeeID = a_users.user_id LEFT OUTER JOIN " +
                            "       c_look_up ON a_return.Status = c_look_up.ValueId LEFT OUTER JOIN " +
                            "       c_look_up AS c_look_up_1 ON a_return.Reason = c_look_up_1.ValueId " +
                            "WHERE  (c_look_up_1.LookUpDiv = 'ReturnReason') AND (c_look_up.LookUpDiv = 'Return') ";

            if (txtSalesOrderNo.Text.Trim().Length > 0)
            {
                sQuery += " AND a_return.SalesOrderNo=@SalesOrderNo ";
            }

            if (chkTransactionDate.Checked)
            {
                sQuery += " AND a_return.TransactionDate BETWEEN @TransactionDate1 AND @TransactionDate2 ";
            }

            if (txtSlipNo.Text.Trim().Length > 0)
            {
                sQuery += " AND a_return.ReturnSlipNo=@ReturnSlipNo ";
            }

            if (cmbReason.Text.Trim().Length > 0)
            {
                sQuery += " AND c_look_up_1.LookUpName=@Reason ";
            }

            if (cmbStatus.Text.Trim().Length > 0)
            {
                sQuery += " AND c_look_up.LookUpName=@Status "; 
            }

            if (cmbCustomer.Text.Trim().Length > 0)
            {
                sQuery += " AND a_customer.CompanyName=@CustomerName ";
            }

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(sQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@SalesOrderNo", txtSalesOrderNo.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@TransactionDate1", dtpTransactionDate1.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@TransactionDate2", dtpTransactionDate2.Value.Date.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@ReturnSlipNo", txtSlipNo.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@Reason", cmbReason.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@Status", cmbStatus.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@CustomerName", cmbCustomer.Text.Trim());

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvProductReturn.DataSource = null;
                dgvProductReturn.DataSource = myDataView;
                txtMessage.Text = "[RESULTS: " + dgvProductReturn.RowCount + "] ";

                //--Return amount Total--
                double dReturnAmount = 0.00;
                for (int iCtr=0; iCtr < dgvProductReturn.Rows.Count; iCtr++)
                {
                    dReturnAmount += double.Parse(dgvProductReturn.Rows[iCtr].Cells[4].Value.ToString());
                }
                txtMessage.Text += "[Total Returns: " + dReturnAmount.ToString("n") + "]";

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

        private void fReturnSearch_Load(object sender, EventArgs e)
        {
            btnChoose.Visible = allowChoose;

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbReason, "SELECT LookUpName FROM c_look_up WHERE LookUpDiv='ReturnReason' OR LookUpDiv='Blank'", "LookUpName");
            cmbReason.Text = " ";

            myDatabase.showCombo(cmbCustomer, "SELECT CompanyName FROM a_customer UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='Blank'", "CompanyName");
            cmbCustomer.Text = " ";
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            //--ReturnID            0--
            //--ReturnSlipNo        1--
            //--Transaction Date    2--
            //--Sales Order No      3--
            //--Amount              4--
            //--Reason              5--
            //--Status              6--
            //--Remarks             7--
            //--Customer            8--
            //--Last Edit By        9--
            if (allowChoose && dgvProductReturn.Rows.Count > 0 && dgvProductReturn.SelectedRows.Count > 0 && dgvProductReturn.CurrentRow != null)
            {
                this.Visible = false;
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fReturnEdit")
                {
                    AlavaSoft.Transaction.Return.fReturnEdit myPage = (AlavaSoft.Transaction.Return.fReturnEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.ParameterHeader(int.Parse(dgvProductReturn.CurrentRow.Cells[0].Value.ToString()), dgvProductReturn.CurrentRow.Cells[1].Value.ToString(),
                                           dgvProductReturn.CurrentRow.Cells[3].Value.ToString(), dgvProductReturn.CurrentRow.Cells[7].Value.ToString(),
                                           dgvProductReturn.CurrentRow.Cells[5].Value.ToString(), dgvProductReturn.CurrentRow.Cells[2].Value.ToString(),
                                           dgvProductReturn.CurrentRow.Cells[6].Value.ToString(), double.Parse(dgvProductReturn.CurrentRow.Cells[4].Value.ToString()));
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fProductReturnVoucher")
                {
                    AlavaSoft.Reports.Returns.fProductReturnVoucher myPage = (AlavaSoft.Reports.Returns.fProductReturnVoucher)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setReturnID(int.Parse(dgvProductReturn.CurrentRow.Cells[0].Value.ToString()));
                }

                //--Insert Interface Here--
                this.Close();
            }
        }

        private void dgvProductReturn_DoubleClick(object sender, EventArgs e)
        {
            btnChoose_Click(sender, e);
        }

    }
}
