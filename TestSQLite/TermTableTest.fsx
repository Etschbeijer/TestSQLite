
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
//open System.Linq
//open System.Linq.Expressions
//open BioFSharp.BioItem
open System.Collections.Generic
open FSharp.Care.IO
open BioFSharp.IO



///Defining Types

type [<CLIMutable>] AnalysisSoftware =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    Name                   : string 
    RowVersion             : DateTime
    AnalysisSoftwareParams : List<AnalysisSoftwareParam>
    }

and [<CLIMutable>] AnalysisSoftwareParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : AnalysisSoftware
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

and [<CLIMutable>] DBSequence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    Accession        : string
    Name             : string
    SearchDB         : SearchDatabase
    RowVersion       : DateTime 
    DBSequenceParams : List<DBSequenceParam>
    }

and [<CLIMutable>] DBSequenceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : DBSequence
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }
//Added because it is part of the original document and refferd to in the code
and [<CLIMutable>] MassTable =
    {
    ID : int
    Name : string
    MSLevel : List<int> //MS spectrum that the MassTable reffers to, e.g. "1" for MS1
    RowVersion : DateTime
    MassTableParams : List<MassTableParam> 
    }

and [<CLIMutable>] MassTableParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : MassTable
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

//Create connection to ModLocation?
and [<CLIMutable>] Modification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                    : int
    Name                  : string 
    Residues              : string 
    MonoisotopicMassDelta : float 
    AvgMassDelta          : float 
    ModLocation           : ModLocation //Added to create connection to ModLocation
    RowVersion            : DateTime 
    ModificationParams    : List<ModificationParam>
    }

and [<CLIMutable>] ModificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Modification
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }
//Not part of the original document
and [<CLIMutable>] ModLocation =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                : int
    Peptide           : Peptide
    Modification      : Modification
    Location          : int
    Residue           : string
    RowVersion        : DateTime
    ModLocationParams : List<ModLocation>
    }

and [<CLIMutable>] ModLocationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : ModLocation
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }
and [<CLIMutable>] Ontology = 
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

and [<CLIMutable>] Organization =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                 : int
    Name               : string
    Parent             : Parent //Added to create connection to Parent
    RowVersion         : DateTime
    OrganizationParams : List<OrganizationParam>
    }

and [<CLIMutable>] OrganizationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Organization
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] Parent =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int            //Not part of the original document
    Name         : string         //Not part of the original document
    Organization : Organization 
    Country      : string         //Not part of the original document
    RowVersion   : DateTime
    ParentParams : List<ParentParam>
    }

and [<CLIMutable>] ParentParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID               : int
    FKParamContainer : Parent
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime   
    }

and [<CLIMutable>] Peptide =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID            : int
    Name          : string             //Part of the original document
    Sequence      : string             //Not part of the original document
    RowVersion    : DateTime 
    PeptideParams : List<PeptideParam>
    }

and [<CLIMutable>] PeptideParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Peptide
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

and [<CLIMutable>] PeptideEvidence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                    : int
    DBSequence            : DBSequence
    Peptide               : Peptide
    isDecoy               : string 
    Frame                 : string 
    Start                 : int 
    End                   : int 
    Pre                   : string 
    Post                  : string 
    Translation           : Translation              //Refers to TranslationTable
    RowVersion            : DateTime 
    PeptideEvidenceParams : List<PeptideEvidenceParam>
    }

and [<CLIMutable>] PeptideEvidenceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : PeptideEvidence
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] PeptideHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                           : int
    PeptideEvidence              : PeptideEvidence
    PeptideDetectionHypothesisID : int                //Not part of the original document
    RowVersion                   : DateTime  
    PeptideHypothesisParams      : List<PeptideHypothesisParam>
    }

and [<CLIMutable>] PeptideHypothesisParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : PeptideHypothesis
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] Person =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID             : int
    FirstName      : string 
    LastName       : string 
    MiddleName     : string 
    Organization   : Organization         //Not part of the original document
    RowVersion     : DateTime  
    PersonParams   : List<PersonParam>
    }

and [<CLIMutable>] PersonParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Person
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime   
    }

and [<CLIMutable>] ProteinAmbiguityGroup =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                          : int
    ProteinDetectionList        : ProteinDetectionList //Not part of the original document
    Name                        : string 
    RowVersion                  : DateTime
    ProteinAmbiguityGroupParams : List<ProteinAmbiguityGroupParam>
    }

and [<CLIMutable>] ProteinAmbiguityGroupParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : ProteinAmbiguityGroup
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime     
    }
//Added because it is part of the original document
and [<CLIMutable>] ProteinDetection =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID                        : int
    Name                      : string
    ProteinDetectionList      : ProteinDetectionList
    ProteinDetectionProtocoll : ProteinDetectionProtocol
    RowVersion                : DateTime
    ProteinDetectionParams    : List<ProteinDetectionParam>
    }

