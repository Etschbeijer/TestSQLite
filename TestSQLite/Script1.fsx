
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.IO.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.IO.dll"

open BioFSharp.IO.Obo 
open FSharp.Care.IO

let readFile path =
    FileIO.readFile path
    |> parseOboTerms
let xyz = readFile @"C:\F#-Projects\ParserStuff\Psi-MS.txt" |> Seq.toList

let abc = Seq.item 0 xyz

let d = xyz |> Seq.mapi (fun i item -> i,item) |> Seq.toList //|> List.mapi (fun i item -> i,item)
Seq.length xyz

let nList = [1..10]
let double x = x*2

let createDoubleList (inPutList : int list) =
    let mutable newList = Array.create inPutList.Length 0
    let rec loop (acc : int) (accF: int->int) (accL : int []) =
        if acc = inPutList.Length-1 then 
            accL |> Array.toList
        else
            accL.[acc] <- (accF inPutList.[acc])
            loop (acc+1) accF accL
    loop 0 double newList
createDoubleList nList
