
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
open System.Data


//open System.Reflection



///Defining types and relations for DB/////////////////////////////
///////////////////////////////////////////////////////////////////////////////////////////////
//type [<CLIMutable>] 
//     MzIdentML =
//     {
//      [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//      ID              : int
//      Name            : string
//      Version         : string
//      CreationDate    : DateTime
//      RowVersion      : DateTime
//      MzIdentMLParams : List<MzIdentMLParam>
//     }

//and [<CLIMutable>] 
//    MzIdentMLParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                     : int
//    FKParamContainer       : MzIdentMLParam
//    [<RequiredAttribute()>]
//    Term                   : Term
//    Unit                   : Term
//    Value                  : string
//    CVs                    : List<CV>
//    AnalysisSoftware       : List<AnalysisSoftware>
//    Providers              : List<Provider>
//    AuditCollections       : List<AuditCollection>
//    AnalysisSamples        : List<AnalysisSample>
//    Sequences              : List<Sequences>
//    AnalysisCollection     : AnalysisCollection
//    AnalysisProtocolls     : List<AnalysisProtocoll>
//    Datas                  : List<Datas>
//    BiblioGraphicReference : BiblioGraphicReference
//    RowVersion             : DateTime  
//    }
///Don`t see a need for it
//and [<CLIMutable>]
//    Affiliation =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID           : int
//     Organization : Organization
//     RowVersion   : DateTime
//    }

///Add Suplements to each Table

//and [<CLIMutable>]
//    AmbitousResidue =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID   : int
//     code : char
//    }

type [<CLIMutable>]
    AnalysisCollection =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID : int
     SpectrumIdentification : SpectrumIdentification
     ProteinDetection       : ProteinDetection
     RowVersion             : DateTime  
    }

and [<CLIMutable>]
    AnalysisData =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID : int
     Spectrumidentifications : List<SpectrumIdentification>
     Proteindetections       : List<ProteinDetection>
     RowVersion              : DateTime
    }

and [<CLIMutable>] 
    AnalysisProtocollCollection =
    {
     ID : int
     SpectrumIdentificationProtocol : SpectrumIdentificationProtocol
     ProteinDetectionProtocol       : ProteinDetectionProtocol
     DataRowVersion                 : DateTime
    }

and [<CLIMutable>] 
    AnalysisSampleCollection =
    {
     ID         : int
     Sample     : Sample
     RowVersion : DateTime
    }

and [<CLIMutable>] 
    AnalysisSoftware =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    Name                   : string
    //ContactRole            : ContactRole
    Customizations         : List<string>
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
    AnalysisSoftwareList =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID               : int
     AnalysisSoftware : List<AnalysisSoftware>
     RowVersion       : DateTime
    }

and [<CLIMutable>]
    AuditCollection =
    {
     ID           : int
     Person       : Person
     Organization : Organization
     RowVersion   : DateTime
    }

and [<CLIMutable>] 
    BiblioGraphicReference =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
     ID          : int
     Name        : string
     Issue       : string
     Authors     : string
     DOI         : string
     Editor      : string
     Pages       : List<int>
     Publication : string
     Publisher   : string
     Title       : string
     Volume      : string
     Year        : int
     RowVersion  : DateTime
    }

//and [<CLIMutable>] 
//    ContactRole =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
//     ID         : int
//     Role       : Role
//     Person     : Person // SelfAdded
//     RowVersion : DateTime
//    }

and [<CLIMutable>] 
    CV =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
     ID         : int
     FullName   : string
     URL        : string
     Version    : string
     RowVersion : DateTime
    }

and [<CLIMutable>]
    CVLsit =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
     ID         : int
     CVs        : List<CV>
     RowVersion : DateTime
    }

//and [<CLIMutable>] 
//    DataBaseFilters =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID             : int
//     Filters        : List<Filter>
//     RowVersion     : DateTime
//    }

and [<CLIMutable>]
    DatabaseName =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID         : int
     Name       : string
     RowVersion : DateTime
     DatabseNameParams : List<DatabaseNameParam>
    }

and [<CLIMutable>] 
    DatabaseNameParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : DatabaseName
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] 
    DatabaseTranslation =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID          : int
     Frames      : List<string> ///SelfAdded
     Translation : Translation
     RowVersion  : DateTime
    }

//and [<CLIMutable>]
//    DataCollection =
//    {
//     ID            : int
//     Inputs        : List<Input>
//     AnalysisDatas : List<AnalysisData>
//    }

