
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
//open System.Reflection



///Defining types and relations for DB/////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////

type [<CLIMutable>] 
    AnalysisSoftware =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    Name                   : string 
    RowVersion             : DateTime
    AnalysisSoftwareParams : List<AnalysisSoftwareParam>
    }

and [<CLIMutable>] 
    AnalysisSoftwareParam =
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

and [<CLIMutable>] 
    DBSequence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    Accession        : string
    Name             : string
    SearchDB         : SearchDatabase
    RowVersion       : DateTime 
    DBSequenceParams : List<DBSequenceParam>
    }

and [<CLIMutable>] 
    DBSequenceParam =
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
and [<CLIMutable>] 
    MassTable =
    {
    ID : int
    Name : string
    MSLevel : List<int> //MS spectrum that the MassTable reffers to, e.g. "1" for MS1
    RowVersion : DateTime
    MassTableParams : List<MassTableParam> 
    }

and [<CLIMutable>] 
    MassTableParam =
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
and [<CLIMutable>] 
    Modification =
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

and [<CLIMutable>] 
    ModificationParam =
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
and [<CLIMutable>] 
    ModLocation =
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

and [<CLIMutable>] 
    ModLocationParam =
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
and [<CLIMutable>] 
    Ontology = 
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID             : int
    Name           : string
    RowVersion     : DateTime
    OntologyParams : List<OntologyParam>
    Terms          : List<Term>
    }

and [<CLIMutable>] 
    OntologyParam =
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

and [<CLIMutable>] 
    Organization =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                 : int
    Name               : string
    Parent             : Parent //Added to create connection to Parent
    RowVersion         : DateTime
    OrganizationParams : List<OrganizationParam>
    }

and [<CLIMutable>] 
    OrganizationParam =
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

and [<CLIMutable>] 
    Parent =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int            //Not part of the original document
    Name         : string         //Not part of the original document
    Organization : Organization 
    Country      : string         //Not part of the original document
    RowVersion   : DateTime
    ParentParams : List<ParentParam>
    }

and [<CLIMutable>] 
    ParentParam =
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

and [<CLIMutable>] 
    Peptide =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID            : int
    Name          : string             //Part of the original document
    Sequence      : string             //Not part of the original document
    RowVersion    : DateTime 
    PeptideParams : List<PeptideParam>
    }

and [<CLIMutable>] 
    PeptideParam =
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

and [<CLIMutable>] 
    PeptideEvidence =
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

and [<CLIMutable>] 
    PeptideEvidenceParam =
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

and [<CLIMutable>] 
    PeptideHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                           : int
    PeptideEvidence              : PeptideEvidence
    PeptideDetectionHypothesisID : int                //Not part of the original document
    RowVersion                   : DateTime  
    PeptideHypothesisParams      : List<PeptideHypothesisParam>
    }

and [<CLIMutable>] 
    PeptideHypothesisParam =
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

and [<CLIMutable>] 
    Person =
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

and [<CLIMutable>] 
    PersonParam =
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

and [<CLIMutable>] 
    ProteinAmbiguityGroup =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                          : int
    ProteinDetectionList        : ProteinDetectionList //Not part of the original document
    Name                        : string 
    RowVersion                  : DateTime
    ProteinAmbiguityGroupParams : List<ProteinAmbiguityGroupParam>
    }

and [<CLIMutable>] 
    ProteinAmbiguityGroupParam =
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
and [<CLIMutable>] 
    ProteinDetection =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID                        : int
    Name                      : string
    ProteinDetectionList      : ProteinDetectionList
    ProteinDetectionProtocoll : ProteinDetectionProtocol
    RowVersion                : DateTime
    ProteinDetectionParams    : List<ProteinDetectionParam>
    }

and [<CLIMutable>] 
    ProteinDetectionParam =
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

and [<CLIMutable>] 
    ProteinDetectionHypothesis =
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

and [<CLIMutable>] 
    ProteinDetectionHypothesisParam =
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

and [<CLIMutable>] 
    ProteinDetectionList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                         : int
    Accession                  : string
    Name                       : string
    SearchDB                   : SearchDatabase       //Not part of the original document
    RowVersion                 : DateTime
    ProteinDetectionListParams : List<ProteinDetectionListParam>
    }

and [<CLIMutable>] 
    ProteinDetectionListParam =
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

and [<CLIMutable>] 
    ProteinDetectionProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                             : int
    Name                           : string 
    AnalysisSoftware               : AnalysisSoftware
    RowVersion                     : DateTime
    ProteinDetectionProtocolParams : List<ProteinDetectionProtocolParam>
    }

and [<CLIMutable>] 
    ProteinDetectionProtocolParam =
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
and [<CLIMutable>] 
    Sample =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int
    Name         : string 
    RowVersion   : DateTime
    SampleParams : List<SampleParam>
    }

and [<CLIMutable>] 
    SampleParam =
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
and [<CLIMutable>] 
    SearchDatabase =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                             : int
    Name                           : string 
    Location                       : string //Location of the datafile, commonly an URL
    RowVersion                     : DateTime
    SearchDatabaseParams : List<SearchDatabaseParam>
    }

and [<CLIMutable>] 
    SearchDatabaseParam =
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
and [<CLIMutable>] 
    SpectraData =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                : int
    Name              : string 
    Location          : string //Location of the datafile, commonly an URL
    RowVersion        : DateTime
    SpectraDataParams : List<SpectraDataParam>
    }

and [<CLIMutable>] 
    SpectraDataParam =
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

and [<CLIMutable>] 
    SpectrumIdentification =
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

and [<CLIMutable>] 
    SpectrumIdentificationParam =
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

