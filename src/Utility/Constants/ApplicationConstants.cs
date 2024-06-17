using System.Runtime.CompilerServices;

namespace Utility.Constants
{
    public class ApplicationConstants
    {
        public const string KEYID_EXISTED = "KeyId {0} đã tồn tại.";
        public const string KeyId = "KeyId";
        public const string DUPLICATE = "Symtem_id is duplicated";
    }

    public class ResponseCodeConstants
    {
        public const string NOT_FOUND = "Not found!";
        public const string SUCCESS = "Success!";
        public const string FAILED = "Failed!";
        public const string EXISTED = "Existed!";
        public const string DUPLICATE = "Duplicate!";
        public const string INTERNAL_SERVER_ERROR = "INTERNAL_SERVER_ERROR";
        public const string INVALID_INPUT = "Invalid input!";
        public const string UNAUTHORIZED = "Unauthorized!";
        public const string FORBIDDEN = "Forbidden!";
        public const string EXPIRED = "Expired!"; 
    }
    public class ReponseMessageConstantsCommon
    {
        public const string EXISTED = "Already existed!";
    }


    
    public class ReponseMessageIdentity
    {
        public const String INVALID_USER = "Người dùng không tồn tại.";
        public const String UNAUTHENTICATED = "Không xác thực.";
        public const String PASSWORD_NOT_MATCH = "Mật khẩu không khớp nhau.";
        public const String PASSWORD_WRONG = "Mật khẩu không đúng.";
        public const String EXISTED_USER = "Người dùng đã tồn tại ";
        public const String EXISTED_EMAIL = "Email đã tồn tại.";
        public const String EXISTED_PHONE = "Số điện thoại đã tồn tại.";
        public const String TOKEN_INVALID = "Token không xác thực.";
        public const String TOKEN_EXPIRED = "Token không xác thực hoặc đã hết hạn.";
        public const String TOKEN_INVALID_OR_EXPIRED = "Token không xác thực hoặc đã hết hạn.";
        public const String EMAIL_VALIDATE = "Email đã được xác thực.";
        public const String PHONE_VALIDATE = "Số điện thoại đã được xác thực.";
        public const String ROLE_INVALID = "Quyền không xác thực.";
        public const String CLAIM_NOTFOUND = "Không tìm thấy claim.";
        public const String EXISTED_ROLE = "Role đã tồn tại.";

        public const String USERNAME_REQUIRED = "Tên người dùng không được để trống.";
        public const String NAME_REQUIRED = "Tên không được để trống.";
        public const String USERCODE_REQUIRED = "Mã người dùng không được để trống.";
        public const String PASSWORD_REQUIRED = "Mật khẩu không được để trống.";
        public const String PASSSWORD_LENGTH = "Mật khẩu phải có ít nhất 8 ký tự.";
        public const String CONFIRM_PASSWORD_REQUIRED = "Xác nhận mật khẩu không được để trống.";
        public const String EMAIL_REQUIRED = "Email không được để trống.";
        public const String PHONENUMBER_REQUIRED = "Số điện thoại không được để trống.";
        public const String PHONENUMBER_INVALID = "Số điện thoại không hợp lệ.";
        public const String PHONENUMBER_LENGTH = "Số điện thoại phải có chính xác 10 số.";
        public const String PHONGBANID_REQUIRED = "Phòng ban không được để trống.";
        public const String NHAMAYIDS_REQUIRED = "Nhà máy không được để trống.";
        public const String ROLES_REQUIRED = "Quyền không được để trống.";


    }

    public class ResponseMessageIdentitySuccess
    {
        public const string REGIST_USER_SUCCESS = "Dang ky tai khoan thanh cong! Vui long xac thuc email de kich hoat tai khoan";
        public const string VERIFY_PHONE_SUCCESS = "Xac thuc so dien thoai thanh cong!";
        public const string VERIFY_EMAIL_SUCCESS = "Xac thuc email thanh cong!";
        public const string FORGOT_PASSWORD_SUCCESS = "Yeu cau cap lai mat khau thanh cong, vui long kiem tra email";
        public const string RESET_PASSWORD_SUCCESS = "Cap lai mat khau thanh cong!";
        public const string CHANGE_PASSWORD_SUCCESS = "Doi mat khau thanh cong!";
        public const string RESEND_EMAIL_SUCCESS = "Gui lai email xac thuc thanh cong!";
        public const string UPDATE_USER_SUCCESS = "Cap nhat thong tin nguoi dung thanh cong!";
        public const string DELETE_USER_SUCCESS = "Xoa nguoi dung thanh cong!";
        public const string ADD_ROLE_SUCCESS = "Them role thanh cong!";
        public const string UPDATE_ROLE_SUCCESS = "Cap nhat role thanh cong!";
        public const string DELETE_ROLE_SUCCESS = "Xoa role thanh cong!";

    }

    // Response message constants for entities: not found, existed, update success, delete success

    public class ResponseMessageConstantsVet
    {
        public const string VET_NOT_FOUND = "Khong tim thay bac si";
        public const string VET_EXISTED = "Bac si da ton tai";
        public const string ADD_VET_SUCCESS = "Them bac si thanh cong";
        public const string UPDATE_VET_SUCCESS = "Cap nhat bac si thanh cong";
        public const string DELETE_VET_SUCCESS = "Xoa bac si thanh cong";
    }

    public class ReponseMessageConstantsPet
    {
        public const string PET_NOT_FOUND = "Không tìm thấy thú cưng";
        public const string PET_EXISTED = "Thú cưng đã tồn tại";
        public const string ADD_PET_SUCCESS = "Thêm thú cưng thành công";
        public const string UPDATE_PET_SUCCESS = "Cập nhật thú cưng thành công";
        public const string DELETE_PET_SUCCESS = "Xóa thú cưng thành công";
        public const string OWNER_NOT_FOUND = "Không tìm thấy chủ thú cưng";
        public const string NOT_YOUR_PET = "Thú cưng không phải là của bạn";
    }

    public class ReponseMessageConstantsService
    {
        public const string SERVICE_NOT_FOUND = "Khong tim thay dich vu";
        public const string SERVICE_EXISTED = "Dich vu da ton tai";
        public const string ADD_SERVICE_SUCCESS = "Them dich vu thanh cong";
        public const string UPDATE_SERVICE_SUCCESS = "Cap nhat dich vu thanh cong";
        public const string DELETE_SERVICE_SUCCESS = "Xoa dich vu thanh cong";
    }
}