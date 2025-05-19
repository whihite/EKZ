using System.Windows;
using System.Windows.Controls;
using TestEkz.Entities;

namespace TestEkz
{
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameTextBox.Text.Trim();
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            using (var context = new TestEkzContext())
            {
                if (context.Employees.Any(e => e.Username == username))
                {
                    MessageBox.Show("Такой пользователь уже существует!");
                    return;
                }

                var userRole = context.Roles.FirstOrDefault(r => r.RoleName == "Пользователь");
                if (userRole == null)
                {
                    MessageBox.Show("Роль 'Пользователь' не найдена.");
                    return;
                }

                var newEmployee = new Employee
                {
                    FullName = fullName,
                    Username = username,
                    Password = password,
                    RoleId = userRole.RoleId,
                    AccountStatus = true,
                    BirthDate = DateTime.Now
                };

                context.Employees.Add(newEmployee);
                context.SaveChanges();
                MessageBox.Show("Регистрация успешна!");

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}