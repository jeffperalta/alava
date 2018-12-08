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
    public partial class fRefundSearch : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;
        Boolean allowChoose = false;

        public fRefundSearch(Boolean allowChoose)
        {
            InitializeComponent();
            this.allowChoose = allowChoose;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            tabControl1.SelectedTab = tabPage1;
            
            String sQueryString = "SELECT a_refund.RefundID AS [Refund ID], a_refund.RefundSlipNo AS [Refund Slip No], a_refund.RefundDate AS [Transaction Date], " +
                                  "       a_refund.Amount AS [Refund Amount], a_refund.AmountReleased AS [Amount Released], c_look_up.LookUpName AS Status,  " +
                                  "       a_sales_order.SalesOrderNo AS [Sales Order No], a_customer.CompanyName AS Customer, a_refund.ReceivedBy AS [Received By], a_refund.Remarks,  " +
                                  "       a_users.f_name + ' ' + a_users.l_name AS [Last Edit By], a_sales_order.PaidAmount AS [Paid Amount], a_sales_order.RefundAmount AS [Refund Amount], a_sales_order.Amount-a_sales_order.ReturnAmount AS [Billing Amount] " +
                                  "FROM   a_refund INNER JOIN a_sales_order ON a_refund.SalesOrderNo = a_sales_order.SalesOrderNo INNER JOIN " +
                                  "       a_customer ON a_sales_order.CustomerID = a_customer.CustomerID INNER JOIN " +
                                  "       c_look_up ON a_refund.Status = c_look_up.ValueId INNER JOIN " +
                                  "       a_users ON a_refund.EmployeeID = a_users.user_id " +
                                  "WHERE  (c_look_up.LookUpDiv = 'REFUND') ";
            
            if (txtSalesOrderNo.Text.Trim().Length > 0)
            {
                sQueryString += " AND a_sales_order.SalesOrderNo=@SalesOrderNo ";
            }

            if (chkTransactionDate.Checked)
            {
                sQueryString += " AND a_refund.RefundDate BETWEEN @RefundDate1 AND @RefundDate2 ";
            }

            if (txtRefundSlipNo.Text.Trim().Length > 0)
            {
                sQueryString += " AND a_refund.RefundSlipNo = @RefundSlipNo ";
            }

            if (cmbStatus.Text.Trim().Length > 0 && cmbStatus.Text.Trim().ToLower() != "all")
            {
                sQueryString += " AND c_look_up.LookUpName=@Status ";
            }

            if (cmbCustomer.Text.Trim().Length > 0)
            {
                sQueryString += " AND a_customer.CompanyName=@Customer ";
            }

            if (chkWithRelease.Checked)
            {
                sQueryString += " AND a_refund.Amount > a_refund.AmountReleased ";
            }

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(sQueryString, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@SalesOrderNo", txtSalesOrderNo.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@RefundDate1", dtpTS.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@RefundDate2", dtpTE.Value.Date.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@RefundSlipNo", txtRefundSlipNo.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@Status", cmbStatus.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@Customer", cmbCustomer.Text.Trim());

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvRefund.DataSource = null;
                dgvRefund.DataSource = myDataView;
                txtMessage.Text = "RESULTS: " + dgvRefund.RowCount + " ";

                //--RefundID            0--
                //--RefundSlipNo        1--
                //--RefundDate          2--
                //--Amount              3--
                //--AmountReleased      4--
                //--Status              5--
                //--SalesOrderNo        6--
                //--CustomerName        7--
                //--ReceivedBy          8--
                //--Remarks             9--
                //--LastEditBy          10--
                //--SO Payment amount   11--
                //--Refund Amount       12--
                //--SOAmount            13--
                double dTotalRefund = 0.00, dAmountReleased = 0.00, dTotalForRelease = 0.00;
                for (int iCtr = 0; iCtr < dgvRefund.Rows.Count; iCtr++)
                {
                    dTotalRefund += double.Parse(dgvRefund.Rows[iCtr].Cells[3].Value.ToString());
                    dAmountReleased += double.Parse(dgvRefund.Rows[iCtr].Cells[4].Value.ToString());
                    
                    if(dgvRefund.Rows[iCtr].Cells[5].Value.ToString().Trim().ToLower() == "unreleased")
                    {
                        dTotalForRelease+=(double.Parse(dgvRefund.Rows[iCtr].Cells[3].Value.ToString()) - double.Parse(dgvRefund.Rows[iCtr].Cells[4].Value.ToString()));
                    }
                }
                dgvRefund.Columns[11].Visible = false;
                dgvRefund.Columns[12].Visible = false;
                dgvRefund.Columns[13].Visible = false;
                txtMessage.Text = "[Total Amount: " + dTotalRefund.ToString("n") + "] [Total Amount Released: " + dAmountReleased.ToString("n") + "] [For Release: " + dTotalForRelease.ToString("n") + "]";

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

        private void fRefundSearch_Load(object sender, EventArgs e)
        {
            btnChoose.Visible = allowChoose;
            AlavaSoft.Class.Database myDatebase = new AlavaSoft.Class.Database();
            myDatebase.showCombo(cmbCustomer, "SELECT CompanyName FROM a_customer UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='BLANK'", "CompanyName");
            cmbCustomer.Text = "";
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            //--RefundID            0--
            //--RefundSlipNo        1--
            //--RefundDate          2--
            //--Amount              3--
            //--AmountReleased      4--
            //--Status              5--
            //--SalesOrderNo        6--
            //--CustomerName        7--
            //--ReceivedBy          8--
            //--Remarks             9--
            //--LastEditBy          10--
            //--SO Payment amount   11--
            //--Refund Amount       12--
            //--SOAmount            13--
            if (allowChoose && dgvRefund.Rows.Count > 0 && dgvRefund.SelectedRows.Count > 0 && dgvRefund.CurrentRow != null)
            {
                this.Visible = false;
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fRefundEdit")
                {
                    AlavaSoft.Transaction.Refund.fRefundEdit myPage = (AlavaSoft.Transaction.Refund.fRefundEdit)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.ParameterHeader(int.Parse(dgvRefund.CurrentRow.Cells[0].Value.ToString()), dgvRefund.CurrentRow.Cells[1].Value.ToString(),
                                           dgvRefund.CurrentRow.Cells[6].Value.ToString(), dgvRefund.CurrentRow.Cells[8].Value.ToString(),
                                           dgvRefund.CurrentRow.Cells[9].Value.ToString(), dgvRefund.CurrentRow.Cells[2].Value.ToString(),
                                           double.Parse(dgvRefund.CurrentRow.Cells[3].Value.ToString()), double.Parse(dgvRefund.CurrentRow.Cells[4].Value.ToString()),
                                           dgvRefund.CurrentRow.Cells[5].Value.ToString(), double.Parse(dgvRefund.CurrentRow.Cells[11].Value.ToString()),
                                           double.Parse(dgvRefund.CurrentRow.Cells[12].Value.ToString()), double.Parse(dgvRefund.CurrentRow.Cells[13].Value.ToString()));
                }

                //--Insert Interface Her--
                this.Close();
            }
        }

        private void dgvRefund_DoubleClick(object sender, EventArgs e)
        {
            btnChoose_Click(sender, e);
        }

       
    }
}
