module ImGuiNET.FSharp.Styles

open System.Drawing
open System.Numerics

open ImGuiNET

let colorToVec4 (c:Color) = Vector4(float32(int c.R), float32(int c.G), float32(int c.B), float32(int c.A))

let setGreenColorScheme() = 
    let colors = ImGui.GetStyle().Colors
    colors.[int ImGuiCol.Text]                   <- Vector4(1.00f, 1.00f, 1.00f, 1.00f);
    colors.[int ImGuiCol.TextDisabled]           <- Vector4(0.50f, 0.50f, 0.50f, 1.00f);
    colors.[int ImGuiCol.WindowBg]               <- Vector4(0.06f, 0.06f, 0.06f, 0.94f);
    colors.[int ImGuiCol.ChildBg]                <- Vector4(0.00f, 0.00f, 0.00f, 0.00f);
    colors.[int ImGuiCol.PopupBg]                <- Vector4(0.08f, 0.08f, 0.08f, 0.94f);
    colors.[int ImGuiCol.Border]                 <- Vector4(0.43f, 0.43f, 0.50f, 0.50f);
    colors.[int ImGuiCol.BorderShadow]           <- Vector4(0.00f, 0.00f, 0.00f, 0.00f);
    colors.[int ImGuiCol.FrameBg]                <- Vector4(0.44f, 0.44f, 0.44f, 0.60f);
    colors.[int ImGuiCol.FrameBgHovered]         <- Vector4(0.57f, 0.57f, 0.57f, 0.70f);
    colors.[int ImGuiCol.FrameBgActive]          <- Vector4(0.76f, 0.76f, 0.76f, 0.80f);
    colors.[int ImGuiCol.TitleBg]                <- Vector4(0.04f, 0.04f, 0.04f, 1.00f);
    colors.[int ImGuiCol.TitleBgActive]          <- Vector4(0.16f, 0.16f, 0.16f, 1.00f);
    colors.[int ImGuiCol.TitleBgCollapsed]       <- Vector4(0.00f, 0.00f, 0.00f, 0.60f);
    colors.[int ImGuiCol.MenuBarBg]              <- Vector4(0.14f, 0.14f, 0.14f, 1.00f);
    colors.[int ImGuiCol.ScrollbarBg]            <- Vector4(0.02f, 0.02f, 0.02f, 0.53f);
    colors.[int ImGuiCol.ScrollbarGrab]          <- Vector4(0.31f, 0.31f, 0.31f, 1.00f);
    colors.[int ImGuiCol.ScrollbarGrabHovered]   <- Vector4(0.41f, 0.41f, 0.41f, 1.00f);
    colors.[int ImGuiCol.ScrollbarGrabActive]    <- Vector4(0.51f, 0.51f, 0.51f, 1.00f);
    colors.[int ImGuiCol.CheckMark]              <- Vector4(0.13f, 0.75f, 0.55f, 0.80f);
    colors.[int ImGuiCol.SliderGrab]             <- Vector4(0.13f, 0.75f, 0.75f, 0.80f);
    colors.[int ImGuiCol.SliderGrabActive]       <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Button]                 <- Vector4(0.13f, 0.75f, 0.55f, 0.40f);
    colors.[int ImGuiCol.ButtonHovered]          <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.ButtonActive]           <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Header]                 <- Vector4(0.13f, 0.75f, 0.55f, 0.40f);
    colors.[int ImGuiCol.HeaderHovered]          <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.HeaderActive]           <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Separator]              <- Vector4(0.13f, 0.75f, 0.55f, 0.40f);
    colors.[int ImGuiCol.SeparatorHovered]       <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.SeparatorActive]        <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.ResizeGrip]             <- Vector4(0.13f, 0.75f, 0.55f, 0.40f);
    colors.[int ImGuiCol.ResizeGripHovered]      <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.ResizeGripActive]       <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Tab]                    <- Vector4(0.13f, 0.75f, 0.55f, 0.80f);
    colors.[int ImGuiCol.TabHovered]             <- Vector4(0.13f, 0.75f, 0.75f, 0.80f);
    colors.[int ImGuiCol.TabActive]              <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.TabUnfocused]           <- Vector4(0.18f, 0.18f, 0.18f, 1.00f);
    colors.[int ImGuiCol.TabUnfocusedActive]     <- Vector4(0.36f, 0.36f, 0.36f, 0.54f);
    colors.[int ImGuiCol.DockingPreview]         <- Vector4(0.13f, 0.75f, 0.55f, 0.80f);
    colors.[int ImGuiCol.DockingEmptyBg]         <- Vector4(0.13f, 0.13f, 0.13f, 0.80f);
    colors.[int ImGuiCol.PlotLines]              <- Vector4(0.61f, 0.61f, 0.61f, 1.00f);
    colors.[int ImGuiCol.PlotLinesHovered]       <- Vector4(1.00f, 0.43f, 0.35f, 1.00f);
    colors.[int ImGuiCol.PlotHistogram]          <- Vector4(0.90f, 0.70f, 0.00f, 1.00f);
    colors.[int ImGuiCol.PlotHistogramHovered]   <- Vector4(1.00f, 0.60f, 0.00f, 1.00f);
(*        colors.[int ImGuiCol.TableHeaderBg]          <- Vector4(0.19f, 0.19f, 0.20f, 1.00f);
    colors.[int ImGuiCol.TableBorderStrong]      <- Vector4(0.31f, 0.31f, 0.35f, 1.00f);
    colors.[int ImGuiCol.TableBorderLight]       <- Vector4(0.23f, 0.23f, 0.25f, 1.00f);
    colors.[int ImGuiCol.TableRowBg]             <- Vector4(0.00f, 0.00f, 0.00f, 0.00f);
    colors.[int ImGuiCol.TableRowBgAlt]          <- Vector4(1.00f, 1.00f, 1.00f, 0.07f);*)
    colors.[int ImGuiCol.TextSelectedBg]         <- Vector4(0.26f, 0.59f, 0.98f, 0.35f);
    colors.[int ImGuiCol.DragDropTarget]         <- Vector4(1.00f, 1.00f, 0.00f, 0.90f);
    colors.[int ImGuiCol.NavHighlight]           <- Vector4(0.26f, 0.59f, 0.98f, 1.00f);
    colors.[int ImGuiCol.NavWindowingHighlight]  <- Vector4(1.00f, 1.00f, 1.00f, 0.70f);
    colors.[int ImGuiCol.NavWindowingDimBg]      <- Vector4(0.80f, 0.80f, 0.80f, 0.20f);
    colors.[int ImGuiCol.ModalWindowDimBg]       <- Vector4(0.80f, 0.80f, 0.80f, 0.35f);