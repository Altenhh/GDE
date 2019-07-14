﻿using GDE.App.Main.Containers.KeyBindingContainers;
using GDE.App.Main.Panels;
using GDE.App.Main.Screens.Edit.Components;
using GDE.App.Main.Screens.Edit.Components.IDMigration;
using GDE.App.Main.UI;
using GDEdit.Application.Editor;
using GDEdit.Utilities.Objects.General;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;
using System;
using System.Collections.Generic;
using static GDE.App.Main.Colors.GDEColors;
using static GDEdit.Utilities.Objects.General.SourceTargetRange;

namespace GDE.App.Main.Screens.Edit
{
    public class IDMigrationPanel : Panel
    {
        private static Color4 greenEnabledColor = FromHex("246c48");
        private static Color4 redEnabledColor = FromHex("6c2424");
        private static Color4 grayEnabledColor = FromHex("242424");

        private NumberTextBox sourceFrom;
        private NumberTextBox sourceTo;
        private NumberTextBox targetFrom;
        private NumberTextBox targetTo;

        private FadeButton performAction;
        private FadeButton createStep;
        private FadeButton removeSteps;
        private FadeButton cloneSteps;
        private FadeButton selectAll;
        private FadeButton deselectAll;
        private FadeButton loadSteps;
        private FadeButton saveSteps;

        private IDMigrationTabControl tabControl;
        private IDMigrationStepList[] stepLists = new IDMigrationStepList[4];
        private IDMigrationStepList currentStepList;

        private Editor editor;

        /// <summary>The common <seealso cref="SourceTargetRange"/> of the currently selected ID migration steps.</summary>
        public readonly Bindable<SourceTargetRange> CommonIDMigrationStep = new Bindable<SourceTargetRange>();

        public IDMigrationStepList CurrentStepList
        {
            get => currentStepList;
            set
            {
                var previous = currentStepList;

                previous.FadeTo(0, 200);
                previous.StepSelected = null;
                previous.StepDeselected = null;
                previous.SelectionChanged = null;
                
                // Why does this not work?
                value.FadeTo(1, 200);
                value.StepSelected = HandleStepSelected;
                value.StepDeselected = HandleStepDeselected;
                value.SelectionChanged = HandleSelectionChanged;

                CommonIDMigrationStep.UnbindAll();
                CommonIDMigrationStep.BindTo(value.CommonIDMigrationStep);
                CommonIDMigrationStep.ValueChanged += CommonIDMigrationStepChanged;
                CommonIDMigrationStep.TriggerChange();

                editor.SelectedIDMigrationMode = value.IDMigrationMode;
                currentStepList = value;

                // Since the step list has been changed, technically the selection has changed too; so triggering this is not as hacky as it seems
                HandleSelectionChanged();
            }
        }

