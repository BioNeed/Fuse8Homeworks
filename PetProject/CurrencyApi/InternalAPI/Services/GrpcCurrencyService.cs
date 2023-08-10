using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using InternalAPI.Contracts.GrpcContracts;

namespace InternalAPI.Services
{
    public class GrpcCurrencyService : GrpcCurrency.GrpcCurrencyBase
    {
        public override async Task<ExchangeRate> GetCurrentExchangeRate(CurrencyInfo request, ServerCallContext context)
        {
            return base.GetCurrentExchangeRate(request, context);
        }

        public override async Task<ExchangeRate> GetExchangeRateOnDate(CurrencyOnDateRequest request, ServerCallContext context)
        {
            return base.GetExchangeRateOnDate(request, context);
        }

        public override async Task<ApiSettings> GetApiSettings(Empty request, ServerCallContext context)
        {
            return base.GetApiSettings(request, context);
        }
    }
}
