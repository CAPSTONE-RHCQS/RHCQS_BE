using System.CodeDom;

namespace RHCQS_BE.Extenstion
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Login
        {
            public const string LoginEndpoint = ApiEndpoint + "/Login";
        }

        public static class Logout
        {
            public const string LogoutEndpoint = ApiEndpoint + "/Logout";
        }

        public static class Warranty
        {
            public const string WarrantyEndpoint = ApiEndpoint + "/warranty";
            public const string WarrantyEndpointNo = ApiEndpoint + "/warrantyno";
            public const string WarrantyByIdEndpoint = WarrantyEndpoint + "/{id}";
            public const string WarrantyConditionEndpoint = WarrantyEndpoint + "/{id}/conditions";
            public const string WarrantySearch = WarrantyEndpoint + "/search";
        }

        public static class PurchasePrice
        {
            // Do Huu Thuan
            public const string PurchasePriceEndpoint = ApiEndpoint + "/PurchasePrice";
            public const string PurchasePriceByIdEndpoint = PurchasePriceEndpoint + "/id";

        }
        public static class ProcessPrice
        {
            // Do Huu Thuan
            public const string ProcessPriceEndpoint = ApiEndpoint + "/ProcessPrice";
            public const string ProcessPriceByIdEndpoint = ProcessPriceEndpoint + "/id";

        }
        public static class Stall
        {
            // Do Huu Thuan
            public const string StallEndpoint = ApiEndpoint + "/Stall";
            public const string StallByIdEndpoint = StallEndpoint + "/{id}";
        }
        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/Account";
            // Do Huu Thuan
            public const string AccountByRoleIdEndpoint = ApiEndpoint + "/AccountByRole";
            public const string TotalAccountEndpoint = ApiEndpoint + "/Account/Total-account";
            public const string ActiveAccountEndpoint = ApiEndpoint + "/Account/Active-account";

            public const string AccountByIdEndpoint = AccountEndpoint + "/id";
            public const string AccountProfileEndpoint = AccountEndpoint + "/Profile";
            public const string AccountProfileUpdateEndpoint = AccountProfileEndpoint + "/Update";
            public const string SearchAccountEndpoint = AccountEndpoint + "/name";
            public const string SearchMemberEndpoint = ApiEndpoint + "/Account/search/member";
            //public const string AccountsWithDeflagFalseEndpoint = AccountEndpoint + "/DeflagFalse";
            public const string FilteredAccountsEndpoint = AccountEndpoint + "/Filtered";
            public const string UpdateDeflagEndpoint = AccountEndpoint + "/UpdateDeflag/{id}";
            public const string CheckPhoneEndpoint = AccountEndpoint + "/CheckPhone/{phone}";
        }

        public static class Role
        {
            public const string RoleEndpoint = ApiEndpoint + "/Role";
            public const string RoleByIdEndpoint = RoleEndpoint + "/id";
        }

        public static class Category
        {
            public const string CategoryEndpoint = ApiEndpoint + "/Category";
            public const string CategoryByIdEndpoint = CategoryEndpoint + "/id";
            public const string CategoryByNameEndpoint = CategoryEndpoint + "/categoryName";
        }


        public static class SubProduct
        {
            // Do Huu Thuan
            public const string SubProductsEndpoint = ApiEndpoint + "/SubProducts";
        }

        public static class Product
        {
            public const string ProductEndpoint = ApiEndpoint + "/Product";
            public const string AllProductEndpoint = ApiEndpoint + "/Product/All";
            public const string ProductByIdEndpoint = ProductEndpoint + "/id";
            public const string ProductByCategoryEndpoint = ProductEndpoint + "/category";
            public const string ProductByCodeEndpoint = ProductEndpoint + "/code";
            public const string ProductByNameEndpoint = ProductEndpoint + "/productName";
            public const string ProductByCodePromotionEndpoint = ProductByCodeEndpoint + "/promotion";
            public const string ProductItemByCodePromotionEndpoint = ProductByCodeEndpoint + "/promotion/item";
            public const string GetTotalProductCountEndpoint = ProductEndpoint + "/totalcount";
            public const string GetProductCountByCategoryEndpoint = ProductEndpoint + "/countbycategory";

            public const string FilterProductEndpoint = ProductEndpoint + "/filter";
            public const string ProductAutocompleteEndpoint = ProductEndpoint + "/autocomplete";

            // Do Huu Thuan
            public const string ProductBySubIdEndpoint = ProductEndpoint + "/subid";
            public const string TotalPurchasePriceEndpoint = ApiEndpoint + "/Product/PurchasePrice";
        }

        public static class Promotion
        {
            public const string PromotionEndpoint = ApiEndpoint + "/Promotion";
            public const string PromotionByIdEndpoint = PromotionEndpoint + "/id";
            public const string PromotionGroupEndpoint = PromotionEndpoint + "/group";

        }
        public static class Dashboard
        {
            public const string DashboardEndpoint = ApiEndpoint + "/Dashboard";
            public const string AccountDashboardEndpoint = DashboardEndpoint + "/account";
            public const string MemberDashboardEndpoint = DashboardEndpoint + "/member";
            public const string CategoryDashboardEndpoint = DashboardEndpoint + "/category";
            public const string OrderDashboardEndpoint = DashboardEndpoint + "/order";
        }
        public static class Transaction
        {
            public const string TransactionEndpoint = ApiEndpoint + "/Transaction";
            public const string TransactionOrderEndpoint = TransactionEndpoint + "/orderId";
        }

        public static class Material
        {
            public const string MaterialEndpoint = ApiEndpoint + "/Material";
            public const string MaterialByIdEndpoint = MaterialEndpoint + "/id";
            public const string MaterialByNameEndpoint = MaterialEndpoint + "/materialName";
        }
        public static class Diamond
        {
            public const string DiamondEndpoint = ApiEndpoint + "/Diamond";
            public const string DiamondByDiamondIdEndpoint = DiamondEndpoint + "/id";
            public const string DiamondByCodeEndpoint = DiamondEndpoint + "/code";
            public const string DiamondByNameEndpoint = DiamondEndpoint + "/diamondName";
            public const string DiamondAutocompleteEndpoint = DiamondEndpoint + "/autocomplete";
            public const string GetTotalDiamondCountEndpoint = DiamondEndpoint + "/totalcount";
        }
        public static class Membership
        {
            public const string MembershipEndpoint = ApiEndpoint + "/membership";
            public const string MembershipByIdEndpoint = MembershipEndpoint + "/{id}";
            public const string MembershipByUserIdEndpoint = MembershipEndpoint + "/userId";
            public const string MembershipExpired = MembershipEndpoint + "/expired";
            public const string MembershipByName = MembershipEndpoint + "/{name}";
            public const string MembershipUserMoney = MembershipEndpoint + "/userMoney";
            public const string MembershipTotal = MembershipEndpoint + "/total";
            public const string MembershipActive = MembershipEndpoint + "/active";
            public const string MembershipUnActive = MembershipEndpoint + "/unavailable";
            public const string MembershipOrder = MembershipEndpoint + "/phone";
        }

        public static class Order
        {
            public const string OrderEndpoint = ApiEndpoint + "/order";
            public const string OrderEndpointTest = ApiEndpoint + "/orderTest";
            public const string OrderUpdateEndpoint = OrderEndpoint + "/id";
            public const string SearchOrderEndpoint = AllOrdersEndpoint + "/search";
            public const string OrderByIdEndpoint = OrderEndpoint + "/id";
            public const string OrderCheckPromotionEndpoint = ApiEndpoint + "/check";
            public const string OrderStatic = ApiEndpoint + "/static";
            public const string OrderStaticMonth = ApiEndpoint + "/static/month";
            public const string OrderStaticYear = ApiEndpoint + "/static/year";
            public const string AllOrdersEndpoint = ApiEndpoint + "/order/GetListOrders"; 
            public const string OrderEndpointList = ApiEndpoint + "/orderlist";
            public const string OrderListCusomerPhone = OrderEndpoint + "/customer";
            public const string OrderListDetail = OrderEndpoint + "/detail";
            public const string OrderDiscount = OrderEndpoint + "/option";
            public const string OrderUpdate = OrderEndpoint + "/update";
        }
        //  Do Huu Thuan
        public static class OrderDetail
        {
            public const string OrderDetailEndpoint = ApiEndpoint + "/OrderDetail";
            public const string OrderDetailByIdEndpoint = OrderDetailEndpoint + "/id";
            public const string OrderDetailByOrderIdEndpoint = OrderDetailEndpoint + "/OrderID/id";

        }

        public static class Discount
        {
            public const string DiscountConfirmEndpoint = ApiEndpoint + "/discount/confirm";
            public const string DiscountEndpoint = ApiEndpoint + "/discount";
            public const string DiscountByIdEndpoint = DiscountEndpoint + "/id";
            public const string DiscountAccepted = DiscountEndpoint + "/accepted";
        }

        public static class ConditionWarranty
        {
            public const string ConditionWarrantyEndpoint = ApiEndpoint + "/condition";
            public const string ConditionWarrantyByIdEndpoint = ConditionWarrantyEndpoint + "/{id}";
        }

        public static class GoldRate
        {
            public const string GoldRateEndpoint = ApiEndpoint + "/GoldRate";
        }
        public static class SilverRate
        {
            public const string SilverRateEndpoint = ApiEndpoint + "/SilverRate";
        }
    }
}
