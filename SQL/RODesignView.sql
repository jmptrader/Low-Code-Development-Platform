if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.SrUsr') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.SrUsr AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[SrUsr] AS
SELECT UsrId, UsrTitle = LoginName + ISNULL(' [' + UsrName + ']','') + CASE WHEN Active='N' THEN ' (Inactive)' ELSE '' END
, UsrDetail = ISNULL(UsrEmail,'') + ISNULL(' ' + UsrMobile,'')
, UsrName
, UsrEmail
, PicMed, ExtPassword
, CultureId, UsrGroupLs, CompanyLs, ProjectLs, InvestorId, CustomerId, VendorId, AgentId, BrokerId, MemberId, LenderId, BorrowerId, GuarantorId, Active
FROM dbo.Usr
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwAppItem') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwAppItem AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwAppItem] AS
	SELECT a.AppInfoDesc, b.ItemOrder, b.DbProviderCd, b.AppItemName, b.MultiDesignDb, b.AppItemCode
	FROM dbo.AppInfo a INNER JOIN dbo.AppItem b ON a.AppInfoId = b.AppInfoId
	WHERE a.VersionDt is not null AND b.ObjectTypeCd = 'D'
	--full history
	--20 year max of history
	--AND a.VersionDt > dateadd(mm,-240,convert(datetime,convert(varchar,getdate(),102)))
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwCheckBox') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwCheckBox AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwCheckBox] AS
select CultureId = CultureTypeId, CheckBoxCd = ISNULL(y.CheckboxCd, z.CheckBoxCd), CheckBoxName = ISNULL(y.CheckBoxName, z.CheckBoxName)
from dbo.ctCulture x
left outer join dbo.CtCheckBox y on x.CultureTypeId = y.CultureId
left outer join dbo.CtCheckBox z on y.CultureId is null and z.CultureId = 1
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwClnAppItem') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwClnAppItem AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwClnAppItem] AS
	SELECT a.AppInfoDesc, b.ItemOrder, b.ObjectTypeCd, b.AppItemName, b.AppItemCode
	FROM dbo.AppInfo a INNER JOIN dbo.AppItem b ON a.AppInfoId = b.AppInfoId
	WHERE a.VersionDt is not null AND b.ObjectTypeCd = 'C'
-- full history
--	AND a.VersionDt > dateadd(mm,-120,convert(datetime,convert(varchar,getdate(),102)))
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwCompanyPref') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwCompanyPref AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwCompanyPref]
AS
select 
c.CompanyId, c.CompanyLogo, c.CompPrefDesc
, s.WebAddress
, LogoUrl = REPLACE(c.CompanyLogo, '~/', ISNULL(s.WebAddress + '/',''))
from
dbo.CompPref c
left outer join dbo.Systems s on s.SystemId = 3
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwCompanyPref') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwCompanyPref AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwCompanyPref]
AS
select 
c.CompanyId, c.CompanyLogo, c.CompPrefDesc
, s.WebAddress
, LogoUrl = REPLACE(c.CompanyLogo, '~/', ISNULL(s.WebAddress + '/',''))
from
dbo.CompPref c
left outer join dbo.Systems s on s.SystemId = 3
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwCulture') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwCulture AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwCulture] AS
	SELECT CultureId = CultureTypeId, CultureTypeName, CultureTypeDesc, CultureDefault, EnNumberRule, CountryCd, CurrencyCd
	FROM dbo.CtCulture
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwCultureLbl') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwCultureLbl AS SELECT DUMMY=1')
GO
-- ??Design only:
ALTER VIEW [dbo].[VwCultureLbl] AS
	SELECT a.CultureLblId, a.CultureTypeId, a.CultureId, a.CultureTypeLabel, b.CultureTypeName, b.CountryCd, b.CurrencyCd
	FROM dbo.CtCultureLbl a INNER JOIN dbo.CtCulture b ON a.CultureTypeId = b.CultureTypeId
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwDisplayType') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwDisplayType AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwDisplayType] AS SELECT TypeId, TypeName, TypeDesc, DisplayDefault FROM dbo.CtDisplayType WHERE TypeId IN (1,3,4,5,17,18,20,33,35,38,40,44,52)
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwIntUsr') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwIntUsr AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwIntUsr] AS
SELECT UsrId, LoginName, UsrName, MemberId, Active, PicMed FROM dbo.Usr WHERE InternalUsr = 'Y'
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwLabel') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwLabel AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwLabel] AS
SELECT LabelId, CultureId, LabelCat, LabelKey = '[['+LabelKey+']]', LabelText, SortOrder
	, LabelLink = 'AdmLabel.aspx?col=LabelId&typ=N&key=' + convert(varchar(20),LabelId)
	FROM dbo.Label
	WHERE CompanyId is null
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwLinkType') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwLinkType AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwLinkType] AS SELECT LinkTypeCd, LinkTypeName, LinkTypeDesc FROM dbo.CtLinkType WHERE UsrDefined = 'Y'
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwMailProfile') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwMailProfile AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwMailProfile]
AS
SELECT ProfileNm = [name] FROM msdb.dbo.sysmail_profile
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwMIMEType') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwMIMEType AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwMIMEType] AS

