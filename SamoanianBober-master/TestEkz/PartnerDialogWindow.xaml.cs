using System.Windows;
using Microsoft.EntityFrameworkCore;
using TestEkz.Entities;

namespace TestEkz
{
    public partial class PartnerDialogWindow : Window
    {
        public Partner Partner { get; private set; }
        public string WindowTitle { get; private set; }

        public PartnerDialogWindow(Partner partner = null)
        {
            InitializeComponent();
            DataContext = this;

            if (partner == null)
            {
                Partner = new Partner
                {
                    DirectorName = "Неизвестно",
                    LegalAddress = "Не указан"
                };
                WindowTitle = "Добавить нового партнера";
            }
            else
            {
                Partner = new Partner
                {
                    PartnerId = partner.PartnerId,
                    CompanyName = partner.CompanyName,
                    PartnerTypeId = partner.PartnerTypeId,
                    TaxId = partner.TaxId,
                    Phone = partner.Phone,
                    Email = partner.Email,
                    DirectorName = partner.DirectorName ?? "Неизвестно",
                    LegalAddress = partner.LegalAddress ?? "Не указан",
                    CurrentDiscount = partner.CurrentDiscount,
                    Rating = partner.Rating,
                    TotalSalesVolume = partner.TotalSalesVolume
                };
                WindowTitle = "Редактировать партнера";
            }

            LoadPartnerTypes();
            LoadPartnerData();
        }

        private void LoadPartnerTypes()
        {
            using (var context = new TestEkzContext())
            {
                PartnerTypeComboBox.ItemsSource = context.PartnerTypes.ToList();
            }
        }

        private void LoadPartnerData()
        {
            CompanyNameTextBox.Text = Partner.CompanyName;
            TaxIdTextBox.Text = Partner.TaxId;
            PhoneTextBox.Text = Partner.Phone;
            EmailTextBox.Text = Partner.Email;
            DirectorNameTextBox.Text = Partner.DirectorName;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CompanyNameTextBox.Text))
            {
                MessageBox.Show("Введите название компании!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(TaxIdTextBox.Text))
            {
                MessageBox.Show("Введите ИНН!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (PartnerTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип партнера!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Partner.CompanyName = CompanyNameTextBox.Text.Trim();
            Partner.TaxId = TaxIdTextBox.Text.Trim();
            Partner.Phone = PhoneTextBox.Text?.Trim();
            Partner.Email = EmailTextBox.Text?.Trim();
            Partner.PartnerTypeId = ((PartnerType)PartnerTypeComboBox.SelectedItem).PartnerTypeId;
            Partner.DirectorName = string.IsNullOrWhiteSpace(DirectorNameTextBox.Text) ? "Неизвестно" : DirectorNameTextBox.Text.Trim();
            Partner.LegalAddress = Partner.LegalAddress ?? "Не указан"; // Ensure non-null

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}