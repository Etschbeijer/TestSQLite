
///Another Test
#I @"..\packages\Microsoft.AspNet.Identity.EntityFramework.2.2.1\lib\net45"
#I @"..\packages\Microsoft.AspNet.Identity.Core.2.2.1\lib\net45\"
#r "Microsoft.AspNet.Identity.Core.dll"
#r "Microsoft.AspNet.Identity.EntityFramework.dll"
#r "System.ComponentModel.DataAnnotations.dll"
#r @"..\TestSQLite\bin\Debug\netstandard.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
#r @"..\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\System.Data.SQLite.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.IO.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.Mz.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.dll"
#r @"C:\Users\Patrick\source\repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.IO.dll"


//#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\System.Data.SQLite.dll"
//#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.dll"
//#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\BioFSharp.Mz.dll"
//#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.dll"
//#r @"C:\Users\PatrickB\Source\Repos\BioFSharp.Mz\src\BioFSharp.Mz\bin\Debug\FSharp.Care.IO.dll"
//#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\netstandard.dll"
//#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.dll"
//#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\Microsoft.EntityFrameworkCore.Sqlite.dll"
//#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\FSharp.Data.TypeProviders.dll"
//#r @"C:\Users\PatrickB\Source\Repos\TestSQLite\TestSQLite\bin\Debug\System.Linq.dll"
//#r @"C:\Users\PatrickB\Source\Repos\DatenBankTest\TestTabelleDavidFirma\bin\Debug\FSharp.Plotly.dll"
//#r @"C:\Users\PatrickB\Source\Repos\DatenBankTest\TestTabelleDavidFirma\bin\Debug\EntityFramework.dll"

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
open BioFSharp.Mz.MzIdentMLModel
//open System
//open System.Data
//open System.IO
//open System.Data.SQLite
//open BioFSharp.Formula.Table
//open BioFSharp.BioID.FastA
//open FSharp.Care.IO.SchemaReader
//open BioFSharp.ModificationInfo
open BioFSharp.IO.Obo


///types for the DataBank
[<CLIMutable>]
type AnalysisSoftware =
    {
    ID : int
    Name : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type AnalysisSoftwareParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime  
    }

[<CLIMutable>]
type DBSequence =
    {
    ID : int
    Accession : string
    Name : string
    SearchDBID : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type DBSequenceParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ModLocation =
    {
    ID : int
    PeptideID : int
    ModificationID : int
    Location : int
    Residue : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ModLocationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type Modification =
    {
    ID : int
    Name : string 
    Residues : string 
    MonoisotopicMassDelta : float 
    AvgMassDelta : float 
    RowVersion : DateTime 
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
    RowVersion : DateTime
    }

[<CLIMutable>]
type Organization =
    {
    ID : int
    Name : string
    Parent_ID : int
    RowVersion : DateTime 
    }

[<CLIMutable>]
type OrganizationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type Parent =
    {
    ID : int
    Name : string
    Country : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type ParentParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime   
    }

[<CLIMutable>]
type Peptide =
    {
    ID : int
    Sequence : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime  
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
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideEvidenceParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type PeptideHypothesis =
    {
    ID : int
    PeptideEvidenceID : int
    PeptideDetectionHypothesisID : int 
    RowVersion : DateTime    
    }

[<CLIMutable>]
type PeptideHypothesisParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type Person =
    {
    ID : int
    FirstName : string 
    LastName : string 
    MiddleName : string 
    OrganisationID : int
    RowVersion : DateTime    
    }

[<CLIMutable>]
type PersonParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime   
    }

[<CLIMutable>]
type ProteinAmbiguityGroup =
    {
    ID : int
    ProteinDetectionListID : int
    Name : string 
    RowVersion : DateTime
    }

[<CLIMutable>]
type ProteinAmbiguityGroupParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type ProteinDetectionHypothesis =
    {
    ID : int
    DBSequenceID : int
    ProteinAmbiguityGroupID : int
    Name : string 
    PassThreshold : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type ProteinDetectionHypothesisParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime
    }

[<CLIMutable>]
type ProteinDetectionList =
    {
    ID : int
    Accession : string
    Name : string
    SearchDBID : string
    RowVersion : DateTime    
    }

[<CLIMutable>]
type ProteinDetectionListParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime
    }

[<CLIMutable>]
type ProteinDetectionProtocol =
    {
    ID : int
    Name : string 
    AnalysisSoftwareID : int
    RowVersion : DateTime  
    }

[<CLIMutable>]
type ProteinDetectionProtocolParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime  
    }
    
