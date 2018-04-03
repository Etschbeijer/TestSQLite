
//#I @"..\packages\Microsoft.AspNet.Identity.EntityFramework.2.2.1\lib\net45"
//#I @"..\packages\Microsoft.AspNet.Identity.Core.2.2.1\lib\net45\"
//#r "Microsoft.AspNet.Identity.Core.dll"
//#r "Microsoft.AspNet.Identity.EntityFramework.dll"
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
open System.ComponentModel.DataAnnotations
open System.ComponentModel.DataAnnotations.Schema
open Microsoft.EntityFrameworkCore
open System.Linq
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
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }

//and [<CLIMutable>] DBSequence =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    Accession        : string
//    Name             : string
//    SearchDB         : SearchDatabase
//    RowVersion       : DateTime 
//    DBSequenceParams : List<DBSequenceParam>
//    }

//and [<CLIMutable>] DBSequenceParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : DBSequence
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }
////Added because it is part of the original document and refferd to in the code
//and [<CLIMutable>] MassTable =
//    {
//    ID : int
//    Name : string
//    MSLevel : List<int> //MS spectrum that the MassTable reffers to, e.g. "1" for MS1
//    RowVersion : DateTime
//    MassTableParams : List<MassTableParam> 
//    }

//and [<CLIMutable>] MassTableParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : MassTable
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }
////Not part of the original document
//and [<CLIMutable>] ModLocation =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                : int
//    Peptide           : Peptide
//    Modification      : Modification
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
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }
////Create connection to ModLocation?
//and [<CLIMutable>] Modification =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                    : int
//    Name                  : string 
//    Residues              : string 
//    MonoisotopicMassDelta : float 
//    AvgMassDelta          : float 
//    ModLocation           : ModLocation //Added to create connection to ModLocation
//    RowVersion            : DateTime 
//    ModificationParams    : List<ModificationParam>
//    }

//and [<CLIMutable>] ModificationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Modification
//    [<RequiredAttribute()>]
//    Term             : Term
//    Unit             : Term
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
    [<RequiredAttribute()>]
    Term                   : Term
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

//and [<CLIMutable>] Organization =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                 : int
//    Name               : string
//    ParentID           : Parent //Added to create connection to Parent
//    RowVersion         : DateTime
//    OrganizationParams : List<OrganizationParam>
//    }

//and [<CLIMutable>] OrganizationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Organization
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] Parent =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID           : int            //Not part of the original document
//    Name         : string         //Not part of the original document
//    Organization : Organization 
//    Country      : string         //Not part of the original document
//    RowVersion   : DateTime
//    ParentParams : List<ParentParam>
//    }

//and [<CLIMutable>] ParentParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//    ID               : int
//    FKParamContainer : Parent
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime   
//    }

//and [<CLIMutable>] Peptide =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID            : int
//    Name          : string             //Part of the original document
//    Sequence      : string             //Not part of the original document
//    RowVersion    : DateTime 
//    PeptideParams : List<PeptideParam>
//    }

//and [<CLIMutable>] PeptideParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Peptide
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }


//and [<CLIMutable>] PeptideEvidence =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                    : int
//    DBSequence            : DBSequence
//    Peptide               : Peptide
//    isDecoy               : string 
//    Frame                 : string 
//    Start                 : int 
//    End                   : int 
//    Pre                   : string 
//    Post                  : string 
//    Translation           : Translation              //Refers to TranslationTable
//    RowVersion            : DateTime 
//    PeptideEvidenceParams : List<PeptideEvidenceParam>
//    }

//and [<CLIMutable>] PeptideEvidenceParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : PeptideEvidence
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] PeptideHypothesis =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                           : int
//    PeptideEvidence              : PeptideEvidence
//    PeptideDetectionHypothesisID : int                //Not part of the original document
//    RowVersion                   : DateTime  
//    PeptideHypothesisParams      : List<PeptideHypothesisParam>
//    }

//and [<CLIMutable>] PeptideHypothesisParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : PeptideHypothesis
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
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
//    Organisation   : Organization         //Not part of the original document
//    RowVersion     : DateTime  
//    PersonParams   : List<PersonParam>
//    }

//and [<CLIMutable>] PersonParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Person
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime   
//    }

