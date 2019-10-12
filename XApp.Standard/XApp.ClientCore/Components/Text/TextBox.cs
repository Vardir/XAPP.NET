﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

using Vardirsoft.XApp.API.Validations;

namespace Vardirsoft.XApp.Components
{
    public class TextBox : TextComponent
    {
        private bool hasErrors;
        private bool isReadonly;

        protected ObservableCollection<ValidationError> validationErrors;

        public bool HasErrors
        {
            get => hasErrors;
            private set => SetWithNotify(ref hasErrors, value);
        }
        public bool IsReadonly
        {
            get => isReadonly;
            set => SetWithNotify(ref isReadonly, value);
        }
        public override string Text
        {
            get => text ?? Default;
            set
            {
                if (text == value || IsReadonly || !IsEnabled)
                    return;

                HasErrors = false;
                validationErrors.Clear();
                Validations.ForEach(v => v.Validate(value, nameof(Text), ValidationErrors));

                if (ValidationErrors.Count == 0)
                {
                    text = value;
                    NotifyPropertyChanged();
                }
                else
                {
                    HasErrors = true;
                }
            }
        }
        public string Default { get; }
        public List<ValueValidation<string>> Validations { get; }
        public ReadOnlyObservableCollection<ValidationError> ValidationErrors { get; }

        public TextBox(string id, string defaultValue = null) : base(id)
        {
            Default = defaultValue;
            Validations = new List<ValueValidation<string>>();
            validationErrors = new ObservableCollection<ValidationError>();
            ValidationErrors = new ReadOnlyObservableCollection<ValidationError>(validationErrors);
        }

        public void Clear()
        {
            if (IsEnabled)
                Text = Default;
        }

        protected override LayoutComponent CloneInternal(string id)
        {
            var clone = new TextBox(id, Default);
            clone.isReadonly = isReadonly;
            clone.text = text;

            return clone;
        }

        protected virtual TextBox Create(string id) => new TextBox(id, Default);
    }
}