and [<CLIMutable>] ProteinDetectionParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : ProteinDetection
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime
    }

and [<CLIMutable>] ProteinDetectionHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    Name                             : string
    DBSequence                       : DBSequence
    ProteinAmbiguityGroup            : ProteinAmbiguityGroup //Not part of the original document 
    PassThreshold                    : string
    RowVersion                       : DateTime  
    ProteinDetectionHypothesisParams : List<ProteinDetectionHypothesisParam>
    }

and [<CLIMutable>] ProteinDetectionHypothesisParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : ProteinDetectionHypothesis
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime
    }

and [<CLIMutable>] ProteinDetectionList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                         : int
    Accession                  : string
    Name                       : string
    SearchDB                   : SearchDatabase       //Not part of the original document
    RowVersion                 : DateTime
    ProteinDetectionListParams : List<ProteinDetectionListParam>
    }

and [<CLIMutable>] ProteinDetectionListParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : ProteinDetectionList
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime
    }

and [<CLIMutable>] ProteinDetectionProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                             : int
    Name                           : string 
    AnalysisSoftware               : AnalysisSoftware
    RowVersion                     : DateTime
    ProteinDetectionProtocolParams : List<ProteinDetectionProtocolParam>
    }

and [<CLIMutable>] ProteinDetectionProtocolParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : ProteinDetectionProtocol
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }
//Added because it is part of the original document and refferd to in the code
and [<CLIMutable>] Sample =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int
    Name         : string 
    RowVersion   : DateTime
    SampleParams : List<SampleParam>
    }

and [<CLIMutable>] SampleParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Sample
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

//Added because it exists in the original document and is referred multiple times in the code
and [<CLIMutable>] SearchDatabase =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                             : int
    Name                           : string 
    Location                       : string //Location of the datafile, commonly an URL
    RowVersion                     : DateTime
    SearchDatabaseParams : List<SearchDatabaseParam>
    }

and [<CLIMutable>] SearchDatabaseParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : SearchDatabaseParam
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

//Added because it exists in the original document and is referred in the code
and [<CLIMutable>] SpectraData =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                : int
    Name              : string 
    Location          : string //Location of the datafile, commonly an URL
    RowVersion        : DateTime
    SpectraDataParams : List<SpectraDataParam>
    }

and [<CLIMutable>] SpectraDataParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : SpectraData
    [<RequiredAttribute()>]
    Term                   : Term
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

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

and [<CLIMutable>] SpectrumIdentificationItem =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    SpectrumIdentificationResult     : SpectrumIdentificationResult
    Sample                           : Sample          
    Peptide                          : Peptide
    MassTable                        : MassTable
    Name                             : string 
    PassThreshold                    : string
    Rank                             : int 
    CalculatedMassToCharge           : float 
    ExperimentalMassToCharge         : float
    ChargeState                      : int
    CalculatedIP                     : float 
    Fragmentation                    : DateTime  
    RowVersion                       : DateTime
    SpectrumIdentificationItemParams : List<SpectrumIdentificationItemParam>
    }

and [<CLIMutable>] SpectrumIdentificationItemParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : SpectrumIdentificationItem
    [<RequiredAttribute()>]
    Term           : Term
    Unit           : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] SpectrumIdentificationList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    Name                             : string 
    NumSequencesSearched             : int 
    RowVersion                       : DateTime
    SpectrumIdentificationListParams : List<SpectrumIdentificationListParam>
    }

and [<CLIMutable>] SpectrumIdentificationListParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : SpectrumIdentificationList
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] SpectrumIdentificationProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                   : int
    Name                                 : string 
    AnalysisSoftware                     : AnalysisSoftware
    RowVersion                           : DateTime 
    SpectrumIdentificationProtocolParams : List<SpectrumIdentificationProtocolParam>
    }

and [<CLIMutable>] SpectrumIdentificationProtocolParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : SpectrumIdentificationProtocol
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] SpectrumIdentificationResult =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                 : int
    SpectrumID                         : int    //Refers to SpectrumID, unique in the SpectraData for the specific Spectrum
    SpectraData                        : SpectraData       
    SpectrumIdentificationList         : SpectrumIdentificationList
    Name                               : string 
    RowVersion                         : DateTime 
    SpectrumIdentificationResultParams : List<SpectrumIdentificationResultParam>
    }

and [<CLIMutable>] SpectrumIdentificationResultParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : SpectrumIdentificationResult
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime    
    }

and [<CLIMutable>] Term =
    {
    ID             : string
    Name           : string
    Ontology       : Ontology
    RowVersion     : DateTime 
    }

