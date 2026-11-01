using System.Windows;

namespace ExcelTool.Features.Customer
{
    public partial class CustomerDetailWindow : Window
    {
        public CustomerDetailWindow(
            string id,
            string shortName,
            string fullName,
            string address,
            string taxCode,
            string contact,
            string email,
            string phone)
        {
            InitializeComponent();

            IdText.Text = id;
            ShortNameText.Text = shortName;
            FullNameText.Text = fullName;
            AddressText.Text = address;
            TaxCodeText.Text = taxCode;
            ContactText.Text = contact;
            EmailText.Text = email;
            PhoneText.Text = phone;
        }


        // ==========================================
        // SỬA
        // ==========================================

        private void EditButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Customer customer = new Customer
            {
                Id = IdText.Text,
                ShortName = ShortNameText.Text,
                FullName = FullNameText.Text,
                Address = AddressText.Text,
                TaxCode = TaxCodeText.Text,
                ContactPerson = ContactText.Text,
                Email = EmailText.Text,
                Phone = PhoneText.Text
            };

            CustomerEditWindow editWindow =
                new CustomerEditWindow(customer);

            editWindow.Owner = this;

            bool? result =
                editWindow.ShowDialog();

            if (result == true)
            {
                IdText.Text = customer.Id;
                ShortNameText.Text = customer.ShortName;
                FullNameText.Text = customer.FullName;
                AddressText.Text = customer.Address;
                TaxCodeText.Text = customer.TaxCode;
                ContactText.Text = customer.ContactPerson;
                EmailText.Text = customer.Email;
                PhoneText.Text = customer.Phone;

                DialogResult = true;
            }
        }


        // ==========================================
        // ĐÓNG
        // ==========================================

        private void CloseButton_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}