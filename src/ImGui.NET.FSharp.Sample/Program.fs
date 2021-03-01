open ImGuiNET
open ImGuiNET.FSharp
open System.Drawing

[<EntryPoint>]
let main _ =
    let viewModel = {| 
        itemOk = ref false
        radiobox = ref 0
    |}
    
    let rec startup g = 
        Gui.window "Splash" [ 
            fun g -> 
                Styles.setGreenColorScheme()
                g.UpdateBuilder(page1)
        ] g

    and page1 (g) = 
        Gui.app [        
            Gui.window "Demo Window" [
                Gui.text "Page 1" ++ Gui.button "Page 2" (updateGui page2)                
            
                Gui.checkbox "Item OK?" viewModel.itemOk (fun _ var -> printfn $"it's {var}")

                Gui.radioboxes [
                    "radio 1", 1
                    "radio 2", 2
                    "radio 3", 3
                    ] viewModel.radiobox 
                    (fun _ var -> printfn $"clicked {var}") 
                    (fun _ var -> printfn $"changed {var}")
            ]

            commonStatusBar
        ] g

    and page2 (g) =   
        Gui.app [
            Gui.window "Demo Window" [
                Gui.button "Page 1" (updateGui page1) ++ Gui.text "Page 2"

                Gui.text "Nothing to see here"
            ]

            commonStatusBar
        ] g

    and commonStatusBar(g) = 
        let framerate = ImGui.GetIO().Framerate

        Gui.statusBar "Status Bar" [
            Gui.button "Quit" closeGui
            Gui.text "\t"
            Gui.text $"{framerate} FPS"
            Gui.text "\t"
            Gui.text "Item Status: " ++ (match !viewModel.itemOk with
                | true -> Gui.coloredText Color.Green "OK"
                | false -> Gui.coloredText Color.Red "Failed"
            )
        ] g

    // only one will run at a time, but can create it after it's destroyed
    // if gui is disposed it will be stopped
    let gui = startGui(startup)

    0 // return an integer exit code