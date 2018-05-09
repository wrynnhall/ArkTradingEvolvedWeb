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
    /// Interaction logic for frmAddEditCollection.xaml
    /// </summary>
    public partial class frmAddEditCollection : Window
    {
        private CollectionManager _collectionManager;
        private Collection _collection;
        private CollectionForm _type;
        private User _user;

        public frmAddEditCollection()
        {
            InitializeComponent();
        }

        public frmAddEditCollection(CollectionManager collMgr, Collection collection, CollectionForm type)
        {
            _collectionManager = collMgr;
            _collection = collection;
            _type = type;
            InitializeComponent();
        }

        public frmAddEditCollection(CollectionManager collMgr, User user)
        {
            _collectionManager = collMgr;
            _collection = null;
            _type = CollectionForm.Add;
            _user = user;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_type)
            {
                case CollectionForm.Add:
                    setupAddForm();
                    break;
                case CollectionForm.Edit:
                    setupEditForm();
                    break;
                case CollectionForm.View:
                    setupViewForm();
                    break;
            }
            
            this.txtCollectionID.IsEnabled = false;
        }


        private void setupAddForm()
        {
            btnAddEdit.Content = "Save";
            this.Title = "Add a New Collection";
        }

        private void setupEditForm()
        {
            btnAddEdit.Content = "Save";
            this.Title = "Edit the Collection";
            populateControls();
            setControls(false);
        }

        private void setupViewForm()
        {
            btnAddEdit.Content = "Edit";
            this.Title = "Review the Collection";
            populateControls();
            setControls();
        }


        private void populateControls()
        {
            this.txtCollectionID.Text = _collection.CollectionID.ToString();
            this.txtName.Text = _collection.Name;
        }

        private void setControls(bool readOnly = true)
        {
            this.txtName.IsReadOnly = readOnly;
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            if(_type == CollectionForm.View)
            {
                setupEditForm();
                _type = CollectionForm.Edit;
            }
            else if(_type == CollectionForm.Edit)
            {
                performEdit();
            }
            else if (_type == CollectionForm.Add)
            {
                performAdd();
            }
        }

        private void performAdd()
        {
            if (validateInputs())
            {
                var newCollection = new Collection()
                {
                    Name = txtName.Text,
                    UserID = _user.UserID
                };
                try
                {
                    int result = _collectionManager.AddCollection(newCollection);
                    if (result != 1)
                    {
                        throw new ApplicationException("Collection was not added");
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

        private void performEdit()
        {
            if (validateInputs())
            {
                var newCollection = new Collection()
                {
                    CollectionID = _collection.CollectionID,
                    Name = txtName.Text,
                    UserID = _collection.UserID
                };
                try
                {
                    int result = _collectionManager.EditCollection(_collection, newCollection);
                    if (result != 1)
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

        private bool validateInputs()
        {
            bool isValid = true;

            if(txtName.Text == "")
            {
                MessageBox.Show("Name field cannot be empty", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            if(txtName.Text.Length > 100)
            {
                MessageBox.Show("Name field cannot be greater than 100 characters", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            
            return isValid;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result  = MessageBox.Show("Are you sure you want to Cancel?\nCanceling will discard any unsaved changes!", "Cancel Warning", MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
    }
}
