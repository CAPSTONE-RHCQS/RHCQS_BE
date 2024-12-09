using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Protocols;
using RHCQS_BusinessObject.Payload.Request.Contract;
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
        public const string Test = ApiEndpoint + "/test";

        public static class Auth
        {
            public const string RegisterEndpoint = ApiEndpoint + "/register";
            public const string LoginEndpoint = ApiEndpoint + "/login";
            public const string LogoutEndpoint = ApiEndpoint + "/logout";
            public const string DesTokenEndpoin = ApiEndpoint + "/des";
            public const string RefreshTokenEndpoint = ApiEndpoint + "/refresh-token";
            
        }

        public static class Role
        {
            public const string RoleEndpoint = ApiEndpoint + "/role";
            public const string TotalRoleEndpoint = RoleEndpoint + "/total";
        }
        public static class Notification
        {
            public const string NotificationEndpoint = ApiEndpoint + "/notification";

            public const string SendNotificationRoleEndpoint = NotificationEndpoint + "/send";

            public const string GetNotificationsEndpoint = NotificationEndpoint + "/get-notifications/email";

            public const string SaveDeviceTokenEndpoint = NotificationEndpoint + "/save-token";

            public const string GetDeviceTokenEndpoint = NotificationEndpoint + "/get-token/email";
        }
        public static class PackageType
        {
            public const string PackageTypeEndpoint = ApiEndpoint + "/packagetype";
        }
        public static class Package
        {
            public const string PackageEndpoint = ApiEndpoint + "/package";
            public const string PackageListEndpoint = ApiEndpoint + "/allpackage";
            public const string PackageDetailPDFEndpoint = PackageEndpoint + "/generate-pdf";
            public const string PackageDetailEndpoint = PackageEndpoint + "/id";
            public const string PackageByNameEndpoint = PackageEndpoint + "/name";
            public const string PackageAutoCharacter = PackageEndpoint + "/contain/name";
        }
        public static class Blog
        {
            public const string BlogEndpoint = ApiEndpoint + "/blogs";
            public const string BlogListEndpoint = ApiEndpoint + "/listblog";
            public const string BlogDetailEndpoint = BlogEndpoint + "/id";
            public const string BlogByAccountEndpoint = BlogEndpoint + "/accountid";
        }
        public static class Account
        {
            public const string AccountEndpoint = ApiEndpoint + "/account";
            public const string ProfileEndpoint = AccountEndpoint + "/profile";
            public const string CreateStaffEndpoint = AccountEndpoint + "/create-staff";
            public const string UpdatePasswordEndpoint = AccountEndpoint + "/password";
            public const string AccountByIdEndpoint = AccountEndpoint + "/id";
            public const string AccountByRoleIdEndpoint = AccountEndpoint + "/roleid";
            public const string ActiveAccountEndpoint = ApiEndpoint + "/account/active-account";
            public const string TotalAccountEndpoint = ApiEndpoint + "/account/total-account";
            public const string AccountProfileEndpoint = AccountEndpoint + "/profile";
            public const string SearchAccountEndpoint = AccountEndpoint + "/name";
            public const string UpdateDeflagEndpoint = AccountEndpoint + "/updatedeflag/id";
            public const string UploadImageProfileEndpoint = ApiEndpoint + "/upload-profile-images";
            public const string TotalSStaffAccountEndpoint = ApiEndpoint + "/account/total-s-staff-account";
            public const string TotalDStaffAccountEndpoint = ApiEndpoint + "/account/total-d-staff-account";
            public const string TotalCustomerAccountEndpoint = ApiEndpoint + "/account/total-customer-account";
            public const string TotalCustomerTodayAccountEndpoint = ApiEndpoint + "/account/total-cus-today-account";

        }
        public static class EquiqmentExcel
        {
            public const string EquimentExcelEndpoint = ApiEndpoint + "/equiqment/excel";
            public const string WorkTemplateExcelEndpoint = ApiEndpoint + "/worktemplate/excel";
        }

        public static class AssignTask
        {
            public const string AssignTaskEndpoint = ApiEndpoint + "/task";
            public const string AssignTaskDetailEndpoint = AssignTaskEndpoint + "/id";
            public const string AssignTaskDesignStaffAvailableEndpoint = AssignTaskEndpoint + "/design-staff/available";
            public const string AssignTaskSaleStaffAvailableEndpoint = AssignTaskEndpoint + "/sale-staff/available";
        }

        public static class HouseTemplate
        {
            public const string HouseTemplateEndpoint = ApiEndpoint + "/housetemplate";
            public const string HouseTemplateShorterEndpoint = ApiEndpoint + "/house-templates-list";
            public const string HouseTemplateListEndpoint = ApiEndpoint + "/allhousetemplate";
            public const string SearchHouseTemplateEndpoint = HouseTemplateEndpoint + "/name";
            public const string HouseTemplateDetail = HouseTemplateEndpoint + "/id";
            public const string SubTemplateDesignEndpoint = ApiEndpoint + "/sub-template";
            public const string SubTemplateDesignDetailEndpoint = SubTemplateDesignEndpoint + "/id";
            public const string UploadImageDrawingEndpoint = ApiEndpoint + "/upload-design-images";
            public const string PackageHouseEndpoint = ApiEndpoint + "/package-house";
            public const string UploadImagePackHouseEndpoint = ApiEndpoint + "/upload-package-house";
            public const string UploadImageSubTemplateEndpoint = ApiEndpoint + "/upload-sub-template";
            public const string UploadImageOutSideEndpoint = ApiEndpoint + "/upload-out-side";
            public const string UploadImagePackageHouseEndpoint = ApiEndpoint + "/upload-package";
        }

        public static class Project
        {
            public const string ProjectEndpoint = ApiEndpoint + "/project";
            public const string ProjectSalesStaffEndpoint = ProjectEndpoint + "/sales";
            public const string ProjectDetailEndpoint = ProjectEndpoint + "/id";
            public const string ProjectDesignStaffEndpoint = ProjectEndpoint + "/design-staff";
            public const string ProjectByNumberPhone = ProjectEndpoint + "/phone";
            public const string ProjectListForCustomerEndpoint = ProjectEndpoint + "/email";
            public const string ProjectAssignEndpoint = ProjectEndpoint + "/assign";
            public const string ProjectCancelEndpoint = ProjectEndpoint + "/cancel";
            public const string ProjectTrackingEndpoint = ProjectEndpoint + "/tracking";
            public const string ProjectTemplateHouseEndpoint = ProjectEndpoint + "/template-house";
            public const string ProjectDeleteEndpoint = ProjectEndpoint + "/delete";
            public const string ProjectFilterByStatusEndpoint = ProjectEndpoint + "/filter";
            public const string ProjectSearchByContainName = ProjectEndpoint + "/contain/name";
            public const string ProjectHaveDrawingEndpoint = ProjectEndpoint + "/drawing-have";
            public const string FilterProjectsMultiParams = ProjectEndpoint + "/multi-filter";
            public const string TotalProjectEndpoint = ProjectEndpoint + "/total-project";
            public const string ProjectStatusEndpoint = ProjectEndpoint + "/status/id";
        }

        public static class Construction
        {
            public const string ConstructionEndpoint = ApiEndpoint + "/construction";
            public const string ConstructionDetailEndpoint = ConstructionEndpoint + "/id";
            public const string ConstructionDetailByNameEndpoint = ConstructionEndpoint + "/name";
            public const string ConstructionContainNameEndpoint = ConstructionEndpoint + "/contain/name";
            public const string ConstructionRoughEndpoint = ConstructionEndpoint + "/type";
            public const string ConstructionSearchWorkEndpoint = ConstructionEndpoint + "/construction-work/search";
            public const string ConstructionItemSearchTypeWorkEndpoint = ConstructionEndpoint + "/item-work/search";
            public const string GetConstructionEndpoint = ConstructionEndpoint + "/construction-work";
            public const string GetConstructionByPackageAndTypeEndpoint = ConstructionEndpoint + "/construction-work/package";
        }

        public static class ConstructionWork
        {
            public const string ConstructionWorkEndpoint = ApiEndpoint + "/construction-work";
            public const string ConstructionWorkDetailEndpoint = ConstructionWorkEndpoint + "/id";
            public const string ConstructionWorkByConIdEndpoint = ConstructionWorkEndpoint + "/consructionid";
            public const string ConstructionWorkPriceEndpoint = ConstructionWorkEndpoint + "/workid";
            public const string WorkTemplateEndpoint = ApiEndpoint + "/work-template";
            public const string ConstructionWorkFileEndpoint = ConstructionWorkEndpoint + "/import-file";
            public const string FilterConstructionWorkMultiParamsEndpoint = ConstructionWorkEndpoint + "/multi-filter";
        }

        public static class Utility
        {
            public const string UtilityEndpoint = ApiEndpoint + "/utilities";
            public const string UtilityDetaidEndpoint = UtilityEndpoint + "/id";
            public const string UtilityByTypeEndpoint = UtilityEndpoint + "/type";
            public const string UtilitySectionEndpoint = UtilityEndpoint + "/section";
            public const string UtilitySectionSearchEndpoint = UtilitySectionEndpoint + "/search";
            public const string UtilitySectionDEndpoint = UtilitySectionEndpoint + "/id";
            public const string UtilityItemHiddenEndpoint = UtilityEndpoint + "/hidden/id";
            public const string UtilityItemEndpoint = UtilityEndpoint + "/item";
            public const string UtilityItemSearchEndpoint = UtilityItemEndpoint + "/search/name";
            public const string UtilityAutoCharacterEndpoint = UtilityEndpoint + "/contain/name";
        }

        public static class HouseDesignDrawing
        {
            public const string HouseDesignDrawingEndpoint = ApiEndpoint + "/housedesign";
            public const string HouseDesignDrawingListEndpoint = HouseDesignDrawingEndpoint + "/list";
            public const string HouseDesignDrawingDesignStaffEndpoint = HouseDesignDrawingEndpoint + "/design";
            public const string HouseDesignDrawingPreviousEndpoint = HouseDesignDrawingEndpoint + "/previous";
            public const string HouseDesignDrawingDetailEndpoint = HouseDesignDrawingEndpoint + "/id";
            public const string HouseDesignDrawingTypeEndpoint = HouseDesignDrawingEndpoint + "/type";
            public const string HouseDesignDrawingTask = HouseDesignDrawingEndpoint + "/task";
            public const string HouseDesignDrawingAssignTask = HouseDesignDrawingEndpoint + "/assign";
            public const string HouseDesignConfirmProjectHaveDrawing = HouseDesignDrawingEndpoint + "/confirm/drawing-have";
            public const string HouseDesignDesignRequiment = HouseDesignDrawingEndpoint + "/request/design";
            public const string HouseDesignStatusEndpoint = HouseDesignDrawingEndpoint + "/status/id";
        }

        public static class HouseDesignVersion
        {
            public const string HouseDesignVersionEndpoint = ApiEndpoint + "/design";
            public const string HouseDesignVersionDetailEndpoint = HouseDesignVersionEndpoint + "/id";
            public const string HouseDesignVersionUploadFilesEndpoint = HouseDesignVersionEndpoint + "/upload-files";
            public const string ApproveHouseDesignVersionEndpoint = HouseDesignVersionEndpoint + "/approve";
            public const string HouseDesignVerisonConfirmEndpoint = HouseDesignVersionEndpoint + "/confirm";
            public const string HouseDesignVersionFeedbackEndpoint = HouseDesignVersionEndpoint + "/feedback";
        }

        public static class InitialQuotation
        {
            public const string InitialQuotationEndpoint = ApiEndpoint + "/quotation" + "/initial";
            public const string InitialQuotationDetailEndpoint = InitialQuotationEndpoint + "/id";
            public const string InitialQuotationDetailByCustomerEndpoint = InitialQuotationEndpoint + "/customer/name";
            public const string AssignInitialQuotationEndpoint = InitialQuotationEndpoint + "/assign";
            public const string ApproveInitialEndpoint = InitialQuotationEndpoint + "/approve";
            public const string InitialQuotationUpdateEndpoint = InitialQuotationEndpoint + "/update";
            public const string InitialQuotationProjectEndpoint = InitialQuotationEndpoint + "/list";
            public const string InitialQuotationNewVersionEndpoint = InitialQuotationEndpoint + "/new";
            public const string InitialQuotationCustomerComment = InitialQuotationEndpoint + "/comment";
            public const string InitialQuotationCustomerAgree = ApiEndpoint + "/quotation" + "/finalized";
            public const string InitialQuotationDesignStaffEndpoint = InitialQuotationEndpoint + "/design-staff";
            public const string InitialQuotationStatusEndpoint = InitialQuotationEndpoint + "/status/id";
        }
        public static class FinalQuotation
        {

            public const string FinalQuotationEndpoint = ApiEndpoint + "/quotation" + "/final";
            public const string FinalQuotationDetailEndpoint = FinalQuotationEndpoint + "/id";
            public const string FinalQuotationDetailByProjectIdEndpoint = FinalQuotationEndpoint + "/projectid";
            public const string FinalQuotationDetailByCustomerEndpoint = FinalQuotationEndpoint + "/customer/name";
            public const string AssignFinalQuotationEndpoint = FinalQuotationEndpoint + "/assign";
            public const string CancelFinalQuotationEndpoint = FinalQuotationEndpoint + "/cancel";
            public const string ApproveFinalQuotationEndpoint = FinalQuotationEndpoint + "/approve";
            public const string FinalQuotationProjectEndpoint = FinalQuotationEndpoint + "/list";
            public const string FinalQuotationCustomerAgree = FinalQuotationEndpoint + "/finalized";
            public const string FinalQuotationCustomerComment = FinalQuotationEndpoint + "/comment";
            public const string FianlQuotationStatusEndpoint = FinalQuotationEndpoint + "/status/id";
        }
        public static class Email
        {

            public const string SendEmailEndpoint = ApiEndpoint + "/email" + "/send";

        }
        public static class Contract
        {
            public const string ContractEndpoint = ApiEndpoint + "/contract";
            public const string ContractDetailEndpoint = ContractEndpoint + "/id";
            public const string ContractDesignEndpoint = ContractEndpoint + "/design";
            public const string ContractDesignTypeEndpoint = ContractEndpoint + "/type";
            public const string ContractDesignApproveEndpoint = ContractDesignEndpoint + "/approved";
            public const string ContractDesignDetailEndpoint = ContractDesignEndpoint + "/id";
            public const string ContractDesignSignCompletedEndpoint = ContractDesignEndpoint + "/sign/completed";
            public const string ContractConstructionEndpoint = ContractEndpoint + "/construction";
            public const string ContractConstructionDetailEndpoint = ContractConstructionEndpoint + "/id";
            public const string ContractConstructionApproveEndpoint = ContractConstructionEndpoint + "/approved";
            public const string ContractConstructionSignCompletedEndpoint = ContractConstructionEndpoint + "/sign/completed";
            public const string ContractFileEndpoint = ContractEndpoint + "/file";
            public const string PaymentBatchDesignConfirmEndpoint = ContractEndpoint + "/design/confirm";
            public const string PaymentBatchConstructionConfirmEndpoint = ContractEndpoint + "/construction/confirm";
            public const string PaymentBatchAppendixConfirmEndpoint = ContractEndpoint + "/appendix/confirm/bill";
            public const string FinalToContract = ContractEndpoint + "/final-to-contract/construction";
            public const string UploadFileContractEndpoint = ContractEndpoint + "/upload/file";
            public const string ManagerApproveBillFromCustomerEndpoint = ContractEndpoint + "/approve/bill";
            public const string ContractAppendixEndpoint = ContractEndpoint + "/appendix";
            public const string CustomerDeleteBillEndpoint = ContractEndpoint + "/bill/delete";
        }

        public static class Promotion
        {
            public const string PromotionEndpoint = ApiEndpoint + "/promotion";
            public const string PromotionValidEndpoint = PromotionEndpoint + "/valid";
            public const string PromotionDetailEndpoint = PromotionEndpoint + "/id";
            public const string PromotionNameEndpoint = PromotionEndpoint + "/name";
            public const string PromotionBanEndpoint = PromotionEndpoint + "/ban";
        }

        public static class Payment
        {
            public const string PaymentEndpoint = ApiEndpoint + "/payment";
            public const string PaymentDetailEndpoint = PaymentEndpoint + "/id";
            public const string PaymentTypeEndpoint = PaymentEndpoint + "/type";
            public const string PaymnetBatchEndpoint = PaymentEndpoint + "/batch/id";
            public const string PaymentBatchForCustomerEndpoint = PaymentEndpoint + "/list/app";
            public const string PaymentConfirmEndpoint = PaymentEndpoint + "/confirm";
        }

        public static class General
        {
            public const string UploadImageDrawingEndpoint = ApiEndpoint + "/drawing/version";
            public const string UploadAvatarEndpoint = ApiEndpoint + "/avatar";
        }

        public static class Material
        {
            public const string MaterialEndpoint = ApiEndpoint + "/material";
            public const string MaterialListEndpoint = ApiEndpoint + "/allmaterials";
            public const string SearchMaterialEndpoint = MaterialEndpoint + "/name";
            public const string SearchMaterialWithoutPackageEndpoint = MaterialEndpoint + "/allname";
            public const string FilterMaterialEndpoint = MaterialEndpoint + "/materialsectionid";
            public const string MaterialDetailEndpoint = MaterialEndpoint + "/id";
            public const string ImportExcel = MaterialEndpoint + "/import-excel";
        }

        public static class Labor
        {
            public const string LaborEndpoint = ApiEndpoint + "/labor";
            public const string LaborListEndpoint = ApiEndpoint + "/alllabors";
            public const string SearchLaborEndpoint = LaborEndpoint + "/name";
            public const string SearchLaborWithoutPackageEndpoint = LaborEndpoint + "/allname";
            public const string LaborDetailEndpoint = LaborEndpoint + "/id";
            public const string ImportExcel = LaborEndpoint + "/import-excel";
        }

        public static class MaterialType
        {
            public const string MaterialTypeEndpoint = ApiEndpoint + "/materialtype";
            public const string MaterialTypeListEndpoint = ApiEndpoint + "/allmaterialtypes";
            public const string SearchMaterialTypeEndpoint = MaterialTypeEndpoint + "/name";
            public const string MaterialTypeDetailEndpoint = MaterialTypeEndpoint + "/id";
        }

        public static class MaterialSection
        {
            public const string MaterialSectionEndpoint = ApiEndpoint + "/materialsection";
            public const string MaterialSectionListEndpoint = ApiEndpoint + "/allmaterialsections";
            public const string SearchMaterialSectionEndpoint = MaterialSectionEndpoint + "/name";
            public const string MaterialSectionDetailEndpoint = MaterialSectionEndpoint + "/id";
        }

        public static class Supplier
        {
            public const string SupplierEndpoint = ApiEndpoint + "/supplier";
            public const string SupplierListEndpoint = ApiEndpoint + "/allsuppliers";
            public const string SearchSupplierEndpoint = SupplierEndpoint + "/name";
            public const string SupplierDetailEndpoint = SupplierEndpoint + "/id";
        }

        public static class Room
        {
            public const string RoomEndpoint = ApiEndpoint + "/room";
            public const string RoomListWaitingEndpoint = RoomEndpoint + "/waiting";
            public const string ListMessageEndpoint = RoomEndpoint + "/message";
        }
    }
}