and [<CLIMutable>] TermRelationShip =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    Term             : Term
    RelationShipType : string
    FKRelatedTerm    : Term
    RowVersion       : DateTime     
    }

and [<CLIMutable>] TermTag =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Term       : Term
    Name       : string
    Value      : string
    RowVersion : DateTime 
    }
//Added because it is part of the original document and refferd to in the code
and [<CLIMutable>] Translation =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                : int
    Name              : string 
    TranslationParams : List<TranslationParam>
    }

and [<CLIMutable>] TranslationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Translation
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime    
    }

///Defining Context of DB
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

    [<DefaultValue>] val mutable m_masstable : DbSet<MassTable>
    member public this.Masstable with get() = this.m_masstable
                                                and set value = this.m_masstable <- value

    [<DefaultValue>] val mutable m_masstableParam : DbSet<MassTableParam>
    member public this.MasstableParam with get() = this.m_masstableParam
                                                and set value = this.m_masstableParam <- value

    [<DefaultValue>] val mutable m_modification : DbSet<Modification>
    member public this.Modification with get() = this.m_modification
                                                and set value = this.m_modification <- value

    [<DefaultValue>] val mutable m_modificationParam : DbSet<ModificationParam>
    member public this.ModificationParam with get() = this.m_modificationParam
                                                and set value = this.m_modificationParam <- value

    [<DefaultValue>] val mutable m_modLocation : DbSet<ModLocation>
    member public this.ModLocation with get() = this.m_modLocation
                                                and set value = this.m_modLocation <- value

    [<DefaultValue>] val mutable m_modLocationParam : DbSet<ModLocationParam>
    member public this.ModLocationParam with get() = this.m_modLocationParam
                                                and set value = this.m_modLocationParam <- value

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

    [<DefaultValue>] val mutable m_proteinDetection : DbSet<ProteinDetection>
    member public this.ProteinDetection with get() = this.m_proteinDetection
                                                    and set value = this.m_proteinDetection <- value

    [<DefaultValue>] val mutable m_proteinDetectionParam : DbSet<ProteinDetectionParam>
    member public this.ProteinDetectionParam with get() = this.m_proteinDetectionParam
                                                    and set value = this.m_proteinDetectionParam <- value

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
    member public this.ProteinDetectionProtocolParam with get() = this.m_proteinDetectionProtocolParam
                                                        and set value = this.m_proteinDetectionProtocolParam <- value

    [<DefaultValue>] val mutable m_sample : DbSet<Sample>
    member public this.Sample with get() = this.m_sample
                                                        and set value = this.m_sample <- value

    [<DefaultValue>] val mutable m_sampleParam : DbSet<SampleParam>
    member public this.SampleParam with get() = this.m_sampleParam
                                                        and set value = this.m_sampleParam <- value

    [<DefaultValue>] val mutable m_searchDatabase : DbSet<SearchDatabase>
    member public this.SearchDatabase with get() = this.m_searchDatabase
                                            and set value = this.m_searchDatabase <- value

    [<DefaultValue>] val mutable m_searchDatabaseParam : DbSet<SearchDatabaseParam>
    member public this.SearchDatabaseParam with get() = this.m_searchDatabaseParam
                                                and set value = this.m_searchDatabaseParam <- value

    [<DefaultValue>] val mutable m_spectraData : DbSet<SpectraData>
    member public this.SpectraData with get() = this.m_spectraData
                                                and set value = this.m_spectraData <- value

    [<DefaultValue>] val mutable m_spectraDataParam : DbSet<SpectraDataParam>
    member public this.SpectraDataParam with get() = this.m_spectraDataParam
                                                and set value = this.m_spectraDataParam <- value

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

    [<DefaultValue>] val mutable m_translation : DbSet<Translation>
    member public this.Translation with get() = this.m_translation
                                        and set value = this.m_translation <- value

    [<DefaultValue>] val mutable m_translationParam : DbSet<TranslationParam>
    member public this.TranslationParam with get() = this.m_translationParam
                                        and set value = this.m_translationParam <- value

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

///Define functions to create input for DB
let createAnalysisSoftware inputName inputASParams =
    {
     AnalysisSoftware.ID                     = 0
     AnalysisSoftware.Name                   = inputName
     AnalysisSoftware.RowVersion             = DateTime.Now.Date
     AnalysisSoftware.AnalysisSoftwareParams = inputASParams
    }

let createAnalysisSoftwareParam inputAnalysisSoftware inputValue inputTerm inputUnit =
    {
     AnalysisSoftwareParam.ID               = 0
     AnalysisSoftwareParam.FKParamContainer = inputAnalysisSoftware
     AnalysisSoftwareParam.Value            = inputValue
     AnalysisSoftwareParam.Term             = inputTerm
     AnalysisSoftwareParam.Unit             = inputUnit
     AnalysisSoftwareParam.RowVersion       = DateTime.Now.Date
    }

