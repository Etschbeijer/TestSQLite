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


open System
open System.Diagnostics
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
//open Microsoft.EntityFrameworkCore.Migrations.Operations
open System.Linq
//open FSharp.Plotly
//open FSharp.Plotly.HTML
//open BioFSharp
//open FSharp
//open FSharp.Care.Collections
open FSharp.Care.IO
//open System
//open System.Data
//open System.IO
//open System.Data.SQLite
//open BioFSharp.Formula.Table
//open BioFSharp.BioID.FastA
//open FSharp.Care.IO.SchemaReader
//open BioFSharp.ModificationInfo
open BioFSharp.IO.Obo


let fileDir = __SOURCE_DIRECTORY__ 
let dbPath = fileDir + "\Ontologies_Terms\DavidsDatenbank.db"

///types for the DataBank

type PrimaryKeyAttribute() =
    class
        inherit Attribute()
    end
//[<PrimaryKey>]

[<CLIMutable>]
type AnalysisSoftware =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name       : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type AnalysisSoftwareParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime  
    }

[<CLIMutable>]
type DBSequence =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Accession  : string
    Name       : string
    SearchDBID : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type DBSequenceParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type ModLocation =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    PeptideID      : int
    ModificationID : int
    Location       : int
    Residue        : string
    RowVersion     : DateTime 
    }

[<CLIMutable>]
type ModLocationParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type Modification =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name                  : string 
    Residues              : string 
    MonoisotopicMassDelta : float 
    AvgMassDelta          : float 
    RowVersion            : DateTime 
    }

[<CLIMutable>]
type ModificationParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    }

[<CLIMutable>]
type Ontology = 
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name       : string
    RowVersion : DateTime
    }

[<CLIMutable>]
type Organization =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name       : string
    ParentID   : int
    RowVersion : DateTime 
    }

[<CLIMutable>] 
type OrganizationParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string option
    Value            : string
    RowVersion       : DateTime 
    }

let s2 = Option<string>.None

//[<AllowNullLiteral>] // Propably a Solution
//type OrganizationParam (id : int, fkParamContainer : int, fkTerm : string, value : string, rowVersion : DateTime, fkUnit : string option) =
//    [<PrimaryKey>]
//    let mutable id               = id
//    let mutable fkParamContainer = fkParamContainer
//    let mutable fkTerm           = fkTerm
//    let mutable valuE            = value
//    let mutable fkUnit           = fkUnit
//    let mutable rowVersion       = rowVersion
//    [<PrimaryKey>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] member this.ID with get() = id and set(value) = id <- value
//    member this.FKParamContainer with get() = fkParamContainer and set(value) = fkParamContainer <- value
//    member this.FKTerm           with get() = fkTerm           and set(value) = fkTerm           <- value
//    member this.Value            with get() = valuE            and set(value) = valuE            <- value
//    member this.FKUnit           with get() = fkUnit           and set(value) = fkUnit           <- value
//    member this.RowVersion       with get() = rowVersion       and set(value) = rowVersion       <- value


[<CLIMutable>]
type Parent =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name       : string
    Country    : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ParentParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime   
    }

[<CLIMutable>]
type Peptide =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Sequence   : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime  
    }

[<CLIMutable>]
type PeptideEvidence =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    DBSequenceID   : int
    PeptideID      : int
    isDecoy        : string 
    Frame          : string 
    Start          : int 
    End            : int 
    Pre            : string 
    Post           : string 
    TranslationsID : int 
    RowVersion     : DateTime 
    }

[<CLIMutable>]
type PeptideEvidenceParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type PeptideHypothesis =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    PeptideEvidenceID            : int
    PeptideDetectionHypothesisID : int 
    RowVersion                   : DateTime    
    }

[<CLIMutable>]
type PeptideHypothesisParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type Person =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FirstName      : string 
    LastName       : string 
    MiddleName     : string 
    OrganisationID : int
    RowVersion     : DateTime    
    }

[<CLIMutable>]
type PersonParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime   
    }

[<CLIMutable>]
type ProteinAmbiguityGroup =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    ProteinDetectionListID : int
    Name                   : string 
    RowVersion             : DateTime
    }

[<CLIMutable>]
type ProteinAmbiguityGroupParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime     
    }

