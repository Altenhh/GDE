using GDE.App.Main.Containers.KeyBindingContainers;
using GDE.App.Main.UI.Containers;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;
using static GDE.App.Main.Colors.GDEColors;
using static System.IO.Directory;
using static System.IO.Path;
using static System.Math;
using static System.String;

namespace GDE.App.Main.UI.FileDialogComponents
{
    public class DirectoryItemContainer : KeyBindingActionContainer<FileDialogAction>
    {
        public const float ItemSpacing = 2.5f;

        private int? currentSelectionIndex;
        private string currentlyLoadedDirectory;

        private readonly GDEScrollContainer scrollContainer;
        private readonly FadeSearchContainer fileContainer;
        private readonly FillFlowContainer fileFillFlowContainer;

        private readonly DrawableItem currentSelection;

        /// <summary>The button that performs the file dialog's action.</summary>
        protected GDEButton ActionButton;

        public readonly Bindable<DrawableItem> CurrentSelection = new Bindable<DrawableItem>();

        public readonly Bindable<string> SelectedItemBindable = new Bindable<string>();
        public readonly Bindable<string> CurrentDirectoryBindable = new Bindable<string>("");

        public event Action PerformActionRequested;

        public string SelectedItem
        {
            get => SelectedItemBindable.Value;
            set => SelectedItemBindable.Value = value;
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
            get => currentSelectionIndex ?? (currentSelectionIndex = fileFillFlowContainer.Children.ToList().IndexOf(CurrentlySelectedItem)).Value;
            set => CurrentlySelectedItem = fileFillFlowContainer.Children[(currentSelectionIndex = value).Value] as DrawableItem;
        }

