﻿
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

///Upper-most hierarchy of MzIdentML with sub-containers to describe relations of data. 
type [<CLIMutable>] 
    AnalysisSoftware =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    Name                   : string 
    RowVersion             : DateTime
    AnalysisSoftwareParams : List<AnalysisSoftwareParam>
    }

///CvParam for AmbitousResidue
and [<CLIMutable>] 
    AmbiguousResidueParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    FKParamContainer : AnalysisSoftware
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
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
    UserParam        : List<UserParam>
    }

///Params of DBSequence.
and [<CLIMutable>] 
    DBSequenceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : DBSequence
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
    MSLevel =
    {
    ID              : int
    MSLevel         : int
    MassTable       : MassTable
    RowVersion      : DateTime
    }

///Params of MassTable.
and [<CLIMutable>] 
    MassTableParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : MassTable
    [<RequiredAttribute()>]
    Term             : Term
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

//Create connection to ModLocation?
and [<CLIMutable>] 
    Measure =
    {
    ID              : int
    Name            : string
    //MassTable       : MassTable
    RowVersion      : DateTime
    MeasureParams   : List<MeasureParam>
    }

///Params of Measure.
and [<CLIMutable>] 
    MeasureParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : Measure
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

///A molecule modification specification. For every modification found in the peptide an extra entry should be created.
and [<CLIMutable>] 
    Modification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                    : int
    Name                  : string 
    Residues              : string 
    MonoisotopicMassDelta : float 
    AvgMassDelta          : float 
    ModLocation           : ModLocation 
    RowVersion            : DateTime 
    ModificationParams    : List<ModificationParam>
    }

///Params of Moodification.
and [<CLIMutable>] 
    ModificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : Modification
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

///The specification of static/variable modifications that are to be considered in the spectra search.
and [<CLIMutable>] 
    ModificationParams =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                    : int
    SearchModification    : SearchModification 
    RowVersion            : DateTime 
    }
//Not part of the original document
///Param that describes the location of the modification and the altered residue.
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

///Params of ModLocation.
and [<CLIMutable>] 
    ModLocationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : ModLocation
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

///Standarized vocabulary for MS-Database.
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

///Params for Ontology.
and [<CLIMutable>] 
    OntologyParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : Ontology
    [<RequiredAttribute()>]
    Term                   : Term
    [<RequiredAttribute()>]
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

///Entity which is responsible for analysis-software, data-file, etc. 
and [<CLIMutable>] 
    Organization =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                 : int
    Name               : string
    Parent             : Parent
    RowVersion         : DateTime
    OrganizationParams : List<OrganizationParam>
    }

///Params of Organization.
and [<CLIMutable>] 
    OrganizationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : Organization
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

///The containing organization.
and [<CLIMutable>] 
    Parent =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int            
    //Name         : string
    Organization : Organization 
    //Country      : string         
    RowVersion   : DateTime
    //ParentParams : List<ParentParam>
    }

and [<CLIMutable>] 
    ParentTolerance =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int            
    Term         : Term         
    RowVersion   : DateTime
    }
///One peptide, which can have modifications. The combination of peptide and modification must be unique.
and [<CLIMutable>] 
    Peptide =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                       : int
    Name                     : string            
    PeptideSequence          : PeptideSequence
    Modification             : Modification
    SubstitutionModification : SubstitutionModification
    RowVersion               : DateTime 
    PeptideParams            : List<PeptideParam>
    UserParams               : List<UserParam>
    }

///Params of Peptide.
and [<CLIMutable>] 
    PeptideParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : Peptide
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

///PeptideEvidence links a specific Peptide element to a specific position in a DBSequence. There MUST only be one PeptideEvidence item per Peptide-to-DBSequence-position.
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
    TranslationTable      : TranslationTable
    RowVersion            : DateTime 
    PeptideEvidenceParams : List<PeptideEvidenceParam>
    UserParams            : List<UserParam>
    }

///Params of PeptideEvidence.
and [<CLIMutable>] 
    PeptideEvidenceParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : PeptideEvidence
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

///Reference to the PeptideEvidence element identified.
and [<CLIMutable>] 
    PeptideEvidenceRef =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                    : int
    PeptideEvidence       : PeptideEvidence
    RowVersion            : DateTime 
    }

///Peptide evidence on which this ProteinHypothesis is based by reference to a PeptideEvidence element.
and [<CLIMutable>] 
    PeptideHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                            : int
    PeptideEvidenceRef            : PeptideEvidenceRef
    SpectrumIdentificationItemRef : SpectrumIdentificationItemRef
    RowVersion                    : DateTime  
    //PeptideHypothesisParams      : List<PeptideHypothesisParam>
    }

and [<CLIMutable>] 
    PeptideSequence =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                           : int
    Sequence                     : string
    RowVersion                   : DateTime  
    //PeptideHypothesisParams      : List<PeptideHypothesisParam>
    }

///person's name and contact details.
and [<CLIMutable>] 
    Person =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID             : int
    FirstName      : string 
    LastName       : string 
    MiddleName     : string
    Affiliation    : Affiliation
    //Organization   : Organization
    RowVersion     : DateTime  
    PersonParams   : List<PersonParam>
    UserParams     : List<UserParam>
    }

///Params of Person.
and [<CLIMutable>] 
    PersonParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : Person
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime   
    }

///A set of logically related results from a protein detection.
and [<CLIMutable>] 
    ProteinAmbiguityGroup =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                          : int
    Name                        : string 
    ProteinDetectionHypothesis  : ProteinDetectionHypothesis
    RowVersion                  : DateTime
    ProteinAmbiguityGroupParams : List<ProteinAmbiguityGroupParam>
    UserParams                  : List<UserParam>
    }

///Params of ProteinAmbiguityGroup.
and [<CLIMutable>] 
    ProteinAmbiguityGroupParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : ProteinAmbiguityGroup
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime     
    }

///An Analysis which assembles a set of peptides to proteins.
and [<CLIMutable>] 
    ProteinDetection =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>]
    ID                          : int
    Name                        : string
    ProteinDetectionList        : ProteinDetectionList
    ProteinDetectionProtocol    : ProteinDetectionProtocol
    InputSpectrumIdentification : InputSpectrumIdentification
    RowVersion                  : DateTime
    //ProteinDetectionParams      : List<ProteinDetectionParam>
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

///A single result of the ProteinDetection analysis.
and [<CLIMutable>] 
    ProteinDetectionHypothesis =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    Name                             : string
    DBSequence                       : DBSequence
    PassThreshold                    : bool
    PeptideHypothesis                : PeptideHypothesis
    RowVersion                       : DateTime  
    ProteinDetectionHypothesisParams : List<ProteinDetectionHypothesisParam>
    UserParams                       : List<UserParam>
    }

///Params of ProteinDetectionHypothesis.
and [<CLIMutable>] 
    ProteinDetectionHypothesisParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : ProteinDetectionHypothesis
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime
    }

///The protein list resulting from a protein detection process.
and [<CLIMutable>] 
    ProteinDetectionList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                         : int
    Name                       : string
    ProteinAmbiguityGroup      : ProteinAmbiguityGroup
    RowVersion                 : DateTime
    ProteinDetectionListParams : List<ProteinDetectionListParam>
    UserParams                 : List<UserParam>
    }

///Params of ProteindetectionList.
and [<CLIMutable>] 
    ProteinDetectionListParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : ProteinDetectionList
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime
    }

///The parameters and settings of a ProteinDetection process.
and [<CLIMutable>] 
    ProteinDetectionProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                             : int
    Name                           : string 
    AnalysisSoftware               : AnalysisSoftware
    AnalysisParams                 : AnalysisParams
    Threshold                      : Threshold
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
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

///A description of the sample analysed by mass spectrometry using CVParams or UserParams.
and [<CLIMutable>] 
    Sample =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID           : int
    Name         : string 
    ContactRole  : ContactRole
    SubSample    : SubSample
    RowVersion   : DateTime
    SampleParams : List<SampleParam>
    UserParams   : List<UserParam>
    }

///Params of Sample.
and [<CLIMutable>] 
    SampleParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : Sample
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

//Added because it exists in the original document and is referred multiple times in the code.
and [<CLIMutable>] 
    SearchDatabase =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                          : int
    Name                        : string 
    Location                    : string //Location of the datafile, commonly an URL
    NumDatabaseSequences        : int
    NumResidues                 : int
    ReleaseDate                 : DateTime
    Version                     : string
    ExternalFormatDocumentation : ExternalFormatDocumentation
    FileFormat                  : FileFormat
    DatabaseName                : DatabaseName
    RowVersion                  : DateTime
    SearchDatabaseParams        : List<SearchDatabaseParam>
    }

