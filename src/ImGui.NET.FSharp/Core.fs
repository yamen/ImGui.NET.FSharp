[<AutoOpen>]
module ImGuiNET.FSharp.Core

open System
open System.Threading
open System.Collections.Concurrent

open Veldrid
open Veldrid.StartupUtilities

type IGuiRunner = 
    inherit IDisposable
    abstract Close: unit -> unit
    abstract UpdateBuilder: ImGuiBuilder -> unit

and ImGuiBuilder = unit -> unit

type private ImGuiRunner() = 
    static let mutable instance = new ImGuiRunner()
    static let mutable running = false

    let mutable disposing = false
    let mutable started = false

    let mutable window = Unchecked.defaultof<Sdl2.Sdl2Window>
    let mutable gd = Unchecked.defaultof<GraphicsDevice>
    let mutable cl = Unchecked.defaultof<CommandList>
    let mutable imguiRenderer = Unchecked.defaultof<ImGuiRenderer>

    let mutable guiBuilder:ImGuiBuilder = fun () -> () // empty builder

    let actionQueue = ConcurrentQueue<_>()
    let actionQueueLock = Object()

    member private x.Start(windowName, initialGuiBuilder, postInit) = 

        VeldridStartup.CreateWindowAndGraphicsDevice(
            new WindowCreateInfo(50, 50, 960, 540, WindowState.Normal, windowName),
            new GraphicsDeviceOptions(true, System.Nullable(), true, ResourceBindingModel.Improved, true, true), // sync to vertical important here for CPU usage
            &window, &gd 
        )

        cl <- gd.ResourceFactory.CreateCommandList()
        
        imguiRenderer <- new ImGuiRenderer(gd, gd.MainSwapchain.Framebuffer.OutputDescription, window.Width, window.Height)

        window.add_Resized(fun () -> 
            imguiRenderer.WindowResized(window.Width, window.Height);
            gd.MainSwapchain.Resize((uint)window.Width, (uint)window.Height);
        )

        window.add_Closed(fun () -> (x :> IDisposable).Dispose())

        x.UpdateBuilder(initialGuiBuilder)
        started <- true
        postInit()
        
        while window.Exists do
            if actionQueue.Count > 0 then
                Monitor.Enter(actionQueueLock)
                try
                    while actionQueue.Count > 0 do
                        match actionQueue.TryDequeue() with
                        | true, action -> action()
                        | false, _ -> ()
                finally
                    Monitor.Exit(actionQueueLock)

            let snapshot = window.PumpEvents()

            imguiRenderer.Update(2f / 60f, snapshot) // 30 fps is enough

            guiBuilder()               

            if window.Exists then            
                cl.Begin()
                cl.SetFramebuffer(gd.MainSwapchain.Framebuffer);
                cl.ClearColorTarget(0u, new RgbaFloat(0f, 0f, 0.2f, 1f))
                imguiRenderer.Render(gd, cl); // [3]
                cl.End()
                gd.SubmitCommands(cl)
                gd.SwapBuffers(gd.MainSwapchain)

    static member Start(windowName, guiBuilder:ImGuiBuilder) = 
        if not running then
            instance <- new ImGuiRunner()

            use waiter = new ManualResetEventSlim()
            Thread(fun () -> 
                running <- true
                instance.Start(windowName, guiBuilder, waiter.Set) |> ignore
                running <- false
            ).Start()
            waiter.Wait()
            
        instance :> IGuiRunner

    static member UpdateBuilder(builder) = 
        instance.UpdateBuilder(builder)

    static member Close() = 
        instance.Close()

    member private x.ScheduleAction(action) = 
        actionQueue.Enqueue(action)

    member private x.UpdateBuilder(builder) = 
        guiBuilder <- builder

    member private x.Close() = window.Close()

    interface IGuiRunner with 
        member _.Close() = window.Close()
        member x.UpdateBuilder(builder) = x.ScheduleAction(fun () -> x.UpdateBuilder(builder))

        member _.Dispose() = 
            if started && not disposing then
                disposing <- true
                if window.Exists then window.Close()
                gd.WaitForIdle()
                imguiRenderer.Dispose()
                cl.Dispose()
                gd.Dispose()

            started <- false

let startGui (windowName, guiBuilder) = ImGuiRunner.Start(windowName, guiBuilder)
let updateGui (newGui: ImGuiBuilder) = ImGuiRunner.UpdateBuilder(newGui)
let closeGui () = ImGuiRunner.Close()