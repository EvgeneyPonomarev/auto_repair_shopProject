using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Linq;
using System.Text;
using System;

namespace auto_repair_shopProject.EmployeeFold
{
    public partial class EmployeeAdd : Page
    {
        private Employees _employee = new Employees();

        public EmployeeAdd(Employees selectedEmployee)
        {
            InitializeComponent();

            var listSpec = auto_repair_shopEntities.GetContext().Specialities.ToList();
            comboSpec.ItemsSource = listSpec;

            if (selectedEmployee != null)
            {
                _employee = selectedEmployee;

                var spec = listSpec.Where(g => g.id_specialties.Equals(selectedEmployee.id_specialties));
                var specEnumerable = spec as Specialities[] ?? spec.ToArray();
                if (specEnumerable.Any())
                    comboSpec.SelectedValue = specEnumerable.First();
            }

            name.MaxLength = 20;
            surname.MaxLength = 30;
            patronymic.MaxLength = 40;
            jobtitle.MaxLength = 40;
            email.MaxLength = 60;
            login.MaxLength = 20;
            password.MaxLength = 20;

            DataContext = _employee;
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(pattern);

            if (string.IsNullOrWhiteSpace(_employee.name))
                errors.AppendLine("Введите имя");
            if (string.IsNullOrWhiteSpace(_employee.surname))
                errors.AppendLine("Введите фамилию");
            if (string.IsNullOrWhiteSpace(_employee.job_title))
                errors.AppendLine("Введите должность");
            if (string.IsNullOrWhiteSpace(_employee.email))
                errors.AppendLine("Введите почту");
            else
            {
                if (!regex.IsMatch(_employee.email))
                    errors.AppendLine("Почта введена неккоректно");
            }
            if (string.IsNullOrWhiteSpace(_employee.login))
                errors.AppendLine("Введите логин");
            if (string.IsNullOrWhiteSpace(_employee.password))
                errors.AppendLine("Введите пароль");

            if (comboSpec.SelectedItem is Specialities selectedSpec)
                _employee.id_specialties = selectedSpec.id_specialties;

            if (comboSpec.SelectedItem == null)
                errors.AppendLine("Выберите специальность");
            
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_employee.id_employee == 0)
                auto_repair_shopEntities.GetContext().Employees.Add(_employee);

            try
            {
                auto_repair_shopEntities.GetContext().SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void login_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9]+");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
