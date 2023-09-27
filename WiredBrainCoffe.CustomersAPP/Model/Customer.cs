namespace WiredBrainCoffe.CustomersAPP.Model
{
    public class Customer : Observable
    {
        private string _firstName;
        private string _lastName;
        private bool isDeveloper;

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }
        public string LastName
        {
            get => _lastName; set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
        public bool IsDeveloper
        {
            get => isDeveloper; set
            {
                isDeveloper = value;
                OnPropertyChanged();
            }
        }
    }
}
