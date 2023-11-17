using Microsoft.AspNetCore.Mvc;

namespace Controllers.Models
{
    public class Book
    {
        //[FromQuery]
        public int? BookId { get; set; }
        public string? Author { get; set; }

        public override string ToString()
        {
            return $"Book Object - Book Id : {BookId}, Author : {Author}";
        }
    }
}
