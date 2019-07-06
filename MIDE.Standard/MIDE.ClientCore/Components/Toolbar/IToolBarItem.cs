﻿using MIDE.API;

namespace MIDE.Components
{
    public interface IToolBarItem : ICloneable<LayoutComponent>
    {
        int Order { get; }
        string Id { get; }
        string Group { get; }
    }
}