using System;
using System.Runtime.InteropServices;

namespace SourceSkyBoxer.Net.Core
{
    public enum Platform
    {
        Unknown,
        Windows,
        Linux,
        Android,
        MacOS,
        IOS
    }

    public abstract class LibraryContainer
    {
        public static Platform Platform { get; set; } = RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
            ? RuntimeInformation.IsOSPlatform(OSPlatform.Create("ANDROID"))
                ? Platform.Android
                : Platform.Linux
            : RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Platform.Windows
            : RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
                ? RuntimeInformation.IsOSPlatform(OSPlatform.Create("IOS"))
                    ? Platform.IOS 
                    : Platform.MacOS
            : Platform.Unknown;

        public string GetLibraryPath => Platform switch
        {
            Platform.Unknown => ThrowInvalidPlatform(),
            Platform.Windows => Windows,
            Platform.Linux => Linux,
            Platform.Android => Android,
            Platform.MacOS => MacOS,
            Platform.IOS => IOS,
            _ => ThrowInvalidPlatform()
        };

        public abstract string Windows { get; }

        public abstract string Linux { get; }

        public abstract string MacOS { get; }

        public virtual string Android => Linux;

        public virtual string IOS => MacOS;

        private static string ThrowInvalidPlatform()
        {
            throw new PlatformNotSupportedException("Invalid/unsupported operating system.");
        }
    }
}
