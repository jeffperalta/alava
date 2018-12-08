using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace AlavaSoft.Class
{
    class PropertyReader
    {
        private String _StrFile = " ";
        //private TextReader tr;
        private String _StrSeparator = "<NEXT>";

        //--Application variables--
        private String DatabaseConnectionString = "";
        private String ReportLocation = "";
        private String HelpFile = "";
        //--<Insert New Application Variables>--

        public PropertyReader() {
            this._StrFile = "C:\\test1.properties";
        }

        public PropertyReader(String File) {
            this._StrFile = File;
        }

        public void setFileLocation(String File) {
            this._StrFile = File;
        }

        public String getFileLocation() {
            return this._StrFile;
        }

        public String getHelpFile()
        {
            return this.HelpFile;
        }

        public void ReadFile() {
            StreamReader tr = new StreamReader(this._StrFile);
            Regex RE = new Regex(this._StrSeparator);
            string[] buffer = RE.Split(tr.ReadToEnd());
            
            //--Get Property--
            this.DatabaseConnectionString = buffer[0];
            this.ReportLocation = buffer[1].Replace("\r\n",String.Empty);
            this.ReportLocation = this.ReportLocation.Replace("\n\r", String.Empty);
            this.HelpFile = buffer[2].Replace("\r\n", String.Empty);
            this.HelpFile = this.HelpFile.Replace("\n\r", String.Empty);
            //--<Insert New Application Variable>--

            tr.Close();
        }

        public String getConnectionString() {
            return this.DatabaseConnectionString;
        }

        public String getReportConnection()
        {
            return this.ReportLocation;
        }
    }
}
