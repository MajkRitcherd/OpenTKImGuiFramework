using OpenTK.Windowing.Desktop;
using OpenTKImGuiFramework.Core;

namespace BasicDemo
{
    internal class BasicDemoApp
    {
        internal static void Main()
        {
            var gameWindowSettings = GameWindowSettings.Default;
            var nativeWindowSettings = new NativeWindowSettings()
            {
                ClientSize = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "Basic demo app"
            };

            using (var window = new OpenTKWindow(gameWindowSettings, nativeWindowSettings, true))
            {
                window.Run();
            }
        }
    }
}
