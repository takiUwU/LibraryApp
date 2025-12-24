using System.ComponentModel;

namespace LibraryApp.code
{
    public class BookRowsecond : INotifyPropertyChanged
    {
        int? id;
        string? name;
        string? author;
        string? description;
        DateOnly? releaseDate;
        int? pageCount;
        int? count;

        public BookRowsecond(int? id, string? Name, string? Author, string? Description, DateOnly? ReleaseDate, int? PageCount, int? Count)
        {
            this.id = id;
            this.Name = Name;
            this.Author = Author;
            this.Description = Description;
            this.ReleaseDate = ReleaseDate;
            this.PageCount = PageCount;
            this.Count = Count;
        }

        public int? Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(id)); }
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

        public int? Count
        {
            get => count;
            set { count = value; OnPropertyChanged(nameof(count)); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public class AuthorRow : INotifyPropertyChanged
    {
        int id;
        string? name;

        public AuthorRow(int id, string? Name)
        {
            this.id = id;
            this.Name = Name;
        }

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(id)); }
        }

        public string? Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(name)); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public class UserRow : INotifyPropertyChanged
    {
        int id;
        string? name;
        string? role;

        public UserRow(int id, string? Name, string Role)
        {
            this.id = id;
            this.Name = Name;
            this.Role = Role;
        }

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(id)); }
        }

        public string? Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(name)); }
        }

        public string? Role
        {
            get => role;
            set { role = value; OnPropertyChanged(nameof(role)); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public class ReaderRow : INotifyPropertyChanged
    {
        int id;
        string? name;
        string? phone;
        int loan_count;
        int expired_loan_count;
        int returned_loan_count;

        public ReaderRow(int id, string? Name, string? Phone)
        {
            this.id = id;
            this.Name = Name;
            this.Phone = Phone;
            if (Phone == null)
                throw new Exception("Отсутсвует номер телефона!");
            Reader? reader = LibraryCore.FindReaderByPhone(Phone);
            if (reader == null)
                throw new Exception("Читатель не был найден!");
            ICollection<Loan>? loans = LibraryCore.GetLoansByReader(reader);
            ICollection<Loan>? returned_loans = loans.Where(l => l.ReturnDate != null).ToList();
            ICollection<Loan>? expired_loans = loans.Where(l => (DateTime.Now - l.BorrowDate).Days > 14).ToList();
            
            if (loans == null)
            {
                loan_count = 0;
                expired_loan_count = 0;
                returned_loan_count = 0;
            }
            else
            {
                loan_count = loans.Count;
                expired_loan_count = expired_loans.Count;
                returned_loan_count = returned_loans.Count;
            }
        }

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(id)); }
        }

        public string? Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(name)); }
        }


        public string? Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(nameof(phone)); }
        }

        public int Loan_Count
        {
            get => loan_count;
            set { loan_count = value; OnPropertyChanged(nameof(loan_count)); }
        }

        public int Expired_Loan_Count
        {
            get => expired_loan_count;
            set { expired_loan_count = value; OnPropertyChanged(nameof(Expired_Loan_Count)); }
        }

        public int Returned_Loan_Count
        {
            get => returned_loan_count;
            set { returned_loan_count = value; OnPropertyChanged(nameof(returned_loan_count)); }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }


    public class LoanRow : INotifyPropertyChanged
    {
        int original_book_id;
        int original_reader_id;
        int id;
        string? reader;
        string? book;
        DateTime? burrowTime;
        DateTime? returnTime;



        public LoanRow(Loan loan, Reader reader, Book book)
        {
            original_book_id = book.ID;
            original_reader_id = reader.ID;
            Id = loan.ID;
            Reader = reader.Name + " (" + reader.Phone + ")";
            Book = book.Name + " (" + book.Author.Name + ")";
            BurrowTime = loan.BorrowDate;
            ReturnTime = loan.ReturnDate;
        }

        public int Id
        {
            get => id;
            set { id = value; OnPropertyChanged(nameof(id)); }
        }

        public string? Reader
        {
            get => reader;
            set { reader = value; OnPropertyChanged(nameof(reader)); }
        }

        public string? Book
        {
            get => book;
            set { book = value; OnPropertyChanged(nameof(book)); }
        }

        public DateTime? BurrowTime
        {
            get => burrowTime;
            set { burrowTime = value; OnPropertyChanged(nameof(burrowTime)); }
        }

        public DateTime? ReturnTime
        {
            get => returnTime;
            set { returnTime = value; OnPropertyChanged(nameof(returnTime)); }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}