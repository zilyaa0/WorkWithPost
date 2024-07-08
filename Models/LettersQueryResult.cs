using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkWithPost.Models
{
    public class LettersQueryResult
    {
        public List<Letter> Letters { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }
    }
    public class Letter
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Headers { get; set; }
        public string Text { get; set; }
        public string MessageId { get; set; }
    }


}
