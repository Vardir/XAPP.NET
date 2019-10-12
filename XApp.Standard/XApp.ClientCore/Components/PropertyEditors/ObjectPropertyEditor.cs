﻿namespace Vardirsoft.XApp.Components.PropertyEditors
{
    /// <summary>
    /// A basic object property editor
    /// </summary>
    public class ObjectPropertyEditor : BasePropertyEditor<object>
    {
        public ObjectPropertyEditor(string id, bool isReadonly = false) : base(id, isReadonly)
        {

        }

        protected override BasePropertyEditor<object> Create(string id, bool isReadonly)
        {
            var clone = new ObjectPropertyEditor(id, isReadonly);

            return clone;
        }
    }
}
