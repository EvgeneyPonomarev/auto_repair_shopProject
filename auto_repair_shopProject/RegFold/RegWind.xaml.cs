using auto_repair_shopProject.LogIn;
using auto_repair_shopProject.ProfileFold;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace auto_repair_shopProject.RegFold
{
    public partial class RegWind : Window
    {
        private Clients _client = new Clients();
        private int _count;

        public RegWind(Clients client, int count)
        {
            InitializeComponent();

            _count = count;

            if (client != null)
                _client = client;

            name.MaxLength = 20;
            surname.MaxLength = 20;
            patronymic.MaxLength = 40;
            email.MaxLength = 50;
            log.MaxLength = 20;
            pswd.MaxLength = 20;

            DataContext = _client;
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            var busyLog = auto_repair_shopEntities.GetContext().Clients.FirstOrDefault(p => p.login == log.Text);

            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(pattern);

            if (string.IsNullOrWhiteSpace(_client.name))
                errors.AppendLine("Введите имя");
            if (string.IsNullOrWhiteSpace(_client.surname))
                errors.AppendLine("Введите фамилию");
            if (string.IsNullOrWhiteSpace(_client.email))
                errors.AppendLine("Введите почту");
            else
            {
                if (!regex.IsMatch(_client.email))
                    errors.AppendLine("Почта введена неккоректно");
            }
            if (string.IsNullOrWhiteSpace(_client.login))
                errors.AppendLine("Введите логин");
            if (string.IsNullOrWhiteSpace(_client.password))
                errors.AppendLine("Введите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_client.id_client == 0)
                auto_repair_shopEntities.GetContext().Clients.Add(_client);

            try
            {
                auto_repair_shopEntities.GetContext().SaveChanges();

                if (_count == 0)
                {
                    LogWind wind = new LogWind();
                    wind.Show();
                    Close();
                }
                else
                {
                    Profile wind = new Profile(_client);
                    wind.Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            if (_count == 0)
            {
                LogWind wind = new LogWind();
                wind.Show();
                Close();
            }
            else
            {
                Profile wind = new Profile(_client);
                wind.Show();
                Close();
            }
        }

        private void name_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^А-Яа-я]+");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void email_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9@.-]+");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void log_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]+");
            e.Handled = rgx.IsMatch(e.Text);
        }
    }
}
