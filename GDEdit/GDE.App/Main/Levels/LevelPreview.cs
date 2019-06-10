using GDE.App.Main.Graphics.Containers;
using GDE.App.Main.Objects;
using GDE.App.Main.Screens.Edit;
using GDE.App.Main.Screens.Edit.Components;
using GDE.App.Main.Graphics.UserInterface;
using GDEdit.Application;
using GDEdit.Application.Editor;
using GDEdit.Utilities.Objects.GeometryDash;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Bindings;
using System.Collections.Generic;

namespace GDE.App.Main.Levels
{
    public class LevelPreview : Container<ObjectBase>, IKeyBindingHandler<GlobalAction>, IDraggable
    {
        private EditorScreen editorScreen;

        private readonly int i;

        private Database database;

        private bool modifier;

        public IReadOnlyList<ObjectBase> Objects => Children;

        public bool Draggable => true;

        public Level Level => database.UserLevels[i];

        public LevelPreview(EditorScreen editorScreen, int index)
        {
            this.editorScreen = editorScreen;
            i = index;

            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection databases)
        {
            database = databases[0];

            foreach (var o in Level.LevelObjects)
                Add(new ObjectBase(o));
        }

        public bool OnPressed(GlobalAction action)
        {
            var val = modifier ? Editor.SmallMovementStep : Editor.NormalMovementStep;

            foreach (var i in Objects)
            {
                if (i.State == SelectionState.Selected)
                    switch (action)
                    {
                        case GlobalAction.ObjMoveRight:
                            i.ObjectX += val;
                            break;
                        case GlobalAction.ObjMoveLeft:
                            i.ObjectX -= val;
                            break;
                        case GlobalAction.ObjMoveUp:
                            i.ObjectY += val;
                            break;
                        case GlobalAction.ObjMoveDown:
                            i.ObjectY -= val;
                            break;
                    }
            }

            switch (action)
            {
                case GlobalAction.ObjMoveModifier:
                    modifier = !modifier;
                    break;
            }

            return true;
        }

        public bool OnReleased(GlobalAction action) => true;
    }
}
