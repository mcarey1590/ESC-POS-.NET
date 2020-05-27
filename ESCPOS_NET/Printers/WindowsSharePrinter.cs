using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace ESCPOS_NET
{
    public class WindowsSharePrinter : BasePrinter
    {
        private readonly FileStream _file;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);
        private SafeFileHandle handleValue = null;

        public WindowsSharePrinter(string filePath) : base()
        {
            handleValue = CreateFile(filePath, FileAccess.Write, 0, IntPtr.Zero, FileMode.OpenOrCreate, 0, IntPtr.Zero);

            _file = new FileStream(handleValue, FileAccess.ReadWrite);
            Writer = new BinaryWriter(_file);
            Reader = new BinaryReader(_file);
        }

        ~WindowsSharePrinter()
        {
            Dispose(false);
        }

        protected override void OverridableDispose()
        {
            _file?.Close();
            _file?.Dispose();
            handleValue.Close();
            handleValue.Dispose();
        }
    }
}
