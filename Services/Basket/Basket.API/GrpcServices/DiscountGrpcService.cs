using Discount.Grpc.Protos;

namespace Basket.API.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService ?? throw new ArgumentNullException(nameof(discountProtoService));
        }

        public DiscountProtoService.DiscountProtoServiceClient Get_discountProtoService()
        {
            return _discountProtoService;
        }

        public async Task<CouponModel> GetDiscount(string productname)
        {
            var discountRequest = new GetDiscountRequest { ProductName=productname };
            var response = await _discountProtoService.GetDiscountAsync(discountRequest);
            return response;
        }
    }
}