and [<CLIMutable>] 
    DBSequence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    Accession        : string
    Name             : string
    SearchDB         : SearchDatabase
    //Sequence         : Sequence
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

//and [<CLIMutable>]
//    Enzyme =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID           : int
//     Name         : string
//     SemiSpecific : bool
//     CTermGain    : string
//     NTermGain    : string
//     MinDistance  : int
//     CleavageSite : string
//     RowVersion   : DateTime
//     EnzymeParams : List<EnzymeParam>
//    }

//and [<CLIMutable>] 
//    EnzymeParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : EnzymeParam
//    [<RequiredAttribute()>]
//    Term             : Term
//    Unit             : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//and [<CLIMutable>]
//    Enzymes =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID         : int
//     Enzymes    : List<Enzymes>
//     RowVersion : DateTime
//    }

//and [<CLIMutable>]
//    Exclude =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID            : int
//     Term          : Term
//     RowVerion     : DateTime
//     ExcludeParams : List<ExcludeParam>
//    }

//and [<CLIMutable>] 
//    ExcludeParams =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : Exclude
//    [<RequiredAttribute()>]
//    Term             : Term
//    Unit             : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

and [<CLIMutable>]
    FileFormat =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID               : int
     Format           : string
     RowVersion       : DateTime
     FileFormatParams : List<FileFormatParam>
    }

and [<CLIMutable>] 
    FileFormatParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : FileFormat
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

//and [<CLIMutable>]
//    Filter =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID         : int
//     FilterType : FilterType
//     Exclude    : Exclude
//     Include    : Include
//     RowVersion : DateTime
//    }

and [<CLIMutable>]
    FilterType =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID               : int
     Name             : string
     RowVersion       : DateTime
     FilterTypeParams : List<FilterTypeParam>
    }

and [<CLIMutable>] 
    FilterTypeParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : FilterTypeParam
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

//and [<CLIMutable>] 
//    FragmentArray =
//    {
//     ID                 : int
//     Values             : List<Floats>
//     FragmentationTable : Fragmentation
//     RowVersion         : DateTime
//    }

//and [<CLIMutable>]
//    Fragmentation =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID         : int
//     IonType    : IonType
//     RowVersion : DateTime
//    }

//and [<CLIMutable>]
//    FragmentationTable =
//    {
//     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//     ID         : int
//     Measure    : Measure
//     RowVersion : DateTime
//    }

and [<CLIMutable>]
    FragmentTolerance =
    {
     [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
     ID     : int
     Value : float
     RowVersion : DateTime
     FragmentToleranceParams : List<FragmentToleranceParam>
    }

and [<CLIMutable>] 
    FragmentToleranceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : FragmentTolerance
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>]
    Include =
    {
     ID : int
     Term : Term
     RowVersion : DateTime
     IncludeParams : List<IncludeParam>
    }

and [<CLIMutable>] 
    IncludeParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : Include
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

//and [<CLIMutable>]
//    Input =
//    {
//     ID             : int
//     URL            : string ///Added by self
//     SourceFile     : SourceFile
//     SearchDatabase : SearchDatabase
//     SpectraData    : SpectraData
//     RowVersion     : DateTime
//    }

///InputSpectra???
///InputSpectrumIdentifications????

//and [<CLIMutable>]
//    IonType =
//    {
//     ID            : int
//     Type          : string ///Added by self
//     Charge        : int
//     Index         : List<int>
//     FragmentArray : FragmentArray
//     RowVersion    : DateTime
//     IonTypeParams : List<IonTypeParam>
//    }

//and [<CLIMutable>] 
//    IonTypeParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID               : int
//    FKParamContainer : IonType
//    [<RequiredAttribute()>]
//    Term             : Term
//    Unit             : Term
//    Value            : string
//    RowVersion       : DateTime 
//    }