///Params of SearchDatabase.
and [<CLIMutable>] 
    SearchDatabaseParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : SearchDatabaseParam
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime  
    }

///One of the search databases used.
and [<CLIMutable>] 
    SearchDatabaseRef =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID             : int
    SearchDatabase : SearchDatabase
    RowVersion     : DateTime
    }

///Specification of a search modification as parameter for a spectra search. Contains the name of the modification, the mass, the specificity and whether it is a static modification.
and [<CLIMutable>] 
    SearchDatabaseModification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    fixedMod                         : bool
    massDelta                        : float
    Residues                         : string
    SpecificityRules                 : SpecificityRules
    RowVersion                       : DateTime
    SearchDatabaseModificationParams : List<SearchDatabaseModificationParam>
    }

///Params of SearchDatabaseModification
and [<CLIMutable>] 
    SearchDatabaseModificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : SearchDatabaseModification
    [<RequiredAttribute()>]
    Term                   : Term
    [<RequiredAttribute()>]
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

///Specification of a search modification as parameter for a spectra search. Contains the name of the modification, the mass, the specificity and whether it is a static modification.
and [<CLIMutable>] 
    SearchModification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                       : int
    FixedMod                 : bool
    MassDelta                : float
    Residues                 : string
    SpecificityRules         : SpecificityRules
    RowVersion               : DateTime
    SearchModificationParams : List<SearchModification>
    }

///Params of SearchModification
and [<CLIMutable>] 
    SearchModificationParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : SearchModification
    [<RequiredAttribute()>]
    Term                   : Term
    [<RequiredAttribute()>]
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

///The type of search performed e.g. PMF, Tag searches, MS-MS
and [<CLIMutable>] 
    SearchType =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID          : int
    Term        : Term
    UserParam   : UserParam
    RowVersion  : DateTime
    }

///The actual sequence of amino acids or nucleic acid.
and [<CLIMutable>] 
    Seq =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID          : int
    Sequence    : string
    RowVersion  : DateTime
    }

///The collection of sequences (DBSequence or Peptide) identified and their relationship between each other (PeptideEvidence) to be referenced elsewhere in the results.
and [<CLIMutable>] 
    SequenceCollection =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID              : int
    DBSequence      : DBSequence
    Peptide         : Peptide
    PeptideEvidence : PeptideEvidence
    RowVersion      : DateTime
    }

///Regular expression for specifying the enzyme cleavage site.
and [<CLIMutable>] 
    SiteRegexp =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Sequence   : string
    RowVersion : DateTime
    }

///Name of a SoftwarePackage
and [<CLIMutable>] 
    SoftwareName =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Name       : string
    //Term       : Term
    //UserParam  : UserParam
    RowVersion : DateTime
    }

///A file from which this mzIdentML instance was created.
and [<CLIMutable>] 
    SourceFile =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                          : int
    Name                        : string
    Location                    : string
    ExternalFormatDocumentation : ExternalFormatDocumentation
    FileFormat                  : FileFormat
    RowVersion                  : DateTime
    SourceFileParams            : List<SourceFileParam>
    UserParams                  : List<UserParam>
    }

///Params of SourceFile
and [<CLIMutable>] 
    SourceFileParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                     : int
    FKParamContainer       : SourceFile
    [<RequiredAttribute()>]
    Term                   : Term
    [<RequiredAttribute()>]
    Unit                   : Term
    Value                  : string
    RowVersion             : DateTime
    }

///The specificity rules of the searched modification including for example the probability of a modification's presence or peptide or protein termini. Standard fixed or variable status should be provided by the attribute fixedMod.
and [<CLIMutable>] 
    SpecificityRules =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Term       : Term
    RowVersion : DateTime
    }

///A data set containing spectra data (consisting of one or more spectra).
and [<CLIMutable>] 
    SpectraData =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                          : int
    Name                        : string 
    Location                    : string //Location of the datafile, commonly an URL
    ExternalFormatDocumentation : ExternalFormatDocumentation
    FileFormat                  : FileFormat
    SpectrumIDFormat            : SpectrumIDFormat
    RowVersion                  : DateTime
    //SpectraDataParams : List<SpectraDataParam>
    }

//and [<CLIMutable>] 
//    SpectraDataParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                     : int
//    FKParamContainer       : SpectraData
//    [<RequiredAttribute()>]
//    Term                   : Term
//    Unit                   : Term
//    Value                  : string
//    RowVersion             : DateTime
//    }

///An Analysis which tries to identify peptides in input spectra, referencing the database searched, the input spectra, the output results and the protocol that is run.
and [<CLIMutable>] 
    SpectrumIdentification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                              : int
    Name                            : string 
    ActivityDate                    : DateTime
    SpectrumIdentificationList      : SpectrumIdentificationList
    SpectrumIdentificationProtocol  : SpectrumIdentificationProtocol
    InputSpectra                    : InputSpectra
    SearchDatabaseRef               : SearchDatabaseRef
    RowVersion                      : DateTime
    //SpectrumIdentificationParams      : List<SpectrumIdentificationParam>
    }

//and [<CLIMutable>] 
//    SpectrumIdentificationParam =
//    {
//    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
//    ID                     : int
//    FKParamContainer       : SpectrumIdentification
//    [<RequiredAttribute()>]
//    Term                   : Term
//    Unit                   : Term
//    Value                  : string
//    RowVersion             : DateTime
//    }

///An identification of a single (poly)peptide, resulting from querying an input spectra, along with the set of confidence values for that identification.
and [<CLIMutable>] 
    SpectrumIdentificationItem =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    Name                             : string
    SpectrumIdentificationResult     : SpectrumIdentificationResult
    Sample                           : Sample          
    Peptide                          : Peptide
    MassTable                        : MassTable 
    PassThreshold                    : bool
    Rank                             : int 
    CalculatedMassToCharge           : float 
    ExperimentalMassToCharge         : float
    ChargeState                      : int
    CalculatedIP                     : float 
    Fragmentation                    : Fragmentation
    PeptideEvidenceRef               : PeptideEvidenceRef
    RowVersion                       : DateTime
    SpectrumIdentificationItemParams : List<SpectrumIdentificationItemParam>
    UserParams                       : List<UserParam>
    }

///Params of SpectrumIdentification.
and [<CLIMutable>] 
    SpectrumIdentificationItemParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : SpectrumIdentificationItem
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Term           : Term
    Unit           : Term
    Value            : string
    RowVersion       : DateTime 
    }

and [<CLIMutable>] 
    SpectrumIdentificationItemRef =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                         : int
    SpectrumIdentificationItem : SpectrumIdentificationItem
    RowVersion                 : DateTime
    }

///Represents the set of all search results from SpectrumIdentification.
and [<CLIMutable>] 
    SpectrumIdentificationList =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                               : int
    Name                             : string 
    NumSequencesSearched             : int
    FragmentationTable               : FragmentationTable
    SpectrumIdentificationResult     : SpectrumIdentificationResult
    RowVersion                       : DateTime
    SpectrumIdentificationListParams : List<SpectrumIdentificationListParam>
    UserParams                       : List<UserParam>
    }

///Params of SpectrumIdentificationList.
and [<CLIMutable>] 
    SpectrumIdentificationListParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : SpectrumIdentificationList
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime 
    }

///The parameters and settings of a SpectrumIdentification analysis.
and [<CLIMutable>] 
    SpectrumIdentificationProtocol =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                   : int
    Name                                 : string 
    AnalysisSoftware                     : AnalysisSoftware
    SearchType                           : SearchType
    AdditionalSearchparams               : AdditionalSearchparams
    ModificationParams                   : ModificationParams
    Enzymes                              : Enzymes
    MassTable                            : MassTable
    FragmentTolerance                    : FragmentTolerance
    ParentTolerance                      : ParentTolerance
    Threshold                            : Threshold
    DatabaseFilters                      : DatabaseFilters
    DatabaseTranslation                  : DatabaseTranslation
    RowVersion                           : DateTime 
    //SpectrumIdentificationProtocolParams : List<SpectrumIdentificationProtocolParam>
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

///All identifications made from searching one spectrum.
and [<CLIMutable>] 
    SpectrumIdentificationResult =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                                 : int
    Name                               : string
    SpectrumID                         : int    //Refers to SpectrumID, unique in the SpectraData for the specific Spectrum
    SpectraData                        : SpectraData       
    SpectrumIdentificationItem         : SpectrumIdentificationItem 
    RowVersion                         : DateTime 
    SpectrumIdentificationResultParams : List<SpectrumIdentificationResultParam>
    UserParams                         : UserParam
    }

and [<CLIMutable>] 
    SpectrumIdentificationResultParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    //FKParamContainer : SpectrumIdentificationResult
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    Value            : string
    RowVersion       : DateTime    
    }

