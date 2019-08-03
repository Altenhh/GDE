using GDEdit.Application;
using GDEdit.Utilities.Objects.GeometryDash;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Testing;
using System.Threading.Tasks;

namespace GDE.Tests.Application
{
    public class TestSceneDatabaseMain : TestScene
    {
        private Database database;
        private LevelCollection levels;
        private readonly SpriteText name, description, revision, version, objectCount, length;
        private bool finishedLoading;

        public TestSceneDatabaseMain()
        {
            Children = new[]
            {
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.X,
                    RelativePositionAxes = Axes.X,
                    Direction = FillDirection.Vertical,
                    Children = new[]
                    {
                        name = new SpriteText
                        {
                            RelativeSizeAxes = Axes.X,
                            Font = new FontUsage(size: 40),
                        },
                        description = new SpriteText
                        {
                            RelativeSizeAxes = Axes.X,
                            Font = new FontUsage(size: 15),
                        },
                        revision = new SpriteText
                        {
                            RelativeSizeAxes = Axes.X,
                            Font = new FontUsage(size: 20),
                        },
                        version = new SpriteText
                        {
                            RelativeSizeAxes = Axes.X,
                            Font = new FontUsage(size: 20),
                        },
                        objectCount = new SpriteText
                        {
                            RelativeSizeAxes = Axes.X,
                            Font = new FontUsage(size: 20),
                        },
                        length = new SpriteText
                        {
                            RelativeSizeAxes = Axes.X,
                            Font = new FontUsage(size: 20),
                        },
                    }
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(DatabaseCollection d)
        {
            database = d[0];
        }

        protected override void Update()
        {
            if (!finishedLoading && (finishedLoading = database.GetLevelsStatus >= TaskStatus.RanToCompletion))
            {
                if ((levels = database.UserLevels).Count > 0)
                {
                    Level level = levels[0];
                    name.Text = $"Name: {level.Name}";
                    description.Text = $"Description: {level.Description}";
                    revision.Text = $"Revision: {level.Revision}";
                    version.Text = $"Version: {level.Version}";
                    objectCount.Text = $"Object Count: {level.ObjectCount}";
                    length.Text = $"Length: {level.Length}";
                }
                else
                    name.Text = "No levels";
            }
            if (!finishedLoading)
                name.Text = "Loading";

            base.Update();
        }
    }
}
