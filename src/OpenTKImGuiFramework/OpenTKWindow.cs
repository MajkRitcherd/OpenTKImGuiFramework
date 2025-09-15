using System.Runtime.InteropServices;
using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKImGuiFramework.UI;

namespace OpenTKImGuiFramework.Core
{
    public class OpenTKWindow : GameWindow
    {
        private readonly bool _useUI;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenTKWindow"/> class.
        /// </summary>
        /// <param name="gameWindowSettings">Settings of game window.</param>
        /// <param name="nativeWindowSettings">Settings of a window.</param>
        /// <param name="useUI">Whether or not to use ImGui UI.</param>
        public OpenTKWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, bool useUI = false)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _useUI = useUI;

            GL.DebugMessageCallback(WindowDebugProcedure, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            if (_useUI)
                InitUI();
        }

        public override void Dispose()
        {
            ImGuiUI?.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Gets ImGui UI.
        /// </summary>
        public ImGuiUI? ImGuiUI { get; private set; }

        /// <inheritdoc/>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            RenderUI();
            SwapBuffers();
        }

        /// <summary>
        /// Outputs debug messages from GL to the console.
        /// </summary>
        /// <param name="source">Debug source type.</param>
        /// <param name="type">Debug type.</param>
        /// <param name="id">ID.</param>
        /// <param name="severity">Message severity.</param>
        /// <param name="length">Message length.</param>
        /// <param name="messagePtr">Message pointer.</param>
        /// <param name="userParam">User parameter.</param>
        private static void WindowDebugProcedure(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr messagePtr, IntPtr userParam)
        {
            string message = Marshal.PtrToStringAnsi(messagePtr, length);
            var showMessage = source switch
            {
                DebugSource.DebugSourceApplication => false,
                _ => true,
            };

            if (showMessage)
            {
                switch (severity)
                {
                    case DebugSeverity.DontCare:
                        Console.WriteLine($"[DontCare] [{source}] {message}");
                        break;

                    case DebugSeverity.DebugSeverityNotification:
                        break;

                    case DebugSeverity.DebugSeverityHigh:
                        Console.Error.WriteLine($"Error: [{source}] {message}");
                        break;

                    case DebugSeverity.DebugSeverityMedium:
                        Console.WriteLine($"Warning: [{source}] {message}");
                        break;

                    case DebugSeverity.DebugSeverityLow:
                        Console.WriteLine($"Info: [{source}] {message}");
                        break;

                    default:
                        Console.WriteLine($"[default] [{source}] {message}");
                        break;
                }
            }
        }

        /// <summary>
        /// Initializes an ImGui UI.
        /// </summary>
        private void InitUI()
        {
            ImGuiUI = new ImGuiUI(this);
        }

        /// <summary>
        /// Renders the ImGui UI to the window.
        /// </summary>
        private void RenderUI()
        {
            if (!_useUI)
                return;

            ImguiImplOpenGL3.NewFrame();
            ImguiImplOpenTK4.NewFrame();
            ImGui.NewFrame();

            ImGui.DockSpaceOverViewport();

            ImGui.ShowDemoWindow();

            ImGui.Render();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());
            GL.Viewport(0, 0, FramebufferSize.X, FramebufferSize.Y);
            GL.ClearColor(new Color4(0, 32, 48, 255));

            if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable))
            {
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault();
                Context.MakeCurrent();
            }
        }
    }
}