SELECT
MimeTypeId, MimeType, MimeTypeDesc=ISNULL(MimeTypeDesc, MimeType)
FROM
(VALUES 
(1, 'text/plain', NULL )
,(2, 'application/json', NULL )
,(3, 'application/x-www-form-urlencoded', NULL )
) 
x (MimeTypeId, MimeType, MimeTypeDesc)
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwNumSuccess') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwNumSuccess AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwNumSuccess] AS
SELECT LoginName, LoginSuccess, NumAttempt = count(1) FROM dbo.UsrAudit GROUP BY LoginName, LoginSuccess
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwPermKey') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwPermKey AS SELECT DUMMY=1')
GO
-- ??Design only
ALTER VIEW [dbo].[VwPermKey]
AS
SELECT PermKeyId = CtPermKeyId, PermKeyDesc = TableName
FROM dbo.CtPermKey
WHERE CtPermKeyId IN (1,2,3,4,6,7,8,9,10,11,12,18,19,20)
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwPermKeyRow') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwPermKeyRow AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwPermKeyRow]
AS
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + a.AgentId, PermKeyId=p.CtPermKeyId, PermId=AgentId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + AgentName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId, BrokerId=NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Agent a INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 1
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + b.BrokerId, PermKeyId=p.CtPermKeyId, PermId=BrokerId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + BrokerName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Broker b INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 2
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + c.CultureId, PermKeyId=p.CtPermKeyId, PermId=CultureId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + CultureTypeDesc
, Active='Y'
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM dbo.VwCulture c INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 3
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + cu.CustomerId, PermKeyId=p.CtPermKeyId, PermId=CustomerId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + CustomerName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Customer cu INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 4
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + i.InvestorId, PermKeyId=p.CtPermKeyId, PermId=InvestorId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + InvestorName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL, InvestorId, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Investor i INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 6
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + m.MemberId, PermKeyId=p.CtPermKeyId, PermId=MemberId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + MemberName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL, InvestorId = NULL, MemberId, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Member m INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 7
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + u.UsrGroupId, PermKeyId=p.CtPermKeyId, PermId=UsrGroupId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + UsrGroupName
, Active='Y'
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId, CompanyId, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId=NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM dbo.UsrGroup u INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 8
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + v.VendorId, PermKeyId=p.CtPermKeyId, PermId=VendorId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + VendorName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Vendor v INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 9
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + ln.LenderId, PermKeyId=p.CtPermKeyId, PermId=LenderId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + LenderName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId=null, LenderId, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Lender ln INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 18
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + br.BorrowerId, PermKeyId=p.CtPermKeyId, PermId=BorrowerId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + BorrowerName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId=null, LenderId = NULL, BorrowerId, GuarantorId = NULL
FROM ROCmon.dbo.Borrower br INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 19
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + ga.GuarantorId, PermKeyId=p.CtPermKeyId, PermId=GuarantorId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + GuarantorName
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId=null, LenderId = NULL, BorrowerId = NULL, GuarantorId
FROM ROCmon.dbo.Guarantor ga INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 20
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + c.CompanyId, PermKeyId=p.CtPermKeyId, PermId=CompanyId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + CompanyDesc
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Company c INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 10
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + pj.ProjectId, PermKeyId=p.CtPermKeyId, PermId=ProjectId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + ProjectDesc
, Active
, UsrId=NULL, CompanyLs=NULL, ProjectLs=NULL, UsrGroupId = NULL, CompanyId = NULL, ProjectId, CultureId = NULL
, AgentId = NULL, BrokerId = NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM ROCmon.dbo.Project pj
INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 11
UNION
SELECT PermKeyRowId = p.CtPermKeyId * 100000000 + u.UsrId, PermKeyId=p.CtPermKeyId, PermId=UsrId, PermIdText = UPPER(LEFT(p.TableName,4)) + ': ' + UsrName
, Active
, UsrId, CompanyLs, ProjectLs, UsrGroupId = NULL, CompanyId = NULL, ProjectId = NULL, CultureId = NULL
, AgentId = NULL, BrokerId=NULL, CustomerId=NULL,  InvestorId = NULL, MemberId = NULL, VendorId = NULL, LenderId = NULL, BorrowerId = NULL, GuarantorId = NULL
FROM dbo.Usr u INNER JOIN dbo.CtPermKey p on p.CtPermKeyId = 12
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwReportCriHlp') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwReportCriHlp AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwReportCriHlp]
AS
SELECT a.ReportCriHlpId, a.ReportCriHlpDesc, a.ReportCriId, b.ReportId, a.CultureId
FROM dbo.ReportCriHlp a INNER JOIN dbo.ReportCri b ON a.ReportCriId = b.ReportCriId
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwRintagi') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwRintagi AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwRintagi] AS
SELECT 
s.*
FROM dbo.Systems s
WHERE s.SysProgram = 'Y' AND s.Active = 'Y'
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwRowAuth') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwRowAuth AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwRowAuth] AS
	SELECT RowAuthId, RowAuthName, AllowSel, AllowAdd, AllowUpd, AllowDel, SysAdmin, OvrideId
	FROM dbo.CtRowAuth
	UNION
	SELECT RowAuthId, RowAuthName, AllowSel, AllowAdd, AllowUpd, AllowDel, SysAdmin, OvrideId
	FROM dbo.AtRowAuth
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwRulAppItem') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwRulAppItem AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwRulAppItem] AS
	SELECT a.AppInfoDesc, b.ItemOrder, b.ObjectTypeCd, b.AppItemName, b.AppItemCode
	FROM dbo.AppInfo a INNER JOIN dbo.AppItem b ON a.AppInfoId = b.AppInfoId
	WHERE a.VersionDt is not null AND b.ObjectTypeCd = 'R'
