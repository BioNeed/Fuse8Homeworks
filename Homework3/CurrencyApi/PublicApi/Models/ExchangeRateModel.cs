﻿namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Models
{
    /// <summary>
    /// Курс валюты относительно базовой валюты
    /// </summary>
    public record ExchangeRateModel
    {
        /// <summary>
        /// Код валюты, в которой нужно узнать курс
        /// </summary>
        public string Code { get; init; }

        /// <summary>
        /// Курс валюты относительно базовой валюты
        /// </summary>
        public decimal Value { get; init; }
    }
}