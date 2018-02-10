
#r @"..\TestSQLite\bin\Debug\netstandard.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r @"..\TestSQLite\bin\Debug\System.Data.SQLite.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.IO.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.Mz.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.IO.dll"
#r @"C:\Users\Patrick\source\repos\VertiefungsPraktikum\VertiefungsPraktikum\bin\Debug\FSharp.Plotly.dll"

open FSharp.Plotly
open BioFSharp
open BioFSharp.Formula
open BioFSharp.Mz
open BioFSharp.Mz.Fragmentation
open BioFSharp.Mz.Fragmentation.Series


///Defining functions!!!

let inputSequence = "PEPTIDE"

let toBioList input =
    input
    //|> BioArray.ofAminoAcidString
    |> BioList.ofAminoAcidString

let bIons input =
    bOfBioList monoisoMass input
    |> List.map (fun listItem -> listItem.MainPeak.Mass)
    |> List.map (fun fragmentMass -> [Mass.toMZ fragmentMass 1.; Mass.toMZ fragmentMass 2.; Mass.toMZ fragmentMass 3.; Mass.toMZ fragmentMass 4.])
    |> List.map(List.filter (fun chargedFrag -> chargedFrag <= 1300. && chargedFrag >= 250.))
    //|> List.concat

let yIons input =
    yOfBioList monoisoMass input
    |> List.map (fun listItem -> listItem.MainPeak.Mass)
    |> List.map (fun fragmentMass -> [Mass.toMZ fragmentMass 1.; Mass.toMZ fragmentMass 2.; Mass.toMZ fragmentMass 3.; Mass.toMZ fragmentMass 4.])
    |> List.map(List.filter (fun chargedFrag -> chargedFrag <= 1300. && chargedFrag >= 250.))
    //|> List.concat

let fuseList input1 input2 =
    List.append input1 input2

let mOverZPeptide input =
    let b = bIons input
    let y = yIons input
    fuseList b y

let showCHart input =
    Chart.Column([0. .. (List.length input)|> float], input)
    |> Chart.withX_AxisStyle("fragments")
    |> Chart.withY_AxisStyle("m/z")
    |> Chart.Show

let chartOfYFragments input =
    let genericList = [1.; 1.; 1.; 1.]
    yIons input
    |> List.mapi (fun i fragMassesMoverZforY -> Chart.Column(fragMassesMoverZforY, genericList,Name = sprintf "y-ion Fragment Number %i" (i+1)))
    |> Chart.Combine
    |> Chart.Show

let chartOfBFragments input =
    let genericList = [1.; 1.; 1.; 1.]
    bIons input
    |> List.mapi (fun i fragMassesMoverZforB -> Chart.Column(fragMassesMoverZforB, genericList,Name = sprintf "B-ion Fragment Number %i" (i+1)))
    |> Chart.Combine
    |> Chart.Show

let chartOfBYFragments input =
    let genericList = [1.; 1.; 1.; 1.]
    List.append (bIons input) (bIons input)
    |> List.mapi (fun i fragMassesMoverZforBY -> Chart.Column(fragMassesMoverZforBY, genericList,Name = sprintf "BY-ion Fragment Number %i" (i+1)))
    |> Chart.Combine
    |> Chart.Show

/// Applying Functions!!!

let peptide = toBioList inputSequence

let mOverZPetideFragments = mOverZPeptide peptide |> List.concat

showCHart mOverZPetideFragments

let findYIons = yIons peptide
let findBIons = bIons peptide

chartOfYFragments peptide
chartOfBFragments peptide
chartOfBYFragments peptide

BioFSharp.IO.FastA.fromFile BioArray.ofAminoAcidSymbolString @"C:\Users\Patrick\Desktop\F#_Project_DZ-PB_DB-SQLite\FastaFileChlamy.txt"