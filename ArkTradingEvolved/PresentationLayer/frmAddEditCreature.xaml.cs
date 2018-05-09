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
    /// Interaction logic for frmAddEditCreature.xaml
    /// </summary>
    public partial class frmAddEditCreature : Window
    {
        private Creature _creature;
        private CreatureManager _creatureManager;
        private CreatureTypeManager _creatureTypeManager;
        private CreatureDietManager _creatureDietManager;
        private CreatureForm _type;

        private List<CreatureDiet> _creatureDietList;
        private List<CreatureType> _creatureTypeList;

        public frmAddEditCreature(CreatureManager creatureMgr, CreatureTypeManager creatureTMgr, CreatureDietManager creatureDMgr, CreatureForm type, Creature creature)
        {
            this._creature = creature;
            this._creatureManager = creatureMgr;
            this._creatureTypeManager = creatureTMgr;
            this._creatureDietManager = creatureDMgr;
            this._type = type;
            
            InitializeComponent();
        }

        public frmAddEditCreature(CreatureManager creatureMgr, CreatureTypeManager creatureTMgr, CreatureDietManager creatureDMgr)
        {
            this._type = CreatureForm.Add;
            this._creatureManager = creatureMgr;
            this._creatureTypeManager = creatureTMgr;
            this._creatureDietManager = creatureDMgr;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _creatureDietList = _creatureDietManager.RetrieveCreatureDietListActive();
                _creatureTypeList = _creatureTypeManager.RetrieveCreatureTypeListActive();
                populateDietList();
                populateTypeList();
            }
            catch (Exception)
            {

                MessageBox.Show("There was an error loading creature data");
                this.DialogResult = false;
                this.Close();
            }

            switch (_type)
            {
                case CreatureForm.Add:
                    setupAddForm();
                    break;
                case CreatureForm.Edit:
                    setupEditForm();
                    break;
                case CreatureForm.View:
                    setupViewForm();
                    break;
                default:
                    break;
            }
        }

        private void setupViewForm()
        {
            populateControls();
            this.btnAddEdit.Content = "Edit";
            this.Title = "Viewing Creature";
            setControls(false);
        }

        private void setupEditForm()
        {
            populateControls();
            this.btnAddEdit.Content = "Save";
            this.Title = "Edit Creature";

            setControls();
        }

        private void setupAddForm()
        {
            this.btnAddEdit.Content = "Save";
            this.Title = "Add Creature";
          
            setControls();
        }

        private void populateTypeList()
        {
            this.cboTypes.ItemsSource = _creatureTypeList;
            this.cboTypes.DisplayMemberPath = "CreatureTypeID";
        }

        private void populateDietList()
        {
            this.cboDiets.ItemsSource = _creatureDietList;
            this.cboDiets.DisplayMemberPath = "CreatureDietID";
        }

        private void setControls(bool readOnly = true)
        {
            this.txtCreatureName.IsEnabled = readOnly;
            this.cboTypes.IsEnabled = readOnly;
            this.cboDiets.IsEnabled = readOnly;
        }

        private void populateControls()
        {
            this.txtCreatureName.Text = _creature.CreatureID;
            foreach(CreatureType t in _creatureTypeList)
            {
                if(t.CreatureTypeID == _creature.CreatureTypeID)
                {
                    this.cboTypes.SelectedIndex = cboTypes.Items.IndexOf(t);
                }
            }

            foreach(CreatureDiet d in _creatureDietList)
            {
                if(d.CreatureDietID == _creature.CreatureDietID)
                {
                    this.cboDiets.SelectedIndex = cboDiets.Items.IndexOf(d);
                }
            }
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CreatureForm.Add:
                    performAdd();
                    break;
                case CreatureForm.Edit:
                    performEdit();
                    break;
                case CreatureForm.View:
                    setupEditForm();
                    _type = CreatureForm.Edit;
                    break;
                default:
                    break;
            }
        }

        private void performEdit()
        {
            if (validateInputs())
            {
                var creature = new Creature()
                {
                    CreatureID = txtCreatureName.Text,
                    CreatureDietID = ((CreatureDiet)cboDiets.SelectedItem).CreatureDietID,
                    CreatureTypeID = ((CreatureType)cboTypes.SelectedItem).CreatureTypeID
                };

                try
                {
                    int result = _creatureManager.UpdateCreature(_creature, creature);
                    if (result != 1)
                    {
                        throw new ApplicationException("Creature was not updated properly!");
                    }
                    MessageBox.Show("Creature was updated!");
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
                var creature = new Creature()
                {
                    CreatureID = txtCreatureName.Text,
                    CreatureDietID = ((CreatureDiet)cboDiets.SelectedItem).CreatureDietID,
                    CreatureTypeID = ((CreatureType)cboTypes.SelectedItem).CreatureTypeID
                };

                try
                {
                    int result = _creatureManager.AddCreature(creature);
                    if(result != 1)
                    {
                        throw new ApplicationException("Creature was not added!");
                    }
                    MessageBox.Show("Creature was added!");
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
            if(this.txtCreatureName.Text == "" || this.cboTypes.SelectedItem == null || this.cboDiets.SelectedItem == null)
            {
                MessageBox.Show("All fields are Required", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if(this.txtCreatureName.Text.Length > 50)
            {
                MessageBox.Show("The Creature Name cannot be larger than 50 characters", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