-- full history
--	AND a.VersionDt > dateadd(mm,-120,convert(datetime,convert(varchar,getdate(),102)))
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwScrButton') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwScrButton AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwScrButton]
AS
SELECT     ButtonTypeId, ButtonTypeDesc
FROM         dbo.CtButtonType
WHERE ObjectType = 'S'
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwScreenCriHlp') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwScreenCriHlp AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwScreenCriHlp]
AS
SELECT a.ScreenCriHlpId, a.ScreenCriHlpDesc, a.ScreenCriId, b.ScreenId, a.CultureId
FROM dbo.ScreenCriHlp a INNER JOIN dbo.ScreenCri b ON a.ScreenCriId = b.ScreenCriId
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwScreenObjHlp') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwScreenObjHlp AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwScreenObjHlp]
AS
SELECT a.ScreenObjHlpId, a.ScreenObjHlpDesc, a.ScreenObjId, b.ScreenId, a.CultureId
FROM dbo.ScreenObjHlp a INNER JOIN dbo.ScreenObj b ON a.ScreenObjId = b.ScreenObjId
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwServerRuleRunMode') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwServerRuleRunMode AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwServerRuleRunMode]
AS
SELECT
RunModeCd, RunModeDesc, SrtOrder
FROM
(VALUES 
('S', 'Suppress This Rule', 3)
,('B', 'Before Upstream', 1)
,('A', 'After Upstream', 2)
) 
x (RunModeCd, RunModeDesc, SrtOrder)
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwTimeZoneInfo') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwTimeZoneInfo AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwTimeZoneInfo] AS 

SELECT 
id = ROW_NUMBER() OVER (ORDER BY current_utc_offset, name),
tz_desc = name + ' (UTC ' + current_utc_offset + CASE WHEN tz.is_currently_dst = 1 THEN ' DST' ELSE '' END + ')', *
FROM
sys.time_zone_info tz
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwUsr') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwUsr AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwUsr] AS
SELECT UsrId, LoginName, UsrName, UsrEmail, UsrPassword, Active FROM dbo.Usr
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'dbo.VwUsrAUdit') and OBJECTPROPERTY(id, N'IsView') = 1)
EXEC('CREATE VIEW dbo.VwUsrAUdit AS SELECT DUMMY=1')
GO
ALTER VIEW [dbo].[VwUsrAUdit] AS
SELECT AttemptDt, IpAddress, LoginName, ValidUser = CASE WHEN UsrId is NULL THEN 'N' ELSE 'Y' END, LoginSuccess FROM dbo.UsrAudit
GO