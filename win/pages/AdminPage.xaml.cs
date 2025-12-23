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

namespace LibraryApp.win.pages
{
    public partial class AdminPage : Page
    {
        public Dictionary<string,string> Databases = new Dictionary<string, string>() 
        { 
            { "Книги", "book" }, 
            { "Авторы", "author" }, 
            { "Читатели", "readers" },
            { "Долги", "loans" },
            { "Пользователи", "users" },
        };
        public AdminPage()
        {
            InitializeComponent();
            DBSelectList.ItemsSource = Databases;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }

        private void DBSelectionChanged(object sender, SelectionChangedEventArgs e)
        {




            UpdateTable();
        }

        private void UpdateTable()
        {
            DataTable.Items.Clear();
            DataTable.Columns.Clear();
            
            switch(DBSelectList.SelectedItem)
            {
                case "book":
                    
                    break;
            }
        }

        private void SearchFieldCreate()
        {

        }
    }
}
