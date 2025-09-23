using System.Numerics;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKImGuiFramework.Core;
using OpenTKImGuiFramework.UI;

namespace BasicDemo
{
    /// <summary>
    /// Simple example using <see cref="OpenTKWindow"/> to render background and ImGui UI (Dockable demo window).
    /// </summary>
    internal class SimpleWindow : OpenTKWindow
    {
        private ImGuiUI _imGui;

        /// <summary>
        /// Basic implementation of the <see cref="OpenTKWindow"/>.
        /// </summary>
        /// <param name="gameWindowSettings">Game window settings.</param>
        /// <param name="nativeWindowSettings">Native window settings.</param>
        public SimpleWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            _imGui = new ImGuiUI(this);
            OnRenderFrameAction += RenderAction;
        }

        /// <summary>
        /// Action to render window scene. <br />
        /// Called from <see cref="OpenTKWindow.OnRenderFrame(FrameEventArgs)"/> method and registered in the constructor of this class.
        /// </summary>
        /// <param name="args"></param>
        private void RenderAction(FrameEventArgs args)
        {
            RenderScene();
            RenderUI();
        }

        /// <summary>
        /// Custom logic to render scene into the window.
        /// </summary>
        private void RenderScene()
        {
            // Just clear the screen so ImGui is not in the scene after move
            GL.ClearColor(0.15f, 0.15f, 0.15f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        /// <summary>
        /// Custom logic to render UI. <br />
        /// In this example using ImGui demo with docking.
        /// </summary>
        private void RenderUI()
        {
            _imGui.BeginRender(this);

            var dockspaceID = ImGui.GetID("DockSpace");
            var windowFlags = ImGuiWindowFlags.NoDocking;
            windowFlags |= ImGuiWindowFlags.NoTitleBar
                | ImGuiWindowFlags.NoCollapse
                | ImGuiWindowFlags.NoResize
                | ImGuiWindowFlags.NoMove
                | ImGuiWindowFlags.NoBringToFrontOnFocus
                | ImGuiWindowFlags.NoNavFocus
                | ImGuiWindowFlags.NoBackground;

            var viewport = ImGui.GetMainViewport();
            ImGui.SetNextWindowPos(viewport.Pos);
            ImGui.SetNextWindowSize(viewport.Size);
            ImGui.SetNextWindowViewport(viewport.ID);

            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new Vector2(0.0f, 0.0f));
            ImGui.Begin("DockSpace", windowFlags);
            ImGui.PopStyleVar(3);

            ImGui.DockSpace(dockspaceID, new Vector2(0.0f, 0.0f), ImGuiDockNodeFlags.PassthruCentralNode);
            ImGui.End();
            ImGui.ShowDemoWindow();

            _imGui.EndRender(this);
        }
    }
}