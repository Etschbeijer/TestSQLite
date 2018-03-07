
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
#r @"..\TestSQLite\bin\Debug\BioFSharp.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.IO.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.Mz.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.IO.dll"

//open System
//open System.Diagnostics
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
//open System.Linq
//open BioFSharp.BioItem
open System.Collections.Generic
open Microsoft.EntityFrameworkCore.Storage
open Microsoft.EntityFrameworkCore.Migrations
open BioFSharp.Mz.MzIdentMLModel.Db

//open FSharp.Plotly
//open FSharp.Plotly.HTML


///Defining the Types/////////////////////////
//////////////////////////////////////////////////////////////////

//type PersonenVerzeichnis(personenVerzeichnisid : int, name : string, abteilungen : Abteilungen, rollen : Rollen) =
//    let mutable personenVerzeichnisid = personenVerzeichnisid
//    let mutable name                  = name
//    //let mutable abteilungenID = abteilungsid
//    //let mutable rollenID      = rollenid
//    let mutable abteilungen   = abteilungen
//    let mutable rollen        = rollen
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//    member this.PersonenVerzeichnisID with get() = personenVerzeichnisid and set(value) = personenVerzeichnisid <- value
//    member this.Name          with get()         = name          and set(value)         = name                  <- value
////  member this.AbteilungenID with get()         = abteilungenID and set(value)         = abteilungenID         <- value
////  member this.RollenID      with get()         = rollenID      and set(value)         = rollenID              <- value
//    member this.Abteilungen   with get()         = abteilungen   and set(value)         = abteilungen           <- value
//    member this.Rollen        with get()         = rollen        and set(value)         = rollen                <- value
   
//and Abteilungen(abteilungenid : int, name : string, personenVerzeichnis : List<PersonenVerzeichnis>) =
//    let mutable abteilungenID       = abteilungenid
//    let mutable name                = name
//    let mutable personenVerzeichnis = personenVerzeichnis
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//    member this.AbteilungenID with get()       = abteilungenID and set(value)       = abteilungenID       <- value
//    member this.Name          with get()       = name          and set(value)       = name                <- value
//    member this.PersonenVerzeichnis with get() = personenVerzeichnis and set(value) = personenVerzeichnis <- value

//and Rollen(rollenid : int, name : string, personenVerzeichnis : List<PersonenVerzeichnis>) =
//    let mutable rollenID            = rollenid
//    let mutable name                = name
//    let mutable personenVerzeichnis = personenVerzeichnis
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//    member this.RollenID with get()            = rollenID and set(value)            = rollenID            <- value
//    member this.Name     with get()            = name     and set(value)            = name                <- value
//    member this.PersonenVerzeichnis with get() = personenVerzeichnis and set(value) = personenVerzeichnis <- value

[<CLIMutable>]
type PersonenVerzeichnis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    PersonenVerzeichnisID : int
    Name                  : string
    [<ForeignKey("FKAbteilung")>]
    Abteilung             : Abteilung
    //[<ForeignKey("AbteilungID2")>]
    //FKUnit                : Abteilung
    Rolle                 : Rolle
    //[<PrimaryKey(TypeName = "Abteilung")>]
    Somethings            : List<Something>
    }

and [<CLIMutable>] 
    Abteilung =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID                  : int
    Name                : string
    Rollen              : List<Something>
    FKAbteilung         : List<PersonenVerzeichnis>
    //[<Column("FKUnit")>]
    FKUnit              : List<PersonenVerzeichnis>
    }

and [<CLIMutable>] 
    Rolle =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    RolleID             : int
    Name                : string
    Abteilungen         : List<Something>
    //PersonenVerzeichnis : List<PersonenVerzeichnis>
    }

