using System;
using System.Runtime.InteropServices;

namespace SourceSkyBoxer.Net.Core
{
    public abstract class LibraryLoader 
    {
        private string _preifx;
        private nint libraryhandle, _librarysymbol;

        /*
         *  Load library and prefix example "SDL_", "gtk_" etc
         */
        public LibraryLoader(string librarypath, string preifx = null)
        {
            _preifx = preifx;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                libraryhandle = Kernel32.LoadLibrary(librarypath);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                libraryhandle = Libdl.dlopen(librarypath, Libdl.RtldNow);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                libraryhandle = Libdl.dlopen(librarypath, Libdl.RtldNow);
            else
                throw new PlatformNotSupportedException("Error: Nothing uses for current platform.");
        }


        /*
         *  LoadMethod example "SDL_Init" If Load library passes with preifx "SDL_" then "Init"
         */
        public T LoadMethod<T>(string methodname)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (_preifx == null)
                    _librarysymbol = Kernel32.GetProcAddress(libraryhandle, methodname);
                else
                    _librarysymbol = Kernel32.GetProcAddress(libraryhandle, _preifx+methodname);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (_preifx == null)
                    _librarysymbol = Libdl.dlsym(libraryhandle, methodname);
                else
                    _librarysymbol = Libdl.dlsym(libraryhandle, _preifx + methodname);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (_preifx == null)
                    _librarysymbol = Libdl.dlsym(libraryhandle, methodname);
                else
                    _librarysymbol = Libdl.dlsym(libraryhandle, _preifx + methodname);
            }
            else
                throw new NotImplementedException("Error: Method or Implement doesn't find in library.");

            return Marshal.GetDelegateForFunctionPointer<T>(_librarysymbol);
        }
    }
}