let createDBSequence inputName inputAccession inputSearchDB inputDBSParams =
    {
     DBSequence.ID               = 0
     DBSequence.Name             = inputName
     DBSequence.Accession        = inputAccession
     DBSequence.SearchDB         = inputSearchDB
     DBSequence.RowVersion       = DateTime.Now.Date
     DBSequence.DBSequenceParams = inputDBSParams
    }

let createDBSequenceParam inputDBSequence inputValue inputTerm inputUnit =
    {
     DBSequenceParam.ID               = 0
     DBSequenceParam.FKParamContainer = inputDBSequence
     DBSequenceParam.Value            = inputValue
     DBSequenceParam.Term             = inputTerm
     DBSequenceParam.Unit             = inputUnit
     DBSequenceParam.RowVersion       = DateTime.Now.Date
    }

let createMassTable inputName inputMSLevel inputMTParams =
    {
     MassTable.ID              = 0
     MassTable.Name            = inputName
     MassTable.MSLevel         = inputMSLevel
     MassTable.RowVersion      = DateTime.Now.Date
     MassTable.MassTableParams = inputMTParams
    }

let createMassTableParam inputMassTable inputValue inputTerm inputUnit =
    {
     MassTableParam.ID               = 0
     MassTableParam.FKParamContainer = inputMassTable
     MassTableParam.Value            = inputValue
     MassTableParam.Term             = inputTerm
     MassTableParam.Unit             = inputUnit
     MassTableParam.RowVersion       = DateTime.Now.Date
    }

let createModification inputName inputAvgMassDelta inputMIMD inputResidues inputModLocation inputModificationParams =
    {
     Modification.ID                    = 0
     Modification.Name                  = inputName
     Modification.AvgMassDelta          = inputAvgMassDelta
     Modification.MonoisotopicMassDelta = inputMIMD
     Modification.Residues              = inputResidues
     Modification.ModLocation           = inputModLocation
     Modification.RowVersion            = DateTime.Now.Date
     Modification.ModificationParams    = inputModificationParams
    }

let createModificationParam inputModification inputValue inputTerm inputUnit =
    {
     ModificationParam.ID               = 0
     ModificationParam.FKParamContainer = inputModification
     ModificationParam.Value            = inputValue
     ModificationParam.Term             = inputTerm
     ModificationParam.Unit             = inputUnit
     ModificationParam.RowVersion       = DateTime.Now.Date
    }

let createOntology inputName inputParams inputTerms =
    {
     Ontology.ID             = 0
     Ontology.Name           = inputName
     Ontology.RowVersion     = DateTime.Now.Date
     Ontology.OntologyParams = inputParams
     Ontology.Terms          = inputTerms
    }

let createOntologyParam inputOntology inputValue inputTerm inputUnit =
    {
     OntologyParam.ID               = 0
     OntologyParam.FKParamContainer = inputOntology
     OntologyParam.Value            = inputValue
     OntologyParam.Term             = inputTerm
     OntologyParam.Unit             = inputUnit
     OntologyParam.RowVersion       = DateTime.Now.Date
    }

let createOrganization inputName inputParent inputOrganizationParams =
    {
     Organization.ID                 = 0
     Organization.Name               = inputName
     Organization.Parent             = inputParent
     Organization.RowVersion         = DateTime.Now.Date
     Organization.OrganizationParams = inputOrganizationParams
    }

let createOrganizationParam inputOrganization inputValue inputTerm inputUnit =
    {
     OrganizationParam.ID               = 0
     OrganizationParam.FKParamContainer = inputOrganization
     OrganizationParam.Value            = inputValue
     OrganizationParam.Term             = inputTerm
     OrganizationParam.Unit             = inputUnit
     OrganizationParam.RowVersion       = DateTime.Now.Date
    }

let createParent inputName inputCountry inputOrganization inputParentParams =
    {
     Parent.ID           = 0
     Parent.Name         = inputName
     Parent.Country      = inputCountry
     Parent.Organization = inputOrganization
     Parent.RowVersion   = DateTime.Now.Date
     Parent.ParentParams = inputParentParams
    }

let createParentParam inputParent inputValue inputTerm inputUnit =
    {
     ParentParam.ID               = 0
     ParentParam.FKParamContainer = inputParent
     ParentParam.Value            = inputValue
     ParentParam.Term             = inputTerm
     ParentParam.Unit             = inputUnit
     ParentParam.RowVersion       = DateTime.Now.Date
    }

let createPeptide inputName inputSequence inputPeptideParams =
    {
     Peptide.ID            = 0
     Peptide.Name          = inputName
     Peptide.Sequence      = inputSequence
     Peptide.RowVersion    = DateTime.Now.Date
     Peptide.PeptideParams = inputPeptideParams
    }

