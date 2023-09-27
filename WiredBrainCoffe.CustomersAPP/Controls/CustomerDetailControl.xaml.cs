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
        // Using a DependencyProperty as the backing store for Customer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerProperty =
            DependencyProperty.Register("Customer", typeof(Customer), typeof(CustomerDetailControl), new PropertyMetadata(null));


        public CustomerDetailControl()
        {
            InitializeComponent();
        }

        public Customer Customer
        {
            get { return (Customer)GetValue(CustomerProperty); }
            set { SetValue(CustomerProperty, value); }
        }
    }
}
