
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

//type [<CLIMutable>] AnalysisSoftware =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                     : int
//    Name                   : string 
//    RowVersion             : DateTime
//    AnalysisSoftwareParams : List<AnalysisSoftwareParam>
//    }


//and [<CLIMutable>] AnalysisSoftwareParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : AnalysisSoftware
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }

//and [<CLIMutable>] DBSequence =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    Accession        : string
//    Name             : string
//    SearchDBID       : string //Refers to SearchDB???
//    RowVersion       : DateTime 
//    DBSequenceParams : List<DBSequenceParam>
//    }

//and [<CLIMutable>] DBSequenceParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : DBSequenceParam
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] ModLocation =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                : int
//    PeptideID         : Peptide
//    ModificationID    : Modification
//    Location          : int
//    Residue           : string
//    RowVersion        : DateTime
//    ModLocationParams : List<ModLocation>
//    }

//and [<CLIMutable>] ModLocationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ModLocation
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] Modification =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                    : int
//    Name                  : string 
//    Residues              : string 
//    MonoisotopicMassDelta : float 
//    AvgMassDelta          : float 
//    RowVersion            : DateTime 
//    ModificationParams    : List<ModificationParam>
//    }

//and [<CLIMutable>] ModificationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Modification
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

type [<CLIMutable>] Ontology = 
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID             : int
    Name           : string
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

//and [<CLIMutable>] Organization =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                 : int
//    Name               : string
//    ParentID           : Parent
//    RowVersion         : DateTime
//    OrganizationParams : List<OrganizationParam>
//    }

//and [<CLIMutable>] OrganizationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Organization
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] Parent =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID           : int
//    Name         : string
//    Country      : string 
//    RowVersion   : DateTime
//    ParentParams : List<ParentParam>
//    }

//and [<CLIMutable>] ParentParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//    ID               : int
//    FKParamContainer : Parent
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime   
//    }

//and [<CLIMutable>] Peptide =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID            : int
//    Sequence      : string
//    RowVersion    : DateTime 
//    PeptideParams : List<PeptideParam>
//    }

//and [<CLIMutable>] PeptideParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Peptide
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }


//and [<CLIMutable>] PeptideEvidence =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                    : int
//    DBSequenceID          : int //Refers to DBSequenceList
//    PeptideID             : Peptide
//    isDecoy               : string 
//    Frame                 : string 
//    Start                 : int 
//    End                   : int 
//    Pre                   : string 
//    Post                  : string 
//    TranslationsID        : int //Refers to Translation
//    RowVersion            : DateTime 
//    PeptideEvidenceParams : List<PeptideEvidenceParam>
//    }

//and [<CLIMutable>] PeptideEvidenceParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : PeptideEvidence
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] PeptideHypothesis =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                           : int
//    PeptideEvidenceID            : PeptideEvidence
//    PeptideDetectionHypothesisID : int //Refers to peptideDetectionHyptothesis
//    RowVersion                   : DateTime  
//    PeptideHypothesisParams      : List<PeptideHypothesisParam>
//    }

//and [<CLIMutable>] PeptideHypothesisParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : PeptideHypothesis
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] Person =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID             : int
//    FirstName      : string 
//    LastName       : string 
//    MiddleName     : string 
//    OrganisationID : int //Refers to Organisation
//    RowVersion     : DateTime  
//    PersonParams   : List<PersonParam>
//    }

//and [<CLIMutable>] PersonParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Person
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime   
//    }

//and [<CLIMutable>] ProteinAmbiguityGroup =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                          : int
//    ProteinDetectionListID      : int //Refers to ProteinList
//    Name                        : string 
//    RowVersion                  : DateTime
//    ProteinAmbiguityGroupParams : List<ProteinAmbiguityGroupParam>
//    }

//and [<CLIMutable>] ProteinAmbiguityGroupParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinAmbiguityGroup
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime     
//    }

//and [<CLIMutable>] ProteinDetectionHypothesis =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                               : int
//    DBSequenceID                     : int //Refers to DBSequenceList
//    ProteinAmbiguityGroupID          : ProteinAmbiguityGroup
//    Name                             : string 
//    PassThreshold                    : string
//    RowVersion                       : DateTime  
//    ProteinDetectionHypothesisParams : List<ProteinDetectionHypothesisParam>
//    }

//and [<CLIMutable>] ProteinDetectionHypothesisParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinDetectionHypothesis
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime
//    }

