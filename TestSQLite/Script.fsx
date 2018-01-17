
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\netstandard.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\FSharp.Data.TypeProviders.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\System.Linq.dll"
//#r @"C:\Users\Patrick\source\repos\DatenBankTest\TestTabelleDavidFirma\bin\Debug\EntityFramework.dll"
//#r @"C:\Users\Patrick\source\repos\DatenBankTest\TestTabelleDavidFirma\bin\Debug\FSharp.Plotly.dll"

#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\netstandard.dll"
#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\FSharp.Data.TypeProviders.dll"
#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\System.Linq.dll"
#r @"C:\Users\PatrickB\Source\Repos\DatenBankTest\TestTabelleDavidFirma\bin\Debug\FSharp.Plotly.dll"
#r @"C:\Users\PatrickB\Source\Repos\DatenBankTest\TestTabelleDavidFirma\bin\Debug\EntityFramework.dll"

open System
open System.Diagnostics
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
open System.Linq
open FSharp.Plotly
open FSharp.Plotly.HTML

///Defining the Types/////////////////////////
//////////////////////////////////////////////////////////////////

type Blog(blogid : int, url : string) =
    let mutable blogID = blogid
    let mutable url    = url
    member this.BlogID with get() = blogID and set(value) = blogID <- value
    member this.URL    with get() = url    and set(value) = url    <- value

type Post(postid : int, title : string, content : string, blogid : int) =
    let mutable postID  = postid
    let mutable title   = title
    let mutable content = content
    let mutable blogID  = blogid
    member this.PostID  with get() = postID  and set(value) = postID  <- value
    member this.Title   with get() = title   and set(value) = title   <- value
    member this.Content with get() = content and set(value) = content <- value
    member this.BlogID  with get() = blogID  and set(value) = blogID  <- value


type BloggingContext() =
    inherit DbContext()

    [<DefaultValue>] val mutable m_blog : DbSet<Blog>
    member public this.Blog with get() = this.m_blog 
                                        and set v = this.m_blog  <- v

    [<DefaultValue>] val mutable m_post : DbSet<Post>
    member public this.Post with get() = this.m_post 
                                        and set v = this.m_post <- v

    override this.OnConfiguring (optionsbuilder :  DbContextOptionsBuilder) =
        optionsbuilder.UseSqlite(@"Data Source=C:\F#-Projects\blogging.db") |> ignore

//type Abteilungen(abteilungenid : int, name : string) =
//    let mutable abteilungenID = abteilungenid
//    let mutable name     = name
//    member this.AbteilungenID with get() = abteilungenID and set(value) = abteilungenID <- value
//    member this.Name          with get() = name          and set(value) = name          <- value

//type Rollen(rollenid : int, name : string) =
//    let mutable rollenID = rollenid
//    let mutable name     = name
//    member this.RollenID with get() = rollenID and set(value) = rollenID <- value
//    member this.Name     with get() = name     and set(value) = name     <- value

//type PersonenVerzeichnis(name : string, abteilungsid : int, rollenid : int) =
//    let mutable id            = 0
//    let mutable name          = name
//    let mutable abteilungenID = abteilungsid
//    let mutable rollenID      = rollenid
//    member this.ID            with get() = id           and set(value) = id             <- value
//    member this.Name          with get() = name         and set(value) = name           <- value
//    member this.AbteilungenID with get() = abteilungenID and set(value) = abteilungenID <- value
//    member this.RollenID      with get() = rollenID     and set(value) = rollenID       <- value

//type PersonenContext() =
//    inherit DbContext()
    
//    [<DefaultValue>] val mutable m_abteilungen : DbSet<Abteilungen>
//    member public this.Abteilungen with get() = this.m_abteilungen
//                                           and set value = this.m_abteilungen <- value


//    [<DefaultValue>] val mutable m_rollen : DbSet<Rollen>
//    member public this.Rollen with get() = this.m_rollen
//                                           and set value = this.m_rollen <- value

//    [<DefaultValue>] val mutable m_personenverzeichnis : DbSet<PersonenVerzeichnis>
//    member public this.PersonenVerzeichnis with get() = this.m_personenverzeichnis
//                                                        and set value = this.m_personenverzeichnis <- value

//    override this.OnConfiguring (optionsbuilder :  DbContextOptionsBuilder) =
//        optionsbuilder.UseSqlite(@"Data Source=C:\F#-Projects\TestDatenBank.db") |> ignore


[<CLIMutable>]
type Abteilungen = 
    {
    ID : int
    Name : string
    }

[<CLIMutable>]
type Rollen = 
    {
    ID : int
    Name : string
    }

