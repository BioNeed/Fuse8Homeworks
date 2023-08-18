using InternalAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InternalAPI.DataAccess.Configurations
{
    public class ExchangeRateConfiguration //: IEntityTypeConfiguration<ExchangeRateModel>
    {
        public void Configure(EntityTypeBuilder<ExchangeRateModel> builder)
        {
            //throw new NotImplementedException();
        }
    }
}
