using GDAPI.Application;
using GDAPI.Application.Editor;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Development;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GDE.App.Main
{
    public class GDEAppBase : Game
    {
        private const string mainResourceFile = "GDE.Resources.dll";

        private static int allowableExceptions = DebugUtils.IsDebugBuild ? 0 : 1;

        private DependencyContainer dependencies;
        private Storage storage;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public GDEAppBase()
        {
            storage = new NativeStorage("GD Edit");
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(mainResourceFile));

            dependencies.Cache(storage);

            dependencies.Cache(this);
            dependencies.CacheAs(new Editor(null));
            dependencies.CacheAs(new DatabaseCollection());

            //Purista | Default font
            Fonts.AddStore(new GlyphStore(Resources, @"Fonts/Purista/Purista"));
            //Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-Italic"));

            //Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-Bold"));
            //Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-BoldItalic"));

//            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-SemiBold"));
//            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-SemiBoldItalic"));

//            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-Light"));
//            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-LightItalic"));

//            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-Thin"));
//            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Purista/Purista-ThinItalic"));

            //Noto | To support most Unicodes & Japanese letters
            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Noto/Noto-Basic"));
            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Noto/Noto-Hangul"));

            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Noto/Noto-CJK-Basic"));
            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Noto/Noto-CJK-Compatibility"));

            //Digitall | All numbers will be handeled with this font with some few exceptions
            Fonts.AddStore(new GlyphStore(Resources, "Fonts/Digitall/Digitall"));
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            var config = new FrameworkConfigManager(storage);

            Window.Title = @"GD Edit";

            host.ExceptionThrown += ExceptionHandler;
        }

        protected virtual bool ExceptionHandler(Exception arg)
        {
            bool continueExecution = Interlocked.Decrement(ref allowableExceptions) >= 0;

            Logger.Log($"Unhandled exception has been {(continueExecution ? $"allowed with {allowableExceptions} more allowable exceptions" : "denied")}.");
            Task.Delay(1000).ContinueWith(_ => Interlocked.Increment(ref allowableExceptions));

            return continueExecution;
        }
    }
}