and [<CLIMutable>] 
    SpectrumIDFormat =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Term       : Term
    RowVersion : DateTime 
    }

///References to the individual component samples within a mixed parent sample.
and [<CLIMutable>] 
    SubSample =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Sample     : Sample
    RowVersion : DateTime 
    }

///A modification where one residue is substituted by another (amino acid change).
and [<CLIMutable>] 
    SubstitutionModification =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID                    : int
    Location              : int
    AvgMassDelta          : float
    MonoIsotopicMassDelta : float
    OriginalResidue       : string
    ReplacementResidue    : string
    RowVersion            : DateTime 
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

///The threshold(s) applied to determine that a result is significant. If multiple terms are used it is assumed that all conditions are satisfied by the passing results.
and [<CLIMutable>] 
    Threshold =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID         : int
    Term       : Term
    UserParam  : UserParam
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
    [<RequiredAttribute()>]
    Unit             : Term
    UnitName         : string
    Value            : string
    RowVersion       : DateTime    
    }

and [<CLIMutable>] 
    UserParam =
    {
    [<DatabaseGenerated(DatabaseGeneratedOption.Identity)>] 
    ID               : int
    [<RequiredAttribute()>]
    Term             : Term
    [<RequiredAttribute()>]
    Unit             : Term
    UnitName         : string
    Value            : string
    RowVersion       : DateTime    
    }