let createPeptideParam inputPeptide inputValue inputTerm inputUnit =
    {
     PeptideParam.ID               = 0
     PeptideParam.FKParamContainer = inputPeptide
     PeptideParam.Value            = inputValue
     PeptideParam.Term             = inputTerm
     PeptideParam.Unit             = inputUnit
     PeptideParam.RowVersion       = DateTime.Now.Date
    }

let createPeptideEvidence inputPeptide inputDBSequence inputIsDecoy inputFrame inputTranslation inputStart inputEnd inputPre inputPost inputPEParams =
    {
     PeptideEvidence.ID                    = 0
     PeptideEvidence.Peptide               = inputPeptide
     PeptideEvidence.DBSequence            = inputDBSequence
     PeptideEvidence.isDecoy               = inputIsDecoy
     PeptideEvidence.Frame                 = inputFrame
     PeptideEvidence.Translation           = inputTranslation
     PeptideEvidence.Start                 = inputStart
     PeptideEvidence.End                   = inputEnd
     PeptideEvidence.Pre                   = inputPre
     PeptideEvidence.Post                  = inputPost
     PeptideEvidence.RowVersion            = DateTime.Now.Date
     PeptideEvidence.PeptideEvidenceParams = inputPEParams
    }

let createPeptideEvidenceParam inputPeptideEvidence inputValue inputTerm inputUnit =
    {
     PeptideEvidenceParam.ID               = 0
     PeptideEvidenceParam.FKParamContainer = inputPeptideEvidence
     PeptideEvidenceParam.Value            = inputValue
     PeptideEvidenceParam.Term             = inputTerm
     PeptideEvidenceParam.Unit             = inputUnit
     PeptideEvidenceParam.RowVersion       = DateTime.Now.Date
    }

let createPeptideHypothesis inputPDHID inputPeptideEvidence inputPeptideHypothesisParams =
    {
     PeptideHypothesis.ID                           = 0
     PeptideHypothesis.PeptideDetectionHypothesisID = inputPDHID
     PeptideHypothesis.PeptideEvidence              = inputPeptideEvidence
     PeptideHypothesis.RowVersion                   = DateTime.Now.Date
     PeptideHypothesis.PeptideHypothesisParams      = inputPeptideHypothesisParams
    }

let createPeptideHypothesisParam inputPeptideHypothesis inputValue inputTerm inputUnit =
    {
     PeptideHypothesisParam.ID               = 0
     PeptideHypothesisParam.FKParamContainer = inputPeptideHypothesis
     PeptideHypothesisParam.Value            = inputValue
     PeptideHypothesisParam.Term             = inputTerm
     PeptideHypothesisParam.Unit             = inputUnit
     PeptideHypothesisParam.RowVersion       = DateTime.Now.Date
    }

let createPerson inputFirstName inputMiddleName inputLastName inputOrganization inputPersonenParams =
    {
     Person.ID           = 0
     Person.FirstName    = inputFirstName
     Person.MiddleName   = inputMiddleName
     Person.LastName     = inputLastName
     Person.Organization = inputOrganization
     Person.RowVersion   = DateTime.Now.Date
     Person.PersonParams = inputPersonenParams
    }

let createPersonParam inputPerson inputValue inputTerm inputUnit =
    {
     PersonParam.ID               = 0
     PersonParam.FKParamContainer = inputPerson
     PersonParam.Value            = inputValue
     PersonParam.Term             = inputTerm
     PersonParam.Unit             = inputUnit
     PersonParam.RowVersion       = DateTime.Now.Date
    }

let createProteinAmbiguityGroup inputName inputPDL inputPAP =
    {
     ProteinAmbiguityGroup.ID                          = 0
     ProteinAmbiguityGroup.Name                        = inputName
     ProteinAmbiguityGroup.ProteinDetectionList        = inputPDL
     ProteinAmbiguityGroup.RowVersion                  = DateTime.Now.Date
     ProteinAmbiguityGroup.ProteinAmbiguityGroupParams = inputPAP
    }

let createProteinAmbiguityGroupParam inputProteinAmbiguityGroup inputValue inputTerm inputUnit =
    {
     ProteinAmbiguityGroupParam.ID               = 0
     ProteinAmbiguityGroupParam.FKParamContainer = inputProteinAmbiguityGroup
     ProteinAmbiguityGroupParam.Value            = inputValue
     ProteinAmbiguityGroupParam.Term             = inputTerm
     ProteinAmbiguityGroupParam.Unit             = inputUnit
     ProteinAmbiguityGroupParam.RowVersion       = DateTime.Now.Date
    }

let createProteinDetection inputName inputPDProtocoll inputPDList inputPDParams =
    {
     ProteinDetection.ID                        = 0
     ProteinDetection.Name                      = inputName
     ProteinDetection.ProteinDetectionProtocoll = inputPDProtocoll
     ProteinDetection.ProteinDetectionList      = inputPDList
     ProteinDetection.RowVersion                = DateTime.Now.Date
     ProteinDetection.ProteinDetectionParams    = inputPDParams
    }

