using System.ComponentModel.DataAnnotations;

namespace CurrenciesDataAccessLibrary.Models
{
    public class CacheSettings
    {
        [StringLength(3, MinimumLength = 3)]
        public string BaseCurrency { get; set; }
    }
}
