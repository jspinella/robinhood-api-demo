
namespace Stocksby.Models.Robinhood
{
    public class Eps
    {
        public string estimate { get; set; }
        public string actual { get; set; }
    }

    public class Report
    {
        public string date { get; set; }
        public string timing { get; set; }
        public bool verified { get; set; }
    }

    public class Call
    {
        public DateTime datetime { get; set; }
        public string broadcast_url { get; set; }
        public string replay_url { get; set; }
    }

    public class Result
    {
        public string symbol { get; set; }
        public string instrument { get; set; }
        public int year { get; set; }
        public int quarter { get; set; }
        public Eps eps { get; set; }
        public Report report { get; set; }
        public Call call { get; set; }
    }

    public class EarningsResponse
    {
        public List<Result> results { get; set; }
    }
}