and [<CLIMutable>] 
    SpectrumIdentificationItem =
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

and [<CLIMutable>] 
    SpectrumIdentificationItemParam =
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

and [<CLIMutable>] 
    SpectrumIdentificationList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    Name                             : string 
    NumSequencesSearched             : int 
    RowVersion                       : DateTime
    SpectrumIdentificationListParams : List<SpectrumIdentificationListParam>
    }

and [<CLIMutable>] 
    SpectrumIdentificationListParam =
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

and [<CLIMutable>] 
    SpectrumIdentificationProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                   : int
    Name                                 : string 
    AnalysisSoftware                     : AnalysisSoftware
    RowVersion                           : DateTime 
    SpectrumIdentificationProtocolParams : List<SpectrumIdentificationProtocolParam>
    }

and [<CLIMutable>] 
    SpectrumIdentificationProtocolParam =
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

and [<CLIMutable>] 
    SpectrumIdentificationResult =
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

and [<CLIMutable>] 
    SpectrumIdentificationResultParam =
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

and [<CLIMutable>] 
    Term =
    {
    ID             : string
    Name           : string
    Ontology       : Ontology
    RowVersion     : DateTime 
    }

and [<CLIMutable>] 
    TermRelationShip =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    Term             : Term
    RelationShipType : string
    RelatedTerm      : Term
    RowVersion       : DateTime     
    }

and [<CLIMutable>] 
    TermTag =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Term       : Term
    Name       : string
    Value      : string
    RowVersion : DateTime 
    }
//Added because it is part of the original document and refferd to in the code
and [<CLIMutable>] 
    Translation =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                : int
    Name              : string 
    TranslationParams : List<TranslationParam>
    }

