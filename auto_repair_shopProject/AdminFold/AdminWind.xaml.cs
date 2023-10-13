using auto_repair_shopProject.EmployeeFold;
using auto_repair_shopProject.LogIn;
using auto_repair_shopProject.PartsFold;
using auto_repair_shopProject.ServicesFold;
using auto_repair_shopProject.SpecialitiesFold;
using System.Windows;

namespace auto_repair_shopProject.AdminFold
{
    public partial class AdminWind : Window
    {
        public AdminWind()
        {
            InitializeComponent();
        }

        private void employee_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new EmployeeAdmin();
        }

        private void service_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new ServicesAdmin();
        }

        private void parts_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new SparePartsAdmin();
        }

        private void specialities_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new SpecialitiesAdmin();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            LogWind wind = new LogWind();
            wind.Show();
            Close();
        }
    }
}
