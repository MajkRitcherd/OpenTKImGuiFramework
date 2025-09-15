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
        /// <summary>
        /// Initializes a new instance of the <see cref="ImGuiUI"/> class.
        /// </summary>
        /// <param name="nativeWindow">Window to bind UI with.</param>
        public ImGuiUI(NativeWindow nativeWindow)
        {
            ImGuiInit(nativeWindow);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            ImguiImplOpenGL3.Shutdown();
            ImguiImplOpenTK4.Shutdown();
        }

        /// <summary>
        /// Initializes the ImGui.
        /// </summary>
        /// <param name="nativeWindow">Window to bind UI with.</param>
        private void ImGuiInit(NativeWindow nativeWindow)
        {
            ImGui.CreateContext();
            ImGuiIOPtr io = ImGui.GetIO();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable;

            ImGui.StyleColorsDark();

            ImGuiStylePtr style = ImGui.GetStyle();
            if ((io.ConfigFlags & ImGuiConfigFlags.ViewportsEnable) != 0)
            {
                style.WindowRounding = 0.0f;
                style.Colors[(int)ImGuiCol.WindowBg].W = 1.0f;
            }

            ImguiImplOpenTK4.Init(nativeWindow);
            ImguiImplOpenGL3.Init();
        }
    }
}