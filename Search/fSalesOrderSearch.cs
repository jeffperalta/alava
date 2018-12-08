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
    public partial class fSalesOrderSearch : Form
    {
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;
        private Boolean allowChoose = false;

        public fSalesOrderSearch(Boolean allowChoose)
        {
            InitializeComponent();
            this.allowChoose = allowChoose;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            TabControl1.SelectedTab = TabPage1;
            String strQuery = "SELECT   a_sales_order.SalesOrderNo AS [Sales Order No], a_customer.CompanyName AS Customer, a_sales_order.TransactionDate AS [Transaction Date], " +
                              "         c_look_up.LookUpName AS Status, a_sales_order.Amount AS [Billing Amount], a_sales_order.PaidAmount AS Payment, a_sales_order.Discount, " +
                              "         a_sales_order.RequiredDate AS [Required Date], a_sales_order.ShippedDate AS [Shipping Date], a_sales_order.Terms, DATEADD(DAY, " +
                              "         a_sales_order.Terms, a_sales_order.ShippedDate) AS [Due Date], a_sales_order.ReturnAmount AS [Return Amount], " +
                              "         a_sales_order.RefundAmount AS [Refund Amount], a_sales_order.Remarks, a_sales_order.DestinationAddress AS [Delivery Address], " +
                              "         a_users.f_name + ' ' + a_users.l_name AS [Last Edit By] " +
                              "FROM     a_sales_order LEFT OUTER JOIN " +
                              "         a_customer ON a_sales_order.CustomerID = a_customer.CustomerID LEFT OUTER JOIN " +
                              "         a_users ON a_sales_order.EmployeeID = a_users.user_id LEFT OUTER JOIN " +
                              "         c_look_up ON a_sales_order.Status = c_look_up.ValueId " +
                              "WHERE    (c_look_up.LookUpDiv = 'SalesOrder') ";

            if (txtSalesOrder.Text.Trim().Length > 0)
            {
                strQuery += " AND a_sales_order.SalesOrderNo=@SalesOrderNo ";
            }

            if (cmbCustomer.Text.Trim().Length > 0)
            {
                strQuery += " AND a_customer.CompanyName=@CompanyName ";
            }

            if (chkTransactioDate.Checked)
            {
                strQuery += " AND a_sales_order.TransactionDate BETWEEN @TS AND @TE ";
            }

            if (chkRequiredDate.Checked)
            {
                strQuery += " AND a_sales_order.RequiredDate BETWEEN @RS AND @RE ";
            }

            if (chkShippingDate.Checked)
            {
                strQuery += " AND a_sales_order.ShippedDate BETWEEN @SS AND @SE ";
            }

            if (chkOverdue.Checked)
            {
                strQuery += " AND DATEADD(DAY, a_sales_order.Terms, a_sales_order.ShippedDate) <= @OS AND c_look_up.LookUpName='for collection' ";
            }

            if (chkForShipment.Checked || chkForCollection.Checked || chkPaid.Checked || chkUncollectible.Checked)
            {
                strQuery += " AND ( ";
                Boolean hasStatus = false;
                if(chkForShipment.Checked) 
                {
                    if (hasStatus) strQuery += " OR ";
                    strQuery += " c_look_up.LookUpName='For Shipment' ";
                    hasStatus = true;
                }

                if (chkForCollection.Checked)
                {
                    if (hasStatus) strQuery += " OR ";
                    strQuery += " c_look_up.LookUpName='For Collection' ";
                    hasStatus = true;
                }

                if (chkPaid.Checked)
                {
                    if (hasStatus) strQuery += " OR ";
                    strQuery += " c_look_up.LookUpName='Paid' ";
                    hasStatus = true;
                }

                if (chkUncollectible.Checked)
                {
                    if (hasStatus) strQuery += " OR ";
                    strQuery += " c_look_up.LookUpName='Uncollectible' ";
                    hasStatus = true;
                }

                strQuery += " ) ";
            }

            if (chkExcessPayment.Checked)
            {
                strQuery += " AND (a_sales_order.Amount-a_sales_order.PaidAmount-a_sales_order.ReturnAmount+a_sales_order.RefundAmount)<0  ";
            }

            if (chkProductReturns.Checked)
            {
                strQuery += " AND a_sales_order.ReturnAmount >0 ";
            }

            if (chkWithBalance.Checked)
            {
                strQuery += " AND (a_sales_order.Amount-a_sales_order.PaidAmount-a_sales_order.ReturnAmount)>0 "; 
            }

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(strQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@SalesOrderNo", txtSalesOrder.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@CompanyName", cmbCustomer.Text.Trim());
                mySqlCommand.Parameters.AddWithValue("@TS", dtpTS.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@TE", dtpTE.Value.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@RS", dtpRS.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@RE", dtpRE.Value.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@SS", dtpSS.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@SE", dtpSE.Value.AddHours(23).AddMinutes(59));
                mySqlCommand.Parameters.AddWithValue("@OS", dtpOS.Value.Date);

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvSalesOrder.DataSource = null;
                dgvSalesOrder.DataSource = myDataView;
                txtMessage.Text = "RESULTS: " + dgvSalesOrder.RowCount;

                //Sales Order No            0
                //Customer Name             1
                //Transaction Date          2
                //Status                    3
                //Billing Amount            4
                //Payment                   5
                //Discount                  6
                //Required Date             7
                //Shipping Date             8
                //Terms                     9
                //Due Date                  10
                //Return Amount             11
                //Refund Amount             12
                //Remarks                   13
                //Delivery Address          14
                //Last Edit By              15
                dgvSalesOrder.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvSalesOrder.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvSalesOrder.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvSalesOrder.Columns[9].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvSalesOrder.Columns[11].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvSalesOrder.Columns[12].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

                //--Compute Summary--
                double dAmount = 0.00, dPayment = 0.00, dTotalBalance = 0.00, dTotalExcess = 0.00, dTotalRefund=0.00;
                for (int iCtr = 0; iCtr < dgvSalesOrder.Rows.Count; iCtr++)
                {
                    dAmount += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[4].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[11].Value.ToString());
                    dPayment += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString());

                    //if (double.Parse(dgvSalesOrder.Rows[iCtr].Cells[4].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString()) > 0)
                    //{
                    //    dTotalBalance += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[4].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString());
                    //}

                    if (double.Parse(dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[4].Value.ToString()) + double.Parse(dgvSalesOrder.Rows[iCtr].Cells[11].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[12].Value.ToString()) > 0)
                    {
                        dTotalExcess += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[5].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[4].Value.ToString()) + double.Parse(dgvSalesOrder.Rows[iCtr].Cells[11].Value.ToString()) - double.Parse(dgvSalesOrder.Rows[iCtr].Cells[12].Value.ToString());
                    }

                    dTotalRefund += double.Parse(dgvSalesOrder.Rows[iCtr].Cells[12].Value.ToString());

                }

                dTotalBalance = dAmount - dPayment;
                if (dTotalBalance < 0)
                {
                    dTotalBalance = 0;
                }

                txtMessage.Text = "[Total Billing: " + dAmount.ToString("n") + "] [Total Payment: " + dPayment.ToString("n") + "] [Total Balance: " + dTotalBalance.ToString("n") + "] [Total Excess: " + dTotalExcess.ToString("n") + "] [Total Refund: " + dTotalRefund.ToString("n") + "] ";

            }
            catch (Exception ex)
            {
                txtMessage.Text = ex.ToString();
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

        private void fSalesOrderSearch_Load(object sender, EventArgs e)
        {
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            myDatabase.showCombo(cmbCustomer, "SELECT CompanyName FROM a_customer UNION SELECT LookUpName FROM c_look_up WHERE LookUpDiv='Blank'", "CompanyName");
            cmbCustomer.Text = " ";
            btnChoose.Visible = allowChoose;
        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (allowChoose)
            {
                if (dgvSalesOrder.Rows.Count > 0 && dgvSalesOrder.SelectedRows.Count > 0)
                {
                    this.Visible = false;
                    //Sales Order No            0
                    //Customer Name             1
                    //Transaction Date          2
                    //Status                    3
                    //Billing Amount            4
                    //Payment                   5
                    //Discount                  6
                    //Required Date             7
                    //Shipping Date             8
                    //Terms                     9
                    //Due Date                  10
                    //Return Amount             11
                    //Refund Amount             12
                    //Remarks                   13
                    //Delivery Address          14
                    //Last Edit By              15
                    if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fSalesOrderEdit")
                    {
                        AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit myPage = (AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit) Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.ParameterHeader(dgvSalesOrder.CurrentRow.Cells[0].Value.ToString(), dgvSalesOrder.CurrentRow.Cells[1].Value.ToString(),
                                               dgvSalesOrder.CurrentRow.Cells[14].Value.ToString(), int.Parse(dgvSalesOrder.CurrentRow.Cells[9].Value.ToString()),
                                               dgvSalesOrder.CurrentRow.Cells[13].Value.ToString(), dgvSalesOrder.CurrentRow.Cells[2].Value.ToString(),
                                               dgvSalesOrder.CurrentRow.Cells[7].Value.ToString(), dgvSalesOrder.CurrentRow.Cells[8].Value.ToString(),
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[4].Value.ToString()), 
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[5].Value.ToString()),
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[11].Value.ToString()), 
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[12].Value.ToString()),
                                               dgvSalesOrder.CurrentRow.Cells[3].Value.ToString());
                    }

                    if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fPayment")
                    {
                        double dPaymentBillingAmount = double.Parse(dgvSalesOrder.CurrentRow.Cells[4].Value.ToString()) - double.Parse(dgvSalesOrder.CurrentRow.Cells[11].Value.ToString());
                        AlavaSoft.Transaction.Payment.fPayment myPage = (AlavaSoft.Transaction.Payment.fPayment)Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.addSalesOrder(dgvSalesOrder.CurrentRow.Cells[0].Value.ToString(), 
                                             dgvSalesOrder.CurrentRow.Cells[1].Value.ToString(),
                                             dgvSalesOrder.CurrentRow.Cells[2].Value.ToString(), 
                                             dPaymentBillingAmount, 
                                             dPaymentBillingAmount - double.Parse(dgvSalesOrder.CurrentRow.Cells[5].Value.ToString()),
                                             0.00,
                                             dgvSalesOrder.CurrentRow.Cells[3].Value.ToString());

                    }

                    if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fRefundAdd")
                    {
                        AlavaSoft.Transaction.Refund.fRefundAdd myPage = (AlavaSoft.Transaction.Refund.fRefundAdd)Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.HeaderParameter(dgvSalesOrder.CurrentRow.Cells[0].Value.ToString(),
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[4].Value.ToString()) - double.Parse(dgvSalesOrder.CurrentRow.Cells[11].Value.ToString()),
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[5].Value.ToString()),
                                               double.Parse(dgvSalesOrder.CurrentRow.Cells[12].Value.ToString()),
                                               dgvSalesOrder.CurrentRow.Cells[1].Value.ToString());
                    }

                    if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fReturnAdd")
                    {
                        AlavaSoft.Transaction.Return.fReturnAdd myPage = (AlavaSoft.Transaction.Return.fReturnAdd) Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.showSODetails(dgvSalesOrder.CurrentRow.Cells[0].Value.ToString(), dgvSalesOrder.CurrentRow.Cells[3].Value.ToString());
                    }

                    if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fRefundVoucher")
                    {
                        AlavaSoft.Reports.Refund.fRefundVoucher myPage = (AlavaSoft.Reports.Refund.fRefundVoucher)Program.ofAlavaSoft.ActiveMdiChild;
                        myPage.setSalesOrderNo(dgvSalesOrder.CurrentRow.Cells[0].Value.ToString());
                    }

                    //--Insert Interface Here--
                    this.Close();
                }
            }
        }

        private void dgvSalesOrder_DoubleClick(object sender, EventArgs e)
        {
            this.btnChoose_Click(sender, e);
        }
    }
}

