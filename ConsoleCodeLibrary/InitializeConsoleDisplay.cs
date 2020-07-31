using System;

using System.Runtime.InteropServices;





namespace ConsoleCodeLibrary
{
    class InitializeConsoleDisplay
    {
        public static int[] SetDisplayAndFontSize()
        {
            SetFontAndSize();
            int [] consoleSize = Maximize();
            return consoleSize;
        }


        private static int[] Maximize()
        {
            int[] consoleSize = new int[2];
            consoleSize[0] = Console.LargestWindowWidth;
            consoleSize[1] = Console.LargestWindowHeight;
            Console.SetWindowSize(consoleSize[0], consoleSize[1]);
            Console.SetBufferSize(consoleSize[0], consoleSize[1]);
            return consoleSize;
        }


        [DllImport("kernel32.dll", SetLastError = true)]
        static extern Int32 SetCurrentConsoleFontEx(
            IntPtr ConsoleOutput,
            bool MaximumWindow,
            ref CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx);

        private enum StdHandle
        {
            OutputHandle = -11
        }

        [DllImport("kernel32")]
        private static extern IntPtr GetStdHandle(StdHandle index);

        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        private static void SetFontAndSize()
        {
            Console.CursorVisible = false;
            CONSOLE_FONT_INFO_EX ConsoleFontInfo = new CONSOLE_FONT_INFO_EX();
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            ConsoleFontInfo.cbSize = (uint)Marshal.SizeOf(ConsoleFontInfo);
            ConsoleFontInfo.FaceName = "Source Code Pro";
            ConsoleFontInfo.dwFontSize.X = 10;
            ConsoleFontInfo.dwFontSize.Y = 20;

            SetCurrentConsoleFontEx(GetStdHandle(StdHandle.OutputHandle), false, ref ConsoleFontInfo);
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;

            public COORD(short X, short Y)
            {
                this.X = X;
                this.Y = Y;
            }
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CONSOLE_FONT_INFO_EX
        {
            public uint cbSize;
            public uint nFont;
            public COORD dwFontSize;
            public int FontFamily;
            public int FontWeight;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] // Edit sizeconst if the font name is too big
            public string FaceName;
        }
    }
}
