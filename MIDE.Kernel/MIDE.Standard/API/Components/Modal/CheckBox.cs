﻿using System;

namespace MIDE.API.Components
{
    public class CheckBox : LayoutComponent
    {
        private bool isChecked;
        private string caption;

        public bool IsChecked
        {
            get => isChecked;
            set
            {
                if (isChecked == value || !IsEnabled)
                    return;
                isChecked = value;
                OnPropertyChanged(nameof(IsChecked));
                CheckedChanged?.Invoke(isChecked);
                OnCheckedChanged();
            }
        }
        public string Caption
        {
            get => caption;
            set
            {
                string localized = localization[value];
                if (localized == caption)
                    return;
                caption = localized;
                OnPropertyChanged(nameof(Caption));
            }
        }

        public event Action<bool> CheckedChanged;

        public CheckBox(string id) : base(id) {}

        protected override LayoutComponent CloneInternal(string id)
        {
            CheckBox clone = new CheckBox(id);
            clone.caption = caption;
            return clone;
        }

        protected virtual void OnCheckedChanged() {}
    }
}