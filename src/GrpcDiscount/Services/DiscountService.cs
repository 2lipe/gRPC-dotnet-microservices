using System.Linq;
using System.Threading.Tasks;
using DiscountGrpc.Protos;
using Grpc.Core;
using GrpcDiscount.Data;
using Microsoft.Extensions.Logging;

namespace GrpcDiscount.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(ILogger<DiscountService> logger)
        {
            _logger = logger;
        }

        public override Task<DiscountModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var discount = DiscountContext.Discounts.FirstOrDefault(d => d.Code == request.DiscountCode);
            
            _logger.LogInformation(
                "Discount is operated with the {discountCode} code and " + "the amount is : {discountAmount}", discount.Code, discount.Amount);

            return Task.FromResult(new DiscountModel
            {
                DiscountId = discount.Id,
                Code = discount.Code,
                Amount = discount.Amount
            });
        }
    }
}