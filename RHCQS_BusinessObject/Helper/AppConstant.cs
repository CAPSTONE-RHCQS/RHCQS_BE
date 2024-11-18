using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace RHCQS_BusinessObjects;

public static class AppConstant
{
    public class MessageError : Exception
    {
        public int Code { get; set; }
        public new string Message { get; set; }

        public MessageError(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class General
    {
        public const string Bill = "Hóa đơn";
        public const string PaymentDesign = "Hợp đồng tư vấn và thiết kế bản vẽ nhà ở dân dụng";
        public const string Initial = "Báo giá sơ bộ";
        public const string Final = "Báo giá chi tiết";
        public const string Active = "Active";
        public const double MaxVersion = 10.0;
    }
    public class Unit
    {
        public const string UnitPrice = "VNĐ";
        public const string UnitPriceD = "đ";
        public const string Percent = "%";
    }
    public class Role
    {
        public const string Manager = "Manager";
        public const string Staff = "Staff";
        public const string Customer = "Customer";
    }

    public class Type
    {
        public const string ROUGH = "ROUGH";
        public const string FINISHED = "FINISHED";
        public const string ALL = "ALL";
        public const string TEMPLATE = "TEMPLATE";
        public const string PHOICANH = "PHOICANH";
        public const string KIENTRUC = "KIENTRUC";
        public const string KETCAU = "KETCAU";
        public const string DIENNUOC = "DIENNUOC";
        public const string DRAWINGHAVE = "HAVE_DRAWING";
    }

    public class ProjectStatus
    {
        public const string PROCESSING = "Processing";
        public const string DESIGNED = "Designed";
        public const string UNDER_REVIEW = "Reviewing";
        public const string SIGNED_CONTRACT = "Signed Contract";
        public const string FINALIZED = "Finalized";
        public const string ENDED = "Ended";
    }

    public class QuotationStatus
    {
        public const string PENDING = "Pending";
        public const string PROCESSING = "Processing";
        public const string REJECTED = "Rejected";
        public const string REVIEWING = "Reviewing";
        public const string APPROVED = "Approved";
        public const string FINALIZED = "Finalized";
        public const string CANCELED = "Canceled";
    }

    public class HouseDesignStatus
    {
        public const string PROCESSING = "Processing";
        public const string REVIEWING = "Reviewing";
        public const string REJECTED = "Rejected";
        public const string UPDATING = "Updating";
        public const string APPROVED = "Approved";
        public const string FINALIZED = "Finalized";
        public const string CANCELED = "Canceled";
        public const string ACCEPTED = "Accepted";
    }

    public class ContractStatus
    {
        public const string PROCESSING = "Processing";
        public const string COMPLETED = "Completed";
        public const string FINISHED = "Finished";
        public const string ENDED = "Ended";
    }
    public class Template
    {
        public const string Drawing = "Bản vẽ";
        public const string Structuredrawings = "Cấu trúc";
        public const string Exteriorsdrawings = "Ngoại cảnh";
        public const string OverallDrawing = "Tổng thể";
        public const string PackageFinished = "Gói thi hoàn thiện";
    }

    public class NameTemplate
    {
        public const string Drawing = "Ban_ve";
        public const string Structuredrawings = "Cau_truc";
        public const string Exteriorsdrawings = "Ngoai_canh";
        public const string OverallDrawing = "Tong_the";
        public const string PackageFinished = "Goi_hoan_thien";
    }
    public class Status
    {
        public const string PROCESSING = "Processing";
        public const string UPDATED = "Updated";
    }

    public class PaymentStatus
    {
        public const string PROGRESS = "Progress";
        public const string PAID = "Paid";
    }
    public class Profile
    {
        public const string IMAGE = "Ảnh Profile";
    }
    public enum ContractType
    {
        [Description("Hợp đồng tư vấn và thiết kế bản vẽ nhà ở dân dụng")]
        Design,
        [Description("Hợp đồng thi công nhà ở dân dụng")]
        Construction
    }

    public enum DesignDrawing
    {
        [Description("Phối cảnh")]
        Perspective,

        [Description("Kiến trúc")]
        Architecture,

        [Description("Kết cấu")]
        Structure,

        [Description("Điện & nước")]
        ElectricityWater
    }

    public enum ErrCode
    {
        Success = 200,
        Internal_Server_Error = 500,
        Bad_Request = 400,
        Unauthorized = 401,
        Forbidden = 403,
        Not_Found = 404,
        Conflict = 409,
        Too_Many_Requests = 429,
        Unprocessable_Entity = 422,
        NotFound = 501
    }

    public class Message
    {
        public const string APPROVED = "Báo giá đã được phê duyệt thành công!";
        public const string REJECTED = "Báo giá đã bị từ chối!";
        public const string ERROR = "Có lỗi xảy ra trong quá trình lưu!";
        public const string SUCCESSFUL_INITIAL = "Cập nhập báo giá sơ bộ thành công";
        public const string SUCCESSFUL_FINAL = "Cập nhập báo giá chi tiết thành công";
        public const string SUCCESSFUL_CANCELFINAL = "Hủy báo giá chi tiết thành công";
        public const string SUCCESSFUL_CREATEFINAL = "Tạo báo giá chi tiết thành công";
        public const string SUCCESSFUL_SAVE = "Lưu thành công!";
        public const string SUCCESSFUL_CREATE = "Tạo thành công";
        public const string SUCCESSFUL_UPDATE = "Cập nhật thành công";
        public const string SUCCESSFUL_DELETE = "Xóa thành công";
        public const string SEND_SUCESSFUL = "Gửi thành công!";
        public const string PASSWORD_SUCESSFUL = "Cập nhật mật khẩu thành công!";
    }
    public class ErrMessage
    {
        public const string Internal_Server_Error = "Hệ thống xảy ra lỗi, vui lòng thử lại";
        public const string Oops = "Oops !!! Something Wrong. Try Again.";
        public const string Not_Found_Resource = "The server can not find the requested resource.";
        public const string Bad_Request = "Cú pháp không hợp lệ";
        public const string Unauthorized = "Unauthorized.";
        public const string PromotionIllegal = "Khuyến mãi không hợp lệ";
        public const string ProjectNotExit = "Dự án không tồn tại!";
        public const string PhoneIsEmpty = "Nhập lại số điện thoại của khách hàng";
        public const string Fail_Save = "Lưu thất bại!";
        public const string Send_Fail = "Gửi thất bại!";
        public const string InvalidData = "Dữ liệu không hợp lệ!";
        public const string InvalidFile = "Không có file";

        //Construction
        public const string ConstructionExit = "Hạng mục xây dựng đã tồn tại!";
        public const string ConstructionNoExit = "Hạng mục xây dựng không tồn tại!";
        public const string ConstructionNameExit = "Tên hạng mục đã tồn tại! Hãy nhập tên khác!";

        //Login
        public const string EmailExists = "Email đã tồn tại!";
        public const string AccountInActive = "Tài khoản bị khóa!";
        public const string InvalidPassword = "Mật khẩu không đúng!";
        public const string PasswordMismatch = "Mật khẩu không trùng khớp!";
        public const string Not_Found_Account = "Tài khoản không tồn tại!";
        public const string RoleNotFound = "Role này không tồn tại!";
        public const string CreateAccountError = "Tạo tài khoản thất bại!";
        public const string NullValue = "Invalid input: Field cannot be empty";

        //Acount
        public const string AccountIdError = "AccounntId không hợp lệ (phải theo kiểu Gui)!";
        public const string PageAndSizeError = "Page and size must be greater than 0!";
        public const string FailedToGetList = "Lấy danh sách account thất bại!";
        public const string BanAccount = "Ban tài khoản thất bại!";
        public const string UpdateAccount = "Cập nhật tài khoản thất bại!";
        public const string UpdatePasswordFailed = "Cập nhật mật khẩu thất bại!";
        public const string IncorrectPassword = "Password cũ không đúng!";
        public const string Invalid_Customer = "Khách hàng không tồn tại trong hệ thống!";

        //HouseTemplate
        public const string Not_Found_HouseTemplate = "Không tìm thấy mẫu nhà!";
        public const string Not_Access_DesignDrawing = "Tài khoản này không có quyền truy cập bản vẽ này!";
        public const string Not_Found_DesignDrawing = "Không tìm thấy bản vẽ!";
        public const string UpdateTempalte = "Cập nhật mẫu nhà thất bại!";
        public const string DesignTemplate = "Mẫu nhà này đã tồn tại!";
        public const string TemplateItem = "Tempalate item này đã tồn tại!";
        public const string SubTemplateItem = "SubTempalate item này đã tồn tại!";
        public const string TemplateItemNotFound = "TemplateItem không tìm thấy.";
        public const string SubTemplateNotFound = "SubTemplate không tìm thấy.";

        //House Design Drawing Version
        public const string FailUploadDrawing = "Cập nhập bản vẽ thất bại!";
        public const string OverloadProjectDrawing = "Dự án đã khởi tạo 4 bản vẽ thiết kế!";
        public const string DesignNoAccess = "Không có quyền truy cập dự án này!";
        public const string HouseDesignDrawing = "Bản thiết kế không tồn tại!";
        public const string House_Design_Not_Found = "Không tìm thấy bản vẽ!";
        public const string Invalid_Perspective = "Cần cung cấp đủ bản vẽ phối cảnh";
        public const string Invalid_Architecture = "Cần cung cấp đủ bản vẽ kiến trúc";
        public const string Invalid_Structure = "Cần cung cấp đủ bản vẽ kết cấu";
        public const string Invalid_ElectricityWater = "Cần cung cấp đủ bản vẽ điện & nước";

        //Quotation
        public const string Not_Found_InitialQuotaion = "Không tìm thấy báo giá sơ bộ!";
        public const string Not_Found_FinalQuotaion = "Không tìm thấy báo giá chi tiết!";
        public const string FinalQuotaionExists = "FinalQuotaion đã tồn tại.";
        public const string CreateFinalQuotaion = "Tạo FinalQuotaion thất bại.";
        public const string CancelFinalQuotaion = "Cancel FinalQuotaion thất bại.";
        public const string CancelFinalQuotaionAlready = "Quotes không thể cancel được vì status không phù hợp.";
        public const string Not_Finalized_Final_Quotation = "Báo giá chưa kết thúc!";
        public const string Conflict_Version = "Hiện tại đã có phiên bản báo giá này!";
        public const string Reason_Rejected_Required = "Cần phải có lý do báo giá bị từ chối!";
        public const string InvalidPackageQuotation = "Bắt buộc phải có gói thi công! Hãy chọn gói thi công trước khi hoàn tất";
        public const string InvalidBatchPayment = "Các đợt thanh toán hợp đồng là bắt buộc!";
        public const string MaxVersionQuotation = "Báo giá đã đạt tới lần chỉnh sửa thứ 10, dự án sẽ bị hủy!";

        //Role
        public const string Not_Found_Role = "Không tìm thấy role theo id đã nhập.";

        //PackageType
        public const string CreatePackageType = "Tạo packagetype thất bại.";
        public const string PackageTypeExists = "PackageType đã tồn tại.";

        //Assign staff
        public const string OverloadStaff = "Sales đã nhận đủ dự án báo giá! Hãy chọn Sales khác!";
        public const string QuotationHasStaff = "Dự án đã có nhân viên đảm nhận!";
        public const string NotFinalizedQuotationInitial = "Báo giá sơ bộ chưa kết thúc! Hãy hoàn thành báo giá sơ bộ trước khi phân công!";
        public const string NotFinishedContractDesign = "Hợp đồng thiết kế chưa được hoàn thành! " +
            "Hãy hoàn thành hợp đồng thiết kế trước khi phân công";
        public const string NotStartContractDesign = "Hợp đồng thiết chưa thống nhất! Hãy kí hợp đồng trước khi phân công!";

        //Package
        public const string CreatePackage = "Tạo packaget thất bại.";
        public const string UpdatePackage = "Cập nhật packaget thất bại.";
        public const string PackageExists = "Package đã tồn tại.";
        public const string PackageNotFound = "Package không tìm thấy.";
        public const string PackageLaborNotFound = "Package labor không tìm thấy.";
        public const string PackageHouseNotFound = "Package house không tìm thấy.";
        public const string PackagematerialNotFound = "Package material không tìm thấy.";

        //Media
        public const string Not_Found_Media = "Không tìm thấy Media!";

        //Contract
        public const string Invail_Quotation = "Báo giá chưa chốt!";
        public const string Contract_Not_Found = "Không tìm thấy báo giá!";
        public const string Contract_Waiting = "Hợp đồng đang chờ duyệt";

        //Promotion
        public const string Invalid_Start_Time = "Thời gian bắt đầu ở quá khứ hoặc trùng với thời gian hiện tại!";
        public const string Invalid_Exp_Time = "Thời gian kết thúc ở quá khứ!";
        public const string Invalid_Distance_Time = "Khoảng cách giữa thời gian bắt đầu và kết thúc phải lớn hơn 2 ngày!";
        public const string Not_Found_Promotion = "Không tìm thấy khuyến mãi này!";
        public const string Promotion_No_Update = "Khuyến mãi đang được thực hiện, không thể thay đổi!";
        public const string Invail_Time = "Thời gian không hợp lệ!";

        //Payment
        public const string Invalid_Payment = "Thanh toán này không tìm thấy!";
        public const string Type_Not_Found = "Không tìm thấy kiểu hợp đồng!";

        //Blog
        public const string Not_Found_Blog = "Bài đăng này không tìm thấy!";

        //Utility
        public const string Utility_Not_Found = "Không tìm thấy tiện ích!";
        public const string Utility_Duplicate = "Tên tiện ích đã bị trùng! Hãy đổi tên khác!";

        //FinalQuotation
        public const string FinalQuotationExists = "FinalQuotation đã tồn tại.";


    }
}

