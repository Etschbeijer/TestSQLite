///Another Test
#I @"..\packages\Microsoft.AspNet.Identity.EntityFramework.2.2.1\lib\net45"
#I @"..\packages\Microsoft.AspNet.Identity.Core.2.2.1\lib\net45\"
#r "Microsoft.AspNet.Identity.Core.dll"
#r "Microsoft.AspNet.Identity.EntityFramework.dll"
#r "System.ComponentModel.DataAnnotations.dll"
#r @"..\TestSQLite\bin\Debug\netstandard.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r @"..\TestSQLite\bin\Debug\System.Data.SQLite.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.IO.dll"
#r @"..\TestSQLite\bin\Debug\BioFSharp.Mz.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.dll"
#r @"..\TestSQLite\bin\Debug\FSharp.Care.IO.dll"

open System
open System.Diagnostics
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
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
open BioFSharp.ModificationInfo
open FSharp.Care.Collections

let fileDir = __SOURCE_DIRECTORY__ 
let dbPath = fileDir + "\Ontologies_Terms\DavidsDatenbank.db"

///types for the DataBank
[<CLIMutable>]
type AnalysisSoftware =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type AnalysisSoftwareParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime  
    }

[<CLIMutable>]
type DBSequence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Accession : string
    Name : string
    SearchDBID : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type DBSequenceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ModLocation =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    PeptideID : int
    ModificationID : int
    Location : int
    Residue : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ModLocationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type Modification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string 
    Residues : string 
    MonoisotopicMassDelta : float 
    AvgMassDelta : float 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ModificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    }

[<CLIMutable>]
type Ontology = 
    {
    ID : int
    Name : string
    RowVersion : DateTime
    }

[<CLIMutable>]
type Organization =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string
    ParentID : int
    RowVersion : DateTime 
    }

[<CLIMutable>]
type OrganizationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type Parent =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string
    Country : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ParentParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime   
    }

[<CLIMutable>]
type Peptide =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Sequence : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime  
    }

[<CLIMutable>]
type PeptideEvidence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    DBSequenceID : int
    PeptideID : int
    isDecoy : string 
    Frame : string 
    Start : int 
    End : int 
    Pre : string 
    Post : string 
    TranslationsID : int 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideEvidenceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    PeptideEvidenceID : int
    PeptideDetectionHypothesisID : int 
    RowVersion : DateTime    
    }

[<CLIMutable>]
type PeptideHypothesisParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type Person =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FirstName : string 
    LastName : string 
    MiddleName : string 
    OrganisationID : int
    RowVersion : DateTime    
    }

[<CLIMutable>]
type PersonParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime   
    }

[<CLIMutable>]
type ProteinAmbiguityGroup =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    ProteinDetectionListID : int
    Name : string 
    RowVersion : DateTime
    }

[<CLIMutable>]
type ProteinAmbiguityGroupParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type ProteinDetectionHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    DBSequenceID : int
    ProteinAmbiguityGroupID : int
    Name : string 
    PassThreshold : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type ProteinDetectionHypothesisParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime
    }

[<CLIMutable>]
type ProteinDetectionList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Accession : string
    Name : string
    SearchDBID : string
    RowVersion : DateTime    
    }

[<CLIMutable>]
type ProteinDetectionListParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime
    }

[<CLIMutable>]
type ProteinDetectionProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string 
    AnalysisSoftwareID : int
    RowVersion : DateTime  
    }

[<CLIMutable>]
type ProteinDetectionProtocolParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime  
    }
    
[<CLIMutable>]
type SpectrumIdentification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string 
    ActivityDate : string 
    SpectrumIdentificationListID : int
    SpectrumIdentificationProtocollID : int
    RowVersion : DateTime   
    }

[<CLIMutable>]
type SpectrumIdentificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime    
    }

[<CLIMutable>]
type SpectrumIdentificationItem =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
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
    Fragmentation : DateTime  
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationItemParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string 
    NumSequencesSeqrched : int 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationListParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    Name : string 
    AnalysisSoftwareID : int
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationProtocolParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationResult =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    SpectrumID : int
    //SpectraDataID : string // quetionable???
    SpectrumIdentificationListID : int 
    Name : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationResultParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type Term =
    {
    ID : string
    Name : string
    OntologyID : int
    RowVersion : DateTime 
    }

[<CLIMutable>]
type TermRelationShip =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    TermID : int 
    RelationShipType : string
    FKRelatedTerm : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type TermTag =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    TermID : int 
    Name : string
    Value : string
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

    [<DefaultValue>] val mutable m_organizationParam : DbSet<OrganizationParam>
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
    Ontology.ID = id
    Ontology.Name = name
    Ontology.RowVersion = rowversion
    }

