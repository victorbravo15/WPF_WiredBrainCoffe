using System.Windows;
using System.Windows.Controls;
using WiredBrainCoffe.CustomersAPP.Model;

namespace WiredBrainCoffe.CustomersAPP.Controls
{
    /// <summary>
    /// Lógica de interacción para CustomerDetailControl.xaml
    /// </summary>
    public partial class CustomerDetailControl : UserControl
    {
        public CustomerDetailControl()
        {
            InitializeComponent();
        }

        private bool isChangingSelectedCustomer;
        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set
            {
                this.isChangingSelectedCustomer = true;
                _customer = value;
                this.txtFirstName.Text = this._customer?.FirstName ?? "";
                this.txtLastName.Text = this._customer?.LastName ?? "";
                this.chkIsDeveloper.IsChecked = this._customer?.IsDeveloper;
                this.isChangingSelectedCustomer = false;
            }
        }

        private void TextBox_IsChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateCustomer();
        }

        private void CheckBox_IsChanged(object sender, RoutedEventArgs e)
        {
            this.UpdateCustomer();
        }

        private void UpdateCustomer()
        {
            if (this.Customer != null && !this.isChangingSelectedCustomer)
            {
                this.Customer.FirstName = this.txtFirstName.Text;
                this.Customer.LastName = this.txtLastName.Text;
                this.Customer.IsDeveloper = this.chkIsDeveloper.IsChecked.GetValueOrDefault();
            }
        }
    }
}
