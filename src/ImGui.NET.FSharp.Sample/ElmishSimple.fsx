#r "nuget: Elmish"
#r "nuget:ImGUI.Net"
#r "nuget:Veldrid"
#r "nuget:Veldrid.ImGui"
#r "nuget:Veldrid.StartupUtilities"

#I "../ImGui.Net.FSharp/bin/Debug/net5.0"
#r "ImGui.NET.FSharp.dll"

open System
open ImGuiNET
open ImGuiNET.FSharp
open Elmish

type Model = {
    Value: int
    Checkbox: bool
}

type Msg = 
    | Incr of int
    | Decr of int
    | Checked of bool

let update (msg:Msg) (model:Model) = 
    match msg with
    | Incr(amount) ->  { model with Value = model.Value + amount }, Cmd.none
    | Decr(amount) ->  { model with Value = model.Value - amount }, Cmd.none
    | Checked(flag) -> {model with Checkbox = flag}, Cmd.none

let init () = 
    {
        Value = 0
        Checkbox = false
    }, Cmd.none
    
// a viewmodel has a few purposes
// 1. mutable reference bindings for Imgui elements that need it (eg checkbox, selection etc)
// 2. generation of strings and other items only when model changes (rather than per loop)

let viewModel = {|
    Checkbox = ref false
    ValueString = ref ""
|}

let mapModelToViewModel (model:Model) = 
    viewModel.Checkbox := model.Checkbox
    viewModel.ValueString := model.Value.ToString()

let view (model:Model) (dispatch:Msg -> unit) = 
    mapModelToViewModel model

    let gui = 
        Gui.app [
            Gui.window "Demo" [
                Gui.sameLine [
                    Gui.text !viewModel.ValueString
                    Gui.button "+" (fun () -> Incr 1 |> dispatch )
                    Gui.button "-" (fun () -> Decr 1 |> dispatch )
                ] |> Gui.alignText

                Gui.checkbox "Test" viewModel.Checkbox (Checked >> dispatch)
            ]
        ]

    startOrUpdateGuiWith "Demo" gui |> ignore

let guiDispatch innerDispatch = 
    fun msg -> dispatchToGui (fun () -> innerDispatch msg)

Program.mkProgram init update view
|> Program.withSyncDispatch guiDispatch
|> Program.run