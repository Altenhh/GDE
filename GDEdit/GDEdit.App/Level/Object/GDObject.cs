using GDEdit.App.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GDEdit.App.Level.Object
{
    public abstract class GDObject
    {
        /// <summary>ID of the object.</summary>
        public abstract int ID { get; set; }

        /// <summary>Origin offset.</summary>
        public abstract Vector2 Offset { get; set; }

        public abstract Anchor Anchor { get; set; }

        /// <summary>How many times should the <see cref="Sprite" /> be duplicated in a circle</summary>
        protected virtual int DuplicationAmount { get; set; } = 1;

        /// <summary>Create what you would see on the Level Preview. Only use this for absolutely special objects.</summary>
        /// <returns>The correct interpretation of the object.</returns>
        public virtual Drawable CreateDrawable(TextureStore store)
        {
            var rotationOffset = 360f / DuplicationAmount;

            var container = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            };

            for (var i = 0; i < DuplicationAmount; i++)
            {
                container.Add(new Sprite
                {
                    Texture = store.Get($"Objects/{ID}.png"),
                    FillMode = FillMode.Fit,
                    Rotation = rotationOffset,
                    Origin = Anchor
                });

                rotationOffset += rotationOffset;
            }

            var drawable = new ConstrainedSpriteContainer
            {
                Size = new Vector2(store.Get($"Objects/{ID}.png").Height / 2),
                Sprite = container
            };

            return drawable;
        }
    }
}