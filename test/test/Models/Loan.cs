using System;
using System.Collections.Generic;
using System.Text;

namespace test.Models
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int ReaderId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; } // подумать над этой переменной
        public DateTime? ReturnDate { get; set; }
    }
}