//Added because it is part of the original document and refferd to in the code
and [<CLIMutable>] 
    MassTable =
    {
    ID              : int
    Name            : string
    MSLevel         : List<int> //MS spectrum that the MassTable reffers to, e.g. "1" for MS1
    RowVersion      : DateTime
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

///Add other Tables after MassTable

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

    let createAnalysisSoftware name analysisSoftwareParams =
        {
         AnalysisSoftware.ID                     = 0
         AnalysisSoftware.Name                   = name
         AnalysisSoftware.RowVersion             = DateTime.Now.Date
         AnalysisSoftware.AnalysisSoftwareParams = analysisSoftwareParams
        }

    let createAnalysisSoftwareParam analysisSoftware value term unit =
        {
         AnalysisSoftwareParam.ID               = 0
         AnalysisSoftwareParam.FKParamContainer = analysisSoftware
         AnalysisSoftwareParam.Value            = value
         AnalysisSoftwareParam.Term             = term
         AnalysisSoftwareParam.Unit             = unit
         AnalysisSoftwareParam.RowVersion       = DateTime.Now.Date
        }

    let createDBSequence name accession searchDB dBSequenceParams =
        {
         DBSequence.ID               = 0
         DBSequence.Name             = name
         DBSequence.Accession        = accession
         DBSequence.SearchDB         = searchDB
         DBSequence.RowVersion       = DateTime.Now.Date
         DBSequence.DBSequenceParams = dBSequenceParams
        }

    let createDBSequenceParam dBSequence value term unit =
        {
         DBSequenceParam.ID               = 0
         DBSequenceParam.FKParamContainer = dBSequence
         DBSequenceParam.Value            = value
         DBSequenceParam.Term             = term
         DBSequenceParam.Unit             = unit
         DBSequenceParam.RowVersion       = DateTime.Now.Date
        }

    let createMassTable name mSLevel massTableParams =
        {
         MassTable.ID              = 0
         MassTable.Name            = name
         MassTable.MSLevel         = mSLevel
         MassTable.RowVersion      = DateTime.Now.Date
         MassTable.MassTableParams = massTableParams
        }

    let createMassTableParam massTable value term unit =
        {
         MassTableParam.ID               = 0
         MassTableParam.FKParamContainer = massTable
         MassTableParam.Value            = value
         MassTableParam.Term             = term
         MassTableParam.Unit             = unit
         MassTableParam.RowVersion       = DateTime.Now.Date
        }

    let createModification name avgMassDelta monoisotopicMassDelta residues modLocation modificationParams =
        {
         Modification.ID                    = 0
         Modification.Name                  = name
         Modification.AvgMassDelta          = avgMassDelta
         Modification.MonoisotopicMassDelta = monoisotopicMassDelta
         Modification.Residues              = residues
         Modification.ModLocation           = modLocation
         Modification.RowVersion            = DateTime.Now.Date
         Modification.ModificationParams    = modificationParams
        }

    let createModificationParam modification value term unit =
        {
         ModificationParam.ID               = 0
         ModificationParam.FKParamContainer = modification
         ModificationParam.Value            = value
         ModificationParam.Term             = term
         ModificationParam.Unit             = unit
         ModificationParam.RowVersion       = DateTime.Now.Date
        }

    let createModLocation modification location peptide residue modlocationParams =
        {
         ModLocation.ID                = 0
         ModLocation.Modification      = modification
         ModLocation.Location          = location
         ModLocation.Peptide           = peptide
         ModLocation.Residue           = residue
         ModLocation.RowVersion        = DateTime.Now.Date
         ModLocation.ModLocationParams = modlocationParams
        }

    let createModLocationParam modLocation value term unit =
        {
         ModLocationParam.ID               = 0
         ModLocationParam.FKParamContainer = modLocation
         ModLocationParam.Value            = value
         ModLocationParam.Term             = term
         ModLocationParam.Unit             = unit
         ModLocationParam.RowVersion       = DateTime.Now.Date
        }

    let createOntology name params terms =
        {
         Ontology.ID             = 0
         Ontology.Name           = name
         Ontology.RowVersion     = DateTime.Now.Date
         Ontology.OntologyParams = params
         Ontology.Terms          = terms
        }

    let createOntologyParam ontology value term unit =
        {
         OntologyParam.ID               = 0
         OntologyParam.FKParamContainer = ontology
         OntologyParam.Value            = value
         OntologyParam.Term             = term
         OntologyParam.Unit             = unit
         OntologyParam.RowVersion       = DateTime.Now.Date
        }

    let createOrganization name parent organizationParams =
        {
         Organization.ID                 = 0
         Organization.Name               = name
         Organization.Parent             = parent
         Organization.RowVersion         = DateTime.Now.Date
         Organization.OrganizationParams = organizationParams
        }

    let createOrganizationParam organization value term unit =
        {
         OrganizationParam.ID               = 0
         OrganizationParam.FKParamContainer = organization
         OrganizationParam.Value            = value
         OrganizationParam.Term             = term
         OrganizationParam.Unit             = unit
         OrganizationParam.RowVersion       = DateTime.Now.Date
        }

    let createParent name country organization parentParams =
        {
         Parent.ID           = 0
         Parent.Name         = name
         Parent.Country      = country
         Parent.Organization = organization
         Parent.RowVersion   = DateTime.Now.Date
         Parent.ParentParams = parentParams
        }

    let createParentParam parent value term unit =
        {
         ParentParam.ID               = 0
         ParentParam.FKParamContainer = parent
         ParentParam.Value            = value
         ParentParam.Term             = term
         ParentParam.Unit             = unit
         ParentParam.RowVersion       = DateTime.Now.Date
        }

    let createPeptide name sequence peptideParams =
        {
         Peptide.ID            = 0
         Peptide.Name          = name
         Peptide.Sequence      = sequence
         Peptide.RowVersion    = DateTime.Now.Date
         Peptide.PeptideParams = peptideParams
        }

    let createPeptideParam peptide value term unit =
        {
         PeptideParam.ID               = 0
         PeptideParam.FKParamContainer = peptide
         PeptideParam.Value            = value
         PeptideParam.Term             = term
         PeptideParam.Unit             = unit
         PeptideParam.RowVersion       = DateTime.Now.Date
        }

    let createPeptideEvidence peptide dBSequence isDecoy frame translation start ende pre post peptideEvidenceParams =
        {
         PeptideEvidence.ID                    = 0
         PeptideEvidence.Peptide               = peptide
         PeptideEvidence.DBSequence            = dBSequence
         PeptideEvidence.isDecoy               = isDecoy
         PeptideEvidence.Frame                 = frame
         PeptideEvidence.Translation           = translation
         PeptideEvidence.Start                 = start
         PeptideEvidence.End                   = ende
         PeptideEvidence.Pre                   = pre
         PeptideEvidence.Post                  = post
         PeptideEvidence.RowVersion            = DateTime.Now.Date
         PeptideEvidence.PeptideEvidenceParams = peptideEvidenceParams
        }

    let createPeptideEvidenceParam peptideEvidence value term unit =
        {
         PeptideEvidenceParam.ID               = 0
         PeptideEvidenceParam.FKParamContainer = peptideEvidence
         PeptideEvidenceParam.Value            = value
         PeptideEvidenceParam.Term             = term
         PeptideEvidenceParam.Unit             = unit
         PeptideEvidenceParam.RowVersion       = DateTime.Now.Date
        }

    let createPeptideHypothesis peptideDetectionHypothesisID peptideEvidence peptideHypothesisParams =
        {
         PeptideHypothesis.ID                           = 0
         PeptideHypothesis.PeptideDetectionHypothesisID = peptideDetectionHypothesisID
         PeptideHypothesis.PeptideEvidence              = peptideEvidence
         PeptideHypothesis.RowVersion                   = DateTime.Now.Date
         PeptideHypothesis.PeptideHypothesisParams      = peptideHypothesisParams
        }

    let createPeptideHypothesisParam peptideHypothesis value term unit =
        {
         PeptideHypothesisParam.ID               = 0
         PeptideHypothesisParam.FKParamContainer = peptideHypothesis
         PeptideHypothesisParam.Value            = value
         PeptideHypothesisParam.Term             = term
         PeptideHypothesisParam.Unit             = unit
         PeptideHypothesisParam.RowVersion       = DateTime.Now.Date
        }

    let createPerson firstName middleName lastName organization personenParams =
        {
         Person.ID           = 0
         Person.FirstName    = firstName
         Person.MiddleName   = middleName
         Person.LastName     = lastName
         Person.Organization = organization
         Person.RowVersion   = DateTime.Now.Date
         Person.PersonParams = personenParams
        }

    let createPersonParam person value term unit =
        {
         PersonParam.ID               = 0
         PersonParam.FKParamContainer = person
         PersonParam.Value            = value
         PersonParam.Term             = term
         PersonParam.Unit             = unit
         PersonParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinAmbiguityGroup name proteinDetectionList proteinAmbiguityGroupParams =
        {
         ProteinAmbiguityGroup.ID                          = 0
         ProteinAmbiguityGroup.Name                        = name
         ProteinAmbiguityGroup.ProteinDetectionList        = proteinDetectionList
         ProteinAmbiguityGroup.RowVersion                  = DateTime.Now.Date
         ProteinAmbiguityGroup.ProteinAmbiguityGroupParams = proteinAmbiguityGroupParams
        }

    let createProteinAmbiguityGroupParam proteinAmbiguityGroup value term unit =
        {
         ProteinAmbiguityGroupParam.ID               = 0
         ProteinAmbiguityGroupParam.FKParamContainer = proteinAmbiguityGroup
         ProteinAmbiguityGroupParam.Value            = value
         ProteinAmbiguityGroupParam.Term             = term
         ProteinAmbiguityGroupParam.Unit             = unit
         ProteinAmbiguityGroupParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetection name proteinDetectionProtocoll proteinDetectionList proteinDetectionParams =
        {
         ProteinDetection.ID                        = 0
         ProteinDetection.Name                      = name
         ProteinDetection.ProteinDetectionProtocoll = proteinDetectionProtocoll
         ProteinDetection.ProteinDetectionList      = proteinDetectionList
         ProteinDetection.RowVersion                = DateTime.Now.Date
         ProteinDetection.ProteinDetectionParams    = proteinDetectionParams
        }

    let createProteinDetectionParam proteinDetection value term unit =
        {
         ProteinDetectionParam.ID               = 0
         ProteinDetectionParam.FKParamContainer = proteinDetection
         ProteinDetectionParam.Value            = value
         ProteinDetectionParam.Term             = term
         ProteinDetectionParam.Unit             = unit
         ProteinDetectionParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetectionHypothesis name dBSequence passThreshold proteinAmbiguityGroup proteinDetectionHypothesisParams =
        {
         ProteinDetectionHypothesis.ID                               = 0
         ProteinDetectionHypothesis.Name                             = name
         ProteinDetectionHypothesis.DBSequence                       = dBSequence
         ProteinDetectionHypothesis.PassThreshold                    = passThreshold
         ProteinDetectionHypothesis.ProteinAmbiguityGroup            = proteinAmbiguityGroup
         ProteinDetectionHypothesis.RowVersion                       = DateTime.Now.Date
         ProteinDetectionHypothesis.ProteinDetectionHypothesisParams = proteinDetectionHypothesisParams
        }

    let createProteinDetectionHypothesisParam proteinDetectionHypothesis value term unit =
        {
         ProteinDetectionHypothesisParam.ID               = 0
         ProteinDetectionHypothesisParam.FKParamContainer = proteinDetectionHypothesis
         ProteinDetectionHypothesisParam.Value            = value
         ProteinDetectionHypothesisParam.Term             = term
         ProteinDetectionHypothesisParam.Unit             = unit
         ProteinDetectionHypothesisParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetectionList name searchDB accession proteinDetectionListParams =
        {
         ProteinDetectionList.ID                         = 0
         ProteinDetectionList.Name                       = name
         ProteinDetectionList.SearchDB                   = searchDB
         ProteinDetectionList.Accession                  = accession
         ProteinDetectionList.RowVersion                 = DateTime.Now.Date
         ProteinDetectionList.ProteinDetectionListParams = proteinDetectionListParams
        }

    let createProteinDetectionListParam proteinDetectionList value term unit =
        {
         ProteinDetectionListParam.ID               = 0
         ProteinDetectionListParam.FKParamContainer = proteinDetectionList
         ProteinDetectionListParam.Value            = value
         ProteinDetectionListParam.Term             = term
         ProteinDetectionListParam.Unit             = unit
         ProteinDetectionListParam.RowVersion       = DateTime.Now.Date
        }

    let createProteinDetectionProtocol name analysisSoftware proteinDetectionProtocolParams =
        {
         ProteinDetectionProtocol.ID                             = 0
         ProteinDetectionProtocol.Name                           = name
         ProteinDetectionProtocol.AnalysisSoftware               = analysisSoftware
         ProteinDetectionProtocol.RowVersion                     = DateTime.Now.Date
         ProteinDetectionProtocol.ProteinDetectionProtocolParams = proteinDetectionProtocolParams
        }

    let createProteinDetectionProtocolParam proteinDetectionProtocol value term unit =
        {
         ProteinDetectionProtocolParam.ID               = 0
         ProteinDetectionProtocolParam.FKParamContainer = proteinDetectionProtocol
         ProteinDetectionProtocolParam.Value            = value
         ProteinDetectionProtocolParam.Term             = term
         ProteinDetectionProtocolParam.Unit             = unit
         ProteinDetectionProtocolParam.RowVersion       = DateTime.Now.Date
        }

    let createSample name sampleParams =
        {
         Sample.ID           = 0
         Sample.Name         = name
         Sample.RowVersion   = DateTime.Now.Date
         Sample.SampleParams = sampleParams
        }

    let createSampleParam sample value term unit =
        {
         SampleParam.ID               = 0
         SampleParam.FKParamContainer = sample
         SampleParam.Value            = value
         SampleParam.Term             = term
         SampleParam.Unit             = unit
         SampleParam.RowVersion       = DateTime.Now.Date
        }

    let createSearchDatabase name location searchDatabaseParams =
        {
         SearchDatabase.ID                   = 0
         SearchDatabase.Name                 = name
         SearchDatabase.Location             = location
         SearchDatabase.RowVersion           = DateTime.Now.Date
         SearchDatabase.SearchDatabaseParams = searchDatabaseParams
        }

    let createSearchDatabaseParam searchDatabase value term unit =
        {
         SearchDatabaseParam.ID               = 0
         SearchDatabaseParam.FKParamContainer = searchDatabase
         SearchDatabaseParam.Value            = value
         SearchDatabaseParam.Term             = term
         SearchDatabaseParam.Unit             = unit
         SearchDatabaseParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectraData name location spectraDataParams =
        {
         SpectraData.ID                = 0
         SpectraData.Name              = name
         SpectraData.Location          = location
         SpectraData.RowVersion        = DateTime.Now.Date
         SpectraData.SpectraDataParams = spectraDataParams
        }

    let createSpectraDataParam spectraData value term unit =
        {
         SpectraDataParam.ID               = 0
         SpectraDataParam.FKParamContainer = spectraData
         SpectraDataParam.Value            = value
         SpectraDataParam.Term             = term
         SpectraDataParam.Unit             = unit
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

    let createSpectrumIdentificationItemParam spectrumIdentificationItem value term unit =
        {
         SpectrumIdentificationItemParam.ID               = 0
         SpectrumIdentificationItemParam.FKParamContainer = spectrumIdentificationItem
         SpectrumIdentificationItemParam.Value            = value
         SpectrumIdentificationItemParam.Term             = term
         SpectrumIdentificationItemParam.Unit             = unit
         SpectrumIdentificationItemParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationList name numSequencesSearched spectrumIdentificationListParams =
        {
         SpectrumIdentificationList.ID                               = 0
         SpectrumIdentificationList.Name                             = name
         SpectrumIdentificationList.NumSequencesSearched             = numSequencesSearched
         SpectrumIdentificationList.RowVersion                       = DateTime.Now.Date
         SpectrumIdentificationList.SpectrumIdentificationListParams = spectrumIdentificationListParams
        }

    let createSpectrumIdentificationListParam spectrumIdentificationList value term unit =
        {
         SpectrumIdentificationListParam.ID               = 0
         SpectrumIdentificationListParam.FKParamContainer = spectrumIdentificationList
         SpectrumIdentificationListParam.Value            = value
         SpectrumIdentificationListParam.Term             = term
         SpectrumIdentificationListParam.Unit             = unit
         SpectrumIdentificationListParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationProtocol name analysisSoftware spectrumIdentificationProtocolParams =
        {
         SpectrumIdentificationProtocol.ID                                   = 0
         SpectrumIdentificationProtocol.Name                                 = name
         SpectrumIdentificationProtocol.AnalysisSoftware                     = analysisSoftware
         SpectrumIdentificationProtocol.RowVersion                           = DateTime.Now.Date
         SpectrumIdentificationProtocol.SpectrumIdentificationProtocolParams = spectrumIdentificationProtocolParams
        }

    let createSpectrumIdentificationProtocolParam spectrumIdentificationProtocol value term unit =
        {
         SpectrumIdentificationProtocolParam.ID               = 0
         SpectrumIdentificationProtocolParam.FKParamContainer = spectrumIdentificationProtocol
         SpectrumIdentificationProtocolParam.Value            = value
         SpectrumIdentificationProtocolParam.Term             = term
         SpectrumIdentificationProtocolParam.Unit             = unit
         SpectrumIdentificationProtocolParam.RowVersion       = DateTime.Now.Date
        }

    let createSpectrumIdentificationResult name spectrumID spectraData spectrumIdentificationList spectrumIdentificationResultParams =
        {
         SpectrumIdentificationResult.ID                                 = 0
         SpectrumIdentificationResult.Name                               = name
         SpectrumIdentificationResult.SpectrumID                         = spectrumID
         SpectrumIdentificationResult.SpectraData                        = spectraData
         SpectrumIdentificationResult.SpectrumIdentificationList         = spectrumIdentificationList
         SpectrumIdentificationResult.RowVersion                         = DateTime.Now.Date
         SpectrumIdentificationResult.SpectrumIdentificationResultParams = spectrumIdentificationResultParams
        }

    let createSpectrumIdentificationResultParam spectrumIdentificationResult value term unit =
        {
         SpectrumIdentificationResultParam.ID               = 0
         SpectrumIdentificationResultParam.FKParamContainer = spectrumIdentificationResult
         SpectrumIdentificationResultParam.Value            = value
         SpectrumIdentificationResultParam.Term             = term
         SpectrumIdentificationResultParam.Unit             = unit
         SpectrumIdentificationResultParam.RowVersion       = DateTime.Now.Date
        }

    let createTerm ontology (oboTerm : seq<Obo.OboTerm>) =
        new System
            .Collections
            .Generic
            .List<Term>(
            oboTerm
                |> Seq.map (fun item ->
                                        {
                                         Term.ID         = item.Id
                                         Term.Name       = item.Name
                                         Term.Ontology   = ontology
                                         Term.RowVersion = DateTime.Now.Date
                                        }
                           )   
                       )
    let createTermCustom id name ontology =
        {
         Term.ID = id
         Term.Name = name
         Term.Ontology = ontology
         Term.RowVersion = DateTime.Now.Date
        }

    let createTermRelationship term relationshipType relatedTerm =
        {
         TermRelationShip.ID               = 0
         TermRelationShip.Term             = term
         TermRelationShip.RelationShipType = relationshipType
         TermRelationShip.RelatedTerm      = relatedTerm
         TermRelationShip.RowVersion       = DateTime.Now.Date
        }

    let createTermTag name value term =
        {
         TermTag.ID         = 0
         TermTag.Name       = name
         TermTag.Value      = value
         TermTag.Term       = term
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
     createOntology "Custom" null (new System.Collections.Generic.List<Term>([createTermCustom "000000" "Unitless" (createOntology "Custom" null null)]))

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

let selectSpectrumIdentificationByID iD =
    (query {
            for i in context.SpectrumIdentification do
                if i.ID = iD 
                then select i
           }
    )

let selectSpectrumIdentificationByName name =
    (query {
            for i in context.SpectrumIdentification do
                if i.Name = name 
                    then select i
           }
    )

let selectSpectrumIdentificationByActivityDate activityDate =
    (query {
            for i in context.SpectrumIdentification do
                if i.ActivityDate = activityDate 
                    then select i
           }
    )

let selectSpectrumIdentificationBySIList sIList =
    (query {
            for i in context.SpectrumIdentification do
                if i.SpectrumIdentificationList = sIList 
                    then select i
           }
    )

let selectSpectrumIdentificationBySIProtocoll sIProtocoll =
    (query {
            for i in context.SpectrumIdentification do
                if i.SpectrumIdentificationProtocoll = sIProtocoll 
                    then select i
           }
    )

let selectSpectrumIdentificationParamBySI sI =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer = sI 
                    then select (i, i.Term, i.Unit)
           }
    )

let selectSpectrumIdentificationParamBySIID sIID =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer.ID = sIID
                    then select (i, i.Term, i.Unit)
           }
    )

let selectSpectrumIdentificationParamBySIName sIName =
    (query {
            for i in context.SpectrumIdentificationParam do
                if i.FKParamContainer.Name = sIName
                    then select (i, i.Term, i.Unit)
           }
    )

let selectTermByOntologyID ontologyID =
    (query {
            for i in context.Term do
                if i.Ontology.ID = ontologyID
                    then select (i)
           }
    )

let selectTermByOntologyName ontologyName =
    (query {
            for i in context.Term do
                if i.Ontology.Name = ontologyName
                    then select (i)
           }
    )

let selectTermByTermID termID =
    (query {
            for i in context.Term do
                if i.ID = termID
                    then select (i)
           }
    )

let selectTermByTermName termName =
    (query {
            for i in context.Term do
                if i.Name = termName
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
 