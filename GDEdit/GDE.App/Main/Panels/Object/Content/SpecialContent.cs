using System.Collections.Generic;
using System.ComponentModel;
using GDE.App.Main.UI.Shadowed;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace GDE.App.Main.Panels.Object.Content
{
    public class SpecialContent : GeneralContent
    {
        protected override Drawable[,] CreateContent()
        {
            int rows = 10;
            int columns = 5;

            var content = new Drawable[rows, columns];

            int cellIndex = 0;

            for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
                content[r, c] = new Cell(cellIndex++);

            return content;
        }

        private class Cell : SpriteText
        {
            public Cell(int index)
            {
                Text = $"Special Cell {index}";
            }
        }
    }
}