let createProteinDetectionParam inputProteinDetection inputValue inputTerm inputUnit =
    {
     ProteinDetectionParam.ID               = 0
     ProteinDetectionParam.FKParamContainer = inputProteinDetection
     ProteinDetectionParam.Value            = inputValue
     ProteinDetectionParam.Term             = inputTerm
     ProteinDetectionParam.Unit             = inputUnit
     ProteinDetectionParam.RowVersion       = DateTime.Now.Date
    }

let createProteinDetectionHypothesis inputName inputDBSequence inputPassThreshold inputPAGroup inputPDHParams =
    {
     ProteinDetectionHypothesis.ID                               = 0
     ProteinDetectionHypothesis.Name                             = inputName
     ProteinDetectionHypothesis.DBSequence                       = inputDBSequence
     ProteinDetectionHypothesis.PassThreshold                    = inputPassThreshold
     ProteinDetectionHypothesis.ProteinAmbiguityGroup            = inputPAGroup
     ProteinDetectionHypothesis.RowVersion                       = DateTime.Now.Date
     ProteinDetectionHypothesis.ProteinDetectionHypothesisParams = inputPDHParams
    }

let createProteinDetectionHypothesisParam inputProteinDetectionHypothesis inputValue inputTerm inputUnit =
    {
     ProteinDetectionHypothesisParam.ID               = 0
     ProteinDetectionHypothesisParam.FKParamContainer = inputProteinDetectionHypothesis
     ProteinDetectionHypothesisParam.Value            = inputValue
     ProteinDetectionHypothesisParam.Term             = inputTerm
     ProteinDetectionHypothesisParam.Unit             = inputUnit
     ProteinDetectionHypothesisParam.RowVersion       = DateTime.Now.Date
    }

let createProteinDetectionList inputName inputSearchDB inputAccession inputPDLParams =
    {
     ProteinDetectionList.ID                         = 0
     ProteinDetectionList.Name                       = inputName
     ProteinDetectionList.SearchDB                   = inputSearchDB
     ProteinDetectionList.Accession                  = inputAccession
     ProteinDetectionList.RowVersion                 = DateTime.Now.Date
     ProteinDetectionList.ProteinDetectionListParams = inputPDLParams
    }

let createProteinDetectionListParam inputProteinDetectionList inputValue inputTerm inputUnit =
    {
     ProteinDetectionListParam.ID               = 0
     ProteinDetectionListParam.FKParamContainer = inputProteinDetectionList
     ProteinDetectionListParam.Value            = inputValue
     ProteinDetectionListParam.Term             = inputTerm
     ProteinDetectionListParam.Unit             = inputUnit
     ProteinDetectionListParam.RowVersion       = DateTime.Now.Date
    }

let createProteinDetectionProtocol inputName inputAnalysisSoftware inputPDPParams =
    {
     ProteinDetectionProtocol.ID                             = 0
     ProteinDetectionProtocol.Name                           = inputName
     ProteinDetectionProtocol.AnalysisSoftware               = inputAnalysisSoftware
     ProteinDetectionProtocol.RowVersion                     = DateTime.Now.Date
     ProteinDetectionProtocol.ProteinDetectionProtocolParams = inputPDPParams
    }

let createProteinDetectionProtocolParam inputProteinDetectionProtocol inputValue inputTerm inputUnit =
    {
     ProteinDetectionProtocolParam.ID               = 0
     ProteinDetectionProtocolParam.FKParamContainer = inputProteinDetectionProtocol
     ProteinDetectionProtocolParam.Value            = inputValue
     ProteinDetectionProtocolParam.Term             = inputTerm
     ProteinDetectionProtocolParam.Unit             = inputUnit
     ProteinDetectionProtocolParam.RowVersion       = DateTime.Now.Date
    }

let createSample inputName inputSampleParams =
    {
     Sample.ID           = 0
     Sample.Name         = inputName
     Sample.RowVersion   = DateTime.Now.Date
     Sample.SampleParams = inputSampleParams
    }

let createSampleParam inputSample inputValue inputTerm inputUnit =
    {
     SampleParam.ID               = 0
     SampleParam.FKParamContainer = inputSample
     SampleParam.Value            = inputValue
     SampleParam.Term             = inputTerm
     SampleParam.Unit             = inputUnit
     SampleParam.RowVersion       = DateTime.Now.Date
    }

let createSearchDatabase inputName inputLocation inputSDParams =
    {
     SearchDatabase.ID                   = 0
     SearchDatabase.Name                 = inputName
     SearchDatabase.Location             = inputLocation
     SearchDatabase.RowVersion           = DateTime.Now.Date
     SearchDatabase.SearchDatabaseParams = inputSDParams
    }

