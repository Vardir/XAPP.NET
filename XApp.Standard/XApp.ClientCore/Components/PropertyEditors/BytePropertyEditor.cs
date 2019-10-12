﻿namespace Vardirsoft.XApp.Components.PropertyEditors
{
    /// <summary>
    /// A basic byte property editor
    /// </summary>
    public class BytePropertyEditor : BasePropertyEditor<byte>
    {
        public BytePropertyEditor(string id, bool isReadonly = false) : base(id, isReadonly)
        {

        }

        protected override BasePropertyEditor<byte> Create(string id, bool isReadonly)
        {
            var clone = new BytePropertyEditor(id, isReadonly);

            return clone;
        }
    }
}
