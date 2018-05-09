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
    /// Interaction logic for frmAddEditCreatureType.xaml
    /// </summary>
    public partial class frmAddEditCreatureType : Window
    {

        private CreatureTypeManager _creatureTypeManager;
        private CreatureType _creatureType;
        private CreatureTypeForm _type;

        public frmAddEditCreatureType(CreatureTypeManager creatureTMgr, CreatureTypeForm type, CreatureType creatureType) 
        {
            this._creatureTypeManager = creatureTMgr;
            this._type = type;
            this._creatureType = creatureType;
            InitializeComponent();
        }

        public frmAddEditCreatureType(CreatureTypeManager creatureTMgr)
        {
            this._creatureTypeManager = creatureTMgr;
            this._type = CreatureTypeForm.Add;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CreatureTypeForm.Add:
                    break;
                case CreatureTypeForm.Edit:
                    populateControls();
                    break;
                default:
                    break;
            }
        }

        private void populateControls()
        {
            this.txtCreatureType.Text = this._creatureType.CreatureTypeID;
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CreatureTypeForm.Add:
                    performAdd();
                    break;
                case CreatureTypeForm.Edit:
                    performEdit();
                    break;
                default:
                    break;
            }
        }

        private void performEdit()
        {
            if (validateInputs())
            {
                var type = new CreatureType()
                {
                    CreatureTypeID = txtCreatureType.Text
                };

                try
                {
                    int result = _creatureTypeManager.UpdateCreatureType(_creatureType, type);
                    if (result != 1)
                    {
                        throw new ApplicationException("Creature Type was not updated!");
                    }
                    MessageBox.Show("Creature Type was updated!");
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
                var type = new CreatureType()
                {
                    CreatureTypeID = txtCreatureType.Text
                };

                try
                {
                    int result = _creatureTypeManager.AddCreatureType(type);
                    if(result != 1)
                    {
                        throw new ApplicationException("Creature Type was not added!");
                    }
                    MessageBox.Show("Creature Type was added!");
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
            if(this.txtCreatureType.Text == "")
            {
                MessageBox.Show("The creature type cannot be empty", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if(this.txtCreatureType.Text.Length > 30)
            {
                MessageBox.Show("The creature type cannot be longer than 30 characters", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }


            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
