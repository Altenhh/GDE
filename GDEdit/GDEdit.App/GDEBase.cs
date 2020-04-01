using System;
using System.Threading;
using System.Threading.Tasks;
using GDEdit.Resources;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Development;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace GDEdit.App
{
    /// <summary>Provides the basis for the App.</summary>
    public class GDEBase : Game
    {
        private DependencyContainer dependencies;
        private readonly Storage storage;
        
        private int allowableExceptions = DebugUtils.IsDebugBuild ? 0 : 1;

        public GDEBase()
        {
            Name = "GD Edit";
            storage = new NativeStorage("GD Edit");
        }
        
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new DllResourceStore(GDEResources.ResourceAssembly));
            
            dependencies.Cache(this);
            dependencies.CacheAs(storage);
            
            AddFont(Resources, @"Fonts/Torus-Regular");
            AddFont(Resources, @"Fonts/Torus-Light");
            AddFont(Resources, @"Fonts/Torus-SemiBold");
            AddFont(Resources, @"Fonts/Torus-Bold");

            AddFont(Resources, @"Fonts/Noto-Basic");
            AddFont(Resources, @"Fonts/Noto-Hangul");
            AddFont(Resources, @"Fonts/Noto-CJK-Basic");
            AddFont(Resources, @"Fonts/Noto-CJK-Compatibility");
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
            var continueExecution = Interlocked.Decrement(ref allowableExceptions) >= 0;

            Logger.Log($"Unhandled exception has been {(continueExecution ? $"allowed with {allowableExceptions} more allowable exceptions" : "denied")}.");
            Task.Delay(1000).ContinueWith(_ => Interlocked.Increment(ref allowableExceptions));

            return continueExecution;
        }
    }
}