//and [<CLIMutable>] ProteinDetectionList =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                         : int
//    Accession                  : string
//    Name                       : string
//    SearchDBID                 : string //Refers to SearchDBList
//    RowVersion                 : DateTime
//    ProteinDetectionListParams : List<ProteinDetectionListParam>
//    }

//and [<CLIMutable>] ProteinDetectionListParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinDetectionList
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime
//    }

//and [<CLIMutable>] ProteinDetectionProtocol =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                             : int
//    Name                           : string 
//    AnalysisSoftwareID             : int
//    RowVersion                     : DateTime
//    ProteinDetectionProtocolParams : List<ProteinDetectionProtocolParam>
//    }

//and [<CLIMutable>] ProteinDetectionProtocolParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinDetectionProtocol
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }
    
and [<CLIMutable>] SpectrumIdentification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                : int
    Name                              : string 
    ActivityDate                      : string 
    SpectrumIdentificationListID      : int //Refers to SpectrumidentificationList normally
    SpectrumIdentificationProtocollID : int //Refers to SpectrumIdentificationProtocol normally
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

//and [<CLIMutable>] SpectrumIdentificationItem =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                               : int
//    SpectrumIdentificationResultID   : SpectrumIdentificationResult
//    SampleID                         : int 
//    PeptideID                        : Peptide
//    MassTableID                      : int //Refers to MassTableList
//    Name                             : string 
//    PassThreshold                    : string
//    Rank                             : int 
//    CalculatedMassToCharge           : float 
//    ExperimentalMassToCharge         : float
//    ChargeState                      : int
//    CalculatedIP                     : float 
//    Fragmentation                    : DateTime  
//    RowVersion                       : DateTime
//    SpectrumIdentificationItemParams : List<SpectrumIdentificationItemParam>
//    }

//and [<CLIMutable>] SpectrumIdentificationItemParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SpectrumIdentificationItem
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationList =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                               : int
//    Name                             : string 
//    NumSequencesSeqrched             : int 
//    RowVersion                       : DateTime
//    SpectrumIdentificationListParams : List<SpectrumIdentificationListParam>
//    }

//and [<CLIMutable>] SpectrumIdentificationListParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SpectrumIdentificationList
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationProtocol =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                                   : int
//    Name                                 : string 
//    AnalysisSoftwareID                   : int
//    RowVersion                           : DateTime 
//    SpectrumIdentificationProtocolParams : List<SpectrumIdentificationProtocolParam>
//    }

//and [<CLIMutable>] SpectrumIdentificationProtocolParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SpectrumIdentificationProtocol
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationResult =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                           : int
//    SpectrumID                   : int//Refers to SpectrumIdentificationID
//    //SpectraDataID : string // quetionable???
//    SpectrumIdentificationListID : int 
//    Name                         : string 
//    RowVersion                   : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationResultParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : int
//    FKTerm           : Term
//    FKUnit           : Term
//    Value            : string
//    RowVersion       : DateTime    
//    }