[<CLIMutable>]
type PersonenVerzeichnis = 
    {
    ID : int
    Name : string
    AbteilungenID : int
    RollenID : int
    }

let a:Abteilungen = 
    {
    ID=1;
    Name=""
    }

type PersonenContext() =
    inherit DbContext()
    [<DefaultValue>] val mutable m_abteilungen : DbSet<Abteilungen>
    member public this.Abteilungen with get() = this.m_abteilungen
                                                and set value = this.m_abteilungen <- value


    [<DefaultValue>] val mutable m_rollen : DbSet<Rollen>
    member public this.Rollen with get() = this.m_rollen
                                           and set value = this.m_rollen <- value

    [<DefaultValue>] val mutable m_personenverzeichnis : DbSet<PersonenVerzeichnis>
    member public this.PersonenVerzeichnis with get() = this.m_personenverzeichnis
                                                        and set value = this.m_personenverzeichnis <- value

    override this.OnConfiguring (optionsbuilder :  DbContextOptionsBuilder) =
        optionsbuilder.UseSqlite(@"Data Source=C:\F#-Projects\TestDatenBank.db") |> ignore

///Manipulating the databases/////////////////////////
/////////////////////////////////////////////////////////////////////

let program (id : int) (url : string) =
    let db = new BloggingContext()
    db.Add(new Blog(id, url)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let program2 (id : int) (title : string) (content : string) (blogid : int) =
    let db = new BloggingContext()
    db.Add(new Post(id, title, content, blogid)) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingAbteilungen (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add({Abteilungen.ID=id; Abteilungen.Name=name}) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingRollen (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add({Rollen.ID=id; Rollen.Name=name}) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingPersonenVerzeichnis (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add({ID=id; Name=name; AbteilungenID=1; RollenID=1}) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingPersonenVerzeichnisMultipleTransactions (x : int) =
    let db = new PersonenContext()
    let timer = new Stopwatch()
    timer.Start()
    for i = 1 to x do
        db.Add({ID=i;Name=(sprintf "Bob%i") i; AbteilungenID=1;RollenID=1}) |> ignore
    db.SaveChanges() |>ignore
    timer.Stop()
    timer.Elapsed.TotalMilliseconds

///Testing Area

program 3 "http://sample.com"
program2 3 "jo" "joo" 1

sqlTestingAbteilungen 1 "BioTech"
sqlTestingRollen 1 "Professor"
sqlTestingPersonenVerzeichnis 1 "Bob"

sqlTestingPersonenVerzeichnisMultipleTransactions 100

///Plot every Graph together//////////////////////////////////////

let numberOfDataPoints = [10.; 100.; 1000.; 10000.; 100000.; 1000000.]

let insertionTimeAspNetMS = List.map2 (fun x y -> (x,y)) numberOfDataPoints [3.6251; 6.421; 9.4021; 77.537; 502.9515; 5079.1631]
let insertionTimeTypeProviderMS = List.map2 (fun x y -> (x,y)) numberOfDataPoints [84.; 112.; 492.; 4110.; 41360.; 481047.]
let insertionTimeEntityFrameworkCoreMS = List.map2 (fun x y -> (x,y)) numberOfDataPoints [743.9926; 767.0876; 778.1466; 1505.5596; 8959.6743; 84528.5174]

let readTimeAspNetMS = List.map2 (fun x y -> (x,y)) numberOfDataPoints [0.3083; 0.3113; 1.044; 8.1912; 139.1479; 1273.2844]
let readTimeTypeProviderMS =List.map2 (fun x y -> (x,y)) numberOfDataPoints [60.; 62.; 73.; 225.; 2138.; 22043.]
let readTimeEntityFrameworkCoreMS = List.map2 (fun x y -> (x,y)) numberOfDataPoints [589.; 692.; 577.; 620.; 1078.; 5447.]

[Chart.Line (insertionTimeAspNetMS, "Asp.Net");Chart.Line(insertionTimeTypeProviderMS, "SQLTypeProvider");Chart.Line(insertionTimeEntityFrameworkCoreMS, "EntityFrameworkCore")]
|> Chart.Combine
|> Chart.withX_AxisStyle(title="dataset size")
|> Chart.withY_AxisStyle(title="insertion time [ms]")
|> Chart.Show

[Chart.Line (readTimeAspNetMS,"Asp.Net");Chart.Line(readTimeTypeProviderMS,"SQLTypeProvider");Chart.Line(readTimeEntityFrameworkCoreMS,"EntityFrameworkCore")]
|> Chart.Combine
|> Chart.withX_AxisStyle(title="dataset size")
|> Chart.withY_AxisStyle(title="reading time [ms]")
|> Chart.Show

/// QueryTesting

let context = new PersonenContext()

#time
let query1 =
    query{
    for personen in context.PersonenVerzeichnis do
    select personen
     } 
     |> Seq.toArray

#time
query1
|> Array.iter (fun personen -> printfn "ID: %i Name: %s AbteilungsID: %i RollenID: %i" personen.ID personen.Name personen.AbteilungenID personen.RollenID)


let idList = [1;3;5]

let idQuery = 
    query{
    for id in idList do
    select id
    }

let query2 =
    query{
    for personen in context.PersonenVerzeichnis do
    where (idQuery.Contains(personen.ID))
    select personen
    }
    |> Seq.toArray


///Another Test
//#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\bin\BioFSharp.Mz\System.Data.SQLite.dll"
//#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\bin\BioFSharp.Mz\BioFSharp.dll"
//#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\bin\BioFSharp.Mz\BioFSharp.Mz.dll"
//#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\bin\BioFSharp.Mz\FSharp.Care.dll"
//#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\bin\BioFSharp.Mz\FSharp.Care.IO.dll"

#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\System.Data.SQLite.dll"
#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.Mz.dll"
#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.dll"
#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.IO.dll"
#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.dll"

open BioFSharp
open FSharp
open FSharp.Care.Collections
open FSharp.Care.IO
open BioFSharp.Mz.MzIdentMLModel
open BioFSharp.Mz.MzIdentMLModel.Db
open System
open System.Data
open System.IO
open System.Data.SQLite
open BioFSharp.Formula.Table
open BioFSharp.BioID.FastA
open FSharp.Care.IO.SchemaReader

///types for the DataBank
[<CLIMutable>]
type AnalysisSoftware =
    {
    ID : int
    Name : string
    }

[<CLIMutable>]
type AnalysisSoftwareParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type DBSequence =
    {
    ID : int
    Accession : string
    Name : string
    SearchDBID : string
    }

[<CLIMutable>]
type DBSequenceParams =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type ModLocation =
    {
    ID : int
    PeptideID : int
    ModificationID : int
    Location : int
    Residue : string
    }

[<CLIMutable>]
type ModLocationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type Modification =
    {
    ID : int
    Name : string
    Residues : string
    MonoisotopicMassDelta : float
    AvgMassDelta : float
    }

[<CLIMutable>]
type ModificationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type Ontology = 
    {
    ID : string
    Name : string
    }

[<CLIMutable>]
type Organization =
    {
    ID : int
    Name : string
    Parent_ID : string
    }

[<CLIMutable>]
type OrganizationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type Peptide =
    {
    ID : int
    Sequence : string
    }

[<CLIMutable>]
type PeptideParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type PeptideEvidence =
    {
    ID : int
    DBSequenceID : int
    PeptideID : int
    isDecoy : string
    Frame : string
    Start : int
    End : int
    Pre : string
    Post : string
    TranslationsID : int
    }

[<CLIMutable>]
type PeptideEvidenceParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type PeptideHypothesis =
    {
    ID : int
    PeptideEvidenceID : int
    PeptideDetectionHypothesisID : int
    }

[<CLIMutable>]
type PeptideHypothesisParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type Person =
    {
    ID : int
    FirstName : string
    LastName : string
    MiddleName : string
    OrganisationID : int
    }

[<CLIMutable>]
type PersonParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type ProteinAmbiguityGroup =
    {
    ID : int
    ProteinDetectionListID : int
    Name : string
    }

[<CLIMutable>]
type ProteinAmbiguityGroupParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type ProteinDetectionHypothesis =
    {
    ID : int
    DBSequenceID : int
    ProteinAmbiguityGroupID : int
    Name : string
    PassThreshold : string
    }

[<CLIMutable>]
type ProteinDetectionHypothesisParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type ProteinDetectionList =
    {
    ID : int
    Accession : string
    Name : string
    SearchDBID : string

    }

[<CLIMutable>]
type ProteinDetectionListParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type ProteinDetectionProtocol =
    {
    ID : int
    Name : string
    AnalysisSoftwareID : int
    }

[<CLIMutable>]
type ProteinDetectionProtocolParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }
    
[<CLIMutable>]
type SpectrumIdentification =
    {
    ID : int
    Name : string
    ActivityDate : string
    SpectrumIdentificationListID : int
    SpectrumIdentificationProtocollID : int
    }

[<CLIMutable>]
type SpectrumIdentificationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type SpectrumIdentificationItem =
    {
    ID : int
    SpectrumIdentificationResultID : int
    SampleID : int
    PeptideID : int
    MassTableID : int
    Name : string
    PassThreshold : string
    Rank : int
    CalculatedMassToCharge : float
    ExperimentalMassToCharge : float
    ChargeState : int
    CalculatedIP : float
    Fragmentation : string //// Look up
    }

[<CLIMutable>]
type SpectrumIdentificationItemParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type SpectrumIdentificationList =
    {
    ID : int
    Name : string
    NumSequencesSeqrched : int
    }

[<CLIMutable>]
type SpectrumIdentificationListParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type SpectrumIdentificationProtocol =
    {
    ID : int
    Name : string
    AnalysisSoftwareID : int
    }

[<CLIMutable>]
type SpectrumIdentificationProtocoltParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type SpectrumIdentificationResult =
    {
    ID : int
    SpectrumID : int
    SpectraDataID : int
    SpectrumIdentificationListID : int
    Name : string
    }

[<CLIMutable>]
type SpectrumIdentificationResultParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }




//creates OntologyItem with ID, OntologyID and Name
let createOntologyItem id name =
    {
    ID = id
    Name = name
    }

type OntologyContext() =
    inherit DbContext()
    [<DefaultValue>] val mutable m_ontology : DbSet<Ontology>
    member public this.Ontology with get() = this.m_ontology
                                                and set value = this.m_ontology <- value

    override this.OnConfiguring (optionsbuilder :  DbContextOptionsBuilder) =
        optionsbuilder.UseSqlite(@"Data Source=C:\F#-Projects\DavidsDatenbank.db") |> ignore
 
//Condition of grouping lines
let private same_group (l : string) =             
    not (String.length l = 0 || l <> "[Term]") //needs to change

/// Reads FastaItem from file. Converter determines type of sequence by converting seq<char> -> type
let fromFileEnumerator (fileEnumerator) =
    // Matches grouped lines and concatenates them
    let record d = 
        match Seq.item 0 d = "[Term]" with
        |true  ->      let id   = Seq.item 1 d
                       let name = Seq.item 2 d
                       createOntologyItem id name
                                                         
        |false -> raise (System.Exception "Incorrect ontology format")
    
    // main
    fileEnumerator
    |> Seq.groupWhen same_group
    |> Seq.map (fun l -> record l)

let findTerm (arrayOfFile : string seq) =
    let rec loop acc =
        if (acc + 1) = (Seq.length arrayOfFile) then Seq.skip acc arrayOfFile
        else
            if Seq.item acc arrayOfFile = "[Term]"
               then
               (Seq.skip acc arrayOfFile)
            else
            loop (acc+1)
    loop 0

let sqlTestingOntologySequenceTransactions (x : seq<Ontology>) =
    let db = new OntologyContext()
    let timer = new Stopwatch()
    timer.Start()
    for i = 0 to (Seq.length x)-1 do
        db.Add({ID=(Seq.item i x).ID; Name=(Seq.item i x).Name}) |> ignore
    db.SaveChanges() |>ignore
    timer.Stop()
    timer.Elapsed.TotalMilliseconds

/// Reads FastaItem from file. Converter determines type of sequence by converting seq<char> -> type
///Testing
let fromFile (filePath) =
    FileIO.readFile filePath
    |> findTerm
    //|> fromFileEnumerator
    //|> sqlTestingOntologySequenceTransactions

let test = fromFile @"C:\F#-Projects\ParserStuff\Psi-MS.txt"
test

Seq.item 1 test

initDB "C:\F#-Projects\DavidsDatenbank.db"

let everyNth1 (input : 'a seq) = 
    let n = 1
    seq{ use en = input.GetEnumerator()
            // Call MoveNext at most 'n' times (or return false earlier)
         let rec nextN n = 
            if n = 0 then true
            else 
                if en.Current = "[Term]" then
                    en.MoveNext() && (nextN (n - 1))
   
                else en.MoveNext() && (nextN n)
            // While we can move n elements forward...
        while nextN n do
        // Retrun each nth element
        yield en.Current }

let everyNth2 (input : 'a seq) = 
    let n = 1
    seq{ use en = input.GetEnumerator()
            // Call MoveNext at most 'n' times (or return false earlier)
         let rec nextN n = 
            if n = 0 then true
            else 
                if en.Current = "[Term]" then
                    en.MoveNext() && en.MoveNext() && (nextN (n - 1))
   
                else en.MoveNext() && (nextN n)
            // While we can move n elements forward...
        while nextN n do
        // Retrun each nth element
        yield en.Current }

let idSequence = everyNth1 test 
let nameSequence = everyNth2 test
Seq.map2 ( fun x y -> createOntologyItem x y) idSequence nameSequence


let frage (x : 'a seq) =
    if x.Contains("A") then printfn "it contains A"
    else printfn "it does not contain A"

frage ["A";"B"]
