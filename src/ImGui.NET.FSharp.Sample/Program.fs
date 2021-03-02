open ImGuiNET
open ImGuiNET.FSharp
open System.Drawing

[<EntryPoint>]
let main _ =
    let viewModel = {| 
        itemOk = ref false
        radiobox = ref 0
    |}
    
    let rec startup() = 
        Gui.window "Splash" [ 
            fun g -> 
                Styles.setGreenColorScheme()
                setGuiBuilder page1
        ]()

    and page1() = 
        Gui.app [        
            Gui.window "Demo Window" [

                Gui.text "Page 1" ++ Gui.button "Page 2" (fun () -> setGuiBuilder page2) |> Gui.alignText
            
                Gui.checkbox "Item OK?" viewModel.itemOk (fun var -> printfn $"it's {var}")

                Gui.radioboxes [
                    "radio 1", 1
                    "radio 2", 2
                    "radio 3", 3
                    ] viewModel.radiobox 
                    (fun var -> printfn $"clicked {var}") 
                    (fun var -> printfn $"changed {var}")
            ]

            commonStatusBar
        ]()

    and page2 () =   
        Gui.app [
            Gui.window "Demo Window" [
                Gui.button "Page 1" (fun () -> setGuiBuilder page1) ++ Gui.text "Page 2" |> Gui.alignText

                Gui.text "Nothing to see here"
            ]

            commonStatusBar
        ]()

    and commonStatusBar() = 
        let framerate = ImGui.GetIO().Framerate

        Gui.statusBar "Status Bar" [
            Gui.button "Quit" closeGui
            Gui.text "\t"
            Gui.text "Item Status: " ++ (match !viewModel.itemOk with
                | true -> Gui.coloredText Color.Green "OK"
                | false -> Gui.coloredText Color.Red "Failed"
            )
            Gui.text "\t\t"
            Gui.text $"{framerate} FPS"
        ]()

    // only one will run at a time, but can create it after it's destroyed
    // if gui is disposed it will be stopped
    let gui = startOrUpdateGuiWith "Demo" startup

    System.Threading.Thread.Sleep(2000)

    dispatchToGui(fun () -> viewModel.radiobox := 2)

    //setGuiBuilder startup

    0 // return an integer exit code