and [<CLIMutable>] Term =
    {
    ID             : string
    Name           : string
    Ontology       : Ontology
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

    //[<DefaultValue>] val mutable m_spectrumIdentificationItem : DbSet<SpectrumIdentificationItem>
    //member public this.SpectrumIdentificationItem with get() = this.m_spectrumIdentificationItem
    //                                                and set value = this.m_spectrumIdentificationItem <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationItemParam : DbSet<SpectrumIdentificationItemParam>
    //member public this.SpectrumIdentificationItemParam with get() = this.m_spectrumIdentificationItemParam
    //                                                   and set value = this.m_spectrumIdentificationItemParam <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationList : DbSet<SpectrumIdentificationList>
    //member public this.SpectrumIdentificationList with get() = this.m_spectrumIdentificationList
    //                                                and set value = this.m_spectrumIdentificationList <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationListParam : DbSet<SpectrumIdentificationListParam>
    //member public this.SpectrumIdentificationListParam with get() = this.m_spectrumIdentificationListParam
    //                                                    and set value = this.m_spectrumIdentificationListParam <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationProtocol : DbSet<SpectrumIdentificationProtocol>
    //member public this.SpectrumIdentificationProtocol with get() = this.m_spectrumIdentificationProtocol
    //                                                    and set value = this.m_spectrumIdentificationProtocol <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationProtocolParam : DbSet<SpectrumIdentificationProtocolParam>
    //member public this.SpectrumIdentificationProtocolParam with get() = this.m_spectrumIdentificationProtocolParam
    //                                                        and set value = this.m_spectrumIdentificationProtocolParam <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationResult : DbSet<SpectrumIdentificationResult>
    //member public this.SpectrumIdentificationResult with get() = this.m_spectrumIdentificationResult
    //                                                and set value = this.m_spectrumIdentificationResult <- value

    //[<DefaultValue>] val mutable m_spectrumIdentificationResultParam : DbSet<SpectrumIdentificationResultParam>
    //member public this.SpectrumIdentificationResultParam with get() = this.m_spectrumIdentificationResultParam
    //                                                        and set value = this.m_spectrumIdentificationResultParam <- value

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
                 Ontology.ID             = 0
                 Ontology.Name           = "Psi-MS"
                 Ontology.RowVersion     = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms          = new System.Collections.Generic.List<Term>
                    (fromPsiMS
                    |> Seq.map (fun item -> {
                                             Term.ID             = item.Id; 
                                             Term.Name           = item.Name; 
                                             Term.Ontology       = {
                                                                      Ontology.ID             = 0 
                                                                      Ontology.Name           = "Psi-MS" 
                                                                      Ontology.RowVersion     = DateTime.Now.Date 
                                                                      Ontology.OntologyParams = null 
                                                                      Ontology.Terms          = null
                                                                   } 
                                             Term.RowVersion     = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let ontologyPride = 
                {
                 Ontology.ID             = 0
                 Ontology.Name           = "Pride"
                 Ontology.RowVersion     = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms          = new System.Collections.Generic.List<Term>
                    (fromPride
                    |> Seq.map (fun item -> {
                                             Term.ID             = item.Id; 
                                             Term.Name           = item.Name; 
                                             Term.Ontology       = {
                                                                      Ontology.ID             = 0 
                                                                      Ontology.Name           = "Pride" 
                                                                      Ontology.RowVersion     = DateTime.Now.Date 
                                                                      Ontology.OntologyParams = null 
                                                                      Ontology.Terms          = null
                                                                   } 
                                             Term.RowVersion     = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let ontologyUniMod = 
                {
                 Ontology.ID             = 0
                 Ontology.Name           = "UniMod"
                 Ontology.RowVersion     = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms          = new System.Collections.Generic.List<Term>
                    (fromUniMod
                    |> Seq.map (fun item -> {
                                             Term.ID             = item.Id 
                                             Term.Name           = item.Name 
                                             Term.Ontology       = {
                                                                      Ontology.ID             = 0 
                                                                      Ontology.Name           = "UniMod" 
                                                                      Ontology.RowVersion     = DateTime.Now.Date 
                                                                      Ontology.OntologyParams = null 
                                                                      Ontology.Terms          = null
                                                                   } 
                                             Term.RowVersion     = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let ontologyUnit_Ontology = 
                {
                 Ontology.ID             = 0
                 Ontology.Name           = "Unit_Ontology"
                 Ontology.RowVersion     = DateTime.Now.Date
                 Ontology.OntologyParams = null
                 Ontology.Terms          = new System.Collections.Generic.List<Term>
                    (fromUnit_Ontology
                    |> Seq.map (fun item -> {
                                             Term.ID             = item.Id; 
                                             Term.Name           = item.Name; 
                                             Term.Ontology       = {
                                                                      Ontology.ID             = 0 
                                                                      Ontology.Name           = "Unit_Ontology"
                                                                      Ontology.RowVersion     = DateTime.Now.Date 
                                                                      Ontology.OntologyParams = null 
                                                                      Ontology.Terms          = null
                                                                   } 
                                             Term.RowVersion     = DateTime.Now.Date; 
                                             Term.OntologyParams = null
                                            }
                                )
                    )
                }

let Unitless = {
                Ontology.ID             = -1
                Ontology.Name           = ""
                Ontology.RowVersion     = DateTime.Now.Date
                Ontology.OntologyParams = null
                Ontology.Terms          = new System.Collections.Generic.List<Term>
                    ([{
                       Term.ID             = ""; 
                       Term.Name           = ""; 
                       Term.Ontology       = {
                                                Ontology.ID             = -1 
                                                Ontology.Name           = "" 
                                                Ontology.RowVersion     = DateTime.Now.Date
                                                Ontology.OntologyParams = null 
                                                Ontology.Terms          = null
                                             } 
                       Term.RowVersion     = DateTime.Now.Date; 
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