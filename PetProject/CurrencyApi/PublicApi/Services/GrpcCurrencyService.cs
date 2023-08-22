﻿using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Contracts.GrpcContracts;
using Fuse8_ByteMinds.SummerSchool.PublicApi.Models;
using Google.Protobuf.WellKnownTypes;

namespace Fuse8_ByteMinds.SummerSchool.PublicApi.Services
{
    public class GrpcCurrencyService : IGrpcCurrencyService
    {
        private readonly GrpcCurrency.GrpcCurrencyClient _grpcCurrencyClient;
        private readonly ISettingsService _settingsService;
        private readonly IFavouritesService _favouritesService;

        public GrpcCurrencyService(GrpcCurrency.GrpcCurrencyClient grpcCurrencyClient, ISettingsService settingsService, IFavouritesService favouritesService)
        {
            _grpcCurrencyClient = grpcCurrencyClient;
            _settingsService = settingsService;
            _favouritesService = favouritesService;
        }

        public async Task<ExchangeRateModel> GetExchangeRateAsync(string currencyCode, CancellationToken cancellationToken)
        {
            CurrencyInfo currencyInfo = new CurrencyInfo
            {
                CurrencyCode = currencyCode,
            };

            ExchangeRate exchangeRate = await _grpcCurrencyClient.GetCurrentExchangeRateAsync(
                currencyInfo,
                cancellationToken: cancellationToken);

            Settings settings = await _settingsService.GetApplicationSettingsAsync(cancellationToken);
            decimal roundedValue = decimal.Round(exchangeRate.Value, settings.CurrencyRoundCount);
            return new ExchangeRateModel
            {
                Code = exchangeRate.Code,
                Value = roundedValue,
            };
        }

        public async Task<ExchangeRateModel> GetExchangeRateOnDateTimeAsync(
            string currencyCode,
            DateTime dateTime,
            CancellationToken cancellationToken)
        {
            CurrencyOnDateRequest request = new CurrencyOnDateRequest
            {
                CurrencyCode = currencyCode,
                Date = Timestamp.FromDateTime(dateTime),
            };

            ExchangeRate exchangeRate = await _grpcCurrencyClient
                .GetExchangeRateOnDateAsync(request,
                                            cancellationToken: cancellationToken);

            Settings settings = await _settingsService.GetApplicationSettingsAsync(cancellationToken);
            decimal roundedValue = decimal.Round(exchangeRate.Value, settings.CurrencyRoundCount);
            return new ExchangeRateModel
            {
                Code = exchangeRate.Code,
                Value = roundedValue,
            };
        }

        public async Task<CurrencyConfigurationModel> GetSettingsAsync(CancellationToken cancellationToken)
        {
            ApiInfo apiInfo = await _grpcCurrencyClient.GetApiInfoAsync(
                new Empty(),
                cancellationToken: cancellationToken);

            Settings settings = await _settingsService.GetApplicationSettingsAsync(cancellationToken);

            return new CurrencyConfigurationModel
            {
                DefaultCurrency = settings.DefaultCurrency.ToString(),
                BaseCurrency = apiInfo.BaseCurrency,
                NewRequestsAvailable = apiInfo.IsRequestAvailable,
                CurrencyRoundCount = settings.CurrencyRoundCount,
            };
        }

        public async Task<ExchangeRateWithBaseModel?> GetFavouriteExchangeRateAsync(
            string favouriteName,
            CancellationToken cancellationToken)
        {
            FavouriteExchangeRate? favourite = await _favouritesService
                .GetFavouriteByNameAsync(favouriteName,
                                         cancellationToken);

            if (favourite == null)
            {
                return null;
            }

            FavouriteInfo favouriteInfo = new FavouriteInfo
            {
                BaseCurrency = favourite.BaseCurrency,
                Currency = favourite.Currency,
            };

            ExchangeRateWithBase exchangeRateWithBase = await _grpcCurrencyClient
                .GetCurrentFavouriteExchangeRateAsync(favouriteInfo,
                                                      cancellationToken: cancellationToken);

            Settings settings = await _settingsService.GetApplicationSettingsAsync(cancellationToken);
            decimal roundedValue = decimal.Round(exchangeRateWithBase.Value, settings.CurrencyRoundCount);

            return new ExchangeRateWithBaseModel
            {
                BaseCurrency = exchangeRateWithBase.BaseCurrency,
                Currency = exchangeRateWithBase.Currency,
                Value = roundedValue,
            };
        }
    }
}
