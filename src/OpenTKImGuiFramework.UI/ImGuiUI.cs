using Dear_ImGui_Sample.Backends;
using ImGuiNET;
using OpenTK.Windowing.Desktop;

namespace OpenTKImGuiFramework.UI
{
    /// <summary>
    /// User Interface using ImGui.
    /// </summary>
    public class ImGuiUI : IDisposable
    {
        private ImGuiIOPtr _imGuiIO;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiUI"/> class.
        /// </summary>
        /// <param name="nativeWindow">Window to bind UI with.</param>
        public ImGuiUI(NativeWindow nativeWindow)
        {
            ImGui.CreateContext();
            _imGuiIO = ImGui.GetIO();

            ImGuiInit(nativeWindow);
        }

        /// <summary>
        /// Begins rendering ImGui.
        /// </summary>
        /// <param name="nativeWindow">Native window.</param>
        public void BeginRender(NativeWindow nativeWindow)
        {
            ImguiImplOpenGL3.NewFrame();
            ImguiImplOpenTK4.NewFrame();
            ImGui.NewFrame();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            ImguiImplOpenGL3.Shutdown();
            ImguiImplOpenTK4.Shutdown();
        }

        /// <summary>
        /// Ends rendering of ImGui.
        /// </summary>
        /// <param name="nativeWindow">Native window.</param>
        public void EndRender(NativeWindow nativeWindow)
        {
            ImGui.Render();
            ImguiImplOpenGL3.RenderDrawData(ImGui.GetDrawData());

            if (ImGui.GetIO().ConfigFlags.HasFlag(ImGuiConfigFlags.ViewportsEnable))
            {
                ImGui.UpdatePlatformWindows();
                ImGui.RenderPlatformWindowsDefault();
                nativeWindow.Context.MakeCurrent();
            }
        }

        /// <summary>
        /// Initializes the ImGui.
        /// </summary>
        /// <param name="nativeWindow">Window to bind UI with.</param>
        private void ImGuiInit(NativeWindow nativeWindow)
        {
            ImGui.CreateContext();
            _imGuiIO.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            _imGuiIO.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
            _imGuiIO.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            _imGuiIO.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

            ImGui.StyleColorsDark();

            ImGuiStylePtr style = ImGui.GetStyle();
            if ((_imGuiIO.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
            {
                style.WindowRounding = 0.0f;
                style.Colors[(int)ImGuiCol.WindowBg].W = 1.0f;
            }

            ImguiImplOpenTK4.Init(nativeWindow);
            ImguiImplOpenGL3.Init();
        }
    }
}