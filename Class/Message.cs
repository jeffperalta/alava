using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlavaSoft.Class
{
    class Message
    {
        private int MessageNumber = 0;
        private string[] MessageString = {"", //Leave this slot blank
                                          "%0 field is required.", //Message#1
                                          "%0 field must be unique.", //Message#2
                                          "%0 field is required and value must be %1.", //Message#3
                                          "Choose an item that you would like to %0", //Message#4
                                          "No records were affected by this action.", //Message#5
                                          "Cannot delete a referenced record.", //Message#6
                                          "The changes were successfully saved.", //Message#7
                                          "Invalid entry format (%0 field must be %1)", //Message#8
                                          "Cannot delete an active account", //Message#9
                                          "Do you really like to delete this record?", //Message#10
                                          "%0 is invalid; Entry must be%1",  //Message#11
                                          "Cannot delete %0", //Message#12 ex. --Cannot delete a <status> SO--
                                          "Inventory Stock is insufficient.", //Message#13
                                          "Payment must be equal to the SO amount.", //Message#14
                                          "%0 field must be a positive number.", //Message#15
                                          "This SO has no excess payment. Continue?", //Message#16
                                          "Refund is not possible for this Sales Order.", //Message#17
                                          "Refund amount must not exceed the Total Payment.", //Message#18
                                          "Cannot delete %0 with %1 status."//Message#19
                                         }; 

        private List<int> InformationMessages = new List<int>(); //Not Error Type Message
        private List<int> ExclamationMessages = new List<int>(); //Not Error Type Message
        private List<int> CriticalMessages = new List<int>(); //Not Error Type Message
        private List<int> QuestionMessages = new List<int>(); //Not Error Type Message
        private static int MaxParameter = 50;
        private List<String> Parameter = new List<String>();
        private Boolean boolMessage = false;
        private AlavaSoft.Message.fMessage myMessageBox = null;

        public Message() {
            Parameter.Clear();
            ExclamationMessages.Add(1);
            ExclamationMessages.Add(2);
            ExclamationMessages.Add(3);
            ExclamationMessages.Add(4);
            ExclamationMessages.Add(5);
            ExclamationMessages.Add(6);
            InformationMessages.Add(7);
            ExclamationMessages.Add(8);
            ExclamationMessages.Add(9);
            QuestionMessages.Add(10);
            ExclamationMessages.Add(11);
            ExclamationMessages.Add(12);
            ExclamationMessages.Add(13);
            ExclamationMessages.Add(14);
            ExclamationMessages.Add(15);
            QuestionMessages.Add(16);
            ExclamationMessages.Add(17);
            CriticalMessages.Add(18);
            ExclamationMessages.Add(19);
        }

        public Boolean isInformationMessage() 
        {
            return InformationMessages.Contains(this.MessageNumber);
        }

        public Boolean isExclamationMessage() 
        {
            return ExclamationMessages.Contains(this.MessageNumber);
        }

        public Boolean isCriticalMessage() 
        {
            return CriticalMessages.Contains(this.MessageNumber);
        }

        public Boolean isQuestionMessage()
        {
            return QuestionMessages.Contains(this.MessageNumber);
        }

        public void reset() {
            boolMessage = false;
            MessageNumber = 0;
            Parameter.Clear();
        }

        public void setMessageNumber(int value) {
            if (value < MaxParameter)
            {
                MessageNumber = value;
                boolMessage = true;
            }
            else 
            {
                MessageNumber = 0;
                boolMessage = false;
            }
        }

        public int getMessageNumber() 
        {
            return MessageNumber;
        }

        public void addParameter(String value) {
            Parameter.Add(value);
        }

        public Boolean hasMessage() {
            return boolMessage;
        }

        private String getMessage() {
            String strMessage = MessageString[MessageNumber];
            for (int iCtr = 0; iCtr < Parameter.Count; iCtr++) { 
                strMessage = strMessage.Replace("%" + iCtr, Parameter.ElementAt<String>(iCtr));    
            }
            this.reset(); //Does not reset the actual message;
            return strMessage;
        }

        public AlavaSoft.Message.fMessage.eButtonClicked showMessage() 
        {
            if (hasMessage())
            {
                //--Determine Message Type--
                String type = "";
                if (isExclamationMessage()) type = "Exclamation";
                else if (isQuestionMessage()) type = "Question";
                else if (isCriticalMessage()) type = "Critical";
                else type = "Information";

                myMessageBox = new AlavaSoft.Message.fMessage(type, getMessage());
                myMessageBox.ShowDialog();
                return myMessageBox.getClicked();
            }
            else 
            {
                return AlavaSoft.Message.fMessage.eButtonClicked.Cancel;
            }
        }
    }
}