        public DirectoryItemContainer()
            : base()
        {
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    CornerRadius = 5,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = FromHex("121212"),
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(10),
                            Child = scrollContainer = new GDEScrollContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    fileContainer = new FadeSearchContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Child = fileFillFlowContainer = new FillFlowContainer
                                        {
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(0, ItemSpacing),
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                        }
                                    },
                                },
                                ScrollOnKeyDown = false,
                            },
                        },
                    }
                },
            };

            CurrentSelection.ValueChanged += HandleSelectionChanged;
            CurrentDirectoryBindable.ValueChanged += HandleDirectoryChanged;
            SelectedItemBindable.ValueChanged += HandleItemChanged;
        }

        public void NavigateUp() => NavigateTo(CurrentSelectionIndex - 1);
        public void NavigateDown() => NavigateTo(CurrentSelectionIndex + 1);
        public void NavigatePageUp() => NavigateTo(CurrentSelectionIndex - GetItemCountPerPage());
        public void NavigatePageDown() => NavigateTo(CurrentSelectionIndex + GetItemCountPerPage());
        public void NavigateToStart() => NavigateTo(0);
        public void NavigateToEnd() => NavigateTo(fileFillFlowContainer.Count - 1);
        public void NavigateToPreviousDirectory()
        {
            string[] dirs = AnalyzePath(CurrentDirectory);
            if (dirs.Length < 3)
                return;

            CurrentDirectory = dirs.SkipLast(2).ToList().ConvertAll(AddDirectorySuffix).Aggregate(AggregateDirectories);
        }
        public void NavigateTo(int index)
        {
            if (fileFillFlowContainer.Count > 0)
                CurrentSelectionIndex = Clamp(index, 0, fileFillFlowContainer.Count - 1);
        }

        public void HandleSelectionChanged(ValueChangedEvent<DrawableItem> value)
        {
            if (value.OldValue != null)
                value.OldValue.Selected = false;
            if (value.NewValue != null)
                SelectItem(value.NewValue);
            SelectedItem = value.NewValue?.ItemName;
        }
        public void HandleItemChanged(ValueChangedEvent<string> value)
        {
            string newItem = value.NewValue;

            if (CurrentlySelectedItem != null && fileFillFlowContainer.Contains(CurrentlySelectedItem) && CurrentlySelectedItem.ItemName == newItem && CurrentlySelectedItem.Selected)
                return;

            currentSelectionIndex = null;

            if (newItem?.Length > 0)
            {
                if (fileFillFlowContainer.Children.ToList().Find(MatchesItem) is DrawableItem item)
                {
                    SelectItem(item);
                    return;
                }
            }

            CurrentlySelectedItem = null;

            bool MatchesItem(Drawable item) => (item as DrawableItem)?.ItemName == newItem;
        }
        public void HandleDirectoryChanged(ValueChangedEvent<string> value)
        {
            try
            {
                UpdateItemList();
                SelectedItem = GetPreviousPathDirectoryInNewPath(value.OldValue, value.NewValue);
            }
            catch
            {
                CurrentDirectory = value.OldValue; // Reset if something goes wrong
                CurrentlySelectedItem.FlashError();
            }
        }

        private void SelectItem(DrawableItem item)
        {
            scrollContainer.ScrollIntoView(item);
            item.Selected = true;
        }

        private void ForceUpdateItemList() => UpdateItemList(true);
        private void UpdateItemList(bool forceUpdate = false)
        {
            // Mainly to prevent reloading the currently displayed items if the directory is reset on exception
            if (!forceUpdate && currentlyLoadedDirectory == CurrentDirectory)
                return;

            string[] directories = GetDirectories(CurrentDirectory);
            string[] files = GetFiles(CurrentDirectory);

            fileFillFlowContainer.Clear();

            foreach (string d in directories)
                fileFillFlowContainer.Add(GetNewDrawableItem(GetIndividualItemName(d), ItemType.Directory));
            foreach (string f in files)
                fileFillFlowContainer.Add(GetNewDrawableItem(GetIndividualItemName(f), ItemType.File));

            currentlyLoadedDirectory = CurrentDirectory;

            SelectedItemBindable.TriggerChange();
            // Scrolling to currently selected item fails because the scroll container automatically scrolls to the beginning since the items were updated
        }

        private void HandleSelection(DrawableItem selectedItem)
        {
            if (CurrentlySelectedItem != null && CurrentlySelectedItem != selectedItem)
                CurrentlySelectedItem.Selected = false;
            CurrentlySelectedItem = selectedItem;
        }
        private void HandleClick(DrawableItem clickedItem)
        {
            if (!clickedItem.Selected)
                CurrentlySelectedItem = null;
        }
        private void HandleDoubleClick(DrawableItem doubleClickedItem) => PerformAction();

        private void PerformAction() => PerformActionRequested?.Invoke();

        private int GetItemCountPerPage() => (int)(DrawHeight / (DrawableItem.DefaultHeight + ItemSpacing) + 0.5);

        protected override void InitializeActionDictionary()
        {
            // Capacity is greater than the total actions to allow future improvements without *constantly* having to change the constant
            Actions = new Dictionary<FileDialogAction, Action>(20)
            {
                { FileDialogAction.NavigateUp, NavigateUp },
                { FileDialogAction.NavigateDown, NavigateDown },
                { FileDialogAction.NavigatePageUp, NavigatePageUp },
                { FileDialogAction.NavigatePageDown, NavigatePageDown },
                { FileDialogAction.NavigateToStart, NavigateToStart },
                { FileDialogAction.NavigateToEnd, NavigateToEnd },
                { FileDialogAction.NavigateToPreviousDirectory, NavigateToPreviousDirectory },
                { FileDialogAction.PerformAction, PerformAction },
                { FileDialogAction.Refresh, ForceUpdateItemList },
            };
        }

        private DrawableItem GetNewDrawableItem(string name, ItemType type) => new DrawableItem(name, type)
        {
            OnSelected = HandleSelection,
            OnClicked = HandleClick,
            OnDoubleClicked = HandleDoubleClick,
        };

        // TODO: Move those to a class, since they're copy-pasted
        private static string AddDirectorySuffix(string name) => $@"{name}\";
        private static string AggregateDirectories(string left, string right) => $@"{left}{right}";

        private static string[] AnalyzePath(string path) => path.Replace('/', '\\').Split('\\');

        private static string FixDirectoryPath(string dirPath)
        {
            string result = dirPath.Replace('/', '\\');
            if (!result.EndsWith('\\'))
                result += '\\';
            return result;
        }

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
            List<string> result = new List<string>();
            int index = -1;
            while (++index < splitNew.Length)
                if (splitPrevious[index] != splitNew[index])
                    break;
            return splitPrevious[index];
        }
        private static string GetIndividualItemName(string path) => path.Replace('/', '\\').Split('\\').Last();

        private static ItemType DetermineItemType(string path)
        {
            if (path.EndsWith('\\') || IsNullOrWhiteSpace(GetExtension(path)))
                return ItemType.Directory;
            return ItemType.File;
        }
    }
}
