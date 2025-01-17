IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SrUsr') AND type='V')
DROP VIEW dbo.SrUsr
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwAppItem') AND type='V')
DROP VIEW dbo.VwAppItem
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwCheckBox') AND type='V')
DROP VIEW dbo.VwCheckBox
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwClnAppItem') AND type='V')
DROP VIEW dbo.VwClnAppItem
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwCompanyPref') AND type='V')
DROP VIEW dbo.VwCompanyPref
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwCulture') AND type='V')
DROP VIEW dbo.VwCulture
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwCultureLbl') AND type='V')
DROP VIEW dbo.VwCultureLbl
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwDisplayType') AND type='V')
DROP VIEW dbo.VwDisplayType
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwIntUsr') AND type='V')
DROP VIEW dbo.VwIntUsr
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwLabel') AND type='V')
DROP VIEW dbo.VwLabel
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwLinkType') AND type='V')
DROP VIEW dbo.VwLinkType
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwMailProfile') AND type='V')
DROP VIEW dbo.VwMailProfile
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwMIMEType') AND type='V')
DROP VIEW dbo.VwMIMEType
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwNumSuccess') AND type='V')
DROP VIEW dbo.VwNumSuccess
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwPermKey') AND type='V')
DROP VIEW dbo.VwPermKey
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwPermKeyRow') AND type='V')
DROP VIEW dbo.VwPermKeyRow
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwReportCriHlp') AND type='V')
DROP VIEW dbo.VwReportCriHlp
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwRintagi') AND type='V')
DROP VIEW dbo.VwRintagi
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwRowAuth') AND type='V')
DROP VIEW dbo.VwRowAuth
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwRulAppItem') AND type='V')
DROP VIEW dbo.VwRulAppItem
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwScrButton') AND type='V')
DROP VIEW dbo.VwScrButton
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwScreenCriHlp') AND type='V')
DROP VIEW dbo.VwScreenCriHlp
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwScreenObjHlp') AND type='V')
DROP VIEW dbo.VwScreenObjHlp
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwServerRuleRunMode') AND type='V')
DROP VIEW dbo.VwServerRuleRunMode
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwTimeZoneInfo') AND type='V')
DROP VIEW dbo.VwTimeZoneInfo
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwUsr') AND type='V')
DROP VIEW dbo.VwUsr
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.VwUsrAUdit') AND type='V')
DROP VIEW dbo.VwUsrAUdit
GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AppInfo') AND type='U')
DROP TABLE dbo.AppInfo
CREATE TABLE AppInfo ( 
AppInfoId int IDENTITY(1,1) NOT NULL ,
AppInfoDesc varchar (50) NULL ,
VersionMa smallint NOT NULL ,
VersionMi smallint NOT NULL ,
VersionDt datetime NULL ,
AppZipId int NULL ,
Prerequisite nvarchar (max) NULL ,
Readme nvarchar (max) NULL ,
AppItemLink varchar (200) NULL ,
CultureTypeName varchar (10) NULL ,
VersionValue money NULL ,
CONSTRAINT PK_AppInfo PRIMARY KEY CLUSTERED (
AppInfoId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AppItem') AND type='U')
DROP TABLE dbo.AppItem
CREATE TABLE AppItem ( 
AppItemId int IDENTITY(1,1) NOT NULL ,
AppItemDesc varchar (200) NULL ,
AppInfoId int NOT NULL ,
ItemOrder smallint NULL ,
ObjectTypeCd char (1) NOT NULL ,
LanguageCd char (1) NULL ,
FrameworkCd char (1) NULL ,
DbProviderCd char (1) NULL ,
RelativePath varchar (100) NULL ,
AppItemName varchar (50) NOT NULL ,
MultiDesignDb char (1) NOT NULL ,
AppItemCode nvarchar (max) NULL ,
RemoveItem char (1) NOT NULL ,
ScreenId int NULL ,
ReportId int NULL ,
WizardId int NULL ,
CustomId int NULL ,
AppItemLink varchar (200) NULL ,
CONSTRAINT PK_AppItem PRIMARY KEY CLUSTERED (
AppItemId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AppLog') AND type='U')
DROP TABLE dbo.AppLog
CREATE TABLE AppLog ( 
AppLogId int IDENTITY(1,1) NOT NULL ,
AppLogDesc varchar (200) NOT NULL ,
AppLogNote ntext NOT NULL ,
InputBy int NOT NULL ,
InputOn datetime NOT NULL ,
CONSTRAINT PK_AppLog PRIMARY KEY CLUSTERED (
AppLogId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AppZipId') AND type='U')
DROP TABLE dbo.AppZipId
CREATE TABLE AppZipId ( 
DocId int IDENTITY(1,1) NOT NULL ,
MasterId int NOT NULL ,
DocName nvarchar (100) NOT NULL ,
MimeType varchar (100) NOT NULL ,
DocSize bigint NOT NULL ,
DocImage varbinary (max) NOT NULL ,
InputBy int NULL ,
InputOn datetime NULL ,
Active char (1) NOT NULL ,
CONSTRAINT PK_AppZipId PRIMARY KEY CLUSTERED (
DocId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtRowAuth') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtRowAuth') AND type='U')
DROP TABLE dbo.AtRowAuth
CREATE TABLE AtRowAuth ( 
RowAuthId smallint IDENTITY(10000,1) NOT NULL ,
RowAuthName varchar (50) NOT NULL ,
AllowSel char (1) NOT NULL ,
AllowAdd char (1) NOT NULL ,
AllowUpd char (1) NOT NULL ,
AllowDel char (1) NOT NULL ,
SysAdmin char (1) NOT NULL ,
OvrideId smallint NULL ,
CONSTRAINT PK_AtRowAuth PRIMARY KEY CLUSTERED (
RowAuthId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtRowAuthPrm') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtRowAuthPrm') AND type='U')
DROP TABLE dbo.AtRowAuthPrm
CREATE TABLE AtRowAuthPrm ( 
RowAuthPrmId smallint IDENTITY(10000,1) NOT NULL ,
RowAuthId smallint NOT NULL ,
PermKeyId smallint NOT NULL ,
SelLevel char (1) NOT NULL ,
CONSTRAINT PK_AtRowAuthPrm PRIMARY KEY CLUSTERED (
RowAuthPrmId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtServerRule') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtServerRule') AND type='U')
DROP TABLE dbo.AtServerRule
CREATE TABLE AtServerRule ( 
ServerRuleId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
RuleTypeId tinyint NOT NULL ,
MasterTable char (1) NOT NULL ,
RuleName nvarchar (100) NOT NULL ,
RuleDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
RuleOrder smallint NOT NULL ,
ProcedureName varchar (50) NOT NULL ,
ParameterNames varchar (max) NULL ,
ParameterTypes varchar (max) NULL ,
CallingParams varchar (max) NULL ,
OnAdd char (1) NOT NULL ,
OnUpd char (1) NOT NULL ,
OnDel char (1) NOT NULL ,
BeforeCRUD char (1) NOT NULL ,
RuleCode nvarchar (max) NULL ,
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
LastGenDt datetime NULL ,
Guid varchar (50) NOT NULL ,
RunMode char (1) NULL ,
SrcNS varchar (30) NULL ,
CONSTRAINT PK_AtServerRule PRIMARY KEY CLUSTERED (
ServerRuleId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtServerRuleOvrd') AND type='U')
DROP TABLE dbo.AtServerRuleOvrd
CREATE TABLE AtServerRuleOvrd ( 
AtServerRuleOvrdId int IDENTITY(1,1) NOT NULL ,
ServerRuleOvrdDesc varchar (1000) NULL ,
ServerRuleOvrdName varchar (500) NOT NULL ,
ServerRuleId int NOT NULL ,
Disable char (1) NOT NULL ,
ServerRuleGuid varchar (50) NULL ,
ScreenId int NULL ,
Priority smallint NULL ,
Guid varchar (50) NOT NULL CONSTRAINT DF_AtServerRuleOvrd_Guid DEFAULT (newid()),
RunMode char (1) NULL ,
CONSTRAINT PK_AtServerRuleOvrd PRIMARY KEY CLUSTERED (
AtServerRuleOvrdId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.AtServerRuleOvrdPrm') AND type='U')
DROP TABLE dbo.AtServerRuleOvrdPrm
CREATE TABLE AtServerRuleOvrdPrm ( 
ServerRuledOvrdPrmId int IDENTITY(1,1) NOT NULL ,
PermKeyId smallint NOT NULL ,
AndCondition char (1) NOT NULL ,
AtServerRuleOvrdId int NOT NULL CONSTRAINT DF_AtServerRuleOvrdPrm_AtServerRuleOvrdId DEFAULT ((-1)),
Match char (1) NOT NULL CONSTRAINT DF_AtServerRuleOvrdPrm_Match DEFAULT ('Y'),
PermKeyRowId int NULL ,
PermId int NULL ,
Guid varchar (50) NOT NULL CONSTRAINT DF_AtServerRuleOvrdPrm_Guid DEFAULT (newid()),
AtServerRuleOvrdGuid varchar (50) NULL ,
CONSTRAINT PK_AtServerRuleOvrdPrm PRIMARY KEY CLUSTERED (
ServerRuledOvrdPrmId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ButtonHlp') AND type='U')
DROP TABLE dbo.ButtonHlp
CREATE TABLE ButtonHlp ( 
ButtonHlpId int IDENTITY(1,1) NOT NULL ,
ScreenId int NULL ,
ReportId int NULL ,
WizardId int NULL ,
CultureId smallint NOT NULL ,
ButtonTypeId tinyint NOT NULL ,
ButtonName nvarchar (200) NULL ,
ButtonLongNm nvarchar (400) NULL ,
ButtonToolTip nvarchar (400) NULL ,
ButtonVisible char (1) NOT NULL ,
TopVisible char (1) NOT NULL CONSTRAINT DF_ButtonHlp_TopVisible DEFAULT ('N'),
RowVisible char (1) NOT NULL CONSTRAINT DF_ButtonHlp_RowVisible DEFAULT ('N'),
BotVisible char (1) NOT NULL CONSTRAINT DF_ButtonHlp_BotVisible DEFAULT ('N'),
CONSTRAINT PK_ButtonHlp PRIMARY KEY CLUSTERED (
ButtonHlpId,
CultureId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ClientRule') AND type='U')
DROP TABLE dbo.ClientRule
CREATE TABLE ClientRule ( 
ClientRuleId int IDENTITY(1,1) NOT NULL ,
RuleTypeId tinyint NOT NULL ,
RuleName nvarchar (100) NOT NULL ,
RuleDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
RuleMethodId tinyint NOT NULL CONSTRAINT DF_ClientRule_RuleMethodId DEFAULT ((1)),
ScreenId int NULL ,
ReportId int NULL ,
CultureId smallint NOT NULL ,
ScreenObjHlpId int NULL ,
ScreenCriHlpId int NULL ,
ReportCriHlpId int NULL ,
ClientScript smallint NULL ,
UserScriptEvent varchar (50) NULL ,
UserScriptName nvarchar (1000) NULL ,
ScriptParam nvarchar (500) NULL ,
RuleCntTypeId tinyint NULL ,
ClientRuleProg nvarchar (max) NULL ,
CONSTRAINT PK_ClientRule PRIMARY KEY CLUSTERED (
ClientRuleId,
CultureId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ClientTier') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ClientTier') AND type='U')
DROP TABLE dbo.ClientTier
CREATE TABLE ClientTier ( 
ClientTierId tinyint IDENTITY(1,1) NOT NULL ,
ClientTierName nvarchar (50) NOT NULL ,
EntityId int NOT NULL ,
LanguageCd char (1) NOT NULL ,
FrameworkCd char (1) NOT NULL ,
DevProgramPath varchar (100) NOT NULL ,
WsProgramPath varchar (100) NULL ,
XlsProgramPath varchar (100) NULL ,
DevCompilePath varchar (100) NULL ,
WsCompilePath varchar (100) NULL ,
XlsCompilePath varchar (100) NULL ,
CombineAsm char (1) NOT NULL CONSTRAINT DF_ClientTier_CombineAsm DEFAULT ('N'),
IsDefault char (1) NOT NULL ,
CONSTRAINT PK_ClientTier PRIMARY KEY CLUSTERED (
ClientTierId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ColOvrd') AND type='U')
DROP TABLE dbo.ColOvrd
CREATE TABLE ColOvrd ( 
ColOvrdId int IDENTITY(1,1) NOT NULL ,
ColOvrdDesc varchar (200) NULL ,
ScreenObjId int NOT NULL ,
ScreenId int NULL ,
ColVisible char (1) NOT NULL ,
ColReadOnly char (1) NOT NULL ,
ColExport char (1) NOT NULL ,
ToolTip nvarchar (200) NULL ,
ColumnHeader nvarchar (50) NULL ,
ErrMessage nvarchar (300) NULL ,
PermKeyId smallint NULL ,
PermId int NULL ,
Priority smallint NULL ,
PermKeyRowId int NULL ,
CONSTRAINT PK_ColOvrd PRIMARY KEY CLUSTERED (
ColOvrdId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CompPref') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CompPref') AND type='U')
DROP TABLE dbo.CompPref
CREATE TABLE CompPref ( 
CompPrefId int IDENTITY(1,1) NOT NULL ,
CompanyId int NOT NULL ,
CompanyLogo varchar (200) NULL ,
CompPrefDesc nvarchar (200) NULL ,
SystemLs varchar (1000) NULL ,
CONSTRAINT PK_CompPref PRIMARY KEY CLUSTERED (
CompPrefId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CronJob') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CronJob') AND type='U')
DROP TABLE dbo.CronJob
CREATE TABLE CronJob ( 
CronJobId int IDENTITY(1,1) NOT NULL ,
CronJobName nvarchar (200) NOT NULL ,
Year smallint NULL ,
Month tinyint NULL ,
Day tinyint NULL ,
Hour tinyint NULL ,
Minute tinyint NULL ,
DayOfWeek tinyint NULL ,
LastRun datetime NULL ,
NextRun datetime NULL ,
JobLink varchar (200) NOT NULL ,
LastStatus nvarchar (500) NULL ,
CONSTRAINT PK_CronJob PRIMARY KEY CLUSTERED (
CronJobId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtAccess') AND type='U')
DROP TABLE dbo.CtAccess
CREATE TABLE CtAccess ( 
AccessCd char (1) NOT NULL ,
AccessName nvarchar (10) NOT NULL ,
AccessSort tinyint NULL ,
CONSTRAINT PK_CtAccess PRIMARY KEY CLUSTERED (
AccessCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtAggregate') AND type='U')
DROP TABLE dbo.CtAggregate
CREATE TABLE CtAggregate ( 
AggregateCd char (1) NOT NULL ,
AggregateName nvarchar (10) NOT NULL ,
AggregateSort tinyint NULL ,
CONSTRAINT PK_CtAggregate PRIMARY KEY CLUSTERED (
AggregateCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtAlignment') AND type='U')
DROP TABLE dbo.CtAlignment
CREATE TABLE CtAlignment ( 
AlignmentCd char (1) NOT NULL ,
AlignmentName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtAlignment PRIMARY KEY CLUSTERED (
AlignmentCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtAndOr') AND type='U')
DROP TABLE dbo.CtAndOr
CREATE TABLE CtAndOr ( 
AndOrCd char (1) NOT NULL ,
AndOrName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtAndOr PRIMARY KEY CLUSTERED (
AndOrCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtBgGradType') AND type='U')
DROP TABLE dbo.CtBgGradType
CREATE TABLE CtBgGradType ( 
BgGradTypeId tinyint NOT NULL ,
BgGradTypeName varchar (20) NOT NULL ,
BgGradTypeDesc varchar (20) NOT NULL ,
CONSTRAINT PK_CtBgGradType PRIMARY KEY CLUSTERED (
BgGradTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtBorderStyle') AND type='U')
DROP TABLE dbo.CtBorderStyle
CREATE TABLE CtBorderStyle ( 
BorderStyleId tinyint NOT NULL ,
BorderStyleName nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtBorderStyle PRIMARY KEY CLUSTERED (
BorderStyleId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_CtButtonHlp' AND o.name = 'CtButtonHlp')
DROP INDEX CtButtonHlp.IU_CtButtonHlp 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtButtonHlp') AND type='U')
DROP TABLE dbo.CtButtonHlp
CREATE TABLE CtButtonHlp ( 
ButtonHlpId int IDENTITY(1,1) NOT NULL ,
CultureId smallint NOT NULL ,
ButtonTypeId tinyint NOT NULL ,
ButtonName nvarchar (50) NOT NULL ,
ButtonLongNm nvarchar (100) NOT NULL CONSTRAINT DF_CtButtonHlp_ButtonLongNm DEFAULT ('.'),
ButtonToolTip nvarchar (200) NOT NULL ,
CONSTRAINT PK_CtButtonHlp PRIMARY KEY CLUSTERED (
ButtonHlpId,
CultureId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtButtonStyle') AND type='U')
DROP TABLE dbo.CtButtonStyle
CREATE TABLE CtButtonStyle ( 
ButtonStyleCd char (1) NOT NULL ,
ButtonStyleDesc nvarchar (100) NOT NULL ,
CONSTRAINT PK_CtButtonStyle PRIMARY KEY CLUSTERED (
ButtonStyleCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtButtonType') AND type='U')
DROP TABLE dbo.CtButtonType
CREATE TABLE CtButtonType ( 
ButtonTypeId tinyint NOT NULL ,
ObjectType char (1) NOT NULL ,
ButtonTypeName varchar (20) NOT NULL ,
ButtonTypeDesc nvarchar (50) NOT NULL ,
ButtonVisible char (1) NOT NULL ,
ViewOnlyVisible char (1) NOT NULL CONSTRAINT DF_CtButtonType_ViewOnlyVisible DEFAULT ('Y'),
TopVisible char (1) NOT NULL CONSTRAINT DF_CtButtonType_TopVisible DEFAULT ('N'),
RowVisible char (1) NOT NULL CONSTRAINT DF_CtButtonType_RowVisible DEFAULT ('N'),
BotVisible char (1) NOT NULL CONSTRAINT DF_CtButtonType_BotVisible DEFAULT ('N'),
CONSTRAINT PK_CtButtonType PRIMARY KEY CLUSTERED (
ButtonTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCheckBox') AND type='U')
DROP TABLE dbo.CtCheckBox
CREATE TABLE CtCheckBox ( 
CultureId tinyint NOT NULL ,
CheckBoxCd char (1) NOT NULL ,
CheckBoxName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtCheckBox PRIMARY KEY CLUSTERED (
CultureId,
CheckBoxCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtClientScript') AND type='U')
DROP TABLE dbo.CtClientScript
CREATE TABLE CtClientScript ( 
ClientScriptId smallint NOT NULL ,
ClientScriptEvent varchar (50) NOT NULL ,
ClientScriptName varchar (50) NOT NULL ,
ClientScriptDesc nvarchar (100) NOT NULL ,
ClientScriptHelp nvarchar (1000) NULL ,
CONSTRAINT PK_CtClientScript PRIMARY KEY CLUSTERED (
ClientScriptId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCountry') AND type='U')
DROP TABLE dbo.CtCountry
CREATE TABLE CtCountry ( 
CountryCd char (3) NOT NULL ,
CountryName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtCountry PRIMARY KEY CLUSTERED (
CountryCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCrawler') AND type='U')
DROP TABLE dbo.CtCrawler
CREATE TABLE CtCrawler ( 
CrawlerCd char (2) NOT NULL ,
CrawlerName nvarchar (50) NOT NULL ,
CrawlerURL varchar (100) NOT NULL ,
CrawlerREF varchar (100) NOT NULL ,
PreText varchar (200) NOT NULL ,
PostText varchar (200) NOT NULL ,
ResultRegex varchar (100) NOT NULL ,
CONSTRAINT PK_CtCrawler PRIMARY KEY CLUSTERED (
CrawlerCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCrudType') AND type='U')
DROP TABLE dbo.CtCrudType
CREATE TABLE CtCrudType ( 
CrudTypeCd char (1) NOT NULL ,
CrudTypeName nvarchar (50) NOT NULL ,
CrudTypeDesc nvarchar (200) NOT NULL ,
CrudTypeSort tinyint NOT NULL ,
CONSTRAINT PK_CtCrudType PRIMARY KEY CLUSTERED (
CrudTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCudAction') AND type='U')
DROP TABLE dbo.CtCudAction
CREATE TABLE CtCudAction ( 
CudAction char (1) NOT NULL ,
CudActionDesc nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtCudAction PRIMARY KEY CLUSTERED (
CudAction
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_CtCulture' AND o.name = 'CtCulture')
DROP INDEX CtCulture.IX_CtCulture 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCulture') AND type='U')
DROP TABLE dbo.CtCulture
CREATE TABLE CtCulture ( 
CultureTypeId smallint IDENTITY(1,1) NOT NULL ,
CultureTypeName varchar (10) NOT NULL ,
CultureTypeDesc nvarchar (50) NOT NULL ,
CultureDefault char (1) NOT NULL ,
EnNumberRule char (1) NOT NULL ,
CountryCd char (3) NULL ,
CurrencyCd char (3) NULL ,
ToTranslate char (1) NOT NULL CONSTRAINT DF_CtCulture_ToTranslate DEFAULT ('N'),
ImportFileName nvarchar (30) NULL ,
CONSTRAINT PK_CtCulture PRIMARY KEY CLUSTERED (
CultureTypeId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_CtCultureLbl' AND o.name = 'CtCultureLbl')
DROP INDEX CtCultureLbl.IX_CtCultureLbl 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtCultureLbl') AND type='U')
DROP TABLE dbo.CtCultureLbl
CREATE TABLE CtCultureLbl ( 
CultureLblId int IDENTITY(1,1) NOT NULL ,
CultureTypeId smallint NOT NULL ,
CultureId smallint NOT NULL ,
CultureTypeLabel nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtCultureLbl PRIMARY KEY CLUSTERED (
CultureLblId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtDataType') AND type='U')
DROP TABLE dbo.CtDataType
CREATE TABLE CtDataType ( 
DataTypeId tinyint IDENTITY(1,1) NOT NULL ,
DataTypeName varchar (20) NOT NULL ,
DataTypeSqlName varchar (20) NOT NULL ,
DataTypeSysName varchar (20) NOT NULL ,
DataTypeSByteOle varchar (20) NOT NULL ,
DataTypeDByteOle varchar (20) NOT NULL ,
DataTypeRdlParm varchar (20) NULL ,
DataTypeDesc varchar (80) NOT NULL ,
NumericData char (1) NOT NULL ,
DoubleByte char (1) NOT NULL ,
DisplayModeId tinyint NULL ,
CONSTRAINT PK_CtDataType PRIMARY KEY CLUSTERED (
DataTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtDbProvider') AND type='U')
DROP TABLE dbo.CtDbProvider
CREATE TABLE CtDbProvider ( 
DbProviderCd char (1) NOT NULL ,
DbProviderName nvarchar (50) NOT NULL ,
DbProviderOle varchar (30) NOT NULL ,
CONSTRAINT PK_CtDbProvider PRIMARY KEY CLUSTERED (
DbProviderCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtDirection') AND type='U')
DROP TABLE dbo.CtDirection
CREATE TABLE CtDirection ( 
DirectionCd char (1) NOT NULL ,
DirectionName varchar (10) NOT NULL ,
DirectionDesc nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtDirection PRIMARY KEY CLUSTERED (
DirectionCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtDisplayType') AND type='U')
DROP TABLE dbo.CtDisplayType
CREATE TABLE CtDisplayType ( 
TypeId tinyint NOT NULL ,
TypeName varchar (20) NOT NULL ,
TypeDesc varchar (20) NOT NULL ,
DisplayDesc nvarchar (1000) NULL ,
DisplayDefault char (1) NOT NULL ,
CONSTRAINT PK_CtDisplayType PRIMARY KEY CLUSTERED (
TypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtDtlLstPos') AND type='U')
DROP TABLE dbo.CtDtlLstPos
CREATE TABLE CtDtlLstPos ( 
DtlLstPosId tinyint NOT NULL ,
DtlLstPosNm nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtDtlLstPos PRIMARY KEY CLUSTERED (
DtlLstPosId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_CtEvent' AND o.name = 'CtEvent')
DROP INDEX CtEvent.IX_CtEvent 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtEvent') AND type='U')
DROP TABLE dbo.CtEvent
CREATE TABLE CtEvent ( 
EventId tinyint NOT NULL ,
EventCode char (6) NOT NULL ,
EventDesc nvarchar (100) NOT NULL ,
CONSTRAINT PK_CtEvent PRIMARY KEY CLUSTERED (
EventId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtEveryone') AND type='U')
DROP TABLE dbo.CtEveryone
CREATE TABLE CtEveryone ( 
EveryoneCd char (1) NOT NULL ,
EveryoneName nvarchar (20) NOT NULL ,
EveryoneSort tinyint NOT NULL ,
CONSTRAINT PK_CtEveryone PRIMARY KEY CLUSTERED (
EveryoneCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtFontStyle') AND type='U')
DROP TABLE dbo.CtFontStyle
CREATE TABLE CtFontStyle ( 
FontStyleCd char (1) NOT NULL ,
FontStyleName varchar (10) NOT NULL ,
CONSTRAINT PK_CtFontStyle PRIMARY KEY CLUSTERED (
FontStyleCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtFontWeight') AND type='U')
DROP TABLE dbo.CtFontWeight
CREATE TABLE CtFontWeight ( 
FontWeightCd tinyint NOT NULL ,
FontWeightName varchar (10) NOT NULL ,
FontWeightDesc nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtFontWeight PRIMARY KEY CLUSTERED (
FontWeightCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtFormat') AND type='U')
DROP TABLE dbo.CtFormat
CREATE TABLE CtFormat ( 
FormatCd char (1) NOT NULL ,
FormatName nvarchar (10) NOT NULL ,
FormatSort tinyint NULL ,
CONSTRAINT PK_CtFormat PRIMARY KEY CLUSTERED (
FormatCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtFramework') AND type='U')
DROP TABLE dbo.CtFramework
CREATE TABLE CtFramework ( 
FrameworkCd char (1) NOT NULL ,
FrameworkName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtFramework PRIMARY KEY CLUSTERED (
FrameworkCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtGridGrp') AND type='U')
DROP TABLE dbo.CtGridGrp
CREATE TABLE CtGridGrp ( 
GridGrpCd char (1) NOT NULL ,
GridGrpName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtGridGrp PRIMARY KEY CLUSTERED (
GridGrpCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtHintQuestion') AND type='U')
DROP TABLE dbo.CtHintQuestion
CREATE TABLE CtHintQuestion ( 
HintQuestionId tinyint NOT NULL ,
HintQuestionDesc nvarchar (100) NOT NULL ,
CONSTRAINT PK_CtHintQuestion PRIMARY KEY CLUSTERED (
HintQuestionId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtJustify') AND type='U')
DROP TABLE dbo.CtJustify
CREATE TABLE CtJustify ( 
JustifyCd char (1) NOT NULL ,
JustifyName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtJustify PRIMARY KEY CLUSTERED (
JustifyCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtLanguage') AND type='U')
DROP TABLE dbo.CtLanguage
CREATE TABLE CtLanguage ( 
LanguageCd char (1) NOT NULL ,
LanguageName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtLanguage PRIMARY KEY CLUSTERED (
LanguageCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtLineType') AND type='U')
DROP TABLE dbo.CtLineType
CREATE TABLE CtLineType ( 
LineTypeCd char (1) NOT NULL ,
LineTypeName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtLineType PRIMARY KEY CLUSTERED (
LineTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtLinkType') AND type='U')
DROP TABLE dbo.CtLinkType
CREATE TABLE CtLinkType ( 
LinkTypeCd char (3) NOT NULL ,
LinkTypeName nvarchar (50) NOT NULL ,
LinkTypeDesc nvarchar (500) NULL ,
UsrDefined char (1) NOT NULL CONSTRAINT DF_CtLinkType_UsrDefined DEFAULT ('N'),
MobileCd char (1) NOT NULL CONSTRAINT DF_CtLinkType_MobileCd DEFAULT ('S'),
OneMstOnly char (1) NOT NULL ,
OneDtlOnly char (1) NOT NULL ,
CONSTRAINT PK_CtLinkType PRIMARY KEY CLUSTERED (
LinkTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtMatch') AND type='U')
DROP TABLE dbo.CtMatch
CREATE TABLE CtMatch ( 
MatchCd char (1) NOT NULL ,
MatchName nvarchar (50) NOT NULL ,
MatchSort tinyint NULL ,
CONSTRAINT PK_CtMatch PRIMARY KEY CLUSTERED (
MatchCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtMsgType') AND type='U')
DROP TABLE dbo.CtMsgType
CREATE TABLE CtMsgType ( 
MsgTypeCd char (1) NOT NULL ,
MsgTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtMsgType PRIMARY KEY CLUSTERED (
MsgTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtObjectType') AND type='U')
DROP TABLE dbo.CtObjectType
CREATE TABLE CtObjectType ( 
ObjectTypeCd char (1) NOT NULL ,
ObjectTypeName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtObject PRIMARY KEY CLUSTERED (
ObjectTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtOperator') AND type='U')
DROP TABLE dbo.CtOperator
CREATE TABLE CtOperator ( 
OperatorId tinyint NOT NULL ,
OperatorName varchar (10) NOT NULL ,
OperatorDesc nvarchar (30) NULL ,
CONSTRAINT PK_CtOperator PRIMARY KEY CLUSTERED (
OperatorId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtOrientation') AND type='U')
DROP TABLE dbo.CtOrientation
CREATE TABLE CtOrientation ( 
OrientationCd char (1) NOT NULL ,
OrientationName nvarchar (30) NOT NULL ,
CONSTRAINT PK_CtOrientation PRIMARY KEY CLUSTERED (
OrientationCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtOsType') AND type='U')
DROP TABLE dbo.CtOsType
CREATE TABLE CtOsType ( 
OsTypeCd char (1) NOT NULL ,
OsTypeName nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtOsType PRIMARY KEY CLUSTERED (
OsTypeCd
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_CtPermKey' AND o.name = 'CtPermKey')
DROP INDEX CtPermKey.IX_CtPermKey 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtPermKey') AND type='U')
DROP TABLE dbo.CtPermKey
CREATE TABLE CtPermKey ( 
CtPermKeyId smallint IDENTITY(1,1) NOT NULL ,
CtPermKeyName varchar (50) NOT NULL ,
CtPermKeyParam varchar (20) NOT NULL ,
TableName varchar (50) NOT NULL ,
ColumnName varchar (50) NOT NULL ,
AndCondition char (1) NOT NULL ,
MultiValue char (1) NOT NULL CONSTRAINT DF_CtPermKey_MultiValue DEFAULT ('N'),
CONSTRAINT PK_CtPermKey PRIMARY KEY CLUSTERED (
CtPermKeyId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtReleaseType') AND type='U')
DROP TABLE dbo.CtReleaseType
CREATE TABLE CtReleaseType ( 
ReleaseTypeId tinyint NOT NULL ,
ReleaseTypeAbbr varchar (4) NOT NULL ,
ReleaseTypeName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtReleaseType PRIMARY KEY CLUSTERED (
ReleaseTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtReportSct') AND type='U')
DROP TABLE dbo.CtReportSct
CREATE TABLE CtReportSct ( 
ReportSctId tinyint NOT NULL ,
ReportSctName varchar (10) NOT NULL ,
ReportSctDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtReportSct PRIMARY KEY CLUSTERED (
ReportSctId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtReportType') AND type='U')
DROP TABLE dbo.CtReportType
CREATE TABLE CtReportType ( 
ReportTypeCd char (1) NOT NULL ,
ReportTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtReportType PRIMARY KEY CLUSTERED (
ReportTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRowAuth') AND type='U')
DROP TABLE dbo.CtRowAuth
CREATE TABLE CtRowAuth ( 
RowAuthId smallint IDENTITY(1,1) NOT NULL ,
RowAuthName varchar (50) NOT NULL ,
AllowSel char (1) NOT NULL ,
AllowAdd char (1) NOT NULL ,
AllowUpd char (1) NOT NULL ,
AllowDel char (1) NOT NULL ,
SysAdmin char (1) NOT NULL ,
OvrideId smallint NULL ,
CONSTRAINT PK_CtRowAuth PRIMARY KEY CLUSTERED (
RowAuthId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRowAuthPrm') AND type='U')
DROP TABLE dbo.CtRowAuthPrm
CREATE TABLE CtRowAuthPrm ( 
RowAuthPrmId smallint IDENTITY(1,1) NOT NULL ,
RowAuthId smallint NOT NULL ,
PermKeyId smallint NOT NULL ,
SelLevel char (1) NOT NULL ,
CONSTRAINT PK_CtRowAuthPrm PRIMARY KEY CLUSTERED (
RowAuthPrmId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptChart') AND type='U')
DROP TABLE dbo.CtRptChart
CREATE TABLE CtRptChart ( 
RptChartCd char (1) NOT NULL ,
RptChartName nvarchar (100) NOT NULL ,
CONSTRAINT PK_CtRptChart PRIMARY KEY CLUSTERED (
RptChartCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptChaType') AND type='U')
DROP TABLE dbo.CtRptChaType
CREATE TABLE CtRptChaType ( 
RptChaTypeCd char (2) NOT NULL ,
RptChaTypeName nvarchar (30) NOT NULL ,
CONSTRAINT PK_CtRptChaType PRIMARY KEY CLUSTERED (
RptChaTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptCtrType') AND type='U')
DROP TABLE dbo.CtRptCtrType
CREATE TABLE CtRptCtrType ( 
RptCtrTypeCd char (1) NOT NULL ,
RptCtrTypeName nvarchar (20) NOT NULL ,
RptCtrTypeDesc nvarchar (400) NULL ,
HasChildCtr char (1) NOT NULL CONSTRAINT DF_CtRptCtrType_HasChildCtr DEFAULT ('N'),
CONSTRAINT PK_CtRptCtrType PRIMARY KEY CLUSTERED (
RptCtrTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptElmType') AND type='U')
DROP TABLE dbo.CtRptElmType
CREATE TABLE CtRptElmType ( 
RptElmTypeCd char (1) NOT NULL ,
RptElmTypeName nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtRptElmType PRIMARY KEY CLUSTERED (
RptElmTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptGroup') AND type='U')
DROP TABLE dbo.CtRptGroup
CREATE TABLE CtRptGroup ( 
RptGroupId tinyint NOT NULL ,
RptGroupName nvarchar (100) NOT NULL ,
CONSTRAINT PK_CtRptGroup PRIMARY KEY CLUSTERED (
RptGroupId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptObjType') AND type='U')
DROP TABLE dbo.CtRptObjType
CREATE TABLE CtRptObjType ( 
RptObjTypeCd char (1) NOT NULL ,
RptObjTypeName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtRptObjType PRIMARY KEY CLUSTERED (
RptObjTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptStyDef') AND type='U')
DROP TABLE dbo.CtRptStyDef
CREATE TABLE CtRptStyDef ( 
DefaultCd char (2) NOT NULL ,
DefaultName nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtRptStyDef PRIMARY KEY CLUSTERED (
DefaultCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRptTblType') AND type='U')
DROP TABLE dbo.CtRptTblType
CREATE TABLE CtRptTblType ( 
RptTblTypeCd char (1) NOT NULL ,
RptTblTypeName nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtRptTblType PRIMARY KEY CLUSTERED (
RptTblTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleAsmxType') AND type='U')
DROP TABLE dbo.CtRuleAsmxType
CREATE TABLE CtRuleAsmxType ( 
RuleAsmxTypeId tinyint NOT NULL ,
RuleAsmxTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtRuleAsmxType PRIMARY KEY CLUSTERED (
RuleAsmxTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleCntType') AND type='U')
DROP TABLE dbo.CtRuleCntType
CREATE TABLE CtRuleCntType ( 
RuleCntTypeId tinyint NOT NULL ,
RuleCntTypeName nvarchar (50) NOT NULL ,
RuleCntTypeEvent varchar (20) NOT NULL ,
RuleCntTypeDesc nvarchar (1000) NOT NULL ,
CONSTRAINT PK_CtRuleCntType PRIMARY KEY CLUSTERED (
RuleCntTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleLayer') AND type='U')
DROP TABLE dbo.CtRuleLayer
CREATE TABLE CtRuleLayer ( 
RuleLayerCd char (1) NOT NULL ,
RuleLayerDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtRuleLayer PRIMARY KEY CLUSTERED (
RuleLayerCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleMethod') AND type='U')
DROP TABLE dbo.CtRuleMethod
CREATE TABLE CtRuleMethod ( 
RuleMethodId tinyint NOT NULL ,
RuleMethodName nvarchar (50) NOT NULL ,
RuleMethodDesc nvarchar (1000) NOT NULL ,
CONSTRAINT PK_CtRuleMethod PRIMARY KEY CLUSTERED (
RuleMethodId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleReactType') AND type='U')
DROP TABLE dbo.CtRuleReactType
CREATE TABLE CtRuleReactType ( 
RuleReactTypeId tinyint NOT NULL ,
RuleReactTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtRuleReactType PRIMARY KEY CLUSTERED (
RuleReactTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleReduxType') AND type='U')
DROP TABLE dbo.CtRuleReduxType
CREATE TABLE CtRuleReduxType ( 
RuleReduxTypeId tinyint NOT NULL ,
RuleReduxTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtRuleReduxType PRIMARY KEY CLUSTERED (
RuleReduxTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleServiceType') AND type='U')
DROP TABLE dbo.CtRuleServiceType
CREATE TABLE CtRuleServiceType ( 
RuleServiceTypeId tinyint NOT NULL ,
RuleServiceTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtRuleServiceType PRIMARY KEY CLUSTERED (
RuleServiceTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtRuleType') AND type='U')
DROP TABLE dbo.CtRuleType
CREATE TABLE CtRuleType ( 
RuleTypeId tinyint NOT NULL ,
RuleTypeDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtRuleType PRIMARY KEY CLUSTERED (
RuleTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtScreenType') AND type='U')
DROP TABLE dbo.CtScreenType
CREATE TABLE CtScreenType ( 
ScreenTypeId tinyint IDENTITY(1,1) NOT NULL ,
ScreenTypeName char (2) NOT NULL ,
ScreenTypeDesc varchar (50) NOT NULL ,
CONSTRAINT PK_CtScreenType PRIMARY KEY CLUSTERED (
ScreenTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtSection') AND type='U')
DROP TABLE dbo.CtSection
CREATE TABLE CtSection ( 
SectionCd char (1) NOT NULL ,
SectionName nvarchar (200) NOT NULL ,
NeedRegen char (1) NOT NULL CONSTRAINT DF_CtSection_NeedRegen DEFAULT ('N'),
CONSTRAINT PK_CtSection PRIMARY KEY CLUSTERED (
SectionCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtSelectType') AND type='U')
DROP TABLE dbo.CtSelectType
CREATE TABLE CtSelectType ( 
SelectTypeCd char (1) NOT NULL ,
SelectTypeName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtSelectType PRIMARY KEY CLUSTERED (
SelectTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtSProcOnly') AND type='U')
DROP TABLE dbo.CtSProcOnly
CREATE TABLE CtSProcOnly ( 
SProcOnlyCd char (1) NOT NULL ,
SProcOnlyName nvarchar (50) NOT NULL ,
CONSTRAINT PK_CtSProcOnly PRIMARY KEY CLUSTERED (
SProcOnlyCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtSysListVisible') AND type='U')
DROP TABLE dbo.CtSysListVisible
CREATE TABLE CtSysListVisible ( 
SysListVisibleCode char (1) NOT NULL ,
SysListVisibleDesc nvarchar (50) NOT NULL ,
CONSTRAINT PK_UsrPrefSysVis PRIMARY KEY CLUSTERED (
SysListVisibleCode
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtTextAlign') AND type='U')
DROP TABLE dbo.CtTextAlign
CREATE TABLE CtTextAlign ( 
TextAlignCd char (1) NOT NULL ,
TextAlignName varchar (20) NOT NULL ,
CONSTRAINT PK_CtTextAlign PRIMARY KEY CLUSTERED (
TextAlignCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtTextDecor') AND type='U')
DROP TABLE dbo.CtTextDecor
CREATE TABLE CtTextDecor ( 
TextDecorCd char (1) NOT NULL ,
TextDecorName varchar (20) NOT NULL ,
TextDecorDesc nvarchar (20) NOT NULL ,
CONSTRAINT PK_CtTextDecor PRIMARY KEY CLUSTERED (
TextDecorCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtTierType') AND type='U')
DROP TABLE dbo.CtTierType
CREATE TABLE CtTierType ( 
TierTypeCd char (1) NOT NULL ,
TierTypeName nvarchar (10) NOT NULL ,
CONSTRAINT PK_CtTierType PRIMARY KEY CLUSTERED (
TierTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtUnit') AND type='U')
DROP TABLE dbo.CtUnit
CREATE TABLE CtUnit ( 
UnitCd char (2) NOT NULL ,
UnitDesc nvarchar (50) NOT NULL ,
UnitRef nvarchar (100) NULL ,
UnitOrder tinyint NOT NULL ,
CONSTRAINT PK_CtUnit PRIMARY KEY CLUSTERED (
UnitCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtVerticalAlign') AND type='U')
DROP TABLE dbo.CtVerticalAlign
CREATE TABLE CtVerticalAlign ( 
VerticalAlignCd char (1) NOT NULL ,
VerticalAlignName varchar (20) NOT NULL ,
CONSTRAINT PK_CtVerticalAlign PRIMARY KEY CLUSTERED (
VerticalAlignCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtViewType') AND type='U')
DROP TABLE dbo.CtViewType
CREATE TABLE CtViewType ( 
ViewTypeCd char (1) NOT NULL ,
ViewTypeName nvarchar (30) NOT NULL ,
ViewTypeSort tinyint NULL ,
CONSTRAINT PK_CtViewType PRIMARY KEY CLUSTERED (
ViewTypeCd
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtWizardType') AND type='U')
DROP TABLE dbo.CtWizardType
CREATE TABLE CtWizardType ( 
WizardTypeId tinyint IDENTITY(1,1) NOT NULL ,
WizardTypeName char (2) NOT NULL ,
WizardTypeDesc varchar (50) NOT NULL ,
CONSTRAINT PK_CtWizardType PRIMARY KEY CLUSTERED (
WizardTypeId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.CtWritingMode') AND type='U')
DROP TABLE dbo.CtWritingMode
CREATE TABLE CtWritingMode ( 
WritingModeCd char (1) NOT NULL ,
WritingModeName varchar (10) NOT NULL ,
WritingModeDesc varchar (20) NOT NULL ,
CONSTRAINT PK_CtWritingMode PRIMARY KEY CLUSTERED (
WritingModeCd
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.DataTier') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.DataTier') AND type='U')
DROP TABLE dbo.DataTier
CREATE TABLE DataTier ( 
DataTierId tinyint IDENTITY(1,1) NOT NULL ,
DataTierName nvarchar (50) NOT NULL ,
EntityId int NOT NULL ,
DbProviderCd char (1) NOT NULL ,
ServerName varchar (20) NOT NULL ,
DesServer varchar (20) NOT NULL ,
DesDatabase varchar (20) NOT NULL ,
DesUserId varchar (50) NOT NULL ,
DesPassword varchar (50) NOT NULL ,
PortBinPath varchar (100) NOT NULL ,
InstBinPath varchar (100) NOT NULL ,
ScriptPath varchar (100) NOT NULL ,
DbDataPath varchar (100) NOT NULL ,
IsDevelopment char (1) NOT NULL ,
IsDefault char (1) NOT NULL ,
CONSTRAINT PK_DataTier PRIMARY KEY CLUSTERED (
DataTierId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.DbColumn') AND type='U')
DROP TABLE dbo.DbColumn
CREATE TABLE DbColumn ( 
ColumnId int IDENTITY(1,1) NOT NULL ,
TableId int NOT NULL ,
ColumnIndex smallint NULL ,
ExternalTable varchar (50) NULL ,
ColumnName varchar (50) NOT NULL ,
ColumnDesc varchar (100) NULL ,
DataType tinyint NOT NULL ,
ColumnLength smallint NOT NULL ,
ColumnScale tinyint NULL ,
DefaultValue nvarchar (50) NULL ,
AllowNulls char (1) NOT NULL ,
ColumnIdentity char (1) NOT NULL ,
PrimaryKey char (1) NOT NULL ,
IsIndexU char (1) NOT NULL CONSTRAINT DF_DbColumn_IsIndexU DEFAULT ('N'),
IsIndex char (1) NOT NULL CONSTRAINT DF_DbColumn_IsIndex DEFAULT ('N'),
ColObjective nvarchar (200) NULL ,
PrevColName varchar (20) NULL ,
CONSTRAINT PK_DbColumn PRIMARY KEY CLUSTERED (
ColumnId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_DbKey_KeyName' AND o.name = 'DbKey')
DROP INDEX DbKey.IX_DbKey_KeyName 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.DbKey') AND type='U')
DROP TABLE dbo.DbKey
CREATE TABLE DbKey ( 
KeyId int IDENTITY(1,1) NOT NULL ,
KeyName varchar (50) NOT NULL ,
TableId int NOT NULL ,
ColumnId int NOT NULL ,
RefTableId int NOT NULL ,
RefColumnId int NOT NULL ,
CONSTRAINT PK_DbKey PRIMARY KEY CLUSTERED (
KeyId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_DbTable' AND o.name = 'DbTable')
DROP INDEX DbTable.IU_DbTable 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.DbTable') AND type='U')
DROP TABLE dbo.DbTable
CREATE TABLE DbTable ( 
TableId int IDENTITY(1,1) NOT NULL ,
SystemId tinyint NOT NULL ,
TableName varchar (500) NOT NULL ,
TableDesc nvarchar (100) NOT NULL ,
MultiDesignDb char (1) NOT NULL ,
VirtualTbl char (1) NOT NULL CONSTRAINT DF_DbTable_VirtualTbl DEFAULT ('N'),
VirtualSql nvarchar (max) NULL ,
TblObjective nvarchar (500) NULL ,
PrevTblName varchar (20) NULL ,
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
LastSyncDt datetime NULL ,
CONSTRAINT PK_DbTable PRIMARY KEY CLUSTERED (
TableId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Deleted') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Deleted') AND type='U')
DROP TABLE dbo.Deleted
CREATE TABLE Deleted ( 
DbName varchar (50) NOT NULL ,
TableName varchar (50) NOT NULL ,
PKeyId varchar (100) NOT NULL ,
DContent nvarchar (max) NULL ,
DeletedBy int NOT NULL ,
DeletedOn datetime NOT NULL 
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Document') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Document') AND type='U')
DROP TABLE dbo.Document
CREATE TABLE Document ( 
DocumentId int IDENTITY(1,1) NOT NULL ,
DocuTblName varchar (50) NOT NULL ,
MenuTblName varchar (50) NOT NULL ,
HelpTblName varchar (50) NOT NULL ,
DocuFilePath varchar (100) NOT NULL ,
DocumentTitle nvarchar (50) NOT NULL ,
ProgramName varchar (50) NOT NULL ,
DocumentNotes varchar (4000) NULL ,
CONSTRAINT PK_Document PRIMARY KEY CLUSTERED (
DocumentId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Entity') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Entity') AND type='U')
DROP TABLE dbo.Entity
CREATE TABLE Entity ( 
EntityId smallint NOT NULL ,
EntityImg varbinary (max) NULL ,
EntityName nvarchar (100) NOT NULL ,
EntityCode varchar (10) NOT NULL ,
DeployPath varchar (100) NOT NULL ,
CONSTRAINT PK_Entity PRIMARY KEY CLUSTERED (
EntityId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Flowchart') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Flowchart') AND type='U')
DROP TABLE dbo.Flowchart
CREATE TABLE Flowchart ( 
ChartId int IDENTITY(1,1) NOT NULL ,
ChartName nvarchar (500) NOT NULL ,
ChartDesc nvarchar (max) NULL ,
ChartData nvarchar (max) NULL ,
CompanyId int NULL ,
CompanyDefault char (1) NOT NULL CONSTRAINT DF_Flowchart_CompanyDefault DEFAULT ('N'),
CONSTRAINT PK_Flowchart PRIMARY KEY CLUSTERED (
ChartId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.GlobalFilter') AND type='U')
DROP TABLE dbo.GlobalFilter
CREATE TABLE GlobalFilter ( 
UsrId int NOT NULL ,
ScreenId int NOT NULL ,
FilterClause varchar (1800) NOT NULL ,
FilterDesc ntext NOT NULL ,
FilterDefault char (1) NOT NULL ,
CONSTRAINT PK_GlobalFilter PRIMARY KEY CLUSTERED (
UsrId,
ScreenId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.GlobalFilterKey') AND type='U')
DROP TABLE dbo.GlobalFilterKey
CREATE TABLE GlobalFilterKey ( 
UsrId int NOT NULL ,
ScreenId int NOT NULL ,
GlobalFilterKey bigint NOT NULL ,
CONSTRAINT PK_GlobalFilterKey PRIMARY KEY CLUSTERED (
UsrId,
ScreenId,
GlobalFilterKey
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.GroupCol') AND type='U')
DROP TABLE dbo.GroupCol
CREATE TABLE GroupCol ( 
GroupColId smallint IDENTITY(1,1) NOT NULL ,
ColCssClass varchar (20) NULL ,
GroupColName nvarchar (50) NOT NULL ,
CONSTRAINT PK_GroupCol PRIMARY KEY CLUSTERED (
GroupColId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.GroupRow') AND type='U')
DROP TABLE dbo.GroupRow
CREATE TABLE GroupRow ( 
GroupRowId smallint IDENTITY(1,1) NOT NULL ,
RowCssClass varchar (20) NOT NULL ,
GroupRowName nvarchar (50) NOT NULL ,
CONSTRAINT PK_GroupRow PRIMARY KEY CLUSTERED (
GroupRowId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_Label' AND o.name = 'Label')
DROP INDEX Label.IX_Label 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Label') AND type='U')
DROP TABLE dbo.Label
CREATE TABLE Label ( 
LabelId int IDENTITY(1,1) NOT NULL ,
LabelDesc varchar (200) NULL ,
CultureId smallint NOT NULL ,
LabelCat varchar (50) NOT NULL CONSTRAINT DF_Label_LabelCat DEFAULT ('NA'),
LabelKey varchar (50) NOT NULL ,
LabelText nvarchar (max) NOT NULL ,
CompanyId int NULL ,
SortOrder smallint NULL ,
CONSTRAINT PK_Label PRIMARY KEY CLUSTERED (
LabelId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.LastEmail') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.LastEmail') AND type='U')
DROP TABLE dbo.LastEmail
CREATE TABLE LastEmail ( 
LastEmailDt datetime NOT NULL ,
LastEmailCnt int NOT NULL ,
CONSTRAINT PK_LastEmail PRIMARY KEY CLUSTERED (
LastEmailDt
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MaintMsg') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MaintMsg') AND type='U')
DROP TABLE dbo.MaintMsg
CREATE TABLE MaintMsg ( 
MaintMsgId smallint IDENTITY(1,1) NOT NULL ,
MaintMsgName nvarchar (200) NOT NULL ,
MaintMessage nvarchar (2000) NOT NULL ,
ShowOnLogin char (1) NOT NULL ,
LastEmailDt datetime NULL ,
CONSTRAINT PK_MaintMsg PRIMARY KEY CLUSTERED (
MaintMsgId
)
)
END

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_MemTrans' AND o.name = 'MemTrans')
DROP INDEX MemTrans.IU_MemTrans 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MemTrans') AND type='U')
DROP TABLE dbo.MemTrans
CREATE TABLE MemTrans ( 
MemTransId int IDENTITY(1,1) NOT NULL ,
InStr nvarchar (2000) NOT NULL ,
CultureTypeId smallint NOT NULL ,
OutStr nvarchar (2000) NOT NULL ,
CONSTRAINT PK_MemTrans PRIMARY KEY CLUSTERED (
MemTransId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Menu') AND type='U')
DROP TABLE dbo.Menu
CREATE TABLE Menu ( 
MenuId int IDENTITY(1,1) NOT NULL ,
MenuIndex smallint NOT NULL ,
Miscellaneous varchar (1000) NULL ,
ParentId int NULL ,
ReportId int NULL ,
ScreenId int NULL ,
WizardId int NULL ,
StaticPgId int NULL ,
IconUrl nvarchar (300) NULL ,
Popup char (1) NOT NULL CONSTRAINT DF_Menu_Popup DEFAULT ('N'),
ReactQuickMenu char (1) NOT NULL CONSTRAINT DF_Menu_ReactQuickMenu DEFAULT ('N'),
CONSTRAINT PK_Menu PRIMARY KEY CLUSTERED (
MenuId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_MenuHlp_MenuId' AND o.name = 'MenuHlp')
DROP INDEX MenuHlp.IX_MenuHlp_MenuId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MenuHlp') AND type='U')
DROP TABLE dbo.MenuHlp
CREATE TABLE MenuHlp ( 
MenuHlpId int IDENTITY(1,1) NOT NULL ,
MenuId int NOT NULL ,
CultureId smallint NOT NULL ,
MenuText nvarchar (50) NOT NULL ,
CONSTRAINT PK_MenuHlp PRIMARY KEY CLUSTERED (
MenuHlpId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MenuPrm') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MenuPrm') AND type='U')
DROP TABLE dbo.MenuPrm
CREATE TABLE MenuPrm ( 
MenuPrmId int IDENTITY(1,1) NOT NULL ,
MenuId int NOT NULL ,
GrantDeny char (1) NOT NULL ,
PermKeyId smallint NOT NULL ,
PermId int NOT NULL ,
CONSTRAINT PK_MenuPrm PRIMARY KEY CLUSTERED (
MenuPrmId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Msg') AND type='U')
DROP TABLE dbo.Msg
CREATE TABLE Msg ( 
MsgId int IDENTITY(1,1) NOT NULL ,
MsgTypeCd char (1) NOT NULL ,
MsgSource varchar (50) NOT NULL ,
MsgName nvarchar (500) NOT NULL ,
CONSTRAINT PK_Msg PRIMARY KEY CLUSTERED (
MsgId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_MsgCenter' AND o.name = 'MsgCenter')
DROP INDEX MsgCenter.IX_MsgCenter 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MsgCenter') AND type='U')
DROP TABLE dbo.MsgCenter
CREATE TABLE MsgCenter ( 
MsgCenterId int IDENTITY(1,1) NOT NULL ,
MsgId int NOT NULL ,
CultureId smallint NOT NULL ,
Msg nvarchar (800) NOT NULL ,
CONSTRAINT PK_MsgCenter PRIMARY KEY CLUSTERED (
MsgCenterId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.MyMenuOpt') AND type='U')
DROP TABLE dbo.MyMenuOpt
CREATE TABLE MyMenuOpt ( 
MenuOptId int IDENTITY(1,1) NOT NULL ,
MenuOptName nvarchar (100) NOT NULL ,
MenuOptDesc nvarchar (400) NULL ,
MenuOptIco nvarchar (300) NULL ,
MenuOptImg nvarchar (300) NULL ,
TopMenuCss varchar (max) NULL ,
TopMenuJs varchar (max) NULL ,
TopMenuIvk varchar (max) NULL ,
SidMenuCss varchar (max) NULL ,
SidMenuJs varchar (max) NULL ,
SidMenuIvk varchar (max) NULL ,
OvaRating tinyint NULL ,
InputBy int NULL ,
InputOn datetime NULL ,
ProjectId bigint NULL ,
CONSTRAINT PK_MyMenuOpt PRIMARY KEY CLUSTERED (
MenuOptId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_Num2Word_Digits' AND o.name = 'Num2Word')
DROP INDEX Num2Word.IX_Num2Word_Digits 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Num2Word') AND type='U')
DROP TABLE dbo.Num2Word
CREATE TABLE Num2Word ( 
Num2WordId int IDENTITY(1,1) NOT NULL ,
CultureId tinyint NOT NULL ,
Digit10 tinyint NULL ,
Digit09 tinyint NULL ,
Digit08 tinyint NULL ,
Digit07 tinyint NULL ,
Digit06 tinyint NULL ,
Digit05 tinyint NULL ,
Digit04 tinyint NULL ,
Digit03 tinyint NULL ,
Digit02 tinyint NULL ,
Digit01 tinyint NULL ,
Word nvarchar (50) NOT NULL ,
CONSTRAINT PK_Num2Word PRIMARY KEY CLUSTERED (
Num2WordId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Ovride') AND type='U')
DROP TABLE dbo.Ovride
CREATE TABLE Ovride ( 
OvrideId smallint IDENTITY(1,1) NOT NULL ,
OvrideName nvarchar (50) NOT NULL ,
PromptAlways char (1) NOT NULL ,
PromptModal char (1) NOT NULL ,
CONSTRAINT PK_Ovride PRIMARY KEY CLUSTERED (
OvrideId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.OvrideGrp') AND type='U')
DROP TABLE dbo.OvrideGrp
CREATE TABLE OvrideGrp ( 
OvrideGrpId int IDENTITY(1,1) NOT NULL ,
OvrideId smallint NOT NULL ,
UsrGroupId smallint NOT NULL ,
CONSTRAINT PK_OvrideGrp PRIMARY KEY CLUSTERED (
OvrideGrpId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PageLnk') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PageLnk') AND type='U')
DROP TABLE dbo.PageLnk
CREATE TABLE PageLnk ( 
PageLnkId int IDENTITY(1,1) NOT NULL ,
PageObjId smallint NOT NULL ,
PageLnkCss varchar (1000) NULL ,
Popup char (1) NULL ,
PageLnkRef varchar (1000) NULL ,
PageLnkTxt nvarchar (4000) NULL ,
PageLnkImg varchar (1000) NULL ,
PageLnkAlt varchar (1000) NULL ,
PageLnkOrd smallint NOT NULL ,
CONSTRAINT PK_PageLnk PRIMARY KEY CLUSTERED (
PageLnkId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PageObj') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.PageObj') AND type='U')
DROP TABLE dbo.PageObj
CREATE TABLE PageObj ( 
PageObjId int IDENTITY(1,1) NOT NULL ,
PageObjDesc nvarchar (200) NULL ,
SectionCd char (1) NOT NULL ,
GroupRowId smallint NULL ,
GroupColId smallint NULL ,
PageObjCss varchar (1000) NULL ,
PageObjSrp varchar (1000) NULL ,
PageObjOrd smallint NOT NULL ,
LinkTypeCd char (3) NOT NULL ,
SctGrpRow varchar (200) NULL ,
SctGrpCol varchar (200) NULL ,
CONSTRAINT PK_PageObj PRIMARY KEY CLUSTERED (
PageObjId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Printer') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Printer') AND type='U')
DROP TABLE dbo.Printer
CREATE TABLE Printer ( 
PrinterId smallint NOT NULL ,
PrinterName varchar (100) NOT NULL ,
PrinterPath varchar (300) NOT NULL ,
UpdatePrinted char (1) NOT NULL ,
CONSTRAINT PK_Printer PRIMARY KEY CLUSTERED (
PrinterId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Printer_MemberId') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Printer_MemberId') AND type='U')
DROP TABLE dbo.Printer_MemberId
CREATE TABLE Printer_MemberId ( 
PrinterId smallint NOT NULL ,
MemberId int NOT NULL ,
CONSTRAINT PK_Printer_MemberId PRIMARY KEY CLUSTERED (
PrinterId,
MemberId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Printer_UsrGroupId') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Printer_UsrGroupId') AND type='U')
DROP TABLE dbo.Printer_UsrGroupId
CREATE TABLE Printer_UsrGroupId ( 
PrinterId smallint NOT NULL ,
UsrGroupId smallint NOT NULL ,
CONSTRAINT PK_Printer_UsrGroupId PRIMARY KEY CLUSTERED (
PrinterId,
UsrGroupId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Release') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Release') AND type='U')
DROP TABLE dbo.Release
CREATE TABLE Release ( 
ReleaseId smallint IDENTITY(1,1) NOT NULL ,
ReleaseName nvarchar (50) NOT NULL ,
ReleaseDesc nvarchar (80) NULL ,
ReleaseBuild varchar (20) NULL ,
ReleaseDate datetime NULL ,
ReleaseOs char (1) NOT NULL ,
EntityId smallint NOT NULL ,
ReleaseTypeId tinyint NOT NULL ,
TarScriptAft varchar (max) NULL ,
ReadMe varchar (max) NULL ,
CONSTRAINT PK_Release PRIMARY KEY CLUSTERED (
ReleaseId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReleaseDtl') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReleaseDtl') AND type='U')
DROP TABLE dbo.ReleaseDtl
CREATE TABLE ReleaseDtl ( 
ReleaseDtlId int IDENTITY(1,1) NOT NULL ,
ReleaseId smallint NOT NULL ,
RunOrder smallint NULL ,
ObjectName varchar (max) NOT NULL ,
SrcObject varchar (50) NULL ,
ObjectExempt varchar (max) NULL ,
ObjectType char (1) NOT NULL ,
SrcClientTierId tinyint NULL ,
SrcRuleTierId tinyint NULL ,
SrcDataTierId tinyint NULL ,
TarDataTierId tinyint NULL ,
SProcOnly char (1) NOT NULL CONSTRAINT DF_ReleaseDtl_SProcOnly DEFAULT ('Y'),
DoSpEncrypt char (1) NOT NULL CONSTRAINT DF_ReleaseDtl_DoSpEncrypt DEFAULT ('Y'),
CONSTRAINT PK_ReleaseDtl PRIMARY KEY CLUSTERED (
ReleaseDtlId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Report') AND type='U')
DROP TABLE dbo.Report
CREATE TABLE Report ( 
ReportId int IDENTITY(1,1) NOT NULL ,
ProgramName varchar (20) NOT NULL ,
ReportDesc nvarchar (50) NULL ,
OrientationCd char (1) NOT NULL ,
ReportTypeCd char (1) NOT NULL ,
TemplateName varchar (50) NULL ,
RptTemplate int NULL ,
CopyReportId int NULL ,
TopMargin decimal (8,2) NOT NULL CONSTRAINT DF_Report_TopMargin DEFAULT ((1)),
BottomMargin decimal (8,2) NOT NULL CONSTRAINT DF_Report_BottomMargin DEFAULT ((1)),
LeftMargin decimal (8,2) NOT NULL CONSTRAINT DF_Report_LeftMargin DEFAULT ((1)),
RightMargin decimal (8,2) NOT NULL CONSTRAINT DF_Report_RightMargin DEFAULT ((1)),
PageHeight decimal (8,2) NOT NULL CONSTRAINT DF_Report_PageHeight DEFAULT ((11)),
PageWidth decimal (8,2) NOT NULL CONSTRAINT DF_Report_PageWidth DEFAULT ((8.5)),
UnitCd char (2) NOT NULL CONSTRAINT DF_Report_UnitCd DEFAULT ('in'),
WhereClause varchar (1000) NULL ,
RegClause varchar (400) NULL ,
RegCode nvarchar (max) NULL ,
ValClause varchar (400) NULL ,
ValCode nvarchar (max) NULL ,
UpdClause varchar (200) NULL ,
UpdCode nvarchar (max) NULL ,
XlsClause varchar (200) NULL ,
XlsCode nvarchar (max) NULL ,
GenerateRp char (1) NOT NULL ,
AllowSelect char (1) NOT NULL CONSTRAINT DF_Report_AllowSelect DEFAULT ('N'),
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
LastGenDt datetime NULL ,
CompanyLs varchar (1000) NULL ,
NeedRegen char (1) NOT NULL CONSTRAINT DF_Report_NeedRegen DEFAULT ('N'),
AuthRequired char (1) NOT NULL CONSTRAINT DF_Report_AuthRequired DEFAULT ('Y'),
CommandTimeOut smallint NULL ,
CONSTRAINT PK_Report PRIMARY KEY CLUSTERED (
ReportId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportCri') AND type='U')
DROP TABLE dbo.ReportCri
CREATE TABLE ReportCri ( 
ReportCriId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
ReportCriDesc varchar (100) NULL ,
ReportGrpId int NOT NULL ,
LabelCss varchar (100) NULL ,
ContentCss varchar (100) NULL ,
TableId int NULL ,
TableAbbr varchar (10) NULL ,
ColumnName varchar (50) NOT NULL ,
TabIndex smallint NOT NULL ,
DataTypeId tinyint NOT NULL ,
DataTypeSize smallint NULL ,
DisplayModeId tinyint NOT NULL ,
ColumnSize smallint NULL ,
RowSize smallint NULL ,
DdlKeyColumnName varchar (50) NULL ,
DdlRefColumnName varchar (50) NULL ,
DdlSrtColumnName varchar (50) NULL ,
DdlFtrColumnId int NULL ,
RequiredValid char (1) NOT NULL ,
DefaultValue nvarchar (100) NULL ,
WhereClause varchar (1000) NULL ,
RegClause varchar (400) NULL ,
CONSTRAINT PK_ReportCri PRIMARY KEY CLUSTERED (
ReportCriId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ReportCriHlp_ReportCriId' AND o.name = 'ReportCriHlp')
DROP INDEX ReportCriHlp.IX_ReportCriHlp_ReportCriId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportCriHlp') AND type='U')
DROP TABLE dbo.ReportCriHlp
CREATE TABLE ReportCriHlp ( 
ReportCriHlpId int IDENTITY(1,1) NOT NULL ,
ReportCriHlpDesc varchar (200) NULL ,
ReportCriId int NOT NULL ,
CultureId smallint NOT NULL ,
ColumnHeader nvarchar (50) NULL ,
CONSTRAINT PK_ReportCriHlp PRIMARY KEY CLUSTERED (
ReportCriHlpId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportDel') AND type='U')
DROP TABLE dbo.ReportDel
CREATE TABLE ReportDel ( 
ReportDelId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
ProgramName varchar (100) NOT NULL ,
CONSTRAINT PK_ReportDel PRIMARY KEY CLUSTERED (
ReportDelId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportGrp') AND type='U')
DROP TABLE dbo.ReportGrp
CREATE TABLE ReportGrp ( 
ReportGrpId int IDENTITY(1,1) NOT NULL ,
ReportId int NULL ,
ParentGrpId int NULL ,
ReportGrpName nvarchar (50) NOT NULL ,
ReportGrpIndex varchar (20) NULL ,
TabIndex smallint NULL ,
ContentVertical char (1) NOT NULL ,
LabelVertical char (1) NOT NULL ,
BorderWidth tinyint NOT NULL ,
GrpStyle varchar (200) NULL ,
CONSTRAINT PK_ReportGrp PRIMARY KEY CLUSTERED (
ReportGrpId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ReportHlp_ReportId' AND o.name = 'ReportHlp')
DROP INDEX ReportHlp.IX_ReportHlp_ReportId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportHlp') AND type='U')
DROP TABLE dbo.ReportHlp
CREATE TABLE ReportHlp ( 
ReportHlpId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
CultureId smallint NOT NULL ,
DefaultHlpMsg nvarchar (max) NOT NULL ,
ReportTitle nvarchar (50) NOT NULL ,
CONSTRAINT PK_ReportHlp PRIMARY KEY CLUSTERED (
ReportHlpId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportLstCri') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ReportLstCri' AND o.name = 'ReportLstCri')
DROP INDEX ReportLstCri.IX_ReportLstCri 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportLstCri') AND type='U')
DROP TABLE dbo.ReportLstCri
CREATE TABLE ReportLstCri ( 
ReportLstCriId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
ReportId int NOT NULL ,
ReportCriId int NOT NULL ,
LastCriteria nvarchar (900) NULL ,
CONSTRAINT PK_ReportLstCri PRIMARY KEY CLUSTERED (
ReportLstCriId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportObj') AND type='U')
DROP TABLE dbo.ReportObj
CREATE TABLE ReportObj ( 
ReportObjId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
RptObjTypeCd char (1) NOT NULL CONSTRAINT DF_ReportObj_RptObjTypeCd DEFAULT ('F'),
ColumnName varchar (50) NOT NULL ,
ColumnDesc varchar (100) NULL ,
TabIndex smallint NOT NULL ,
ColumnFormat varchar (20) NULL ,
PaddSize tinyint NULL ,
PaddChar nchar (1) NULL ,
DataTypeId tinyint NOT NULL ,
OperatorId tinyint NULL ,
ReportCriId int NULL ,
CONSTRAINT PK_ReportObj PRIMARY KEY CLUSTERED (
ReportObjId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ReportObjHlp_ReportObjId' AND o.name = 'ReportObjHlp')
DROP INDEX ReportObjHlp.IX_ReportObjHlp_ReportObjId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ReportObjHlp') AND type='U')
DROP TABLE dbo.ReportObjHlp
CREATE TABLE ReportObjHlp ( 
ReportObjHlpId int IDENTITY(1,1) NOT NULL ,
ReportObjHlpDesc varchar (200) NULL ,
ReportObjId int NOT NULL ,
CultureId smallint NOT NULL ,
ColumnHeader nvarchar (50) NULL ,
HeaderWidth smallint NULL ,
CONSTRAINT PK_ReportObjHlp PRIMARY KEY CLUSTERED (
ReportObjHlpId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RowOvrd') AND type='U')
DROP TABLE dbo.RowOvrd
CREATE TABLE RowOvrd ( 
RowOvrdId smallint IDENTITY(1,1) NOT NULL ,
RowOvrdDesc varchar (200) NULL ,
ScreenId int NULL ,
ReportId int NULL ,
RowAuthId smallint NOT NULL ,
AllowSel char (1) NOT NULL ,
AllowAdd char (1) NOT NULL ,
AllowUpd char (1) NOT NULL ,
AllowDel char (1) NOT NULL ,
CONSTRAINT PK_RowOvrd PRIMARY KEY CLUSTERED (
RowOvrdId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RowOvrdPrm') AND type='U')
DROP TABLE dbo.RowOvrdPrm
CREATE TABLE RowOvrdPrm ( 
RowOvrdPrmId smallint IDENTITY(1,1) NOT NULL ,
RowOvrdId smallint NOT NULL ,
PermKeyId smallint NOT NULL ,
SelLevel char (1) NOT NULL ,
AndCondition char (1) NOT NULL CONSTRAINT DF_RowOvrdPrm_AndCondition DEFAULT ('Y'),
CONSTRAINT PK_RowOvrdPrm PRIMARY KEY CLUSTERED (
RowOvrdPrmId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptCel') AND type='U')
DROP TABLE dbo.RptCel
CREATE TABLE RptCel ( 
RptCelId int IDENTITY(1,1) NOT NULL ,
RptCelDesc nvarchar (200) NULL ,
RptTblId int NOT NULL ,
RowNum smallint NOT NULL ,
RowHeight decimal (8,2) NOT NULL ,
RowVisibility varchar (1000) NULL ,
CelNum int NOT NULL ,
CelColSpan smallint NULL ,
CONSTRAINT PK_RptCel PRIMARY KEY CLUSTERED (
RptCelId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptCha') AND type='U')
DROP TABLE dbo.RptCha
CREATE TABLE RptCha ( 
RptChaId int IDENTITY(1,1) NOT NULL ,
RptChaDesc nvarchar (200) NULL ,
RptCtrId int NOT NULL ,
ReportId int NULL ,
RptChaTypeCd char (2) NOT NULL ,
ThreeD char (1) NOT NULL ,
CategoryGrp int NOT NULL ,
ChartData nvarchar (1000) NOT NULL ,
SeriesGrp int NULL ,
CONSTRAINT PK_RptCha PRIMARY KEY CLUSTERED (
RptChaId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptCtr') AND type='U')
DROP TABLE dbo.RptCtr
CREATE TABLE RptCtr ( 
RptCtrId int IDENTITY(1,1) NOT NULL ,
RptCtrDesc nvarchar (200) NULL ,
PRptCtrId int NULL ,
RptElmId int NULL ,
RptCelId int NULL ,
ReportId int NOT NULL ,
RptCtrTypeCd char (1) NOT NULL ,
RptCtrName nvarchar (100) NOT NULL ,
RptStyleId int NULL ,
CtrTop decimal (8,2) NULL ,
CtrLeft decimal (8,2) NULL ,
CtrHeight decimal (8,2) NULL ,
CtrWidth decimal (8,2) NULL ,
CtrZIndex smallint NULL ,
CtrAction varchar (500) NULL ,
CtrGrouping int NULL ,
CtrVisibility char (1) NULL ,
CtrToggle int NULL ,
CtrToolTip nvarchar (200) NULL ,
CtrPgBrStart char (1) NOT NULL ,
CtrPgBrEnd char (1) NOT NULL ,
CtrValue nvarchar (1000) NULL ,
CtrCanGrow char (1) NOT NULL ,
CtrCanShrink char (1) NOT NULL ,
CtrTogether char (1) NOT NULL ,
CONSTRAINT PK_RptCtr PRIMARY KEY CLUSTERED (
RptCtrId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptElm') AND type='U')
DROP TABLE dbo.RptElm
CREATE TABLE RptElm ( 
RptElmId int IDENTITY(1,1) NOT NULL ,
RptElmDesc nvarchar (200) NULL ,
ReportId int NOT NULL ,
RptElmTypeCd char (1) NOT NULL ,
RptStyleId int NULL ,
ElmHeight decimal (8,2) NOT NULL ,
ElmColumns smallint NULL ,
ElmColSpacing decimal (8,2) NULL ,
ElmPrintFirst char (1) NOT NULL ,
ElmPrintLast char (1) NOT NULL ,
CONSTRAINT PK_RptElm PRIMARY KEY CLUSTERED (
RptElmId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_RptMemCri' AND o.name = 'RptMemCri')
DROP INDEX RptMemCri.IX_RptMemCri 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptMemCri') AND type='U')
DROP TABLE dbo.RptMemCri
CREATE TABLE RptMemCri ( 
RptMemCriId int IDENTITY(1,1) NOT NULL ,
RptMemFldId int NOT NULL ,
RptMemCriName nvarchar (200) NOT NULL ,
RptMemCriDesc nvarchar (500) NULL ,
RptMemCriLink varchar (200) NULL ,
ReportId int NOT NULL ,
UsrId int NULL ,
InputBy int NULL ,
InputOn datetime NULL ,
ModifiedOn datetime NULL ,
ViewedOn datetime NULL ,
CompanyLs varchar (1000) NULL ,
CONSTRAINT PK_RptMemCri PRIMARY KEY CLUSTERED (
RptMemCriId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptMemCriDtl') AND type='U')
DROP TABLE dbo.RptMemCriDtl
CREATE TABLE RptMemCriDtl ( 
RptMemCriDtlId int IDENTITY(1,1) NOT NULL ,
RptMemCriId int NOT NULL ,
ReportCriId int NOT NULL ,
MemCriteria nvarchar (900) NULL ,
CONSTRAINT PK_RptMemCriDtl PRIMARY KEY CLUSTERED (
RptMemCriDtlId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptMemFld') AND type='U')
DROP TABLE dbo.RptMemFld
CREATE TABLE RptMemFld ( 
RptMemFldId int IDENTITY(1,1) NOT NULL ,
RptMemFldName nvarchar (200) NOT NULL ,
UsrId int NULL ,
InputBy int NULL ,
CompanyLs varchar (1000) NULL ,
AccessCd char (1) NOT NULL CONSTRAINT DF_RptMemFld_AccessCd DEFAULT ('V'),
CONSTRAINT PK_RptMemFld PRIMARY KEY CLUSTERED (
RptMemFldId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptStyle') AND type='U')
DROP TABLE dbo.RptStyle
CREATE TABLE RptStyle ( 
RptStyleId int IDENTITY(1,1) NOT NULL ,
RptStyleDesc nvarchar (300) NOT NULL ,
BorderColorD varchar (100) NULL ,
BorderColorL varchar (100) NULL ,
BorderColorR varchar (100) NULL ,
BorderColorT varchar (100) NULL ,
BorderColorB varchar (100) NULL ,
BorderStyleD tinyint NULL ,
BorderStyleL tinyint NULL ,
BorderStyleR tinyint NULL ,
BorderStyleT tinyint NULL ,
BorderStyleB tinyint NULL ,
BorderWidthD tinyint NULL ,
BorderWidthL tinyint NULL ,
BorderWidthR tinyint NULL ,
BorderWidthT tinyint NULL ,
BorderWidthB tinyint NULL ,
BgColor varchar (100) NULL ,
BgGradType tinyint NULL ,
BgGradColor varchar (100) NULL ,
BgImage varchar (200) NULL ,
FontStyle char (1) NULL ,
FontFamily varchar (100) NULL ,
FontSize tinyint NULL ,
FontWeight tinyint NULL ,
Format varchar (100) NULL ,
TextDecor char (1) NULL ,
TextAlign char (1) NULL ,
VerticalAlign char (1) NULL ,
Color varchar (100) NULL ,
PadLeft smallint NULL ,
PadRight smallint NULL ,
PadTop smallint NULL ,
PadBottom smallint NULL ,
LineHeight smallint NULL ,
Direction char (1) NULL ,
WritingMode char (1) NULL ,
DefaultCd char (2) NULL ,
CONSTRAINT PK_RptStyle PRIMARY KEY CLUSTERED (
RptStyleId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptTbl') AND type='U')
DROP TABLE dbo.RptTbl
CREATE TABLE RptTbl ( 
RptTblId int IDENTITY(1,1) NOT NULL ,
RptTblDesc nvarchar (200) NULL ,
ParentId int NULL ,
RptCtrId int NOT NULL ,
ReportId int NULL ,
RptTblTypeCd char (1) NOT NULL ,
TblRepeatNew char (1) NOT NULL ,
TblOrder smallint NULL ,
ColWidth decimal (8,2) NULL ,
TblGrouping int NULL ,
TblVisibility char (1) NULL ,
TblToggle int NULL ,
CONSTRAINT PK_RptTbl PRIMARY KEY CLUSTERED (
RptTblId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptTemplate') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_RptTemplate' AND o.name = 'RptTemplate')
DROP INDEX RptTemplate.IX_RptTemplate 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptTemplate') AND type='U')
DROP TABLE dbo.RptTemplate
CREATE TABLE RptTemplate ( 
DocId int IDENTITY(1,1) NOT NULL ,
MasterId int NOT NULL ,
DocName nvarchar (100) NOT NULL ,
MimeType varchar (100) NOT NULL ,
DocSize bigint NOT NULL ,
DocImage varbinary (max) NOT NULL ,
InputBy int NULL ,
InputOn datetime NULL ,
Active char (1) NOT NULL ,
CONSTRAINT PK_RptTemplate PRIMARY KEY CLUSTERED (
DocId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Rptwiz') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Rptwiz') AND type='U')
DROP TABLE dbo.Rptwiz
CREATE TABLE Rptwiz ( 
RptwizId int IDENTITY(1,1) NOT NULL ,
RptwizName nvarchar (50) NOT NULL ,
RptwizDesc nvarchar (400) NOT NULL ,
RptwizTypeCd char (1) NOT NULL ,
RptwizCatId smallint NOT NULL ,
TemplateName varchar (50) NULL ,
ReportId int NULL ,
AccessCd char (1) NOT NULL ,
UsrId int NOT NULL ,
TopMargin decimal (8,2) NULL ,
BottomMargin decimal (8,2) NULL ,
LeftMargin decimal (8,2) NULL ,
RightMargin decimal (8,2) NULL ,
OrientationCd char (1) NOT NULL CONSTRAINT DF_Rptwiz_OrientationCd DEFAULT ('Y'),
UnitCd char (2) NOT NULL CONSTRAINT DF_Rptwiz_UnitCd DEFAULT ('in'),
RptChaTypeCd char (2) NULL ,
ThreeD char (1) NOT NULL CONSTRAINT DF_Rptwiz_ThreeD DEFAULT ('N'),
GMinValue decimal (19,4) NULL ,
GLowRange decimal (19,4) NULL ,
GMidRange decimal (19,4) NULL ,
GMaxValue decimal (19,4) NULL ,
GNeedle decimal (19,4) NULL ,
GMinValueId int NULL ,
GLowRangeId int NULL ,
GMidRangeId int NULL ,
GMaxValueId int NULL ,
GNeedleId int NULL ,
GPositive char (1) NULL ,
GFormat char (1) NULL ,
CONSTRAINT PK_Rptwiz PRIMARY KEY CLUSTERED (
RptwizId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptwizCat') AND type='U')
DROP TABLE dbo.RptwizCat
CREATE TABLE RptwizCat ( 
RptwizCatId smallint IDENTITY(1,1) NOT NULL ,
RptwizTypId smallint NOT NULL CONSTRAINT DF_RptwizCat_RptwizTypId DEFAULT ((1)),
RptwizCatName nvarchar (100) NOT NULL ,
RptwizCatDesc nvarchar (200) NULL ,
CatDescription nvarchar (400) NULL ,
TableId int NOT NULL CONSTRAINT DF_RptwizCat_TableId DEFAULT ((0)),
SampleImage varbinary (max) NULL ,
CONSTRAINT PK_RptwizCat PRIMARY KEY CLUSTERED (
RptwizCatId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptwizCatDtl') AND type='U')
DROP TABLE dbo.RptwizCatDtl
CREATE TABLE RptwizCatDtl ( 
RptwizCatDtlId int IDENTITY(1,1) NOT NULL ,
RptwizCatId smallint NOT NULL ,
ColumnId int NOT NULL ,
DisplayModeId tinyint NULL ,
DdlKeyColNm varchar (50) NULL ,
DdlRefColNm varchar (50) NULL ,
ColumnSize smallint NULL ,
RowSize smallint NULL ,
RegClause varchar (400) NULL ,
StoredProc varchar (max) NULL ,
CONSTRAINT PK_RptwizCatDtl PRIMARY KEY CLUSTERED (
RptwizCatDtlId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptwizDtl') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptwizDtl') AND type='U')
DROP TABLE dbo.RptwizDtl
CREATE TABLE RptwizDtl ( 
RptwizDtlId int IDENTITY(1,1) NOT NULL ,
RptwizId int NOT NULL ,
ColumnId int NOT NULL ,
ColHeader nvarchar (50) NULL ,
CriOperatorId tinyint NULL ,
CriSelect smallint NULL ,
CriHeader nvarchar (50) NULL ,
ColSelect smallint NULL ,
ColGroup smallint NULL ,
ColSort smallint NULL ,
AggregateCd char (1) NULL ,
RptChartCd char (1) NULL ,
CONSTRAINT PK_RptwizDtl PRIMARY KEY CLUSTERED (
RptwizDtlId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RptwizTyp') AND type='U')
DROP TABLE dbo.RptwizTyp
CREATE TABLE RptwizTyp ( 
RptwizTypId smallint IDENTITY(1,1) NOT NULL ,
RptwizTypName nvarchar (100) NOT NULL ,
CONSTRAINT PK_RptwizTyp PRIMARY KEY CLUSTERED (
RptwizTypId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RuleAsmx') AND type='U')
DROP TABLE dbo.RuleAsmx
CREATE TABLE RuleAsmx ( 
RuleAsmxId int IDENTITY(1,1) NOT NULL ,
RuleAsmxTypeId tinyint NOT NULL ,
RuleAsmxName nvarchar (100) NOT NULL ,
RuleAsmxDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
ScreenId int NOT NULL ,
RuleAsmxProg nvarchar (max) NOT NULL ,
CONSTRAINT PK_RuleAsmx PRIMARY KEY CLUSTERED (
RuleAsmxId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RuleReact') AND type='U')
DROP TABLE dbo.RuleReact
CREATE TABLE RuleReact ( 
RuleReactId int IDENTITY(1,1) NOT NULL ,
RuleReactTypeId tinyint NOT NULL ,
RuleReactName nvarchar (100) NOT NULL ,
RuleReactDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
ScreenId int NOT NULL ,
ScreenObjId int NULL ,
RuleReactProg nvarchar (max) NOT NULL ,
CONSTRAINT PK_RuleReact PRIMARY KEY CLUSTERED (
RuleReactId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RuleTier') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.RuleTier') AND type='U')
DROP TABLE dbo.RuleTier
CREATE TABLE RuleTier ( 
RuleTierId tinyint IDENTITY(1,1) NOT NULL ,
RuleTierName nvarchar (50) NOT NULL ,
EntityId int NOT NULL ,
LanguageCd char (1) NOT NULL ,
FrameworkCd char (1) NOT NULL ,
DevProgramPath varchar (100) NOT NULL ,
IsDefault char (1) NOT NULL ,
CONSTRAINT PK_RuleTier PRIMARY KEY CLUSTERED (
RuleTierId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScrAudit') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScrAudit' AND o.name = 'ScrAudit')
DROP INDEX ScrAudit.IX_ScrAudit 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScrAudit') AND type='U')
DROP TABLE dbo.ScrAudit
CREATE TABLE ScrAudit ( 
ScrAuditId bigint IDENTITY(1,1) NOT NULL ,
CudAction char (1) NOT NULL ,
ScreenId int NOT NULL ,
MasterTable char (1) NOT NULL ,
TableId int NOT NULL ,
RowId bigint NOT NULL ,
RowDesc nvarchar (max) NOT NULL ,
ChangedBy int NOT NULL ,
ChangedOn datetime NOT NULL ,
CONSTRAINT PK_ScrAudit PRIMARY KEY CLUSTERED (
ScrAuditId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScrAuditDtl') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScrAuditDtl' AND o.name = 'ScrAuditDtl')
DROP INDEX ScrAuditDtl.IX_ScrAuditDtl 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScrAuditDtl') AND type='U')
DROP TABLE dbo.ScrAuditDtl
CREATE TABLE ScrAuditDtl ( 
ScrAuditDtlId bigint IDENTITY(1,1) NOT NULL ,
ScrAuditId bigint NOT NULL ,
ScreenObjId int NOT NULL ,
ScreenObjDesc nvarchar (max) NOT NULL ,
ColumnId int NOT NULL ,
ColumnDesc nvarchar (max) NOT NULL ,
ChangedFr nvarchar (max) NULL ,
ChangedTo nvarchar (max) NULL ,
CONSTRAINT PK_ScrAuditDtl PRIMARY KEY CLUSTERED (
ScrAuditDtlId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Screen') AND type='U')
DROP TABLE dbo.Screen
CREATE TABLE Screen ( 
ScreenId int IDENTITY(1,1) NOT NULL ,
ProgramName varchar (20) NOT NULL ,
ScreenDesc nvarchar (50) NULL ,
ScreenTypeId tinyint NOT NULL ,
MasterTableId int NULL ,
DetailTableId int NULL ,
SearchTableId int NULL ,
SearchId int NULL ,
SearchAscending char (1) NOT NULL ,
SearchIdR int NULL ,
SearchDtlId int NULL ,
SearchDtlIdR int NULL ,
SearchUrlId int NULL ,
SearchImgId int NULL ,
GridRows tinyint NULL ,
ScreenObj varchar (100) NULL ,
ScreenFilter varchar (100) NULL ,
GenerateSc char (1) NOT NULL CONSTRAINT DF_Screen_GenerateSc DEFAULT ('Y'),
GenerateSr char (1) NOT NULL CONSTRAINT DF_Screen_GenerateSr DEFAULT ('Y'),
ReactGenerated char (1) NOT NULL CONSTRAINT DF_Screen_ReactGenerated DEFAULT ('N'),
HasDeleteAll char (1) NOT NULL ,
ShowGridHead char (1) NOT NULL CONSTRAINT DF_Screen_ShowGridHead DEFAULT ('Y'),
ValidateReq char (1) NOT NULL ,
DeferError char (1) NOT NULL CONSTRAINT DF_Screen_DeferError DEFAULT ('N'),
AuthRequired char (1) NOT NULL CONSTRAINT DF_Screen_AuthRequired DEFAULT ('Y'),
ViewOnly char (1) NOT NULL CONSTRAINT DF_Screen_ViewOnly DEFAULT ('N'),
GenAudit char (1) NOT NULL CONSTRAINT DF_Screen_GenAudit DEFAULT ('N'),
NeedRegen char (1) NOT NULL CONSTRAINT DF_Screen_NeedRegen DEFAULT ('N'),
NeedReactRegen char (1) NOT NULL CONSTRAINT DF_Screen_NeedReactRegen DEFAULT ('N'),
CONSTRAINT PK_Screen PRIMARY KEY CLUSTERED (
ScreenId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenCri') AND type='U')
DROP TABLE dbo.ScreenCri
CREATE TABLE ScreenCri ( 
ScreenCriId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
ScreenCriDesc varchar (100) NULL ,
LabelCss varchar (100) NULL ,
ContentCss varchar (100) NULL ,
ColumnId int NOT NULL ,
OperatorId tinyint NOT NULL CONSTRAINT DF_ScreenCri_OperatorId DEFAULT ((1)),
TabIndex smallint NOT NULL ,
DisplayModeId tinyint NOT NULL ,
ColumnJustify char (1) NULL ,
ColumnSize smallint NULL ,
RowSize smallint NULL ,
DdlKeyColumnId int NULL ,
DdlRefColumnId int NULL ,
DdlSrtColumnId int NULL ,
DdlFtrColumnId int NULL ,
RequiredValid char (1) NOT NULL ,
CONSTRAINT PK_ScreenCri PRIMARY KEY CLUSTERED (
ScreenCriId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScreenCriHlp_ScreenCriId' AND o.name = 'ScreenCriHlp')
DROP INDEX ScreenCriHlp.IX_ScreenCriHlp_ScreenCriId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenCriHlp') AND type='U')
DROP TABLE dbo.ScreenCriHlp
CREATE TABLE ScreenCriHlp ( 
ScreenCriHlpId int IDENTITY(1,1) NOT NULL ,
ScreenCriHlpDesc varchar (200) NULL ,
ScreenCriId int NOT NULL ,
CultureId smallint NOT NULL ,
ColumnHeader nvarchar (50) NULL ,
CONSTRAINT PK_ScreenCriHlp PRIMARY KEY CLUSTERED (
ScreenCriHlpId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenDel') AND type='U')
DROP TABLE dbo.ScreenDel
CREATE TABLE ScreenDel ( 
ScreenDelId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
ProgramName varchar (100) NOT NULL ,
MultiDesignDb char (1) NOT NULL ,
CONSTRAINT PK_ScreenDel PRIMARY KEY CLUSTERED (
ScreenDelId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScreenFilter_Id_Name' AND o.name = 'ScreenFilter')
DROP INDEX ScreenFilter.IX_ScreenFilter_Id_Name 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenFilter') AND type='U')
DROP TABLE dbo.ScreenFilter
CREATE TABLE ScreenFilter ( 
ScreenFilterId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
ScreenFilterName varchar (50) NOT NULL ,
ScreenFilterDesc varchar (100) NULL ,
FilterClause varchar (1500) NOT NULL ,
FilterOrder tinyint NOT NULL ,
ApplyToMst char (1) NOT NULL CONSTRAINT DF_ScreenFilter_ApplyToMst DEFAULT ('Y'),
CONSTRAINT PK_ScreenFilter PRIMARY KEY CLUSTERED (
ScreenFilterId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScreenFilterHlp_FilterId' AND o.name = 'ScreenFilterHlp')
DROP INDEX ScreenFilterHlp.IX_ScreenFilterHlp_FilterId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenFilterHlp') AND type='U')
DROP TABLE dbo.ScreenFilterHlp
CREATE TABLE ScreenFilterHlp ( 
ScreenFilterHlpId int IDENTITY(1,1) NOT NULL ,
ScreenFilterId int NOT NULL ,
CultureId smallint NOT NULL ,
FilterName nvarchar (50) NOT NULL ,
CONSTRAINT PK_ScreenFilterHlp PRIMARY KEY CLUSTERED (
ScreenFilterHlpId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenHlp') AND type='U')
DROP TABLE dbo.ScreenHlp
CREATE TABLE ScreenHlp ( 
ScreenHlpId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
CultureId smallint NOT NULL ,
DefaultHlpMsg nvarchar (max) NOT NULL ,
FootNote nvarchar (400) NULL ,
ScreenTitle nvarchar (50) NOT NULL ,
AddMsg nvarchar (100) NULL ,
UpdMsg nvarchar (100) NULL ,
DelMsg nvarchar (100) NULL ,
IncrementMsg nvarchar (100) NULL ,
MasterLstTitle nvarchar (100) NULL ,
MasterLstSubtitle nvarchar (100) NULL ,
MasterRecTitle nvarchar (100) NULL ,
MasterRecSubtitle nvarchar (100) NULL ,
DetailLstTitle nvarchar (100) NULL ,
DetailLstSubtitle nvarchar (100) NULL ,
DetailRecTitle nvarchar (100) NULL ,
DetailRecSubtitle nvarchar (100) NULL ,
NoMasterMsg nvarchar (100) NULL ,
NoDetailMsg nvarchar (100) NULL ,
AddMasterMsg nvarchar (100) NULL ,
AddDetailMsg nvarchar (100) NULL ,
MasterFoundMsg nvarchar (100) NULL ,
DetailFoundMsg nvarchar (100) NULL ,
CONSTRAINT PK_ScreenHlp PRIMARY KEY CLUSTERED (
ScreenHlpId,
CultureId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenLstCri') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScreenLstCri' AND o.name = 'ScreenLstCri')
DROP INDEX ScreenLstCri.IX_ScreenLstCri 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenLstCri') AND type='U')
DROP TABLE dbo.ScreenLstCri
CREATE TABLE ScreenLstCri ( 
ScreenLstCriId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
ScreenId int NOT NULL ,
ScreenCriId int NOT NULL ,
LastCriteria nvarchar (900) NULL ,
CONSTRAINT PK_ScreenLstCri PRIMARY KEY CLUSTERED (
ScreenLstCriId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenLstInf') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScreenLstInf' AND o.name = 'ScreenLstInf')
DROP INDEX ScreenLstInf.IX_ScreenLstInf 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenLstInf') AND type='U')
DROP TABLE dbo.ScreenLstInf
CREATE TABLE ScreenLstInf ( 
ScreenLstInfId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
ScreenId int NOT NULL ,
LastPageInfo nvarchar (900) NULL ,
CONSTRAINT PK_ScreenLstInf PRIMARY KEY CLUSTERED (
ScreenLstInfId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenObj') AND type='U')
DROP TABLE dbo.ScreenObj
CREATE TABLE ScreenObj ( 
ScreenObjId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
MasterTable char (1) NOT NULL ,
NewGroupRow char (1) NOT NULL CONSTRAINT DF_ScreenObj_NewGroupRow DEFAULT ('N'),
GroupRowId smallint NOT NULL CONSTRAINT DF_ScreenObj_GroupRowId DEFAULT ((78)),
GroupColId smallint NOT NULL ,
LabelVertical char (1) NOT NULL CONSTRAINT DF_ScreenObj_LabelVertical DEFAULT ('N'),
LabelCss varchar (1000) NULL ,
ContentCss varchar (1000) NULL ,
ColumnId int NULL ,
ColumnName varchar (50) NOT NULL ,
ColumnDesc varchar (200) NULL ,
DefaultValue nvarchar (200) NULL ,
HyperLinkUrl nvarchar (200) NULL ,
DefAfter char (1) NOT NULL CONSTRAINT DF_ScreenObj_DefAfter DEFAULT ('N'),
SystemValue nvarchar (200) NULL ,
DefAlways char (1) NOT NULL CONSTRAINT DF_ScreenObj_DefAlways DEFAULT ('N'),
ColumnWrap char (1) NOT NULL ,
GridGrpCd char (1) NOT NULL CONSTRAINT DF_ScreenObj_GridGrpCd DEFAULT ('N'),
HideOnTablet char (1) NOT NULL CONSTRAINT DF_ScreenObj_HideOnTablet DEFAULT ('N'),
HideOnMobile char (1) NOT NULL CONSTRAINT DF_ScreenObj_HideOnMobile DEFAULT ('N'),
ColumnJustify char (1) NULL ,
ColumnSize smallint NULL ,
ResizeWidth smallint NULL ,
ColumnHeight smallint NULL ,
ResizeHeight smallint NULL ,
DisplayModeId tinyint NOT NULL ,
DdlKeyColumnId int NULL ,
DdlRefColumnId int NULL ,
DdlSrtColumnId int NULL ,
DdlAdnColumnId int NULL ,
DdlFtrColumnId int NULL ,
AggregateCd char (1) NULL ,
GenerateSp char (1) NOT NULL ,
TabFolderId int NOT NULL ,
TabIndex smallint NOT NULL ,
SortOrder smallint NULL ,
DtlLstPosId tinyint NULL ,
RequiredValid char (1) NOT NULL ,
MaskValid varchar (100) NULL ,
RangeValidType varchar (50) NULL ,
RangeValidMax varchar (50) NULL ,
RangeValidMin varchar (50) NULL ,
ColumnLink varchar (1000) NULL ,
RefreshOnCUD char (1) NOT NULL ,
TrimOnEntry char (1) NOT NULL CONSTRAINT DF_ScreenObj_TrimOnEntry DEFAULT ('Y'),
MatchCd char (1) NULL ,
IgnoreConfirm char (1) NOT NULL CONSTRAINT DF_ScreenObj_IgnoreConfirm DEFAULT ('Y'),
CONSTRAINT PK_ScreenObj PRIMARY KEY CLUSTERED (
ScreenObjId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenObjHlp') AND type='U')
DROP TABLE dbo.ScreenObjHlp
CREATE TABLE ScreenObjHlp ( 
ScreenObjHlpId int IDENTITY(1,1) NOT NULL ,
ScreenObjHlpDesc varchar (200) NULL ,
ScreenObjId int NOT NULL ,
CultureId smallint NOT NULL ,
ColumnHeader nvarchar (max) NULL ,
ToolTip nvarchar (1000) NULL ,
ErrMessage nvarchar (1000) NULL ,
TbHint nvarchar (1000) NULL ,
CONSTRAINT PK_ScreenObjHlp PRIMARY KEY CLUSTERED (
ScreenObjHlpId,
CultureId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenTab') AND type='U')
DROP TABLE dbo.ScreenTab
CREATE TABLE ScreenTab ( 
ScreenTabId int IDENTITY(1,1) NOT NULL ,
ScreenTabDesc nvarchar (200) NULL ,
ScreenId int NOT NULL ,
TabFolderName nvarchar (50) NOT NULL ,
TabFolderOrder tinyint NOT NULL ,
CONSTRAINT PK_ScreenTab PRIMARY KEY CLUSTERED (
ScreenTabId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ScreenTabHlp_ScreenTabId' AND o.name = 'ScreenTabHlp')
DROP INDEX ScreenTabHlp.IX_ScreenTabHlp_ScreenTabId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScreenTabHlp') AND type='U')
DROP TABLE dbo.ScreenTabHlp
CREATE TABLE ScreenTabHlp ( 
ScreenTabHlpId int IDENTITY(1,1) NOT NULL ,
ScreenTabId int NOT NULL ,
CultureId smallint NOT NULL ,
TabFolderName nvarchar (50) NOT NULL ,
CONSTRAINT PK_ScreenTabHlp PRIMARY KEY CLUSTERED (
ScreenTabHlpId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScrMemCri') AND type='U')
DROP TABLE dbo.ScrMemCri
CREATE TABLE ScrMemCri ( 
ScrMemCriId int IDENTITY(1,1) NOT NULL ,
ScrMemCriName nvarchar (200) NOT NULL ,
ScreenId int NOT NULL ,
UsrId int NULL ,
CONSTRAINT PK_ScrMemCri PRIMARY KEY CLUSTERED (
ScrMemCriId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ScrMemCriDtl') AND type='U')
DROP TABLE dbo.ScrMemCriDtl
CREATE TABLE ScrMemCriDtl ( 
ScrMemCriDtlId int IDENTITY(1,1) NOT NULL ,
ScrMemCriId int NOT NULL ,
ScreenCriId int NOT NULL ,
MemCriteria nvarchar (900) NULL ,
CONSTRAINT PK_ScrMemCriDtl PRIMARY KEY CLUSTERED (
ScrMemCriDtlId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SctGrpCol') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_SctGrpCol' AND o.name = 'SctGrpCol')
DROP INDEX SctGrpCol.IU_SctGrpCol 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SctGrpCol') AND type='U')
DROP TABLE dbo.SctGrpCol
CREATE TABLE SctGrpCol ( 
SctGrpColId smallint IDENTITY(1,1) NOT NULL ,
SctGrpColDesc nvarchar (200) NULL ,
SectionCd char (1) NOT NULL ,
GroupColId smallint NOT NULL ,
SctGrpColCss varchar (1000) NULL ,
SctGrpColDiv varchar (1000) NULL ,
CONSTRAINT PK_SctGrpCol PRIMARY KEY CLUSTERED (
SctGrpColId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SctGrpRow') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_SctGrpRow' AND o.name = 'SctGrpRow')
DROP INDEX SctGrpRow.IU_SctGrpRow 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SctGrpRow') AND type='U')
DROP TABLE dbo.SctGrpRow
CREATE TABLE SctGrpRow ( 
SctGrpRowId smallint IDENTITY(1,1) NOT NULL ,
SctGrpRowDesc nvarchar (200) NULL ,
SectionCd char (1) NOT NULL ,
GroupRowId smallint NOT NULL ,
SctGrpRowCss varchar (1000) NULL ,
CONSTRAINT PK_SctGrpRow PRIMARY KEY CLUSTERED (
SctGrpRowId
)
)
END

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_ServerRule' AND o.name = 'ServerRule')
DROP INDEX ServerRule.IX_ServerRule 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.ServerRule') AND type='U')
DROP TABLE dbo.ServerRule
CREATE TABLE ServerRule ( 
ServerRuleId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
RuleTypeId tinyint NOT NULL ,
MasterTable char (1) NOT NULL ,
RuleName nvarchar (100) NOT NULL ,
RuleDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
RuleOrder smallint NOT NULL ,
ProcedureName varchar (50) NOT NULL ,
ParameterNames varchar (max) NULL ,
ParameterTypes varchar (max) NULL ,
CallingParams varchar (max) NULL ,
OnAdd char (1) NOT NULL ,
OnUpd char (1) NOT NULL ,
OnDel char (1) NOT NULL ,
BeforeCRUD char (1) NOT NULL ,
RuleCode nvarchar (max) NULL ,
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
LastGenDt datetime NULL ,
Guid varchar (50) NOT NULL CONSTRAINT DF_ServerRule_Guid DEFAULT (newid()),
RunMode char (1) NULL ,
SrcNS varchar (30) NULL ,
CONSTRAINT PK_ServerRule PRIMARY KEY CLUSTERED (
ServerRuleId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SqlToSybMap') AND type='U')
DROP TABLE dbo.SqlToSybMap
CREATE TABLE SqlToSybMap ( 
SqlToSybMapId int IDENTITY(1,1) NOT NULL ,
ProjectId smallint NOT NULL ,
OrigWord varchar (200) NOT NULL ,
DestWord varchar (200) NOT NULL ,
CONSTRAINT PK_SqlToSybMap PRIMARY KEY CLUSTERED (
SqlToSybMapId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SredMebr') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_SredMebr' AND o.name = 'SredMebr')
DROP INDEX SredMebr.IU_SredMebr 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SredMebr') AND type='U')
DROP TABLE dbo.SredMebr
CREATE TABLE SredMebr ( 
SredMebrId int IDENTITY(1,1) NOT NULL ,
MemberId int NOT NULL ,
MemberName nvarchar (50) NULL ,
UserId int NOT NULL ,
MemberTitle nvarchar (50) NULL ,
LT10PercShare char (1) NOT NULL ,
MnSalary money NOT NULL ,
MnNtxBenefit money NOT NULL ,
MnTaxBenefit money NOT NULL ,
MnWorkHours decimal (9,2) NOT NULL ,
CONSTRAINT PK_SredMebr PRIMARY KEY CLUSTERED (
SredMebrId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SredTime') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.SredTime') AND type='U')
DROP TABLE dbo.SredTime
CREATE TABLE SredTime ( 
SredTimeId int IDENTITY(1,1) NOT NULL ,
MemberId int NOT NULL ,
SredTimeDt datetime NOT NULL ,
Accomplished nvarchar (1000) NOT NULL ,
HourSpent decimal (9,2) NOT NULL ,
EnteredBy int NULL ,
EnteredOn datetime NULL ,
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
CONSTRAINT PK_SredTime PRIMARY KEY CLUSTERED (
SredTimeId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.StaticCs') AND type='U')
DROP TABLE dbo.StaticCs
CREATE TABLE StaticCs ( 
StaticCsId smallint IDENTITY(1,1) NOT NULL ,
StaticCsNm nvarchar (200) NOT NULL ,
StyleDef nvarchar (max) NULL ,
CONSTRAINT PK_StaticCs PRIMARY KEY CLUSTERED (
StaticCsId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.StaticFi') AND type='U')
DROP TABLE dbo.StaticFi
CREATE TABLE StaticFi ( 
StaticFiId int IDENTITY(1,1) NOT NULL ,
StaticFiUrl nvarchar (200) NOT NULL ,
CONSTRAINT PK_StaticFi PRIMARY KEY CLUSTERED (
StaticFiId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.StaticJs') AND type='U')
DROP TABLE dbo.StaticJs
CREATE TABLE StaticJs ( 
StaticJsId smallint IDENTITY(1,1) NOT NULL ,
StaticJsNm nvarchar (200) NOT NULL ,
ScriptDef nvarchar (max) NULL ,
CONSTRAINT PK_StaticJs PRIMARY KEY CLUSTERED (
StaticJsId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.StaticPg') AND type='U')
DROP TABLE dbo.StaticPg
CREATE TABLE StaticPg ( 
StaticPgId int IDENTITY(1,1) NOT NULL ,
StaticPgDesc nvarchar (300) NULL ,
StaticPgNm varchar (50) NOT NULL CONSTRAINT DF_StaticPg_StaticPgNm DEFAULT ('PgNm'),
StaticPgTitle nvarchar (100) NOT NULL ,
StaticMeta nvarchar (1000) NULL ,
StaticPgCss nvarchar (max) NULL ,
StaticPgJs nvarchar (max) NULL ,
StaticPgHtm nvarchar (max) NOT NULL ,
MasterPgFile varchar (100) NULL ,
StaticCsId smallint NULL ,
StaticJsId smallint NULL ,
StaticPgUrl varchar (200) NULL ,
CONSTRAINT PK_StaticPg PRIMARY KEY CLUSTERED (
StaticPgId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Systems') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Systems') AND type='U')
DROP TABLE dbo.Systems
CREATE TABLE Systems ( 
SystemId tinyint NOT NULL ,
SystemName nvarchar (50) NOT NULL ,
SystemAbbr varchar (4) NOT NULL CONSTRAINT DF_Systems_SystemAbbr DEFAULT ('Abc'),
ServerName varchar (50) NOT NULL ,
dbAppProvider varchar (50) NOT NULL ,
dbAppServer varchar (50) NOT NULL ,
dbAppDatabase varchar (50) NOT NULL ,
dbDesDatabase varchar (50) NOT NULL ,
dbAppUserId varchar (50) NOT NULL ,
dbAppPassword varchar (50) NOT NULL ,
dbX01Provider varchar (50) NULL ,
dbX01Server varchar (50) NULL ,
dbX01Database varchar (50) NULL ,
dbX01UserId varchar (50) NULL ,
dbX01Password varchar (50) NULL ,
dbX01Extra varchar (200) NULL ,
SysProgram char (1) NOT NULL ,
AdminEmail nvarchar (50) NOT NULL CONSTRAINT DF_Systems_AdminEmail DEFAULT ('noreply@gmail.com'),
AdminPhone varchar (20) NULL ,
CustServEmail nvarchar (50) NOT NULL CONSTRAINT DF_Systems_CustServEmail DEFAULT ('noreply@gmail.com'),
CustServPhone varchar (20) NULL ,
CustServFax varchar (20) NULL ,
WebAddress varchar (50) NULL ,
Active char (1) NOT NULL CONSTRAINT DF_Systems_Active DEFAULT ('Y'),
DfltTimezone nvarchar (150) NULL ,
CONSTRAINT PK_Systems PRIMARY KEY CLUSTERED (
SystemId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.TbdRule') AND type='U')
DROP TABLE dbo.TbdRule
CREATE TABLE TbdRule ( 
TbdRuleId int IDENTITY(1,1) NOT NULL ,
ScreenId int NOT NULL ,
TbdRuleName nvarchar (100) NOT NULL ,
TbdRuleDesc nvarchar (max) NULL ,
CONSTRAINT PK_TbdRule PRIMARY KEY CLUSTERED (
TbdRuleId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Template') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_Template_TmplDefault' AND o.name = 'Template')
DROP INDEX Template.IX_Template_TmplDefault 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Template') AND type='U')
DROP TABLE dbo.Template
CREATE TABLE Template ( 
TemplateId smallint IDENTITY(1,1) NOT NULL ,
TemplateName nvarchar (30) NOT NULL ,
TmplPrefix varchar (10) NOT NULL ,
TmplDefault char (1) NOT NULL ,
AltTemplateId smallint NULL ,
CONSTRAINT PK_Template PRIMARY KEY CLUSTERED (
TemplateId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Usage') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_Usage_UsrId' AND o.name = 'Usage')
DROP INDEX Usage.IX_Usage_UsrId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Usage') AND type='U')
DROP TABLE dbo.Usage
CREATE TABLE Usage ( 
UsageDt datetime NOT NULL ,
UsrId int NOT NULL ,
UsageNote nvarchar (200) NULL ,
EntityTitle nvarchar (50) NOT NULL ,
ScreenId int NULL ,
ReportId int NULL ,
WizardId int NULL ,
Miscellaneous varchar (100) NULL 
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Usr') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_Usr' AND o.name = 'Usr')
DROP INDEX Usr.IU_Usr 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Usr') AND type='U')
DROP TABLE dbo.Usr
CREATE TABLE Usr ( 
UsrId int IDENTITY(1,1) NOT NULL ,
LoginName nvarchar (32) NOT NULL ,
UsrEmail nvarchar (50) NULL ,
UsrMobile varchar (50) NULL ,
UsrName nvarchar (50) NULL ,
UsrPassword varbinary (32) NULL ,
ConfirmPwd varbinary (32) NULL ,
ExtPassword varbinary (255) NULL ,
CultureId smallint NOT NULL CONSTRAINT DF_Usr_CultureId DEFAULT ((1)),
InternalUsr char (1) NOT NULL CONSTRAINT DF_Usr_InternalUsr DEFAULT ('N'),
TechnicalUsr char (1) NOT NULL CONSTRAINT DF_Usr_TechnicalUsr DEFAULT ('N'),
UsrGroupLs varchar (1000) NOT NULL ,
CompanyLs varchar (1000) NULL ,
ProjectLs varchar (1000) NULL ,
InvestorId int NULL ,
CustomerId int NULL ,
VendorId int NULL ,
AgentId int NULL ,
BrokerId int NULL ,
MemberId int NULL ,
LenderId int NULL ,
BorrowerId int NULL ,
GuarantorId int NULL ,
DefSystemId tinyint NOT NULL ,
DefProjectId int NULL ,
DefCompanyId int NULL ,
FailedAttempt tinyint NULL ,
FailedAttemptFP tinyint NULL ,
LastFailedDt datetime NULL ,
LastSuccessDt datetime NULL ,
LastPwdChgDt datetime NULL ,
HintQuestionId tinyint NULL ,
HintAnswer nvarchar (50) NULL ,
Active char (1) NOT NULL ,
ModifiedOn datetime NOT NULL ,
UsrImprLink varchar (200) NULL ,
IPAlert char (1) NOT NULL CONSTRAINT DF_Usr_IPAlert DEFAULT ('N'),
PwdNoRepeat smallint NOT NULL CONSTRAINT DF_Usr_PwdNoRepeat DEFAULT ((1)),
PwdDuration smallint NOT NULL CONSTRAINT DF_Usr_PwdDuration DEFAULT ((9999)),
PwdWarn smallint NOT NULL CONSTRAINT DF_Usr_PwdWarn DEFAULT ((0)),
PicMed varbinary (max) NULL ,
OTPSecret varchar (64) NULL ,
TwoFactorAuth char (1) NOT NULL CONSTRAINT DF_Usr_TwoFactorAuth DEFAULT ('N'),
CONSTRAINT PK_Usr PRIMARY KEY CLUSTERED (
UsrId,
CultureId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrAudit') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_UsrAudit' AND o.name = 'UsrAudit')
DROP INDEX UsrAudit.IX_UsrAudit 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrAudit') AND type='U')
DROP TABLE dbo.UsrAudit
CREATE TABLE UsrAudit ( 
AttemptDt datetime NOT NULL ,
LoginName nvarchar (32) NOT NULL ,
UsrId int NULL ,
IpAddress varchar (50) NOT NULL ,
LoginSuccess char (1) NOT NULL ,
CONSTRAINT PK_UsrAudit PRIMARY KEY CLUSTERED (
AttemptDt
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrDevice') AND type='U')
DROP TABLE dbo.UsrDevice
CREATE TABLE UsrDevice ( 
UsrDeviceId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
DeviceId varchar (max) NULL ,
PushNotificationType varchar (5) NULL ,
UsrAgent varchar (max) NULL ,
LastAccessOnUTC datetime NULL ,
Status char (1) NOT NULL CONSTRAINT DF_UsrDevice_Status DEFAULT ('Y'),
Fingerprint varchar (max) NULL ,
AppSig varchar (max) NULL ,
ClientIP varchar (max) NULL ,
CONSTRAINT PK_UsrDevice PRIMARY KEY CLUSTERED (
UsrDeviceId
)
)

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrGroup') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_UsrGroup_UsrGroupName' AND o.name = 'UsrGroup')
DROP INDEX UsrGroup.IX_UsrGroup_UsrGroupName 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrGroup') AND type='U')
DROP TABLE dbo.UsrGroup
CREATE TABLE UsrGroup ( 
UsrGroupId smallint IDENTITY(1,1) NOT NULL ,
UsrGroupName nvarchar (50) NOT NULL ,
RowAuthorityId smallint NOT NULL ,
CompanyId int NULL ,
CONSTRAINT PK_UsrGroup PRIMARY KEY CLUSTERED (
UsrGroupId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrGroupAuth') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_UsrGroupAuth' AND o.name = 'UsrGroupAuth')
DROP INDEX UsrGroupAuth.IX_UsrGroupAuth 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrGroupAuth') AND type='U')
DROP TABLE dbo.UsrGroupAuth
CREATE TABLE UsrGroupAuth ( 
UsrGroupAuthId int IDENTITY(1,1) NOT NULL ,
UsrGroupId smallint NOT NULL ,
CompanyId int NULL ,
ProjectId smallint NULL ,
SystemId tinyint NULL ,
SysRowAuthorityId smallint NOT NULL ,
CONSTRAINT PK_UsrGroupAuth PRIMARY KEY CLUSTERED (
UsrGroupAuthId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrImpr') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrImpr') AND type='U')
DROP TABLE dbo.UsrImpr
CREATE TABLE UsrImpr ( 
UsrImprId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
ImprUsrId int NOT NULL ,
UsrImprDesc nvarchar (200) NULL ,
UsrImprLink varchar (200) NULL ,
TestCulture varchar (10) NULL ,
TestCurrency money NULL ,
SignOff varbinary (max) NULL ,
InputBy int NULL ,
InputOn datetime NULL ,
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
CONSTRAINT PK_UsrImpr PRIMARY KEY CLUSTERED (
UsrImprId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrPref') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrPref') AND type='U')
DROP TABLE dbo.UsrPref
CREATE TABLE UsrPref ( 
UsrPrefId int IDENTITY(1,1) NOT NULL ,
UsrId int NULL ,
UsrGroupId smallint NULL ,
CompanyId int NULL ,
MemberId int NULL ,
AgentId int NULL ,
BrokerId int NULL ,
CustomerId int NULL ,
InvestorId int NULL ,
VendorId int NULL ,
LenderId int NULL ,
BorrowerId int NULL ,
GuarantorId int NULL ,
ProjectId int NULL ,
SystemId tinyint NULL ,
PrefDefault char (1) NOT NULL ,
UsrPrefDesc nvarchar (50) NOT NULL ,
SysListVisible char (1) NOT NULL ,
ComListVisible char (1) NOT NULL ,
PrjListVisible char (1) NOT NULL CONSTRAINT DF_UsrPref_PrjListVisible DEFAULT ('N'),
MenuOptId int NOT NULL ,
UsrStyleSheet varchar (max) NULL ,
MasterPgFile varchar (100) NULL ,
LoginImage varchar (200) NULL ,
MobileImage varchar (200) NULL ,
SampleImage varbinary (max) NULL ,
CONSTRAINT PK_UsrPref PRIMARY KEY CLUSTERED (
UsrPrefId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrProvider') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_Provider_LoginName' AND o.name = 'UsrProvider')
DROP INDEX UsrProvider.IX_Provider_LoginName 
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_UsrId' AND o.name = 'UsrProvider')
DROP INDEX UsrProvider.IX_UsrId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrProvider') AND type='U')
DROP TABLE dbo.UsrProvider
CREATE TABLE UsrProvider ( 
UsrProviderId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
ProviderCd varchar (5) NOT NULL ,
LoginName nvarchar (200) NOT NULL ,
LoginMeta nvarchar (max) NULL ,
Remark nvarchar (max) NULL ,
CreatedOn datetime NOT NULL CONSTRAINT DF_UsrProvider_CreatedOn DEFAULT (getutcdate()),
DisabledOn datetime NULL ,
Active char (1) NOT NULL CONSTRAINT DF_UsrProvider_Active DEFAULT ('Y'),
CONSTRAINT PK_UsrProvider PRIMARY KEY CLUSTERED (
UsrProviderId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrPwd') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrPwd') AND type='U')
DROP TABLE dbo.UsrPwd
CREATE TABLE UsrPwd ( 
UsrId int NOT NULL ,
LastCnt smallint NOT NULL ,
UsrPassword varbinary (32) NOT NULL ,
CONSTRAINT PK_UsrPwd PRIMARY KEY CLUSTERED (
UsrId,
LastCnt
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrSafeIP') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UsrSafeIP') AND type='U')
DROP TABLE dbo.UsrSafeIP
CREATE TABLE UsrSafeIP ( 
UsrId int NOT NULL ,
IpAddress varchar (50) NOT NULL ,
CONSTRAINT PK_UsrSafeIP PRIMARY KEY CLUSTERED (
UsrId,
IpAddress
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReport') and type='U')
BEGIN
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_UtReport_ProgramName' AND o.name = 'UtReport')
DROP INDEX UtReport.IX_UtReport_ProgramName 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReport') AND type='U')
DROP TABLE dbo.UtReport
CREATE TABLE UtReport ( 
ReportId int IDENTITY(1,1) NOT NULL ,
ProgramName varchar (20) NOT NULL ,
ReportDesc nvarchar (50) NULL ,
OrientationCd char (1) NOT NULL ,
ReportTypeCd char (1) NOT NULL ,
TemplateName varchar (50) NULL ,
RptTemplate int NULL ,
CopyReportId int NULL ,
TopMargin decimal (8,2) NOT NULL CONSTRAINT DF_UtReport_TopMargin DEFAULT ((1)),
BottomMargin decimal (8,2) NOT NULL CONSTRAINT DF_UtReport_BottomMargin DEFAULT ((1)),
LeftMargin decimal (8,2) NOT NULL CONSTRAINT DF_UtReport_LeftMargin DEFAULT ((1)),
RightMargin decimal (8,2) NOT NULL CONSTRAINT DF_UtReport_RightMargin DEFAULT ((1)),
PageHeight decimal (8,2) NOT NULL CONSTRAINT DF_UtReport_PageHeight DEFAULT ((11)),
PageWidth decimal (8,2) NOT NULL CONSTRAINT DF_UtReport_PageWidth DEFAULT ((8.5)),
UnitCd char (2) NOT NULL CONSTRAINT DF_UtReport_UnitCd DEFAULT ('in'),
WhereClause varchar (1000) NULL ,
RegClause varchar (400) NULL ,
RegCode nvarchar (max) NULL ,
ValClause varchar (400) NULL ,
ValCode nvarchar (max) NULL ,
UpdClause varchar (200) NULL ,
UpdCode nvarchar (max) NULL ,
XlsClause varchar (200) NULL ,
XlsCode nvarchar (max) NULL ,
GenerateRp char (1) NOT NULL ,
AllowSelect char (1) NOT NULL CONSTRAINT DF_UtReport_AllowSelect DEFAULT ('N'),
ModifiedBy int NULL ,
ModifiedOn datetime NULL ,
LastGenDt datetime NULL ,
CompanyLs varchar (1000) NULL ,
NeedRegen char (1) NOT NULL CONSTRAINT DF_UtReport_NeedRegen DEFAULT ('N'),
CONSTRAINT PK_UtReport PRIMARY KEY CLUSTERED (
ReportId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportCri') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportCri') AND type='U')
DROP TABLE dbo.UtReportCri
CREATE TABLE UtReportCri ( 
ReportCriId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
ReportCriDesc varchar (100) NULL ,
ReportGrpId int NOT NULL ,
LabelCss varchar (100) NULL ,
ContentCss varchar (100) NULL ,
TableId int NULL ,
TableAbbr varchar (10) NULL ,
ColumnName varchar (50) NULL ,
TabIndex smallint NOT NULL ,
DataTypeId tinyint NOT NULL ,
DataTypeSize smallint NULL ,
DisplayModeId tinyint NOT NULL ,
ColumnSize smallint NULL ,
RowSize smallint NULL ,
DdlKeyColumnName varchar (50) NULL ,
DdlRefColumnName varchar (50) NULL ,
DdlSrtColumnName varchar (50) NULL ,
DdlFtrColumnId int NULL ,
RequiredValid char (1) NOT NULL ,
WhereClause varchar (1000) NULL ,
RegClause varchar (400) NULL ,
CONSTRAINT PK_UtReportCri PRIMARY KEY CLUSTERED (
ReportCriId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportCriHlp') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportCriHlp') AND type='U')
DROP TABLE dbo.UtReportCriHlp
CREATE TABLE UtReportCriHlp ( 
ReportCriHlpId int IDENTITY(1,1) NOT NULL ,
ReportCriHlpDesc varchar (200) NULL ,
ReportCriId int NOT NULL ,
CultureId smallint NOT NULL ,
ColumnHeader nvarchar (50) NULL ,
CONSTRAINT PK_UtReportCriHlp PRIMARY KEY CLUSTERED (
ReportCriHlpId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportDel') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportDel') AND type='U')
DROP TABLE dbo.UtReportDel
CREATE TABLE UtReportDel ( 
UtReportDelId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
ProgramName varchar (100) NOT NULL ,
CONSTRAINT PK_UtReportDel PRIMARY KEY CLUSTERED (
UtReportDelId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportHlp') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportHlp') AND type='U')
DROP TABLE dbo.UtReportHlp
CREATE TABLE UtReportHlp ( 
ReportHlpId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
CultureId smallint NOT NULL ,
DefaultHlpMsg nvarchar (max) NOT NULL ,
ReportTitle nvarchar (50) NOT NULL ,
CONSTRAINT PK_UtReportHlp PRIMARY KEY CLUSTERED (
ReportHlpId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportLstCri') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportLstCri') AND type='U')
DROP TABLE dbo.UtReportLstCri
CREATE TABLE UtReportLstCri ( 
ReportLstCriId int IDENTITY(1,1) NOT NULL ,
UsrId int NOT NULL ,
ReportId int NOT NULL ,
ReportCriId int NOT NULL ,
LastCriteria nvarchar (900) NULL ,
CONSTRAINT PK_UtReportLstCri PRIMARY KEY CLUSTERED (
ReportLstCriId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportObj') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportObj') AND type='U')
DROP TABLE dbo.UtReportObj
CREATE TABLE UtReportObj ( 
ReportObjId int IDENTITY(1,1) NOT NULL ,
ReportId int NOT NULL ,
RptObjTypeCd char (1) NOT NULL CONSTRAINT DF_UtReportObj_RptObjTypeCd DEFAULT ('F'),
ColumnName varchar (50) NULL ,
ColumnDesc varchar (100) NULL ,
TabIndex smallint NOT NULL ,
ColumnFormat varchar (20) NULL ,
PaddSize tinyint NULL ,
PaddChar nchar (1) NULL ,
DataTypeId tinyint NOT NULL ,
OperatorId tinyint NULL ,
ReportCriId int NULL ,
CONSTRAINT PK_UtReportObj PRIMARY KEY CLUSTERED (
ReportObjId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportObjHlp') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtReportObjHlp') AND type='U')
DROP TABLE dbo.UtReportObjHlp
CREATE TABLE UtReportObjHlp ( 
ReportObjHlpId int IDENTITY(1,1) NOT NULL ,
ReportObjHlpDesc varchar (200) NULL ,
ReportObjId int NOT NULL ,
CultureId smallint NOT NULL ,
ColumnHeader nvarchar (50) NULL ,
HeaderWidth smallint NULL ,
CONSTRAINT PK_UtReportObjHlp PRIMARY KEY CLUSTERED (
ReportObjHlpId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptCel') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptCel') AND type='U')
DROP TABLE dbo.UtRptCel
CREATE TABLE UtRptCel ( 
RptCelId int IDENTITY(1,1) NOT NULL ,
RptCelDesc nvarchar (200) NULL ,
RptTblId int NOT NULL ,
RowNum smallint NOT NULL ,
RowHeight decimal (8,2) NOT NULL ,
RowVisibility varchar (1000) NULL ,
CelNum smallint NOT NULL ,
CelColSpan smallint NULL ,
CONSTRAINT PK_UtRptCel PRIMARY KEY CLUSTERED (
RptCelId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptCha') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptCha') AND type='U')
DROP TABLE dbo.UtRptCha
CREATE TABLE UtRptCha ( 
RptChaId int IDENTITY(1,1) NOT NULL ,
RptChaDesc nvarchar (200) NULL ,
RptCtrId int NOT NULL ,
ReportId int NULL ,
RptChaTypeCd char (2) NOT NULL ,
ThreeD char (1) NOT NULL ,
CategoryGrp int NOT NULL ,
ChartData nvarchar (1000) NOT NULL ,
SeriesGrp int NULL ,
CONSTRAINT PK_UtRptCha PRIMARY KEY CLUSTERED (
RptChaId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptCtr') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptCtr') AND type='U')
DROP TABLE dbo.UtRptCtr
CREATE TABLE UtRptCtr ( 
RptCtrId int IDENTITY(1,1) NOT NULL ,
RptCtrDesc nvarchar (200) NULL ,
PRptCtrId int NULL ,
RptElmId int NULL ,
RptCelId int NULL ,
ReportId int NOT NULL ,
RptCtrTypeCd char (1) NOT NULL ,
RptCtrName nvarchar (100) NOT NULL ,
RptStyleId int NULL ,
CtrTop decimal (8,2) NULL ,
CtrLeft decimal (8,2) NULL ,
CtrHeight decimal (8,2) NULL ,
CtrWidth decimal (8,2) NULL ,
CtrZIndex smallint NULL ,
CtrAction varchar (500) NULL ,
CtrGrouping int NULL ,
CtrVisibility char (1) NULL ,
CtrToggle int NULL ,
CtrToolTip nvarchar (200) NULL ,
CtrPgBrStart char (1) NOT NULL ,
CtrPgBrEnd char (1) NOT NULL ,
CtrValue nvarchar (1000) NULL ,
CtrCanGrow char (1) NOT NULL ,
CtrCanShrink char (1) NOT NULL ,
CtrTogether char (1) NOT NULL ,
CONSTRAINT PK_UtRptCtr PRIMARY KEY CLUSTERED (
RptCtrId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptElm') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptElm') AND type='U')
DROP TABLE dbo.UtRptElm
CREATE TABLE UtRptElm ( 
RptElmId int IDENTITY(1,1) NOT NULL ,
RptElmDesc nvarchar (200) NULL ,
ReportId int NOT NULL ,
RptElmTypeCd char (1) NOT NULL ,
RptStyleId int NULL ,
ElmHeight decimal (8,2) NOT NULL ,
ElmColumns smallint NULL ,
ElmColSpacing decimal (8,2) NULL ,
ElmPrintFirst char (1) NOT NULL ,
ElmPrintLast char (1) NOT NULL ,
CONSTRAINT PK_UtRptElm PRIMARY KEY CLUSTERED (
RptElmId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptMemCri') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptMemCri') AND type='U')
DROP TABLE dbo.UtRptMemCri
CREATE TABLE UtRptMemCri ( 
RptMemCriId int IDENTITY(1,1) NOT NULL ,
RptMemFldId int NOT NULL ,
RptMemCriName nvarchar (200) NOT NULL ,
RptMemCriDesc nvarchar (500) NULL ,
RptMemCriLink varchar (200) NULL ,
ReportId int NOT NULL ,
UsrId int NULL ,
InputBy int NULL ,
InputOn datetime NULL ,
ModifiedOn datetime NULL ,
ViewedOn datetime NULL ,
CompanyLs varchar (1000) NULL ,
CONSTRAINT PK_UtRptMemCri PRIMARY KEY CLUSTERED (
RptMemCriId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptMemCriDtl') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptMemCriDtl') AND type='U')
DROP TABLE dbo.UtRptMemCriDtl
CREATE TABLE UtRptMemCriDtl ( 
RptMemCriDtlId int IDENTITY(1,1) NOT NULL ,
RptMemCriId int NOT NULL ,
ReportCriId int NOT NULL ,
MemCriteria nvarchar (900) NULL ,
CONSTRAINT PK_UtRptMemCriDtl PRIMARY KEY CLUSTERED (
RptMemCriDtlId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptMemFld') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptMemFld') AND type='U')
DROP TABLE dbo.UtRptMemFld
CREATE TABLE UtRptMemFld ( 
RptMemFldId int IDENTITY(1,1) NOT NULL ,
RptMemFldName nvarchar (200) NOT NULL ,
UsrId int NULL ,
InputBy int NULL ,
CompanyLs varchar (1000) NULL ,
AccessCd char (1) NOT NULL CONSTRAINT DF_UtRptMemFld_AccessCd DEFAULT ('V'),
CONSTRAINT PK_UtRptMemFld PRIMARY KEY CLUSTERED (
RptMemFldId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptTbl') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.UtRptTbl') AND type='U')
DROP TABLE dbo.UtRptTbl
CREATE TABLE UtRptTbl ( 
RptTblId int IDENTITY(1,1) NOT NULL ,
RptTblDesc nvarchar (200) NULL ,
ParentId int NULL ,
RptCtrId int NOT NULL ,
ReportId int NULL ,
RptTblTypeCd char (1) NOT NULL ,
TblRepeatNew char (1) NOT NULL ,
TblOrder smallint NULL ,
ColWidth decimal (8,2) NULL ,
TblGrouping int NULL ,
TblVisibility char (1) NULL ,
TblToggle int NULL ,
CONSTRAINT PK_UtRptTbl PRIMARY KEY CLUSTERED (
RptTblId
)
)
END

GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.utServerRuleOvrd') and type='U')
BEGIN
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.utServerRuleOvrd') AND type='U')
DROP TABLE dbo.utServerRuleOvrd
CREATE TABLE utServerRuleOvrd ( 
AtServerRuleOvrdId int NOT NULL ,
ServerRuleOvrdDesc varchar (1000) NULL ,
ServerRuleOvrdName varchar (500) NOT NULL ,
ServerRuleId int NOT NULL ,
Disable char (1) NOT NULL ,
ServerRuleGuid varchar (50) NULL ,
ScreenId int NULL ,
Priority smallint NULL ,
Guid varchar (50) NOT NULL CONSTRAINT DF_utServerRuleOvrd_Guid DEFAULT (newid()),
RunMode char (1) NULL ,
CONSTRAINT PK_utServerRuleOvrd PRIMARY KEY CLUSTERED (
AtServerRuleOvrdId
)
)
END

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.utServerRuleOvrdPrm') AND type='U')
DROP TABLE dbo.utServerRuleOvrdPrm
CREATE TABLE utServerRuleOvrdPrm ( 
ServerRuledOvrdPrmId int IDENTITY(1,1) NOT NULL ,
PermKeyId smallint NOT NULL ,
AndCondition char (1) NOT NULL ,
AtServerRuleOvrdId int NOT NULL CONSTRAINT DF_utServerRuleOvrdPrm_AtServerRuleOvrdId DEFAULT ((-1)),
Match char (1) NOT NULL CONSTRAINT DF_utServerRuleOvrdPrm_Match DEFAULT ('Y'),
PermKeyRowId int NULL ,
PermId int NULL ,
Guid varchar (50) NOT NULL CONSTRAINT DF_utServerRuleOvrdPrm_Guid DEFAULT (newid()),
AtServerRuleOvrdGuid varchar (50) NULL ,
CONSTRAINT PK_utServerRuleOvrdPrm PRIMARY KEY CLUSTERED (
ServerRuledOvrdPrmId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.WebRule') AND type='U')
DROP TABLE dbo.WebRule
CREATE TABLE WebRule ( 
WebRuleId int IDENTITY(1,1) NOT NULL ,
RuleTypeId tinyint NOT NULL ,
RuleName nvarchar (100) NOT NULL ,
RuleDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
ScreenId int NOT NULL ,
ScreenObjId int NULL ,
ButtonTypeId tinyint NULL ,
EventId tinyint NOT NULL ,
WebRuleProg nvarchar (max) NOT NULL ,
ReactEventId tinyint NULL ,
ReactRuleProg nvarchar (max) NULL ,
ReduxEventId tinyint NULL ,
ReduxRuleProg nvarchar (max) NULL ,
ServiceEventId tinyint NULL ,
ServiceRuleProg nvarchar (max) NULL ,
AsmxEventId tinyint NULL ,
AsmxRuleProg nvarchar (max) NULL ,
ForCompanyId int NULL ,
CONSTRAINT PK_WebRule PRIMARY KEY CLUSTERED (
WebRuleId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IU_Wizard' AND o.name = 'Wizard')
DROP INDEX Wizard.IU_Wizard 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.Wizard') AND type='U')
DROP TABLE dbo.Wizard
CREATE TABLE Wizard ( 
WizardId int IDENTITY(1,1) NOT NULL ,
WizardTypeId tinyint NOT NULL ,
MasterTableId int NOT NULL ,
WizardTitle nvarchar (50) NOT NULL ,
ProgramName varchar (50) NOT NULL ,
DefWorkSheet nvarchar (50) NOT NULL ,
DefStartRow smallint NOT NULL ,
DefOverwrite char (1) NOT NULL ,
OvwrReadonly char (1) NOT NULL ,
NeedRegen char (1) NOT NULL CONSTRAINT DF_Wizard_NeedRegen DEFAULT ('N'),
AuthRequired char (1) NOT NULL CONSTRAINT DF_Wizard_AuthRequired DEFAULT ('Y'),
NoTrans char (1) NOT NULL CONSTRAINT DF_Wizard_NoTrans DEFAULT ('N'),
CommandTimeOut smallint NULL ,
CONSTRAINT PK_Wizard PRIMARY KEY CLUSTERED (
WizardId
)
)

GO
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.WizardDel') AND type='U')
DROP TABLE dbo.WizardDel
CREATE TABLE WizardDel ( 
WizardDelId int IDENTITY(1,1) NOT NULL ,
WizardId int NOT NULL ,
ProgramName varchar (100) NOT NULL ,
CONSTRAINT PK_WizardDel PRIMARY KEY CLUSTERED (
WizardDelId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_WizardObj_WizardId' AND o.name = 'WizardObj')
DROP INDEX WizardObj.IX_WizardObj_WizardId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.WizardObj') AND type='U')
DROP TABLE dbo.WizardObj
CREATE TABLE WizardObj ( 
WizardObjId int IDENTITY(1,1) NOT NULL ,
WizardId int NOT NULL ,
ColumnId int NULL ,
ColumnDesc varchar (100) NULL ,
TabIndex smallint NOT NULL ,
CONSTRAINT PK_WizardObj PRIMARY KEY CLUSTERED (
WizardObjId
)
)

GO
IF EXISTS (SELECT i.name FROM sysindexes i INNER JOIN sysobjects o ON i.id = o.id WHERE i.name = 'IX_WizardRule_WizardId' AND o.name = 'WizardRule')
DROP INDEX WizardRule.IX_WizardRule_WizardId 
IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'dbo.WizardRule') AND type='U')
DROP TABLE dbo.WizardRule
CREATE TABLE WizardRule ( 
WizardRuleId int IDENTITY(1,1) NOT NULL ,
WizardId int NOT NULL ,
RuleTypeId tinyint NOT NULL ,
RuleName nvarchar (100) NOT NULL ,
RuleDesc nvarchar (150) NULL ,
RuleDescription nvarchar (500) NULL ,
RuleOrder smallint NOT NULL ,
ProcedureName varchar (50) NOT NULL ,
BeforeCRUD char (1) NOT NULL ,
CONSTRAINT PK_WizardRule PRIMARY KEY CLUSTERED (
WizardRuleId
)
)

GO