let createSearchDatabaseParam inputSearchDatabase inputValue inputTerm inputUnit =
    {
     SearchDatabaseParam.ID               = 0
     SearchDatabaseParam.FKParamContainer = inputSearchDatabase
     SearchDatabaseParam.Value            = inputValue
     SearchDatabaseParam.Term             = inputTerm
     SearchDatabaseParam.Unit             = inputUnit
     SearchDatabaseParam.RowVersion       = DateTime.Now.Date
    }

let createSpectraData inputName inputLocation inputSpectraDataParams =
    {
     SpectraData.ID                = 0
     SpectraData.Name              = inputName
     SpectraData.Location          = inputLocation
     SpectraData.RowVersion        = DateTime.Now.Date
     SpectraData.SpectraDataParams = inputSpectraDataParams
    }

let createSpectraDataParam inputSpectraData inputValue inputTerm inputUnit =
    {
     SpectraDataParam.ID               = 0
     SpectraDataParam.FKParamContainer = inputSpectraData
     SpectraDataParam.Value            = inputValue
     SpectraDataParam.Term             = inputTerm
     SpectraDataParam.Unit             = inputUnit
     SpectraDataParam.RowVersion       = DateTime.Now.Date
    }

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

let createSpectrumIdentificationItem inputName inputPeptide inputChargeState inputSample inputPassThreshold inputFragmentation inputRank inputmassTable inputCalculatedIP inputCalculatedMassToCharge inputExperimentalMassToCharge inputSpectrumIdentificationResult inputSpectrumIdentificationItemParams =
    {
     SpectrumIdentificationItem.ID                               = 0
     SpectrumIdentificationItem.Name                             = inputName
     SpectrumIdentificationItem.Peptide                          = inputPeptide
     SpectrumIdentificationItem.ChargeState                      = inputChargeState
     SpectrumIdentificationItem.Sample                           = inputSample
     SpectrumIdentificationItem.PassThreshold                    = inputPassThreshold
     SpectrumIdentificationItem.Fragmentation                    = inputFragmentation
     SpectrumIdentificationItem.Rank                             = inputRank
     SpectrumIdentificationItem.MassTable                        = inputmassTable
     SpectrumIdentificationItem.CalculatedIP                     = inputCalculatedIP
     SpectrumIdentificationItem.CalculatedMassToCharge           = inputCalculatedMassToCharge
     SpectrumIdentificationItem.ExperimentalMassToCharge         = inputExperimentalMassToCharge
     SpectrumIdentificationItem.SpectrumIdentificationResult     = inputSpectrumIdentificationResult
     SpectrumIdentificationItem.RowVersion                       = DateTime.Now.Date
     SpectrumIdentificationItem.SpectrumIdentificationItemParams = inputSpectrumIdentificationItemParams
    }

let createSpectrumIdentificationItemParam inputSpectrumIdentificationItem inputValue inputTerm inputUnit =
    {
     SpectrumIdentificationItemParam.ID               = 0
     SpectrumIdentificationItemParam.FKParamContainer = inputSpectrumIdentificationItem
     SpectrumIdentificationItemParam.Value            = inputValue
     SpectrumIdentificationItemParam.Term             = inputTerm
     SpectrumIdentificationItemParam.Unit             = inputUnit
     SpectrumIdentificationItemParam.RowVersion       = DateTime.Now.Date
    }

let createSpectrumIdentificationList inputName inputNumSequencesSearched inputSpectrumIdentificationListParams =
    {
     SpectrumIdentificationList.ID                               = 0
     SpectrumIdentificationList.Name                             = inputName
     SpectrumIdentificationList.NumSequencesSearched             = inputNumSequencesSearched
     SpectrumIdentificationList.RowVersion                       = DateTime.Now.Date
     SpectrumIdentificationList.SpectrumIdentificationListParams = inputSpectrumIdentificationListParams
    }

let createSpectrumIdentificationListParam inputSpectrumIdentificationList inputValue inputTerm inputUnit =
    {
     SpectrumIdentificationListParam.ID               = 0
     SpectrumIdentificationListParam.FKParamContainer = inputSpectrumIdentificationList
     SpectrumIdentificationListParam.Value            = inputValue
     SpectrumIdentificationListParam.Term             = inputTerm
     SpectrumIdentificationListParam.Unit             = inputUnit
     SpectrumIdentificationListParam.RowVersion       = DateTime.Now.Date
    }

