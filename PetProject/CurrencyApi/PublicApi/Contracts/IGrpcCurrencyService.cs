﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts.GrpcContracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts
{
    public interface IGrpcCurrencyService
    {
        Task<ExchangeRateModel> GetExchangeRateAsync(string currencyCode, CancellationToken cancellationToken);

        Task<ExchangeRateModel> GetExchangeRateOnDateTimeAsync(string currencyCode, DateTime dateTime, CancellationToken cancellationToken);

        Task<ExchangeRateWithBaseModel?> GetFavouriteExchangeRateAsync(string favouriteName, CancellationToken cancellationToken);

        Task<ExchangeRateWithBaseModel?> GetFavouriteExchangeRateOnDateAsync(string favouriteName, DateTime dateTime, CancellationToken cancellationToken);

        Task<CurrencyConfigurationModel> GetSettingsAsync(CancellationToken cancellationToken);
    }
}