namespace ImGuiNET.FSharp

open ImGuiNET
open ImGuiNET.FSharp
open System.Numerics

[<RequireQualifiedAccess>]
module Gui = 
    let app (builder:#seq<ImGuiBuilder>) = fun () ->
        builder |> Seq.iter (fun i -> i())

    let window label (builder:#seq<ImGuiBuilder>) = fun () ->
        if (ImGui.Begin(label)) then
            builder |> Seq.iter (fun i -> i())
            ImGui.End()

    let button label onClick = fun () -> 
        if (ImGui.Button(label)) then onClick()

    let checkbox label (var: bool ref) onClick = fun () ->
        if (ImGui.Checkbox(label, &var.contents)) then
            onClick var.Value

    let text value = fun () -> 
        ImGui.Text(value)

    let coloredText (color:System.Drawing.Color) value = fun () -> 
        ImGui.TextColored(Styles.colorToVec4 color, value)

    let alignText (f) = fun () ->
        ImGui.AlignTextToFramePadding()
        f()

    let radioboxes (values: #seq<string * int>) (var: int ref) onClick onChange = fun () -> 
        let oldValue = var.Value

        for label, value in values do
            if ImGui.RadioButton(label, &var.contents, value) then
                onClick var.Value

        if var.Value <> oldValue then
            onChange var.Value

    let sameLine (builder:#seq<ImGuiBuilder>) = fun () ->
        let items = builder |> Array.ofSeq
        let lastItem = items |> Array.last

        for i = 0 to items.Length - 2 do
            items.[i]()
            ImGui.SameLine()

        lastItem()

    let private statusBarFlags =
        ImGuiWindowFlags.NoMove |||
        ImGuiWindowFlags.NoResize |||
        ImGuiWindowFlags.NoCollapse |||
        ImGuiWindowFlags.NoSavedSettings |||
        ImGuiWindowFlags.NoTitleBar

    let statusBar label (statusBarItems:#seq<ImGuiBuilder>) = fun () -> 
        let height = ImGui.GetTextLineHeightWithSpacing() + (ImGui.GetStyle().WindowPadding.Y * 2f)        
        ImGui.SetNextWindowPos(Vector2(0f, ImGui.GetIO().DisplaySize.Y - height))
        ImGui.SetNextWindowSize(Vector2(ImGui.GetIO().DisplaySize.X, 0f))

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0f)
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0f)
        if (ImGui.Begin(label, statusBarFlags)) then
            sameLine(statusBarItems)()
            ImGui.End()                        
        ImGui.PopStyleVar()

[<AutoOpen>]
module Operators = 
    let inline (++) (f1:ImGuiBuilder) (f2:ImGuiBuilder) = 
        fun () -> 
            f1()
            ImGui.SameLine()
            f2()