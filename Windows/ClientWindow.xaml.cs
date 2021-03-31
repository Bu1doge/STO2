using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ООО_Поломка_STO2.EFData;

namespace ООО_Поломка_STO2
{
    
    public partial class ClientWindow : Window
    {
        private int PageNumber = 1;
        private int NumberOfPages;

        public ClientWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RecordsAmount_CMB.ItemsSource = new List<string>() { "10", "50", "200", "ВСЕ" };
            RecordsAmount_CMB.SelectedItem = "ВСЕ";

            List<string> genders = DataFrame.Context.Gender.Select(i => i.Name).ToList();
            genders.Add("ВСЕ");
            Gender_CMB.ItemsSource = genders;
            Gender_CMB.SelectedItem = "ВСЕ";

            SortField_CMB.ItemsSource = new List<string>() { "Идентификатор", "Фамилия", "Количество посещений" };
            SortField_CMB.SelectedItem = "Идентификатор";

            SortMethod_CMB.ItemsSource = new List<string>() { "Убыванию", "Возрастанию"};
            SortMethod_CMB.SelectedItem = "Возрастанию";

            Delete_BTN.Visibility = Visibility.Collapsed;

            ViewClients();
        }

        private List<ClientView> Sort(List<ClientView> clients, string field, string method)
        {
            //Выборка по полям и методу сортировки
            switch (field)
            {
                case "Идентификатор":
                    clients = (method.Equals("Убыванию") ? clients.OrderByDescending(i => i.Id) : clients.OrderBy(i => i.Id)).ToList();
                    break;
                case "Фамилия":
                    clients = (method.Equals("Убыванию") ? clients.OrderByDescending(i => i.LastName) : clients.OrderBy(i => i.LastName)).ToList();
                    break;
                case "Количество посещений":
                    clients = (method.Equals("Убыванию") ? clients.OrderByDescending(i => i.VisitAmount) : clients.OrderBy(i => i.VisitAmount)).ToList();
                    break;
            }

            return clients;
        }

        private void ViewClients()
        {
            List<ClientView> selectedClients = DataFrame.Context.ClientView.ToList();
            int amountOfRecordsInDB = selectedClients.Count;

            //Выделение поисковых запросов
            string FirstNameQuery   = FirstName_TXB.Text;
            string SurnameQuery     = Surname_TXB.Text;
            string PatronymicQuery  = Patronymic_TXB.Text;
            string EmailQuery       = EMail_TXB.Text;
            string PhoneNumberQuery = PhoneNumber_TXB.Text;

            //Филтрация по поисковым запросам
            if (!string.IsNullOrWhiteSpace(FirstNameQuery))
            {
                selectedClients = selectedClients.Where(i => i.FirstName.ToLower().Contains(FirstNameQuery.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(SurnameQuery))
            {
                selectedClients = selectedClients.Where(i => i.LastName.ToLower().Contains(SurnameQuery.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(PatronymicQuery))
            {
                selectedClients = selectedClients.Where(i => i.Patronymic.ToLower().Contains(PatronymicQuery.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(EmailQuery))
            {
                selectedClients = selectedClients.Where(i => i.Email.ToLower().Contains(EmailQuery.ToLower())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(PhoneNumberQuery))
            {
                selectedClients = selectedClients.Where(i => i.PhoneNumber.ToLower().Contains(PhoneNumberQuery.ToLower())).ToList();
            }

            //Фильтрация по гендеру
            if (!(Gender_CMB.SelectedItem is null))
            {
                if (!Gender_CMB.SelectedItem.Equals("ВСЕ"))
                {
                    selectedClients = selectedClients.Where(i => i.Gender.Equals(Gender_CMB.SelectedItem)).ToList();
                }
            }

            //Выделение клиентов с датой рождения в этом месяце
            if (BirtDayInThatMounth_Check.IsChecked == true)
            {
                selectedClients = selectedClients.Where(i => i.BirthDate.Value.Month == DateTime.Now.Month).ToList();
            }

            //Сортировка
            if (!(SortField_CMB.SelectedItem is null) && !(SortMethod_CMB.SelectedItem is null))
            {
                selectedClients = Sort(selectedClients, SortField_CMB.SelectedItem.ToString(), SortMethod_CMB.SelectedItem.ToString());
            }

            //Вывод количества выбранных записей
            PickedOutOfAll_TXT.Text = selectedClients.Count.ToString() + " из " + amountOfRecordsInDB.ToString();

            //Разброс по страницам
            if (!(RecordsAmount_CMB.SelectedItem is null))
            {
                if (!RecordsAmount_CMB.SelectedItem.ToString().Equals("ВСЕ"))
                {
                    int pickedAmount = int.Parse(RecordsAmount_CMB.SelectedItem.ToString());
                    NumberOfPages = selectedClients.Count / pickedAmount + (selectedClients.Count % pickedAmount > 0 ? 1 : 0);
                    selectedClients = selectedClients.Skip((PageNumber - 1) * pickedAmount).Take(pickedAmount).ToList();
                }
            }

            Client_LV.ItemsSource = selectedClients;
        }

        private void RecordsAmount_CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageNumber = 1;
            ViewClients();
        }

        private void PrevPage_Click(object sender, RoutedEventArgs e)
        {
            PageNumber -= PageNumber > 1 ? 1 : 0;
            ViewClients();
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            PageNumber += PageNumber < NumberOfPages ? 1 : 0;
            ViewClients();
        }

        private void Gender_CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageNumber = 1;
            ViewClients();
        }

        private void Search_TXB_TextChanged(object sender, TextChangedEventArgs e)
        {
            PageNumber = 1;
            ViewClients();
        }

        private void Sort_CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PageNumber = 1;
            ViewClients();
        }

        private void BirtDayInThatMounth_CheckChange(object sender, RoutedEventArgs e)
        {
            PageNumber = 1;
            ViewClients();
        }

        private void Delete_BTN_Click(object sender, RoutedEventArgs e)
        {
            ClientView selectedClient = Client_LV.SelectedItem as ClientView;
            if (selectedClient.VisitAmount > 0)
            {
                MessageBox.Show("Ошибка!", "Данный клиент не может быть удален, так как имеет записи о посещениях");
            }
            else
            {
                DataFrame.Context.Client.Remove(DataFrame.Context.Client.Find(selectedClient.Id));
                DataFrame.Context.SaveChanges();        
            }
        }

        private void Client_LV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Delete_BTN.Visibility = Client_LV.SelectedItems.Count == 1 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
