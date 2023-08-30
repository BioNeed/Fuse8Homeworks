using CurrenciesDataAccessLibrary.Enums;

namespace CurrenciesDataAccessLibrary.Models
{
    public class CacheTask
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public CacheTaskStatus Status { get; set; }

        public CacheTaskInfo? TaskInfo { get; set; }
    }
}
