
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