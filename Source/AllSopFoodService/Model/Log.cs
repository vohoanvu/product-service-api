namespace AllSopFoodService.Model
{
    using System;

    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } = default!;
        public string MessageTemplate { get; set; } = default!;
        public string Level { get; set; } = default!;

        public DateTime TimeStamp { get; set; }
        public string Exception { get; set; } = default!;
        public string Properties { get; set; } = default!; // XML properties
        public string LogEvent { get; set; } = default!;
    }
}
