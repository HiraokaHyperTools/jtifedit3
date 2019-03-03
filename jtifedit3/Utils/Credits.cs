using jtifedit3.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace jtifedit3.Utils {
    class Credits {
        public static IEnumerable<Credit> List() {
            yield return new Credit {
                name = "Microsoft Visual Studio 2017",
                url = "https://visualstudio.microsoft.com/ja/",
            };
            yield return new Credit {
                name = "Microsoft .NET Framework Version 4.0",
                url = "https://docs.microsoft.com/ja-jp/dotnet/framework/get-started/",
            };
            yield return new Credit {
                name = "NSIS (Nullsoft Scriptable Install System)",
                url = "http://nsis.sourceforge.net/Main_Page",
                license = "NSIS License",
            };
            yield return new Credit {
                name = "twaindotnet",
                url = "https://github.com/tmyroadctfig/twaindotnet",
                license = "MIT",
            };
            yield return new Credit {
                name = "WIA Scanner in C# Windows Forms",
                url = "https://www.codeproject.com/Tips/792316/WIA-Scanner-in-Csharp-Windows-Forms",
                license = "The Code Project Open License (CPOL)",
            };
#if false
            yield return new Credit {
                name = "",
                url = "",
            };
#endif
        }
    }
}
