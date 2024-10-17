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
        public string Message { get; set; }

        public MessageError(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }

    public class General
    {
        public const string Bill = "Hóa đơn";
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
        public const string PHOICANH = "PHOICANH";
        public const string KIENTRUC = "KIENTRUC";
        public const string KETCAU = "KETCAU";
        public const string DIENNUOC = "DIENNUOC";
    }

    public class ProjectStatus
    {
        public const string PROCESSING = "Processing";
        public const string DESIGNED = "Designed";
        public const string UNDER_REVIEW = "Reviewing";
        public const string SIGNED_DESIGN_CONTRACT = "Signed Design Contract";
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

    public class ConstractStatus
    {
        public const string PROCESSING = "Processing";
        public const string COMPLETED = "Completed";
        public const string FINISHED = "Finished";
        public const string ENDED = "Ended";
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
        Unprocessable_Entity = 422
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

        //Quotation
        public const string Not_Found_InitialQuotaion = "Không tìm thấy báo giá sơ bộ!";
        public const string Not_Found_FinalQuotaion = "Không tìm thấy báo giá chi tiết!";

        public const string FinalQuotaionExists = "FinalQuotaion đã tồn tại.";
        public const string CreateFinalQuotaion = "Tạo FinalQuotaion thất bại.";
        public const string CancelFinalQuotaion = "Cancel FinalQuotaion thất bại.";
        public const string CancelFinalQuotaionAlready = "Quotes không thể cancel được vì status không phù hợp.";

        //Role
        public const string Not_Found_Role = "Không tìm thấy role theo id đã nhập.";

        //PackageType
        public const string CreatePackageType = "Tạo packagetype thất bại.";
        public const string PackageTypeExists = "PackageType đã tồn tại.";

        //Assign staff
        public const string OverloadStaff = "Sales đã nhận đủ dự án báo giá! Hãy chọn Sales khác!";
        public const string QuotationHasStaff = "Dự án đã có nhân viên đảm nhận!";

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
    }
}

