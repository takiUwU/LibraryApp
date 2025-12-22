using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public LibrarianSelectReader(string phone = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(phone))
                PhoneTextBox.Text = phone;
        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibrarianPage());
            
        }

        private void Select_User_Button(object sender, RoutedEventArgs e)
        {
            try
            {

                Reader? reader = LibraryCore.FindReaderByPhone(PhoneTextBox.Text);
                if (reader == null)
                    throw new Exception("Пользователя с таким телефоном нету!");
                


                if (ReturnMode)
                    NavigationService.Navigate(new LibrarianSelectLoan(reader)); 
                else
                {
                    if (LibraryCore.CanReaderLoanABook(reader) == false)
                        throw new Exception("У данного пользователя плохая репутация!");
                    NavigationService.Navigate(new LibrarianSelectBook(reader));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