and [<CLIMutable>] 
    Something =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID             : int
    Name           : string
    //PersonenVerzeichnis : List<PersonenVerzeichnis>
    }
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
    /// Read up again EntitySplitting
    override this.OnModelCreating (modelbuilder : ModelBuilder) =
        modelbuilder.Entity<PersonenVerzeichnis>()
            .HasOne(fun field -> field.Abteilung)
            .WithMany("FKAbteilung")
            .IsRequired() |> ignore
        modelbuilder.Entity<PersonenVerzeichnis>()
            .Property<System.Nullable<int>>("AbteilungID")
            .HasColumnName<System.Nullable<int>>("FKUnit")
            .IsRequired(false) |> ignore
        modelbuilder.Entity<PersonenVerzeichnis>()
            .Property(fun field -> field.Somethings)
            .

        modelbuilder.Entity<Something>()
            .ToTable<Something>("Stuff") |> ignore
        //modelbuilder.Entity<PersonenVerzeichnis>()
        //    .ToTable<PersonenVerzeichnis>("Abteilung") |> ignore
            
            //.Property(fun x -> x.FKUnit).HasColumnName<List<PersonenVerzeichnis>>("FKUnit") |> ignore
       
            //.ToTable("FKUnit") |> ignore
            //|> (fun x -> 
            //        x.Property(fun personItem -> 
            //            personItem.Somethings |> Seq.map 
            //                    (fun abteilungField ->
            //                        {
            //                         Abteilung.ID = abteilungField.ID 
            //                         Abteilung.Name = abteilungField.Name 
            //                         Abteilung.Rollen = null
            //                        }
            //                    )
            //                  ) |> ignore
            //        x.ToTable("Abteilung")    
            //    ) |> ignore


    override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
        let fileDir = __SOURCE_DIRECTORY__ 
        let dbPath = fileDir + "\Ontologies_Terms\PersonenDatenbank.db"
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore

///Working with Database

let initDB =
    let db = new PersonenContext()
    db.Database.EnsureCreated() |> ignore
    let abteilungen1 =
        {
        Abteilung.ID     = 0
        Abteilung.Name   = "BoB"
        Abteilung.Rollen = null
        Abteilung.FKAbteilung = null
        Abteilung.FKUnit = null
        }
    let abteilungen2 =
        {
        Abteilung.ID     = 0
        Abteilung.Name   = "BoB2"
        Abteilung.Rollen = null
        Abteilung.FKAbteilung = null
        Abteilung.FKUnit = null
        }
    let something1 =
        {
        Something.ID   = 0
        Something.Name = "NOOOOO"

        //Abteilung.PersonenVerzeichnis = null
        }
    let something2 =
        {
        Something.ID   = 0
        Something.Name = "JOOOO"
        //Abteilung.PersonenVerzeichnis = null
        }
    let Rollen =
        {
        Rolle.RolleID     = 0
        Rolle.Name        = "BoB"
        Rolle.Abteilungen = null
        //Rolle.PersonenVerzeichnis = null
        }
    let PersonenVerzeichnis =
        {
        PersonenVerzeichnis.PersonenVerzeichnisID = 0
        PersonenVerzeichnis.Name = "BoB"
        PersonenVerzeichnis.Abteilung = abteilungen1
        //PersonenVerzeichnis.FKUnit = null
        PersonenVerzeichnis.Rolle = Rollen
        PersonenVerzeichnis.Somethings = new System.Collections.Generic.List<Something>([something1;something2])
        }
    //let PersonenVerzeichnis2 =
    //    {
    //    PersonenVerzeichnis.PersonenVerzeichnisID = 0
    //    PersonenVerzeichnis.Name = "BoB"
    //    PersonenVerzeichnis.Abteilung = Abteilungen2
    //    //PersonenVerzeichnis.FKUnit = 3
    //    PersonenVerzeichnis.Rolle = Rollen
    //    }
    db.PersonenVerzeichnis.Add(PersonenVerzeichnis) |> ignore
    //db.PersonenVerzeichnis.Add(PersonenVerzeichnis2) |> ignore
    db.SaveChanges()

///Take Elements of List in Type and fill in right Table
/// Have a Lsit of Spesen which is put into Table of Abteilung when put in PersonenVerzeichnis but put in Rolle when put in Abteilung!!!