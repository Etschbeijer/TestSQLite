
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\FSharp.Care.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\FSharp.Care.IO.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\BioFSharp.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\BioFSharp.IO.dll"
#r @"C:\Users\Patrick\source\repos\VertiefungsPraktikum\VertiefungsPraktikum\bin\Debug\FSharp.Plotly.dll"

open FSharp.Plotly
open FSharp.Care
open FSharp.Care.IO
open BioFSharp
open BioFSharp.IO
open BioFSharp.BioSeq

let fromFile path =
    BioFSharp.IO.FastA.fromFile BioSeq.ofAminoAcidString path
    |> Seq.toList
    |> Seq.map (fun x -> x.Sequence)

let proteom = 
    fromFile @"C:\Users\Patrick\source\repos\FastaFileChlamy.txt"


let protease =
    Digestion.Table.getProteaseBy "Trypsin"

let digestProteom input =
    Seq.map (fun proteomItem -> Digestion.BioSeq.digest protease proteomItem) input
    |> Seq.concat

let digestedProteom =
    digestProteom proteom

let calculateMassesOfPeptides input =
    input
    |> Seq.map (fun peptideItem -> toMonoisotopicMass peptideItem)

let massesOfPetides =
    calculateMassesOfPeptides digestedProteom

let chartOfPeptideMasses input =
    input
    |> Chart.Histogram
    |> Chart.Show

chartOfPeptideMasses massesOfPetides