using System.ComponentModel.DataAnnotations;

namespace CurrenciesDataAccessLibrary.Models
{
    /// <summary>
    /// Детали задачи кэша
    /// </summary>
    public class CacheTaskInfo
    {
        /// <summary>
        /// Уникальный Id задачи, к которой относятся детали
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Базовая валюта для пересчета кэша
        /// </summary>
        [StringLength(maximumLength: 3, MinimumLength = 3)]
        public string NewBaseCurrency { get; set; }

        /// <summary>
        /// Задача кэша, к которой относятся эти детали
        /// </summary>
        public CacheTask? Task { get; set; }
    }
}
