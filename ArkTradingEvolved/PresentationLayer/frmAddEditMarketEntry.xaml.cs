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
using LogicLayer;
using DataTransferObjects;
using System.Text.RegularExpressions;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for frmAddEditMarketEntry.xaml
    /// </summary>
    public partial class frmAddEditMarketEntry : Window
    {
        private List<Resource> _resourceList;
        private ResourceManager _resourceManager;
        private MarketEntryManager _marketEntryManager;
        private MarketEntryForm _type;

        private MarketEntryDetails _marketEntryDetail;
        private CollectionEntryDetails _collectionEntryDetails;


        public frmAddEditMarketEntry(ResourceManager resMgr, MarketEntryManager mEMgr, MarketEntryDetails mEDets, MarketEntryForm type)
        {
            this._resourceManager = resMgr;
            this._marketEntryManager = mEMgr;
            this._marketEntryDetail = mEDets;
            this._type = type;
            InitializeComponent();
        }

        public frmAddEditMarketEntry(ResourceManager resMgr, MarketEntryManager mEMgr, CollectionEntryDetails cEDets)
        {
            this._resourceManager = resMgr;
            this._marketEntryManager = mEMgr;
            this._collectionEntryDetails = cEDets;
            this._type = MarketEntryForm.Add;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _resourceList = _resourceManager.RetrieveResources();
                populateResourceList();
            }
            catch (Exception)
            {
                MessageBox.Show("There was an error loading resource data");
                this.DialogResult = false;
                this.Close();
            }


            switch (_type)
            {
                case MarketEntryForm.Add:
                    setupAddForm();
                    break;
                case MarketEntryForm.Edit:
                    setupEditForm();
                    break;
                case MarketEntryForm.View:
                    setupViewForm();
                    break;
               
            }
        }

        private void setupViewForm()
        {
            populateControls();
            this.btnAddEdit.Content = "Edit";
            this.Title = "Market Entry Details";
            this.lblHeader.Content = "Viewing: " + _marketEntryDetail.CollectionEntry.Name + ", " + _marketEntryDetail.CollectionEntry.CreatureID;
            setControls(false);
        }

        private void setControls(bool readOnly = true)
        {
            this.cboResources.IsEnabled = readOnly;
            this.txtAmount.IsEnabled = readOnly;
        }

        private void populateControls()
        {
            this.txtAmount.Text = _marketEntryDetail.MarketEntry.ResourceAmount.ToString();
            foreach (Resource r in _resourceList)
            {
                if(r.ResourceID == _marketEntryDetail.MarketEntry.ResourceID)
                {
                    this.cboResources.SelectedIndex = cboResources.Items.IndexOf(r);
                }
            }
        }

        private void setupEditForm()
        {
            populateControls();
            this.btnAddEdit.Content = "Save";
            this.Title = "Edit Market Entry";
            this.lblHeader.Content = "Editing: " + _marketEntryDetail.CollectionEntry.Name + ", " + _marketEntryDetail.CollectionEntry.CreatureID;
            setControls();
        }

        private void setupAddForm()
        {
            this.btnAddEdit.Content = "Save";
            this.Title = "Add Market Entry";
            this.lblHeader.Content = "Selling: " + _collectionEntryDetails.CollectionEntry.Name + ", " + _collectionEntryDetails.CollectionEntry.CreatureID;
            setControls();
        }

        private void populateResourceList()
        {
            this.cboResources.ItemsSource = _resourceList;
            this.cboResources.DisplayMemberPath = "ResourceID";
        }

        private void txtAmount_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
                case MarketEntryForm.Add:
                    performAdd();
                    break;
                case MarketEntryForm.Edit:
                    performEdit();
                    break;
                case MarketEntryForm.View:
                    setupEditForm();
                    _type = MarketEntryForm.Edit;
                    break;
                
            }

        }

        private void performEdit()
        {
            if (validateInputs())
            {
                var updateEntry = new MarketEntry()
                {
                    ResourceID = ((Resource)cboResources.SelectedItem).ResourceID,
                    ResourceAmount = Int32.Parse(txtAmount.Text)
                };

                try
                {
                    int result = _marketEntryManager.UpdateMarketEntry(updateEntry, _marketEntryDetail.MarketEntry);
                    if (result != 1)
                    {
                        throw new ApplicationException("Market Entry could not be updated!");
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

        private void performAdd()
        {
            if (validateInputs())
            {
                var newMarketEntry = new MarketEntry()
                {
                    CollectionEntryID = _collectionEntryDetails.CollectionEntry.CollectionEntryID,
                    ResourceID = ((Resource)cboResources.SelectedItem).ResourceID,
                    ResourceAmount = Int32.Parse(txtAmount.Text)
                };

                try
                {
                    int result = _marketEntryManager.AddMarketEntry(newMarketEntry);
                    if (result != 1)
                    {
                        throw new ApplicationException("Collection Entry was not added to the market!");
                    }
                    MessageBox.Show("Collection Entry was added to the market!");
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

            if (this.txtAmount.Text == "" || this.cboResources.SelectedItem == null)
            {
                MessageBox.Show("All fields are Required", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            //make sure integer inputs are integers
            int p = 0;
            if (!Int32.TryParse(txtAmount.Text, out p))
            {
                MessageBox.Show("The amount must be an integer", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
