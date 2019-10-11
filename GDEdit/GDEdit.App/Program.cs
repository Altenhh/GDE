using osu.Framework;
using osu.Framework.Platform;

namespace GDEdit.App
{
    public static class Program
    {
        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost(@"GDEdit");
            using Game game = new GDEMain();
                host.Run(game);
        }
    }
}