let createSpectrumIdentificationProtocol inputName inputAnalysisSoftware inputSpectrumIdentificationProtocolParams =
    {
     SpectrumIdentificationProtocol.ID                                   = 0
     SpectrumIdentificationProtocol.Name                                 = inputName
     SpectrumIdentificationProtocol.AnalysisSoftware                     = inputAnalysisSoftware
     SpectrumIdentificationProtocol.RowVersion                           = DateTime.Now.Date
     SpectrumIdentificationProtocol.SpectrumIdentificationProtocolParams = inputSpectrumIdentificationProtocolParams
    }

let createSpectrumIdentificationProtocolParam inputSpectrumIdentificationProtocol inputValue inputTerm inputUnit =
    {
     SpectrumIdentificationProtocolParam.ID               = 0
     SpectrumIdentificationProtocolParam.FKParamContainer = inputSpectrumIdentificationProtocol
     SpectrumIdentificationProtocolParam.Value            = inputValue
     SpectrumIdentificationProtocolParam.Term             = inputTerm
     SpectrumIdentificationProtocolParam.Unit             = inputUnit
     SpectrumIdentificationProtocolParam.RowVersion       = DateTime.Now.Date
    }

let createSpectrumIdentificationResult inputName inputSpectrumID inputSpectraData inputSpectrumIdentificationList inputSpectrumIdentificationResultParams =
    {
     SpectrumIdentificationResult.ID                                 = 0
     SpectrumIdentificationResult.Name                               = inputName
     SpectrumIdentificationResult.SpectrumID                         = inputSpectrumID
     SpectrumIdentificationResult.SpectraData                        = inputSpectraData
     SpectrumIdentificationResult.SpectrumIdentificationList         = inputSpectrumIdentificationList
     SpectrumIdentificationResult.RowVersion                         = DateTime.Now.Date
     SpectrumIdentificationResult.SpectrumIdentificationResultParams = inputSpectrumIdentificationResultParams
    }

let createSpectrumIdentificationResultParam inputSpectrumIdentificationResult inputValue inputTerm inputUnit =
    {
     SpectrumIdentificationResultParam.ID               = 0
     SpectrumIdentificationResultParam.FKParamContainer = inputSpectrumIdentificationResult
     SpectrumIdentificationResultParam.Value            = inputValue
     SpectrumIdentificationResultParam.Term             = inputTerm
     SpectrumIdentificationResultParam.Unit             = inputUnit
     SpectrumIdentificationResultParam.RowVersion       = DateTime.Now.Date
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
            if i.ID = "" 
                then select i.Ontology
           }
    )
readLine testTerm3

let testTerm4 =
    (query {
        for i in context.Term do
            if i.Name = "" && i.ID = "" 
                then select i
           }
    )
readLine testTerm4

let selectSpectrumIdentificationByID inputID =
    (query {
            for i in context.SpectrumIdentification do
                if i.ID = inputID 
                then select i})

let selectSpectrumIdentificationByName inputName =
    (query {
            for i in context.SpectrumIdentification do
                if i.Name = inputName 
                    then select i
           }
    )

let selectSpectrumIdentificationByActivityDate inputActivityDate =
    (query {
            for i in context.SpectrumIdentification do
                if i.ActivityDate = inputActivityDate 
                    then select i
           }
    )

let selectSpectrumIdentificationBySIList inputSIList =
    (query {
            for i in context.SpectrumIdentification do
                if i.SpectrumIdentificationList = inputSIList 
                    then select i
           }
    )

let selectSpectrumIdentificationBySIProtocoll inputSIProtocoll =
    (query {
            for i in context.SpectrumIdentification do
                if i.SpectrumIdentificationProtocoll = inputSIProtocoll 
                    then select i
           }
    )

let selectSpectrumIdentificationParamBySI inputSI =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer = inputSI 
                    then select (i, i.Term, i.Unit)
           }
    )

let selectSpectrumIdentificationParamBySIID inputSIID =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer.ID = inputSIID
                    then select (i, i.Term, i.Unit)
           }
    )

let selectSpectrumIdentificationParamBySIName inputSIName =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer.Name = inputSIName
                    then select (i, i.Term, i.Unit)
           }
    )

let selectTermByOntologyID inputOntologyID =
    (query {
            for i in context.Term do
                if i.Ontology.ID = inputOntologyID
                    then select (i)
           }
    )

let selectTermByOntologyName inputOntologyName =
    (query {
            for i in context.Term do
                if i.Ontology.Name = inputOntologyName
                    then select (i)
           }
    )

let selectTermByTermID inputTermID =
    (query {
            for i in context.Term do
                if i.ID = inputTermID
                    then select (i)
           }
    )

let selectTermByTermName inputTermName =
    (query {
            for i in context.Term do
                if i.Name = inputTermName
                    then select (i)
           }
    )

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
selectTermByOntologyID 3
|> readLine
selectTermByOntologyName "Pride"
|> readLine

///Testen verschiedener Varianten, Ergebnis anhand eines Linearitätstests überprüfen: Stabile Linie ja, nein?
 