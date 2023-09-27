using System;
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
        private static bool isChangingSelectedCustomer;

        // Using a DependencyProperty as the backing store for Customer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerProperty =
            DependencyProperty.Register("Customer", typeof(Customer), typeof(CustomerDetailControl), new PropertyMetadata(null, CustomerChangeCallback));


        public CustomerDetailControl()
        {
            InitializeComponent();
        }

        public Customer Customer
        {
            get { return (Customer)GetValue(CustomerProperty); }
            set { SetValue(CustomerProperty, value); }
        }


        private static void CustomerChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomerDetailControl customerDetailControl)
            {
                var customer = e.NewValue as Customer;
                isChangingSelectedCustomer = true;
                customerDetailControl.txtFirstName.Text = customer?.FirstName ?? "";
                customerDetailControl.txtLastName.Text = customer?.LastName ?? "";
                customerDetailControl.chkIsDeveloper.IsChecked = customer?.IsDeveloper;
                isChangingSelectedCustomer = false;
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
            if (this.Customer != null && !isChangingSelectedCustomer)
            {
                this.Customer.FirstName = this.txtFirstName.Text;
                this.Customer.LastName = this.txtLastName.Text;
                this.Customer.IsDeveloper = this.chkIsDeveloper.IsChecked.GetValueOrDefault();
            }
        }
    }
}