[<CLIMutable>]
type ProteinDetectionHypothesis =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    DBSequenceID            : int
    ProteinAmbiguityGroupID : int
    Name                    : string 
    PassThreshold           : string
    RowVersion              : DateTime     
    }

[<CLIMutable>]
type ProteinDetectionHypothesisParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime
    }

[<CLIMutable>]
type ProteinDetectionList =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Accession  : string
    Name       : string
    SearchDBID : string
    RowVersion : DateTime    
    }

[<CLIMutable>]
type ProteinDetectionListParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime
    }

[<CLIMutable>]
type ProteinDetectionProtocol =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name               : string 
    AnalysisSoftwareID : int
    RowVersion         : DateTime  
    }

[<CLIMutable>]
type ProteinDetectionProtocolParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime  
    }
    
[<CLIMutable>]
type SpectrumIdentification =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name                              : string 
    ActivityDate                      : string 
    SpectrumIdentificationListID      : int
    SpectrumIdentificationProtocollID : int
    RowVersion                        : DateTime   
    }

[<CLIMutable>]
type SpectrumIdentificationParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime    
    }

[<CLIMutable>]
type SpectrumIdentificationItem =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    SpectrumIdentificationResultID : int 
    SampleID                       : int 
    PeptideID                      : int
    MassTableID                    : int 
    Name                           : string 
    PassThreshold                  : string
    Rank                           : int 
    CalculatedMassToCharge         : float 
    ExperimentalMassToCharge       : float
    ChargeState                    : int
    CalculatedIP                   : float 
    Fragmentation                  : DateTime  
    RowVersion                     : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationItemParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationList =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name                 : string 
    NumSequencesSeqrched : int 
    RowVersion           : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationListParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationProtocol =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name               : string 
    AnalysisSoftwareID : int
    RowVersion         : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationProtocolParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationResult =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    SpectrumID                   : int
    //SpectraDataID : string // quetionable???
    SpectrumIdentificationListID : int 
    Name                         : string 
    RowVersion                   : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationResultParam =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm           : string
    FKUnit           : string
    Value            : string
    RowVersion       : DateTime     
    }

[<CLIMutable>]
type Term =
    {
    [<Key>] ID         : string
    Name       : string
    OntologyID : int
    RowVersion : DateTime 
    }

[<CLIMutable>]
type TermRelationShip =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    TermID           : string //propably int, need to check
    RelationShipType : string
    FKRelatedTerm    : string
    RowVersion       : DateTime     
    }

