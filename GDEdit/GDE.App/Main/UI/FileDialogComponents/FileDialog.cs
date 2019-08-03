using GDE.App.Main.Containers.KeyBindingContainers;
using GDE.App.Main.Panels;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static GDE.App.Main.Colors.GDEColors;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Math;
using static System.String;

namespace GDE.App.Main.UI.FileDialogComponents
{
    public abstract class FileDialog : Panel
    {
        public const float ItemSpacing = 2.5f;

        private readonly GDEBreadcrumbNavigationTextBox filePathBreadcrumbs;
        private readonly TextBox search;

        private readonly DirectoryItemContainer itemContainer;

        protected abstract bool AllowInexistentFileNames { get; }
        protected virtual string FileDialogActionName { get; set; }

        /// <summary>The button that performs the file dialog's action.</summary>
        protected GDEButton ActionButton;

        public readonly Bindable<DrawableItem> CurrentSelection = new Bindable<DrawableItem>();

        public readonly Bindable<string> SelectedItemBindable = new Bindable<string>();
        public readonly Bindable<string> CurrentDirectoryBindable = new Bindable<string>("");

        public bool IsSelectedPathValid
        {
            get
            {
                if (SelectedPath == null)
                    return false;
                if (File.Exists(SelectedPath))
                    return true;

                // This is hacky as fuck, but the only solution that was quick enough to come up with
                // And mind you, I'm not planning on staying awake any further
                try
                {
                    File.Create(SelectedPath).Close();
                    File.Delete(SelectedPath);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool CanFinalizeSelection => (AllowInexistentFileNames && IsSelectedPathValid) || CurrentlySelectedItem != null;

        public string SelectedItem
        {
            get => SelectedItemBindable.Value;
            set => SelectedItemBindable.Value = value;
        }
        public string SelectedPath
        {
            get => GetCurrentSelectedPath();
            set => UpdatePath(value);
        }
        public string CurrentDirectory
        {
            get => CurrentDirectoryBindable.Value;
            set => CurrentDirectoryBindable.Value = FixDirectoryPath(value);
        }

        public DrawableItem CurrentlySelectedItem
        {
            get => CurrentSelection.Value;
            set => CurrentSelection.Value = value;
        }
        public int CurrentSelectionIndex
        {
            get => itemContainer.CurrentSelectionIndex;
            set => itemContainer.CurrentSelectionIndex = value;
        }

        public event Action<string> OnFileSelected;

        public FileDialog(string defaultDirectory = null)
        {
            CornerRadius = 10;
            Masking = true;

            LockDrag = true;

            AddRangeInternal(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = FromHex("1a1a1a")
                },
                new DrawSizePreservingFillContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(10),
                    Strategy = DrawSizePreservationStrategy.Minimum,
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = $"{FileDialogActionName} File",
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Y = 35,
                            Height = 30,
                            Child = filePathBreadcrumbs = new GDEBreadcrumbNavigationTextBox
                            {
                                Origin = Anchor.CentreLeft,
                                Anchor = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.X,
                                Height = 30,
                            }
                        },
                        new FileDialogActionContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding
                            {
                                Top = 70,
                                Bottom = 40,
                            },
                            Child = itemContainer = new DirectoryItemContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                            },
                        },
                        new Container
                        {
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            RelativeSizeAxes = Axes.X,
                            Height = 30,
                            Children = new Drawable[]
                            {
                                new Container
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    RelativeSizeAxes = Axes.Both,
                                    Padding = new MarginPadding { Right = 110 },
                                    Children = new Drawable[]
                                    {
                                        search = new TextBox
                                        {
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            RelativeSizeAxes = Axes.Both,
                                            PlaceholderText = "Selected Path",
                                            OnCommit = (sender, newText) => SelectedItem = sender.Text,
                                        },
                                        new SpriteIcon
                                        {
                                            Anchor = Anchor.CentreRight,
                                            Origin = Anchor.CentreRight,
                                            Size = new Vector2(15),
                                            Margin = new MarginPadding { Right = 10 },
                                            Icon = FontAwesome.Solid.Search,
                                        },
                                    }
                                },
                                ActionButton = new FadeButton
                                {
                                    Anchor = Anchor.CentreRight,
                                    Origin = Anchor.CentreRight,
                                    Width = 100,
                                    EnabledColor = FromHex("303030"),
                                    Action = ActionButtonAction,
                                    Text = FileDialogActionName,
                                },
                            }
                        }
                    }
                }
            });

            ActionButton.Enabled.Value = false;

            CurrentSelection.BindTo(itemContainer.CurrentSelection);
            CurrentDirectoryBindable.BindTo(itemContainer.CurrentDirectoryBindable);
            SelectedItemBindable.BindTo(itemContainer.SelectedItemBindable);

            CurrentSelection.ValueChanged += HandleCurrentSelectionChanged;
            CurrentDirectoryBindable.ValueChanged += HandleDirectoryChanged;
            SelectedItemBindable.ValueChanged += HandleItemChanged;

            itemContainer.PerformActionRequested += HandlePerformActionRequested;

            filePathBreadcrumbs.OnTextChanged += HandleBreadcrumbsUpdated;
            filePathBreadcrumbs.BreadcrumbNavigation.BreadcrumbClicked += HandleBreadcrumbsUpdated;

            CurrentDirectory = defaultDirectory ?? GetFolderPath(MyDocuments);
        }

        public void UpdateActionButtonState()
        {
            ActionButton.Enabled.Value = CanFinalizeSelection;
            ActionButton.Text = CurrentlySelectedItem?.IsDirectory ?? false ? "Open Folder" : FileDialogActionName;
        }

        public void UpdatePath(string newPath)
        {
            string replaced = FixPath(newPath);
            search.Text = replaced;
            CurrentDirectory = GetDirectoryName(replaced) ?? GetDirectoryRoot(replaced);
        }

        public void HandleItemChanged(ValueChangedEvent<string> value)
        {
            search.Text = value.NewValue;
            UpdateActionButtonState();
        }
        public void HandleDirectoryChanged(ValueChangedEvent<string> value)
        {
            UpdateBreadcrumbs();
        }

        protected virtual void ActionButtonAction() => PerformAction();

        private void HandleBreadcrumbsUpdated(string dir) => CurrentDirectory = GetCurrentBreadcrumbsDirectory();

        private void HandleDirectoryChanged() => UpdateBreadcrumbs();
        private void HandlePerformActionRequested() => PerformAction();

        private void HandleCurrentSelectionChanged(ValueChangedEvent<DrawableItem> obj) => UpdateActionButtonState();

        private void PerformAction()
        {
            if (CurrentlySelectedItem?.ItemType == ItemType.Directory)
                NavigateToSelectedDirectory();
            else if (CanFinalizeSelection)
                FinalizeSelection();
        }

        private void NavigateToSelectedDirectory() => CurrentDirectory = GetCurrentSelectedPath();
        private void FinalizeSelection()
        {
            OnFileSelected?.Invoke(SelectedPath);
            ToggleVisibility();
        }

        private void UpdateBreadcrumbs()
        {
            string[] dirs = AnalyzePath(CurrentDirectory);
            filePathBreadcrumbs.Items.Clear();
            filePathBreadcrumbs.Items.AddRange(dirs);
        }

        private string GetCurrentBreadcrumbsDirectory() => $@"{filePathBreadcrumbs.Items.ToList().ConvertAll(AddDirectorySuffix).Aggregate(AggregateDirectories)}";
        private string GetCurrentSelectedPath() => $@"{CurrentDirectory}{CurrentlySelectedItem?.GetPathSuffix() ?? SelectedItem}";

        private static string FixPath(string dirPath) => dirPath.Replace('/', '\\');
        private static string FixDirectoryPath(string dirPath)
        {
            string result = FixPath(dirPath);
            if (!result.EndsWith('\\'))
                result += '\\';
            return result;
        }

        private static string AddDirectorySuffix(string name) => $@"{name}\";
        private static string AggregateDirectories(string left, string right) => $@"{left}{right}";

        private static string[] AnalyzePath(string path) => FixPath(path).Split('\\');

        private static string GetCommonDirectory(string pathA, string pathB)
        {
            string[] splitA = AnalyzePath(pathA);
            string[] splitB = AnalyzePath(pathB);
            List<string> result = new List<string>();
            int min = Min(splitA.Length, splitB.Length);
            for (int i = 0; i < min; i++)
                if (splitA[i] == splitB[i])
                    result.Add(splitA[i]);
            return result.Aggregate(AggregateDirectories);
        }
        private static string GetPreviousPathDirectoryInNewPath(string previousPath, string newPath)
        {
            string[] splitPrevious = AnalyzePath(previousPath);
            string[] splitNew = AnalyzePath(newPath);
            if (splitNew.Length >= splitPrevious.Length)
                return null;

            int index = -1;
            while (++index < splitNew.Length)
                if (splitPrevious[index] != splitNew[index])
                    break;
            return splitPrevious[index];
        }
        private static string GetIndividualItemName(string path) => AnalyzePath(path).Last();

        private static ItemType DetermineItemType(string path)
        {
            if (path.EndsWith('\\') || IsNullOrWhiteSpace(GetExtension(path)))
                return ItemType.Directory;
            return ItemType.File;
        }
    }
}
