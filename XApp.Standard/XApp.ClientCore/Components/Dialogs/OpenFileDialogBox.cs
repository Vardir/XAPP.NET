﻿using System.Collections.Generic;
using System.Diagnostics;

using Vardirsoft.XApp.IoC;
using Vardirsoft.XApp.API;
using Vardirsoft.XApp.Bindings;
using Vardirsoft.XApp.Components.Complex;

namespace Vardirsoft.XApp.Components
{
    /// <summary>
    /// Allows selecting a file
    /// </summary>
    public sealed class OpenFileDialogBox : BaseDialogBox<string>
    {
        private string _directory;
        private readonly DialogResult[] _validationIgnored = { DialogResult.Cancel };

        public string Filter { get; }

        public string Directory
        {
            [DebuggerStepThrough]
            get => _directory;
            set
            {
                if (value == _directory)
                    return;

                _directory = value;
                FileSystemView.Show(_directory);
            }
        }
        
        public TextBox SelectedFile { get; private set; }
        
        public DialogButton OkButton { get; private set; }
        
        public DialogButton CancelButton { get; private set; }
        
        public FileSystemTreeView FileSystemView { get; private set; }

        public OpenFileDialogBox(string title, string filter) : base(title, DialogMode.Modal)
        {
            Filter = filter;
            InitializeComponents();
            Directory = @"\";
        }

        private void InitializeComponents()
        {
            FileSystemView = new FileSystemTreeView("file-system-view");
            FileSystemView.Generator = directoryItem => new FileSystemTreeViewItem(directoryItem);
            SelectedFile = new TextBox("selected-file");
            OkButton = new DialogButton(this, DialogResult.Ok);
            CancelButton = new DialogButton(this, DialogResult.Cancel);

            var selectedFileBinding = new ObjectBinding<FileSystemTreeView, TextBox>(FileSystemView, SelectedFile);
            selectedFileBinding.BindingKind = BindingKind.OneWay;
            selectedFileBinding.Bind(fsv => fsv.SelectedItem, tb => tb.Text,
                                     item => 
                                     {
                                         if (item is null || !((FileSystemTreeViewItem)item).ObjectClass.IsFile)
                                             return null;
                                         
                                         return ((FileSystemTreeViewItem)item).FullPath;
                                     }, null);
        }

        public override string GetData() => SelectedFile.Text;
        protected override bool Validate() => IoCContainer.Resolve<IFileManager>().FileExists(SelectedFile.Text);
        protected override IEnumerable<DialogResult> GetValidationIgnoredResults() => _validationIgnored;
    }
}