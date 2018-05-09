using DataTransferObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for ChangePassword.xaml
    /// </summary>
    public partial class ChangePassword : Window
    {

        private UserManager _userManager;

        private UserDetails _user;

        public ChangePassword(UserManager userManager, UserDetails user)
        {
            this._userManager = userManager;
            this._user = user;
            InitializeComponent();
        }

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            //storage
            var oldPassword = pwdOldPassword.Password;
            var newPassword = pwdNewPassword.Password;
            var retypePassword = pwdRetypePassword.Password;

            //error checks 
            if (oldPassword == "")
            {
                MessageBox.Show("You must supply your current password");
                //clear password boxes

                ClearPasswordBoxes();
                return;
            }

            if (newPassword.Length < 6)
            {
                MessageBox.Show("New password is invalid. Try again.");
                ClearPasswordBoxes();
                return;
            }

            if (newPassword != retypePassword)
            {
                MessageBox.Show("New password and retyped password must match");
                ClearPasswordBoxes();
                return;
            }

            // update the password
            try
            {
                _user = _userManager.UpdatePassword(_user, oldPassword, newPassword);
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException.Message;
                MessageBox.Show(message, "Update Failed!");
                this.DialogResult = false;
            }


            //close this window and return true - normal exit
            this.DialogResult = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.btnSubmit.IsDefault = true;
            this.pwdOldPassword.Password = "password";
            this.pwdOldPassword.IsEnabled = false;
            this.Title = _user.User.FirstName + ", please change your password";
            ClearPasswordBoxes();
        }

        private void ClearPasswordBoxes()
        {
            pwdNewPassword.Password = "";
            pwdRetypePassword.Password = "";

            if (_user.Roles[0].RoleID == "New User")
            {
                pwdNewPassword.Focus();
            }
            else // existing user change password
            {
                pwdOldPassword.Password = "";
                pwdOldPassword.Focus();

            }
        }
    }
}
