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
using DataTransferObjects;
using LogicLayer;
namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for frmAddEditCreatureDiet.xaml
    /// </summary>
    public partial class frmAddEditCreatureDiet : Window
    {
        private CreatureDietManager _creatureDietManager;
        private CreatureDiet _creatureDiet;
        private CreatureDietForm _type;

        public frmAddEditCreatureDiet(CreatureDietManager creatureDMgr, CreatureDiet creatureDiet, CreatureDietForm type)
        {
            this._creatureDietManager = creatureDMgr;
            this._type = type;
            this._creatureDiet = creatureDiet;
            InitializeComponent();
        }

        public frmAddEditCreatureDiet(CreatureDietManager creatureDMgr)
        {
            this._creatureDietManager = creatureDMgr;
            this._type = CreatureDietForm.Add;
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CreatureDietForm.Add:
                    performAdd();
                    break;
                case CreatureDietForm.Edit:
                    performEdit();
                    break;
                default:
                    break;
            }

        }

        private void populateControls()
        {
            this.txtCreatureDiet.Text = this._creatureDiet.CreatureDietID;
        }


        private void performEdit()
        {
            if (validateInputs())
            {
                var diet = new CreatureDiet()
                {
                    CreatureDietID = txtCreatureDiet.Text
                };

                try
                {
                    int result = _creatureDietManager.UpdateCreatureDiet(_creatureDiet, diet);
                    if (result != 1)
                    {
                        throw new ApplicationException("Creature Diet was not updated!");
                    }
                    MessageBox.Show("Creature Diet was updated!");
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {

                    var message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "\n\n" + ex.InnerException.Message;
                    }
                    //have to display the error
                    MessageBox.Show(message, "Update Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void performAdd()
        {
            if (validateInputs())
            {
                var diet = new CreatureDiet()
                {
                    CreatureDietID = txtCreatureDiet.Text
                };

                try
                {
                    int result = _creatureDietManager.AddCreatureDiet(diet);
                    if (result != 1)
                    {
                        throw new ApplicationException("Creature Diet was not added!");
                    }
                    MessageBox.Show("Creature Diet was added!");
                    this.DialogResult = true;
                    this.Close();
                }
                catch (Exception ex)
                {

                    var message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "\n\n" + ex.InnerException.Message;
                    }
                    //have to display the error
                    MessageBox.Show(message, "Add Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        private bool validateInputs()
        {
            if (this.txtCreatureDiet.Text == "")
            {
                MessageBox.Show("The creature diet cannot be empty", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if (this.txtCreatureDiet.Text.Length > 30)
            {
                MessageBox.Show("The creature diet cannot be longer than 30 characters", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }


            return true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CreatureDietForm.Add:
                    break;
                case CreatureDietForm.Edit:
                    populateControls();
                    break;
                default:
                    break;
            }
        }
    }
}
