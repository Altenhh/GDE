using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Collections.Generic;
using System.Linq;
using GDE.App.Main.UI.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GDE.App.Main.Panels.Object.Content
{
    /// <summary>Generalizes the content for use in other classes.</summary>
    public abstract class GeneralContent : TableContainer
    {
        private const float rowHeight = 25;

        protected GeneralContent()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            RowSize = new Dimension(GridSizeMode.Absolute, rowHeight);

            Content = null;
            
            Columns = CreateHeaders();
            Content = CreateContent();
        }

        private TableColumn[] CreateHeaders()
        {
            var columns = new List<TableColumn>
            {
                new TableColumn("name", Anchor.CentreLeft, new Dimension()),
                new TableColumn("extra name", Anchor.CentreRight, new Dimension()),
                new TableColumn("value", Anchor.CentreLeft, new Dimension(minSize:50, maxSize:100)),
                new TableColumn("extra name", Anchor.CentreRight, new Dimension()),
                new TableColumn("value", Anchor.CentreLeft, new Dimension(minSize:50, maxSize:100)),
            };
            
            return columns.ToArray();
        }

        protected abstract Drawable[,] CreateContent();
        
        protected override Drawable CreateHeader(int index, TableColumn column) => new HeaderText(column?.Header ?? string.Empty);

        private class HeaderText : SpriteText
        {
            public HeaderText(string text)
            {
                Text = text.ToUpper();
                Font = GDEFont.GetFont(size: 20);
            }
        }
    }
}