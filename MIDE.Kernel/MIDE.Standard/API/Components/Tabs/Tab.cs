﻿using System.Linq;
using MIDE.Helpers;
using MIDE.API.Commands;
using System.Collections.Generic;

namespace MIDE.API.Components
{
    /// <summary>
    /// A tab item used to contain a group of components
    /// </summary>
    public abstract class Tab : LayoutContainer
    {
        private string header;
        protected LinkedList<LayoutComponent> registeredComponents;

        public bool AllowDuplicates { get; }
        public string Header
        {
            get => header;
            set
            {
                if (value == header)
                    return;
                header = value;
                OnPropertyChanged(nameof(Header));
            }
        }
        public TabSection ParentSection { get; internal set; }
        public Toolbar TabToolbar { get; }
        public ICommand CloseCommand { get; set; }

        private Tab(string id, Toolbar toolbar, bool allowDuplicates) : base(id)
        {
            AllowDuplicates = allowDuplicates;
            Header = FormatId();
            TabToolbar.Parent = this;
            TabToolbar = toolbar ?? new Toolbar("toolbar");
            registeredComponents = new LinkedList<LayoutComponent>();
            registeredComponents.AddLast(TabToolbar);
        }
        public Tab(string id, bool allowDuplicates = false) : this(id, null, allowDuplicates) { }

        protected override void AddChild_Impl(LayoutComponent component)
        {
            registeredComponents.AddLast(component);
        }
        protected override void RemoveChild_Impl(string id)
        {
            var node = registeredComponents.Find(component => component.Id == id);
            if (node == null)
                return;
            registeredComponents.Remove(node);
        }
        protected override void RemoveChild_Impl(LayoutComponent component)
        {
            registeredComponents.Remove(component);
        }

        public override bool Contains(string id) => registeredComponents.FirstOrDefault(component => component.Id == id) != null;
        public override LayoutComponent Find(string id) => registeredComponents.FirstOrDefault(component => component.Id == id);

        protected override LayoutComponent CloneInternal(string id)
        {
            Tab clone = Create(id, (Toolbar)TabToolbar.Clone(), AllowDuplicates);
            clone.header = header;
            clone.ParentSection = null;
            clone.CloseCommand = null;
            return clone;
        }

        protected abstract Tab Create(string id, Toolbar toolbar, bool allowDuplicates);
    }
}