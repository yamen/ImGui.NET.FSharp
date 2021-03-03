#r "nuget:ImGUI.Net"
#r "nuget:Veldrid"
#r "nuget:Veldrid.ImGui"
#r "nuget:Veldrid.StartupUtilities"

#I "../ImGui.Net.FSharp/bin/Debug/net5.0"
#r "ImGui.NET.FSharp.dll"

open ImGuiNET
open ImGuiNET.FSharp

let commonStatusBar() = 
    Gui.statusBar "Status Bar" [
        Gui.button "Quit" closeGui
        Gui.text $"{ImGui.GetIO().Framerate} FPS"
    ]()

let counter = ref 0
let incr i = fun _ -> counter := !counter + i
let decr i = fun _ -> counter := !counter - i

let page() = 
    Gui.app [
        Gui.window "Window 1" [
            Gui.text "Hello World!"
            Gui.text $"Counter: {!counter}"
            Gui.button "Down" (decr 1) ++ Gui.button "Up" (incr 1)
        ]

        commonStatusBar
    ]()

startOrUpdateGuiWith "Demo" page

let newPage()  = 
    Gui.app [
        Gui.window "Updated!" [
            Gui.text "Updated!"
        ]

        commonStatusBar
    ]()

setGuiBuilder newPage