[<CLIMutable>]
type TermTag =
    {
    [<Key>] [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    TermID     : int 
    Name       : string
    Value      : string
    RowVersion : DateTime 
    }

type DBMSContext() =
    inherit DbContext()

    [<DefaultValue>] val mutable m_analysisSoftware : DbSet<AnalysisSoftware>
    member public this.AnalysisSoftware with get() = this.m_analysisSoftware
                                                and set value = this.m_analysisSoftware <- value

    [<DefaultValue>] val mutable m_analysisSoftwareParam : DbSet<AnalysisSoftwareParam>
    member public this.AnalysisSoftwareParam with get() = this.m_analysisSoftwareParam
                                                and set value = this.m_analysisSoftwareParam <- value
    
    [<DefaultValue>] val mutable m_dbSequence : DbSet<DBSequence>
    member public this.DBSequence with get() = this.m_dbSequence
                                                and set value = this.m_dbSequence <- value

    [<DefaultValue>] val mutable m_dbSequenceParam : DbSet<DBSequenceParam>
    member public this.DBSequenceParam with get() = this.m_dbSequenceParam
                                                and set value = this.m_dbSequenceParam <- value

    [<DefaultValue>] val mutable m_modLocation : DbSet<ModLocation>
    member public this.ModLocation with get() = this.m_modLocation
                                                and set value = this.m_modLocation <- value

    [<DefaultValue>] val mutable m_modLocationParam : DbSet<ModLocationParam>
    member public this.ModLocationParam with get() = this.m_modLocationParam
                                                and set value = this.m_modLocationParam <- value

    [<DefaultValue>] val mutable m_modification : DbSet<Modification>
    member public this.Modification with get() = this.m_modification
                                                and set value = this.m_modification <- value

    [<DefaultValue>] val mutable m_modificationParam : DbSet<ModificationParam>
    member public this.ModificationParam with get() = this.m_modificationParam
                                                and set value = this.m_modificationParam <- value

    [<DefaultValue>] val mutable m_ontology : DbSet<Ontology>
    member public this.Ontology with get() = this.m_ontology
                                                and set value = this.m_ontology <- value

    [<DefaultValue>] val mutable m_organization : DbSet<Organization>
    member public this.Organization with get() = this.m_organization
                                                and set value = this.m_organization <- value

    [<DefaultValue>] 
    val mutable m_organizationParam : DbSet<OrganizationParam>
    member public this.OrganizationParam with get() = this.m_organizationParam
                                                and set value = this.m_organizationParam <- value

    [<DefaultValue>] val mutable m_parent : DbSet<Parent>
    member public this.Parent with get() = this.m_parent
                                                and set value = this.m_parent <- value

    [<DefaultValue>] val mutable m_peptide : DbSet<Peptide>
    member public this.Peptide with get() = this.m_peptide
                                                and set value = this.m_peptide <- value

    [<DefaultValue>] val mutable m_peptideParam : DbSet<PeptideParam>
    member public this.PeptideParam with get() = this.m_peptideParam
                                                and set value = this.m_peptideParam <- value

    [<DefaultValue>] val mutable m_peptideEvidence : DbSet<PeptideEvidence>
    member public this.PeptideEvidence with get() = this.m_peptideEvidence
                                                and set value = this.m_peptideEvidence <- value

    [<DefaultValue>] val mutable m_peptideEvidenceParam : DbSet<PeptideEvidenceParam>
    member public this.PeptideEvidenceParam with get() = this.m_peptideEvidenceParam
                                                and set value = this.m_peptideEvidenceParam <- value

    [<DefaultValue>] val mutable m_peptideHypothesis : DbSet<PeptideHypothesis>
    member public this.PeptideHypothesis with get() = this.m_peptideHypothesis
                                                and set value = this.m_peptideHypothesis <- value

    [<DefaultValue>] val mutable m_peptideHypothesisParam : DbSet<PeptideHypothesisParam>
    member public this.PeptideHypothesisParam with get() = this.m_peptideHypothesisParam
                                                and set value = this.m_peptideHypothesisParam <- value

    [<DefaultValue>] val mutable m_person : DbSet<Person>
    member public this.Person with get() = this.m_person
                                                and set value = this.m_person <- value

    [<DefaultValue>] val mutable m_personParam : DbSet<Person>
    member public this.PersonParam with get() = this.m_personParam
                                                and set value = this.m_personParam <- value

    [<DefaultValue>] val mutable m_proteinAmbiguityGroup : DbSet<ProteinAmbiguityGroup>
    member public this.ProteinAmbiguityGroup with get() = this.m_proteinAmbiguityGroup
                                                and set value = this.m_proteinAmbiguityGroup <- value

    [<DefaultValue>] val mutable m_proteinAmbiguityGroupParam : DbSet<ProteinAmbiguityGroupParam>
    member public this.ProteinAmbiguityGroupParam with get() = this.m_proteinAmbiguityGroupParam
                                                    and set value = this.m_proteinAmbiguityGroupParam <- value

    [<DefaultValue>] val mutable m_proteinDetectionHypothesis : DbSet<ProteinDetectionHypothesis>
    member public this.ProteinDetectionHypothesis with get() = this.m_proteinDetectionHypothesis
                                                    and set value = this.m_proteinDetectionHypothesis <- value

    [<DefaultValue>] val mutable m_proteinDetectionHypothesisParam : DbSet<ProteinDetectionHypothesisParam>
    member public this.ProteinDetectionHypothesisParam with get() = this.m_proteinDetectionHypothesisParam
                                                       and set value = this.m_proteinDetectionHypothesisParam <- value

    [<DefaultValue>] val mutable m_proteinDetectionList : DbSet<ProteinDetectionList>
    member public this.ProteinDetectionList with get() = this.m_proteinDetectionList
                                                    and set value = this.m_proteinDetectionList <- value

    [<DefaultValue>] val mutable m_proteinDetectionListParam : DbSet<ProteinDetectionListParam>
    member public this.ProteinDetectionListParam with get() = this.m_proteinDetectionListParam
                                                    and set value = this.m_proteinDetectionListParam <- value

    [<DefaultValue>] val mutable m_proteinDetectionProtocol : DbSet<ProteinDetectionProtocol>
    member public this.ProteinDetectionProtocol with get() = this.m_proteinDetectionProtocol
                                                    and set value = this.m_proteinDetectionProtocol <- value

    [<DefaultValue>] val mutable m_proteinDetectionProtocolParam : DbSet<ProteinDetectionProtocolParam>
    member public this.ProteinDetectionProtocolPAram with get() = this.m_proteinDetectionProtocolParam
                                                        and set value = this.m_proteinDetectionProtocolParam <- value

    [<DefaultValue>] val mutable m_spectrumIdentification : DbSet<SpectrumIdentification>
    member public this.SpectrumIdentification with get() = this.m_spectrumIdentification
                                                    and set value = this.m_spectrumIdentification <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationParam : DbSet<SpectrumIdentificationParam>
    member public this.SpectrumIdentificationParam with get() = this.m_spectrumIdentificationParam
                                                    and set value = this.m_spectrumIdentificationParam <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationItem : DbSet<SpectrumIdentificationItem>
    member public this.SpectrumIdentificationItem with get() = this.m_spectrumIdentificationItem
                                                    and set value = this.m_spectrumIdentificationItem <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationItemParam : DbSet<SpectrumIdentificationItemParam>
    member public this.SpectrumIdentificationItemParam with get() = this.m_spectrumIdentificationItemParam
                                                       and set value = this.m_spectrumIdentificationItemParam <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationList : DbSet<SpectrumIdentificationList>
    member public this.SpectrumIdentificationList with get() = this.m_spectrumIdentificationList
                                                    and set value = this.m_spectrumIdentificationList <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationListParam : DbSet<SpectrumIdentificationListParam>
    member public this.SpectrumIdentificationListParam with get() = this.m_spectrumIdentificationListParam
                                                        and set value = this.m_spectrumIdentificationListParam <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationProtocol : DbSet<SpectrumIdentificationProtocol>
    member public this.SpectrumIdentificationProtocol with get() = this.m_spectrumIdentificationProtocol
                                                        and set value = this.m_spectrumIdentificationProtocol <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationProtocolParam : DbSet<SpectrumIdentificationProtocolParam>
    member public this.SpectrumIdentificationProtocolParam with get() = this.m_spectrumIdentificationProtocolParam
                                                            and set value = this.m_spectrumIdentificationProtocolParam <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationResult : DbSet<SpectrumIdentificationResult>
    member public this.SpectrumIdentificationResult with get() = this.m_spectrumIdentificationResult
                                                    and set value = this.m_spectrumIdentificationResult <- value

    [<DefaultValue>] val mutable m_spectrumIdentificationResultParam : DbSet<SpectrumIdentificationResultParam>
    member public this.SpectrumIdentificationResultParam with get() = this.m_spectrumIdentificationResultParam
                                                            and set value = this.m_spectrumIdentificationResultParam <- value

    [<DefaultValue>] val mutable m_term : DbSet<Term>
    member public this.Term with get() = this.m_term
                                        and set value = this.m_term <- value

    [<DefaultValue>] val mutable m_termRelationShip : DbSet<TermRelationShip>
    member public this.TermRelationShip with get() = this.m_termRelationShip
                                        and set value = this.m_termRelationShip <- value

    [<DefaultValue>] val mutable m_termTag : DbSet<TermTag>
    member public this.TermTag with get() = this.m_termTag
                                        and set value = this.m_termTag <- value

    override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
        let fileDir = __SOURCE_DIRECTORY__ 
        let dbPath = fileDir + "\Ontologies_Terms\DavidsDatenbank.db"
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore

"C:\Users\Patrick\Source\Repos\TestSQLite\TestSQLite\Ontologies_Terms\DavidsDatenbank.db"

//creates OntologyItem with ID, OntologyID and Name
let createOntologyItem (id : int) (name : string) (rowversion : DateTime) =
    {
    Ontology.ID         = id
    Ontology.Name       = name
    Ontology.RowVersion = rowversion
    }

let createOrganizationItem id name parentid rowversion =
    {
    Organization.ID         = id
    Organization.Name       = name
    Organization.ParentID   = parentid
    Organization.RowVersion = rowversion
    }

let createSequenceOrganizationItems maxNumber =
    let rec loop i acc =
        if i = maxNumber then acc |> List.rev
        else
            let newItem = createOrganizationItem i (sprintf "BoB%i" i) i DateTime.Now
            loop (i+1) (newItem::acc)
    loop 1 []

let createParentItem id name country rowversion =
    {
    Parent.ID = id
    Parent.Name = name
    Parent.Country = country
    Parent.RowVersion = rowversion
    }

let createSequenceParentItems maxNumber =
    let rec loop i acc =
        if i = maxNumber then acc |> List.rev
        else
            let newItem = createParentItem i (sprintf "BoB%i" i) (sprintf "Country %i" i) DateTime.Now
            loop (i+1) (newItem::acc)
    loop 1 []

//let createseqOfOntoItems (inputSeq : seq<OboTerm>) =
//    inputSeq
//    |> Seq.map (fun x -> createOntologyItem 0 x.Name System.DateTime.Now)

let sqlTestingOntologyTermsTransactions (inputSeq : seq<OboTerm>) (inPutName : string) (inputNumber : int) =
    printfn "1"
    let db = new DBMSContext()   
    printfn "2"
    let timer = new Stopwatch()
    printfn "3"
    timer.Start() 
    db.Add({Ontology.ID=inputNumber; Ontology.Name=inPutName; Ontology.RowVersion=DateTime.Now}) |> ignore
    inputSeq
    |> Seq.iter (fun termItem -> 
                    db.Add({Term.ID         = termItem.Id; 
                            Term.Name       = termItem.Name; 
                            Term.OntologyID = inputNumber; 
                            Term.RowVersion = DateTime.Now}) |> ignore
                )
    printfn "4"
    db.SaveChanges() |>ignore 
    printfn "5"
    timer.Stop() 
    timer.Elapsed.TotalMilliseconds

let addEmptyOntologyandTerm (inPutName) =
    let db = new DBMSContext()
    db.Add({Ontology.ID=0; Ontology.Name=inPutName; Ontology.RowVersion=DateTime.Now}) |> ignore
    db.Add({Term.ID         = ""; 
            Term.Name       = ""; 
            Term.OntologyID = 5; 
            Term.RowVersion = DateTime.Now}) |> ignore
    db.SaveChanges() |>ignore

let sqlTestingParentTransactions (inputSeq : seq<Parent>) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start() 
    inputSeq
    |> Seq.iter (fun parentItem -> 
                    db.Add({Parent.ID         = 0; 
                            Parent.Name       = parentItem.Name; 
                            Parent.Country    = parentItem.Country; 
                            Parent.RowVersion = DateTime.Now}) |> ignore
                ) 
    db.SaveChanges() |>ignore 
    timer.Stop() 
    timer.Elapsed.TotalMilliseconds

let sqlTestingOrganTransactions (inputSeq : seq<Organization>) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start() 
    inputSeq
    |> Seq.iter (fun organItem -> 
                    db.Add({Organization.ID         = 0; 
                            Organization.Name       = organItem.Name; 
                            Organization.ParentID   = organItem.ParentID; 
                            Organization.RowVersion = DateTime.Now}) |> ignore
                )
    db.SaveChanges() |>ignore 
    timer.Stop() 
    timer.Elapsed.TotalMilliseconds

/// Reads FastaItem from file. Converter determines type of sequence by converting seq<char> -> type
///Testing
let fromFile (filePath) =
    FileIO.readFile filePath
    |> parseOboTerms
    |> Seq.toList
    //|> createseqOfOntoItems
    |> sqlTestingOntologyTermsTransactions

//Seq.item 100 (fromFile (fileDir + "\Ontologies_Terms\Psi-MS.txt"))

let createDB dbPath =
    BioFSharp.Mz.MzIdentMLModel.Db.initDB dbPath |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Psi-MS.txt")        "Psi-MS"        1 |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Pride.txt")         "Pride"         2 |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Unimod.txt")        "Unimod"        3 |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Unit_Ontology.txt") "Unit_Ontology" 4 |> ignore
    addEmptyOntologyandTerm ""

