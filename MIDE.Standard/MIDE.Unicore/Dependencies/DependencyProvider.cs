﻿using MIDE.Logging;
using MIDE.Services;

namespace MIDE.Dependencies
{
    public static class DependencyProvider
    {
        public static IFileManager FileManager { get; set; }
        public static Logger Logger { get; set; }
    }
}