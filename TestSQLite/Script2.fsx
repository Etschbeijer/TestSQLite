
///Another Test
#I @"..\packages\Microsoft.AspNet.Identity.EntityFramework.2.2.1\lib\net45"
#I @"..\packages\Microsoft.AspNet.Identity.Core.2.2.1\lib\net45\"
#r "Microsoft.AspNet.Identity.Core.dll"
#r "Microsoft.AspNet.Identity.EntityFramework.dll"
#r "System.ComponentModel.DataAnnotations.dll"
#r @"..\TestSQLite\bin\Debug\netstandard.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Relational.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r @"..\TestSQLite\bin\Debug\System.Data.SQLite.dll"
//#r @"..\TestSQLite\bin\Debug\BioFSharp.dll"
//#r @"..\TestSQLite\bin\Debug\BioFSharp.IO.dll"
//#r @"..\TestSQLite\bin\Debug\BioFSharp.Mz.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.IO.dll"

//open System
//open System.Diagnostics
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
//open System.Linq
//open BioFSharp.BioItem
open System.Collections.Generic
open FSharp.Care.IO



//open FSharp.Plotly
//open FSharp.Plotly.HTML


///Defining the Types/////////////////////////
//////////////////////////////////////////////////////////////////

type PersonenVerzeichnis(personenVerzeichnisid : int, name : string, abteilungen : Abteilung, rollen : Rolle) =
    let mutable personenVerzeichnisid = personenVerzeichnisid
    let mutable name                  = name
    let mutable abteilungen   = abteilungen
    let mutable rollen        = rollen
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member this.PersonenVerzeichnisID with get() = personenVerzeichnisid and set(value) = personenVerzeichnisid <- value
    member this.Name          with get()         = name          and set(value)         = name                  <- value
    member this.Abteilungen   with get()         = abteilungen   and set(value)         = abteilungen           <- value
    member this.Rollen        with get()         = rollen        and set(value)         = rollen                <- value
   
and Abteilung(abteilungid : int, name : string, personenVerzeichnis : List<PersonenVerzeichnis>) =
    let mutable abteilungID       = abteilungid
    let mutable name                = name
    //let mutable personenVerzeichnis = personenVerzeichnis
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member this.AbteilungID with get()         = abteilungID and set(value)         = abteilungID       <- value
    member this.Name        with get()         = name          and set(value)       = name              <- value
    //member this.PersonenVerzeichnis with get() = personenVerzeichnis and set(value) = personenVerzeichnis <- value

and Rolle(rollenid : int, name : string, personenVerzeichnis : List<PersonenVerzeichnis>) =
    let mutable rolleID            = rollenid
    let mutable name                = name
    //let mutable personenVerzeichnis = personenVerzeichnis
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    member this.RolleID with get()             = rolleID and set(value)             = rolleID            <- value
    member this.Name    with get()             = name     and set(value)            = name               <- value
    //member this.PersonenVerzeichnis with get() = personenVerzeichnis and set(value) = personenVerzeichnis <- value

type PersonenContext() =
    inherit DbContext()
    
    [<DefaultValue>] 
    val mutable m_abteilungen : DbSet<Abteilung>
    member public this.Abteilung with get() = this.m_abteilungen
                                                and set value = this.m_abteilungen <- value

    [<DefaultValue>] 
    val mutable m_rollen : DbSet<Rolle>
    member public this.Rolle with get() = this.m_rollen
                                           and set value = this.m_rollen <- value

    [<DefaultValue>] 
    val mutable m_personenverzeichnis : DbSet<PersonenVerzeichnis>
    member public this.PersonenVerzeichnis with get() = this.m_personenverzeichnis
                                                        and set value = this.m_personenverzeichnis <- value
    
    override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
        let fileDir = __SOURCE_DIRECTORY__ 
        let dbPath = fileDir + "\Ontologies_Terms\PersonenDatenbank.db"
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore

let initDB =
    let db = new PersonenContext()
    db.Database.EnsureCreated() |> ignore
    let abteilung1 = new Abteilung()
    let rolle1     = new Rolle()
    let PersonenVerzeichnis1 = new PersonenVerzeichnis(0, "Bob", new Abteilung(), new Rolle())
    db.PersonenVerzeichnis.Add(PersonenVerzeichnis1)
