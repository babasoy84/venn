using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Venn.Client.Services
{
    public class BackgroundChecker
    {
        // Windows API kullanarak uygulamanın arka planda olup olmadığını kontrol etmek için gerekli bildirimler
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Uygulamanın arka planda olup olmadığını kontrol eden metot
        public static bool IsApplicationInBackground()
        {
            // Aktif pencerenin bilgilerini al
            IntPtr foregroundWindowHandle = GetForegroundWindow();
            RECT foregroundWindowRect;
            GetWindowRect(foregroundWindowHandle, out foregroundWindowRect);

            // Aktif pencerenin boyutlarına göre, eğer boyutları sıfırsa, uygulama arka planda demektir.
            if ((foregroundWindowRect.Left == 0 && foregroundWindowRect.Top == 0) &&
                (foregroundWindowRect.Right == 0 && foregroundWindowRect.Bottom == 0))
            {
                return true; // Uygulama arka planda
            }
            else
            {
                return false; // Uygulama ön planda
            }
        }
    }
}