//and [<CLIMutable>] ProteinAmbiguityGroup =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                          : int
//    ProteinDetectionList        : ProteinDetectionList //Not part of the original document
//    Name                        : string 
//    RowVersion                  : DateTime
//    ProteinAmbiguityGroupParams : List<ProteinAmbiguityGroupParam>
//    }

//and [<CLIMutable>] ProteinAmbiguityGroupParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinAmbiguityGroup
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime     
//    }
////Added because it is part of the original document
//and [<CLIMutable>] ProteinDetection =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//    ID                        : int
//    Name                      : string
//    ProteinDetectionList      : ProteinDetectionList
//    ProteinDetectionProtocoll : ProteinDetectionProtocoll
//    RowVersion                : DateTime
//    ProteinDetectionParams    : List<ProteinDetectionParam>
//    }

//and [<CLIMutable>] ProteinDetectionParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinDetection
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime
//    }

//and [<CLIMutable>] ProteinDetectionHypothesis =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                               : int
//    DBSequence                       : DBSequence
//    ProteinAmbiguityGroup            : ProteinAmbiguityGroup //Not part of the original document
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
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime
//    }

//and [<CLIMutable>] ProteinDetectionList =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                         : int
//    Accession                  : string
//    Name                       : string
//    SearchDB                   : SearchDatabase       //Not part of the original document
//    RowVersion                 : DateTime
//    ProteinDetectionListParams : List<ProteinDetectionListParam>
//    }

//and [<CLIMutable>] ProteinDetectionListParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinDetectionList
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime
//    }

//and [<CLIMutable>] ProteinDetectionProtocol =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                             : int
//    Name                           : string 
//    AnalysisSoftware               : AnalysisSofware
//    RowVersion                     : DateTime
//    ProteinDetectionProtocolParams : List<ProteinDetectionProtocolParam>
//    }

//and [<CLIMutable>] ProteinDetectionProtocolParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : ProteinDetectionProtocol
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }
////Added because it is part of the original document and refferd to in the code
//and [<CLIMutable>] Sample =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID           : int
//    Name         : string 
//    RowVersion   : DateTime
//    SampleParams : List<SampleParam>
//    }

//and [<CLIMutable>] SampleParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Sample
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }

////Added because it exists in the original document and is referred multiple times in the code
//and [<CLIMutable>] SearchDatabase =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                             : int
//    Name                           : string 
//    Location                       : string //Location of the datafile, commonly an URL
//    RowVersion                     : DateTime
//    SearchDatabaseParams : List<SearchDatabaseParam>
//    }

//and [<CLIMutable>] SearchDatabaseParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SearchDatabaseParam
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime  
//    }

////Added because it exists in the original document and is referred in the code
//and [<CLIMutable>] SpectraData =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                                : int
//    Name                              : string 
//    Location                          : string //Location of the datafile, commonly an URL
//    RowVersion                        : DateTime
//    SpectrumIdentificationParams      : List<SpectraDataParam>
//    }

//and [<CLIMutable>] SpectraDataParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                     : int
//    FKParamContainer       : SpectraData
//    [<RequiredAttribute()>]
//    Term                 : Term
//    Unit                 : Term
//    Value                  : string
//    RowVersion             : DateTime
//    }

and [<CLIMutable>] SpectrumIdentification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                : int
    Name                              : string 
    ActivityDate                      : string  //Not part of the original document
    SpectrumIdentificationList        : int     //Refers to SpectrumidentificationList normally
    SpectrumIdentificationProtocoll   : int     //Refers to SpectrumIdentificationProtocol normally
    RowVersion                        : DateTime
    SpectrumIdentificationParams      : List<SpectrumIdentificationParam>
    }

and [<CLIMutable>] SpectrumIdentificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : SpectrumIdentification
    [<RequiredAttribute()>]
    Term                   : Term
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

//and [<CLIMutable>] SpectrumIdentificationItem =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                               : int
//    SpectrumIdentificationResult     : SpectrumIdentificationResult
//    Sample                           : Sample          
//    Peptide                          : Peptide
//    MassTable                        : MassTable
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
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationList =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                               : int
//    Name                             : string 
//    NumSequencesSearched             : int 
//    RowVersion                       : DateTime
//    SpectrumIdentificationListParams : List<SpectrumIdentificationListParam>
//    }

