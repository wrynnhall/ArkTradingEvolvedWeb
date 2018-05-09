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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationLayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserDetails _user = null;
        private UserManager _userManager = new UserManager();

        // Collections
        private CollectionManager _collectionManager = new CollectionManager();
        private List<Collection> _collectionList;
        private List<CollectionEntryDetails> _collectionEntryList;

        private MarketEntryManager _marketEntryManager = new MarketEntryManager();
        private List<MarketEntryDetails> _marketEntryList;
        private List<MarketEntryDetails> _myMarketEntryList;
        private List<PurchaseDetails> _purchaseList;

        private ResourceManager _resourceManager = new ResourceManager();

        private CreatureManager _creatureManager = new CreatureManager();
        private List<Creature> _creatureList;

        private CreatureTypeManager _creatureTypeManager = new CreatureTypeManager();
        private List<CreatureType> _creatureTypeList;

        private CreatureDietManager _creatureDietManager = new CreatureDietManager();
        private List<CreatureDiet> _creatureDietList;

        //constants for initial input validation
        private const int MIN_PASSWORD_LENGTH = 5; // business rule
        private const int MIN_USERNAME_LENGTH = 8; // forced because we use email addresses
        private const int MAX_USERNAME_LENGTH = 100; // the size of the database field
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hides all the tabs
        /// </summary>
        private void hideAllTabs()
        {
            foreach (var item in tabsetMain.Items)
            {
                ((TabItem)item).Visibility = Visibility.Collapsed;
            }
            tabsetMain.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// When the window is loaded, perform necessary functions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
            this.btnLogin.IsDefault = true;
            hideAllTabs();
            this.txtUsername.Text = "dinotravman123@gmail.com";
            this.txtPassword.Password = "password";


        }

        /// <summary>
        /// Button click event for login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Password;

            if (_user != null)
            {
                logOut();
                return;
            }



            // check for missing or invalid data
            if (username.Length < MIN_USERNAME_LENGTH ||
                username.Length > MAX_USERNAME_LENGTH)
            {
                MessageBox.Show("Invalid username", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                clearLogin();
                return;
            }

            if (password.Length < MIN_PASSWORD_LENGTH)
            {
                MessageBox.Show("Invalid password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                clearLogin();
                return;
            }

            try
            {
                _user = _userManager.AuthenticateUser(username, password);
                showTabs();

                //refreshCollectionList(_user.User.UserID);


                //The user is logged in. Time to clear the UI
                clearLogin();
                txtUsername.Visibility = Visibility.Hidden;
                txtPassword.Visibility = Visibility.Hidden;
                lblPassword.Visibility = Visibility.Hidden;
                lblUsername.Visibility = Visibility.Hidden;
                this.btnLogin.Content = "Log Out";
                this.btnLogin.IsDefault = false;

                //check for expired password
                if (_user.PasswordMustBeChanged)
                {
                    //call a password change function
                    changePassword();
                }

            }
            catch (Exception ex) // you may never throw exceptions at the presentation layer
            {
                var message = ex.Message;
                if (ex.InnerException != null)
                {
                    message += "\n\n" + ex.InnerException.Message;
                }
                //have to display the error
                MessageBox.Show(message, "Login Failed", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                clearLogin();
            }

        }

        /// <summary>
        /// Change password functionality
        /// </summary>
        private void changePassword()
        {
            var passwordChangeWindow = new ChangePassword(_userManager, _user);
            var passwordChangeResult = passwordChangeWindow.ShowDialog();

            if (passwordChangeResult == false)
            {
                // log out the user!
                logOut();
                MessageBox.Show("Window cancelled");
            }
            else
            {
                if (_user.Roles[0].RoleID == "New User")
                {
                    logOut();
                    MessageBox.Show("You have been logged out.", "Update cancelled");
                }
                else
                {
                    MessageBox.Show("Your password was changed");

                }

            }
        }

        /// <summary>
        /// Show necessary tabs
        /// </summary>
        private void showTabs()
        {
            tabsetMain.Visibility = Visibility.Visible;
            foreach (var r in _user.Roles)
            {
                switch (r.RoleID)
                {
                    case "Admin":
                        tabCreatures.Visibility = Visibility.Visible;
                        tabCreatures.IsSelected = true;
                        break;
                    case "General":
                        tabCollection.Visibility = Visibility.Visible;
                        tabCollection.IsSelected = true;
                        refreshCollectionList(_user.User.UserID);
                        tabMarket.Visibility = Visibility.Visible;
                        tabMarketEntries.Visibility = Visibility.Visible;
                        break;
                }
            }

        }

        /// <summary>
        /// Log out functionality
        /// </summary>
        private void logOut()
        {
            _user = null; // clear the current user
            _collectionList = null; //clear the collections list
            dgCollection.ItemsSource = _collectionList;
            _collectionEntryList = null;
            dgCollectionEntries.ItemsSource = _collectionEntryList;
            _purchaseList = null;
            dgPurchases.ItemsSource = _purchaseList;
            _marketEntryList = null;
            dgMarketEntries.ItemsSource = _marketEntryList;
            _myMarketEntryList = null;
            dgMyMarketEntries.ItemsSource = _myMarketEntryList;

            // enable login controls
            txtUsername.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Visible;
            lblPassword.Visibility = Visibility.Visible;
            lblUsername.Visibility = Visibility.Visible;
            this.btnLogin.Content = "Login";
            this.btnLogin.IsDefault = true;
            this.statusMain.Items[0] = "Sign up, or Sign in to enter";
            hideAllTabs();
        }

        /// <summary>
        /// clear login inputs
        /// </summary>
        private void clearLogin()
        {
            txtUsername.Text = "";
            txtPassword.Password = "";
            txtUsername.Focus();
        }

        /// <summary>
        /// refresh the collection list and datagrid
        /// </summary>
        /// <param name="userId"></param>
        private void refreshCollectionList(int userId)
        {
            try
            {
                _collectionList = _collectionManager.RetrieveCollectionList(userId);
                dgCollection.ItemsSource = _collectionList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Collection Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// refreshes the collection entry list and datagrid
        /// </summary>
        /// <param name="collectionId"></param>
        private void refreshCollectionEntryList(int collectionId)
        {
            try
            {
                _collectionEntryList = _collectionManager.RetrieveCollectionEntryDetails(collectionId);
                dgCollectionEntries.ItemsSource = _collectionEntryList;
            }
            catch (Exception ex)
            {

                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Collection Entry Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// When the user double clicks on a collection, the collections collection entries should appear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgCollection_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _collectionEntryList = null;
            var item = (Collection)this.dgCollection.SelectedItem;
            if (item == null)
            {
                return;
            }
            refreshCollectionEntryList(item.CollectionID);
            dgCollectionEntries.ItemsSource = _collectionEntryList;

        }

        /// <summary>
        /// Open edit form and perform edit functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCollection_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCollection.SelectedItems.Count > 0)
            {
                var editForm = new frmAddEditCollection(_collectionManager, (Collection)this.dgCollection.SelectedItem, CollectionForm.Edit);
                var result = editForm.ShowDialog();
                if (result == true)
                {
                    refreshCollectionList(_user.User.UserID);
                }
            }
            else
            {
                MessageBox.Show("You must select something!");
            }

        }

        /// <summary>
        /// Open add form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCollection_Click(object sender, RoutedEventArgs e)
        {
            var addFrom = new frmAddEditCollection(_collectionManager, _user.User);
            var result = addFrom.ShowDialog();
            if (result == true)
            {
                refreshCollectionList(_user.User.UserID);
            }
        }

        /// <summary>
        /// Deactivate the selected collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeactivateCollection_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCollection.SelectedItems.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to Cancel?\nCanceling will discard any unsaved changes!", "Cancel Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _collectionManager.DeactivateCollection(((Collection)this.dgCollection.SelectedItem).CollectionID);
                    refreshCollectionList(_user.User.UserID);
                }
            }
            else
            {
                MessageBox.Show("You must select a collection!");
            }

        }

        /// <summary>
        /// See collection details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCollectionEntries.SelectedItems.Count > 0)
            {
                var detailForm = new frmAddEditCollectionEntry(_collectionManager, (CollectionEntryDetails)this.dgCollectionEntries.SelectedItem, CollectionEntryForm.View);
                var result = detailForm.ShowDialog();
                if (result == true)
                {
                    refreshCollectionEntryList(((Collection)this.dgCollection.SelectedItem).CollectionID);
                }
            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }
        }

        /// <summary>
        /// Edit selected collection entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCollectionEntries.SelectedItems.Count > 0)
            {
                var detailForm = new frmAddEditCollectionEntry(_collectionManager, (CollectionEntryDetails)this.dgCollectionEntries.SelectedItem, CollectionEntryForm.Edit);
                var result = detailForm.ShowDialog();
                if (result == true)
                {
                    refreshCollectionEntryList(((Collection)this.dgCollection.SelectedItem).CollectionID);
                }
            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }
        }

        /// <summary>
        /// Add a collection entry to the selected collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCollection.SelectedItems.Count > 0)
            {
                var detailForm = new frmAddEditCollectionEntry(_collectionManager, (Collection)this.dgCollection.SelectedItem);
                var result = detailForm.ShowDialog();
                if (result == true)
                {
                    refreshCollectionEntryList(((Collection)this.dgCollection.SelectedItem).CollectionID);
                }
            }
            else
            {
                MessageBox.Show("You must select a collection!");
            }

        }

        /// <summary>
        /// Deactivate selected collection entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.dgCollectionEntries.SelectedItems.Count > 0)
                {
                    _collectionManager.DeactivateCollectionEntry(((CollectionEntryDetails)this.dgCollectionEntries.SelectedItem).CollectionEntry.CollectionEntryID);
                    refreshCollectionEntryList(((Collection)this.dgCollection.SelectedItem).CollectionID);
                }
                else
                {
                    MessageBox.Show("You must select an Entry!");
                }
            }
            catch (Exception ex)
            {

                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Deactivation Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        /// <summary>
        /// When the market tab has focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabMarket_GotFocus(object sender, RoutedEventArgs e)
        {

            dgMarketEntries.ItemsSource = _marketEntryList;
        }

        /// <summary>
        /// refreshes the market list and datagrid
        /// </summary>
        private void refreshMarketList()
        {
            try
            {
                _marketEntryList = _marketEntryManager.RetrieveMarketEntryDetailsByStatus("Available");
                this.dgMarketEntries.ItemsSource = _marketEntryList;

            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Market Entry Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Purchase the selected market entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {

            if (this.dgMarketEntries.SelectedItems.Count > 0)
            {
                // check if the market entry user is the same user as current user
                if (((MarketEntryDetails)this.dgMarketEntries.SelectedItem).User.UserID == _user.User.UserID)
                {
                    MessageBox.Show("You cannout purchase your own entry!");
                    return;
                }
                else
                {

                    var purchaseForm = new frmPurchase(_user.User, ((MarketEntryDetails)this.dgMarketEntries.SelectedItem));
                    var result = purchaseForm.ShowDialog();
                    if (result == true)
                    {
                        refreshMarketList();
                    }
                }
            }
            else
            {
                MessageBox.Show("You must select something!");
            }

        }

        /// <summary>
        /// When market entry tab has focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabMarketEntries_GotFocus(object sender, RoutedEventArgs e)
        {
            dgMyMarketEntries.ItemsSource = _myMarketEntryList;
        }

        /// <summary>
        /// refresh the my market list and datagrid
        /// </summary>
        private void refreshMyMarketList()
        {
            try
            {
                _myMarketEntryList = _marketEntryManager.RetrieveMarketEntryDetailsByUser(_user.User.UserID);
                dgMyMarketEntries.ItemsSource = _myMarketEntryList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Market Entry Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        /// <summary>
        /// Refresh lists based on tab click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabsetMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl) //if this event fired from TabControl then enter
            {
                string tabItem = ((sender as TabControl).SelectedItem as TabItem).Header as string;

                switch (tabItem)
                {
                    case "Collection":
                        if (_user != null)
                        {
                            refreshCollectionList(_user.User.UserID);
                        }
                        break;

                    case "Market":
                        refreshMarketList();
                        break;

                    case "My Market Entries":
                        refreshMyMarketList();
                        refreshPurchaseList();
                        break;



                    case "Creatures":
                        refreshCreatureList();
                        refreshCreatureTypeList();
                        refreshCreatureDietList();
                        break;


                }
            }

        }

        /// <summary>
        /// refreshes the creature diet list and datagrid
        /// </summary>
        private void refreshCreatureDietList()
        {
            try
            {
                _creatureDietList = _creatureDietManager.RetrieveCreatureDietList();
                dgCreatureDiets.ItemsSource = _creatureDietList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Purchase Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Refreshes the creature type list and datagrid
        /// </summary>
        private void refreshCreatureTypeList()
        {
            try
            {
                _creatureTypeList = _creatureTypeManager.RetrieveCreatureTypeList();
                dgCreatureTypes.ItemsSource = _creatureTypeList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Creature Type Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Refreshes the creature list and datagrid
        /// </summary>
        private void refreshCreatureList()
        {
            try
            {
                _creatureList = _creatureManager.RetrieveCreatureList();
                dgCreatures.ItemsSource = _creatureList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Creature Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Refresh the purchase list and datagrid
        /// </summary>
        private void refreshPurchaseList()
        {
            try
            {
                _purchaseList = _marketEntryManager.RetrievePurchaseDetailsByUser(_user.User.UserID);
                dgPurchases.ItemsSource = _purchaseList;
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Purchase Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Sell the selected collection entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //check if there is a selected collection
                if (this.dgCollectionEntries.SelectedItems.Count > 0)
                {
                    //check if the collection entry is already on the market
                    int marketRecords = _marketEntryManager.VerifyMarketEntryCollectionEntryPresence(((CollectionEntryDetails)this.dgCollectionEntries.SelectedItem).CollectionEntry.CollectionEntryID);
                    if (marketRecords > 0)
                    {
                        MessageBox.Show(((CollectionEntryDetails)this.dgCollectionEntries.SelectedItem).CollectionEntry.Name + " is already on the market!");
                        return;
                    }

                    //open the market entry form in add mode
                    var marketEntryForm = new frmAddEditMarketEntry(_resourceManager, _marketEntryManager, (CollectionEntryDetails)this.dgCollectionEntries.SelectedItem);
                    var result = marketEntryForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("You must select an Entry!");
                }
            }
            catch (Exception ex)
            {

                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Verify Collection Entry Purchase Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


        }

        /// <summary>
        /// View market entry details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewMarketEntry_Click(object sender, RoutedEventArgs e)
        {
            //check if there is a selected collection
            if (this.dgMyMarketEntries.SelectedItems.Count > 0)
            {

                //open the market entry form in view mode
                var marketEntryForm = new frmAddEditMarketEntry(_resourceManager, _marketEntryManager, (MarketEntryDetails)this.dgMyMarketEntries.SelectedItem, MarketEntryForm.View);
                var result = marketEntryForm.ShowDialog();
                if (result == true)
                {
                    refreshMyMarketList();
                }
            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }
        }

        /// <summary>
        /// Edit selected market entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditMarketEntry_Click(object sender, RoutedEventArgs e)
        {
            //check if there is a selected collection
            if (this.dgMyMarketEntries.SelectedItems.Count > 0)
            {

                //open the market entry form in view mode
                var marketEntryForm = new frmAddEditMarketEntry(_resourceManager, _marketEntryManager, (MarketEntryDetails)this.dgMyMarketEntries.SelectedItem, MarketEntryForm.Edit);
                var result = marketEntryForm.ShowDialog();
                if (result == true)
                {
                    refreshMyMarketList();
                }
            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }
        }

        /// <summary>
        /// Delete the selected market entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveMarketEntry_Click(object sender, RoutedEventArgs e)
        {

            if (this.dgMyMarketEntries.SelectedItems.Count > 0)
            {

                MessageBoxResult result = MessageBox.Show("Are you sure you want to remove this entry?", "Deactivation Warning", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _marketEntryManager.UpdateMarketEntryStatus(((MarketEntryDetails)this.dgMyMarketEntries.SelectedItem).MarketEntry, "Closed");
                        refreshMyMarketList();
                    }
                    catch (Exception ex)
                    {

                        var message = ex.Message + "\n\n" + ex.InnerException;
                        MessageBox.Show(message, "Market Entry Removal Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                }
            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }

        }

        /// <summary>
        /// Message the purchaser of the selected my market entry record.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMessagePurchaser_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgMyMarketEntries.SelectedItems.Count > 0)
            {
                var entry = (MarketEntryDetails)this.dgMyMarketEntries.SelectedItem;


                if (entry.MarketEntry.MarketEntryStatusID == "Sold")
                {
                    try
                    {
                        User purchaser = _userManager.RetreiveUserByMarketEntryPurchaseMarketEntryID(entry.MarketEntry.MarketEntryID);
                        var messageForm = new frmMessaging(entry, _user.User, purchaser);
                        messageForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {

                        var message = ex.Message + "\n\n" + ex.InnerException;
                        MessageBox.Show(message, "Purchaser Retreival Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }

                }
                else
                {
                    MessageBox.Show("Market entry must be Sold to message the buyer!");
                }


            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }
        }

        /// <summary>
        /// Message the seller of the selected purchase record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMessageSeller_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgPurchases.SelectedItems.Count > 0)
            {
                var entry = (PurchaseDetails)this.dgPurchases.SelectedItem;


                if (entry.MarketEntryDetails.MarketEntry.MarketEntryStatusID == "Sold")
                {

                    var messageForm = new frmMessaging(entry.MarketEntryDetails, _user.User, entry.User);
                    messageForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Market entry must be Sold to message the buyer!");
                }


            }
            else
            {
                MessageBox.Show("You must select an Entry!");
            }
        }

        /// <summary>
        /// Add a creature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCreature_Click(object sender, RoutedEventArgs e)
        {
            var creatureForm = new frmAddEditCreature(_creatureManager, _creatureTypeManager, _creatureDietManager);
            var result = creatureForm.ShowDialog();
            if (result == true)
            {
                refreshCreatureList();
            }
        }

        /// <summary>
        /// Edit a creature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCreature_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatures.SelectedItems.Count > 0)
            {
                var creatureForm = new frmAddEditCreature(_creatureManager, _creatureTypeManager, _creatureDietManager, CreatureForm.Edit, (Creature)this.dgCreatures.SelectedItem);
                var result = creatureForm.ShowDialog();
                if (result == true)
                {
                    refreshCreatureList();
                }
            }
            else
            {
                MessageBox.Show("You must select a creature!");
            }
        }

        /// <summary>
        /// View the selected creature
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewCreature_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatures.SelectedItems.Count > 0)
            {
                var creatureForm = new frmAddEditCreature(_creatureManager, _creatureTypeManager, _creatureDietManager, CreatureForm.View, (Creature)this.dgCreatures.SelectedItem);
                var result = creatureForm.ShowDialog();
                if (result == true)
                {
                    refreshCreatureList();
                }
            }
            else
            {
                MessageBox.Show("You must select a creature!");
            }
        }

        /// <summary>
        /// Toggle the selected creatures active field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToggleCreatureActive_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatures.SelectedItems.Count > 0)
            {

                try
                {
                    _creatureManager.UpdateCreatureActive(((Creature)this.dgCreatures.SelectedItem).CreatureID, !((Creature)this.dgCreatures.SelectedItem).Active);
                    refreshCreatureList();
                }
                catch (Exception ex)
                {

                    var message = ex.Message + "\n\n" + ex.InnerException;
                    MessageBox.Show(message, "Toggle Active Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("You must select a creature!");
            }
        }

        /// <summary>
        /// Add a creature type record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCreatureType_Click(object sender, RoutedEventArgs e)
        {
            var typeForm = new frmAddEditCreatureType(_creatureTypeManager);
            var result = typeForm.ShowDialog();
            if (result == true)
            {
                refreshCreatureTypeList();
            }
        }

        /// <summary>
        /// Edit the selected creature type record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCreatureType_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatureTypes.SelectedItems.Count > 0)
            {
                var typeForm = new frmAddEditCreatureType(_creatureTypeManager, CreatureTypeForm.Edit, (CreatureType)this.dgCreatureTypes.SelectedItem);
                var result = typeForm.ShowDialog();
                if (result == true)
                {
                    refreshCreatureTypeList();
                    refreshCreatureList();
                }
            }
            else
            {
                MessageBox.Show("You must select a creature type!");
            }

        }

        /// <summary>
        /// Toggle the active field of the selected creature type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToggleCreatureTypeActive_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatureTypes.SelectedItems.Count > 0)
            {

                try
                {
                    _creatureTypeManager.UpdateCreatureTypeActive(((CreatureType)this.dgCreatureTypes.SelectedItem).CreatureTypeID, !((CreatureType)this.dgCreatureTypes.SelectedItem).Active);
                    refreshCreatureTypeList();
                }
                catch (Exception ex)
                {

                    var message = ex.Message + "\n\n" + ex.InnerException;
                    MessageBox.Show(message, "Toggle Active Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("You must select a creature type!");
            }
        }

        /// <summary>
        /// Add a creature diet record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCreatureDiet_Click(object sender, RoutedEventArgs e)
        {
            var dietForm = new frmAddEditCreatureDiet(_creatureDietManager);
            var result = dietForm.ShowDialog();
            if (result == true)
            {
                refreshCreatureDietList();
                refreshCreatureList();
            }
        }

        /// <summary>
        /// Edit the selected creature diet record
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCreatureDiet_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatureDiets.SelectedItems.Count > 0)
            {
                var dietForm = new frmAddEditCreatureDiet(_creatureDietManager, (CreatureDiet)this.dgCreatureDiets.SelectedItem, CreatureDietForm.Edit);
                var result = dietForm.ShowDialog();
                if (result == true)
                {
                    refreshCreatureDietList();
                    refreshCreatureList();
                }
            }
            else
            {
                MessageBox.Show("You must select a creature diet!");
            }

        }

        /// <summary>
        /// Toggle the active field of the creature diet field
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToggleCreatureDietActive_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgCreatureDiets.SelectedItems.Count > 0)
            {
                try
                {
                    _creatureDietManager.UpdateCreatureDietActive(((CreatureDiet)this.dgCreatureDiets.SelectedItem).CreatureDietID, !((CreatureDiet)this.dgCreatureDiets.SelectedItem).Active);
                    refreshCreatureDietList();
                }
                catch (Exception ex)
                {

                    var message = ex.Message + "\n\n" + ex.InnerException;
                    MessageBox.Show(message, "Toggle Active Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }

            }
            else
            {
                MessageBox.Show("You must select a creature diet!");
            }
        }

        /// <summary>
        /// Mark a purchase completed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (this.dgPurchases.SelectedItems.Count > 0)
            {
                try
                {
                    if (this._collectionManager.RetrieveCollectionList(_user.User.UserID).Count == 0)
                    {
                        MessageBox.Show("You need to create a collection first!");
                    }
                    else
                    {
                        var purchaseCompleteForm = new frmCompletePurchase(_collectionManager, _marketEntryManager, _user.User, (PurchaseDetails)this.dgPurchases.SelectedItem);
                        var result = purchaseCompleteForm.ShowDialog();
                        if (result == true)
                        {
                            refreshPurchaseList();
                        }
                    }
                }
                catch (Exception ex)
                {

                    var message = ex.Message + "\n\n" + ex.InnerException;
                    MessageBox.Show(message, "Purchase Complete Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("You must select a purchase!");
            }

        }
    }
}
