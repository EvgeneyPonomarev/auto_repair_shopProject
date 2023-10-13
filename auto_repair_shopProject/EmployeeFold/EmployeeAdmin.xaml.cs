using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace auto_repair_shopProject.EmployeeFold
{
    public partial class EmployeeAdmin : Page
    {
        public EmployeeAdmin()
        {
            InitializeComponent();
        }

        private void UpdateEmployee()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            gridEmployee.ItemsSource = auto_repair_shopEntities.GetContext().Employees.ToList();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            if (gridEmployee.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить специалиста?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (gridEmployee.SelectedItem is Employees selectedEmployee)
                    {
                        auto_repair_shopEntities.GetContext().Employees.Remove(selectedEmployee);
                        auto_repair_shopEntities.GetContext().SaveChanges();
                        UpdateEmployee();
                    }
                }
            }
            else
                MessageBox.Show("Выберите запись для удаления");
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeeAdd(null));
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (gridEmployee.SelectedItem != null)
            {
                if (gridEmployee.SelectedItem is Employees selectedEmployee)
                    NavigationService.Navigate(new EmployeeAdd(selectedEmployee));
            }
            else
                MessageBox.Show("Выберите запись для изменения");
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateEmployee();
        }
    }
}
