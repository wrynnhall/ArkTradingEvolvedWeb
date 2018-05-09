using DataTransferObjects;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for frmAddEditCollectionEntry.xaml
    /// </summary>
    public partial class frmAddEditCollectionEntry : Window
    {

        private List<Creature> _creatureList;
        private CollectionManager _collectionManager;
        private CollectionEntryForm _type;
        
        private CollectionEntryDetails _collectionEntryDetails;
        private Collection _collection;

        public frmAddEditCollectionEntry(CollectionManager collMgr, CollectionEntryDetails collEntryDets, CollectionEntryForm type)
        {
            _collectionManager = collMgr;
            _collectionEntryDetails = collEntryDets;
            _type = type;
            InitializeComponent();
        }

        public frmAddEditCollectionEntry(CollectionManager collMgr, Collection collection)
        {
            _collectionManager = collMgr;
           
            _collection = collection;
            _type = CollectionEntryForm.Add;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _creatureList = _collectionManager.RetrieveCreatures();
                populateCreaturesList();
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error loading Creature data");
                this.DialogResult = false;
                this.Close();
            }

            switch (_type)
            {
                case CollectionEntryForm.Add:
                    setupAddForm();
                    break;
                case CollectionEntryForm.Edit:
                    setupEditForm();
                    break;
                case CollectionEntryForm.View:
                    setupViewForm();
                    break;
                
            }
            
            this.txtCollectionEntryID.IsEnabled = false;
            this.txtCollectionID.IsEnabled = false;
        }

        private void setupViewForm()
        {
            populateControls();
            this.btnAddEdit.Content = "Edit";
            this.Title = "Collection Entry Details";
            setControls(false);
        }

        private void setupEditForm()
        {
            populateControls();
            this.btnAddEdit.Content = "Save";
            this.Title = "Edit Collection Entry";
            setControls();
        }

        private void setupAddForm()
        {
            this.btnAddEdit.Content = "Save";
            this.Title = "Add Collection Entry";
            setControls();
        }

        private void setControls(bool readOnly = true)
        {
            this.txtName.IsEnabled = readOnly;
            this.txtLevel.IsEnabled = readOnly;
            this.txtHealth.IsEnabled = readOnly;
            this.txtStamina.IsEnabled = readOnly;
            this.txtOxygen.IsEnabled = readOnly;
            this.txtFood.IsEnabled = readOnly;
            this.txtWeight.IsEnabled = readOnly;
            this.txtBaseDamage.IsEnabled = readOnly;
            this.txtSpeed.IsEnabled = readOnly;
            this.txtTorpor.IsEnabled = readOnly;
            this.txtImprint.IsEnabled = readOnly;
            this.cboCreatures.IsEnabled = readOnly;
        }

        private void populateControls()
        {
            this.txtCollectionEntryID.Text = _collectionEntryDetails.CollectionEntry.CollectionEntryID.ToString();
            this.txtCollectionID.Text = _collectionEntryDetails.CollectionEntry.CollectionID.ToString();
            // set creature control
            foreach(Creature c in cboCreatures.Items)
            {
                if(c.CreatureID == _collectionEntryDetails.Creature.CreatureID)
                {
                    this.cboCreatures.SelectedIndex = cboCreatures.Items.IndexOf(c);
                    break;
                }
            }
            
            this.txtName.Text = _collectionEntryDetails.CollectionEntry.Name;
            this.txtLevel.Text = _collectionEntryDetails.CollectionEntry.Level.ToString();
            this.txtHealth.Text = _collectionEntryDetails.CollectionEntry.Health.ToString();
            this.txtStamina.Text = _collectionEntryDetails.CollectionEntry.Stamina.ToString();
            this.txtOxygen.Text = _collectionEntryDetails.CollectionEntry.Oxygen.ToString();
            this.txtFood.Text = _collectionEntryDetails.CollectionEntry.Food.ToString();
            this.txtWeight.Text = _collectionEntryDetails.CollectionEntry.Weight.ToString();
            this.txtBaseDamage.Text = _collectionEntryDetails.CollectionEntry.BaseDamage.ToString();
            this.txtSpeed.Text = _collectionEntryDetails.CollectionEntry.MovementSpeed.ToString();
            this.txtTorpor.Text = _collectionEntryDetails.CollectionEntry.Torpor.ToString();
            this.txtImprint.Text = _collectionEntryDetails.CollectionEntry.Imprint.ToString();
        }

        private void populateCreaturesList()
        {
            this.cboCreatures.ItemsSource = _creatureList;
            this.cboCreatures.DisplayMemberPath = "CreatureID";
        }

        private void txtLevel_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = isTextAllowed(e.Text);
        }

        private bool isTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CollectionEntryForm.Add:
                    performAdd();
                    break;
                case CollectionEntryForm.Edit:
                    performEdit();
                    break;
                case CollectionEntryForm.View:
                    setupEditForm();
                    _type = CollectionEntryForm.Edit;
                    break;
            }
        }

        private void performEdit()
        {
            if (validateInputs())
            {
                
                var updateEntry = new CollectionEntry()
                {
                    CollectionID = _collectionEntryDetails.CollectionEntry.CollectionID,
                    CreatureID = ((Creature)cboCreatures.SelectedItem).CreatureID,
                    Name = txtName.Text,
                    Level = Int32.Parse(txtLevel.Text),
                    Health = Int32.Parse(txtHealth.Text),
                    Stamina = Int32.Parse(txtStamina.Text),
                    Oxygen = Int32.Parse(txtOxygen.Text),
                    Food = Int32.Parse(txtFood.Text),
                    Weight = Int32.Parse(txtWeight.Text),
                    BaseDamage = Int32.Parse(txtBaseDamage.Text),
                    MovementSpeed = Int32.Parse(txtSpeed.Text),
                    Torpor = Int32.Parse(txtTorpor.Text),
                    Imprint = Int32.Parse(txtImprint.Text)
                };

                try
                {
                    int result = _collectionManager.EditCollectionEntry(updateEntry, _collectionEntryDetails.CollectionEntry);
                    if(result != 1)
                    {
                        throw new ApplicationException("Collection edit did not save");
                    }
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
                    MessageBox.Show(message, "Edit Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void performAdd()
        {
            if (validateInputs())
            {
                

                try
                {
                    var newEntry = new CollectionEntry()
                    {
                        CollectionID = _collection.CollectionID,
                        CreatureID = ((Creature)cboCreatures.SelectedItem).CreatureID,
                        Name = txtName.Text,
                        Level = Int32.Parse(txtLevel.Text),
                        Health = Int32.Parse(txtHealth.Text),
                        Stamina = Int32.Parse(txtStamina.Text),
                        Oxygen = Int32.Parse(txtOxygen.Text),
                        Food = Int32.Parse(txtFood.Text),
                        Weight = Int32.Parse(txtWeight.Text),
                        BaseDamage = Int32.Parse(txtBaseDamage.Text),
                        MovementSpeed = Int32.Parse(txtSpeed.Text),
                        Torpor = Int32.Parse(txtTorpor.Text),
                        Imprint = Int32.Parse(txtImprint.Text),
                        Active = true
                    };
                    int result = _collectionManager.AddCollectionEntry(newEntry);
                    if(result != 1)
                    {
                        throw new ApplicationException("Collection Entry was not added!");
                    }
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
            bool result = true;

            // test if inputs are empty
            if(this.txtName.Text == "" || this.txtLevel.Text == "" ||
            this.txtHealth.Text == "" || this.txtStamina.Text == "" ||
            this.txtOxygen.Text == "" || this.txtFood.Text == "" ||
            this.txtWeight.Text == "" || this.txtBaseDamage.Text == "" ||
            this.txtSpeed.Text == "" || this.txtTorpor.Text == "" || 
            this.txtImprint.Text == "" || this.cboCreatures.SelectedItem == null)
            {
                MessageBox.Show("All fields are Required", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            //make sure integer inputs are integers
            int p = 0;
            if(!Int32.TryParse(txtLevel.Text, out p) ||
                !Int32.TryParse(txtHealth.Text, out p) ||
                !Int32.TryParse(txtStamina.Text, out p) ||
                !Int32.TryParse(txtOxygen.Text, out p) ||
                !Int32.TryParse(txtFood.Text, out p) ||
                !Int32.TryParse(txtWeight.Text, out p) ||
                !Int32.TryParse(txtBaseDamage.Text, out p) ||
                !Int32.TryParse(txtSpeed.Text, out p) ||
                !Int32.TryParse(txtTorpor.Text, out p) ||
                !Int32.TryParse(txtImprint.Text, out p))
            {
                MessageBox.Show("One or more creature attributes are invalid\nCreature attributes must be integers", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }


            //make sure the name field is not too long
            if(txtName.Text.Length > 50)
            {
                MessageBox.Show("Name field must be less than 50 characters", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }


            return result;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
