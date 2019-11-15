using osu.Framework.Graphics;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public class Sawblade : GDObject
    {
        public override int ID { get; set; } = 88;
        public override Vector2 Offset { get; set; } = new Vector2();
        public override Anchor Anchor { get; set; } = Anchor.CentreRight;

        protected override int DuplicationAmount { get; set; } = 2;
    }
}