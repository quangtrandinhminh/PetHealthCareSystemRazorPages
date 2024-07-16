namespace Utility.Constants;

public class ConfigurationKey
{
    public const string HospitalizationPrice = "HospitalizationPrice";
    public const string PayOsClientId = "PayOsClientId";
    public const string PayOsApiKey = "PayOsApiKey";
    public const string PayOsChecksumKey = "PayOsChecksumKey";
    public const string PayOsOrderId = "PayOsOrderId";
    public const string BookPrice = "BookPrice";

    public static readonly Dictionary<string, string> KeyDictionary = new()
    {
        {HospitalizationPrice,"Giá nhập viện"},
        {PayOsClientId, "Client Id từ PayOS"},
        {PayOsApiKey,"API Key từ PayOS"},
        {PayOsChecksumKey, "Checksum Key từ PayOS"},
        {PayOsOrderId,"Mã đơn hàng của PayOS"},
        {BookPrice, "Giá khám"},
    };
}