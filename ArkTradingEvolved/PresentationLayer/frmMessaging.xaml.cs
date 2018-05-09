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
    /// Interaction logic for frmMessaging.xaml
    /// </summary>
    public partial class frmMessaging : Window
    {
        private List<Message> _messages;
        private MarketEntryDetails _marketEntryDetails;
        private MessageManager _messageManager = new MessageManager();
        private User _user;
        private User _purchaser;

        public frmMessaging(MarketEntryDetails details, User user, User purchaser)
        {
            _marketEntryDetails = details;
            _user = user;
            _purchaser = purchaser;
            InitializeComponent();
            populateControls();
        }

        private void refreshMessages()
        {
            this.stackpanelMessages.Children.Clear();
            // get the list of messages
            try
            {
                this._messages = this._messageManager.RetrieveMessagesByMarketEntryID(_marketEntryDetails.MarketEntry.MarketEntryID);

            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Message Retrieval Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            foreach (Message m in _messages)
            {

                if (m.UserID == this._user.UserID)
                {
                    this.stackpanelMessages.Children.Add(getMyTextBlock("You", m.Text));
                }
                else
                {
                    if (m.UserID == _marketEntryDetails.User.UserID)
                    {
                        this.stackpanelMessages.Children.Add(getTheirTextBlock(_marketEntryDetails.User.Gamertag, m.Text));
                    }
                    else
                    {
                        this.stackpanelMessages.Children.Add(getTheirTextBlock(_purchaser.Gamertag, m.Text));
                    }
                    
                }
            }

        }

        private void populateControls()
        {
            refreshMessages();
        }

        private TextBlock getMyTextBlock(string username, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(10, 2, 10, 2);
            textBlock.Text = username + " > " + text;
            textBlock.Foreground = Brushes.Green;
            textBlock.TextWrapping = TextWrapping.Wrap;
            
            return textBlock;
        }

        private TextBlock getTheirTextBlock(string username, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Margin = new Thickness(10, 5, 10, 5);
            textBlock.Text = username + " > " + text;
            textBlock.Foreground = Brushes.SlateGray;
            textBlock.TextWrapping = TextWrapping.Wrap;
            return textBlock;
        }

        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (txtMessage.Text == "")
            {
                MessageBox.Show("You must enter some text!");
                return;
            }

            if (txtMessage.Text.Length > 150)
            {
                MessageBox.Show("Text length can only be 150 characters or less!");
                return;
            }

            try
            {
                var message = new Message(){
                    UserID = _user.UserID,
                    MarketEntryID = _marketEntryDetails.MarketEntry.MarketEntryID,
                    Text = this.txtMessage.Text,
                    SentTime = DateTime.Now
                };
                _messageManager.AddMessage(message);
                refreshMessages();
                this.txtMessage.Text = "";
            }
            catch (Exception ex)
            {
                var message = ex.Message + "\n\n" + ex.InnerException;
                MessageBox.Show(message, "Message Add Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            refreshMessages();
        }
    }
}
