using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using WiredBrainCoffe.CustomersAPP.Model;

namespace WiredBrainCoffe.CustomersAPP.DataProvider
{
    public class CustomerDataProvider
    {
        private static readonly string _customerFilename = "customer.json";
        private static readonly StorageFolder _localFolder = ApplicationData.Current.LocalFolder;

        public IEnumerable<Customer> LoadCustomersAsync()
        {
            string filePath = Path.Combine(_localFolder.Path, _customerFilename);
            var json = "";
            if (File.Exists(filePath))
            {
                json = File.ReadAllText(filePath);
            }

            List<Customer> customerList;

            if (String.IsNullOrEmpty(json))
            {
                customerList = new List<Customer>
                {
                    new Customer{FirstName="Thomas", LastName="Huber", IsDeveloper=true},
                    new Customer{FirstName="Anna", LastName="Rockstar", IsDeveloper=true},
                    new Customer{FirstName="Julia", LastName="Master"},
                    new Customer{FirstName="Urs", LastName="Meier", IsDeveloper=true},
                    new Customer{FirstName="Sara", LastName="Ramone"},
                    new Customer{FirstName="Elsa", LastName="Queen"},
                    new Customer{FirstName="Alex", LastName="Baier", IsDeveloper=true},
                };
            }
            else
            {
                customerList = new List<Customer>(JsonConvert.DeserializeObject<List<Customer>>(json));
            }
            return customerList;
        }

        public void SaveCustomersAsync(IEnumerable<Customer> customers)
        {
            var json = JsonConvert.SerializeObject(customers);
            string filePath = Path.Combine(_localFolder.Path, _customerFilename);
            File.WriteAllText(filePath, json);
        }
    }
}