///Defining Context of DB
type DBMSContext =

    inherit DbContext

    new() = {inherit DbContext()}
    new(options : DbContextOptions<DBMSContext>) = { inherit DbContext(options) }
    
    [<DefaultValue>] 
    val mutable m_mzIdentML : DbSet<MzIdentML>
    member public this.MzIdentML with get() = this.m_mzIdentML
                                                and set value = this.m_mzIdentML <- value

    //[<DefaultValue>] 
    //val mutable m_mzIdentMLParam : DbSet<MzIdentMLParam>
    //member public this.MzIdentMLParam with get() = this.m_mzIdentMLParam
    //                                            and set value = this.m_mzIdentMLParam <- value

    //[<DefaultValue>] 
    //val mutable m_additionalSearchParam : DbSet<AdditionalSearchparam>
    //member public this.AdditionalSearchParam with get() = this.m_additionalSearchParam
    //                                            and set value = this.m_additionalSearchParam <- value

    [<DefaultValue>] 
    val mutable m_additionalSearchParams : DbSet<AdditionalSearchparams>
    member public this.AdditionalSearchParams with get() = this.m_additionalSearchParams
                                                and set value = this.m_additionalSearchParams <- value

    [<DefaultValue>] 
    val mutable m_Affiliation : DbSet<Affiliation>
    member public this.Affiliation with get() = this.m_Affiliation
                                                and set value = this.m_Affiliation <- value

    [<DefaultValue>] 
    val mutable m_AmbiguousResidue : DbSet<AmbiguousResidue>
    member public this.AmbiguousResidue with get() = this.m_AmbiguousResidue
                                                and set value = this.m_AmbiguousResidue <- value

    [<DefaultValue>] 
    val mutable m_AmbiguousResidueParam : DbSet<AmbiguousResidueParam>
    member public this.AmbiguousResidueParam with get() = this.m_AmbiguousResidueParam
                                                    and set value = this.m_AmbiguousResidueParam <- value

    [<DefaultValue>] 
    val mutable m_analysisCollection : DbSet<AnalysisCollection>
    member public this.AnalysisCollection with get() = this.m_analysisCollection
                                                and set value = this.m_analysisCollection <- value

    [<DefaultValue>] 
    val mutable m_analysisData : DbSet<AnalysisData>
    member public this.AnalysisData with get() = this.m_analysisData
                                                and set value = this.m_analysisData <- value

    [<DefaultValue>] 
    val mutable m_analysisParams : DbSet<AnalysisParams>
    member public this.AnalysisParams with get() = this.m_analysisParams
                                                and set value = this.m_analysisParams <- value

    [<DefaultValue>] 
    val mutable m_analysisProtocolCollection : DbSet<AnalysisProtocolCollection>
    member public this.AnalysisProtocolCollection with get() = this.m_analysisProtocolCollection
                                                            and set value = this.m_analysisProtocolCollection <- value

    [<DefaultValue>] 
    val mutable m_analysisSampleCollection : DbSet<AnalysisSampleCollection>
    member public this.AnalysisSampleCollection with get() = this.m_analysisSampleCollection
                                                            and set value = this.m_analysisSampleCollection <- value


    [<DefaultValue>] 
    val mutable m_analysisSoftware : DbSet<AnalysisSoftware>
    member public this.AnalysisSoftware with get() = this.m_analysisSoftware
                                                and set value = this.m_analysisSoftware <- value
    //[<DefaultValue>] 
    //val mutable m_analysisSoftwareParam : DbSet<AnalysisSoftwareParam>
    //member public this.AnalysisSoftwareParam with get() = this.m_analysisSoftwareParam
    //                                            and set value = this.m_analysisSoftwareParam <- value
  
    [<DefaultValue>] 
    val mutable m_analysisSoftwareList : DbSet<AnalysisSoftwareList>
    member public this.AnalysisSoftwareList with get() = this.m_analysisSoftwareList
                                                and set value = this.m_analysisSoftwareList <- value

    [<DefaultValue>] 
    val mutable m_AuditCollection : DbSet<AuditCollection>
    member public this.AuditCollection with get() = this.m_AuditCollection
                                                and set value = this.m_AuditCollection <- value

    [<DefaultValue>] 
    val mutable m_BiblioGraphicReference : DbSet<BiblioGraphicReference>
    member public this.BiblioGraphicReference with get() = this.m_BiblioGraphicReference
                                                and set value = this.m_BiblioGraphicReference <- value

    [<DefaultValue>] 
    val mutable m_ContactRole : DbSet<ContactRole>
    member public this.ContactRole with get() = this.m_ContactRole
                                                and set value = this.m_ContactRole <- value

    [<DefaultValue>] 
    val mutable m_Customizations : DbSet<Customizations>
    member public this.Customizations with get() = this.m_Customizations
                                                and set value = this.m_Customizations <- value

    [<DefaultValue>] 
    val mutable m_CV : DbSet<CV>
    member public this.CV with get() = this.m_CV
                                    and set value = this.m_CV <- value

    [<DefaultValue>] 
    val mutable m_CVList : DbSet<CVList>
    member public this.CVList with get() = this.CVList
                                        and set value = this.CVList <- value

    //[<DefaultValue>] 
    //val mutable m_CVParam : DbSet<CVParam>
    //member public this.CVParam with get() = this.m_CVParam
    //                                    and set value = this.m_CVParam <- value

    [<DefaultValue>] 
    val mutable m_DatabaseFilters : DbSet<DatabaseFilters>
    member public this.DatabaseFilters with get() = this.m_DatabaseFilters
                                                and set value = this.m_DatabaseFilters <- value

    [<DefaultValue>] 
    val mutable m_DatabaseName : DbSet<DatabaseName>
    member public this.DatabaseName with get() = this.m_DatabaseName
                                            and set value = this.m_DatabaseName <- value

    [<DefaultValue>] 
    val mutable m_DatabaseTranslation : DbSet<DatabaseTranslation>
    member public this.DatabaseTranslation with get() = this.m_DatabaseTranslation
                                                    and set value = this.m_DatabaseTranslation <- value

    [<DefaultValue>] 
    val mutable m_DataCollection : DbSet<DataCollection>
    member public this.DataCollection with get() = this.m_DataCollection
                                                    and set value = this.m_DataCollection <- value

    [<DefaultValue>] 
    val mutable m_dbSequence : DbSet<DBSequence>
    member public this.DBSequence with get() = this.m_dbSequence
                                                and set value = this.m_dbSequence <- value

    [<DefaultValue>] 
    val mutable m_dbSequenceParam : DbSet<DBSequenceParam>
    member public this.DBSequenceParam with get() = this.m_dbSequenceParam
                                                and set value = this.m_dbSequenceParam <- value

    [<DefaultValue>] 
    val mutable m_enzyme : DbSet<Enzyme>
    member public this.Enzyme with get() = this.m_enzyme
                                    and set value = this.m_enzyme <- value

    [<DefaultValue>] 
    val mutable m_enzymeName : DbSet<EnzymeName>
    member public this.EnzymeName with get() = this.m_enzymeName
                                    and set value = this.m_enzymeName <- value

    [<DefaultValue>] 
    val mutable m_enzymes : DbSet<Enzymes>
    member public this.Enzymes with get() = this.m_enzymes
                                    and set value = this.m_enzymes <- value

    [<DefaultValue>] 
    val mutable m_Exclude : DbSet<Exclude>
    member public this.Exclude with get() = this.m_Exclude
                                    and set value = this.m_Exclude <- value

    [<DefaultValue>] 
    val mutable m_ExternalFormatDocumentation : DbSet<ExternalFormatDocumentation>
    member public this.ExternalFormatDocumentation with get() = this.m_ExternalFormatDocumentation
                                                            and set value = this.m_ExternalFormatDocumentation <- value

    [<DefaultValue>] 
    val mutable m_FileFormat : DbSet<FileFormat>
    member public this.FileFormat with get() = this.m_FileFormat
                                                and set value = this.m_FileFormat <- value

    [<DefaultValue>] 
    val mutable m_Filter : DbSet<Filter>
    member public this.Filter with get() = this.m_Filter
                                        and set value = this.m_Filter <- value

    [<DefaultValue>] 
    val mutable m_FilterType : DbSet<FilterType>
    member public this.FilterType with get() = this.m_FilterType
                                        and set value = this.m_FilterType <- value

    [<DefaultValue>] 
    val mutable m_FragmentArray : DbSet<FragmentArray>
    member public this.FragmentArray with get() = this.m_FragmentArray
                                        and set value = this.m_FragmentArray <- value

    [<DefaultValue>] 
    val mutable m_Fragmentation : DbSet<Fragmentation>
    member public this.Fragmentation with get() = this.m_Fragmentation
                                        and set value = this.m_Fragmentation <- value

    [<DefaultValue>] 
    val mutable m_FragmentationTable : DbSet<FragmentationTable>
    member public this.FragmentationTable with get() = this.m_FragmentationTable
                                            and set value = this.m_FragmentationTable <- value

    [<DefaultValue>] 
    val mutable m_FragmentTolerance : DbSet<FragmentTolerance>
    member public this.FragmentTolerance with get() = this.m_FragmentTolerance
                                                and set value = this.m_FragmentTolerance <- value

    [<DefaultValue>] 
    val mutable m_Include : DbSet<Include>
    member public this.Include with get() = this.m_Include
                                        and set value = this.m_Include <- value

    [<DefaultValue>] 
    val mutable m_Input : DbSet<Input>
    member public this.Input with get() = this.m_Input
                                        and set value = this.m_Input <- value

    [<DefaultValue>] 
    val mutable m_InputSpectra : DbSet<InputSpectra>
    member public this.InputSpectra with get() = this.m_InputSpectra
                                        and set value = this.m_InputSpectra <- value

    [<DefaultValue>] 
    val mutable m_InputSpectrumIdentification : DbSet<InputSpectrumIdentification>
    member public this.InputSpectrumIdentification with get() = this.m_InputSpectrumIdentification
                                                        and set value = this.m_InputSpectrumIdentification <- value

    [<DefaultValue>] 
    val mutable m_IonType : DbSet<IonType>
    member public this.IonType with get() = this.m_IonType
                                        and set value = this.m_IonType <- value

    [<DefaultValue>] 
    val mutable m_masstable : DbSet<MassTable>
    member public this.Masstable with get() = this.m_masstable
                                                and set value = this.m_masstable <- value

    [<DefaultValue>] 
    val mutable m_masstableParam : DbSet<MassTableParam>
    member public this.MasstableParam with get() = this.m_masstableParam
                                                and set value = this.m_masstableParam <- value

    [<DefaultValue>] 
    val mutable m_Measure : DbSet<Measure>
    member public this.Measure with get() = this.m_Measure
                                        and set value = this.m_Measure <- value

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
    val mutable m_parentTolerance : DbSet<ParentTolerance>
    member public this.ParentTolerance with get() = this.m_parentTolerance
                                                and set value = this.m_parentTolerance <- value

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
    val mutable m_peptideEvidenceRef : DbSet<PeptideEvidenceRef>
    member public this.PeptideEvidenceRef with get() = this.m_peptideEvidenceRef
                                                and set value = this.m_peptideEvidenceRef <- value

    [<DefaultValue>] 
    val mutable m_peptideEvidenceParam : DbSet<PeptideEvidenceParam>
    member public this.PeptideEvidenceParam with get() = this.m_peptideEvidenceParam
                                                and set value = this.m_peptideEvidenceParam <- value

    [<DefaultValue>] 
    val mutable m_peptideHypothesis : DbSet<PeptideHypothesis>
    member public this.PeptideHypothesis with get() = this.m_peptideHypothesis
                                                and set value = this.m_peptideHypothesis <- value

    //[<DefaultValue>] 
    //val mutable m_peptideHypothesisParam : DbSet<PeptideHypothesisParam>
    //member public this.PeptideHypothesisParam with get() = this.m_peptideHypothesisParam
    //                                            and set value = this.m_peptideHypothesisParam <- value

    [<DefaultValue>] 
    val mutable m_PeptideSequence : DbSet<PeptideSequence>
    member public this.PeptideSequence with get() = this.m_PeptideSequence
                                                and set value = this.m_PeptideSequence <- value

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

    //[<DefaultValue>] 
    //val mutable m_proteinDetectionParam : DbSet<ProteinDetectionParam>
    //member public this.ProteinDetectionParam with get() = this.m_proteinDetectionParam
    //                                                and set value = this.m_proteinDetectionParam <- value

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

    //[<DefaultValue>] 
    //val mutable m_proteinDetectionProtocolParam : DbSet<ProteinDetectionProtocolParam>
    //member public this.ProteinDetectionProtocolParam with get() = this.m_proteinDetectionProtocolParam
    //                                                    and set value = this.m_proteinDetectionProtocolParam <- value

    [<DefaultValue>] 
    val mutable m_Provider : DbSet<Provider>
    member public this.Provider with get() = this.m_Provider
                                        and set value = this.m_Provider <- value

    [<DefaultValue>] 
    val mutable m_Residue : DbSet<Residue>
    member public this.Residue with get() = this.m_Residue
                                        and set value = this.m_Residue <- value

    [<DefaultValue>] 
    val mutable m_Role : DbSet<Role>
    member public this.Role with get() = this.m_Role
                                        and set value = this.m_Role <- value

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
    val mutable m_searchDatabaseRef : DbSet<SearchDatabaseRef>
    member public this.SearchDatabaseRef with get() = this.m_searchDatabaseRef
                                                and set value = this.m_searchDatabaseRef <- value

    [<DefaultValue>] 
    val mutable m_SearchModification : DbSet<SearchModification>
    member public this.SearchModification with get() = this.m_SearchModification
                                                and set value = this.m_SearchModification <- value

    [<DefaultValue>] 
    val mutable m_SearchType : DbSet<SearchType>
    member public this.SearchType with get() = this.m_SearchType
                                            and set value = this.m_SearchType <- value

    [<DefaultValue>] 
    val mutable m_seq : DbSet<Seq>
    member public this.Seq with get() = this.m_seq
                                    and set value = this.m_seq <- value

    [<DefaultValue>] 
    val mutable m_sequenceCollection : DbSet<SequenceCollection>
    member public this.SequenceCollection with get() = this.m_sequenceCollection
                                                and set value = this.m_sequenceCollection <- value

    [<DefaultValue>] 
    val mutable m_siteRegexp : DbSet<SiteRegexp>
    member public this.SiteRegexp with get() = this.m_siteRegexp
                                            and set value = this.m_siteRegexp <- value

    [<DefaultValue>] 
    val mutable m_softwareName : DbSet<SoftwareName>
    member public this.SoftwareName with get() = this.m_softwareName
                                            and set value = this.m_softwareName <- value

    [<DefaultValue>] 
    val mutable m_sourceFile : DbSet<SourceFile>
    member public this.SourceFile with get() = this.m_sourceFile
                                            and set value = this.m_sourceFile <- value

    [<DefaultValue>] 
    val mutable m_specificityRules : DbSet<SpecificityRules>
    member public this.SpecificityRules with get() = this.m_specificityRules
                                            and set value = this.m_specificityRules <- value

    [<DefaultValue>] 
    val mutable m_spectraData : DbSet<SpectraData>
    member public this.SpectraData with get() = this.m_spectraData
                                                and set value = this.m_spectraData <- value

    //[<DefaultValue>] 
    //val mutable m_spectraDataParam : DbSet<SpectraDataParam>
    //member public this.SpectraDataParam with get() = this.m_spectraDataParam
    //                                            and set value = this.m_spectraDataParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentification : DbSet<SpectrumIdentification>
    member public this.SpectrumIdentification with get() = this.m_spectrumIdentification
                                                    and set value = this.m_spectrumIdentification <- value

    //[<DefaultValue>] 
    //val mutable m_spectrumIdentificationParam : DbSet<SpectrumIdentificationParam>
    //member public this.SpectrumIdentificationParam with get() = this.m_spectrumIdentificationParam
    //                                                and set value = this.m_spectrumIdentificationParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationItem : DbSet<SpectrumIdentificationItem>
    member public this.SpectrumIdentificationItem with get() = this.m_spectrumIdentificationItem
                                                        and set value = this.m_spectrumIdentificationItem <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationItemParam : DbSet<SpectrumIdentificationItemParam>
    member public this.SpectrumIdentificationItemParam with get() = this.m_spectrumIdentificationItemParam
                                                       and set value = this.m_spectrumIdentificationItemParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationItemRef : DbSet<SpectrumIdentificationItemRef>
    member public this.SpectrumIdentificationItemRef with get() = this.m_spectrumIdentificationItemRef
                                                            and set value = this.m_spectrumIdentificationItemRef <- value

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

    //[<DefaultValue>] 
    //val mutable m_spectrumIdentificationProtocolParam : DbSet<SpectrumIdentificationProtocolParam>
    //member public this.SpectrumIdentificationProtocolParam with get() = this.m_spectrumIdentificationProtocolParam
    //                                                        and set value = this.m_spectrumIdentificationProtocolParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationResult : DbSet<SpectrumIdentificationResult>
    member public this.SpectrumIdentificationResult with get() = this.m_spectrumIdentificationResult
                                                    and set value = this.m_spectrumIdentificationResult <- value

    [<DefaultValue>] 
    val mutable m_spectrumIdentificationResultParam : DbSet<SpectrumIdentificationResultParam>
    member public this.SpectrumIdentificationResultParam with get() = this.m_spectrumIdentificationResultParam
                                                            and set value = this.m_spectrumIdentificationResultParam <- value

    [<DefaultValue>] 
    val mutable m_spectrumIDFormat : DbSet<SpectrumIDFormat>
    member public this.SpectrumIDFormat with get() = this.m_spectrumIDFormat
                                        and set value = this.m_spectrumIDFormat <- value

    [<DefaultValue>] 
    val mutable m_subSample : DbSet<SubSample>
    member public this.SubSample with get() = this.m_subSample
                                        and set value = this.m_subSample <- value

    [<DefaultValue>] 
    val mutable m_substitutionModification : DbSet<SubstitutionModification>
    member public this.SubstitutionModification with get() = this.m_substitutionModification
                                                        and set value = this.m_substitutionModification <- value

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
    val mutable m_Threshold : DbSet<Threshold>
    member public this.Threshold with get() = this.m_Threshold
                                        and set value = this.m_Threshold <- value

    [<DefaultValue>] 
    val mutable m_translationTable : DbSet<TranslationTable>
    member public this.TranslationTable with get() = this.m_translationTable
                                            and set value = this.m_translationTable <- value

    [<DefaultValue>] 
    val mutable m_translationTableParam : DbSet<TranslationTableParam>
    member public this.TranslationTableParam with get() = this.m_translationTableParam
                                                    and set value = this.m_translationTableParam <- value

    [<DefaultValue>] 
    val mutable m_userParam : DbSet<UserParam>
    member public this.UserParam with get() = this.m_userParam
                                        and set value = this.m_userParam <- value

    //override this.OnConfiguring (optionsBuilder :  DbContextOptionsBuilder) =
    //    let fileDir = __SOURCE_DIRECTORY__ 
    //    let dbPath = fileDir + "\Ontologies_Terms\MSDatenbank.db"
    //    optionsBuilder.EnableSensitiveDataLogging() |> ignore
    //    optionsBuilder.UseSqlite(@"Data Source="+ dbPath) |> ignore