        public IDMigrationPanel(Editor e)
        {
            editor = e;

            Size = new Vector2(700, 650);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            CornerRadius = 10;
            Masking = true;

            for (int i = 0; i < 4; i++)
                stepLists[i] = GetNewStepList(editor, (IDMigrationMode)i);

            AddInternal(new Container
            {
                RelativeSizeAxes = Axes.Both,
                CornerRadius = 10,
                Masking = true,
                Children = new Drawable[]
                {
                    tabControl = new IDMigrationTabControl(),
                    new Container
                    {
                        Size = new Vector2(700, 650 - IDMigrationTabControl.DefaultHeight),
                        Y = IDMigrationTabControl.DefaultHeight,
                        CornerRadius = 10,
                        Masking = true,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = FromHex("1a1a1a")
                            },
                            new IDMigrationActionContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Y,
                                        Width = 520,
                                        CornerRadius = 10,
                                        Masking = true,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = FromHex("111111"),
                                            },
                                            currentStepList = stepLists[0]
                                        }
                                    },
                                    new FillFlowContainer
                                    {
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        Spacing = new Vector2(5),
                                        Margin = new MarginPadding { Top = 5, Left = 10, Right = 10 },
                                        RelativeSizeAxes = Axes.Y,
                                        Width = 160,
                                        Children = new Drawable[]
                                        {
                                            GetNewSpriteText("Source From"),
                                            sourceFrom = GetNewNumberTextBox(),
                                            GetNewSpriteText("Source To"),
                                            sourceTo = GetNewNumberTextBox(),
                                            GetNewSpriteText("Target From"),
                                            targetFrom = GetNewNumberTextBox(),
                                            GetNewSpriteText("Target To"),
                                            targetTo = GetNewNumberTextBox(),
                                        },
                                    },
                                    new FillFlowContainer
                                    {
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        Direction = FillDirection.Vertical,
                                        Spacing = new Vector2(10),
                                        Margin = new MarginPadding { Bottom = 10, Left = 10, Right = 10 },
                                        RelativeSizeAxes = Axes.Y,
                                        Width = 160,
                                        Children = new Drawable[]
                                        {
                                            performAction = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Margin = new MarginPadding { Top = 15 },
                                                Text = "Perform Action",
                                                EnabledColor = greenEnabledColor,
                                                Action = editor.PerformMigration,
                                            },
                                            removeSteps = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Remove Steps",
                                                EnabledColor = redEnabledColor,
                                                Action = CurrentStepList.RemoveSelectedSteps,
                                            },
                                            cloneSteps = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Clone Steps",
                                                EnabledColor = grayEnabledColor,
                                                Action = CurrentStepList.CloneSelectedSteps,
                                            },
                                            deselectAll = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Deselect All",
                                                EnabledColor = grayEnabledColor,
                                                Action = CurrentStepList.DeselectAll,
                                            },
                                            selectAll = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Select All",
                                                EnabledColor = grayEnabledColor,
                                                Action = CurrentStepList.SelectAll,
                                            },
                                            loadSteps = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Load Steps",
                                                EnabledColor = grayEnabledColor,
                                                Action = CurrentStepList.LoadSteps,
                                            },
                                            saveSteps = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Save Steps",
                                                EnabledColor = grayEnabledColor,
                                                Action = CurrentStepList.SaveSteps,
                                            },
                                            createStep = new FadeButton
                                            {
                                                Anchor = Anchor.BottomCentre,
                                                Origin = Anchor.BottomCentre,
                                                RelativeSizeAxes = Axes.X,
                                                Height = 32,
                                                Text = "Create Step",
                                                EnabledColor = greenEnabledColor,
                                                Action = CreateNewStep,
                                            },
                                        },
                                    },
                                }
                            }
                        }
                    }
                }
            });

            tabControl.TabSelected += TabChanged;

            deselectAll.Enabled.Value = false;
            cloneSteps.Enabled.Value = false;
            removeSteps.Enabled.Value = false;
            performAction.Enabled.Value = false;

            sourceFrom.NumberChanged += HandleSourceFromChanged;
            sourceTo.NumberChanged += HandleSourceToChanged;
            targetFrom.NumberChanged += HandleTargetFromChanged;
            targetTo.NumberChanged += HandleTargetToChanged;

            CommonIDMigrationStep.ValueChanged += CommonIDMigrationStepChanged;

            CurrentStepList = currentStepList; // After everything's loaded, initialize the property for things to work properly
        }

        private void CommonIDMigrationStepChanged(ValueChangedEvent<SourceTargetRange> v)
        {
            var newStep = v.NewValue;
            if (newStep != null)
            {
                newStep.SourceTargetRangeChanged += (sf, st, tf, tt) =>
                {
                    HandleStepChanged(newStep);
                    UpdateTextBoxes(newStep);
                };
                HandleStepChanged(newStep);
            }
            UpdateTextBoxes(newStep);
        }

        private void TabChanged(IDMigrationMode newMode)
        {
            CurrentStepList = stepLists[(int)newMode];
        }

        private void HandleStepChanged(SourceTargetRange newStep)
        {
            foreach (var s in CurrentStepList.SelectedSteps)
            {
                if (newStep.SourceFrom > 0)
                    s.SourceFrom = newStep.SourceFrom;
                if (newStep.SourceTo > 0)
                    s.SourceTo = newStep.SourceTo;
                if (newStep.TargetFrom > 0)
                    s.TargetFrom = newStep.TargetFrom;
            }
        }

        private void HandleSourceFromChanged(int newValue)
        {
            if (newValue > 0)
                CommonIDMigrationStep.Value.SourceFrom = newValue;
        }
        private void HandleSourceToChanged(int newValue)
        {
            if (newValue > 0 && newValue >= CommonIDMigrationStep.Value.SourceFrom)
                CommonIDMigrationStep.Value.SourceTo = newValue;
        }
        private void HandleTargetFromChanged(int newValue)
        {
            if (newValue > 0)
                CommonIDMigrationStep.Value.TargetFrom = newValue;
        }
        private void HandleTargetToChanged(int newValue)
        {
            if (newValue > 0 && newValue >= CommonIDMigrationStep.Value.TargetFrom)
                CommonIDMigrationStep.Value.TargetTo = newValue;
        }

        private void HandleStepSelected(IDMigrationStepCard card) => UpdateFadeButtonEnabledStates();
        private void HandleStepDeselected(IDMigrationStepCard card) => UpdateFadeButtonEnabledStates();
        private void HandleSelectionChanged() => UpdateFadeButtonEnabledStates();

        private void CreateNewStep()
        {
            CurrentStepList.CreateNewStep();
            performAction.Enabled.Value = true;
        }

        private void UpdateFadeButtonEnabledStates()
        {
            deselectAll.Enabled.Value = CurrentStepList.SelectedSteps.Count > 0;
            removeSteps.Enabled.Value = CurrentStepList.SelectedSteps.Count > 0;
            cloneSteps.Enabled.Value = CurrentStepList.SelectedSteps.Count > 0;
            performAction.Enabled.Value = editor.CurrentlySelectedIDMigrationSteps.Count > 0;
        }

        private void UpdateTextBoxes(SourceTargetRange range)
        {
            UpdateTextBox(sourceFrom, range?.SourceFrom);
            UpdateTextBox(sourceTo, range?.SourceTo);
            UpdateTextBox(targetFrom, range?.TargetFrom);
            UpdateTextBox(targetTo, range?.TargetTo);
        }
        private void UpdateTextBox(NumberTextBox textBox, int? newValue)
        {
            bool enabled = newValue.HasValue && newValue > -1;
            textBox.InvokeEvents = false;
            if (enabled)
                textBox.Number = newValue.Value;
            else
                textBox.Text = "";
            textBox.InvokeEvents = true;
            textBox.Enabled = enabled;
        }

        private static NumberTextBox GetNewNumberTextBox() => new NumberTextBox(false)
        {
            RelativeSizeAxes = Axes.X,
            Height = 30,
        };
        private static SpriteText GetNewSpriteText(string text) => new SpriteText
        {
            Text = text,
        };
        private static IDMigrationStepList GetNewStepList(Editor editor, IDMigrationMode mode) => new IDMigrationStepList(editor, mode)
        {
            RelativeSizeAxes = Axes.Y,
            Width = 500,
            Padding = new MarginPadding
            {
                Top = 10,
                Bottom = 10,
                //Left = 10,
                //Right = 10
            },
            Alpha = 0,
        };
    }
}