//and [<CLIMutable>] SpectrumIdentificationListParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SpectrumIdentificationList
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationProtocol =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                                   : int
//    Name                                 : string 
//    AnalysisSoftware                     : AnalysisSoftware
//    RowVersion                           : DateTime 
//    SpectrumIdentificationProtocolParams : List<SpectrumIdentificationProtocolParam>
//    }

//and [<CLIMutable>] SpectrumIdentificationProtocolParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SpectrumIdentificationProtocol
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationResult =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                           : int
//    SpectrumID                   : int    //Refers to SpectrumID, unique in the SpectraData for the specific Spectrum
//    SpectraData                  : SpectraData       
//    SpectrumIdentificationList   : SpectrumIdentificationList
//    Name                         : string 
//    RowVersion                   : DateTime 
//    }

//and [<CLIMutable>] SpectrumIdentificationResultParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : SpectrumIdentificationResult
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime    
//    }

and [<CLIMutable>] Term =
    {
    ID             : string
    Name           : string
    Ontology       : Ontology
    RowVersion     : DateTime 
    }

//and [<CLIMutable>] TermRelationShip =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    Term             : Term
//    RelationShipType : string
//    FKRelatedTerm    : Term
//    RowVersion       : DateTime     
//    }

//and [<CLIMutable>] TermTag =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID         : int
//    Term      : Term
//    Name       : string
//    Value      : string
//    RowVersion : DateTime 
//    }
////Added because it is part of the original document and refferd to in the code
//and [<CLIMutable>] Translation =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                : int
//    Name              : string 
//    TranslationParams : List<TranslationParam>
//    }

//and [<CLIMutable>] TranslationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Translation
//    [<RequiredAttribute()>]
//    Term           : Term
//    Unit           : Term
//    Value            : string
//    RowVersion       : DateTime    
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

    //override this.OnModelCreating (modelbuilder : ModelBuilder) =
    //    modelbuilder.Entity<OntologyParam>()
    //        .HasOne(fun field -> field.Term)
    //        .WithMany("OntologyParams")
    //        .IsRequired(true) |> ignore

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

///Define functions to create types for DB
let createOntology inputName inputParams inputTerms =
    {
     Ontology.ID             = 0
     Ontology.Name           = inputName
     Ontology.RowVersion     = DateTime.Now.Date
     Ontology.OntologyParams = inputParams
     Ontology.Terms          = inputTerms
    }

let createTerm inputOntology (inputOboTerm : seq<Obo.OboTerm>) =
    new System
        .Collections
        .Generic
        .List<Term>(
        inputOboTerm
            |> Seq.map (fun item ->
                                    {
                                     Term.ID         = item.Id
                                     Term.Name       = item.Name
                                     Term.Ontology   = inputOntology
                                     Term.RowVersion = DateTime.Now.Date
                                    }
                       )   
                   )

let createSpectrumIdentification inputName inputDate inputList inputProtocoll inputParams =
    {
     SpectrumIdentification.ID                              = 0
     SpectrumIdentification.Name                            = inputName
     SpectrumIdentification.ActivityDate                    = inputDate
     SpectrumIdentification.SpectrumIdentificationList      = inputList
     SpectrumIdentification.SpectrumIdentificationProtocoll = inputProtocoll
     SpectrumIdentification.RowVersion                      = DateTime.Now.Date
     SpectrumIdentification.SpectrumIdentificationParams    = inputParams
    }

let createSpectrumIdentificationParam inputSpectrumIdentification inputValue inputTerm inputUnit =
    {
     SpectrumIdentificationParam.ID               = 0
     SpectrumIdentificationParam.FKParamContainer = inputSpectrumIdentification
     SpectrumIdentificationParam.Value            = inputValue
     SpectrumIdentificationParam.Term             = inputTerm
     SpectrumIdentificationParam.Unit             = inputUnit
     SpectrumIdentificationParam.RowVersion       = DateTime.Now.Date
    }
    
///Create information for Table
let ontologyCustom =
     {
                Ontology.ID             = 1
                Ontology.Name           = "Custom"
                Ontology.RowVersion     = DateTime.Now.Date
                Ontology.OntologyParams = null
                Ontology.Terms          = new System.Collections.Generic.List<Term>
                    ([{
                       Term.ID             = "Custom:000001"; 
                       Term.Name           = "Unitless"; 
                       Term.Ontology       = {
                                                Ontology.ID             = 1 
                                                Ontology.Name           = "Custom" 
                                                Ontology.RowVersion     = DateTime.Now.Date
                                                Ontology.OntologyParams = null 
                                                Ontology.Terms          = null
                                             } 
                       Term.RowVersion     = DateTime.Now.Date 
                    }])
     }

