using System.Collections.Generic;
using System.ComponentModel;
using GDAPI.Objects.GeometryDash.LevelObjects;
using GDE.App.Main.UI;
using GDE.App.Main.UI.Shadowed;
using JetBrains.Annotations;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels.Object.Content
{
    public class ObjectContent : GeneralContent
    {
        private LevelObjectCollection selectedObjects;

        public ObjectContent(LevelObjectCollection objects) => selectedObjects = objects;

        protected override Drawable[,] CreateContent()
        {
            int rows = 8;
            int columns = 5;

            var content = new Drawable[rows, columns];

            int cellIndex = 0;

            var objectIDBind = new BindableDouble(selectedObjects.CommonObjectID);

            var posXBind = new BindableDouble(selectedObjects.CommonX);
            var posYBind = new BindableDouble(selectedObjects.CommonY);

            var rotBind = new BindableDouble(selectedObjects.CommonRotation) { MinValue = 0, MaxValue = 360 };
            var scaBind = new BindableDouble(selectedObjects.CommonScaling);
            
            var zOrdBind = new BindableDouble(selectedObjects.CommonZOrder) { MinValue = 0, MaxValue = double.MaxValue };
            var zLayBind = new BindableDouble(selectedObjects.CommonZLayer) { MinValue = 0, MaxValue = double.MaxValue };
            
            var ed1Bind = new BindableDouble(selectedObjects.CommonEL1) { MinValue = 0, MaxValue = double.MaxValue };
            var ed2Bind = new BindableDouble(selectedObjects.CommonEL2) { MinValue = 0, MaxValue = double.MaxValue };
            
            var lgiBind = new BindableDouble(selectedObjects.CommonLinkedGroupID);

            var position = CreateDrawable(Name = "Position", "X", posXBind, "Y", posYBind);
            var rotation = CreateDrawable(Name = "Rotation", null, rotBind);
            var scale = CreateDrawable(Name = "Scale", null, scaBind);
            var zOrder = CreateDrawable(Name = "Z Order", null, zOrdBind);
            // This one will require the Drawable change
            var zLayer = CreateDrawable(Name = "Z Layer", null, zLayBind);
            var editorLayer = CreateDrawable(Name = "Editor Layer", "1", ed1Bind, "2", ed2Bind);
            var linkedGroupID = CreateDrawable(Name = "Linked Group ID", null, lgiBind);
            // This one will be something else ENTIRELY
            //var groupIDs

            for (int i = 0; i < 5; i++)
            {
                content[0, i] = position[i];
                content[1, i] = rotation[i];
                content[2, i] = scale[i];
                content[3, i] = zOrder[i];
                content[4, i] = zLayer[i];
                content[5, i] = editorLayer[i];
                content[6, i] = linkedGroupID[i];
            }

            return content;
        }

        private Drawable[] CreateDrawable(string title, string extra1, BindableDouble value, string extra2 = null, BindableDouble value1 = null)
        {
            var drawables = new List<Drawable>
            {
                new SpriteText { Text = title, Anchor = Anchor.CentreRight, Origin = Anchor.CentreRight },
                new SpriteText { Text = extra1, Anchor = Anchor.CentreRight, Origin = Anchor.CentreRight },
                new GDENumberTextBox { Anchor = Anchor.CentreLeft, Origin = Anchor.CentreLeft, X = 20, Width = 125 },
                new SpriteText { Text = extra2, Anchor = Anchor.CentreRight, Origin = Anchor.CentreRight }
            };
            if (extra1 != null)
                drawables.Add(new GDENumberTextBox { Anchor = Anchor.CentreLeft, Origin = Anchor.CentreLeft, X = 20, Width = 125 });

            return drawables.ToArray();
        }
        
        // Used to make Z Layer have an extra button
        private Drawable[] CreateDrawable(Drawable title, BindableDouble value, Drawable extra1 = null, BindableDouble value1 = null,
            Drawable extra2 = null, BindableDouble value2 = null)
        {
            List<Drawable> drawables;
            drawables = new List<Drawable>();

            drawables.Add(title);
            drawables.Add(new GDENumberTextBox());
            drawables.Add(extra1);
            drawables.Add(extra1 != null ? new GDENumberTextBox() : null);
            drawables.Add(extra2);
            drawables.Add(extra2 != null ? new GDENumberTextBox() : null);

            return drawables.ToArray();
        }
    }
}