let standardDBPath = fileDir + "\Ontologies_Terms\MSDatenbank.db"
let configureSqlServerContext path = 
    (fun () ->
        let optionsBuilder = new DbContextOptionsBuilder<DBMSContext>()
        optionsBuilder.UseSqlite(@"Data Source=" + path) |> ignore
        new DBMSContext(optionsBuilder.Options)
    )

///Define functions to create  for tables of DB////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////
module InsertStatements =
    open System.ComponentModel
    open System.Security.Policy
    open Microsoft.EntityFrameworkCore.Metadata.Internal
    
    let createMzIdentML name version cvList analysisSoftwareList provider auditCollection analysisSampleCollection sequenceCollection analysisCollection analysisProtocolCollection dataCollection biblioGraphicReference =
        {
         MzIdentML.ID                         = 0
         MzIdentML.Name                       = name
         MzIdentML.Version                    = version
         MzIdentML.CVList                     = cvList
         MzIdentML.AnalysisSoftwareList       = analysisSoftwareList
         MzIdentML.Provider                   = provider
         MzIdentML.AuditCollection            = auditCollection
         MzIdentML.AnalysisSampleCollection   = analysisSampleCollection
         MzIdentML.SequenceCollection         = sequenceCollection
         MzIdentML.AnalysisCollection         = analysisCollection
         MzIdentML.AnalysisProtocolCollection = analysisProtocolCollection
         MzIdentML.DataCollection             = dataCollection
         MzIdentML.BiblioGraphicReference     = biblioGraphicReference
         MzIdentML.RowVersion                 = DateTime.Now.Date
        }

    let createAdditionalSearchParams term userParam =
        {
         AdditionalSearchparams.ID         = 0
         AdditionalSearchparams.Term       = term
         AdditionalSearchparams.UserParam  = userParam
         AdditionalSearchparams.RowVersion = DateTime.Now.Date
        }
    
    let createAffiliation organization =
        {
         DBSequence.ID               = 0
         DBSequence.Name             = name
         DBSequence.Accession        = accession
         DBSequence.Length           = length
         DBSequence.Seq              = sequence
         DBSequence.SearchDB         = searchDB
         DBSequence.RowVersion       = DateTime.Now.Date
         DBSequence.DBSequenceParams = dBSequenceParams
         DBSequence.UserParam        = userParams
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
         Enzyme.ID           = 0
         Enzyme.Name         = name
         Enzyme.CleavageSite = cleavagesite
         Enzyme.CTermGain    = cTermGain
         Enzyme.NTermGain    = nTermGain
         Enzyme.MinDistance  = minDistance
         Enzyme.SemiSpecific = semiSpecific
         Enzyme.RowVersion   = DateTime.Now.Date
         Enzyme.EnzymeParams = enzymeParams
        }

    let createEnzymeName name term userParam =
        {
         EnzymeName.ID         = 0
         EnzymeName.Name       = name
         EnzymeName.Term       = term
         EnzymeName.UserParam  = userParam
         EnzymeName.RowVersion = DateTime.Now.Date
        }

    let createEnzymes independent enzyme =
        {
         Enzymes.ID          = 0
         Enzymes.Independent = independent
         Enzymes.Enzyme      = enzyme
         Enzymes.RowVersion  = DateTime.Now.Date
        }

    let createExclude term userParam =
        {
         Exclude.ID         = 0
         Exclude.Term       = term
         Exclude.UserParam  = userParam
         Exclude.RowVersion = DateTime.Now.Date
        }

    let createExternalFormatDocumentation uri format =
        {
         ExternalFormatDocumentation.ID         = 0
         ExternalFormatDocumentation.URI        = uri
         ExternalFormatDocumentation.Format     = format
         ExternalFormatDocumentation.RowVersion = DateTime.Now.Date
        }

    let createFileFormat format fileFormatParams =
        {
         FileFormat.ID               = 0
         FileFormat.Format           = format 
         FileFormat.RowVersion       = DateTime.Now.Date
         FileFormat.FileFormatParams = fileFormatParams
        }

    let createFilter filterType exclude includes =
        {
         Filter.ID         = 0
         Filter.FilterType = filterType
         Filter.Exclude    = exclude
         Filter.Include    = includes
         Filter.RowVersion = DateTime.Now.Date
        }

    let createFilterType name term userParams =
        {
         FilterType.ID               = 0
         FilterType.Name             = name
         FilterType.FilterTypeParams = term
         FilterType.UserParams       = userParams
         FilterType.RowVersion       = DateTime.Now.Date
        }

    let createFragmentArray values fragmentationTable =
        {
         FragmentArray.ID                 = 0
         FragmentArray.Values             = values
         FragmentArray.FragmentationTable = fragmentationTable
         FragmentArray.RowVersion         = DateTime.Now.Date
        }

    let createValue value fragmentArray =
        {
         Value.ID            = 0
         Value.Value         = value
         Value.FragmentArray = fragmentArray
         Value.RowVersion    = DateTime.Now.Date
        }

    let createFragmentation ionType =
        {
         Fragmentation.ID         = 0
         Fragmentation.IonType    = ionType
         Fragmentation.RowVersion = DateTime.Now.Date
        }

    let createFragmentationTable measure =
        {
         FragmentationTable.ID         = 0
         FragmentationTable.Measure    = measure
         FragmentationTable.RowVersion = DateTime.Now.Date
        }

    let createFragmentTolerance value fragmentToleranceParams =
        {
         FragmentTolerance.ID                      = 0
         FragmentTolerance.Value                   = value
         FragmentTolerance.RowVersion              = DateTime.Now.Date
         FragmentTolerance.FragmentToleranceParams = fragmentToleranceParams
        }

    let createInclude term userParam =
        {
         Include.ID         = 0
         Include.Term       = term
         Include.UserParam  = userParam
         Include.RowVersion = DateTime.Now.Date
        }

    let createInputs searchDatabase sourceFile spectraData =
        {
         Input.ID             = 0
         Input.SearchDatabase = searchDatabase
         Input.SourceFile     = sourceFile
         Input.SpectraData    = spectraData
         Input.RowVersion     = DateTime.Now.Date
        }

    let createInputSpectra spectraData =
        {
         InputSpectra.ID          = 0
         InputSpectra.SpectraData = spectraData
         InputSpectra.RowVersion  = DateTime.Now.Date
        }

    let createInputSpectrumIdentification spectrumIdentificationList =
        {
         InputSpectrumIdentification.ID                         = 0
         InputSpectrumIdentification.SpectrumIdentificationList = spectrumIdentificationList
         InputSpectrumIdentification.RowVersion                 = DateTime.Now.Date
        }

    let createIonType index charge fragmentArray ionTypeParams userParams =
        {
         IonType.ID            = 0
         IonType.Index         = index
         IonType.Charge        = charge
         IonType.FragmentArray = fragmentArray
         IonType.RowVersion    = DateTime.Now.Date
         IonType.IonTypeParams = ionTypeParams
         IonType.UserParams    = userParams
        }

    let createIonTypeParam (*ionType*) value term unit =
        {
         IonTypeParam.ID               = 0
         //IonTypeParam.FKParamContainer = ionType
         IonTypeParam.Value            = value
         IonTypeParam.Term             = term
         IonTypeParam.Unit             = unit
         IonTypeParam.RowVersion       = DateTime.Now.Date
        }

    let createMassTable name msLevel massTableParams userParam =
        {
         MassTable.ID              = 0
         MassTable.Name            = name
         MassTable.MSLevels        = msLevel
         MassTable.RowVersion      = DateTime.Now.Date
         MassTable.MassTableParams = massTableParams
         MassTable.UserParam       = userParam
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
         Measure.ID            = 0
         Measure.Name          = name
         Measure.RowVersion    = DateTime.Now.Date
         Measure.MeasureParams = measureParams
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
         ModLocation.ID                = 0
         ModLocation.Modification      = modification
         ModLocation.Location          = location
         ModLocation.Peptide           = peptide
         ModLocation.Residue           = residue
         ModLocation.RowVersion        = DateTime.Now.Date
         ModLocation.ModLocationParams = modlocationParams
        }

    let createModLocationParam (*modLocation*) value term unit =
        {
         ModLocationParam.ID               = 0
         ModLocationParam.FKParamContainer = ModLocation
         ModLocationParam.Value            = Value
         ModLocationParam.Term             = Term
         ModLocationParam.Unit             = Unit
         ModLocationParam.RowVersion       = DateTime.Now.Date
        }

    let createOntology name ontologyParams terms =
        {
         Ontology.ID             = 0
         Ontology.Name           = name
         Ontology.RowVersion     = DateTime.Now.Date
         Ontology.OntologyParams = ontologyParams
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

    let createOrganizationParam Organization Value Term Unit =
        {
         OrganizationParam.ID               = 0
         OrganizationParam.FKParamContainer = Organization
         OrganizationParam.Value            = Value
         OrganizationParam.Term             = Term
         OrganizationParam.Unit             = Unit
         OrganizationParam.RowVersion       = DateTime.Now.Date
        }

    let createParent organization  =
        {
         Parent.ID           = 0
         //Parent.Name         = name
         //Parent.Country      = country
         Parent.Organization = organization
         Parent.RowVersion   = DateTime.Now.Date
         //Parent.ParentParams = parentParams
        }

    //let createParentParam parent value term unit =
    //    {
    //     ParentParam.ID               = 0
    //     ParentParam.FKParamContainer = parent
    //     ParentParam.Value            = value
    //     ParentParam.Term             = term
    //     ParentParam.Unit             = unit
    //     ParentParam.RowVersion       = DateTime.Now.Date
    //    }

    let createParentTolerance term =
        {
         ParentTolerance.ID         = 0
         ParentTolerance.Term       = term
         ParentTolerance.RowVersion = DateTime.Now.Date
        }

    let createPeptide name peptideSequence modification substitutionModification peptideParams userParams =
        {
         Peptide.ID                         = 0
         Peptide.Name                       = name
         Peptide.PeptideSequence            = peptideSequence
         Peptide.Modification               = modification
         Peptide.SubstitutionModification   = substitutionModification
         Peptide.RowVersion                 = DateTime.Now.Date
         Peptide.PeptideParams              = peptideParams
         Peptide.UserParams = userParams
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

    let createPeptideEvidence peptide dBSequence isDecoy frame translationTable start ende pre post peptideEvidenceParams userParams =
        {
         PeptideEvidence.ID                    = 0
         PeptideEvidence.Peptide               = peptide
         PeptideEvidence.DBSequence            = dBSequence
         PeptideEvidence.isDecoy               = isDecoy
         PeptideEvidence.Frame                 = frame
         PeptideEvidence.TranslationTable      = translationTable
         PeptideEvidence.Start                 = start
         PeptideEvidence.End                   = ende
         PeptideEvidence.Pre                   = pre
         PeptideEvidence.Post                  = post
         PeptideEvidence.RowVersion            = DateTime.Now.Date
         PeptideEvidence.PeptideEvidenceParams = peptideEvidenceParams
         PeptideEvidence.UserParams            = userParams
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

    //let createPeptideHypothesisParam peptideHypothesis value term unit =
    //    {
    //     PeptideHypothesisParam.ID               = 0
    //     PeptideHypothesisParam.FKParamContainer = peptideHypothesis
    //     PeptideHypothesisParam.Value            = value
    //     PeptideHypothesisParam.Term             = term
    //     PeptideHypothesisParam.Unit             = unit
    //     PeptideHypothesisParam.RowVersion       = DateTime.Now.Date
    //    }

    let createPeptideSequence sequence =
        {
         PeptideSequence.ID         = 0
         PeptideSequence.Sequence   = sequence
         PeptideSequence.RowVersion = DateTime.Now.Date
        }

    let createPerson firstName middleName lastName affiliation personenParams userParams =
        {
         Person.ID           = 0
         Person.FirstName    = firstName
         Person.MiddleName   = middleName
         Person.LastName     = lastName
         Person.Affiliation  = affiliation
         //Person.Organization = organization
         Person.RowVersion   = DateTime.Now.Date
         Person.PersonParams = personenParams
         Person.UserParams   = userParams
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

    let createProteinAmbiguityGroup name proteinDetectionHypothesis proteinAmbiguityGroupParams userParams =
        {
         ProteinAmbiguityGroup.ID                          = 0
         ProteinAmbiguityGroup.Name                        = name
         ProteinAmbiguityGroup.ProteinDetectionHypothesis  = proteinDetectionHypothesis
         ProteinAmbiguityGroup.RowVersion                  = DateTime.Now.Date
         ProteinAmbiguityGroup.ProteinAmbiguityGroupParams = proteinAmbiguityGroupParams
         ProteinAmbiguityGroup.UserParams                  = userParams
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

    let createProteinDetection name proteinDetectionProtocol proteinDetectionList inputSpectrumIdentification =
        {
         ProteinDetection.ID                          = 0
         ProteinDetection.Name                        = name
         ProteinDetection.ProteinDetectionProtocol    = proteinDetectionProtocol
         ProteinDetection.ProteinDetectionList        = proteinDetectionList
         ProteinDetection.InputSpectrumIdentification = inputSpectrumIdentification
         ProteinDetection.RowVersion                  = DateTime.Now.Date
         //ProteinDetection.ProteinDetectionParams    = proteinDetectionParams
        }

    //let createProteinDetectionParam proteinDetection value term unit =
    //    {
    //     ProteinDetectionParam.ID               = 0
    //     ProteinDetectionParam.FKParamContainer = proteinDetection
    //     ProteinDetectionParam.Value            = value
    //     ProteinDetectionParam.Term             = term
    //     ProteinDetectionParam.Unit             = unit
    //     ProteinDetectionParam.RowVersion       = DateTime.Now.Date
    //    }

    let createProteinDetectionHypothesis name dBSequence passThreshold peptideHypothesis proteinDetectionHypothesisParams userParams =
        {
         ProteinDetectionHypothesis.ID                               = 0
         ProteinDetectionHypothesis.Name                             = name
         ProteinDetectionHypothesis.DBSequence                       = dBSequence
         ProteinDetectionHypothesis.PassThreshold                    = passThreshold
         ProteinDetectionHypothesis.PeptideHypothesis                = peptideHypothesis
         ProteinDetectionHypothesis.RowVersion                       = DateTime.Now.Date
         ProteinDetectionHypothesis.ProteinDetectionHypothesisParams = proteinDetectionHypothesisParams
         ProteinDetectionHypothesis.UserParams                       = userParams
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

    let createProteinDetectionList name proteinAmbiguityGroup proteinDetectionListParams userParams =
        {
         ProteinDetectionList.ID                         = 0
         ProteinDetectionList.Name                       = name
         ProteinDetectionList.ProteinAmbiguityGroup      = proteinAmbiguityGroup
         ProteinDetectionList.RowVersion                 = DateTime.Now.Date
         ProteinDetectionList.ProteinDetectionListParams = proteinDetectionListParams
         ProteinDetectionList.UserParams                 = userParams
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

    let createProteinDetectionProtocol name analysisSoftware analysisParams threshold =
        {
         ProteinDetectionProtocol.ID                             = 0
         ProteinDetectionProtocol.Name                           = name
         ProteinDetectionProtocol.AnalysisSoftware               = analysisSoftware
         ProteinDetectionProtocol.AnalysisParams                 = analysisParams
         ProteinDetectionProtocol.Threshold                      = threshold
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
         Residue.ID         = 0
         Residue.Sequence   = sequence
         Residue.Mass       = mass
         Residue.RowVersion = DateTime.Now.Date
        }

    let createRole name roleParams =
        {
         Role.ID         = 0
         Role.Name       = name
         Role.RowVersion = DateTime.Now.Date
         Role.RoleParams = roleParams
        }

    let createSample name contactRole subSample sampleParams userParams =
        {
         Sample.ID           = 0
         Sample.Name         = name
         Sample.ContactRole  = contactRole
         Sample.SubSample    = subSample
         Sample.RowVersion   = DateTime.Now.Date
         Sample.SampleParams = sampleParams
         Sample.UserParams   = userParams
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

    let createSearchDatabase name location numResidues numDatabaseSequences releaseDate version externalFormatDocumentation fileFormat dataBaseName searchDatabaseParams =
        {
         SearchDatabase.ID                          = 0
         SearchDatabase.Name                        = name
         SearchDatabase.Location                    = location
         SearchDatabase.NumResidues                 = numResidues
         SearchDatabase.NumDatabaseSequences        = numDatabaseSequences
         SearchDatabase.ReleaseDate                 = releaseDate
         SearchDatabase.Version                     = version
         SearchDatabase.ExternalFormatDocumentation = externalFormatDocumentation
         SearchDatabase.FileFormat                  = fileFormat
         SearchDatabase.DatabaseName                = dataBaseName
         SearchDatabase.RowVersion                  = DateTime.Now.Date
         SearchDatabase.SearchDatabaseParams        = searchDatabaseParams
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
         SearchType.ID         = 0
         SearchType.Term       = term
         SearchType.UserParam  = userParam
         SearchType.RowVersion = DateTime.Now.Date
        }

    let createSeq sequence =
        {
         Seq.ID         = 0
         Seq.Sequence   = sequence
         Seq.RowVersion = DateTime.Now.Date
        }

    let creaeSequenceCollection peptide peptideEvidence dbSequence =
        {
         SequenceCollection.ID              = 0
         SequenceCollection.Peptide         = peptide
         SequenceCollection.PeptideEvidence = peptideEvidence 
         SequenceCollection.DBSequence      = dbSequence
         SequenceCollection.RowVersion      = DateTime.Now.Date
        }

    let createSiteRegexp sequence =
        {
         SiteRegexp.ID         = 0
         SiteRegexp.Sequence   = sequence
         SiteRegexp.RowVersion = DateTime.Now.Date
        }

    let createSoftwareName name =
        {
         SoftwareName.ID         = 0
         SoftwareName.Name       = name
         SoftwareName.RowVersion = DateTime.Now.Date
        }

    let createSourceFile name location fileFormat externalFormatDocumentation sourceFileParams userParams =
        {
         SourceFile.ID                          = 0
         SourceFile.Name                        = name
         SourceFile.Location                    = location
         SourceFile.FileFormat                  = fileFormat
         SourceFile.ExternalFormatDocumentation = externalFormatDocumentation
         SourceFile.RowVersion                  = DateTime.Now.Date
         SourceFile.SourceFileParams            = sourceFileParams
         SourceFile.UserParams                  = userParams
        }

    let createSpecificityRules term =
        {
         SpecificityRules.ID         = 0
         SpecificityRules.Term       = term
         SpecificityRules.RowVersion = DateTime.Now.Date
        }

    let createSpectraData name location fileFormat externalFormatDocumentation spectrumIDFormat =
        {
         SpectraData.ID                          = 0
         SpectraData.Name                        = name
         SpectraData.Location                    = location
         SpectraData.FileFormat                  = fileFormat
         SpectraData.ExternalFormatDocumentation = externalFormatDocumentation
         SpectraData.SpectrumIDFormat            = spectrumIDFormat
         SpectraData.RowVersion                  = DateTime.Now.Date
         //SpectraData.SpectraDataParams = spectraDataParams
        }

    //let createSpectraDataParam spectraData value term unit =
    //    {
    //     SpectraDataParam.ID               = 0
    //     SpectraDataParam.FKParamContainer = spectraData
    //     SpectraDataParam.Value            = value
    //     SpectraDataParam.Term             = term
    //     SpectraDataParam.Unit             = unit
    //     SpectraDataParam.RowVersion       = DateTime.Now.Date
    //    }

    let createSpectrumIdentification name activityDate spectrumIdentificationList spectrumIdentificationProtocol inputSpectra searchDatabaseRef =
        {
         SpectrumIdentification.ID                              = 0
         SpectrumIdentification.Name                            = name
         SpectrumIdentification.ActivityDate                    = activityDate
         SpectrumIdentification.SpectrumIdentificationList      = spectrumIdentificationList
         SpectrumIdentification.SpectrumIdentificationProtocol  = spectrumIdentificationProtocol
         SpectrumIdentification.InputSpectra                    = inputSpectra
         SpectrumIdentification.SearchDatabaseRef               = searchDatabaseRef
         SpectrumIdentification.RowVersion                      = DateTime.Now.Date
         //SpectrumIdentification.SpectrumIdentificationParams    = spectrumIdentificationParams
        }

    //let createSpectrumIdentificationParam spectrumIdentification value term unit =
    //    {
    //     SpectrumIdentificationParam.ID               = 0
    //     SpectrumIdentificationParam.FKParamContainer = spectrumIdentification
    //     SpectrumIdentificationParam.Value            = value
    //     SpectrumIdentificationParam.Term             = term
    //     SpectrumIdentificationParam.Unit             = unit
    //     SpectrumIdentificationParam.RowVersion       = DateTime.Now.Date
    //    }

    let createSpectrumIdentificationItem name peptide chargeState sample passThreshold fragmentation rank massTable calculatedIP calculatedMassToCharge experimentalMassToCharge spectrumIdentificationResult peptideEvidenceRef spectrumIdentificationItemParams userParams =
        {
         SpectrumIdentificationItem.ID                               = 0
         SpectrumIdentificationItem.Name                             = name
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
         SpectrumIdentificationItem.Peptide                          = peptide
         SpectrumIdentificationItem.PeptideEvidenceRef               = peptideEvidenceRef
         SpectrumIdentificationItem.RowVersion                       = DateTime.Now.Date
         SpectrumIdentificationItem.SpectrumIdentificationItemParams = spectrumIdentificationItemParams
         SpectrumIdentificationItem.UserParams                       = userParams
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
         SpectrumIdentificationItemRef.ID                         = 0
         SpectrumIdentificationItemRef.SpectrumIdentificationItem = spectrumIdentificationItem
         SpectrumIdentificationItemRef.RowVersion                 = DateTime.Now.Date
        }

    let createSpectrumIdentificationList name numSequencesSearched fragmentationTable spectrumIdentificationResult spectrumIdentificationListParams userParams =
        {
         SpectrumIdentificationList.ID                               = 0
         SpectrumIdentificationList.Name                             = name
         SpectrumIdentificationList.NumSequencesSearched             = numSequencesSearched
         SpectrumIdentificationList.FragmentationTable               = fragmentationTable
         SpectrumIdentificationList.SpectrumIdentificationResult     = spectrumIdentificationResult
         SpectrumIdentificationList.RowVersion                       = DateTime.Now.Date
         SpectrumIdentificationList.SpectrumIdentificationListParams = spectrumIdentificationListParams
         SpectrumIdentificationList.UserParams                       = userParams
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

    let createSpectrumIdentificationProtocol name analysisSoftware searchType additionalSearchparams modificationParams enzymes massTable fragmentTolerance parentTolerance databaseFilters databaseTranslation threshold spectrumIdentificationProtocolParams =
        {
         SpectrumIdentificationProtocol.ID                     = 0
         SpectrumIdentificationProtocol.Name                   = name
         SpectrumIdentificationProtocol.AnalysisSoftware       = analysisSoftware
         SpectrumIdentificationProtocol.SearchType             = searchType
         SpectrumIdentificationProtocol.AdditionalSearchparams = additionalSearchparams
         SpectrumIdentificationProtocol.ModificationParams     = modificationParams
         SpectrumIdentificationProtocol.Enzymes                = enzymes
         SpectrumIdentificationProtocol.MassTable              = massTable
         SpectrumIdentificationProtocol.FragmentTolerance      = fragmentTolerance
         SpectrumIdentificationProtocol.ParentTolerance        = parentTolerance
         SpectrumIdentificationProtocol.DatabaseFilters        = databaseFilters
         SpectrumIdentificationProtocol.DatabaseTranslation    = databaseTranslation
         SpectrumIdentificationProtocol.Threshold              = threshold
         SpectrumIdentificationProtocol.RowVersion             = DateTime.Now.Date
         //SpectrumIdentificationProtocol.SpectrumIdentificationProtocolParams = spectrumIdentificationProtocolParams
        }

    //let createSpectrumIdentificationProtocolParam spectrumIdentificationProtocol value term unit =
    //    {
    //     SpectrumIdentificationProtocolParam.ID               = 0
    //     SpectrumIdentificationProtocolParam.FKParamContainer = spectrumIdentificationProtocol
    //     SpectrumIdentificationProtocolParam.Value            = value
    //     SpectrumIdentificationProtocolParam.Term             = term
    //     SpectrumIdentificationProtocolParam.Unit             = unit
    //     SpectrumIdentificationProtocolParam.RowVersion       = DateTime.Now.Date
    //    }

    let createSpectrumIdentificationResult name spectrumID spectraData spectrumIdentificationitem spectrumIdentificationResultParams userParams =
        {
         SpectrumIdentificationResult.ID                                 = 0
         SpectrumIdentificationResult.Name                               = name
         SpectrumIdentificationResult.SpectrumID                         = spectrumID
         SpectrumIdentificationResult.SpectraData                        = spectraData
         SpectrumIdentificationResult.SpectrumIdentificationItem         = spectrumIdentificationitem
         SpectrumIdentificationResult.RowVersion                         = DateTime.Now.Date
         SpectrumIdentificationResult.SpectrumIdentificationResultParams = spectrumIdentificationResultParams
         SpectrumIdentificationResult.UserParams                         = userParams
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

    let createSpectrumIDFormat term =
        {
         SpectrumIDFormat.ID         = 0
         SpectrumIDFormat.Term       = term
         SpectrumIDFormat.RowVersion = DateTime.Now.Date
        }

    let createSubSamples sample =
        {
         SubSample.ID         = 0
         SubSample.Sample     = sample
         SubSample.RowVersion = DateTime.Now.Date
        }

    let createSubstitutionModification location avgMassDelta monoIsotopicMassDelta orgResidue repResidue =
        {
         SubstitutionModification.ID                    = 0
         SubstitutionModification.Location              = location
         SubstitutionModification.AvgMassDelta          = avgMassDelta
         SubstitutionModification.MonoIsotopicMassDelta = monoIsotopicMassDelta
         SubstitutionModification.OriginalResidue       = orgResidue
         SubstitutionModification.ReplacementResidue    = repResidue
         SubstitutionModification.RowVersion            = DateTime.Now.Date
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
         Term.ID         = id
         Term.Name       = name
         Term.Ontology   = ontology
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

    let createThreshold term userParam =
        {
         Threshold.ID         = 0
         Threshold.Term       = term
         Threshold.UserParam  = userParam
         Threshold.RowVersion = DateTime.Now.Date
        }

    let createTranslationTable name translationTableParamms =
        {
         TranslationTable.ID                     = 0
         TranslationTable.Name                   = name
         TranslationTable.RowVersion             = DateTime.Now.Date
         TranslationTable.TranslationTableParams = translationTableParamms
        }

    let userParam value term unit unitName =
        {
         UserParam.ID         = 0
         UserParam.Value      = value
         UserParam.Term       = term
         UserParam.Unit       = unit
         UserParam.UnitName   = unitName
         UserParam.RowVersion = DateTime.Now.Date
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

//let spectrumIdentification =
//    createSpectrumIdentification "Test" "Today" 1 2 null

//let spectrumIdentificationParam =
//    createSpectrumIdentificationParam spectrumIdentification "Point" ontologyPride.Terms.[34] ontologyCustom.Terms.[0]

///Creating && Checking && inserting into DB

let initDB path=
    let db = (configureSqlServerContext path)()
    
    db.Database.EnsureCreated()                                     |> ignore

    db.Ontology.Add(ontologyCustom)                                 |> ignore
    db.Ontology.Add(ontologyPsiMS)                                  |> ignore
    db.Ontology.Add(ontologyPride)                                  |> ignore
    db.Ontology.Add(ontologyUniMod)                                 |> ignore
    db.Ontology.Add(ontologyUnit_Ontology)                          |> ignore
    //db.SpectrumIdentification.Add(spectrumIdentification)           |> ignore
    //db.SpectrumIdentificationParam.Add(spectrumIdentificationParam) |> ignore
    db.SaveChanges()

///Define Queries for DB//////////////////////////////
/////////////////////////////////////////////////////////////////////////
    
//let context = new DBMSContext()
    

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

//let readLine (input : seq<'a>) =
//    for i in input do
//        Console.WriteLine(i)

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

//let selectSpectrumIdentificationByID iD =
//    (query {
//            for i in context.SpectrumIdentification do
//                if i.ID = iD 
//                then select i
//           }
//    )

//let selectSpectrumIdentificationByName name =
//    (query {
//            for i in context.SpectrumIdentification do
//                if i.Name = name 
//                    then select i
//           }
//    )

//let selectSpectrumIdentificationByActivityDate activityDate =
//    (query {
//            for i in context.SpectrumIdentification do
//                if i.ActivityDate = activityDate 
//                    then select i
//           }
//    )

//let selectSpectrumIdentificationBySIList sIList =
//    (query {
//            for i in context.SpectrumIdentification do
//                if i.SpectrumIdentificationList = sIList 
//                    then select i
//           }
//    )

//let selectSpectrumIdentificationBySIProtocol sIProtocol =
//    (query {
//            for i in context.SpectrumIdentification do
//                if i.SpectrumIdentificationProtocol = sIProtocol 
//                    then select i
//           }
//    )

//let selectSpectrumIdentificationParamBySI sI =
//    (query {
//            for i in context.SpectrumIdentificationParam do
//                if i.FKParamContainer = sI 
//                    then select (i, i.Term, i.Unit)
//           }
//    )

//let selectSpectrumIdentificationParamBySIID sIID =
//    (query {
//            for i in context.SpectrumIdentificationParam do
//                if i.FKParamContainer.ID = sIID
//                    then select (i, i.Term, i.Unit)
//           }
//    )

//let selectSpectrumIdentificationParamBySIName sIName =
//    (query {
//            for i in context.SpectrumIdentificationParam do
//                if i.FKParamContainer.Name = sIName
//                    then select (i, i.Term, i.Unit)
//           }
//    )

//let selectTermByOntologyID ontologyID =
//    (query {
//            for i in context.Term do
//                if i.Ontology.ID = ontologyID
//                    then select (i)
//           }
//    )

//let selectTermByOntologyName ontologyName =
//    (query {
//            for i in context.Term do
//                if i.Ontology.Name = ontologyName
//                    then select (i)
//           }
//    )

//let selectTermByTermID termID =
//    (query {
//            for i in context.Term do
//                if i.ID = termID
//                    then select (i)
//           }
//    )

//let selectTermByTermName termName =
//    (query {
//            for i in context.Term do
//                if i.Name = termName
//                    then select (i)
//           }
//    )

//context.SaveChanges()


///Apply queries to DB////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////

//selectSpectrumIdentificationByID 1
//|> readLine
//selectSpectrumIdentificationByName "Test"
//|> readLine
//selectSpectrumIdentificationByActivityDate "Today"
//|> readLine
//selectSpectrumIdentificationBySIList 1
//|> readLine
//selectSpectrumIdentificationBySIProtocol 2
//|> readLine
//selectSpectrumIdentificationParamBySI spectrumIdentification
//|> readLine
//selectTermByOntologyID 3
//|> readLine
//selectTermByOntologyName "Pride"
//|> readLine

//Init Database

initDB standardDBPath