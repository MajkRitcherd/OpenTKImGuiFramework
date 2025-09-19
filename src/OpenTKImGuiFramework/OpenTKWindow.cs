using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
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
        /// <param name="useImGuiUI">Whether or not to use ImGui UI.</param>
        public OpenTKWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, bool useImGuiUI = false)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _useUI = useImGuiUI;

            if (_useUI)
                ImGuiUI = new ImGuiUI(this);

#if DEBUG
            GL.DebugMessageCallback(WindowDebugProcedure, IntPtr.Zero);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);
#endif
        }

        /// <summary>
        /// Action to run during <see cref="OnLoad"/> method.
        /// </summary>
        public event Action? OnLoadAction;

        /// <summary>
        /// Action to run during <see cref="OnRenderFrame(FrameEventArgs)"/> method.
        /// </summary>
        public event Action<FrameEventArgs>? OnRenderFrameAction;

        /// <summary>
        /// Action to run during <see cref="OnResize(ResizeEventArgs)"/> method.
        /// </summary>
        public event Action<ResizeEventArgs>? OnResizeFrameAction;

        /// <summary>
        /// Action to run during <see cref="OnUnload"/> method.
        /// </summary>
        public event Action? OnUnloadAction;

        /// <summary>
        /// Action to run during <see cref="OnUpdateFrame(FrameEventArgs)"/> method.
        /// </summary>
        public event Action<FrameEventArgs>? OnUpdateFrameAction;

        /// <summary>
        /// Gets ImGui UI.
        /// </summary>
        public ImGuiUI? ImGuiUI { get; private set; }

        public override void Dispose()
        {
            ImGuiUI?.Dispose();
            base.Dispose();
        }

        /// <inheritdoc/>
        protected override void OnLoad()
        {
            base.OnLoad();
            OnLoadAction?.Invoke();
        }

        /// <inheritdoc/>
        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);
            OnRenderFrameAction?.Invoke(args);
            SwapBuffers();
        }

        /// <inheritdoc/>
        protected override void OnResize(ResizeEventArgs args)
        {
            base.OnResize(args);
            OnResizeFrameAction?.Invoke(args);
        }

        /// <inheritdoc/>
        protected override void OnUnload()
        {
            base.OnUnload();
            OnUnloadAction?.Invoke();
        }

        /// <inheritdoc/>
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            OnUpdateFrameAction?.Invoke(args);
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
    }
}