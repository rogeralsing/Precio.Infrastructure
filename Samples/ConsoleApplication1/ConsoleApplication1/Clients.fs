module Clients
open System.Web
open System.Net
open System.Text
open System

type FooClient() =
    let getBarCompleted = new Event<_>()

    [<CLIEvent>]
    member this.GetBarCompleted = getBarCompleted.Publish

    member this.OnGetBarCompleted(arg) =
        getBarCompleted.Trigger(this, arg)

    member this.GetBarAsync =
          async{
               let client = new WebClient()
               client.Encoding <- Encoding.GetEncoding("utf-8")
               let! html = client.AsyncDownloadString(new Uri("http://www.google.com"))
               System.Threading.Thread.Sleep 1000
               this.OnGetBarCompleted html
          } 
          |> Async.Start
          