using System.Windows;
using System.Windows.Controls;
using TestEkz.Entities;

namespace TestEkz
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow();
            regWindow.Show();
            this.Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = LoginTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            using (var db = new TestEkzContext())
            {
                var user = db.Employees.FirstOrDefault(u => u.Username == username && u.Password == password);
                if (user != null)
                {
                    MessageBox.Show("Успешный вход!", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Information);
                    PartnersWindow partnersWindow = new PartnersWindow();
                    partnersWindow.Show();
                    this.Close();

                } else {
                    MessageBox.Show("Неверный логин или пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}