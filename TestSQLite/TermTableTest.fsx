
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
//#r @"..\TestSQLite\bin\Debug\BioFSharp.Mz.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.IO.dll"

let fileDir = __SOURCE_DIRECTORY__

open System
//open System.Diagnostics
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
//open System.Linq
//open BioFSharp.BioItem
open System.Collections.Generic
open FSharp.Care.IO
open BioFSharp.IO
///Defining Types

type [<CLIMutable>] Ontology = 
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID             : int
    [<Column("Name")>]
    OntologyName   : string
    RowVersion     : DateTime
    OntologyParams : List<OntologyParam>
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
    ID             : string
    Name           : string
    //OntologyID : string
    (*[<ForeignKey("OntologyName")>]*)
    Ontology     : Ontology
    RowVersion     : DateTime 
    OntologyParams : List<OntologyParam>
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

    [<DefaultValue>] val mutable m_ontologyParam : DbSet<OntologyParam>
    member public this.OntologyParam with get() = this.m_ontologyParam
                                                and set value = this.m_ontologyParam <- value

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

    override this.OnModelCreating (modelbuilder : ModelBuilder) =
        modelbuilder.Entity<OntologyParam>()
            .HasOne(fun field -> field.FKTerm)
            .WithMany("OntologyParams")
            .IsRequired(true) |> ignore

    override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
        let fileDir = __SOURCE_DIRECTORY__ 
        let dbPath = fileDir + "\Ontologies_Terms\MSDatenbank.db"
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore

///Define reader for OboFile
let fromFileObo (filePath : string) =
    FileIO.readFile filePath
    |> Obo.parseOboTerms

let fromPsiMS =
    fromFileObo (fileDir + "\Ontologies_Terms\Psi-MS.txt")

let fromPride =
    fromFileObo (fileDir + "\Ontologies_Terms\Pride.txt")

let fromUniMod =
    fromFileObo (fileDir + "\Ontologies_Terms\Unimod.txt")

let fromUnit_Ontology =
    fromFileObo (fileDir + "\Ontologies_Terms\Unit_Ontology.txt")

///Define Types to insert into DB
let ontologyPsiMS = 
                {
                 Ontology.ID = 0
                 Ontology.OntologyName = "Psi-MS"
                 Ontology.RowVersion = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms = new System.Collections.Generic.List<Term>
                    (fromPsiMS
                    |> Seq.map (fun item -> {
                                             Term.ID = item.Id; 
                                             Term.Name = item.Name; 
                                             Term.Ontology = {
                                                              Ontology.ID = 0; 
                                                              Ontology.OntologyName = "Psi-MS"; 
                                                              Ontology.RowVersion = DateTime.Now.Date; 
                                                              Ontology.OntologyParams = null; 
                                                              Ontology.Terms = null
                                                             }; 
                                             Term.RowVersion = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let ontologyPride = 
                {
                 Ontology.ID = 0
                 Ontology.OntologyName = "Pride"
                 Ontology.RowVersion = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms = new System.Collections.Generic.List<Term>
                    (fromPride
                    |> Seq.map (fun item -> {
                                             Term.ID = item.Id; 
                                             Term.Name = item.Name; 
                                             Term.Ontology = {
                                                              Ontology.ID = 0; 
                                                              Ontology.OntologyName = "Pride"; 
                                                              Ontology.RowVersion = DateTime.Now.Date; 
                                                              Ontology.OntologyParams = null; 
                                                              Ontology.Terms = null
                                                             }; 
                                             Term.RowVersion = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let ontologyUniMod = 
                {
                 Ontology.ID = 0
                 Ontology.OntologyName = "UniMod"
                 Ontology.RowVersion = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms = new System.Collections.Generic.List<Term>
                    (fromUniMod
                    |> Seq.map (fun item -> {
                                             Term.ID = item.Id; 
                                             Term.Name = item.Name; 
                                             Term.Ontology = {
                                                              Ontology.ID = 0; 
                                                              Ontology.OntologyName = "UniMod"; 
                                                              Ontology.RowVersion = DateTime.Now.Date; 
                                                              Ontology.OntologyParams = null; 
                                                              Ontology.Terms = null
                                                             }; 
                                             Term.RowVersion = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let ontologyUnit_Ontology = 
                {
                 Ontology.ID = 0
                 Ontology.OntologyName = "Unit_Ontology"
                 Ontology.RowVersion = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms = new System.Collections.Generic.List<Term>
                    (fromUnit_Ontology
                    |> Seq.map (fun item -> {
                                             Term.ID = item.Id; 
                                             Term.Name = item.Name; 
                                             Term.Ontology = {
                                                              Ontology.ID = 0; 
                                                              Ontology.OntologyName = "Unit_Ontology"; 
                                                              Ontology.RowVersion = DateTime.Now.Date; 
                                                              Ontology.OntologyParams = null; 
                                                              Ontology.Terms = null
                                                             }; 
                                             Term.RowVersion = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let Unitless = {
                Ontology.ID = -1
                Ontology.OntologyName = ""
                Ontology.RowVersion = DateTime.Now.Date
                Ontology.OntologyParams = null
                Ontology.Terms = new System.Collections.Generic.List<Term>
                    ([{
                       Term.ID = ""; 
                       Term.Name = ""; 
                       Term.Ontology = {
                                        Ontology.ID = -1; 
                                        Ontology.OntologyName = ""; 
                                        Ontology.RowVersion = DateTime.Now.Date; 
                                        Ontology.OntologyParams = null; 
                                        Ontology.Terms = null
                                       }; 
                       Term.RowVersion = DateTime.Now.Date; 
                       Term.OntologyParams = null
                   }])
               }

//let ontoTest2 = {
//                 Ontology.ID = 0
//                 Ontology.OntologyName = ""
//                 Ontology.RowVersion = DateTime.Now.Date
//                 Ontology.OntologyParams = null
//                 Ontology.Terms = null
//                }
//let termTest1 = {
//                 Term.ID = "Testi"
//                 Term.Name = "TestOntology"
//                 Term.RowVersion = DateTime.Now.Date
//                 Term.Ontology = ontologyPsiMS
//                 Term.OntologyParams = null
//                }
//let termTest2 = {
//                 Term.ID = ""
//                 Term.Name = "TestOntology"
//                 Term.RowVersion = DateTime.Now.Date
//                 Term.Ontology = ontoTest2
//                 Term.OntologyParams = null
//                }
//let ontoParamTest = {
//                     OntologyParam.ID = 0
//                     OntologyParam.Value = "Zero"
//                     OntologyParam.FKParamContainer = ontologyPsiMS
//                     OntologyParam.FKTerm = termTest1
//                     OntologyParam.FKUnit = termTest2
//                     OntologyParam.RowVersion = DateTime.Now.Date
//                    }

///Creating && Checking && inserting into DB
let initDB =
    let db = new DBMSContext()
    db.Database.EnsureCreated() |> ignore

    db.Ontology.Add(ontologyPsiMS)         |> ignore
    db.Ontology.Add(ontologyPride)         |> ignore
    db.Ontology.Add(ontologyUniMod)        |> ignore
    db.Ontology.Add(ontologyUnit_Ontology) |> ignore
    db.Ontology.Add(Unitless)              |> ignore
    db.SaveChanges()
    
//type Test =
//    {Variable : int}

//let x = List<Test>().FirstOrDefault()
//if (box x = null) then None else Some(x)