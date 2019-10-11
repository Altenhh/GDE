using GDEdit.App;
using osu.Framework.Testing;

namespace GDEdit.Tests
{
    public class GDETestBrowser : GDEBase
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();
            
            Add(new TestBrowser("GDEdit.Tests"));
        }
    }
}