using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlavaSoft.Class
{
    public class UserCredential
    {
        private Boolean ValidUser = false;
        private List<String> UserAccess = new List<String>();
        private String FullName = " ";
        private int UserID = 0;

        public UserCredential() { 
        
        }

        public void setValidUser(Boolean ValidUser) {
            this.ValidUser = ValidUser;
        }

        public Boolean isValidUser() {
            return this.ValidUser;
        }

        public void addUserAccess(String UserModuleAccess) {
            UserAccess.Add(UserModuleAccess);
        }

        public Boolean hasAccess(String UserModuleAccess) {
            return this.UserAccess.Contains(UserModuleAccess);
        }

        public void setFullName(String FullName) {
            this.FullName = FullName;
        }

        public String getFullName() {
            return this.FullName;
        }

        public void setUserID(int UserID) {
            this.UserID = UserID;
        }

        public int getUserID() {
            return this.UserID;
        }
    }
}
