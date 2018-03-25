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

open System
//open System.Diagnostics
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
//open System.Linq
//open BioFSharp.BioItem
open System.Collections.Generic


///Defining Types

type [<CLIMutable>] Ontology = 
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID             : int
    Name           : string
    RowVersion     : DateTime
    OntologyParams : OntologyParam
    Terms          : List<Term>
    }

and [<CLIMutable>] OntologyParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : Ontology
    FKTerm                 : Term
    FKUnit                 : Term
    Value                  : string
    RowVersion             : DateTime
    }

and [<CLIMutable>] SpectrumIdentification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                : int
    Name                              : string 
    ActivityDate                      : string 
    SpectrumIdentificationListID      : int //Refers to SpectrumidentificationList normally
    SpectrumIdentificationProtocollID : int //Refers to SpectrumidentificationProtocoll normally
    RowVersion                        : DateTime
    SpectrumIdentificationParams      : List<SpectrumIdentificationParam>
    }

and [<CLIMutable>] SpectrumIdentificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : SpectrumIdentification
    FKTerm                 : Term
    FKUnit                 : Term
    Value                  : string
    RowVersion             : DateTime
    }

and [<CLIMutable>] Term =
    {
    ID         : string
    Name       : string
    //OntologyID : string
    Ontology   : Ontology
    RowVersion : DateTime 
    }

//and [<CLIMutable>] TermRelationShip =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID : int
//    TermID           : Term
//    RelationShipType : string
//    FKRelatedTerm    : Term
//    RowVersion       : DateTime     
//    }

//and [<CLIMutable>] TermTag =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID         : int
//    TermID     : Term
//    Name       : string
//    Value      : string
//    RowVersion : DateTime 
//    }

///Defining Context of DB
type DBMSContext() =
    inherit DbContext()

    [<DefaultValue>] val mutable m_ontology : DbSet<Ontology>
    member public this.Ontology with get() = this.m_ontology
                                                and set value = this.m_ontology <- value

    [<DefaultValue>] val mutable m_spectrumIdentification : DbSet<SpectrumIdentification>
    member public this.SpectrumIdentification with get() = this.m_spectrumIdentification
                                                    and set value = this.m_spectrumIdentification <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationParam : DbSet<SpectrumIdentificationParam>
    member public this.SpectrumIdentificationParam with get() = this.m_spectrumIdentificationParam
                                                    and set value = this.m_spectrumIdentificationParam <- value

    [<DefaultValue>] val mutable m_term : DbSet<Term>
    member public this.Term with get() = this.m_term
                                        and set value = this.m_term <- value

    //[<DefaultValue>] val mutable m_termRelationShip : DbSet<TermRelationShip>
    //member public this.TermRelationShip with get() = this.m_termRelationShip
    //                                    and set value = this.m_termRelationShip <- value

    //[<DefaultValue>] val mutable m_termTag : DbSet<TermTag>
    //member public this.TermTag with get() = this.m_termTag
    //                                    and set value = this.m_termTag <- value

    override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
        let fileDir = __SOURCE_DIRECTORY__ 
        let dbPath = fileDir + "\Ontologies_Terms\DavidsDatenbank.db"
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore


///Creating/Checking for Database

let initDB =
    let db = new DBMSContext()
    db.Database.EnsureCreated() |> ignore