[<CLIMutable>]
type SpectrumIdentification =
    {
    ID : int
    Name : string 
    ActivityDate : string 
    SpectrumIdentificationListID : int
    SpectrumIdentificationProtocollID : int
    RowVersion : DateTime   
    }

[<CLIMutable>]
type SpectrumIdentificationParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime    
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
    Fragmentation : DateTime  
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationItemParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationList =
    {
    ID : int
    Name : string 
    NumSequencesSeqrched : int 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationListParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationProtocol =
    {
    ID : int
    Name : string 
    AnalysisSoftwareID : int
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationProtocolParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationResult =
    {
    ID : int
    SpectrumID : int
    //SpectraDataID : string // quetionable???
    SpectrumIdentificationListID : int 
    Name : string 
    RowVersion : DateTime 
    }

[<CLIMutable>]
type SpectrumIdentificationResultParam =
    {
    ID : int
    FKParamContainer : int
    FKTerm : string
    FKUnit : string
    Value : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type Term =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] ID : int
    OntologyID : string
    Name : string
    RowVersion : DateTime 
    }

[<CLIMutable>]
type TermRelationShip =
    {
    ID : int
    TermID : int 
    RelationShipType : string
    FKRelatedTerm : string
    RowVersion : DateTime     
    }

[<CLIMutable>]
type TermTag =
    {
    TermTagID : int
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
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source=/F#-Projects\DavidsDatenbank.db") |> ignore

 

//creates OntologyItem with ID, OntologyID and Name
let createOntologyItem (id : string) (name : string) (rowversion : DateTime) =
    {
    Ontology.ID = id
    Ontology.Name = name
    Ontology.RowVersion = rowversion
    }

let createseqOfOntoItems (inputSeq : seq<OboTerm>) =
    inputSeq
    |> Seq.map (fun x -> createOntologyItem x.Id x.Name System.DateTime.Now)

let sqlTestingOntologySequenceTransactions (x : seq<Ontology>) =
    let db = new DBMSContext()   
    let timer = new Stopwatch()
    timer.Start()
    x
    |> Seq.iter (fun ontoItem -> 
                 db.Add({Ontology.ID=ontoItem.ID; Ontology.Name=ontoItem.Name; Ontology.RowVersion=DateTime.Now}) |> ignore
                )
    x
    |> Seq.iter (fun termItem -> 
                    db.Add({Term.ID=0; Term.Name=termItem.Name; Term.OntologyID=termItem.ID; Term.RowVersion=DateTime.Now}) |> ignore
                )
    //db.Add({Term.ID=1; Term.Name="Bob"; Term.OntologyID="id: MS:1000001"; Term.RowVersion=DateTime.Now}) |> ignore   
    db.SaveChanges() |>ignore 
    timer.Stop()
    timer.Elapsed.TotalMilliseconds


/// Reads FastaItem from file. Converter determines type of sequence by converting seq<char> -> type
///Testing
let fromFile (filePath) =
    FileIO.readFile filePath
    |> parseOboTerms
    |> Seq.toList
    |> createseqOfOntoItems
    |> sqlTestingOntologySequenceTransactions


let createDB dbPath oboPath1 oboPath2 oboPath3 oboPath4 =
    BioFSharp.Mz.MzIdentMLModel.Db.initDB dbPath |> ignore
    fromFile oboPath1 |> ignore
    fromFile oboPath2 |> ignore
    fromFile oboPath3 |> ignore
    fromFile oboPath4

///Applying functions

createDB "/F#-Projects\DavidsDatenbank.db" "/F#-Projects\ParserStuff\Psi-MS.txt" "/F#-Projects/ParserStuff/Pride.txt" "/F#-Projects\ParserStuff\Unimod.txt" "/F#-Projects\ParserStuff\Unit_Ontology.txt"