let createOrganizationItem id name parentid rowversion =
    {
    Organization.ID = id
    Organization.Name = name
    Organization.ParentID = parentid
    Organization.RowVersion = rowversion
    }

let createSequenceOrganizationItems maxNumber =
    let rec loop i acc =
        if i = maxNumber then acc
        else
            let newItem = createOrganizationItem i (sprintf "BoB%i" i) i DateTime.Now
            loop (i+1) (newItem::acc)
    loop 0 []

let createParentItem id name country rowversion =
    {
    Parent.ID = id
    Parent.Name = name
    Parent.Country = country
    Parent.RowVersion = rowversion
    }

let createSequenceParentItems maxNumber =
    let rec loop i acc =
        if i = maxNumber then acc 
        else
            let newItem = createParentItem i (sprintf "BoB%i" i) (sprintf "Country %i" i) DateTime.Now
            loop (i+1) (newItem::acc)
    loop 0 []

//let createseqOfOntoItems (inputSeq : seq<OboTerm>) =
//    inputSeq
//    |> Seq.map (fun x -> createOntologyItem 0 x.Name System.DateTime.Now)

let sqlTestingOntologyTermsTransactions (inputSeq : seq<OboTerm>) (inPutName : string) (inputNumber : int) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start() 
    db.Add({Ontology.ID=inputNumber; Ontology.Name=inPutName; Ontology.RowVersion=DateTime.Now}) |> ignore
    inputSeq
    |> Seq.iter (fun termItem -> 
                    db.Add({Term.ID=termItem.Id; Term.Name=termItem.Name; Term.OntologyID=inputNumber; Term.RowVersion=DateTime.Now}) |> ignore
                ) 
    db.SaveChanges() |>ignore 
    timer.Stop() 
    timer.Elapsed.TotalMilliseconds

let sqlTestingParentTransactions (inputSeq : seq<Parent>) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start() 
    inputSeq
    |> Seq.iter (fun parentItem -> 
                    db.Add({Parent.ID=parentItem.ID; Parent.Name=parentItem.Name; Parent.Country=parentItem.Country; Parent.RowVersion=DateTime.Now}) |> ignore
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
                    db.Add({Organization.ID=organItem.ID; Organization.Name=organItem.Name; Organization.ParentID=organItem.ParentID; Organization.RowVersion=DateTime.Now}) |> ignore
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
    fromFile (fileDir + "\Ontologies_Terms\Psi-MS.txt") "Psi-MS"                1   |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Pride.txt") "Pride"                  2   |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Unimod.txt") "Unimod"                3   |> ignore
    fromFile (fileDir + "\Ontologies_Terms\Unit_Ontology.txt") "Unit_Ontology"  4

///Applying functions

createDB dbPath

let sequenzOfParents       = createSequenceParentItems       10

sqlTestingParentTransactions sequenzOfParents

let sequenzOfOrganizations = createSequenceOrganizationItems 10

sqlTestingOrganTransactions sequenzOfOrganizations 

/// Working with ParamContainer

open BioFSharp.Mz.MzIdentMLModel
open BioFSharp.Mz.MzIdentMLModel.DataModel

let term1 = 
    Term.create "0f16e34f-1eba-4194-b6cd-1e09206a570b" "Psi-MS" "Ontology!!!"

let term2 = 
    Term.create "0f16e34f-1eba-4194-b6cd-1e09206a570b" "Pride" "Ontology!!!"

let term3 = 
    Term.create "0f16e34f-1eba-4194-b6cd-1e09206a570b" "Unimod" "Ontology!!!"

let term4 = 
    Term.create "0f16e34f-1eba-4194-b6cd-1e09206a570b" "Unit_Ontology" "Ontology!!!"

let a = 
    CvParam.create (Guid.NewGuid()) term1 1

let b =
    CvParam.create (Guid.NewGuid()) term2 2 

let c =
    CvParam.create (Guid.NewGuid()) term3 3

let d =
    CvParam.create (Guid.NewGuid()) term4 4

let ab = [a;b;c;d]

let dadam = ParamContainer.ofSeq ab


let sqlTestingOrganzationParamsTransactions (inputSeq : Collections.Generic.Dictionary<TermId,CvParam>) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start()
    inputSeq
    |> Seq.iter (fun ParamItem -> 
    db.Add({OrganizationParam.ID=0; OrganizationParam.FKTerm=ParamItem.Value.Term.Id; OrganizationParam.Value=ParamItem.Value.Value.ToString(); OrganizationParam.FKUnit=ParamItem.Value.Unit.IsSome.ToString(); OrganizationParam.FKParamContainer=3; OrganizationParam.RowVersion=DateTime.Now}) |> ignore
    )  
    db.SaveChanges() |>ignore 
    timer.Stop()
    timer.Elapsed.TotalMilliseconds

sqlTestingOrganzationParamsTransactions dadam 
/// Change TermID of MzIdentMLModels to String