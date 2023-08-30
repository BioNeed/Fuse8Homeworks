using System.ComponentModel.DataAnnotations;

namespace CurrenciesDataAccessLibrary.Models
{
    public class CacheTaskInfo
    {
        public Guid Id { get; set; }

        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string NewBaseCurrency { get; set; }
    }
}
