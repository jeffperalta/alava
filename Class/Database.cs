using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace AlavaSoft.Class
{
    class Database
    {
        private String ConnectionString = "";
        private SqlConnection myConnection = null;
        private SqlCommand mySqlCommand = null;
        private DataSet myDataSet = null;
        private SqlDataAdapter myDataAdapter = null;
        private DataView myDataView = null;
        private SqlDataReader myDataReader = null;

        public Database() { 
            
        }

        private void setConnectionString() {

            //Read from file
            PropertyReader PR = new PropertyReader("C:\\test1.properties");
            PR.ReadFile();
            this.ConnectionString = PR.getConnectionString();

            if (this.ConnectionString.Length == 0)
            {
                throw new Class.Exception.NoDatabaseLocationException();
            }
        }

        public String getConnectionString() {
            return this.ConnectionString;
        }

        public SqlConnection getConnection() {
            //Setup connection String
            this.setConnectionString();
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();
            return conn;
        }

        public int showList(System.Windows.Forms.DataGridView myControl, String SqlCommandText) {

            try
            {
                myConnection = this.getConnection();
                mySqlCommand = new SqlCommand(SqlCommandText, myConnection);
                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                myDataView = new DataView();
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable,0);
                myDataView.Table = myDataTable[0];

                myControl.DataSource = null;
                myControl.DataSource = myDataView;
                return myControl.RowCount;
            }
            catch (System.Exception e)
            {
                e.ToString();
                return 0;
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }
            }
        }

        public void showCombo(System.Windows.Forms.ComboBox myControl, String SqlCommandText, String Display)
        {
            try
            {
                myConnection = this.getConnection();
                mySqlCommand = new SqlCommand(SqlCommandText, myConnection);
                myDataAdapter = new SqlDataAdapter(mySqlCommand);
                myDataSet = new DataSet();

                myDataAdapter.Fill(myDataSet, "RESULT");
                DataTable[] myDataTable = new DataTable[10];
                myDataSet.Tables.CopyTo(myDataTable, 0);

                myControl.DataSource = null;
                myControl.DataSource = myDataTable[0];
                myControl.DisplayMember = Display;
            }
            catch (System.Exception e)
            {
                e.ToString();
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }
            }
        }

        /// <summary>
        /// Not Yet Tested.
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="WhereClause"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        private Boolean isExist(String TableName, String[] Values, String WhereClause) {
            Boolean myResult = false;
            myConnection = this.getConnection();

            try
            {
                mySqlCommand = myConnection.CreateCommand();
                mySqlCommand.CommandText = "SELECT 'EXIST' FROM " + TableName + " " +
                                           "WHERE " + WhereClause + " ";
                for (int iCtr = 0; iCtr < Values.Length; iCtr++) {
                    mySqlCommand.Parameters.AddWithValue("@" + iCtr, Values[iCtr]);
                }

                SqlDataReader myReader = mySqlCommand.ExecuteReader();
                myReader.Read();

                if (myReader.HasRows)
                {
                    myResult = true;
                }
                else 
                {
                    myResult = false;
                }
            }
            catch (System.Exception e)
            {
                e.ToString();
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }
            }

            return myResult;
        }

        /// <summary>
        /// Validate Where Clause Especially for User Provided Fields
        /// Single Row Record is Fetched
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Field"></param>
        /// <param name="WhereClause"></param>
        /// <returns></returns>
        public List<String> getElement(String TableName, List<String> Field, String WhereClause) {
            List<String> myResult = new List<String>();
            String SqlCommandText = "";

            try
            {
                myConnection = this.getConnection();
                
                //--Assemble Query String--
                SqlCommandText = " SELECT ";
                for (int iCtr = 0; iCtr < Field.Count; iCtr++) {
                    if (iCtr == 0)
                    {
                        SqlCommandText += Field.ElementAt<String>(iCtr);
                    }
                    else 
                    {
                        SqlCommandText += ", " + Field.ElementAt<String>(iCtr);
                    }    
                }
                SqlCommandText += " FROM " + TableName;

                if (WhereClause.Trim().Length != 0) { 
                    SqlCommandText += " WHERE " + WhereClause;
                }

                SqlCommandText += " ; ";
                mySqlCommand = new SqlCommand(SqlCommandText, myConnection);
                myDataReader = mySqlCommand.ExecuteReader();
                
                myResult.Clear();
                while(myDataReader.Read()) {
                    for(int iCtr = 0; iCtr < Field.Count; iCtr++) {
                        myResult.Add(myDataReader.GetString(iCtr));
                    }
                    break;
                }
                
                
            }
            catch (System.Exception e)
            {
                e.ToString();
            }
            finally
            {
                if (this.myConnection != null)
                {
                    this.myConnection.Close();
                }

                if(this.myDataReader != null) {
                    this.myDataReader.Close();
                }
            }

            return myResult;
        }
    
    }
}
