using osu.Framework;
using osu.Framework.Platform;

namespace GDEdit.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using (DesktopGameHost host = Host.GetSuitableHost("GDE Tests", true))
                host.Run(new GDETestBrowser());
        }
    }
}