and [<CLIMutable>] 
    TranslationParam =
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
    
    [<DefaultValue>] 
    val mutable m_analysisSoftware : DbSet<AnalysisSoftware>
    member public this.AnalysisSoftware with get() = this.m_analysisSoftware
                                                and set value = this.m_analysisSoftware <- value

    [<DefaultValue>] 
    val mutable m_analysisSoftwareParam : DbSet<AnalysisSoftwareParam>
    member public this.AnalysisSoftwareParam with get() = this.m_analysisSoftwareParam
                                                and set value = this.m_analysisSoftwareParam <- value
    
    [<DefaultValue>] 
    val mutable m_dbSequence : DbSet<DBSequence>
    member public this.DBSequence with get() = this.m_dbSequence
                                                and set value = this.m_dbSequence <- value

    [<DefaultValue>] 
    val mutable m_dbSequenceParam : DbSet<DBSequenceParam>
    member public this.DBSequenceParam with get() = this.m_dbSequenceParam
                                                and set value = this.m_dbSequenceParam <- value

    [<DefaultValue>] 
    val mutable m_masstable : DbSet<MassTable>
    member public this.Masstable with get() = this.m_masstable
                                                and set value = this.m_masstable <- value

    [<DefaultValue>] 
    val mutable m_masstableParam : DbSet<MassTableParam>
    member public this.MasstableParam with get() = this.m_masstableParam
                                                and set value = this.m_masstableParam <- value

    [<DefaultValue>] 
    val mutable m_modification : DbSet<Modification>
    member public this.Modification with get() = this.m_modification
                                                and set value = this.m_modification <- value

    [<DefaultValue>] 
    val mutable m_modificationParam : DbSet<ModificationParam>
    member public this.ModificationParam with get() = this.m_modificationParam
                                                and set value = this.m_modificationParam <- value

    [<DefaultValue>] 
    val mutable m_modLocation : DbSet<ModLocation>
    member public this.ModLocation with get() = this.m_modLocation
                                                and set value = this.m_modLocation <- value

    [<DefaultValue>] 
    val mutable m_modLocationParam : DbSet<ModLocationParam>
    member public this.ModLocationParam with get() = this.m_modLocationParam
                                                and set value = this.m_modLocationParam <- value

    [<DefaultValue>] 
    val mutable m_ontology : DbSet<Ontology>
    member public this.Ontology with get() = this.m_ontology
                                                and set value = this.m_ontology <- value

    [<DefaultValue>] 
    val mutable m_organization : DbSet<Organization>
    member public this.Organization with get() = this.m_organization
                                                and set value = this.m_organization <- value

    [<DefaultValue>] 
    val mutable m_organizationParam : DbSet<OrganizationParam>
    member public this.OrganizationParam with get() = this.m_organizationParam
                                                and set value = this.m_organizationParam <- value

    [<DefaultValue>] 
    val mutable m_parent : DbSet<Parent>
    member public this.Parent with get() = this.m_parent
                                                and set value = this.m_parent <- value

    [<DefaultValue>] 
    val mutable m_peptide : DbSet<Peptide>
    member public this.Peptide with get() = this.m_peptide
                                                and set value = this.m_peptide <- value

    [<DefaultValue>] 
    val mutable m_peptideParam : DbSet<PeptideParam>
    member public this.PeptideParam with get() = this.m_peptideParam
                                                and set value = this.m_peptideParam <- value

    [<DefaultValue>] 
    val mutable m_peptideEvidence : DbSet<PeptideEvidence>
    member public this.PeptideEvidence with get() = this.m_peptideEvidence
                                                and set value = this.m_peptideEvidence <- value

    [<DefaultValue>] 
    val mutable m_peptideEvidenceParam : DbSet<PeptideEvidenceParam>
    member public this.PeptideEvidenceParam with get() = this.m_peptideEvidenceParam
                                                and set value = this.m_peptideEvidenceParam <- value

    [<DefaultValue>] 
    val mutable m_peptideHypothesis : DbSet<PeptideHypothesis>
    member public this.PeptideHypothesis with get() = this.m_peptideHypothesis
                                                and set value = this.m_peptideHypothesis <- value

    [<DefaultValue>] 
    val mutable m_peptideHypothesisParam : DbSet<PeptideHypothesisParam>
    member public this.PeptideHypothesisParam with get() = this.m_peptideHypothesisParam
                                                and set value = this.m_peptideHypothesisParam <- value

    [<DefaultValue>] 
    val mutable m_person : DbSet<Person>
    member public this.Person with get() = this.m_person
                                                and set value = this.m_person <- value

    [<DefaultValue>] 
    val mutable m_personParam : DbSet<Person>
    member public this.PersonParam with get() = this.m_personParam
                                                and set value = this.m_personParam <- value

    [<DefaultValue>] 
    val mutable m_proteinAmbiguityGroup : DbSet<ProteinAmbiguityGroup>
    member public this.ProteinAmbiguityGroup with get() = this.m_proteinAmbiguityGroup
                                                and set value = this.m_proteinAmbiguityGroup <- value

    [<DefaultValue>] 
    val mutable m_proteinAmbiguityGroupParam : DbSet<ProteinAmbiguityGroupParam>
    member public this.ProteinAmbiguityGroupParam with get() = this.m_proteinAmbiguityGroupParam
                                                    and set value = this.m_proteinAmbiguityGroupParam <- value

    [<DefaultValue>] 
    val mutable m_proteinDetection : DbSet<ProteinDetection>
    member public this.ProteinDetection with get() = this.m_proteinDetection
                                                    and set value = this.m_proteinDetection <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionParam : DbSet<ProteinDetectionParam>
    member public this.ProteinDetectionParam with get() = this.m_proteinDetectionParam
                                                    and set value = this.m_proteinDetectionParam <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionHypothesis : DbSet<ProteinDetectionHypothesis>
    member public this.ProteinDetectionHypothesis with get() = this.m_proteinDetectionHypothesis
                                                    and set value = this.m_proteinDetectionHypothesis <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionHypothesisParam : DbSet<ProteinDetectionHypothesisParam>
    member public this.ProteinDetectionHypothesisParam with get() = this.m_proteinDetectionHypothesisParam
                                                       and set value = this.m_proteinDetectionHypothesisParam <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionList : DbSet<ProteinDetectionList>
    member public this.ProteinDetectionList with get() = this.m_proteinDetectionList
                                                    and set value = this.m_proteinDetectionList <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionListParam : DbSet<ProteinDetectionListParam>
    member public this.ProteinDetectionListParam with get() = this.m_proteinDetectionListParam
                                                    and set value = this.m_proteinDetectionListParam <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionProtocol : DbSet<ProteinDetectionProtocol>
    member public this.ProteinDetectionProtocol with get() = this.m_proteinDetectionProtocol
                                                    and set value = this.m_proteinDetectionProtocol <- value

    [<DefaultValue>] 
    val mutable m_proteinDetectionProtocolParam : DbSet<ProteinDetectionProtocolParam>
    member public this.ProteinDetectionProtocolParam with get() = this.m_proteinDetectionProtocolParam
                                                        and set value = this.m_proteinDetectionProtocolParam <- value

    [<DefaultValue>] 
    val mutable m_sample : DbSet<Sample>
    member public this.Sample with get() = this.m_sample
                                                        and set value = this.m_sample <- value

    [<DefaultValue>] 
    val mutable m_sampleParam : DbSet<SampleParam>
    member public this.SampleParam with get() = this.m_sampleParam
                                                        and set value = this.m_sampleParam <- value

    [<DefaultValue>] 
    val mutable m_searchDatabase : DbSet<SearchDatabase>
    member public this.SearchDatabase with get() = this.m_searchDatabase
                                            and set value = this.m_searchDatabase <- value

    [<DefaultValue>] 
    val mutable m_searchDatabaseParam : DbSet<SearchDatabaseParam>
    member public this.SearchDatabaseParam with get() = this.m_searchDatabaseParam
                                                and set value = this.m_searchDatabaseParam <- value

    [<DefaultValue>] 
    val mutable m_spectraData : DbSet<SpectraData>
    member public this.SpectraData with get() = this.m_spectraData
                                                and set value = this.m_spectraData <- value

    [<DefaultValue>] 
    val mutable m_spectraDataParam : DbSet<SpectraDataParam>
    member public this.SpectraDataParam with get() = this.m_spectraDataParam
                                                and set value = this.m_spectraDataParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentification : DbSet<SpectrumIdentification>
    member public this.SpectrumIdentification with get() = this.m_spectrumIdentification
                                                    and set value = this.m_spectrumIdentification <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationParam : DbSet<SpectrumIdentificationParam>
    member public this.SpectrumIdentificationParam with get() = this.m_spectrumIdentificationParam
                                                    and set value = this.m_spectrumIdentificationParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationItem : DbSet<SpectrumIdentificationItem>
    member public this.SpectrumIdentificationItem with get() = this.m_spectrumIdentificationItem
                                                    and set value = this.m_spectrumIdentificationItem <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationItemParam : DbSet<SpectrumIdentificationItemParam>
    member public this.SpectrumIdentificationItemParam with get() = this.m_spectrumIdentificationItemParam
                                                       and set value = this.m_spectrumIdentificationItemParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationList : DbSet<SpectrumIdentificationList>
    member public this.SpectrumIdentificationList with get() = this.m_spectrumIdentificationList
                                                    and set value = this.m_spectrumIdentificationList <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationListParam : DbSet<SpectrumIdentificationListParam>
    member public this.SpectrumIdentificationListParam with get() = this.m_spectrumIdentificationListParam
                                                        and set value = this.m_spectrumIdentificationListParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationProtocol : DbSet<SpectrumIdentificationProtocol>
    member public this.SpectrumIdentificationProtocol with get() = this.m_spectrumIdentificationProtocol
                                                        and set value = this.m_spectrumIdentificationProtocol <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationProtocolParam : DbSet<SpectrumIdentificationProtocolParam>
    member public this.SpectrumIdentificationProtocolParam with get() = this.m_spectrumIdentificationProtocolParam
                                                            and set value = this.m_spectrumIdentificationProtocolParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationResult : DbSet<SpectrumIdentificationResult>
    member public this.SpectrumIdentificationResult with get() = this.m_spectrumIdentificationResult
                                                    and set value = this.m_spectrumIdentificationResult <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationResultParam : DbSet<SpectrumIdentificationResultParam>
    member public this.SpectrumIdentificationResultParam with get() = this.m_spectrumIdentificationResultParam
                                                            and set value = this.m_spectrumIdentificationResultParam <- value

    [<DefaultValue>] 
    val mutable m_term : DbSet<Term>
    member public this.Term with get() = this.m_term
                                        and set value = this.m_term <- value

    [<DefaultValue>] 
    val mutable m_termRelationShip : DbSet<TermRelationShip>
    member public this.TermRelationShip with get() = this.m_termRelationShip
                                        and set value = this.m_termRelationShip <- value

    [<DefaultValue>] 
    val mutable m_termTag : DbSet<TermTag>
    member public this.TermTag with get() = this.m_termTag
                                        and set value = this.m_termTag <- value

    [<DefaultValue>] 
    val mutable m_translation : DbSet<Translation>
    member public this.Translation with get() = this.m_translation
                                        and set value = this.m_translation <- value

    [<DefaultValue>] 
    val mutable m_translationParam : DbSet<TranslationParam>
    member public this.TranslationParam with get() = this.m_translationParam
                                        and set value = this.m_translationParam <- value

    override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
        let fileDir = __SOURCE_DIRECTORY__ 
        let dbPath = fileDir + "\Ontologies_Terms\MSDatenbank.db"
        optionsBuilder.EnableSensitiveDataLogging() |> ignore
        optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore

