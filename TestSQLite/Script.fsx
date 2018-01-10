
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\netstandard.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\FSharp.Data.TypeProviders.dll"
//#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\System.Linq.dll"

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
    AbteilungenID : int
    Name : string
    }

[<CLIMutable>]
type Rollen = 
    {
    RollenID : int
    Name : string
    }

[<CLIMutable>] [<DatabaseGeneratedAttribute(DatabaseGeneratedOption.ID)>]
type PersonenVerzeichnis = 
    {
    ID : int
    Name : string
    AbteilungenID : int
    RollenID : int
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
        optionsbuilder.UseSqlite(@"Data Source=C:\Users\PatrickB\Desktop\F#Projects\TestDatenBank.db") |> ignore

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
    db.Add({AbteilungenID=id; Name=name}) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingRollen (id : int) (name : string) =
    let db = new PersonenContext()
    db.Add({RollenID=id; Name=name}) |> ignore
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
open System.Data.SQLite
open BioFSharp.Formula.Table

initDB "C:\Users\PatrickB\Desktop\F#Projects\DavidsDatenbank.db"

///Programming a Parser
//OntologyItem contains ID, OntologyID and Name
type OntologyItem<'a> =
    {
    ID : string
    Name : 'a
    }

//creates OntologyItem with ID, OntologyID and Name
let createOntologyItem id name =
    {
    ID = id
    Name = name
    }

//Condition of grouping lines
let private same_group (l : string) =             
    not (String.length l = 0 || l <> "[Term]") //needs to change

//Tokenizer

/// Reads FastaItem from file. Converter determines type of sequence by converting seq<char> -> type
let fromFileEnumerator (converter:seq<string>-> 'a) (fileEnumerator) =
    // Matches grouped lines and concatenates them
    let record d (converter:seq<string>-> 'a) = 
        match d with
        | [] -> raise (System.Exception "Incorrect FastA format")
        | (h:string) :: t when h.StartsWith "[" ->  let id = t.Head
                                                    let name = t.Item 1
                                                    createOntologyItem id name
                                                        
        | h :: _ -> raise (System.Exception "Incorrect format")        
    
    // main
    fileEnumerator
    |> Seq.filter (fun (l:string) -> not (l.StartsWith ";" || l.StartsWith " "))
    |> Seq.groupWhen same_group 
    |> Seq.map (fun l -> record (List.ofSeq l) converter)
    

/// Reads FastaItem from file. Converter determines type of sequence by converting seq<char> -> type

///Testing
let fromFile converter (filePath) =
    FileIO.readFile filePath
    |> fromFileEnumerator converter

fromFile Seq.toArray "C:\Users\PatrickB\Desktop\F#Projects\TermsToParse\Pi-MS.txt"


let a = ["A";"b";"C"]
match a with
    |[] -> ["a"]
    |(h:string) :: t when h.StartsWith "A" -> let head = h.Remove(0)
                                              let name = t
                                              name