///Applying functions

createDB dbPath

let sequenzOfParents       = createSequenceParentItems       50

sqlTestingParentTransactions sequenzOfParents

let sequenzOfOrganizations = createSequenceOrganizationItems 10

sqlTestingOrganTransactions sequenzOfOrganizations 

/// Working with ParamContainer

open BioFSharp.Mz.MzIdentMLModel
//open BioFSharp.Mz.MzIdentMLModel.DataModel

let term1 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 1 "Ontology!!!"

let term2 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 2 "Ontology!!!"

let term3 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 3 "Ontology!!!"

let term4 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 4 "Ontology!!!"

let term5 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 1 "Ontology!!!"

let term6 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 2 "Ontology!!!"

let term7 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 3 "Ontology!!!"

let term8 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 4 "Ontology!!!"

let term9 = 
    BioFSharp.Mz.MzIdentMLModel.Term.create (Guid.NewGuid().ToString()) 4 "Ontology!!!"
let a = 
    CvParam.createWithUnit (Guid.NewGuid()) term1 1 term5

let b =
    CvParam.createWithUnit (Guid.NewGuid()) term2 2 term6

let c =
    CvParam.createWithUnit (Guid.NewGuid()) term3 3 term7

let d =
    CvParam.createWithUnit (Guid.NewGuid()) term4 4 term8

