using System;
using System.Threading;
using System.Threading.Tasks;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Development;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace GDEdit.App
{
    /// <summary>Provides the basis for the App.</summary>
    public class GDEBase : Game
    {
        private DependencyContainer dependencies;

        private LargeTextureStore largeStore;
        private readonly Storage storage;
        
        private int allowableExceptions = DebugUtils.IsDebugBuild ? 0 : 1;

        public GDEBase()
        {
            Name = "GD Edit";
            storage = new NativeStorage("GD Edit");
        }
        
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
            => dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        
        [BackgroundDependencyLoader]
        private void Load()
        {
            Resources.AddStore(new DllResourceStore("GDEdit.Resources.dll"));

            largeStore = new LargeTextureStore(Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, @"GDEdit")));
            
            dependencies.Cache(this);
            dependencies.Cache(largeStore);
            dependencies.Cache(storage);
        }
        
        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

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