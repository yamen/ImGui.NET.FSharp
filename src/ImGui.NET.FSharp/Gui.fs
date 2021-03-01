namespace ImGuiNET.FSharp

open ImGuiNET
open ImGuiNET.FSharp
open System.Numerics

[<RequireQualifiedAccess>]
module Gui = 
    let app (builder:#seq<ImGuiBuilder>) = fun (g:IGuiRunner) ->
        builder |> Seq.iter (fun i -> i(g))

    let window label (builder:#seq<ImGuiBuilder>) = fun (g:IGuiRunner) ->
        if (ImGui.Begin(label)) then
            builder |> Seq.iter (fun i -> i(g))
            ImGui.End()

    let button label onClick = fun (g:IGuiRunner) -> 
        if (ImGui.Button(label)) then onClick g

    let checkbox label (var: bool ref) onClick = fun (g:IGuiRunner) ->
        if (ImGui.Checkbox(label, &var.contents)) then
            onClick g var.Value

    let text value = fun (g:IGuiRunner) -> 
        ImGui.Text(value)

    let coloredText (color:System.Drawing.Color) value = fun (g:IGuiRunner) -> 
        ImGui.TextColored(Styles.colorToVec4 color, value)

    let radioboxes (values: #seq<string * int>) (var: int ref) onClick onChange = fun (g:IGuiRunner) -> 
        let oldValue = var.Value

        for label, value in values do
            if ImGui.RadioButton(label, &var.contents, value) then
                onClick g var.Value

        if var.Value <> oldValue then
            onChange g var.Value

    let sameLine (builder:#seq<ImGuiBuilder>) = fun (g:IGuiRunner) ->
        let items = builder |> Array.ofSeq
        let lastItem = items |> Array.last

        for i = 0 to items.Length - 2 do
            items.[i](g)
            ImGui.SameLine()

        lastItem(g)

    let private statusBarFlags =
        ImGuiWindowFlags.NoMove |||
        ImGuiWindowFlags.NoResize |||
        ImGuiWindowFlags.NoCollapse |||
        ImGuiWindowFlags.NoSavedSettings |||
        ImGuiWindowFlags.NoTitleBar

    let statusBar label (statusBarItems:#seq<ImGuiBuilder>) = fun (g:IGuiRunner) -> 
        let height = ImGui.GetTextLineHeightWithSpacing() + (ImGui.GetStyle().WindowPadding.Y * 2f)        
        ImGui.SetNextWindowPos(Vector2(0f, ImGui.GetIO().DisplaySize.Y - height))
        ImGui.SetNextWindowSize(Vector2(ImGui.GetIO().DisplaySize.X, 0f))

        ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0f)
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0f)
        if (ImGui.Begin(label, statusBarFlags)) then
            sameLine statusBarItems g
            ImGui.End()                        
        ImGui.PopStyleVar()

[<AutoOpen>]
module Operators = 
    let inline (++) (f1:ImGuiBuilder) (f2:ImGuiBuilder) = 
        fun (g:IGuiRunner) -> 
            f1(g)
            ImGui.SameLine()
            f2(g)