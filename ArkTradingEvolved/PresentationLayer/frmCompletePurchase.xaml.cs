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
namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for frmCompletePurchase.xaml
    /// </summary>
    public partial class frmCompletePurchase : Window
    {

        private CollectionManager _collectionManager;
        private User _user;
        private PurchaseDetails _purchaseDetails;
        private List<Collection> _collectionList;
        private MarketEntryManager _marketEntryManager;


        public frmCompletePurchase(CollectionManager colMgr, MarketEntryManager marketEMgr, User user, PurchaseDetails purchaseDetails)
        {
            this._collectionManager = colMgr;
            this._marketEntryManager = marketEMgr;
            this._user = user;
            this._purchaseDetails = purchaseDetails;
            InitializeComponent();
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if (validateInputs())
            {
                try
                {
                    var result = _marketEntryManager.PerformMarketEntryPurchaseComplete(((Collection)cboCollections.SelectedItem).CollectionID, _purchaseDetails);
                    if (result != 3)
                    {
                        throw new ApplicationException("Perform purchase complete failed!");
                    }
                    MessageBox.Show("Purchase was added to your collection!");
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
                    MessageBox.Show(message, "Purchase Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private bool validateInputs()
        {
            if(cboCollections.SelectedItem == null)
            {
                MessageBox.Show("You must select a collection", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _collectionList = _collectionManager.RetrieveCollectionList(_user.UserID);
            }
            catch (Exception ex)
            {

                MessageBox.Show("There was an error loading collection data");
                this.DialogResult = false;
                this.Close();
            }

            populateControls();
        }

        private void populateControls()
        {
            this.cboCollections.ItemsSource = _collectionList;
            this.cboCollections.DisplayMemberPath = "Name";
        }
    }
}
