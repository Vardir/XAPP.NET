﻿namespace MIDE.API.Components.PropertyEditors
{
    /// <summary>
    /// A basic string property editor
    /// </summary>
    public class StringPropertyEditor : BasePropertyEditor<string>
    {
        public StringPropertyEditor(string id, bool isReadonly = false) : base(id, isReadonly)
        {

        }

        protected override BasePropertyEditor<string> Create(string id, bool isReadonly)
        {
            StringPropertyEditor clone = new StringPropertyEditor(id, isReadonly);
            return clone;
        }
    }
}
