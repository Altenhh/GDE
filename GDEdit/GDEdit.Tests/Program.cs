using osu.Framework;

namespace GDEdit.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (var host = Host.GetSuitableHost("GDE Tests", true))
            {
                host.Run(new GDETestBrowser());
            }
        }
    }
}