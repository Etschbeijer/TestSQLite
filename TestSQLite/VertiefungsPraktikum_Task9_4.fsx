
#r @"..\TestSQLite\bin\Debug\BioFSharp.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.Mz.dll"
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
open BioFSharp.BioArray
open BioFSharp.Formula
open BioFSharp.Mz
open BioFSharp.Mz.Fragmentation
open BioFSharp.Mz.Fragmentation.Series

///Defining Functions

let partFromFile amountOfProteins path =
    FastA.fromFile BioSeq.ofAminoAcidString path
    //|> Seq.take amountOfProteins

let protease =
    Digestion.Table.getProteaseBy "Trypsin"

let digestProteom inputPeptide =
    Digestion.BioSeq.digest protease inputPeptide

let calcMassOfPeptides inputPeptide =
    toMonoisotopicMass inputPeptide

let chargePeptide inputPeptide charge =
    Mass.toMZ inputPeptide charge

let filterPeptides (minValue :float) (maxValue : float) (inputPeptide) =
    inputPeptide
    |> Seq.filter (fun (peptideMass) -> peptideMass >= minValue && peptideMass <= maxValue)

let filterPeptides2 (minValue :float) (maxValue : float) (inputPeptide : seq<float * 'b>) =
    inputPeptide
    |> Seq.filter (fun (peptideMass, _) -> peptideMass >= minValue && peptideMass <= maxValue)

let arrangeBinSize (inputSize : float) inputPeptide =
    inputPeptide
    |> Distributions.Frequency.create inputSize

let peptidesOfProteom (amountOfProteins : int) (charge : float) (binSizePeptide : float) =
    partFromFile (amountOfProteins : int) @"C:\Users\Patrick\source\repos\FastaFileChlamy.txt" 
    |> Seq.toList
    |> List.map (fun x -> x.Sequence)
    |> Seq.map (fun protein -> digestProteom protein)
    |> Seq.map (Seq.map (fun peptide -> BioList.toString peptide))
    |> Seq.map (Seq.map (fun peptide -> BioArray.ofAminoAcidString peptide))
    |> Seq.map (Seq.map (fun peptide -> calcMassOfPeptides peptide))
    |> Seq.map (Seq.map (fun peptideMass -> chargePeptide peptideMass charge))
    |> Seq.map (fun peptide -> filterPeptides 200. 1300. peptide)
    |> Seq.map (fun peptide -> (peptide, Seq.length peptide))
    |> Seq.map (fun (peptide,amountPeptides) -> (arrangeBinSize binSizePeptide peptide), amountPeptides)

let fragmentsOfBIonsOfPeptides (amountOfProteins : int) (charge : float) (binSizePeptide : float) =
    partFromFile (amountOfProteins : int) @"C:\Users\Patrick\source\repos\FastaFileChlamy.txt" 
    |> Seq.toList
    |> List.map (fun x -> x.Sequence)
    |> Seq.map (fun protein -> digestProteom protein)
    |> Seq.map (Seq.map (fun peptide -> BioList.toString peptide))
    |> Seq.map (Seq.map (fun peptide -> BioArray.ofAminoAcidString peptide))
    |> Seq.map (Seq.map (fun peptide -> calcMassOfPeptides peptide, peptide))
    |> Seq.map (Seq.map (fun (peptideMass, peptideSeq) -> chargePeptide peptideMass charge, peptideSeq))
    |> Seq.map (fun peptide -> filterPeptides2 300. 1300. peptide)
    |> Seq.map (Seq.map (fun (_, peptide) -> peptide))
    |> Seq.concat
    |> Seq.map (fun peptide -> BioArray.toString peptide)
    |> Seq.map(fun peptide -> BioList.ofAminoAcidString peptide)
    |> Seq.map(fun peptide -> bOfBioList monoisoMass peptide)
    |> Seq.map(List.map(fun bIons -> bIons.MainPeak.Mass))
    |> Seq.map (List.map(fun fragmentMass -> chargePeptide fragmentMass charge))
    //|> Seq.map List.toSeq
    |> Seq.map (fun fragments -> filterPeptides 200. 700. fragments)
    |> Seq.map (fun bIon -> (bIon, Seq.length bIon))
    |> Seq.map (fun (bIon,amountbIons) -> (arrangeBinSize binSizePeptide bIon), amountbIons)

fragmentsOfBIonsOfPeptides 10 1. 0.7

let fragmentsOfYIonsOfPeptides (amountOfProteins : int) (charge : float) (binSizePeptide : float) =
    partFromFile (amountOfProteins : int) @"C:\Users\Patrick\source\repos\FastaFileChlamy.txt" 
    |> Seq.toList
    |> List.map (fun x -> x.Sequence)
    |> Seq.map (fun protein -> digestProteom protein)
    |> Seq.map (Seq.map (fun peptide -> BioList.toString peptide))
    |> Seq.map (Seq.map (fun peptide -> BioArray.ofAminoAcidString peptide))
    |> Seq.map (Seq.map (fun peptide -> calcMassOfPeptides peptide, peptide))
    |> Seq.map (Seq.map (fun (peptideMass, peptideSeq) -> chargePeptide peptideMass charge, peptideSeq))
    |> Seq.map (fun peptide -> filterPeptides2 300. 1300. peptide)
    |> Seq.map (Seq.map (fun (_, peptide) -> peptide))
    |> Seq.concat
    |> Seq.map (fun peptide -> BioArray.toString peptide)
    |> Seq.map(fun peptide -> BioList.ofAminoAcidString peptide)
    |> Seq.map(fun peptide -> bOfBioList monoisoMass peptide)
    |> Seq.map(List.map(fun bIons -> bIons.MainPeak.Mass))
    |> Seq.map (List.map(fun fragmentMass -> chargePeptide fragmentMass charge))
    //|> Seq.map List.toSeq
    |> Seq.map (fun fragments -> filterPeptides 200. 700. fragments)
    |> Seq.map (fun bIon -> (bIon, Seq.length bIon))
    |> Seq.map (fun (bIon,amountbIons) -> (arrangeBinSize binSizePeptide bIon), amountbIons)

let calculateRelativeRatio (input1 : float) (input2 : float) =
    (input1 / input2)*100.

let showCHart input =
    Chart.Column([0. .. (Seq.length input)|> float], input,Name = "ChargeState")
    |> Chart.withX_AxisStyle("amount of checked Charges ")
    |> Chart.withY_AxisStyle("ratio bin with one element to all bins [%]")
    |> Chart.Show

let amountOfBinsWithMoreElemnts input =
    Seq.map (fun (x,_) -> x) input
    |> Seq.map Seq.length
    |> Seq.map (fun x -> float x)

let amountOfBinsWithOneElement input = 
    Seq.map (fun (x,_) -> x) input
    |> Seq.map (fun x -> Map.toList x)
    |> Seq.map (List.filter(fun (_,y) -> y = 1))
    |> Seq.map Seq.length
    |> Seq.map (fun x -> float x)

///Applying Functions

let peptideInfo =
    [1. .. 4.]
    |> Seq.map (fun charge -> peptidesOfProteom 300 charge 0.7)

let amountOfPeptidesWithMoreElements =
    peptideInfo
    |> Seq.map amountOfBinsWithMoreElemnts
    |>Seq.map Seq.sum
    |> Seq.map float

let amountOfPeptidesWithOneElement =
    peptideInfo
    |> Seq.map (fun x -> amountOfBinsWithOneElement x)
    |> Seq.map Seq.sum
    |> Seq.map float

let ratioOfPeptideMoreELemntsToPeptideOneElement =
    Seq.map2 (fun x y -> calculateRelativeRatio x y) amountOfPeptidesWithOneElement amountOfPeptidesWithMoreElements

let fragmentInfo =
    [1. .. 4.]
    |> Seq.map (fun charge -> [fragmentsOfBIonsOfPeptides 300 charge 0.7; fragmentsOfYIonsOfPeptides 10 charge 0.7])

fragmentInfo

let amountOfFragmentsWithMoreElements =
    fragmentInfo
    |> Seq.map (List.map (fun x -> amountOfBinsWithMoreElemnts x))
    |> Seq.map (List.map (fun x -> Seq.sum x))
    |> Seq.map (List.map (fun x -> float x))

amountOfFragmentsWithMoreElements

let amountOfFragmentsWithOneElement =
    fragmentInfo
    |> Seq.map (List.map (fun x -> amountOfBinsWithOneElement x))
    |> Seq.map (List.map (fun x -> Seq.sum x))
    |> Seq.map (List.map (fun x -> float x))

amountOfFragmentsWithOneElement

let ratioOfFragmenMoreElementsToFragmentOneElement =
    Seq.map2 (Seq.map2(fun x y -> calculateRelativeRatio x y)) amountOfFragmentsWithOneElement amountOfFragmentsWithMoreElements

showCHart ratioOfPeptideMoreELemntsToPeptideOneElement

ratioOfFragmenMoreElementsToFragmentOneElement
    |> Seq.map (fun x -> Chart.Column ([0. .. (Seq.length x)|> float], x))
    |> Chart.Combine
    |> Chart.withX_AxisStyle("amount of checked Charges ")
    |> Chart.withY_AxisStyle("ratio bin with one element to all bins [%]")
    |> Chart.Show