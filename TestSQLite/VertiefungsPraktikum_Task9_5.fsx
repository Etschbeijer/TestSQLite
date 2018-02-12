
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
open FSharp.Care
open BioFSharp.BioArray
open BioFSharp.BioList
open BioFSharp.AminoProperties
open FSharp.Care.IO.SchemaReader
open FSharp.Care.IO.SchemaReader.Attribute

///Defining Functions

let partFromFile amountOfProteins path =
    FastA.fromFile BioSeq.ofAminoAcidString path
    |> Seq.take amountOfProteins

let protease =
    Digestion.Table.getProteaseBy "Trypsin"

let digestProteom inputPeptide =
    Digestion.BioSeq.digest protease inputPeptide
    |> Seq.map (Seq.map (fun peptide -> peptide))

let calcMassOfPeptides inputPeptide =
    toMonoisotopicMass inputPeptide

let peptidesOfProteinsOfProteom (amountOfProteins : int) =
    partFromFile amountOfProteins @"C:\Users\Patrick\source\repos\FastaFileChlamy.txt" 
    |> Seq.toList
    |> List.map (fun x -> x.Sequence)
    |> Seq.map (fun protein -> digestProteom protein)
    |> Seq.concat
    |> Seq.map(fun peptide -> BioSeq.toString peptide)
    |> Seq.map (fun peptide -> BioArray.ofAminoAcidSymbolString peptide)


type StringArrayConverter() = 
    inherit ConverterAttribute()
    override this.convertToObj = 
        Converter.Single(fun (str : string) -> 
            str.Split(';')
            |> box)

type AABioListConverter() = 
    inherit ConverterAttribute()
    override this.convertToObj = 
        Converter.Single(fun (str : string) -> 
            (str
             |> BioList.ofAminoAcidString )
             |> box)


type CsvSchema = 
    {
        [<FieldAttribute(0)>]
        RawFile: string
        [<FieldAttribute(1)>]
        ScanIndex: int
        [<FieldAttribute(2)>]
        [<AABioListConverter()>]
        Sequence: BioList<AminoAcids.AminoAcid>
        [<FieldAttribute(3)>]
        Length: int
        [<FieldAttribute(4)>]
        [<StringArrayConverter()>]
        Proteins: string array
        [<FieldAttribute(5)>]
        Charge: int
        [<FieldAttribute(6)>]
        Mz: float
        [<FieldAttribute(7)>]
        Mass: float
        [<FieldAttribute(8)>]
        ///in ppm
        MassError: float
        [<FieldAttribute(9)>]
        ///in ppm
        SimpleMassError: float
        [<FieldAttribute(10)>]
        RetentionTime: float
        [<FieldAttribute(11)>]
        PEP: float
        [<FieldAttribute(12)>]
        Score: float
    }

let csvFile =
    let reader = new Csv.CsvReader<CsvSchema>(schemaMode=Csv.Fill)
    reader.ReadFile(@"C:\Users\Patrick\source\repos\ms2Scans.csv",'\t',false,skipLines=1)
    |> List.ofSeq

let getHydrophobicityIndex (input : BioArray<AminoAcidSymbols.AminoAcidSymbol>) =
    let rec loop n acc =
        if n = input.Length-1 then acc
        else 
            loop (n+1) ((initGetAminoProperty AminoProperty.HydrophobicityIndex input.[n]):: acc)
    loop 0 []

type peptideType =
    {
    PeptideSequence  : BioArray<AminoAcids.AminoAcid>
    Hydrophobicity   : float
    isoelectricPoint : float
    }

let createPeptideType (peptideSequence) (hydrophobicity) (ip) =
    {
    peptideType.PeptideSequence  = peptideSequence
    peptideType.Hydrophobicity   = hydrophobicity
    peptideType.isoelectricPoint = ip
    }

type csvType =
    {
    PeptideSequence  : BioArray<AminoAcids.AminoAcid>
    RetentionTime   : float
    }

let createCSVType (peptideSequence) (retentionTime) =
    {
    csvType.PeptideSequence  = peptideSequence
    csvType.RetentionTime    = retentionTime
    }
///Applying Functions

let peptides =
    peptidesOfProteinsOfProteom 100
    |> Array.ofSeq

//for i in peptides do
//    printfn "%A" i
//Seq.map (fun x -> printfn "%A" x) peptides

let hydrophobicitiesOfPeptides =
    peptides
    |> Seq.map (fun peptide -> peptide, getHydrophobicityIndex peptide)
    |> Seq.map (fun (peptide,hydrophobicityPerAA) -> BioArray.toString peptide |> BioArray.ofAminoAcidString , Seq.sum hydrophobicityPerAA)
    |> Seq.map (fun (peptide,hydrophobicityPerAA) -> createPeptideType peptide hydrophobicityPerAA 0.)
    |> Array.ofSeq
hydrophobicitiesOfPeptides

let sequencesAndRetentionTimes =
    csvFile
    |> Seq.map (fun x -> x.Sequence |> BioList.toString |> BioArray.ofAminoAcidString, x.RetentionTime)
    |> Seq.map (fun (x,y) -> createCSVType x y)
    |> Seq.toArray

sequencesAndRetentionTimes
Seq.length sequencesAndRetentionTimes

hydrophobicitiesOfPeptides
|> Seq.sortBy (fun (createdType) -> -createdType.Hydrophobicity)

sequencesAndRetentionTimes
|> Seq.sortBy (fun (createdType) -> -createdType.RetentionTime)

let findMatchingHPairs (input1 : csvType) (input2 : peptideType array) =
    let rec loop i acc =
        if i = input2.Length-1 then 
            //printfn "%A" acc
            acc
        else
            if input1.PeptideSequence = input2.[i].PeptideSequence then
                let newItem = (input1.PeptideSequence, input1.RetentionTime/ input2.[i].Hydrophobicity)
                //printfn "%A" newItem
                newItem
            else
                loop (i+1) acc
    loop 0 ([|AminoAcids.AminoAcid.Xle|],0.)

let finalSeq =
    sequencesAndRetentionTimes
    |> Array.map (fun x -> findMatchingHPairs x hydrophobicitiesOfPeptides)

finalSeq
|> Array.filter (fun (_,x) -> x <> 0.)
|> Array.sortBy (fun (_,x) -> -x)
