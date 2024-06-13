namespace WaterCity.Core.Constants
{
    public static class WebApiEndpoint
    {
        public const string AreaName = "api";

        public static class ThanhToan
        {
            private const string BaseEndpoint = "~/" + AreaName + "/thanh-toan";
            public const string FilterThanhToan = BaseEndpoint + "/filter";
        }

        //  CRUDEntity = BaseEndpoint + "/do-function/{param}";
        public static class InHoaDon
        {
            private const string BaseEndpoint = "~/" + AreaName + "/in-hoa-don";
            public const string FilterInHoaDon = BaseEndpoint + "/filter-in-hoa-don";
            public const string GetAllInHoaDon = BaseEndpoint + "/get-all";
            public const string AddHoaDon = BaseEndpoint + "/in-hoa-don";
            public const string InKetHoaDon = BaseEndpoint + "/in-ket-hoa-don";
            public const string InLaiHoaDon = BaseEndpoint + "/in-lai-hoa-don";
            public const string InLaiHoaDonTheoMaKhachhang = BaseEndpoint + "/in-lai-hoa-don-theo-ma-khach-hang";
            public const string FetchInHoaDon = BaseEndpoint + "/fetch-data-in-lai-hoa-don";
        }
    }
}