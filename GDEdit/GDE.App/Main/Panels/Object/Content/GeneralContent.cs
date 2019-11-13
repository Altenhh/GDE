using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;

namespace GDE.App.Main.Panels.Object.Content
{
    /// <summary>Generalizes the content for use in other classes.</summary>
    public abstract class GeneralContent : TableContainer
    {
        protected GeneralContent()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            RowSize = new Dimension(GridSizeMode.Absolute, 25f);

            Content = null;

            Columns = CreateHeaders();
            Content = CreateContent();
        }

        private TableColumn[] CreateHeaders()
        {
            var columns = new List<TableColumn>
            {
                new TableColumn("", Anchor.CentreLeft, new Dimension()),  // Name
                new TableColumn("", Anchor.CentreRight, new Dimension()), // Extra Name
                new TableColumn("", Anchor.CentreLeft, new Dimension(minSize:50, maxSize:200)), // Value
                new TableColumn("", Anchor.CentreRight, new Dimension()), // Name
                new TableColumn("", Anchor.CentreLeft, new Dimension(minSize:50, maxSize:200)), // Value
            };

            return columns.ToArray();
        }

        protected abstract Drawable[,] CreateContent();
    }
}