using auto_repair_shopProject.LogIn;
using auto_repair_shopProject.RegFold;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Linq;
using System.Windows;


namespace auto_repair_shopProject.ProfileFold
{
    public partial class Profile : System.Windows.Window
    {
        Clients _client;

        public Profile(Clients client)
        {
            InitializeComponent();

            _client = client;
            HistoryOrder();
        }

        private void HistoryOrder()
        {
            auto_repair_shopEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
            viewOrder.ItemsSource = auto_repair_shopEntities.GetContext().History_Orders.Where(p => p.id_client == _client.id_client).ToList();
            viewNewOrder.ItemsSource = auto_repair_shopEntities.GetContext().Orders.Where(p => p.id_client == _client.id_client).ToList();
            DataClient();
        }

        private void DataClient()
        {
            surname.Content = _client.surname;
            name.Content = _client.name;
            patronymic.Content = _client.patronymic;
            email.Content = _client.email;
        }

        private void viewNewOrder_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (MessageBox.Show("Распечатать чек?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (viewNewOrder.SelectedItem is Orders selectedOrder)
                {
                    Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application().Application;
                    Document doc = wordApp.Documents.Add();

                    // Создание содержимого для печати
                    string content = $"Заказ №{selectedOrder.id_order}\n\n" +
                                     $"Дата заказа: {selectedOrder.date_order}\n" +
                                     $"Статус выполнения: {selectedOrder.execution_status}\n\n" +
                                     "Детали заказа:\n";

                    // Добавление таблицы с данными заказа
                    Table table = doc.Tables.Add(doc.Range(), 1, 5); // 1 строка, 5 столбцов
                    table.Borders.Enable = 1; // Включение границ таблицы

                    // Заполнение заголовков столбцов
                    table.Cell(1, 1).Range.Text = "Автомобиль";
                    table.Cell(1, 2).Range.Text = "Услуга";
                    table.Cell(1, 3).Range.Text = "Дата заказа";
                    table.Cell(1, 4).Range.Text = "Статус выполнения";
                    table.Cell(1, 5).Range.Text = "Цена";


                    var car = auto_repair_shopEntities.GetContext().Cars.FirstOrDefault(p => p.id_car == selectedOrder.id_car);
                    var service = auto_repair_shopEntities.GetContext().Services.FirstOrDefault(p => p.id_service == selectedOrder.id_service);


                    // Заполнение данными
                    table.Rows.Add(); // Добавление новой строки
                    table.Cell(2, 1).Range.Text = car.car_brand;
                    table.Cell(2, 2).Range.Text = service.name_service;
                    table.Cell(2, 3).Range.Text = selectedOrder.date_order.ToString();
                    table.Cell(2, 4).Range.Text = selectedOrder.execution_status.ToString() + "%";
                    table.Cell(2, 5).Range.Text = selectedOrder.price.ToString() + "р.";

                    // Сохранение документа
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Документ Word (*.docx)|*.docx";
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        string filePath = saveFileDialog.FileName;
                        doc.SaveAs2(filePath);
                        doc.Close();
                        wordApp.Quit();
                        MessageBox.Show("Документ успешно сохранен.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        doc.Close();
                        wordApp.Quit();
                    }
                }
            }
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            RegWind wind = new RegWind(_client, 1);
            wind.Show();
            Close();
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auto_repair_shopEntities.GetContext().Clients.Remove(_client);
                auto_repair_shopEntities.GetContext().SaveChanges();

                LogWind wind = new LogWind();
                wind.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow wind = new MainWindow(_client);
            wind.Show();
            Close();
        }
    }
}
