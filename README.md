# LibraryLoader ( It is alternaitive to partial methods by Silk.NET )
It is simple and alternative to Silk.NET without partial methods - It works as well for with-prefix or without-prefix It works fine with Net 6.0 ( No older version support ). 

I am working simple LibraryLoader without arch conflicts

Example:
You use SDL2 for 32 Bit then you need publish universal end-of-user dotnet launcher win-x86 with SDL2.dll 32 bit
PS: I find nonsense about Windows64 and Windows86 from SearchPathContainer.cs by Silk.NET

I don't need that. Because dotnet public creation can be current arch like x64, x86, arm or whatever...

I will explain you how do you create invokes with thrid-party library

```csharp
using SourceSkyBoxer.Net.Core;

namespace SourceSkyBoxer.Net.SDL2
{
    public unsafe class SDL : LibraryLoader
    {
        // Set prefix of SDL2
        protected SDL(string librarypath) : base(librarypath, "SDL_")
        {
        }

        public static SDL GetApi
        {
            get
            {
                // Load library
                return new SDL(new SDLLibraryContainer().GetLibraryPath);
            }
        }

        private delegate int init_type(uint flags);
        public bool Init(uint flags)
        {
            var method = LoadMethod<init_type>("Init");
            return Convert.ToBoolean(method(flags));
        }

        private delegate void Quit_type();
        public void Quit()
        {
            var method = LoadMethod<Quit_type>("Quit");
            if (method != null)
                method();
        }

        private delegate nint* CreateWindow_type(string title, int x, int y, int w, int h, uint flags);
        public SDL_Window* CreateWindow(string title, int x, int y, int w, int h, uint flags)
        {
            var method = LoadMethod<CreateWindow_type>("CreateWindow");
            return (SDL_Window*)method(title, x, y, w, h, flags);
        }

        private delegate void ShowWindow_type(nint* sdl_window);
        public void ShowWindow(SDL_Window* sdl_window)
        {
            var method = LoadMethod<ShowWindow_type>("ShowWindow");
            if (method != null)
                method((nint*)sdl_window);
        }
    }
}
```
And I create instance of `LibraryContainer`
```csharp
using System;
using SourceSkyBoxer.Net.Core;

namespace SourceSkyBoxer.Net.SDL2
{
    class SDLLibraryContainer : LibraryContainer
    {
        public override string Windows => "SDL2.dll";

        public override string Linux => "libSDL2.so";

        public override string MacOS => "libSDL2.dllyb";
    }
}
```
Compile and make sure without errors. And put to Console project with Net 6.0 ( do not use older version of Dotnet )

Verify this if you don't forget prefix or valid method/implement.

Example for Cairo Graphics -> cairo_ for prefix and create -> methodname = cairo_create - It is simple way with prefix and method name.

Happy coding without conficting partial methods by Silk.NET

Thanks for showing of Silk.NET
@Dylan
@Kai
