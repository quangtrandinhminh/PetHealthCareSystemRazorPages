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
        public const string BAD_REQUEST = "Bad request!";
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
    public class ResponseMessageConstantsCommon
    {
        public const string EXISTED = "Already existed!";
        public const string SUCCESS = "Thao tác thành công";
        public const string NO_DATA = "Không có dữ liệu trả về";
        public const string SERVER_ERROR = "Lỗi từ phía server vui lòng liên hệ đội ngũ phát triển";
        public const string DATE_WRONG_FORMAT = "Dữ liệu ngày không đúng định dạng yyyy-mm-dd";
        public const string DATA_NOT_ENOUGH = "Dữ liệu đưa vào không đầy đủ";
    }

    public class ResponseMessageIdentity
    {
        public const string INVALID_USER = "Nguoi dung khong ton tai.";
        public const string UNAUTHENTICATED = "Khong xac thuc.";
        public const string PASSWORD_NOT_MATCH = "Mat khau khong giong nhau.";
        public const string PASSWORD_WRONG = "Mat khau khong dung.";
        public const string EXISTED_USER = "Nguoi dung da ton tai.";
        public const string EXISTED_EMAIL = "Email da ton tai.";
        public const string EXISTED_PHONE = "So dien thoai da ton tai.";
        public const string TOKEN_INVALID = "token khong xac thuc.";
        public const string TOKEN_EXPIRED = "token khong xac thuc hoac da het han.";
        public const string TOKEN_INVALID_OR_EXPIRED = "token khong xac thuc hoac da het han.";
        public const string EMAIL_VALIDATED = "Email da duoc xac thuc.";
        public const string PHONE_VALIDATED = "Phone number is validated.";
        public const string ROLE_INVALID = "Roles khong xac thuc.";
        public const string CLAIM_NOTFOUND = "Khong tim thay claim.";
        public const string EXISTED_ROLE = "Role da ton tai.";

        public const string USERNAME_REQUIRED = "Ten nguoi dung khong duoc de trong.";
        public const string NAME_REQUIRED = "Ten khong duoc de trong.";
        public const string USERCODE_REQUIRED = "Ma nguoi dung khong duoc de trong.";
        public const string PASSWORD_REQUIRED = "Mat khau khong duoc de trong.";
        public const string PASSSWORD_LENGTH = "Mat khau phai co it nhat 5 ky tu.";
        public const string CONFIRM_PASSWORD_REQUIRED = "Xac nhan mat khau khong duoc de trong.";
        public const string EMAIL_REQUIRED = "Email khong duoc de trong.";
        public const string PHONENUMBER_REQUIRED = "So dien thoai khong duoc de trong.";
        public const string PHONENUMBER_INVALID = "So dien thoai khong hop le.";
        public const string PHONENUMBER_LENGTH = "So dien thoai phai co chinh xac 10 so.";
        public const string ROLES_REQUIRED = "Role khong duoc de trong.";
        public const string USER_NOT_ALLOWED = "Ban khong co quyen truy cap vao muc nay";

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
        public const string VET_NOT_FOUND = "Không tìm thấy bác sĩ";
        public const string VET_EXISTED = "Bác sĩ đã tồn tại";
        public const string ADD_VET_SUCCESS = "Thêm bác sĩ thành công";
        public const string UPDATE_VET_SUCCESS = "Cập nhật bác sĩ thành công";
        public const string DELETE_VET_SUCCESS = "Xóa bác sĩ thành công";
    }

    public class ResponseMessageConstantsTimetable
    {
        public const string NOT_FOUND = "Không tìm thấy khung giờ";
    }

    public class ResponseMessageConstantsPet
    {
        public const string PET_NOT_FOUND = "Không tìm thấy thú cưng";
        public const string PET_EXISTED = "Thú cưng đã tồn tại";
        public const string ADD_PET_SUCCESS = "Thêm thú cưng thành công";
        public const string UPDATE_PET_SUCCESS = "Cập nhật thú cưng thành công";
        public const string DELETE_PET_SUCCESS = "Xóa thú cưng thành công";
        public const string OWNER_NOT_FOUND = "Không tìm thấy chủ thú cưng";
        public const string NOT_YOUR_PET = "Thú cưng không phải là của bạn";
        public const string PETID_REQUIRED = "Id thu cung khong duoc de trong";
    }

    public class ResponseMessageConstantsService
    {
        public const string SERVICE_NOT_FOUND = "Khong tim thay dich vu";
        public const string SERVICE_EXISTED = "Dich vu da ton tai";
        public const string ADD_SERVICE_SUCCESS = "Them dich vu thanh cong";
        public const string UPDATE_SERVICE_SUCCESS = "Cap nhat dich vu thanh cong";
        public const string DELETE_SERVICE_SUCCESS = "Xoa dich vu thanh cong";
    }

    public class ResponseMessageConstantsMedicalItem
    {
        public const string MEDICAL_ITEM_NOT_FOUND = "Khong tim thay vat tu y te";
        public const string MEDICAL_ITEM_EXISTED = "Vat tu y te da ton tai";
        public const string ADD_MEDICAL_ITEM_SUCCESS = "Them vat tu y te thanh cong";
        public const string UPDATE_MEDICAL_ITEM_SUCCESS = "Cap nhat vat tu y te thanh cong";
        public const string DELETE_MEDICAL_ITEM_SUCCESS = "Xoa vat tu y te thanh cong";
    }

    public class ResponseMessageConstantsTransaction
    {
        public const string TRANSACTION_NOT_FOUND = "Khong tim thay giao dich";
        public const string TRANSACTION_EXISTED = "Giao dich da ton tai";
        public const string INVALID_TRANSACTION = "Giao dich khong hop le";
        public const string TRANSACTION_DETAIL_REQUIRED = "Vui long nhap chi tiet giao dich";
        public const string INVALID_TRANSACTION_STATUS = "Trang thai giao dich khong hop le";
        public const string INVALID_PAYMENT_METHOD = "Phuong thuc thanh toan khong hop le";
        public const string PAYMENT_REQUIRED = "Vui long nhap ma giao dich";
        public const string ADD_TRANSACTION_SUCCESS = "Them giao dich thanh cong";
        public const string UPDATE_PAYMENT_SUCCESS = "Cap nhat thanh toan thanh cong";
        public const string DELETE_TRANSACTION_SUCCESS = "Xoa giao dich thanh cong";
        public const string TRANSACTION_PAID = "Giao dich da thanh toan";
    }

    public class ResponseMessageConstantsAppointment
    {
        public const string APPOINTMENT_NOT_FOUND = "Không tìm thấy lịch hẹn";
        public const string APPOINTMENT_EXISTED = "Lịch hẹn đã tồn tại";
        public const string ADD_APPOINTMENT_SUCCESS = "Thêm lịch hẹn thành công";
        public const string UPDATE_APPOINTMENT_SUCCESS = "Cập nhật lịch hẹn thành công";
        public const string DELETE_APPOINTMENT_SUCCESS = "Xóa lịch hẹn thành công";
        public const string APPOINTMENT_ID_REQUIRED = "Id lich hen khong duoc de trong";
        public const string APPOINTMENT_PET_NOT_FOUND = "Khong tim thay thong tin thú cưng trong lich hen";
        public const string APPOINTMENT_COMPLETED = "Lich hen da hoan thanh";
    }

    public class ResponseMessageConstantsMedicalRecord
    {
        public const string MEDICAL_RECORD_NOT_FOUND = "Khong tim thay ho so benh an";
        public const string MEDICAL_RECORD_EXISTED = "Ho so benh an da ton tai";
        public const string ADD_MEDICAL_RECORD_SUCCESS = "Them ho so benh an thanh cong";
        public const string UPDATE_MEDICAL_RECORD_SUCCESS = "Cap nhat ho so benh an thanh cong";
        public const string DELETE_MEDICAL_RECORD_SUCCESS = "Xoa ho so benh an thanh cong";
        public const string MEDICAL_RECORD_VET_NOT_ALLOWED = "Yeu cau bac si duoc hen tao ho so benh an";
        public const string NEXT_APPOINTMENT_INVALID = "Ngay hen tiep theo khong hop le";
        public const string ADMISSION_DATE_INVALID = "Ngay nhap vien khong hop le";
        public const string DISCHARGE_DATE_INVALID = "Ngay xuat vien khong hop le";
        public const string PET_WEIGHT_INVALID = "Can nang khong hop le";
    }

    public class ResponseMessageConstantsHospitalization
    {
        public const string HOSPITALIZATION_NOT_FOUND = "Khong tim thay ho so luu chuong";
    }
}