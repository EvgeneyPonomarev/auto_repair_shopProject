using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace auto_repair_shopProject.CarsFold
{
    public partial class CreateCarWind : Window
    {
        Cars _cars = new Cars();
        Clients _client;

        public CreateCarWind(Clients client, Cars selectedCars)
        {
            InitializeComponent();

            if (client == null)
                return;

            if (selectedCars != null)
                _cars = selectedCars;


            brand.MaxLength = 20;
            model.MaxLength = 20;
            year.MaxLength = 4;
            number.MaxLength = 10;


            _client = client;

            DataContext = _cars;
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_cars.car_brand))
                errors.AppendLine("Введите бренд");
            if (string.IsNullOrWhiteSpace(_cars.car_model))
                errors.AppendLine("Введите модель");
            if (string.IsNullOrWhiteSpace(_cars.year_of_manufacture.ToString()))
                errors.AppendLine("Введите год выпуска");
            if (string.IsNullOrWhiteSpace(_cars.number_car))
                errors.AppendLine("Введите номер машины");

            if (_cars.photo == null || _cars.photo.Length == 0)
                errors.AppendLine("Выберите изображение");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            
            if (_cars.id_car == 0)
            {
                _cars.id_client = _client.id_client;
                auto_repair_shopEntities.GetContext().Cars.Add(_cars);
            }

            try
            {
                auto_repair_shopEntities.GetContext().SaveChanges();
                MainWindow wind = new MainWindow(_client);
                wind.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (_cars.photo != null || _cars.photo.Length > 0)
            {
                var car = auto_repair_shopEntities.GetContext().Cars.Find(_cars.id_car);
                if (car != null) car.photo = _cars.photo;
                auto_repair_shopEntities.GetContext().SaveChanges();
            }
        }

        private void addImage_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg";
            if (openFile.ShowDialog() == true)
            {
                _cars.photo = File.ReadAllBytes(openFile.FileName);
                photo.Source = LoadImage(_cars.photo);
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;
            var image = new BitmapImage();

            using (var stream = new MemoryStream(imageData))
            {
                stream.Position = 0;
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }

        private void brand_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^A-Za-z]");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void year_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^0-9]");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void number_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex rgx = new Regex("[^А-Яа-я0-9]");
            e.Handled = rgx.IsMatch(e.Text);
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wind = new MainWindow(_client);
            wind.Show();
            Close();
        }
    }
}
