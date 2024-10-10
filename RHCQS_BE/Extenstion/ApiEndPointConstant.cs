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

        public static class Auth
        {
            public const string RegisterEndpoint = ApiEndpoint + "/register";
            public const string LoginEndpoint = ApiEndpoint + "/login";
            public const string LogoutEndpoint = ApiEndpoint + "/logout";
            public const string DesTokenEndpoin = ApiEndpoint + "/des";
        }

        public static class Role
        {
            public const string RoleEndpoint = ApiEndpoint + "/role";
            public const string TotalRoleEndpoint = RoleEndpoint + "/total";
        }
        public static class PackageType
        {
            public const string PackageTypeEndpoint = ApiEndpoint + "/packagetype";
        }
        public static class Package
        {
            public const string PackageEndpoint = ApiEndpoint + "/package";
            public const string PackageListEndpoint = ApiEndpoint + "/allpackage";
            public const string PackageDetailEndpoint = PackageEndpoint + "/id";
            public const string PackageByNameEndpoint = PackageEndpoint + "/name";
        }

        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/account";
            public const string AccountByIdEndpoint = AccountEndpoint + "/id";
            public const string AccountByRoleIdEndpoint = AccountEndpoint + "/roleid";
            public const string ActiveAccountEndpoint = ApiEndpoint + "/account/active-account";
            public const string TotalAccountEndpoint = ApiEndpoint + "/account/total-account";
            public const string AccountProfileEndpoint = AccountEndpoint + "/profile";
            public const string SearchAccountEndpoint = AccountEndpoint + "/name";
            public const string UpdateDeflagEndpoint = AccountEndpoint + "/updatedeflag/id";

        }

        public static class AssignTask
        {
            public const string AssignTaskEndpoint = ApiEndpoint + "/task";
            public const string AssignTaskDetailEndpoint = AssignTaskEndpoint + "/id";
        }

        public static class HouseTemplate
        {
            public const string HouseTemplateEndpoint = ApiEndpoint + "/housetemplate";
            public const string SearchHouseTemplateEndpoint = HouseTemplateEndpoint + "/name";
            public const string HouseTemplateDetail = HouseTemplateEndpoint + "/id";
        }

        public static class Project
        {
            public const string ProjectEndpoint = ApiEndpoint + "/project";
            public const string ProjectSalesStaffEndpoint = ProjectEndpoint + "/sales";
            public const string ProjectDetailEndpoint = ProjectEndpoint + "/id";
            public const string ProjectByNumberPhone = ProjectEndpoint + "/phone";
            public const string ProjectListForCustomerEndpoint = ProjectEndpoint + "/email";
            public const string ProjectAssignEndpoint = ProjectEndpoint + "/assign";
        }

        public static class Construction
        {
            public const string ConstructionEndpoint = ApiEndpoint + "/construction";
            public const string ConstructionDetailEndpoint = ConstructionEndpoint + "/id";
            public const string COnstructionDetailByNameEndpoint = ConstructionEndpoint + "/name";
            public const string ConstructionRoughEndpoint = ConstructionEndpoint + "/type";
        }

        public static class Utility
        {
            public const string UtilityEndpoint = ApiEndpoint + "/utilities";
            public const string UtilityDetaidEndpoint = UtilityEndpoint + "/id";
            public const string UtilityByTypeEndpoint = UtilityEndpoint + "/type";
            public const string UtilitySectionEndpoint = UtilityEndpoint + "/section";
            public const string UtilitySectionDEndpoint = UtilitySectionEndpoint + "/id";
        }

        public static class HouseDesignDrawing
        {
            public const string HouseDesignDrawingEndpoint = ApiEndpoint + "/housedesign";
            public const string HouseDesignDrawingDetailEndpoint = HouseDesignDrawingEndpoint + "/id";
            public const string HouseDesignDrawingTypeEndpoint = HouseDesignDrawingEndpoint + "/type";
            public const string HouseDesignDrawingTask = HouseDesignDrawingEndpoint + "/task/id";
        }

        public static class HouseDesignVersion
        {
            public const string HouseDesignVersionEndpoint = ApiEndpoint + "/design";
            public const string HouseDesignVersionDetailEndpoint = HouseDesignVersionEndpoint + "/id";
            public const string HouseDesignVersionUploadFilesEndpoint = HouseDesignVersionEndpoint + "/upload-files";
            public const string AssignHouseDesignVersionEndpoint = HouseDesignVersionEndpoint + "/assign";
        }

        public static class InitialQuotation
        {
            public const string InitialQuotationEndpoint = ApiEndpoint + "/quotation" + "/initial";
            public const string InitialQuotationDetailEndpoint = InitialQuotationEndpoint + "/id";
            public const string InitialQuotationDetailByCustomerEndpoint = InitialQuotationEndpoint + "/customer/name";
            public const string AssignInitialQuotationEndpoint = InitialQuotationEndpoint + "/assign";
            public const string ApproveInitialEndpoint = InitialQuotationEndpoint + "/approve";
            public const string InitialQuotationUpdateEndpoibt = InitialQuotationEndpoint + "/update";
        }
        public static class FinalQuotation
        {
            public const string FinalQuotationEndpoint = ApiEndpoint + "/quotation" + "/final";
            public const string FinalQuotationDetailEndpoint = FinalQuotationEndpoint + "/id";
            public const string FinalQuotationDetailByCustomerEndpoint = FinalQuotationEndpoint + "/customer/name";
            public const string AssignFinalQuotationEndpoint = FinalQuotationEndpoint + "/assign";
            public const string ApproveFinalQuotationEndpoint = FinalQuotationEndpoint + "/approve";
            public const string FinalQuotationUpdateEndpoibt = FinalQuotationEndpoint + "/update";
        }
    }
}