let ontologyPsiMS =
    createOntology "Psi-MS" null (createTerm(createOntology "Psi-MS" null null) fromPsiMS)

let ontologyPride =
    createOntology "Pride" null (createTerm(createOntology "Pride" null null) fromPride)

let ontologyUniMod =
    createOntology "UniMod" null (createTerm(createOntology "UniMod" null null) fromUniMod)

let ontologyUnit_Ontology =
    createOntology "Unit_Ontology" null (createTerm(createOntology "Unit_Ontology" null null) fromUnit_Ontology)

let spectrumIdentification =
    createSpectrumIdentification "Test" "Today" 1 2 null

let spectrumIdentificationParam =
    createSpectrumIdentificationParam spectrumIdentification "Point" ontologyPride.Terms.[34] ontologyCustom.Terms.[0]

///Creating && Checking && inserting into DB
let initDB =
    let db = new DBMSContext()
    
    db.Database.EnsureCreated()                                    |> ignore
    db.Ontology.                   Add(ontologyCustom)             |> ignore
    db.Ontology.                   Add(ontologyPsiMS)              |> ignore
    db.Ontology.                   Add(ontologyPride)              |> ignore
    db.Ontology.                   Add(ontologyUniMod)             |> ignore
    db.Ontology.                   Add(ontologyUnit_Ontology)      |> ignore
    db.SpectrumIdentification.     Add(spectrumIdentification)     |> ignore
    db.SpectrumIdentificationParam.Add(spectrumIdentificationParam) |> ignore
    db.SaveChanges()

///Read and manipulate the DB
    
let context = new DBMSContext()

let testOntology =
    query {
        for i in context.Ontology do
        select i
          }

//let testTerm =
//    query {
//        for i in context.Term do
//        select i
//          }

let readLine (input : seq<'a>) =
    for i in input do
        Console.WriteLine(i)

readLine testOntology
//readLine testTerm

//let testTerm2 =
//    (query {
//        for i in context.Term do
//        if i.ID = "" then select i}).Single()
//testTerm2.Name <- "Some BoB"

let testTerm3 =
    (query {
        for i in context.Term do
        if i.ID = "" then select i.Ontology})
readLine testTerm3

let testTerm4 =
    (query {
        for i in context.Term do
            if i.Name = "" && i.ID = "" then select i})
readLine testTerm4

let selectSpectrumIdentificationByID inputID =
    (query {
        for i in context.SpectrumIdentification do
            if i.ID = inputID then select i})

let selectSpectrumIdentificationByName inputName =
    (query {
        for i in context.SpectrumIdentification do
            if i.Name = inputName then select i})

let selectSpectrumIdentificationByActivityDate inputActivityDate =
    (query {
        for i in context.SpectrumIdentification do
            if i.ActivityDate = inputActivityDate then select i})

let selectSpectrumIdentificationBySIList inputSIList =
    (query {
        for i in context.SpectrumIdentification do
            if i.SpectrumIdentificationList = inputSIList then select i})

let selectSpectrumIdentificationBySIProtocoll inputSIProtocoll =
    (query {
        for i in context.SpectrumIdentification do
            if i.SpectrumIdentificationProtocoll = inputSIProtocoll then select i})

let selectSpectrumIdentificationParamBySI inputSI =
    (query {
        for i in context.SpectrumIdentificationParam do
            if i.FKParamContainer = inputSI then select (i, i.Term, i.Unit)})

context.SaveChanges()



selectSpectrumIdentificationByID 1
|> readLine
selectSpectrumIdentificationByName "Test"
|> readLine
selectSpectrumIdentificationByActivityDate "Today"
|> readLine
selectSpectrumIdentificationBySIList 1
|> readLine
selectSpectrumIdentificationBySIProtocoll 2
|> readLine
selectSpectrumIdentificationParamBySI spectrumIdentification
|> readLine

///Testen verschiedener Varianten, Ergebnis anhand eines Linearitätstests überprüfen: Stabile Linie ja, nein?