using GDE.App.Main.Levels;
using GDE.App.Main.Objects;
using GDAPI.Application.Editor;
using GDAPI.Utilities.Objects.GeometryDash.LevelObjects;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osuTK;

namespace GDE.App.Main.Screens.Edit.Components
{
    public class Camera : Container
    {
        private Editor editor;
        private Bindable<Vector2> cameraOffsetBindable = new Bindable<Vector2>();
        private GridSnappedCursorContainer snappedCursorContainer;

        public Camera(Editor Editor)
        {
            editor = Editor;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(snappedCursorContainer = new GridSnappedCursorContainer(cameraOffsetBindable) { Anchor = Anchor.Centre, Origin = Anchor.Centre });
        }

        public Vector2 ConvertMousePositionToEditor(Vector2 mousePosition) => snappedCursorContainer.ConvertMousePositionToEditor(mousePosition);

        public GeneralObject GetClonedGhostObjectLevelObject() => snappedCursorContainer.GhostObject.GetObject().Clone();

        protected override bool OnDrag(DragEvent e)
        {
            cameraOffsetBindable.Value += e.Delta;

            foreach (var child in Children)
            {
                if (child is IDraggable draggable)
                    if (draggable.Draggable)
                        child.Position += e.Delta;

                if (child is Grid grid)
                {
                    grid.Position = new Vector2(
                        -GetCoordinate(cameraOffsetBindable.Value.X) + cameraOffsetBindable.Value.X,
                        -GetCoordinate(cameraOffsetBindable.Value.Y) + cameraOffsetBindable.Value.Y);
                }
            }

            return true;
        }

        protected override bool OnDragEnd(DragEndEvent e) => true;
        protected override bool OnDragStart(DragStartEvent e) => true;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.ShiftPressed)
                if (editor != null)
                    editor.Swipe = true;

            return base.OnKeyDown(e);
        }
        protected override bool OnKeyUp(KeyUpEvent e)
        {
            if (e.ShiftPressed)
                if (editor != null)
                    editor.Swipe = false;

            return base.OnKeyUp(e);
        }

        public void SetGhostObjectID(int id) => snappedCursorContainer.GhostObjectID = id;
        public void HideGhostObject() => snappedCursorContainer.HideGhostObject();
        public void ShowGhostObject() => snappedCursorContainer.ShowGhostObject();

        private static float GetCoordinate(float c)
        {
            float r = c;
            if (r < 0)
                r -= 30;
            return r -= r % 30;
        }

        // TODO: This should be its own seperate class
        private class GridSnappedCursorContainer : CursorContainer, IRequireHighFrequencyMousePosition, IDraggable
        {
            public readonly GhostObject GhostObject;
            public bool Draggable => false;

            public int SnapResolution { get; set; }

            public int GhostObjectID
            {
                get => GhostObject.ObjectID;
                set
                {
                    GhostObject.ObjectID = value;
                    GhostObject.Alpha = 0.5f;
                }
            }

            public static readonly Bindable<Vector2> CameraOffset = new Bindable<Vector2>();

            public GridSnappedCursorContainer(Bindable<Vector2> cameraOffsetBindable, int ghostObjectID = 0, int snapResolution = 30)
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;
                RelativeSizeAxes = Axes.Both;
                AlwaysPresent = true;
                Children = new Drawable[] { GhostObject = new GhostObject
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.TopLeft
                }};
                CameraOffset.BindTo(cameraOffsetBindable);
                GhostObjectID = ghostObjectID;
                SnapResolution = snapResolution;
            }

            protected override bool OnMouseMove(MouseMoveEvent e)
            {
                foreach (var child in Children)
                    child.Position = ConvertMousePositionToEditor(e.ScreenSpaceMousePosition - AnchorPosition - CameraOffset.Value);

                return base.OnMouseMove(e);
            }

            public Vector2 ConvertMousePositionToEditor(Vector2 mousePosition)
            {
                float x = GetCoordinate(mousePosition.X);
                float y = GetCoordinate(mousePosition.Y);
                return new Vector2(x, y) + CameraOffset.Value;
            }

            public void HideGhostObject() => GhostObject.Hide();
            public void ShowGhostObject() => GhostObject.Show();
        }

        private class GhostObject : ObjectBase
        {
            public int SnapResolution { get; set; } = 30;

            private readonly Bindable<Vector2> cameraOffset = new Bindable<Vector2>();

            public GhostObject(GeneralObject o)
                : base(o)
            {
                cameraOffset.BindTo(GridSnappedCursorContainer.CameraOffset);
                Selectable = false;
            }

            public GhostObject(int id = 1)
                : base(id)
            {
                cameraOffset.BindTo(GridSnappedCursorContainer.CameraOffset);
                Selectable = false;
            }

            public GeneralObject GetObject()
            {
                LevelObject.X = Position.X - cameraOffset.Value.X + SnapResolution / 2;
                LevelObject.Y = -Position.Y + cameraOffset.Value.Y - SnapResolution / 2;
                return LevelObject;
            }
        }
    }
}
