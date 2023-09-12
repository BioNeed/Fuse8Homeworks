using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CurrenciesDataAccessLibrary.Enums;

namespace CurrenciesDataAccessLibrary.Models
{
    /// <summary>
    /// Задача кэша
    /// </summary>
    public class CacheTask
    {
        /// <summary>
        /// Уникальный Id задачи
        /// </summary>
        [ForeignKey("TaskInfo")]
        public Guid Id { get; set; }

        /// <summary>
        /// Время создания задачи
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Статус задачи
        /// </summary>
        public CacheTaskStatus Status { get; set; }

        /// <summary>
        /// Детали задачи (например, базовая валюта для пересчета кеша)
        /// </summary>
        public CacheTaskInfo? TaskInfo { get; set; }
    }
}
