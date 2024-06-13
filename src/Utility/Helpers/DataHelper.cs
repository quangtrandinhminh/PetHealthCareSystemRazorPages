namespace Utility.Helpers
{
    public static class DataHelper
    {
        public static int RandomChiSoMoi(int chisocu)
        {
            Random r = new Random();
            int rInt = r.Next(0, 100);

            //for doubles
            int range = chisocu + rInt;
            return range;
        }
    }
}
