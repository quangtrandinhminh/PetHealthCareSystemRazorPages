using Utility.Enum;

namespace BusinessObject.DTO.Transaction;

public class TransactionDropdownDto
{
    public List<EnumResponseDto> PaymentMethods { get; set; }
    public List<EnumResponseDto> TransactionStatus { get; set; }
}