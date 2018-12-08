using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AlavaSoft.Message
{
    public partial class fMessage : Form
    {
        private String myType = "";
        public enum eButton { YesNo, Ok, YesNoCancel, OkCancel };
        public enum eButtonClicked { Yes, No, Ok, Cancel, None };
        private eButton myButton;
        private eButtonClicked myClick;

        public fMessage(String type, String Message, eButton myButton) 
        {
            InitializeComponent(); 
            setup(type, Message);
            this.myButton = myButton;
        }

        public fMessage(String type, String Message)
        {
            InitializeComponent();
            setup(type, Message);
            switch (myType.ToLower())
            {
                case "information":
                    myButton = eButton.Ok; 
                    break;
                case "critical":
                    myButton = eButton.OkCancel;
                    break;
                case "question":
                    myButton = eButton.YesNo;
                    break;
                case "exclamation":
                    myButton = eButton.Ok;
                    break;
            } 
        }

        public eButtonClicked getClicked() 
        {
            return myClick;
        }

        private void setup(String type, String Message) 
        {
            myClick = eButtonClicked.None;
            myType = type;
            label1.Text = Message;
        }

        private void fMessage_Load(object sender, EventArgs e)
        {
            switch (myType)
            {
                case "Information":
                    pictureBox1.Image = AlavaSoft.Properties.Resources.Information;
                    break;
                case "Critical":
                    pictureBox1.Image = AlavaSoft.Properties.Resources.Critical;
                    System.Media.SystemSounds.Asterisk.Play();
                    break;
                case "Question":
                    pictureBox1.Image = AlavaSoft.Properties.Resources.Question;
                    System.Media.SystemSounds.Question.Play();
                    break;
                case "Exclamation":
                    pictureBox1.Image = AlavaSoft.Properties.Resources.Exclamation;
                    System.Media.SystemSounds.Exclamation.Play();
                    break;
            }

            switch (myButton) 
            {
                case eButton.Ok:
                    button1.Text = "Ok";
                    button1.Visible = true;
                    this.AcceptButton = button1;
                    break;
                case eButton.OkCancel:
                    button2.Text = "Ok";
                    button1.Text = "Cancel";
                    button2.Visible = true;
                    button1.Visible = true;
                    this.AcceptButton = button2;
                    this.CancelButton = button1;
                    break;
                case eButton.YesNo:
                    button2.Text = "Yes";
                    button1.Text = "No";
                    button2.Visible = true;
                    button1.Visible = true;
                    this.AcceptButton = button2;
                    this.CancelButton = button1;
                    break;
                case eButton.YesNoCancel:
                    button1.Text = "Cancel";
                    button3.Text = "Yes";
                    button2.Text = "No";
                    button3.Visible = true;
                    button2.Visible = true;
                    button1.Visible = true;
                    this.AcceptButton = button3;
                    this.CancelButton = button1;
                    break;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Trim().ToLower() == "yes") myClick = eButtonClicked.Yes;
            else if (button1.Text.Trim().ToLower() == "no") myClick = eButtonClicked.No;
            else if (button1.Text.Trim().ToLower() == "ok") myClick = eButtonClicked.Ok;
            else if (button1.Text.Trim().ToLower() == "cancel") myClick = eButtonClicked.Cancel;
 
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text.Trim().ToLower() == "yes") myClick = eButtonClicked.Yes;
            else if (button2.Text.Trim().ToLower() == "no") myClick = eButtonClicked.No;
            else if (button2.Text.Trim().ToLower() == "ok") myClick = eButtonClicked.Ok;
            else if (button2.Text.Trim().ToLower() == "cancel") myClick = eButtonClicked.Cancel;
 
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text.Trim().ToLower() == "yes") myClick = eButtonClicked.Yes;
            else if (button3.Text.Trim().ToLower() == "no") myClick = eButtonClicked.No;
            else if (button3.Text.Trim().ToLower() == "ok") myClick = eButtonClicked.Ok;
            else if (button3.Text.Trim().ToLower() == "cancel") myClick = eButtonClicked.Cancel;
            this.Close();
        }

    }
}
