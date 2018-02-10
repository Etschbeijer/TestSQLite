
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\FSharp.Care.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\FSharp.Care.IO.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\FSharp.Stats.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\BioFSharp.dll"
#r @"C:\Users\Patrick\source\repos\F#_Project_DZ-PB_DB-SQLite\BioFSharp\BioFSharp.IO.dll"
#r @"C:\Users\Patrick\source\repos\VertiefungsPraktikum\VertiefungsPraktikum\bin\Debug\FSharp.Plotly.dll"

open BioFSharp
open BioFSharp.IO
open BioFSharp.BioSeq
open FSharp.Plotly
open FSharp.Stats
open FSharp.Stats.Distributions
open FSharp.Care

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

let calculateMoverZ inputPeptide charge =
    Mass.toMZ inputPeptide charge

let filterForMasses (minMass : float) (maxMass : float) (inputSequence) =
    Seq.filter (fun x -> x >= minMass && x <= maxMass) inputSequence

let arrangeBinsizes (input : float) inputSequence =
    Distributions.Frequency.create input inputSequence
    |> Map.toArray

//let calculateAverageBinsize input =
//    (fun x -> Array.average snd x) input

let chartOfPeptideMasses input =
    input
    |> Seq.map (fun x -> Chart.Column x)  

///Applying Functions

let MoverZofPeptides1 =
    Seq.map (fun x -> calculateMoverZ x 1.0) massesOfPetides

let MoverZofPeptides2 =
    Seq.map (fun x -> calculateMoverZ x 2.0) massesOfPetides

let MoverZofPeptides3 =
    Seq.map (fun x -> calculateMoverZ x 3.0) massesOfPetides

let MoverZofPeptides4 =
    Seq.map (fun x -> calculateMoverZ x 4.0) massesOfPetides

let MoverZofAllPeptides =
    [MoverZofPeptides1; MoverZofPeptides2; MoverZofPeptides3; MoverZofPeptides4]

let filteredMassesOfPeptides =
    MoverZofAllPeptides
    |> Seq.map (fun x -> filterForMasses 200. 1300. x)

let filteredMassesOfPeptidesCharge1 =
    filterForMasses 200. 1300. MoverZofPeptides1
let filteredMassesOfPeptidesCharge2 =
    filterForMasses 200. 1300. MoverZofPeptides2
let filteredMassesOfPeptidesCharge3 =
    filterForMasses 200. 1300. MoverZofPeptides3
let filteredMassesOfPeptidesCharge4 =
    filterForMasses 200. 1300. MoverZofPeptides4

let calculatedBins =
    Seq.map2 (fun x y -> arrangeBinsizes x y)[0.1 .. 0.1 .. 0.5] filteredMassesOfPeptides

let calculatedBins1 =
    Seq.map (fun x -> arrangeBinsizes x filteredMassesOfPeptidesCharge1) [0.1 .. 0.1 .. 2.]

let calculatedBins2 =
    Seq.map (fun x -> arrangeBinsizes x filteredMassesOfPeptidesCharge2) [0.1 .. 0.1 .. 2.]

let calculatedBins3 =
    Seq.map (fun x -> arrangeBinsizes x filteredMassesOfPeptidesCharge3) [0.1 .. 0.1 .. 2.]

let calculatedBins4 =
    Seq.map (fun x -> arrangeBinsizes x filteredMassesOfPeptidesCharge4) [0.1 .. 0.1 .. 2.]

let finalList =
    [calculatedBins1; calculatedBins2; calculatedBins3; calculatedBins4]

let test =
    calculatedBins1
    |> Seq.map (Array.map (fun x -> snd x))

finalList
|> Seq.map chartOfPeptideMasses
|> Seq.concat
|> Chart.Combine
|> Chart.Show

///Alternative way

let createBinSizeAverage inputSeq inputBinSize =
    inputSeq
    |> Distributions.Frequency.create inputBinSize
    |> Map.fold (fun (x, y) _ value -> (value + x), (y + 1) ) (0,0)
    |> (fun (sum, count) -> float sum/ float count)

createBinSizeAverage filteredMassesOfPeptidesCharge1 0.1

calculatedBins

filteredMassesOfPeptides

let averageOfAllPeptides =
    Seq.map (fun x -> createBinSizeAverage (filteredMassesOfPeptides |> Seq.concat) x)  [0.1.. 0.1 ..1.]

averageOfAllPeptides

Chart.Point([0.1 .. 0.1 .. 1.], averageOfAllPeptides)
|> Chart.Show
