using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace LibraryApp.win.user_page
{
    public partial class UserPage : Page
    {
        Reader? current_user = null;

        
        public UserPage()
        {
            InitializeComponent();
        }

        public UserPage(Reader reader)
        {
            InitializeComponent();
            current_user = reader;
            UserNameLabel.Content = reader.Name;
            BooksGrid.ItemsSource = CreateTable().DefaultView;
        }

        private List<LoansColumn> Loans()
        {
            List<LoansColumn> LoansColumns = new List<LoansColumn>();
            LoansColumns.Clear();
            foreach (var loan in current_user.Loans)
                LoansColumns.Add(new LoansColumn(loan));
            return LoansColumns;
        }

        private DataTable CreateTable()
        {
            DataTable data = new DataTable();
            data.Columns.Add("Название книги", typeof(string));
            data.Columns.Add("Автор", typeof(string));
            data.Columns.Add("Дата забора", typeof(DateTime));
            data.Columns.Add("Дата возврата", typeof(DateTime));
            foreach (var item in current_user!.Loans)
                data.Rows.Add(item);
            return data;
        }

        private class LoansColumn
        {
            public string bookName = "";
            public string bookAuthor = "";
            public DateTime borrowDate = DateTime.MinValue;
            public DateTime? returnDate;

            public LoansColumn(Loan loan)
            {
                Book? temp_book = MainFrame.MainFrame.Core!.GetBookByID(loan.BookID);
                if (temp_book != null)
                {
                    bookName = temp_book.Name;
                    bookAuthor = temp_book.Author.Name;
                    borrowDate = loan.BorrowDate;
                    returnDate = loan.ReturnDate;
                }
            }
        }
    }
}
