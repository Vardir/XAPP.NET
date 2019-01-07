﻿using MIDE.API.Visuals;
using MIDE.API.Commands;

namespace MIDE.API.Components
{
    public class Button : LayoutComponent
    {
        private string caption;
        private Glyph glyph;

        public string Caption
        {
            get => caption;
            set
            {
                if (value == caption)
                    return;
                caption = value;
                OnPropertyChanged(nameof(Caption));
            }
        }
        public Glyph ButtonGlyph
        {
            get => glyph;
            set
            {
                if (value == glyph)
                    return;
                glyph = value;
                OnPropertyChanged(nameof(ButtonGlyph));
            }
        }
        public ICommand PressCommand { get; set; }
        
        public Button(string id) : base(id)
        {
            Caption = FormatId();
        }

        public virtual void Press(object parameter)
        {
            if (PressCommand.CanExecute(parameter))
                PressCommand.Execute(parameter);
        }
    }
}