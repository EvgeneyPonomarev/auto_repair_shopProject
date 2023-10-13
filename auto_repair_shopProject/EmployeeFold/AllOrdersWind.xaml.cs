using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace auto_repair_shopProject.EmployeeFold
{
    public partial class AllOrdersWind : Window
    {
        private Employees _employee;

        public AllOrdersWind(Employees employee)
        {
            InitializeComponent();

            _employee = employee;

            var employ = auto_repair_shopEntities.GetContext().Employees.ToList();

            employ.Insert(0, new Employees
            {
                surname = "Все мастера"
            });

            comboEmpl.ItemsSource = employ;
            comboEmpl.SelectedIndex = 0;

            UpdateGrid();
        }

        private void UpdateGrid()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            gridOrder.ItemsSource = auto_repair_shopEntities.GetContext().History_Orders.Where(p => p.id_employer != null).ToList();
        }

        private void comboEmpl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (comboEmpl.SelectedIndex == 0)
            {
                gridOrder.ItemsSource = GetAviableStudents();
            }
            else
            {
                var employ = comboEmpl.SelectedItem as Employees;
                if (employ != null)
                {
                    ICollectionView view = CollectionViewSource.GetDefaultView(gridOrder.ItemsSource);
                    view.Filter = s => ((History_Orders)s).id_employer == employ.id_employee;
                    gridOrder.ItemsSource = view;
                }
            }
        }

        private List<History_Orders> GetAviableStudents()
        {
            return auto_repair_shopEntities.GetContext().History_Orders.Where(p => p.id_employer != null).ToList();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            EmployeeWind wind = new EmployeeWind(_employee);
            wind.Show();
            Close();
        }
    }
}
