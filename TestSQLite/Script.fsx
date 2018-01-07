
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\netstandard.dll"
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\FSharp.Data.TypeProviders.dll"
#r @"C:\Users\Patrick\source\repos\TestSQLite\TestSQLite\bin\Debug\System.Linq.dll"

open System
open System.Diagnostics
open Microsoft.EntityFrameworkCore
open System.Linq

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

//type PersonenVerzeichnis(id : int, name : string, abteilungsid : int, rollenid : int) =

//    let mutable id           = id
//    let mutable name         = name
//    let mutable abteilungsID = abteilungsid
//    let mutable rollenID     = rollenid
//    member this.ID           with get() = id           and set(value) = id           <- value
//    member this.Name         with get() = name         and set(value) = name         <- value
//    member this.AbteilungsID with get() = abteilungsID and set(value) = abteilungsID <- value
//    member this.RollenID     with get() = rollenID     and set(value) = rollenID     <- value

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

[<CLIMutable>]
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
        optionsbuilder.UseSqlite(@"Data Source=C:\F#-Projects\TestDatenBank11.db") |> ignore

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
    db.Add({ID=id; Name=name; AbteilungenID=1;RollenID=1}) |> ignore
    let count = db.SaveChanges()
    printfn "%i records saved to database" count

let sqlTestingPersonenVerzeichnisMultipleTransactions (x : int) =
    let db = new PersonenContext()
    let timer = new Stopwatch()
    timer.Start()
    for i = 1 to x do
        db.Add({ID=i; Name=(sprintf "Bob%i") i; AbteilungenID=1;RollenID=1}) |> ignore
    db.SaveChanges() |>ignore
    timer.Stop()
    timer.Elapsed.TotalSeconds

program 3 "http://sample.com"
program2 3 "jo" "joo" 1

sqlTestingAbteilungen 1 "BioTech"
sqlTestingRollen 1 "Professor"
sqlTestingPersonenVerzeichnis 1 "Bob"

sqlTestingPersonenVerzeichnisMultipleTransactions 1000000

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

