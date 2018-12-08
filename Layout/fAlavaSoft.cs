using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft
{
    public partial class fAlavaSoft : Form
    {
        private int iSalesOrder = 0;

        public fAlavaSoft()
        {
            InitializeComponent();
        }

        private void fAlavaSoft_Load(object sender, EventArgs e)
        {
            Program.MainLogInPage.ShowDialog();

            this.startProgress();

            if (Program.MainLogInPage.myLogInCredential.isValidUser())
            {
                toolStripTransaction.Enabled = true;
                menuAlava.Enabled = true;
                toolStripStatusLabel1.Text = "Log In: " + Program.MainLogInPage.myLogInCredential.getFullName();
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("SalesOrder"))
            {
                salesOrderToolStripMenuItem.Visible = false;
                toolStripSplitButton1.Visible = false;
                createNewSalesOrderToolStripMenuItem.ShowShortcutKeys = false;
                createNewSalesOrderToolStripMenuItem.ShortcutKeys = Keys.None;
                editExistingSalesOrderToolStripMenuItem.ShowShortcutKeys = false;
                editExistingSalesOrderToolStripMenuItem.ShortcutKeys = Keys.None;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("User"))
            {
                userAccountToolStripMenuItem.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Payment"))
            {
                paymentToolStripMenuItem.Visible = false;
                toolStripSplitButton2.Visible = false;
                receiveNewPaymentToolStripMenuItem.ShowShortcutKeys = false;
                receiveNewPaymentToolStripMenuItem.ShortcutKeys = Keys.None;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("VoidPayment"))
            {
                voidPaymentToolStripMenuItem.Visible = false;
                voidPaymentReceiptToolStripMenuItem.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Product"))
            {
                toolStripSplitButton6.Visible = false;
                productToolStripMenuItem1.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Inventory"))
            {
                maintainInventoryToolStripMenuItem.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Administration"))
            {
                administrationToolStripMenuItem.Visible = false;
                toolStripSplitButton6.Visible = false;
                productToolStripMenuItem1.Visible = false;
                supplierInformationToolStripMenuItem1.Visible = false;
                toolStripSplitButton7.Visible = false;
                customerInformationToolStripMenuItem1.Visible = false;
                toolStripSplitButton8.Visible = false;
                userAccountToolStripMenuItem.Visible = false;
                maintainInventoryToolStripMenuItem.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Refund"))
            {
                paymentRefundToolStripMenuItem.Visible = false; 
                toolStripSplitButton5.Visible = false;
                fileNewRefundInformationToolStripMenuItem.ShortcutKeys = Keys.None;
                fileNewRefundInformationToolStripMenuItem.ShowShortcutKeys = false;
                editExistingRefundInformationToolStripMenuItem.ShortcutKeys = Keys.None;
                editExistingRefundInformationToolStripMenuItem.ShowShortcutKeys = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Returns"))
            {
                productReturnsToolStripMenuItem.Visible = false;
                toolStripSplitButton4.Visible = false;
                createNewReturnsToolStripMenuItem.ShortcutKeys = Keys.None;
                createNewReturnsToolStripMenuItem.ShowShortcutKeys = false;
                editExistingProductReturnsToolStripMenuItem.ShortcutKeys = Keys.None;
                editExistingProductReturnsToolStripMenuItem.ShowShortcutKeys = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Supplier"))
            {
                supplierInformationToolStripMenuItem1.Visible = false;
                toolStripSplitButton7.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Reports"))
            {
                reportsToolStripMenuItem.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Query"))
            {
                queriesToolStripMenuItem.Visible = false;
                salesOrderToolStripMenuItem1.ShortcutKeys = Keys.None;
                salesOrderToolStripMenuItem2.ShortcutKeys = Keys.None;
                paymentReceiptToolStripMenuItem.ShortcutKeys = Keys.None;
                deliveryToolStripMenuItem.ShortcutKeys = Keys.None;
                productReturnsToolStripMenuItem1.ShortcutKeys = Keys.None;
                paymentRefundToolStripMenuItem1.ShortcutKeys = Keys.None;
                customerToolStripMenuItem.ShortcutKeys = Keys.None;
                supplierToolStripMenuItem.ShortcutKeys = Keys.None;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Delivery"))
            {
                deliveryToolStripMenuItem1.Visible = false;
                toolStripSplitButton3.Visible = false;
                fileNewDeliveryToolStripMenuItem.ShortcutKeys = Keys.None;
                fileNewDeliveryToolStripMenuItem.ShowShortcutKeys = false;
                editExistingDeliveryInformationToolStripMenuItem.ShortcutKeys = Keys.None;
                editExistingDeliveryInformationToolStripMenuItem.ShowShortcutKeys = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Customer"))
            {
                toolStripSplitButton8.Visible = false;
                customerInformationToolStripMenuItem1.Visible = false;
            }

            if (!Program.MainLogInPage.myLogInCredential.hasAccess("Delivery") && !Program.MainLogInPage.myLogInCredential.hasAccess("SalesOrder") &&
                !Program.MainLogInPage.myLogInCredential.hasAccess("Payment") && !Program.MainLogInPage.myLogInCredential.hasAccess("Refund") &&
                !Program.MainLogInPage.myLogInCredential.hasAccess("Returns"))
            {
                transactionsToolStripMenuItem.Visible = false;
            }

            this.endProgress();

        }

        private void fAlavaSoft_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Program.MainLogInPage.myLogInCredential.isValidUser() && MessageBox.Show("Do you want to exit the application?", "Exit Application", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void createNewSalesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd();
            childForm.MdiParent = this;
            childForm.Text = childForm.Text + " " + ++iSalesOrder;
            childForm.Show();
        }

        private void editExistingSalesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void receiveNewPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPayment();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void voidPaymentReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPaymentVoid();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void fileNewDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingDeliveryInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewReturnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingProductReturnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void fileNewRefundInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingRefundInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }



        private void salesOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void salesOrderToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSalesOrderSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void deliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fDeliverySearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void customerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fCustomerSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSupplierSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void productReturnsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReturnSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void paymentRefundToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fRefundSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void paymentReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReceiptSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewProductCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fCategoryAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingProductCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fCategoryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void maintainInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fInventoryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistintSupplierInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingCustomerInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewUserAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingUserAccountInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void userAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserAccess();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void arrangeHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void arrangeVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void closeAllDocumentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

    
      

      
        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            transactionToolStripMenuItem.Checked = !transactionToolStripMenuItem.Checked;
            this.toolStripTransaction.Visible = transactionToolStripMenuItem.Checked;
        }

        //----




        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd();
            childForm.MdiParent = this;
            childForm.Text = childForm.Text + " " + ++iSalesOrder;
            childForm.Show();
        }

        private void addSalesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd();
            childForm.MdiParent = this;
            childForm.Text = childForm.Text + " " + ++iSalesOrder;
            childForm.Show();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSalesOrderSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPayment();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void receivePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPayment();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void voidPaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPaymentVoid();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReceiptSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton3_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fDeliverySearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton4_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void newProductReturnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingReturnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReturnSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton5_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewRefundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingRefundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fRefundSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton6_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewProductToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingProductToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton7_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void newSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSupplierSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void toolStripSplitButton8_ButtonClick(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void newCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void queryToolStripMenuItem7_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fCustomerSearch(false);
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void statusReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.fSalesOrderStatusReport myPage = new AlavaSoft.Reports.fSalesOrderStatusReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void forCollectionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.fSalesOrderOverDueReport myPage = new AlavaSoft.Reports.fSalesOrderOverDueReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void customerOrderHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.fSalesOrderCustomerHistory myPage = new AlavaSoft.Reports.fSalesOrderCustomerHistory();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void periodicSalesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Payment.fPaymentPeriodicSalesReport myPage = new AlavaSoft.Reports.Payment.fPaymentPeriodicSalesReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void periodicSummaryOfSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Payment.fPaymentSummaryOfSales myPage = new AlavaSoft.Reports.Payment.fPaymentSummaryOfSales();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }


        private void periodicCollectionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Payment.fPaymentPeriodicCollectionReport myPage = new AlavaSoft.Reports.Payment.fPaymentPeriodicCollectionReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void productListPricingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Product.fProductList myPage = new AlavaSoft.Reports.Product.fProductList();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void periodicDetailedInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Product.fProductInventory myPage = new AlavaSoft.Reports.Product.fProductInventory();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void forReplenishmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            AlavaSoft.Reports.Product.fForReplenishmentReport myPage = new AlavaSoft.Reports.Product.fForReplenishmentReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
            Program.ofAlavaSoft.endProgress();
        }

        private void averageSalesPerProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            AlavaSoft.Reports.Product.fProductStatistics myPage = new AlavaSoft.Reports.Product.fProductStatistics();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
            Program.ofAlavaSoft.endProgress();
        }

        private void salesOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ofAlavaSoft.startProgress();
            AlavaSoft.Reports.SalesOrder.fSalesOrderForShipmentReport myPage = new AlavaSoft.Reports.SalesOrder.fSalesOrderForShipmentReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
            Program.ofAlavaSoft.endProgress();
        }

        private void printVoucherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.fSalesOrderVoucher myPage = new AlavaSoft.Reports.fSalesOrderVoucher();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void printReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Payment.fPaymentReciept myPage = new AlavaSoft.Reports.Payment.fPaymentReciept();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void inventoryAlterationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Product.fInventoryAlterationDetails myPage = new AlavaSoft.Reports.Product.fInventoryAlterationDetails();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void printVoucherToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Delivery.fDeliveryReceipt myPage = new AlavaSoft.Reports.Delivery.fDeliveryReceipt();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void printVoucherToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Returns.fProductReturnVoucher myPage = new AlavaSoft.Reports.Returns.fProductReturnVoucher();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void printVoucherToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Refund.fRefundVoucher myPage = new AlavaSoft.Reports.Refund.fRefundVoucher();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void statusReportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Delivery.fDetailedDeliveryStatusReport myPage = new AlavaSoft.Reports.Delivery.fDetailedDeliveryStatusReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void returnsDetailedStatusReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Reports.Returns.fDetailedReturnStatusReport myPage = new AlavaSoft.Reports.Returns.fDetailedReturnStatusReport();
            myPage.MdiParent = this;
            myPage.WindowState = FormWindowState.Maximized;
            myPage.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ProgressBar.Value < 100)
            {
                ProgressBar.Value += 2;
            }
            else
            {
                ProgressBar.Value = 0;
            }
        }

        public void startProgress()
        {
            ProgressBar.Visible = true;
            ProgressBar.Value = 0;
            timer1.Enabled = true;
        }

        public void endProgress()
        {
            timer1.Enabled = false;
            ProgressBar.Value = 0;
            ProgressBar.Visible = false;
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void helpFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.Help.frmHelp myPage = new AlavaSoft.Help.frmHelp();
            myPage.Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AlavaSoft.frmAbout myPage = new AlavaSoft.frmAbout();
            myPage.ShowDialog();
        }
      
        #region sample code

        /*
        private void ShowNewForm(object sender, EventArgs e)
        {
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        { 
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        

        private void createNewSalesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderAdd();
            childForm.MdiParent = this;
            childForm.Text = childForm.Text + " " + ++iSalesOrder;
            childForm.Show();
        }

        private void editExistingSalesOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.SalesOrder.fSalesOrderEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void receivePaymentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPayment();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void voidCancelReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Payment.fPaymentVoid();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void receiveNewReturnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Return.fReturnEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewRefundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void modifyRefundInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Refund.fRefundEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editDeliveryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Delivery.fDeliveryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fProductEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void createNewProductCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fCategoryAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingProductCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fCategoryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void maintainInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Product.fInventoryEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingSupplierInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Supplier.fSupplierEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingCustomerInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.Customer.fCustomerEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void addUserAccouToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserAdd();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void editExistingUserAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserEdit();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void userAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Transaction.User.fUserAccess();
            childForm.MdiParent = this;
            childForm.Show();
        }

        

        private void salesOrderToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSalesOrderSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void deliveryToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fDeliverySearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void productToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fProductSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReturnSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void refundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fRefundSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void supplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fSupplierSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void customerToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fCustomerSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }

        private void paymentReceiptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form childForm = new AlavaSoft.Search.fReceiptSearch();
            childForm.MdiParent = this;
            childForm.Show();
        }
       */
        #endregion

    }
}
