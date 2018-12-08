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
    public partial class fReceiptSearch : Form
    {
        Boolean blnAllowChoose = false;
        private double dAmount = 0.00;
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;

        public fReceiptSearch(Boolean AllowChoose)
        {
            InitializeComponent();
            blnAllowChoose = AllowChoose;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            //--Construct SQL Text--
            String mySqlText = "SELECT a_payment.PaymentID AS [Payment ID], " + 
                               "       a_payment.ReceiptNo AS [Receipt No], " +     
                               "       a_payment.TransactionDate AS [Transaction Date], " + 
                               "       a_payment.Amount AS [Amount],  " +
                               "       a_payment.PaidBy AS [Paid By], " + 
                               "       a_payment.ReceivedBy AS [Received By], " + 
                               "       a_users.f_name + ' ' + a_users.l_name AS [Prepared By] " +
                               "FROM   a_payment INNER JOIN a_users ON a_payment.EmployeeID = a_users.user_id INNER JOIN a_payment_detail ON a_payment.PaymentID = a_payment_detail.PaymentID " +
                               "WHERE  (a_payment.Void = 0) ";

            if (txtReceiptNo.Text.Trim().Length > 0)
            {
                mySqlText += " AND a_payment.ReceiptNo LIKE @ReceiptNo ";
            }

            if (dtpTransactionDate.Checked)
            {
                mySqlText += " AND a_payment.TransactionDate BETWEEN @TransactionDate1 AND @TransactionDate2 ";
            }

            if (txtPaidBy.Text.Trim().Length > 0)
            {
                mySqlText += " AND a_payment.PaidBy LIKE @PaidBy ";
            }

            if (txtSalesOrder.Text.Trim().Length > 0)
            {
                mySqlText += " AND a_payment_detail.SalesOrderNo=@SalesOrderNo ";
            }

            mySqlText += " GROUP BY a_payment.PaymentID, a_payment.ReceiptNo, a_payment.TransactionDate, " +
                         "          a_payment.Amount, a_payment.PaidBy, a_payment.ReceivedBy, a_users.f_name + ' ' + a_users.l_name ";
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(mySqlText, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@ReceiptNo", txtReceiptNo.Text.Trim() + "%");
                mySqlCommand.Parameters.AddWithValue("@TransactionDate1", dtpTransactionDate.Value.Date);
                mySqlCommand.Parameters.AddWithValue("@TransactionDate2", dtpTransactionDate.Value.Date.AddHours(23).AddMinutes(59));
                if (txtPaidBy.Text.Trim().Length <= 2)
                {
                    mySqlCommand.Parameters.AddWithValue("@PaidBy", txtPaidBy.Text.Trim() + "%");
                }
                else
                {
                    mySqlCommand.Parameters.AddWithValue("@PaidBy", "%"+ txtPaidBy.Text.Trim() + "%");
                }
                mySqlCommand.Parameters.AddWithValue("@SalesOrderNo", txtSalesOrder.Text.Trim());

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvPayment.DataSource = null;
                dgvPayment.DataSource = myDataView;
                txtMessage.Text = "RESULTS: " + dgvPayment.RowCount;

                //--Payment ID          0--
                //--Receipt No          1--
                //--Transaction Date    2--
                //--Amount              3--
                //--Paid By             4--
                //--Received By         5--
                //--Prepared By         6--
                dAmount = 0;
                for (int iCtr = 0; iCtr < dgvPayment.Rows.Count; iCtr++)
                {
                    dAmount += double.Parse(dgvPayment.Rows[iCtr].Cells[3].Value.ToString());
                }
                txtMessage.Text = "[Total Payment Amount: " + dAmount.ToString("n") + "]";

                dgvPayment.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
            
                //--Update 2 other datagrid--
                dgvPayment.Focus();
                if (dgvPayment.Rows.Count > 0 && dgvPayment.SelectedRows.Count > 0 && dgvPayment.CurrentRow != null)
                {
                    displayPaymentFor(int.Parse(dgvPayment.CurrentRow.Cells[0].Value.ToString()));
                    displayPaymentDetails(int.Parse(dgvPayment.CurrentRow.Cells[0].Value.ToString()));
                }
                else
                {
                    dgvSalesOrder.DataSource = null;
                    dgvPaymentDetails.DataSource = null;
                    dgvSalesOrder.Rows.Clear();
                    dgvPaymentDetails.Rows.Clear();
                }

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

        private void displayPaymentFor(int PaymentID)
        {
            String strQuery = "SELECT a_sales_order.SalesOrderNo AS [SO No], a_customer.CompanyName AS Customer, a_sales_order.TransactionDate AS [Transaction Date], " +
                              "       a_sales_order.Amount - a_sales_order.ReturnAmount AS [Billing Amount], a_payment_detail.Amount AS [Applied Amount] " +
                              "FROM   a_payment_detail INNER JOIN " +
                              "       a_sales_order ON a_payment_detail.SalesOrderNo = a_sales_order.SalesOrderNo INNER JOIN " +
                              "       a_customer ON a_sales_order.CustomerID = a_customer.CustomerID " +
                              "WHERE  (a_payment_detail.PaymentID = @PaymentID) ";

            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(strQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@PaymentID", PaymentID);
               
                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvSalesOrder.DataSource = null;
                dgvSalesOrder.DataSource = myDataView;

                dgvSalesOrder.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                dgvSalesOrder.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;

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

        }

        private void fReceiptSearch_Load(object sender, EventArgs e)
        {
            btnChoose.Visible = blnAllowChoose;
        }

        private void displayPaymentDetails(int PaymentID)
        {
            String strQuery = "SELECT a_check_payment.CheckNo, a_check_payment.CheckBank, a_check_payment.CheckType, a_check_payment.MaturityDate, " +
                              "       a_payment_type_detail.Amount " +
                              "FROM   a_payment_type_detail INNER JOIN " +
                              "       a_check_payment ON a_payment_type_detail.PayTypeDetailID = a_check_payment.PayTypeID " +
                              "WHERE  (a_payment_type_detail.PaymentType = 'CHECK') AND (a_payment_type_detail.PaymentID = @PaymentID) ";
            
            AlavaSoft.Class.Database myDatabase = new AlavaSoft.Class.Database();
            try
            {
                myConnection = myDatabase.getConnection();
                mySqlCommand = new SqlCommand(strQuery, myConnection);
                mySqlCommand.Parameters.Clear();
                mySqlCommand.Parameters.AddWithValue("@PaymentID", PaymentID);

                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);
                myDataView.Table = myDataTable[0];

                dgvPaymentDetails.DataSource = null;
                dgvPaymentDetails.DataSource = myDataView;

                dgvPaymentDetails.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight;
                double dTotalCheck = 0.00;
                for (int iCtr = 0; iCtr < dgvPaymentDetails.Rows.Count; iCtr++)
                {
                    dTotalCheck += double.Parse(dgvPaymentDetails.Rows[iCtr].Cells[4].Value.ToString());
                }
                
                //--Get Cash Payment--
                mySqlCommand.CommandText = "SELECT Amount FROM a_payment_type_detail WHERE PaymentID=@PaymentID AND PaymentType='CASH'";
                double dCash = 0.00; 
                object temp = mySqlCommand.ExecuteScalar();
                if (temp != System.DBNull.Value)
                {
                    dCash = double.Parse(temp.ToString());
                }

                txtMessage.Text = "[Total Payment Amount: " + dAmount.ToString("n") + "] [Total Check: " + dTotalCheck.ToString("n") + "] [Total Cash: " + dCash.ToString("n") +"]";

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
        }

        private void dgvPayment_MouseClick(object sender, MouseEventArgs e)
        {
            if (dgvPayment.Rows.Count > 0 && dgvPayment.SelectedRows.Count > 0 && dgvPayment.CurrentRow != null)
            {
                displayPaymentFor(int.Parse(dgvPayment.CurrentRow.Cells[0].Value.ToString()));
                displayPaymentDetails(int.Parse(dgvPayment.CurrentRow.Cells[0].Value.ToString()));
            }
            else
            {
                dgvSalesOrder.DataSource = null;
                dgvPaymentDetails.DataSource = null;
                dgvSalesOrder.Rows.Clear();
                dgvPaymentDetails.Rows.Clear();
            }

        }

        private void btnChoose_Click(object sender, EventArgs e)
        {
            if (blnAllowChoose && dgvPayment.Rows.Count > 0 && dgvPayment.SelectedRows.Count > 0 && dgvPayment.CurrentRow != null)
            {
                this.Visible = false;
                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fPaymentVoid")
                {
                    AlavaSoft.Transaction.Payment.fPaymentVoid myPage = (AlavaSoft.Transaction.Payment.fPaymentVoid)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setPaymentID(int.Parse(dgvPayment.CurrentRow.Cells[0].Value.ToString()));
                }

                if (Program.ofAlavaSoft.ActiveMdiChild.Name == "fPaymentReciept")
                {
                    AlavaSoft.Reports.Payment.fPaymentReciept myPage = (AlavaSoft.Reports.Payment.fPaymentReciept)Program.ofAlavaSoft.ActiveMdiChild;
                    myPage.setPaymentID(int.Parse(dgvPayment.CurrentRow.Cells[0].Value.ToString()), dgvPayment.CurrentRow.Cells[1].Value.ToString());
                }

                //--Insert Form Here--
                this.Close();
            }
        }

        private void dgvPayment_DoubleClick(object sender, EventArgs e)
        {
            btnChoose_Click(sender, e);
        }

    }
}
