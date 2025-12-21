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
    public partial class LibrarianSelectReader : Page
    {
        public bool ReturnMode;
        public LibrarianSelectReader()
        {
            InitializeComponent();
        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibrarianPage());
        }

        private void Reqister_User_Button(object sender, RoutedEventArgs e)
        {

        }

        private void Select_User_Button(object sender, RoutedEventArgs e)
        {

        }
    }
}
