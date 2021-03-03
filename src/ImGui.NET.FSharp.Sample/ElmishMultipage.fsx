
// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
#r "nuget: Elmish"
#r "nuget:ImGUI.Net"
#r "nuget:Veldrid"
#r "nuget:Veldrid.ImGui"
#r "nuget:Veldrid.StartupUtilities"

#I "../ImGui.Net.FSharp/bin/Debug/net5.0"
#r "ImGui.NET.FSharp.dll"
#r "System.Drawing.Primitives.dll"

open ImGuiNET
open ImGuiNET.FSharp
open Elmish

type Page = 
    | Page1
    | Page2

type Model = {
    Page: Page
    Selection: int
    ItemOk: bool
}

type Msg = 
    | InitialLoad
    | ChangePage of Page
    | ChangeSelection of int
    | ToggleStatus of bool

let update (msg:Msg) (model:Model) = 
    match msg with
    | InitialLoad -> 
        let loader = Gui.window "Loading" [ Styles.setGreenColorScheme ]            
        model, Cmd.OfFunc.perform loader () (fun () -> ChangePage Page1)

    | ChangePage(page) ->  { model with Page = page }, Cmd.none

    | ChangeSelection(selection) ->  { model with Selection = selection }, Cmd.none

    | ToggleStatus(flag) -> {model with ItemOk = flag}, Cmd.none

let init () = 
    {
        Page = Page1
        Selection = 0
        ItemOk = false
    }, Cmd.ofMsg InitialLoad
    
// a viewmodel has a few purposes
// 1. mutable reference bindings for Imgui elements that need it (eg checkbox, selection etc)
// 2. generation of strings and other items only when model changes (rather than per loop)
let viewModel = {|
    Checkbox = ref false
    Radiobox = ref 0
|}

let mapModelToViewModel (model:Model) = 
    viewModel.Checkbox := model.ItemOk
    viewModel.Radiobox := model.Selection

open System.Drawing

let view (model:Model) (dispatch:Msg -> unit) = 
    mapModelToViewModel model

    let commonStatusBar() = 
        let framerate = ImGui.GetIO().Framerate

        Gui.statusBar "Status Bar" [
            Gui.button "Quit" closeGui
            Gui.text "\t"
            Gui.text "Item Status: " ++ (
                match model.ItemOk with
                | true -> Gui.coloredText Color.Green "OK"
                | false -> Gui.coloredText Color.Red "Failed"
            )
            Gui.text "\t\t"
            Gui.text $"{framerate} FPS"
        ]()

    let gui = 
        match model.Page with
        | Page1 -> 
            Gui.app [        
                Gui.window "Demo Window" [

                    Gui.text "Page 1" ++ Gui.button "Page 2" (fun () -> ChangePage Page2 |> dispatch) |> Gui.alignText
            
                    Gui.checkbox "Item OK?" viewModel.Checkbox (ToggleStatus >> dispatch)

                    Gui.radioboxes [
                        "radio 1", 1
                        "radio 2", 2
                        "radio 3", 3
                        ] viewModel.Radiobox 
                        (fun var -> printfn $"clicked {var}") 
                        (ChangeSelection >> dispatch)
                ]

                commonStatusBar
            ]
        | Page2 ->  
            Gui.app [
                Gui.window "Demo Window" [
                    Gui.button "Page 1" (fun () -> ChangePage Page1 |> dispatch) ++ Gui.text "Page 2" |> Gui.alignText

                    Gui.text "Nothing to see here"
                ]

                commonStatusBar
            ]

    startOrUpdateGuiWith "Demo" gui |> ignore

let guiDispatch innerDispatch = 
    fun msg -> dispatchToGui (fun () -> innerDispatch msg)

Program.mkProgram init update view
|> Program.withSyncDispatch guiDispatch
|> Program.run