let e =
    CvParam.create (Guid.NewGuid()) term9 1

let ab = [a; b; c; d]

let dadam = ParamContainer.ofSeq ab

/// Write function for testing isSome for FKUnit!!!
/// Try to implement some/none

let sqLiteOrganParamConTransactionsWithUnit (inputSeqParams : Collections.Generic.Dictionary<DataModel.TermId,DataModel.CvParam>) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start()
    inputSeqParams
    |> Seq.iter (fun paramTermItem -> 
    db.Add({Term.ID         = paramTermItem.Value.Term.Id; 
            Term.Name       = paramTermItem.Value.Term.Name; 
            Term.OntologyID = paramTermItem.Value.Term.FK_Ontology; 
            Term.RowVersion = DateTime.Now}) |> ignore
          )
    inputSeqParams
    |> Seq.iter (fun paramTermItem ->
    match paramTermItem.Value.Unit.IsSome with
    |true -> db.Add({Term.ID         = paramTermItem.Value.Unit.Value.Id; 
                     Term.Name       = paramTermItem.Value.Unit.Value.Name; 
                     Term.OntologyID = paramTermItem.Value.Unit.Value.FK_Ontology; 
                     Term.RowVersion = DateTime.Now}) |> ignore
    |false -> printfn "Contains no other Term"
                   )   
    db.SaveChanges() |>ignore
    inputSeqParams
    |> Seq.iter (fun paramItem ->
    match paramItem.Value.Unit.IsSome with
    |true -> db.Add({OrganizationParam.ID               = 0; 
                     OrganizationParam.FKParamContainer = 1; 
                     OrganizationParam.FKTerm           = paramItem.Value.Term.Id; 
                     OrganizationParam.Value            = paramItem.Value.Value.ToString(); 
                     OrganizationParam.FKUnit           = paramItem.Value.Unit.Value.Id
                     OrganizationParam.RowVersion       = DateTime.Now}) |> ignore
                    
    |false -> db.Add({OrganizationParam.ID               = 0; 
                      OrganizationParam.FKParamContainer = 1; 
                      OrganizationParam.FKTerm           = paramItem.Value.Term.Id; 
                      OrganizationParam.Value            = paramItem.Value.Value.ToString(); 
                      OrganizationParam.FKUnit           = ""; /// Try to implement the option type because an axtra ontology isn`t a good solution
                      OrganizationParam.RowVersion       = DateTime.Now}) |> ignore
                    )
    db.SaveChanges() |> ignore
    timer.Stop()
    timer.Elapsed.TotalMilliseconds

sqLiteOrganParamConTransactionsWithUnit dadam

/// Try to implement the option type because an axtra ontology isn`t a good solution

let insertOrganParams id fkParamContainer fkTerm value rowVersion =
    let db = new DBMSContext()
    db.Add(new OrganizationParam(id, fkParamContainer, fkTerm, value, rowVersion)) |> ignore
    db.SaveChanges() |> ignore

insertOrganParams 0 1 "MS:0000000" "yes"  DateTime.Now