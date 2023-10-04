using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WiredBrainCoffe.CustomersAPP.Model;
using WiredBrainCoffe.CustomersAPP.DataProvider;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;

namespace WiredBrainCoffe.CustomersAPP
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CustomerDataProvider _customerDataProvider;
        private AppConfig _config;

        public MainWindow()
        {
            this.LoadAppConfig();
            this._config.IsDarkTheme = !this._config.IsThemeConfSaved ? this.GetSystemThemePreference() : this._config.IsDarkTheme;
            this.ApplyTheme();
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            App.Current.Exit += App_Exit;
            this._customerDataProvider = new CustomerDataProvider();
        }

        #region WindowsActions
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.customerListView.Items.Clear();

            var customers = this._customerDataProvider.LoadCustomersAsync();
            foreach (var customer in customers)
            {
                this.customerListView.Items.Add(customer);
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            this._customerDataProvider.SaveCustomersAsync(this.customerListView.Items.OfType<Customer>());
            this.SaveAppConfig();
        }
        #endregion

        #region CustomersButtons
        private void ButtonAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = new Customer { FirstName = "Username", LastName = "LastName", IsDeveloper = false };
            this.customerListView.Items.Add(customer);
            this.customerListView.SelectedItem = customer;
            MessageBox.Show("Customer added!", "Added", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ButtonDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            var customer = this.customerListView.SelectedItem as Customer;
            if (customer != null)
            {
                this.customerListView.Items.Remove(customer);
            }
        }
        #endregion

        private void ButtonMove_Click(object sender, RoutedEventArgs e)
        {
            int column = Grid.GetColumn(this.customerListGrid);

            int newColumn = column == 0 ? 2 : 0;
            Grid.SetColumn(this.customerListGrid, newColumn);

            var symbolImage = newColumn == 0 ? "https://icons.veryicon.com/png/o/commerce-shopping/online-retailers/arrow-right-33.png" : "https://icons.veryicon.com/png/o/miscellaneous/eva-icon-fill/arrow-back-8.png";
            this.moveSymbolIcon.Source = new BitmapImage(new Uri(symbolImage, UriKind.RelativeOrAbsolute));
        }


        private void ApplyTheme()
        {
            // Define los recursos de estilo que deseas cambiar según el tema
            ResourceDictionary newTheme = new ResourceDictionary();

            if (this._config.IsDarkTheme)
            {
                // Aplica el tema oscuro
                newTheme.Source = new Uri("Resources/DarkTheme.xaml", UriKind.RelativeOrAbsolute);
                // Elimina los recursos de estilo del tema claro si existen
                var lightTheme = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(dictionary => dictionary.Source.OriginalString == "Resources/LightTheme.xaml");

                if (lightTheme != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(lightTheme);
                }
            }
            else
            {
                // Aplica el tema claro
                newTheme.Source = new Uri("Resources/LightTheme.xaml", UriKind.RelativeOrAbsolute);
                // Elimina los recursos de estilo del tema oscuro si existen
                var darkTheme = Application.Current.Resources.MergedDictionaries
                    .FirstOrDefault(dictionary => dictionary.Source.OriginalString == "Resources/DarkTheme.xaml");

                if (darkTheme != null)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(darkTheme);
                }
            }

            Application.Current.Resources.MergedDictionaries.Add(newTheme);
        }

        private bool GetSystemThemePreference()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
            {
                if (key != null)
                {
                    object themeValue = key.GetValue("AppsUseLightTheme");
                    if (themeValue != null && themeValue is int themeIntValue)
                    {
                        // 0 = Dark theme, 1 = Light theme
                        return themeIntValue == 0;
                    }
                }
            }

            // Por defecto, si no se puede determinar, asumimos tema claro
            return false;
        }

        private void ButtonToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            this._config.IsDarkTheme = !this._config.IsDarkTheme;
            this.ApplyTheme();
            string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            // Inicia un nuevo proceso para reiniciar la aplicación
            System.Diagnostics.Process.Start(appPath);

            // Cierra la instancia actual de la aplicación
            Application.Current.Shutdown();
        }

        public void LoadAppConfig()
        {
            string configFilePath = "config.json"; // Ruta al archivo JSON de configuración
            if (File.Exists(configFilePath))
            {
                string json = File.ReadAllText(configFilePath);
                this._config = JsonConvert.DeserializeObject<AppConfig>(json);
                this._config.IsThemeConfSaved = true;
            }
            else
            {
                // Si el archivo no existe, crea una configuración predeterminada
                this._config = new AppConfig { IsDarkTheme = false, IsThemeConfSaved = false }; // O el valor predeterminado que desees
            }
        }

        public void SaveAppConfig()
        {
            string configFilePath = "config.json"; // Ruta al archivo JSON de configuración
            string json = JsonConvert.SerializeObject(this._config);
            File.WriteAllText(configFilePath, json);
        }
    }
}
