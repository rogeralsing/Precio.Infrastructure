// Learn more about F# at http://fsharp.net
open Clients
open System

let client = new FooClient()
client.GetBarCompleted.Add (fun (s,e) -> Console.WriteLine e)
client.GetBarAsync
Console.WriteLine "start"
Console.ReadLine() |> ignore