///Define functions to create  for tables of DB////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
module InsertStatements =

    let createAnalysisSoftware Name ASParams =
        {
         AnalysisSoftware.ID                     = 0
         AnalysisSoftware.Name                   = Name
         AnalysisSoftware.RowVersion             = DateTime.Now.Date
         AnalysisSoftware.AnalysisSoftwareParams = ASParams
        }

    let createAnalysisSoftwareParam AnalysisSoftware Value Term Unit =
        {
         AnalysisSoftwareParam.ID               = 0
         AnalysisSoftwareParam.FKParamContainer = AnalysisSoftware
         AnalysisSoftwareParam.Value            = Value
         AnalysisSoftwareParam.Term             = Term
         AnalysisSoftwareParam.Unit             = Unit
         AnalysisSoftwareParam.RowVersion       = DateTime.Now.Date
        }

    let createDBSequence Name Accession SearchDB DBSParams =
        {
         DBSequence.ID               = 0
         DBSequence.Name             = Name
         DBSequence.Accession        = Accession
         DBSequence.SearchDB         = SearchDB
         DBSequence.RowVersion       = DateTime.Now.Date
         DBSequence.DBSequenceParams = DBSParams
        }

    let createDBSequenceParam DBSequence Value Term Unit =
        {
         DBSequenceParam.ID               = 0
         DBSequenceParam.FKParamContainer = DBSequence
         DBSequenceParam.Value            = Value
         DBSequenceParam.Term             = Term
         DBSequenceParam.Unit             = Unit
         DBSequenceParam.RowVersion       = DateTime.Now.Date
        }

    let createMassTable Name MSLevel MTParams =
        {
         MassTable.ID              = 0
         MassTable.Name            = Name
         MassTable.MSLevel         = MSLevel
         MassTable.RowVersion      = DateTime.Now.Date
         MassTable.MassTableParams = MTParams
        }

    let createMassTableParam MassTable Value Term Unit =
        {
         MassTableParam.ID               = 0
         MassTableParam.FKParamContainer = MassTable
         MassTableParam.Value            = Value
         MassTableParam.Term             = Term
         MassTableParam.Unit             = Unit
         MassTableParam.RowVersion       = DateTime.Now.Date
        }

    let createModification Name AvgMassDelta MIMD Residues ModLocation ModificationParams =
        {
         Modification.ID                    = 0
         Modification.Name                  = Name
         Modification.AvgMassDelta          = AvgMassDelta
         Modification.MonoisotopicMassDelta = MIMD
         Modification.Residues              = Residues
         Modification.ModLocation           = ModLocation
         Modification.RowVersion            = DateTime.Now.Date
         Modification.ModificationParams    = ModificationParams
        }

    let createModificationParam Modification Value Term Unit =
        {
         ModificationParam.ID               = 0
         ModificationParam.FKParamContainer = Modification
         ModificationParam.Value            = Value
         ModificationParam.Term             = Term
         ModificationParam.Unit             = Unit
         ModificationParam.RowVersion       = DateTime.Now.Date
        }

    let createModLocation Modification Location Peptide Residue ModlocationParams =
        {
         ModLocation.ID = 0
         ModLocation.Modification = Modification
         ModLocation.Location = Location
         ModLocation.Peptide = Peptide
         ModLocation.Residue = Residue
         ModLocation.RowVersion = DateTime.Now.Date
         ModLocation.ModLocationParams = ModlocationParams
        }

    let createModLocationParam ModLocation Value Term Unit =
        {
         ModLocationParam.ID               = 0
         ModLocationParam.FKParamContainer = ModLocation
         ModLocationParam.Value            = Value
         ModLocationParam.Term             = Term
         ModLocationParam.Unit             = Unit
         ModLocationParam.RowVersion       = DateTime.Now.Date
        }

    let createOntology Name Params Terms =
        {
         Ontology.ID             = 0
         Ontology.Name           = Name
         Ontology.RowVersion     = DateTime.Now.Date
         Ontology.OntologyParams = Params
         Ontology.Terms          = Terms
        }

    let createOntologyParam Ontology Value Term Unit =
        {
         OntologyParam.ID               = 0
         OntologyParam.FKParamContainer = Ontology
         OntologyParam.Value            = Value
         OntologyParam.Term             = Term
         OntologyParam.Unit             = Unit
         OntologyParam.RowVersion       = DateTime.Now.Date
        }

    let createOrganization Name Parent OrganizationParams =
        {
         Organization.ID                 = 0
         Organization.Name               = Name
         Organization.Parent             = Parent
         Organization.RowVersion         = DateTime.Now.Date
         Organization.OrganizationParams = OrganizationParams
        }

    let createOrganizationParam Organization Value Term Unit =
        {
         OrganizationParam.ID               = 0
         OrganizationParam.FKParamContainer = Organization
         OrganizationParam.Value            = Value
         OrganizationParam.Term             = Term
         OrganizationParam.Unit             = Unit
         OrganizationParam.RowVersion       = DateTime.Now.Date
        }

    let createParent Name Country Organization ParentParams =
        {
         Parent.ID           = 0
         Parent.Name         = Name
         Parent.Country      = Country
         Parent.Organization = Organization
         Parent.RowVersion   = DateTime.Now.Date
         Parent.ParentParams = ParentParams
        }

    let createParentParam Parent Value Term Unit =
        {
         ParentParam.ID               = 0
         ParentParam.FKParamContainer = Parent
         ParentParam.Value            = Value
         ParentParam.Term             = Term
         ParentParam.Unit             = Unit
         ParentParam.RowVersion       = DateTime.Now.Date
        }

    let createPeptide Name Sequence PeptideParams =
        {
         Peptide.ID            = 0
         Peptide.Name          = Name
         Peptide.Sequence      = Sequence
         Peptide.RowVersion    = DateTime.Now.Date
         Peptide.PeptideParams = PeptideParams
        }

    let createPeptideParam Peptide Value Term Unit =
        {
         PeptideParam.ID               = 0
         PeptideParam.FKParamContainer = Peptide
         PeptideParam.Value            = Value
         PeptideParam.Term             = Term
         PeptideParam.Unit             = Unit
         PeptideParam.RowVersion       = DateTime.Now.Date
        }

    let createPeptideEvidence Peptide DBSequence IsDecoy Frame Translation Start End Pre Post PEParams =
        {
         PeptideEvidence.ID                    = 0
         PeptideEvidence.Peptide               = Peptide
         PeptideEvidence.DBSequence            = DBSequence
         PeptideEvidence.isDecoy               = IsDecoy
         PeptideEvidence.Frame                 = Frame
         PeptideEvidence.Translation           = Translation
         PeptideEvidence.Start                 = Start
         PeptideEvidence.End                   = End
         PeptideEvidence.Pre                   = Pre
         PeptideEvidence.Post                  = Post
         PeptideEvidence.RowVersion            = DateTime.Now.Date
         PeptideEvidence.PeptideEvidenceParams = PEParams
        }

    let createPeptideEvidenceParam PeptideEvidence Value Term Unit =
        {
         PeptideEvidenceParam.ID               = 0
         PeptideEvidenceParam.FKParamContainer = PeptideEvidence
         PeptideEvidenceParam.Value            = Value
         PeptideEvidenceParam.Term             = Term
         PeptideEvidenceParam.Unit             = Unit
         PeptideEvidenceParam.RowVersion       = DateTime.Now.Date
        }

    let createPeptideHypothesis PDHID PeptideEvidence PeptideHypothesisParams =
        {
         PeptideHypothesis.ID                           = 0
         PeptideHypothesis.PeptideDetectionHypothesisID = PDHID
         PeptideHypothesis.PeptideEvidence              = PeptideEvidence
         PeptideHypothesis.RowVersion                   = DateTime.Now.Date
         PeptideHypothesis.PeptideHypothesisParams      = PeptideHypothesisParams
        }

    let createPeptideHypothesisParam PeptideHypothesis Value Term Unit =
        {
         PeptideHypothesisParam.ID               = 0
         PeptideHypothesisParam.FKParamContainer = PeptideHypothesis
         PeptideHypothesisParam.Value            = Value
         PeptideHypothesisParam.Term             = Term
         PeptideHypothesisParam.Unit             = Unit
         PeptideHypothesisParam.RowVersion       = DateTime.Now.Date
        }

    let createPerson FirstName MiddleName LastName Organization PersonenParams =
        {
         Person.ID           = 0
         Person.FirstName    = FirstName
         Person.MiddleName   = MiddleName
         Person.LastName     = LastName
         Person.Organization = Organization
         Person.RowVersion   = DateTime.Now.Date
         Person.PersonParams = PersonenParams
        }

    let createPersonParam Person Value Term Unit =
        {
         PersonParam.ID               = 0
         PersonParam.FKParamContainer = Person
         PersonParam.Value            = Value
         PersonParam.Term             = Term
         PersonParam.Unit             = Unit
         PersonParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinAmbiguityGroup Name PDL PAP =
        {
         ProteinAmbiguityGroup.ID                          = 0
         ProteinAmbiguityGroup.Name                        = Name
         ProteinAmbiguityGroup.ProteinDetectionList        = PDL
         ProteinAmbiguityGroup.RowVersion                  = DateTime.Now.Date
         ProteinAmbiguityGroup.ProteinAmbiguityGroupParams = PAP
        }

    let createProteinAmbiguityGroupParam ProteinAmbiguityGroup Value Term Unit =
        {
         ProteinAmbiguityGroupParam.ID               = 0
         ProteinAmbiguityGroupParam.FKParamContainer = ProteinAmbiguityGroup
         ProteinAmbiguityGroupParam.Value            = Value
         ProteinAmbiguityGroupParam.Term             = Term
         ProteinAmbiguityGroupParam.Unit             = Unit
         ProteinAmbiguityGroupParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetection Name PDProtocoll PDList PDParams =
        {
         ProteinDetection.ID                        = 0
         ProteinDetection.Name                      = Name
         ProteinDetection.ProteinDetectionProtocoll = PDProtocoll
         ProteinDetection.ProteinDetectionList      = PDList
         ProteinDetection.RowVersion                = DateTime.Now.Date
         ProteinDetection.ProteinDetectionParams    = PDParams
        }

    let createProteinDetectionParam ProteinDetection Value Term Unit =
        {
         ProteinDetectionParam.ID               = 0
         ProteinDetectionParam.FKParamContainer = ProteinDetection
         ProteinDetectionParam.Value            = Value
         ProteinDetectionParam.Term             = Term
         ProteinDetectionParam.Unit             = Unit
         ProteinDetectionParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetectionHypothesis Name DBSequence PassThreshold PAGroup PDHParams =
        {
         ProteinDetectionHypothesis.ID                               = 0
         ProteinDetectionHypothesis.Name                             = Name
         ProteinDetectionHypothesis.DBSequence                       = DBSequence
         ProteinDetectionHypothesis.PassThreshold                    = PassThreshold
         ProteinDetectionHypothesis.ProteinAmbiguityGroup            = PAGroup
         ProteinDetectionHypothesis.RowVersion                       = DateTime.Now.Date
         ProteinDetectionHypothesis.ProteinDetectionHypothesisParams = PDHParams
        }

    let createProteinDetectionHypothesisParam ProteinDetectionHypothesis Value Term Unit =
        {
         ProteinDetectionHypothesisParam.ID               = 0
         ProteinDetectionHypothesisParam.FKParamContainer = ProteinDetectionHypothesis
         ProteinDetectionHypothesisParam.Value            = Value
         ProteinDetectionHypothesisParam.Term             = Term
         ProteinDetectionHypothesisParam.Unit             = Unit
         ProteinDetectionHypothesisParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetectionList Name SearchDB Accession PDLParams =
        {
         ProteinDetectionList.ID                         = 0
         ProteinDetectionList.Name                       = Name
         ProteinDetectionList.SearchDB                   = SearchDB
         ProteinDetectionList.Accession                  = Accession
         ProteinDetectionList.RowVersion                 = DateTime.Now.Date
         ProteinDetectionList.ProteinDetectionListParams = PDLParams
        }

    let createProteinDetectionListParam ProteinDetectionList Value Term Unit =
        {
         ProteinDetectionListParam.ID               = 0
         ProteinDetectionListParam.FKParamContainer = ProteinDetectionList
         ProteinDetectionListParam.Value            = Value
         ProteinDetectionListParam.Term             = Term
         ProteinDetectionListParam.Unit             = Unit
         ProteinDetectionListParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetectionProtocol Name AnalysisSoftware PDPParams =
        {
         ProteinDetectionProtocol.ID                             = 0
         ProteinDetectionProtocol.Name                           = Name
         ProteinDetectionProtocol.AnalysisSoftware               = AnalysisSoftware
         ProteinDetectionProtocol.RowVersion                     = DateTime.Now.Date
         ProteinDetectionProtocol.ProteinDetectionProtocolParams = PDPParams
        }

    let createProteinDetectionProtocolParam ProteinDetectionProtocol Value Term Unit =
        {
         ProteinDetectionProtocolParam.ID               = 0
         ProteinDetectionProtocolParam.FKParamContainer = ProteinDetectionProtocol
         ProteinDetectionProtocolParam.Value            = Value
         ProteinDetectionProtocolParam.Term             = Term
         ProteinDetectionProtocolParam.Unit             = Unit
         ProteinDetectionProtocolParam.RowVersion       = DateTime.Now.Date
        }

    let createSample Name SampleParams =
        {
         Sample.ID           = 0
         Sample.Name         = Name
         Sample.RowVersion   = DateTime.Now.Date
         Sample.SampleParams = SampleParams
        }

    let createSampleParam Sample Value Term Unit =
        {
         SampleParam.ID               = 0
         SampleParam.FKParamContainer = Sample
         SampleParam.Value            = Value
         SampleParam.Term             = Term
         SampleParam.Unit             = Unit
         SampleParam.RowVersion       = DateTime.Now.Date
        }

    let createSearchDatabase Name Location SDParams =
        {
         SearchDatabase.ID                   = 0
         SearchDatabase.Name                 = Name
         SearchDatabase.Location             = Location
         SearchDatabase.RowVersion           = DateTime.Now.Date
         SearchDatabase.SearchDatabaseParams = SDParams
        }

    let createSearchDatabaseParam SearchDatabase Value Term Unit =
        {
         SearchDatabaseParam.ID               = 0
         SearchDatabaseParam.FKParamContainer = SearchDatabase
         SearchDatabaseParam.Value            = Value
         SearchDatabaseParam.Term             = Term
         SearchDatabaseParam.Unit             = Unit
         SearchDatabaseParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectraData Name Location SpectraDataParams =
        {
         SpectraData.ID                = 0
         SpectraData.Name              = Name
         SpectraData.Location          = Location
         SpectraData.RowVersion        = DateTime.Now.Date
         SpectraData.SpectraDataParams = SpectraDataParams
        }

    let createSpectraDataParam SpectraData Value Term Unit =
        {
         SpectraDataParam.ID               = 0
         SpectraDataParam.FKParamContainer = SpectraData
         SpectraDataParam.Value            = Value
         SpectraDataParam.Term             = Term
         SpectraDataParam.Unit             = Unit
         SpectraDataParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentification name activityDate spectrumIdentificationList spectrumIdentificationProtocoll spectrumIdentificationParams =
        {
         SpectrumIdentification.ID                              = 0
         SpectrumIdentification.Name                            = name
         SpectrumIdentification.ActivityDate                    = activityDate
         SpectrumIdentification.SpectrumIdentificationList      = spectrumIdentificationList
         SpectrumIdentification.SpectrumIdentificationProtocoll = spectrumIdentificationProtocoll
         SpectrumIdentification.RowVersion                      = DateTime.Now.Date
         SpectrumIdentification.SpectrumIdentificationParams    = spectrumIdentificationParams
        }

    let createSpectrumIdentificationParam spectrumIdentification value term unit =
        {
         SpectrumIdentificationParam.ID               = 0
         SpectrumIdentificationParam.FKParamContainer = spectrumIdentification
         SpectrumIdentificationParam.Value            = value
         SpectrumIdentificationParam.Term             = term
         SpectrumIdentificationParam.Unit             = unit
         SpectrumIdentificationParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationItem name peptide chargeState sample passThreshold fragmentation rank massTable calculatedIP calculatedMassToCharge experimentalMassToCharge spectrumIdentificationResult spectrumIdentificationItemParams =
        {
         SpectrumIdentificationItem.ID                               = 0
         SpectrumIdentificationItem.Name                             = name
         SpectrumIdentificationItem.Peptide                          = peptide
         SpectrumIdentificationItem.ChargeState                      = chargeState
         SpectrumIdentificationItem.Sample                           = sample
         SpectrumIdentificationItem.PassThreshold                    = passThreshold
         SpectrumIdentificationItem.Fragmentation                    = fragmentation
         SpectrumIdentificationItem.Rank                             = rank
         SpectrumIdentificationItem.MassTable                        = massTable
         SpectrumIdentificationItem.CalculatedIP                     = calculatedIP
         SpectrumIdentificationItem.CalculatedMassToCharge           = calculatedMassToCharge
         SpectrumIdentificationItem.ExperimentalMassToCharge         = experimentalMassToCharge
         SpectrumIdentificationItem.SpectrumIdentificationResult     = spectrumIdentificationResult
         SpectrumIdentificationItem.RowVersion                       = DateTime.Now.Date
         SpectrumIdentificationItem.SpectrumIdentificationItemParams = spectrumIdentificationItemParams
        }

    let createSpectrumIdentificationItemParam SpectrumIdentificationItem Value Term Unit =
        {
         SpectrumIdentificationItemParam.ID               = 0
         SpectrumIdentificationItemParam.FKParamContainer = SpectrumIdentificationItem
         SpectrumIdentificationItemParam.Value            = Value
         SpectrumIdentificationItemParam.Term             = Term
         SpectrumIdentificationItemParam.Unit             = Unit
         SpectrumIdentificationItemParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationList Name NumSequencesSearched SpectrumIdentificationListParams =
        {
         SpectrumIdentificationList.ID                               = 0
         SpectrumIdentificationList.Name                             = Name
         SpectrumIdentificationList.NumSequencesSearched             = NumSequencesSearched
         SpectrumIdentificationList.RowVersion                       = DateTime.Now.Date
         SpectrumIdentificationList.SpectrumIdentificationListParams = SpectrumIdentificationListParams
        }

    let createSpectrumIdentificationListParam SpectrumIdentificationList Value Term Unit =
        {
         SpectrumIdentificationListParam.ID               = 0
         SpectrumIdentificationListParam.FKParamContainer = SpectrumIdentificationList
         SpectrumIdentificationListParam.Value            = Value
         SpectrumIdentificationListParam.Term             = Term
         SpectrumIdentificationListParam.Unit             = Unit
         SpectrumIdentificationListParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationProtocol Name AnalysisSoftware SpectrumIdentificationProtocolParams =
        {
         SpectrumIdentificationProtocol.ID                                   = 0
         SpectrumIdentificationProtocol.Name                                 = Name
         SpectrumIdentificationProtocol.AnalysisSoftware                     = AnalysisSoftware
         SpectrumIdentificationProtocol.RowVersion                           = DateTime.Now.Date
         SpectrumIdentificationProtocol.SpectrumIdentificationProtocolParams = SpectrumIdentificationProtocolParams
        }

    let createSpectrumIdentificationProtocolParam SpectrumIdentificationProtocol Value Term Unit =
        {
         SpectrumIdentificationProtocolParam.ID               = 0
         SpectrumIdentificationProtocolParam.FKParamContainer = SpectrumIdentificationProtocol
         SpectrumIdentificationProtocolParam.Value            = Value
         SpectrumIdentificationProtocolParam.Term             = Term
         SpectrumIdentificationProtocolParam.Unit             = Unit
         SpectrumIdentificationProtocolParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationResult Name SpectrumID SpectraData SpectrumIdentificationList SpectrumIdentificationResultParams =
        {
         SpectrumIdentificationResult.ID                                 = 0
         SpectrumIdentificationResult.Name                               = Name
         SpectrumIdentificationResult.SpectrumID                         = SpectrumID
         SpectrumIdentificationResult.SpectraData                        = SpectraData
         SpectrumIdentificationResult.SpectrumIdentificationList         = SpectrumIdentificationList
         SpectrumIdentificationResult.RowVersion                         = DateTime.Now.Date
         SpectrumIdentificationResult.SpectrumIdentificationResultParams = SpectrumIdentificationResultParams
        }

    let createSpectrumIdentificationResultParam SpectrumIdentificationResult Value Term Unit =
        {
         SpectrumIdentificationResultParam.ID               = 0
         SpectrumIdentificationResultParam.FKParamContainer = SpectrumIdentificationResult
         SpectrumIdentificationResultParam.Value            = Value
         SpectrumIdentificationResultParam.Term             = Term
         SpectrumIdentificationResultParam.Unit             = Unit
         SpectrumIdentificationResultParam.RowVersion       = DateTime.Now.Date
        }

    let createTerm Ontology (OboTerm : seq<Obo.OboTerm>) =
        new System
            .Collections
            .Generic
            .List<Term>(
            OboTerm
                |> Seq.map (fun item ->
                                        {
                                         Term.ID         = item.Id
                                         Term.Name       = item.Name
                                         Term.Ontology   = Ontology
                                         Term.RowVersion = DateTime.Now.Date
                                        }
                           )   
                       )

    let createTermRelationship Term RelationType RelatedTerm=
        {
         TermRelationShip.ID               = 0
         TermRelationShip.Term             = Term
         TermRelationShip.RelationShipType = RelationType
         TermRelationShip.RelatedTerm      = RelatedTerm
         TermRelationShip.RowVersion       = DateTime.Now.Date
        }

    let createTermTag Name Value Term =
        {
         TermTag.ID         = 0
         TermTag.Name       = Name
         TermTag.Value      = Value
         TermTag.Term       = Term
         TermTag.RowVersion = DateTime.Now.Date
        }

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

///Applying functions/////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////

open InsertStatements 

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
    
    db.Database.EnsureCreated()                                     |> ignore

    db.Ontology.Add(ontologyCustom)                                 |> ignore
    db.Ontology.Add(ontologyPsiMS)                                  |> ignore
    db.Ontology.Add(ontologyPride)                                  |> ignore
    db.Ontology.Add(ontologyUniMod)                                 |> ignore
    db.Ontology.Add(ontologyUnit_Ontology)                          |> ignore
    db.SpectrumIdentification.Add(spectrumIdentification)           |> ignore
    db.SpectrumIdentificationParam.Add(spectrumIdentificationParam) |> ignore
    db.SaveChanges()

///Define Queries for DB//////////////////////////////
/////////////////////////////////////////////////////////////////////////
    
let context = new DBMSContext()

//let testOntology =
//    query {
//        for i in context.Ontology do
//        select i
//          }

////let testTerm =
////    query {
////        for i in context.Term do
////        select i
////          }

let readLine (input : seq<'a>) =
    for i in input do
        Console.WriteLine(i)

//readLine testOntology
//readLine testTerm

//let testTerm2 =
//    (query {
//        for i in context.Term do
//        if i.ID = "" then select i}).Single()
//testTerm2.Name <- "Some BoB"

//let testTerm3 =
//    (query {
//            for i in context.Term do
//            if i.ID = "" 
//                then select i.Ontology
//           }
//    )
//readLine testTerm3

//let testTerm4 =
//    (query {
//        for i in context.Term do
//            if i.Name = "" && i.ID = "" 
//                then select i
//           }
//    )
//readLine testTerm4

let selectSpectrumIdentificationByID ID =
    (query {
            for i in context.SpectrumIdentification do
                if i.ID = ID 
                then select i
           }
    )

let selectSpectrumIdentificationByName Name =
    (query {
            for i in context.SpectrumIdentification do
                if i.Name = Name 
                    then select i
           }
    )

let selectSpectrumIdentificationByActivityDate ActivityDate =
    (query {
            for i in context.SpectrumIdentification do
                if i.ActivityDate = ActivityDate 
                    then select i
           }
    )

let selectSpectrumIdentificationBySIList SIList =
    (query {
            for i in context.SpectrumIdentification do
                if i.SpectrumIdentificationList = SIList 
                    then select i
           }
    )

let selectSpectrumIdentificationBySIProtocoll SIProtocoll =
    (query {
            for i in context.SpectrumIdentification do
                if i.SpectrumIdentificationProtocoll = SIProtocoll 
                    then select i
           }
    )

let selectSpectrumIdentificationParamBySI SI =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer = SI 
                    then select (i, i.Term, i.Unit)
           }
    )

let selectSpectrumIdentificationParamBySIID SIID =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer.ID = SIID
                    then select (i, i.Term, i.Unit)
           }
    )

let selectSpectrumIdentificationParamBySIName SIName =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer.Name = SIName
                    then select (i, i.Term, i.Unit)
           }
    )

let selectTermByOntologyID OntologyID =
    (query {
            for i in context.Term do
                if i.Ontology.ID = OntologyID
                    then select (i)
           }
    )

let selectTermByOntologyName OntologyName =
    (query {
            for i in context.Term do
                if i.Ontology.Name = OntologyName
                    then select (i)
           }
    )

let selectTermByTermID TermID =
    (query {
            for i in context.Term do
                if i.ID = TermID
                    then select (i)
           }
    )

let selectTermByTermName TermName =
    (query {
            for i in context.Term do
                if i.Name = TermName
                    then select (i)
           }
    )

context.SaveChanges()

///Apply queries to DB////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

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
 