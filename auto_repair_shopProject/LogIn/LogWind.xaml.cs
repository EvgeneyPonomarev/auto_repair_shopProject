using auto_repair_shopProject.AdminFold;
using auto_repair_shopProject.CarsFold;
using auto_repair_shopProject.EmployeeFold;
using auto_repair_shopProject.RegFold;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace auto_repair_shopProject.LogIn
{
    public partial class LogWind : Window
    {
        public LogWind()
        {
            InitializeComponent();

            LoginBox.MaxLength = 20;
            PswdBox.MaxLength = 20;
            Password.MaxLength = 20;
        }

        private void checkPas_Click(object sender, RoutedEventArgs e)
        {
            if (checkPas.IsChecked != null && checkPas.IsChecked.Value)
            {
                PswdBox.Text = Password.Password;
                PswdBox.Visibility = Visibility.Visible;
                Password.Visibility = Visibility.Hidden;
            }
            else
            {
                Password.Password = PswdBox.Text;
                PswdBox.Visibility = Visibility.Hidden;
                Password.Visibility = Visibility.Visible;
            }
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text) || string.IsNullOrWhiteSpace(Password.Password))
            {
                MessageBox.Show("Введите все данные", "Уведомление");
            }
            else
            {
                Clients client = auto_repair_shopEntities.GetContext().Clients.FirstOrDefault(p => p.login == LoginBox.Text);
                Employees employee = auto_repair_shopEntities.GetContext().Employees.FirstOrDefault(p => p.login == LoginBox.Text);
                Admin admin = auto_repair_shopEntities.GetContext().Admin.FirstOrDefault(p => p.login == LoginBox.Text);

                if (client != null && client.password == Password.Password || client != null && client.password == PswdBox.Text)
                {
                    CreateCarWind wind = new CreateCarWind(client, null);
                    wind.Show();
                    Close();
                }
                else if (employee != null && employee.password == Password.Password || employee != null && employee.password == PswdBox.Text)
                {
                    EmployeeWind wind = new EmployeeWind(employee);
                    wind.Show();
                    Close();
                }
                else if (admin != null && admin.password == Password.Password || admin != null && admin.password == PswdBox.Text)
                {
                    AdminWind wind = new AdminWind();
                    wind.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Проверьте корректность данных", "Уведомление");
                }
            }
        }

        private void LoginBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]+");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void BtnReg_Click(object sender, RoutedEventArgs e)
        {
            RegWind wind = new RegWind(null, 0);
            wind.Show();
            Close();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
