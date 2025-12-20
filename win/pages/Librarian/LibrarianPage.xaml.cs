using LibraryApp.win.pages.librarian;
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
    public partial class LibrarianPage : Page
    {
        public LibrarianPage()
        {
            InitializeComponent();
        }

        private void Return_Book_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Give_Book_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Reqister_User_Button(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibrarianCreateNewReader());
        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
