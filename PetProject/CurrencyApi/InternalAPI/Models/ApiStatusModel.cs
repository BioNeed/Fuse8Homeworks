namespace InternalAPI.Models
{
    /// <summary>
    /// Статус API
    /// </summary>
    public class ApiStatusModel
    {
        /// <summary>
        /// Полное количество запросов
        /// </summary>
        public int TotalRequests { get; set; }

        /// <summary>
        /// Количество использованных запросов
        /// </summary>
        public int UsedRequests { get; set; }
    }
}
