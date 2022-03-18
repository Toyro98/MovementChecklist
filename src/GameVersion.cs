using System.Diagnostics;
using System.Linq;

namespace MovementChecklist
{
    public class GameVersion
    {
        public static Version Current { get; private set; }
        public static string MemorySize { get; private set; }

        public string DetectGameVersion()
        {
            Process process = Process.GetProcessesByName("MirrorsEdge").FirstOrDefault();

            if (process == null)
            {
                return null;
            }

            try
            {
                switch (process.MainModule.ModuleMemorySize)
                {
                    case 32976896:
                        Current = Version.Steam;
                        break;

                    case 42889216:
                        Current = Version.Origin;
                        break;

                    case 32632832:
                        Current = Version.GoG;
                        break;

                    case 60298504:
                        Current = Version.Reloaded;
                        break;

                    case 43794432:
                        Current = Version.OriginDLC;
                        break;

                    case 42717184:
                        Current = Version.OriginAsia;
                        break;

                    case 42876928:
                        Current = Version.Dvd;
                        break;

                    default:
                        Current = Version.Unknown;
                        break;
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                return null;
            }

            MemorySize = process.MainModule.ModuleMemorySize.ToString();
            return MemorySize;
        }
    }

    public enum Version
    {
        Unknown,
        Steam,
        GoG,
        Origin,
        Reloaded,
        OriginDLC,
        OriginAsia,
        Dvd

        // GoG = Steam
        // Dvd = Reloaded
    }
}
