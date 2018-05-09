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
    /// Interaction logic for frmPurchase.xaml
    /// </summary>
    public partial class frmPurchase : Window
    {

        private MarketEntryManager _marketEntryManager = new MarketEntryManager();
        private User _user;
        private MarketEntryDetails _marketEntryDetails;

        public frmPurchase(User user, MarketEntryDetails marketEntryDetails)
        {
            _user = user;
            _marketEntryDetails = marketEntryDetails;
            InitializeComponent();
            setControls();
        }

        private void btnPurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var result = _marketEntryManager.AddMarketEntryPurchase(_user, _marketEntryDetails.MarketEntry);
                if(result != 2)
                {
                    throw new ApplicationException("Could not update market entry for purchase");
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
                MessageBox.Show(message, "Could not perform purchase", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.DialogResult = false;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void setControls()
        {
            this.txtUser.Text = _marketEntryDetails.User.Gamertag;
            this.txtCreature.Text = _marketEntryDetails.CollectionEntry.CreatureID;
            this.txtName.Text = _marketEntryDetails.CollectionEntry.Name;
            this.txtLevel.Text = _marketEntryDetails.CollectionEntry.Level.ToString();
            this.txtHealth.Text = _marketEntryDetails.CollectionEntry.Health.ToString();
            this.txtStamina.Text = _marketEntryDetails.CollectionEntry.Stamina.ToString();
            this.txtOxygen.Text = _marketEntryDetails.CollectionEntry.Oxygen.ToString();
            this.txtFood.Text = _marketEntryDetails.CollectionEntry.Food.ToString();
            this.txtWeight.Text = _marketEntryDetails.CollectionEntry.Weight.ToString();
            this.txtBaseDamage.Text = _marketEntryDetails.CollectionEntry.BaseDamage.ToString();
            this.txtTorpor.Text = _marketEntryDetails.CollectionEntry.Torpor.ToString();
            this.txtImprint.Text = _marketEntryDetails.CollectionEntry.Imprint.ToString();
            this.txtSpeed.Text = _marketEntryDetails.CollectionEntry.MovementSpeed.ToString();
        }
    }
}
