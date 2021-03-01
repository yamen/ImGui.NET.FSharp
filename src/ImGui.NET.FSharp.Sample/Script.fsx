#r "nuget:ImGUI.Net"
#r "nuget:Veldrid"
#r "nuget:Veldrid.ImGui"
#r "nuget:Veldrid.StartupUtilities"

#I "../ImGui.Net.FSharp/bin/Debug/net5.0"
#r "ImGui.NET.FSharp.dll"

open ImGuiNET
open ImGuiNET.FSharp

let commonStatusBar(g) = 
    Gui.statusBar "Status Bar" [
        Gui.button "Quit" closeGui
        Gui.text $"{ImGui.GetIO().Framerate} FPS"
    ] g

let original(g) = 
    Gui.app [
        Gui.window "Original" [
            Gui.text "Original"
        ]

        commonStatusBar
    ](g)

let gui = startGui(original)

let replaced(g) = 
    Gui.app [
        Gui.window "Updated!" [
            Gui.text "Updated!"
        ]

        commonStatusBar
    ](g)

gui |> updateGui replaced