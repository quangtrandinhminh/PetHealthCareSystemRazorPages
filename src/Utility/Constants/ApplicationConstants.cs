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
    }
    public class ReponseMessageConstantsCommon
    {
        public const string EXISTED = "Already existed!";
    }
    
    public class ReponseMessageIdentity
    {
        public const string INVALID_USER = "Nguoi dung khong ton tai.";
        public const string UNAUTHENTICATED = "Khong xac thuc.";
        public const string PASSWORD_NOT_MATCH = "Mat khau khong giong nhau.";
        public const string PASSWORD_WRONG = "Mat khau khong dung.";
        public const string EXISTED_USER = "Nguoi dung da ton tai.";
        public const string EXISTED_EMAIL = "Email da ton tai.";
        public const string EXISTED_PHONE = "So dien thoai da ton tai.";
        public const string REGIST_USER_SUCCESS = "Create account successfully! Please verify phone number to activate it";
        public const string TOKEN_INVALID = "token khong xac thuc.";
        public const string TOKEN_INVALID_OR_EXPIRED = "token khong xac thuc hoac da het han.";
        public const string EMAIL_VALIDATE = "Email da duoc xav thuc.";
        public const string ROLE_INVALID = "Roles khong xac thuc.";
        public const string CLAIM_NOTFOUND = "Khong tim thay claim.";
        public const string EXISTED_ROLE = "Role da ton tai.";

        public const string USERNAME_REQUIRED = "Ten nguoi dung khong duoc de trong.";
        public const string NAME_REQUIRED = "Ten khong duoc de trong.";
        public const string USERCODE_REQUIRED = "Ma nguoi dung khong duoc de trong.";
        public const string PASSWORD_REQUIRED = "Mat khau khong duoc de trong.";
        public const string CONFIRM_PASSWORD_REQUIRED = "Xac nhan mat khau khong duoc de trong.";
        public const string EMAIL_REQUIRED = "Email khong duoc de trong.";
        public const string PHONENUMBER_REQUIRED = "So dien thoai khong duoc de trong.";
        public const string PHONENUMBER_INVALID = "So dien thoai khong hop le.";
        public const string PHONENUMBER_LENGTH = "So dien thoai phai co chinh xac 10 so.";
        public const string PHONGBANID_REQUIRED = "Phong ban khong duoc de trong.";
        public const string NHAMAYIDS_REQUIRED = "Nha may khong duoc de trong.";
        public const string ROLEIDS_REQUIRED = "Role khong duoc de trong.";

    }

    // Response message constants for entities: not found, existed, update success, delete success
    public class ReponseMessageConstantsPet
    {
        public const string PET_NOT_FOUND = "Khong tim thay pet";
        public const string PET_EXISTED = "Pet da ton tai";
        public const string ADD_PET_SUCCESS = "Them pet thanh cong";
        public const string UPDATE_PET_SUCCESS = "Cap nhat pet thanh cong";
        public const string DELETE_PET_SUCCESS = "Xoa pet thanh cong";
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