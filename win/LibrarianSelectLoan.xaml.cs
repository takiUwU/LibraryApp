using LibraryApp.win.pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace LibraryApp.win
{
    public partial class LibrarianSelectLoan : Page
    {
        Reader reader;
        public LibrarianSelectLoan(Reader reader)
        {
            InitializeComponent();
            this.reader = reader;
            TableLoad();
            ReaderBoxLabel.Content = $"Телефон: {reader.Phone}\nИмя: {reader.Name}";
        }

        private void Exit_MouseClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LibrarianSelectReader() { ReturnMode = true });
        }


        private void TableLoad()
        {
            BooksTable.ItemsSource = CreateBookRowTable();
        }

        private List<BookRow> CreateBookRowTable(string name, string author, DateOnly? date)
        {
            List<BookRow> books = CreateBookRowTable();
            books = books.Where(b => b.Name!.Contains(name) && b.Author!.Contains(author) && (date == null || b.ReleaseDate == date)).ToList();
            return books;
        }

        private List<BookRow> CreateBookRowTable()
        {
            List<Loan> loans = LibraryCore.GetUnreturnedLoansByReader(reader).ToList();
            List<BookRow> bookRows = new List<BookRow>();
            foreach (var loan in loans)
                bookRows.Add(new BookRow(loan.ID, loan.Book.Name, loan.Book.Author.Name, loan.Book.Description, loan.Book.ReleaseDate, loan.Book.PageCount, loan.BorrowDate));
            return bookRows;
        }


        private void SearchTextChanged(object sender, TextChangedEventArgs e)
        {
            DateTime datetime;
            DateOnly? date = null;
            if (DateTime.TryParse(DateTextBox.Text, out datetime))
            {
                date = DateOnly.FromDateTime(datetime);

            }
            BooksTable.ItemsSource = CreateBookRowTable(NameTextBox.Text, AuthorTextBox.Text, date);

        }

        private void Give_Book_Button(object sender, RoutedEventArgs e)
        {
            if (BooksTable.SelectedItem == null)
                return;
            BookGive((BookRow)BooksTable.SelectedItem);
        }


        private void BookGive(BookRow bookRow)
        {
            try
            {

                MessageBoxResult result = MessageBox.Show($"Действительно ли вы хотите вернуть книгу {bookRow.Name} от человека с телефоном {reader.Phone}?", "Вернуть книгу?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Loan? return_loan = LibraryCore.GetLoanByID(bookRow.original_id);
                    if (return_loan == null)
                        throw new Exception("Ошибка при возрате книги.");
                    LibraryCore.ReturnALoan(return_loan);
                    MessageBox.Show("Книга была успешна возращена!");
                    NavigationService.Navigate(new LibrarianPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private class BookRow : INotifyPropertyChanged
        {
            public int original_id = 0;
            string? name;
            string? author;
            string? description;
            DateOnly? releaseDate;
            int? pageCount;
            DateTime? borrowTime;
            

            public BookRow(int original_id, string? Name, string? Author, string? Description, DateOnly? ReleaseDate, int? PageCount, DateTime? BorrowTime)
            {
                this.original_id = original_id;
                this.Name = Name;
                this.Author = Author;
                this.Description = Description;
                this.ReleaseDate = ReleaseDate;
                this.PageCount = PageCount;
                this.BorrowTime = BorrowTime;
            }

            public string? Name
            {
                get => name;
                set { name = value; OnPropertyChanged(nameof(name)); }
            }


            public string? Author
            {
                get => author;
                set { author = value; OnPropertyChanged(nameof(author)); }
            }

            public string? Description
            {
                get => description;
                set { description = value; OnPropertyChanged(nameof(description)); }
            }

            public DateOnly? ReleaseDate
            {
                get => releaseDate;
                set { releaseDate = value; OnPropertyChanged(nameof(releaseDate)); }
            }

            public int? PageCount
            {
                get => pageCount;
                set { pageCount = value; OnPropertyChanged(nameof(pageCount)); }
            }

            public DateTime? BorrowTime
            {
                get => borrowTime;
                set { borrowTime = value; OnPropertyChanged(nameof(borrowTime)); }
            }


            public event PropertyChangedEventHandler? PropertyChanged;

            private void OnPropertyChanged(string name)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
