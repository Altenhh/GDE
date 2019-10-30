//Code copied from https://github.com/ppy/osu-framework/pull/2255

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;

namespace GDE.App.Main.UI
{
    public abstract class BreadcrumbNavigation<T> : CompositeDrawable
    {
        private readonly FillFlowContainer<Breadcrumb> fillFlowContainer;
        private readonly BindableList<T> items = new BindableList<T>();

        protected BreadcrumbNavigation()
        {
            fillFlowContainer = CreateAndAddFillFlowContainer();

            items.ItemsAdded += ItemsChanged;
            items.ItemsRemoved += ItemsChanged;
        }

        /// <summary>The items displayed in the breadcrumb navigation.</summary>
        public BindableList<T> Items
        {
            get => items;
            set => items.BindTo(value);
        }

        public event Action<T> BreadcrumbClicked;

        protected virtual void ItemsChanged(IEnumerable<T> changeset)
        {
            fillFlowContainer.Clear();

            if (items.Count == 0)
                return;

            fillFlowContainer.AddRange(items.Select(val =>
            {
                var breadcrumb = CreateBreadcrumb(val);
                breadcrumb.Selected += HandleBreadcrumbSelected;
                return breadcrumb;
            }));
        }

        /// <summary>
        ///     Override this method for customising the design of the breadcrumb.
        ///     remember to set
        ///     <code>
        ///    AutoSizeAxes = Axes.X,
        ///    RelativeSizeAxes = Axes.Y,
        /// </code>
        /// </summary>
        /// <param name="value">The value that is supposed to be written in the breadcrumb.</param>
        protected abstract Breadcrumb CreateBreadcrumb(T value);

        /// <summary>Creates and adds the FillFlowContainer that contains all breadcrumbs.</summary>
        protected abstract FillFlowContainer<Breadcrumb> CreateAndAddFillFlowContainer();

        private void HandleBreadcrumbSelected(Breadcrumb breadcrumb)
        {
            UpdateItems(fillFlowContainer.Children.ToList().IndexOf(breadcrumb));
            BreadcrumbClicked?.Invoke(breadcrumb.Value);
        }

        /// <summary>Truncates the items down to the parameter newIndex.</summary>
        /// <param name="newIndex">The index where everything after will get removed.</param>
        private void UpdateItems(int newIndex)
        {
            if (newIndex > Items.Count - 1)
                throw new IndexOutOfRangeException($"Could not find an appropriate item for the index {newIndex}");
            if (newIndex < 0)
                throw new IndexOutOfRangeException("The index can not be below 0.");

            if (newIndex + 1 == Items.Count)
                return;

            Items.RemoveRange(newIndex + 1, Items.Count - newIndex - 1);
        }

        protected abstract class Breadcrumb : CompositeDrawable
        {
            protected Breadcrumb(T value)
            {
                Value = value;
            }

            public T Value { get; }

            public event Action<Breadcrumb> Selected;

            protected override bool OnClick(ClickEvent e)
            {
                Selected?.Invoke(this);
                return true;
            }
        }
    }
}