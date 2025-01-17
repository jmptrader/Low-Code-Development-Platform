namespace RO.Access3.Odbc
{
	using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Data;
	//using System.Data.OleDb;
    using System.Data.Odbc;
	using System.Drawing;
	using System.Text;
	using RO.Common3;
    using RO.Common3.Data;
	using RO.SystemFramewk;

	public class AdminAccess : AdminAccessBase, IDisposable
	{
		//private OleDbDataAdapter da;
        OdbcDataAdapter da;
        private int _CommandTimeout = 1800;
        public AdminAccess(int CommandTimeout = 1800)
		{
			//da = new OleDbDataAdapter();
            da = new OdbcDataAdapter();
            _CommandTimeout = CommandTimeout;
		}

        private static OdbcCommand TransformCmd(OdbcCommand cmd)
        {
            if (cmd.Parameters != null
                && cmd.Parameters.Count > 0
                && cmd.CommandType == CommandType.StoredProcedure
                && !string.IsNullOrEmpty(cmd.CommandText)
                && !cmd.CommandText.StartsWith("{CALL")
                )
            {
                cmd.CommandText = string.Format("{{CALL {0}({1})}}"
                    , cmd.CommandText
                    , string.Join(",", Enumerable.Repeat("?", cmd.Parameters.Count).ToArray())
                    );
            }
            return cmd;
        }
        private DateTime ToOleDbDatetime(DateTime d)
        {
            // MSOLEDBSQL cannot handle extra precisions, only ms
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second, d.Millisecond);
        }
		public override void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(true); // as a service to those who might inherit from us
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			if (da != null)
			{
				if(da.SelectCommand != null)
				{
					if( da.SelectCommand.Connection != null  )
					{
						da.SelectCommand.Connection.Dispose();
					}
					da.SelectCommand.Dispose();
				}    
				da.Dispose();
				da = null;
			}
		}

        // For screens:

        public override string GetMaintMsg()
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetMaintMsg", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ApplicationAssert.CheckCondition(dt.Rows.Count <= 1, "GetMaintMsg", "Maintenance Message", "Cannot obtain maintenance message. Please contact systems adminstrator ASAP.");
            return dt.Rows[0][0].ToString();
        }

        public override DataTable GetHomeTabs(Int32 UsrId, Int32 CompanyId, Int32 ProjectId, byte SystemId)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetHomeTabs", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = UsrId;
            cmd.Parameters.Add("@CompanyId", OdbcType.Numeric).Value = CompanyId;
            cmd.Parameters.Add("@ProjectId", OdbcType.Numeric).Value = ProjectId;
            cmd.Parameters.Add("@SystemId", OdbcType.Numeric).Value = SystemId;
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public override string SetCult(int UsrId, Int16 CultureId)
		{
			string rtn = string.Empty;
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("SetCult", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = UsrId;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			if (dt != null && dt.Rows.Count > 0) { rtn = dt.Rows[0][0].ToString(); }
			return rtn;
		}

		public override byte GetCult(string lang)
		{
			byte rtn = 1;
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("GetCult", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@lang", OdbcType.VarChar).Value = lang;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			if (dt != null && dt.Rows.Count > 0) { rtn = byte.Parse(dt.Rows[0][0].ToString()); }
			return rtn;
		}

		public override DataTable GetLang(Int16 CultureId)
		{
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("GetLang", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		public override DataTable GetLastPageInfo(Int32 screenId, Int32 usrId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetLastPageInfo", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		public override void UpdLastPageInfo(Int32 screenId, Int32 usrId, string lastPageInfo, string dbConnectionString, string dbPassword)
		{
			OdbcConnection cn =  new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
			cn.Open();
			OdbcCommand cmd = new OdbcCommand("UpdLastPageInfo", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
			if (Config.DoubleByteDb) {cmd.Parameters.Add("@LastPageInfo", OdbcType.NVarChar).Value = lastPageInfo;} else {cmd.Parameters.Add("@LastPageInfo", OdbcType.VarChar).Value = lastPageInfo;}
			cmd.CommandTimeout = _CommandTimeout;
			try {TransformCmd(cmd).ExecuteNonQuery();}
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "UpdLastPageInfo", "", e.Message.ToString()); }
			finally {cn.Close(); cmd.Dispose(); cmd = null;}
			return;
		}

        public override DataTable GetLastCriteria(Int32 screenId, Int32 reportId, Int32 usrId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetLastCriteria", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public override void DelLastCriteria(Int32 screenId, Int32 reportId, Int32 usrId, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("DelLastCriteria", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
            cmd.CommandTimeout = _CommandTimeout;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cn.Close(); }
            cmd.Dispose();
            cmd = null;
            return;
        }

        public override void IniLastCriteria(Int32 screenId, Int32 reportId, Int32 usrId, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("IniLastCriteria", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
            cmd.CommandTimeout = _CommandTimeout;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cn.Close(); }
            cmd.Dispose();
            cmd = null;
            return;
        }

        public override void DelDshFldDtl(string DshFldDtlId, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("DelDshFldDtl", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DshFldDtlId", OdbcType.Numeric).Value = DshFldDtlId;
            cmd.CommandTimeout = _CommandTimeout;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cn.Close(); }
            cmd.Dispose();
            cmd = null;
            return;
        }

        public override void DelDshFld(string DshFldId, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("DelDshFld", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@DshFldId", OdbcType.Numeric).Value = DshFldId;
            cmd.CommandTimeout = _CommandTimeout;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cn.Close(); }
            cmd.Dispose();
            cmd = null;
            return;
        }

        public override string UpdDshFld(string PublicAccess, string DshFldId, string DshFldName, Int32 usrId, string dbConnectionString, string dbPassword)
        {
            string rtn = string.Empty;
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("UpdDshFld", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            cmd.Parameters.Add("@PublicAccess", OdbcType.Char).Value = PublicAccess;
            if (DshFldId == string.Empty)
            {
                cmd.Parameters.Add("@DshFldId", OdbcType.Numeric).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DshFldId", OdbcType.Numeric).Value = DshFldId;
            }
            cmd.Parameters.Add("@DshFldName", OdbcType.NVarChar).Value = DshFldName;
            cmd.Parameters.Add("@usrId", OdbcType.Numeric).Value = usrId;
            try
            {
                da.SelectCommand = TransformCmd(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt != null && dt.Rows.Count > 0) { rtn = dt.Rows[0][0].ToString(); }
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cn.Close();
            }
            return rtn;
        }

		public override string GetSchemaScrImp(Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetSchemaScrImp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			StringBuilder sb = new StringBuilder();
			foreach(DataRow dr in dt.Rows){ sb.Append(dr[0].ToString()); }
			return sb.ToString();
		}

        public override string GetScrImpTmpl(Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetScrImpTmpl", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows) { sb.Append(dr[0].ToString()); }
            return sb.ToString();
        }

		public override DataTable GetButtonHlp(Int32 screenId, Int32 reportId, Int32 wizardId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetButtonHlp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = reportId;
			cmd.Parameters.Add("@WizardId", OdbcType.Numeric).Value = wizardId;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		public override DataTable GetClientRule(Int32 screenId, Int32 reportId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetClientRule", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = reportId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		public override DataTable GetScreenHlp(Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = null;
			if (string.IsNullOrEmpty(dbConnectionString))
			{
				cmd = new OdbcCommand("GetScreenHlp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
			}
			else
			{
				cmd = new OdbcCommand("GetScreenHlp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			}
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count == 1, "GetScreenHlp", "Screen Issue", "Default help message not available for Screen #'" + screenId.ToString() + "' and Culture #'" + cultureId.ToString() + "'!");
			return dt;
		}

		public override DataTable GetGlobalFilter(Int32 usrId, Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetGlobalFilter", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@usrId", OdbcType.Numeric).Value = usrId;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		public override DataTable GetScreenFilter(Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetScreenFilter", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

		public override DataTable GetScreenTab(Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetScreenTab", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetScreenTab", "Screen Issue", "Tab Folder Names not available for Screen #'" + screenId.ToString() + "' and Culture #'" + cultureId.ToString() + "'!");
			return dt;
		}

		public override DataTable GetScreenCriHlp(Int32 screenId, Int16 cultureId, string dbConnectionString, string dbPassword)
		{
			if ( da == null ) { throw new System.ObjectDisposedException( GetType().FullName ); }            
			OdbcCommand cmd = new OdbcCommand("GetScreenCriHlp",new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
			cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
            try { da.Fill(dt); }
			catch(Exception e) {ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString());}
			finally {cmd.Dispose(); cmd = null;}
//			ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetScreenCriHlp", "Screen Issue", "Criteria Column Headers not available for Screen #'" + screenId.ToString() + "' and Culture #'" + cultureId.ToString() + "'!");
			return dt;
		}

		public override void LogUsage(Int32 UsrId, string UsageNote, string EntityTitle, Int32 ScreenId, Int32 ReportId, Int32 WizardId, string Miscellaneous, string dbConnectionString, string dbPassword)
		{
			OdbcConnection cn =  new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
			cn.Open();
			OdbcCommand cmd = new OdbcCommand("LogUsage", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = UsrId;
			if (string.IsNullOrEmpty(UsageNote))
			{
				cmd.Parameters.Add("@UsageNote", OdbcType.VarChar).Value = System.DBNull.Value;
			}
			else
			{
				if (Config.DoubleByteDb) {cmd.Parameters.Add("@UsageNote", OdbcType.NVarChar).Value = UsageNote;} 
				else {cmd.Parameters.Add("@UsageNote", OdbcType.VarChar).Value = UsageNote;}
			}
			if (Config.DoubleByteDb) {cmd.Parameters.Add("@EntityTitle", OdbcType.NVarChar).Value = EntityTitle;} 
			else {cmd.Parameters.Add("@EntityTitle", OdbcType.VarChar).Value = EntityTitle;}
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
			cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = ReportId;
			cmd.Parameters.Add("@WizardId", OdbcType.Numeric).Value = WizardId;
			if (string.IsNullOrEmpty(Miscellaneous))
			{
				cmd.Parameters.Add("@Miscellaneous", OdbcType.VarChar).Value = System.DBNull.Value;
			}
			else
			{
				cmd.Parameters.Add("@Miscellaneous", OdbcType.VarChar).Value = Miscellaneous;
			}
			cmd.CommandTimeout = _CommandTimeout;
			try {TransformCmd(cmd).ExecuteNonQuery();}
			catch(Exception e) {ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString());}
			finally {cn.Close(); cmd.Dispose(); cmd = null;}
			return;
		}

		public override DataTable GetInfoByCol(Int32 ScreenId, string ColumnName, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetInfoByCol", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
			cmd.Parameters.Add("@ColumnName", OdbcType.VarChar).Value = ColumnName;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetInfoByCol", "Column not available", "Column '" + ColumnName + "' is not defined for Screen #'" + ScreenId.ToString() + "!");
			return dt;
		}

		public override bool IsValidOvride(Credential cr, Int32 usrId)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
			cn.Open();
			OdbcCommand cmd = new OdbcCommand("IsValidOvride", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@OvrideId", OdbcType.Numeric).Value = Int32.Parse(cr.LoginName);
			cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
			cmd.Parameters.Add("@UsrPassword", OdbcType.VarBinary).Value = cr.Password;
			int rtn = Convert.ToInt32(TransformCmd(cmd).ExecuteScalar());
			cmd.Dispose();
			cmd = null;
			cn.Close();
			if (rtn == 0) {return false;} else {return true;}
		}

		public override DataTable GetMsg(int MsgId, Int16 CultureId, string dbConnectionString, string dbPassword)
		{
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = null;
			if (string.IsNullOrEmpty(dbConnectionString))
			{
				cmd = new OdbcCommand("GetMsg", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
			}
			else
			{
				cmd = new OdbcCommand("GetMsg", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			}
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@MsgId", OdbcType.Numeric).Value = MsgId;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}
        public override DataTable GetCronJob(int? jobId, string jobLink, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = null;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand("GetCronJob", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand("GetCronJob", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            if (jobId == null)
                cmd.Parameters.Add("@jobId", OdbcType.VarChar).Value = DBNull.Value;
            else
                cmd.Parameters.Add("@jobId", OdbcType.VarChar).Value = jobId;
            if (string.IsNullOrEmpty(jobLink))
                cmd.Parameters.Add("@jobLink", OdbcType.VarChar).Value = DBNull.Value;
            else
                cmd.Parameters.Add("@jobLink", OdbcType.VarChar).Value = jobLink;

            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
        public override void UpdCronJob(int jobId, DateTime? lastRun, DateTime? nextRun, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("UpdCronJob", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            cmd.Parameters.Add("@jobId", OdbcType.Numeric).Value = jobId;
            cmd.Parameters.Add("@lastRun", GetOdbcType("datetime")).Value = lastRun.HasValue ? (object) ToOleDbDatetime(lastRun.Value) : DBNull.Value;
            cmd.Parameters.Add("@nextRun", GetOdbcType("datetime")).Value = nextRun.HasValue ? (object) ToOleDbDatetime(nextRun.Value) : DBNull.Value;
            cmd.Parameters[1].Size = 16;
            cmd.Parameters[1].Precision = 20;
            cmd.Parameters[1].Scale = 3;
            cmd.Parameters[2].Size = 16;
            cmd.Parameters[2].Precision = 20;
            cmd.Parameters[2].Scale = 3;
            try
            {
                da.UpdateCommand = cmd;
                TransformCmd(cmd).ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cn.Close();
            }
        }
        public override void UpdCronJobStatus(int jobId, string Error, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("UpdCronJobStatus", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            cmd.Parameters.Add("@jobId", OdbcType.Numeric).Value = jobId;
            cmd.Parameters.Add("@err", OdbcType.NVarChar).Value = Error.ToString();
            try
            {
                da.UpdateCommand = cmd;
                TransformCmd(cmd).ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cn.Close();
            }
        }

		// Obtain translated label one at a time from the table "Label" on system dependent database.
        public override string GetLabel(Int16 CultureId, string LabelCat, string LabelKey, string CompanyId, string dbConnectionString, string dbPassword)
		{
			string rtn = string.Empty;
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcConnection cn;
			if (string.IsNullOrEmpty(dbConnectionString))
			{
				cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
			}
			else
			{
				cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
			}
			cn.Open();
			OdbcCommand cmd = new OdbcCommand("GetLabel", cn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId.ToString();
            cmd.Parameters.Add("@LabelCat", OdbcType.VarChar).Value = LabelCat;
            cmd.Parameters.Add("@LabelKey", OdbcType.VarChar).Value = LabelKey;
			if (string.IsNullOrEmpty(CompanyId))
			{
				cmd.Parameters.Add("@CompanyId", OdbcType.Numeric).Value = System.DBNull.Value;
			}
			else
			{
				cmd.Parameters.Add("@CompanyId", OdbcType.Numeric).Value = CompanyId;
			}
			cmd.CommandTimeout = _CommandTimeout;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			try { da.Fill(dt); rtn = dt.Rows[0][0].ToString(); }
			catch { }
			finally { cn.Close(); cmd.Dispose(); cmd = null; }
			return rtn;
		}

		// Obtain translated labels as one category from the table "Label" on system dependent database.
		public override DataTable GetLabels(Int16 CultureId, string LabelCat, string CompanyId, string dbConnectionString, string dbPassword)
		{
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd;
			if (string.IsNullOrEmpty(dbConnectionString))
			{
				cmd = new OdbcCommand("GetLabels", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
			}
			else
			{
				cmd = new OdbcCommand("GetLabels", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			}
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId.ToString();
			cmd.Parameters.Add("@LabelCat", OdbcType.VarChar).Value = LabelCat;
			if (string.IsNullOrEmpty(CompanyId))
			{
				cmd.Parameters.Add("@CompanyId", OdbcType.Numeric).Value = System.DBNull.Value;
			}
			else
			{
				cmd.Parameters.Add("@CompanyId", OdbcType.Numeric).Value = CompanyId;
			}
			cmd.CommandTimeout = _CommandTimeout;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			return dt;
		}

        public override DataTable GetScrCriteria(string screenId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetScreenCriteria", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try { da.Fill(dt); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cmd.Dispose(); cmd = null; }
            //ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetScrCriteria", "Screen Criteria Issue", "Criteria for Screen #'" + screenId.ToString() + "' not available!");
            return dt;
        }

        public override void MkGetScreenIn(string screenId, string screenCriId, string procedureName, string appDatabase, string sysDatabase, string multiDesignDb, string dbConnectionString, string dbPassword, bool reGen)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("MkGetScreenIn", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@screenCriId", OdbcType.Numeric).Value = screenCriId;
            cmd.Parameters.Add("@procedureName", OdbcType.VarChar).Value = procedureName;
            cmd.Parameters.Add("@appDatabase", OdbcType.VarChar).Value = appDatabase;
            cmd.Parameters.Add("@sysDatabase", OdbcType.VarChar).Value = sysDatabase;
            cmd.Parameters.Add("@multiDesignDb", OdbcType.Char).Value = multiDesignDb;
            cmd.Parameters.Add("@reGen", OdbcType.Char).Value = reGen ? "Y" : "N";
            try
            {
                TransformCmd(cmd).ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString());
            }
            finally { cn.Close(); cmd.Dispose(); cmd = null; }

            return;
        }

        public override DataTable GetScreenIn(string screenId, string procedureName, int TotalCnt, string RequiredValid, int topN, string FilterTxt, bool bAll, string keyId, UsrImpr ui, UsrCurr uc, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            if (string.IsNullOrEmpty(FilterTxt))
            {
                cmd.Parameters.Add("@filterTxt", OdbcType.VarChar).Value = System.DBNull.Value;
            }
            else
            {
                if (Config.DoubleByteDb) { cmd.Parameters.Add("@filterTxt", OdbcType.NVarChar).Value = FilterTxt; } else { cmd.Parameters.Add("@filterTxt", OdbcType.VarChar).Value = FilterTxt; }
            }
            cmd.Parameters.Add("@topN", OdbcType.Numeric).Value = topN;
            cmd.Parameters.Add("@bAll", OdbcType.Char).Value = bAll ? "Y" : "N";
            cmd.Parameters.Add("@keyId", OdbcType.VarChar).Value = string.IsNullOrEmpty(keyId) ? System.DBNull.Value : (object)keyId;
            da.SelectCommand = TransformCmd(cmd);
            cmd.CommandTimeout = _CommandTimeout;
            DataTable dt = new DataTable();
            da.Fill(dt);
            //if (RequiredValid != "Y" && dt.Rows.Count >= TotalCnt) { dt.Rows.InsertAt(dt.NewRow(), 0); }
            if (RequiredValid != "Y") { dt.Rows.InsertAt(dt.NewRow(), 0); }
            return dt;
        }

        public override int CountScrCri(string ScreenCriId, string MultiDesignDb, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("CountScrCri", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenCriId", OdbcType.Numeric).Value = ScreenCriId;
            cmd.Parameters.Add("@MultiDesignDb", OdbcType.Char).Value = MultiDesignDb;
            int rtn = Convert.ToInt32(TransformCmd(cmd).ExecuteScalar());
            cmd.Dispose();
            cmd = null;
            cn.Close();
           return rtn;
        }

        public override void UpdScrCriteria(string screenId, string programName, DataView dvCri, Int32 usrId, bool isCriVisible, DataSet ds, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("Upd" + programName + "In", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@usrId", OdbcType.Numeric).Value = usrId;
            if (isCriVisible) { cmd.Parameters.Add("@isCriVisible", OdbcType.Char).Value = "Y"; } else { cmd.Parameters.Add("@isCriVisible", OdbcType.Char).Value = "N"; }
            if (dvCri != null && ds != null)
            {
                DataRow dr = ds.Tables["DtScreenIn"].Rows[0];
                foreach (DataRowView drv in dvCri)
                {
                    if (drv["RequiredValid"].ToString() == "N" && string.IsNullOrEmpty(dr[drv["ColumnName"].ToString()].ToString().Trim()))
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = System.DBNull.Value; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = System.DBNull.Value; }
                    }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "WChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "VarWChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NVarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = dr[drv["ColumnName"].ToString()]; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    }
                }
            }
            try
            {
                da.UpdateCommand = cmd;
                TransformCmd(cmd).ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cn.Close();
            }
            if (ds.HasErrors)
            {
                ds.Tables["DtScreenIn"].GetErrors()[0].ClearErrors();
            }
            else
            {
                ds.AcceptChanges();
            }
            return;
        }

		public override DataTable GetAuthRow(Int32 ScreenId, string RowAuthoritys, string dbConnectionString, string dbPassword)
		{
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("GetAuthRow", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
			cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = RowAuthoritys;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count == 1, "GetAuthRow", "Authorization Issue", "Authority levels have not been defined for Screen #'" + ScreenId.ToString() + "!");
            return dt;
		}

		public override DataTable GetAuthCol(Int32 ScreenId, UsrImpr ui, UsrCurr uc, string dbConnectionString, string dbPassword)
		{
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("GetAuthCol", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
			cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
			cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
			cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
			cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
			cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
			cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
			cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
			cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
			cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
			cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
			cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
			cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetAuthCol", "Authorization Issue", "Authority levels have not been defined for Screen #'" + ScreenId.ToString() + "!");
			return dt;
		}

		public override DataTable GetAuthExp(Int32 ScreenId, Int16 CultureId, UsrImpr ui, UsrCurr uc, string dbConnectionString, string dbPassword)
		{
			if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("GetAuthExp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId;
			cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
			cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
			cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
			cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
			cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
			cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
			cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
			cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
			cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
			cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
			cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
			cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetAuthExp", "Authorization Issue", "Authority levels have not been defined for Screen #'" + ScreenId.ToString() + "!");
			return dt;
		}

		public override DataTable GetScreenLabel(Int32 ScreenId, Int16 CultureId, UsrImpr ui, UsrCurr uc, string dbConnectionString, string dbPassword)
		{
            //if (!dbConnectionString.Contains("Design")) checkValidLicense();
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
			OdbcCommand cmd = new OdbcCommand("GetScreenLabel", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
			cmd.Parameters.Add("@CultureId", OdbcType.Numeric).Value = CultureId;
			cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
			cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
			cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
			cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
			cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
			cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
			cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
			cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
			cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
			cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
			cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
			cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetScreenLabel", "Screen Issue", "Screen Column Headers not available for Screen #'" + ScreenId.ToString() + "' and Culture #'" + CultureId.ToString() + "'!");
			return dt;
		}

        public override DataTable GetDdl(Int32 screenId, string procedureName, bool bAddNew, bool bAll, int topN, string keyId, string dbConnectionString, string dbPassword, string filterTxt, UsrImpr ui, UsrCurr uc)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            if (bAll) { cmd.Parameters.Add("@bAll", OdbcType.Char).Value = "Y"; } else { cmd.Parameters.Add("@bAll", OdbcType.Char).Value = "N"; }
            if (keyId == string.Empty)
            {
                cmd.Parameters.Add("@keyId", OdbcType.VarChar).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@keyId", OdbcType.VarChar).Value = keyId;
            }
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            if (filterTxt == string.Empty)
            {
                cmd.Parameters.Add("@filterTxt", OdbcType.VarChar).Value = System.DBNull.Value;
            }
            else
            {
                if (Config.DoubleByteDb) { cmd.Parameters.Add("@filterTxt", OdbcType.NVarChar).Value = filterTxt; } else { cmd.Parameters.Add("@filterTxt", OdbcType.VarChar).Value = filterTxt; }
            }
            cmd.Parameters.Add("@topN", OdbcType.Numeric).Value = topN;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (bAddNew) { dt.Rows.InsertAt(dt.NewRow(), 0); }
            return dt;
        }

        public override DataTable RunWrRule(int screenId, string procedureName, string dbConnectionString, string dbPassword, string parameterXML, UsrImpr ui, UsrCurr uc, bool noTrans)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }

            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            OdbcCommand cmd = new OdbcCommand(procedureName, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            if (parameterXML == string.Empty)
            {
                cmd.Parameters.Add("@parameterXML", OdbcType.VarChar).Value = System.DBNull.Value;
            }
            else
            {
                if (Config.DoubleByteDb) { cmd.Parameters.Add("@parameterXML", OdbcType.NVarChar).Value = parameterXML; } else { cmd.Parameters.Add("@parameterXML", OdbcType.VarChar).Value = parameterXML; }
            }
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);

            cn.Open();
            OdbcTransaction tr = noTrans ? null : cn.BeginTransaction();
            try
            {
                if (!noTrans) cmd.Transaction = tr;
                DataSet ds = new DataSet();
                //dt.Load(TransformCmd(cmd).ExecuteReader());
                da.Fill(ds);

                /*
                 * DO NOT USE DataAdapter Fill(DataTable) as error raised is not captured when the SP already return something(and the raiserror is done after that)
                 * , i.e. not behave as one expect, Fill(DataSet) which would correctly capture the error thus we can rollback
                da.Fill(dt); 
                */
                if (!noTrans) tr.Commit();
                if (ds.Tables.Count > 0) return ds.Tables[0]; else return new DataTable();
            }
            catch (Exception e)
            {
                if (!noTrans) tr.Rollback();
                throw new Exception(procedureName + ":" + e.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        public override DataTable GetExp(Int32 screenId, string procedureName, string useGlobalFilter, string dbConnectionString, string dbPassword, Int32 screenFilterId, DataView dvCri, UsrImpr ui, UsrCurr uc, DataSet ds)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@useGlobalFilter", OdbcType.VarChar).Value = useGlobalFilter;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@key", OdbcType.NVarChar).Value = System.DBNull.Value;
            cmd.Parameters.Add("@FilterTxt", OdbcType.NVarChar).Value = System.DBNull.Value;
            if (dvCri != null && ds != null)
            {
                DataRow dr = ds.Tables["DtScreenIn"].Rows[0];
                foreach (DataRowView drv in dvCri)
                {
                    if (drv["RequiredValid"].ToString() == "N" && string.IsNullOrEmpty(dr[drv["ColumnName"].ToString()].ToString().Trim()))
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = System.DBNull.Value; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = System.DBNull.Value; }
                    }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "WChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "VarWChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NVarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = dr[drv["ColumnName"].ToString()]; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    }
                }
            }
            cmd.Parameters.Add("@screenFilterId", OdbcType.Numeric).Value = screenFilterId;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public override DataTable GetLis(Int32 screenId, string procedureName, bool bAddNew, string useGlobalFilter, int topN, string dbConnectionString, string dbPassword, Int32 screenFilterId, string key, string filterTxt, DataView dvCri, UsrImpr ui, UsrCurr uc, DataSet ds)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@useGlobalFilter", OdbcType.VarChar).Value = useGlobalFilter;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            if (string.IsNullOrEmpty(key)) { cmd.Parameters.Add("@key", OdbcType.NVarChar).Value = System.DBNull.Value; } else { cmd.Parameters.Add("@key", OdbcType.NVarChar).Value = key; }
            if (string.IsNullOrEmpty(filterTxt)) { cmd.Parameters.Add("@filterTxt", OdbcType.NVarChar).Value = System.DBNull.Value; } else { cmd.Parameters.Add("@filterTxt", OdbcType.NVarChar).Value = filterTxt; }
            if (dvCri != null && ds != null)
            {
                DataRow dr = ds.Tables["DtScreenIn"].Rows[0];
                foreach (DataRowView drv in dvCri)
                {
                    if (drv["RequiredValid"].ToString() == "N" && string.IsNullOrEmpty(dr[drv["ColumnName"].ToString()].ToString().Trim()) || (drv["DisplayName"].ToString() == "Rating" && dr[drv["ColumnName"].ToString()].ToString() == "0"))
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = System.DBNull.Value; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = System.DBNull.Value; }
                    }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "WChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "VarWChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NVarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = dr[drv["ColumnName"].ToString()]; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    }
                }
            }
            cmd.Parameters.Add("@screenFilterId", OdbcType.Numeric).Value = screenFilterId;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            cmd.Parameters.Add("@topN", OdbcType.Numeric).Value = topN;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (bAddNew) { dt.Rows.InsertAt(dt.NewRow(), 0); }
            return dt;
        }

        public override DataTable GetMstById(string procedureName, string keyId1, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            if (string.IsNullOrEmpty(keyId1)) { cmd.Parameters.Add("@keyId1", OdbcType.NVarChar).Value = System.DBNull.Value; } else { cmd.Parameters.Add("@keyId1", OdbcType.NVarChar).Value = keyId1; }
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        /* Albeit rare, this overload shall take care of more than one column as primary key */
        public override DataTable GetMstById(string procedureName, string keyId1, string keyId2, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            if (string.IsNullOrEmpty(keyId1)) { cmd.Parameters.Add("@keyId1", OdbcType.NVarChar).Value = System.DBNull.Value; } else { cmd.Parameters.Add("@keyId1", OdbcType.NVarChar).Value = keyId1; }
            if (string.IsNullOrEmpty(keyId2)) { cmd.Parameters.Add("@keyId2", OdbcType.NVarChar).Value = System.DBNull.Value; } else { cmd.Parameters.Add("@keyId2", OdbcType.NVarChar).Value = keyId2; }
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public override DataTable GetDtlById(Int32 screenId, string procedureName, string keyId, string dbConnectionString, string dbPassword, Int32 screenFilterId, UsrImpr ui, UsrCurr uc)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@screenId", OdbcType.Numeric).Value = screenId;
            if (string.IsNullOrEmpty(keyId))
            {
                cmd.Parameters.Add("@keyId", OdbcType.NVarChar).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@keyId", OdbcType.NVarChar).Value = keyId;
            }
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@screenFilterId", OdbcType.Numeric).Value = screenFilterId;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        //private OleDbType GetOdbcType(string ColType)
        //{
        //    OleDbType otp;
        //    switch (ColType.ToLower())
        //    {
        //        case "numeric": case "tinyint": case "smallint": case "int": case "bigint":
        //            otp = OdbcType.Numeric; break;
        //        case "single": case "real":
        //            otp = OdbcType.Real; break;
        //        case "double": case "float":
        //            otp = OdbcType.Double; break;
        //        case "currency": case "money":
        //            otp = OdbcType.Numeric; break;
        //        case "binary": case "image":
        //            otp = OdbcType.Binary; break;
        //        case "varbinary":
        //            otp = OdbcType.VarBinary; break;
        //        case "wchar": case "nchar":
        //            otp = OdbcType.NChar; break;
        //        case "varwchar": case "nvarchar": case "ntext":
        //            otp = OdbcType.NVarChar; break;
        //        case "dbTimestamp": case "datetime": case "smalldatetime":
        //            otp = OdbcType.Timestamp; break;
        //        case "char":
        //            otp = OdbcType.Char; break;
        //        case "varchar": case "text":
        //            otp = OdbcType.VarChar; break;
        //        case "decimal":
        //            otp = OdbcType.Decimal; break;
        //        case "dbdate": case "date":
        //            otp = OdbcType.Date; break;
        //        default: throw new Exception("Non-anticipated OleDbType: " + ColType + ".  Please contact admnistrator ASAP.");
        //    }
        //    return otp;
        //}

        private OdbcType GetOdbcType(string ColType)
        {
            OdbcType otp;
            switch (ColType.ToLower())
            {
                case "numeric":
                case "tinyint":
                case "smallint":
                case "int":
                case "bigint":
                    otp = OdbcType.Numeric; break;
                case "single":
                case "real":
                    otp = OdbcType.Real; break;
                case "double":
                case "float":
                    otp = OdbcType.Double; break;
                case "currency":
                case "money":
                    otp = OdbcType.Numeric; break;
                case "binary":
                    otp = OdbcType.Binary; break;
                case "image":
                case "varbinary":
                    otp = OdbcType.VarBinary; break;
                case "wchar":
                case "nchar":
                    otp = OdbcType.VarChar; break;
                case "varwchar":
                case "nvarchar":
                case "ntext":
                    otp = OdbcType.NVarChar; break;
                case "dbtimestamp":
                case "datetime":
                case "smalldatetime":
                    otp = OdbcType.DateTime; break;
                case "char":
                    otp = OdbcType.Char; break;
                case "varchar":
                case "text":
                    otp = OdbcType.VarChar; break;
                case "decimal":
                    otp = OdbcType.Decimal; break;
                case "dbdate":
                case "date":
                    otp = OdbcType.Date; break;
                default: throw new Exception("Non-anticipated OdbcType: " + ColType + ".  Please contact admnistrator ASAP.");
            }
            return otp;
        }

        private object GetCallParam(string callp, LoginUsr LUser, UsrImpr LImpr, UsrCurr LCurr, DataRow row, DataRow dis, string paramType)
        {
            object rtn = string.Empty;
            switch (callp.ToLower())
            {
                case "null": rtn = null; break;
                case "luser.loginname": rtn = LUser.LoginName.ToString(); break;
                case "luser.usrid": rtn = LUser.UsrId.ToString(); break;
                case "luser.usrname": rtn = LUser.UsrName.ToString(); break;
                case "luser.usremail": rtn = LUser.UsrEmail.ToString(); break;
                case "luser.internalusr": rtn = LUser.InternalUsr.ToString(); break;
                case "luser.technicalusr": rtn = LUser.TechnicalUsr.ToString(); break;
                case "luser.cultureid": rtn = LUser.CultureId.ToString(); break;
                case "luser.culture": rtn = LUser.Culture.ToString(); break;
                case "luser.defsystemid": rtn = LUser.DefSystemId.ToString(); break;
                case "luser.defprojectid": rtn = LUser.DefProjectId.ToString(); break;
                case "luser.defcompanyid": rtn = LUser.DefCompanyId.ToString(); break;
                case "luser.pwdchgdt": rtn = LUser.PwdChgDt.ToString(); break;
                case "luser.pwdduration": rtn = LUser.PwdDuration.ToString(); break;
                case "luser.pwdwarn": rtn = LUser.PwdWarn.ToString(); break;

                case "lcurr.companyid": rtn = LCurr.CompanyId.ToString(); break;
                case "lcurr.projectid": rtn = LCurr.ProjectId.ToString(); break;
                case "lcurr.systemid": rtn = LCurr.SystemId.ToString(); break;
                case "lcurr.dbid": rtn = LCurr.DbId.ToString(); break;

                case "limpr.usrs": rtn = LImpr.Usrs.ToString(); break;
                case "limpr.usrgroups": rtn = LImpr.UsrGroups.ToString(); break;
                case "limpr.cultures": rtn = LImpr.Cultures.ToString(); break;
                case "limpr.rowauthoritys": rtn = LImpr.RowAuthoritys.ToString(); break;
                case "limpr.companys": rtn = LImpr.Companys.ToString(); break;
                case "limpr.projects": rtn = LImpr.Projects.ToString(); break;
                case "limpr.investors": rtn = LImpr.Investors.ToString(); break;
                case "limpr.customers": rtn = LImpr.Customers.ToString(); break;
                case "limpr.vendors": rtn = LImpr.Vendors.ToString(); break;
                case "limpr.agents": rtn = LImpr.Agents.ToString(); break;
                case "limpr.brokers": rtn = LImpr.Brokers.ToString(); break;
                case "limpr.members": rtn = LImpr.Members.ToString(); break;
                case "limpr.borrowers": rtn = LImpr.Borrowers.ToString(); break;
                case "limpr.guarantors": rtn = LImpr.Guarantors.ToString(); break;
                case "limpr.lenders": rtn = LImpr.Lenders.ToString(); break;

                case "config.architect": rtn = Config.Architect; break;
                case "config.cookiehttponly": rtn = Config.CookieHttpOnly; break;
                case "config.pwdexpdays": rtn = Config.PwdExpDays; break;
                case "config.adminloginonly": rtn = Config.AdminLoginOnly; break;
                case "config.wsdlexe": rtn = Config.WsdlExe; break;
                case "config.smtpserver": rtn = Config.SmtpServer; break;
                case "config.pmturl": rtn = Config.PmtUrl; break;
                case "config.ordurl": rtn = Config.OrdUrl; break;
                case "config.sslurl": rtn = Config.SslUrl; break;
                case "config.buildexe": rtn = Config.BuildExe; break;
                case "config.backuppath": rtn = Config.BackupPath; break;
                case "config.appnamespace": rtn = Config.AppNameSpace; break;
                case "config.deploytype": rtn = Config.DeployType; break;
                case "config.clienttierpath": rtn = Config.ClientTierPath; break;
                case "config.clanguagecd": rtn = Config.CLanguageCd; break;
                case "config.cframeworkcd": rtn = Config.CFrameworkCd; break;
                case "config.ruletierpath": rtn = Config.RuleTierPath; break;
                case "config.rlanguagecd": rtn = Config.RLanguageCd; break;
                case "config.rframeworkcd": rtn = Config.RFrameworkCd; break;
                case "config.dprovidercd": rtn = Config.DProviderCd; break;
                case "config.webtitle": rtn = Config.WebTitle; break;
                case "config.readonlybcolor": rtn = Config.ReadOnlyBColor; break;
                case "config.mandatorychar": rtn = Config.MandatoryChar; break;
                case "config.pathrtftemplate": rtn = Config.PathRtfTemplate; break;
                case "config.pathtxttemplate": rtn = Config.PathTxtTemplate; break;
                case "config.pathxlsimport": rtn = Config.PathXlsImport; break;
                case "config.pathtmpimport": rtn = Config.PathTmpImport; break;
                case "config.loginimage": rtn = Config.LoginImage; break;
                default:
                    rtn = dis == null || !dis.Table.Columns.Contains(callp) || string.IsNullOrEmpty(row[callp].ToString())
                            ? (object) row[callp].ToString()
                            : dis[callp].ToString().ToLower() == "password" 
                                ? (object) new Credential(string.Empty, row[callp].ToString().Trim()).Password
                                : paramType.ToLower() == "varbinary"
                                    ? (dis[callp].ToString().ToLower() == "imagebutton" ? (object)row[callp] : (object)Convert.FromBase64String((string)row[callp].ToString()))
                                    : (object)row[callp].ToString()
                                ;  
                    break;
            }
            return rtn;
        }

        private bool ExecSRule(string sRowFilter, DataView dvSRule, string firingEvent, string beforeCRUD, LoginUsr LUser, UsrImpr LImpr, UsrCurr LCurr, DataRow row, bool bDeferError, bool bHasErr, System.Collections.Generic.Dictionary<string, string> ErrLst, OdbcConnection cn, OdbcTransaction tr, ref string keyAdded, DataRow dis)
        {
            string callp = string.Empty;
            string param = string.Empty;
            string paramType = string.Empty;
            StringBuilder callingParams = new StringBuilder();
            StringBuilder parameterNames = new StringBuilder();
            StringBuilder parameterTypes = new StringBuilder();
            dvSRule.RowFilter = sRowFilter;
            foreach (DataRowView drv in dvSRule)
            {
                if (drv[firingEvent].ToString() == "Y" && drv["BeforeCRUD"].ToString() == beforeCRUD && (bDeferError || !bHasErr))
                {
                    try
                    {
                        OdbcCommand cmd = new OdbcCommand(drv["ProcedureName"].ToString(), cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        callingParams.Remove(0, callingParams.Length);
                        parameterNames.Remove(0, parameterNames.Length);
                        parameterTypes.Remove(0, parameterTypes.Length);
                        callingParams.Append(drv["CallingParams"].ToString());
                        parameterNames.Append(drv["ParameterNames"].ToString());
                        parameterTypes.Append(drv["ParameterTypes"].ToString());
                        while (parameterNames.Length > 0)
                        {
                            callp = Utils.PopFirstWord(callingParams, (char)44).Trim();
                            param = Utils.PopFirstWord(parameterNames, (char)44).Trim();
                            paramType = Utils.PopFirstWord(parameterTypes, (char)44).Trim();
                            object val = 
                                (callp ?? "").ToLower() == "Action.FiringEvent".ToLower() ? (object) firingEvent
                                : (callp ?? "").ToLower() == "Action.MasterTable".ToLower() ? (object)drv["MasterTable"].ToString()
                                : (callp ?? "").ToLower() == "Action.BeforeCRUD".ToLower() ? (object)beforeCRUD
                                : (callp ?? "").ToLower() == "Action.ServerRuleId".ToLower() ? (object)drv["ServerRuleId"].ToString()
                                : GetCallParam(callp, LUser, LImpr, LCurr, row, dis, paramType);
                            if (string.IsNullOrEmpty(callp) 
                                || val == null
                                || (val is string && string.IsNullOrEmpty(val as string)) 
                                || val as string == Convert.ToDateTime("0001.01.01").ToString())
                            {
                                cmd.Parameters.Add("@" + param, GetOdbcType(paramType)).Value = System.DBNull.Value;
                            }
                            else
                            {
                                cmd.Parameters.Add("@" + param, GetOdbcType(paramType)).Value = val;
                            }
                        }
                        cmd.Transaction = tr; cmd.CommandTimeout = _CommandTimeout;
                        if (beforeCRUD == "S" && firingEvent == "OnAdd")
                        {
                            da.SelectCommand = TransformCmd(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0].ToString())) { keyAdded = dt.Rows[0][0].ToString(); }
                        }
                        else
                        {
                            TransformCmd(cmd).ExecuteNonQuery();
                        }
                        cmd.Dispose();
                        cmd = null;
                    }
                    catch (Exception e) { bHasErr = true; string suffix = ErrLst.ContainsKey(drv["ProcedureName"].ToString()) ? "_" + Guid.NewGuid().ToString().Substring(0, 4) : ""; ErrLst[drv["ProcedureName"].ToString() + suffix] = e.Message; }
                }
            }
            return bHasErr;
        }

        private string AddDataDt(string pMKeyCol, string pMKeyOle, string pMKeyVal, DataRow row, DataRow typDt, DataRow disDt, DataColumnCollection cols, string sql, string pKeyCol, string pKeyOle, OdbcConnection cn, OdbcTransaction tr)
        {
            string rtn = string.Empty;
            OdbcCommand cmd;
            cmd = new OdbcCommand("SET NOCOUNT ON " + sql.Replace("RODesign.", Config.AppNameSpace + "Design."), cn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            if (!string.IsNullOrEmpty(pMKeyOle))    // I2
            {
                if (string.IsNullOrEmpty(pMKeyVal) || pMKeyVal == Convert.ToDateTime("0001.01.01").ToString())
                {
                    cmd.Parameters.Add("@" + pMKeyCol, GetOdbcType(pMKeyOle)).Value = System.DBNull.Value;
                }
                else
                {
                    // this bypass the hyperlink skip below if it is on primary key, sigh !
                    // IOW, we must allow hyperlink or other other 'non-edit' control passed in for the sake of primary key
                    // they will be ignore below but kept here !!!!
                    cmd.Parameters.Add("@" + pMKeyCol, GetOdbcType(pMKeyOle)).Value = pMKeyVal;
                }
            }
            if (string.IsNullOrEmpty(row[pKeyCol].ToString().Trim()) || row[pKeyCol].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString())
            {
                cmd.Parameters.Add("@" + pKeyCol, GetOdbcType(pKeyOle)).Value = System.DBNull.Value;
            }
            else
            {
                // this bypass the hyperlink skip below if it is on primary key, sigh !
                // IOW, we must allow hyperlink or other other 'non-edit' control passed in for the sake of primary key
                // they will be ignore below but kept here !!!!
                cmd.Parameters.Add("@" + pKeyCol, GetOdbcType(pKeyOle)).Value = row[pKeyCol].ToString().Trim();
            }
            foreach (DataColumn dc in cols)
            {
                if (dc.ColumnName != pKeyCol && dc.ColumnName != pMKeyCol)
                {
                    if ("hyperlink,imagelink,hyperpopup,imagepopup,datagridlink,label".IndexOf(disDt[dc.ColumnName].ToString().ToLower()) < 0 
                        && !string.IsNullOrEmpty(typDt[dc.ColumnName].ToString()) 
                        && !(disDt[dc.ColumnName].ToString().ToLower() == "imagebutton" 
                        && typDt[dc.ColumnName].ToString().ToLower() == "varbinary"))
                    {
                        if (string.IsNullOrEmpty(row[dc.ColumnName].ToString().Trim()) || row[dc.ColumnName].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString())
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = System.DBNull.Value;
                        }
                        //else if (disDt[dc.ColumnName].ToString().ToLower() == "currency")
                        //{
                        //    cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = Decimal.Parse(row[dc.ColumnName].ToString().Trim(), System.Globalization.NumberStyles.Currency);
                        //}
                        else if (disDt[dc.ColumnName].ToString().ToLower() == "password")
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = new Credential(string.Empty, row[dc.ColumnName].ToString().Trim()).Password;
                        }
                        else
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = row[dc.ColumnName].ToString();
                        }
                    }
                }
            }
			da.SelectCommand = TransformCmd(cmd);
			DataTable dt = new DataTable();
			da.Fill(dt);
			rtn = dt.Rows[0][0].ToString();
            cmd.Dispose();
            cmd = null;
            return rtn;
        }

        private void UpdDataDt(string pMKeyCol, DataRow row, DataRow typDt, DataRow disDt, DataColumnCollection cols, string sql, string pKeyCol, string pKeyOle, OdbcConnection cn, OdbcTransaction tr)
        {
            OdbcCommand cmd;
            cmd = new OdbcCommand("SET NOCOUNT ON " + sql.Replace("RODesign.", Config.AppNameSpace + "Design."), cn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            // this bypass the hyperlink skip below if it is on primary key, sigh !
            // IOW, we must allow hyperlink or other other 'non-edit' control passed in for the sake of primary key
            // they will be ignore below but kept here !!!!
            cmd.Parameters.Add("@" + pKeyCol, GetOdbcType(pKeyOle)).Value = row[pKeyCol].ToString().Trim();
            foreach (DataColumn dc in cols)
            {
                if (dc.ColumnName != pKeyCol 
                    && dc.ColumnName != pMKeyCol 
                    && "hyperlink,imagelink,hyperpopup,imagepopup,datagridlink,label".IndexOf(disDt[dc.ColumnName].ToString().ToLower()) < 0 
                    && !string.IsNullOrEmpty(typDt[dc.ColumnName].ToString()) 
                    && !(disDt[dc.ColumnName].ToString().ToLower() == "imagebutton" 
                    && typDt[dc.ColumnName].ToString().ToLower() == "varbinary"))
                {
                    if (string.IsNullOrEmpty(row[dc.ColumnName].ToString().Trim()) || row[dc.ColumnName].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString())
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = System.DBNull.Value;
                    }
                    //else if (disDt[dc.ColumnName].ToString().ToLower() == "currency")
                    //{
                    //    cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = Decimal.Parse(row[dc.ColumnName].ToString().Trim(), System.Globalization.NumberStyles.Currency);
                    //}
                    else if (disDt[dc.ColumnName].ToString().ToLower() == "password")
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = new Credential(string.Empty, row[dc.ColumnName].ToString().Trim()).Password;
                    }
                    else
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typDt[dc.ColumnName].ToString())).Value = row[dc.ColumnName].ToString();
                    }
                }
            }
            TransformCmd(cmd).ExecuteNonQuery();
            cmd.Dispose();
            cmd = null;
            return;
        }

        private void DelDataDt(LoginUsr LUser, DataRow row, DataRow typDt, DataRow disDt, DataColumnCollection cols, string sql, string pKeyCol, string pKeyOle, OdbcConnection cn, OdbcTransaction tr)
        {
            OdbcCommand cmd;
            cmd = new OdbcCommand("SET NOCOUNT ON " + sql.Replace("RODesign.", Config.AppNameSpace + "Design."), cn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            cmd.Parameters.Add("@" + pKeyCol, GetOdbcType(pKeyOle)).Value = row[pKeyCol].ToString().Trim();
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = LUser.UsrId.ToString();
            TransformCmd(cmd).ExecuteNonQuery();
            cmd.Dispose();
            cmd = null;
            return;
        }

        // Only I1 or I2 would call this:
        public override string AddData(Int32 ScreenId, bool bDeferError, LoginUsr LUser, UsrImpr LImpr, UsrCurr LCurr, DataSet ds, string dbConnectionString, string dbPassword, CurrPrj CPrj, CurrSrc CSrc, bool noTrans = false)
        {
            bool bHasErr = false;
            string sAddDataDt = string.Empty;
            System.Collections.Generic.Dictionary<string, string> ErrLst = new System.Collections.Generic.Dictionary<string, string>();
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            DataTable dtScr = null;
            DataTable dtAud = null;
            DataView dvSRule = null;
            DataView dvCol = null;
            using (GenScreensAccess dac = new GenScreensAccess())
            {
                dtScr = dac.GetScreenById(ScreenId, CPrj, CSrc);
                dtAud = dac.GetScreenAud(ScreenId, dtScr.Rows[0]["ScreenTypeName"].ToString(), CPrj.SrcDesDatabase, dtScr.Rows[0]["MultiDesignDb"].ToString(), CSrc);
                dvSRule = new DataView(dac.GetServerRule(ScreenId, CPrj, CSrc, LImpr, LCurr));
                dvCol = new DataView(dac.GetScreenColumns(ScreenId, CPrj, CSrc));
            }
            string appDbName = dtScr.Rows[0]["dbAppDatabase"].ToString();
            string screenName = dtScr.Rows[0]["ProgramName"].ToString();
            bool licensedScreen = IsLicensedFeature(appDbName, screenName);
            if (!licensedScreen)
            {
                throw new Exception("please acquire proper license to unlock this feature");
            }
            string pMKeyCol = string.Empty; string pDKeyCol = string.Empty;
            string pMKeyOle = string.Empty; string pDKeyOle = string.Empty;
            dvCol.RowFilter = "PrimaryKey = 'Y'";
            foreach (DataRowView drv in dvCol)
            {
                if (drv["MasterTable"].ToString() == "Y")
                {
                    pMKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                    pMKeyOle = drv["DataTypeDByteOle"].ToString();
                }
                if (drv["MasterTable"].ToString() != "Y")
                {
                    pDKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                    pDKeyOle = drv["DataTypeDByteOle"].ToString();
                }
            }
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcTransaction tr = noTrans ? null : cn.BeginTransaction();
            DataRow row = ds.Tables[0].Rows[0];
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON " + dtAud.Rows[1][0].ToString().Replace("RODesign.", Config.AppNameSpace + "Design."), cn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = _CommandTimeout;
            if (!noTrans) cmd.Transaction = tr;
            DataRow typ = ds.Tables[0].Rows[1]; DataRow dis = ds.Tables[0].Rows[2];
            if (string.IsNullOrEmpty(row[pMKeyCol].ToString().Trim()) || row[pMKeyCol].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString())
            {
                cmd.Parameters.Add("@" + pMKeyCol, GetOdbcType(pMKeyOle)).Value = System.DBNull.Value;
            }
            else
            {
                // this bypass the hyperlink skip below if it is on primary key, sigh !
                // IOW, we must allow hyperlink or other other 'non-edit' control passed in for the sake of primary key
                // they will be ignore below but kept here !!!!
                cmd.Parameters.Add("@" + pMKeyCol, GetOdbcType(pMKeyOle)).Value = row[pMKeyCol].ToString().Trim(); ;
            }
            foreach (DataColumn dc in ds.Tables[0].Columns)
            {
                if (dc.ColumnName != pMKeyCol 
                    && "hyperlink,imagelink,hyperpopup,imagepopup,datagridlink,imagebutton,label".IndexOf(dis[dc.ColumnName].ToString().ToLower()) < 0 
                    && !string.IsNullOrEmpty(typ[dc.ColumnName].ToString()))
                {
                    if (string.IsNullOrEmpty(row[dc.ColumnName].ToString().Trim()) || row[dc.ColumnName].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString())
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = System.DBNull.Value;
                    }
                    //else if (dis[dc.ColumnName].ToString().ToLower() == "currency")
                    //{
                    //    cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = Decimal.Parse(row[dc.ColumnName].ToString().Trim(), System.Globalization.NumberStyles.Currency);
                    //}
                    else if (dis[dc.ColumnName].ToString().ToLower() == "password")
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = new Credential(string.Empty, row[dc.ColumnName].ToString().Trim()).Password;
                    }
                    else if (typ[dc.ColumnName].ToString().ToLower() == "varbinary")    // Not for password
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = Convert.FromBase64String((string)row[dc.ColumnName]);
                    }
                    else
                    {
                        cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = row[dc.ColumnName].ToString();
                    }
                }
            }
            try
            {
                /* create the temp table that can be used between SPs */
                OdbcCommand tempTableCmd = new OdbcCommand("SET NOCOUNT ON CREATE TABLE #CRUDTemp (rid int identity, KeyVal varchar(max), ColumnName varchar(50), Val nvarchar(max), mode char(1), MasterTable char(1))", cn, tr);
                tempTableCmd.ExecuteNonQuery(); tempTableCmd.Dispose();
                /* Before CRUD rules */
                bool SkipAdd = false;
                bool SkipGridAdd = false;
                string pKeyAdded = null, dKeyAdded = null, _dummy = null;
                foreach (DataRowView drv in dvSRule)
                {
                    SkipAdd = SkipAdd || (drv["MasterTable"].ToString() == "Y" && drv["OnAdd"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                    SkipGridAdd = SkipGridAdd || (drv["MasterTable"].ToString() == "N" && drv["OnAdd"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                }
                pKeyAdded = null;
                bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnAdd", "Y", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref pKeyAdded, dis);
                /* Skip(i.e. Replace) CRUD */
                bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnAdd", "S", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref pKeyAdded, dis);
                if (SkipAdd && !string.IsNullOrEmpty(pKeyAdded)) { row[pMKeyCol] = pKeyAdded; }
                if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                {
                    for (int jj = 0; jj < ds.Tables[2].Rows.Count; jj++)
                    {
                        if (SkipAdd && !string.IsNullOrEmpty(pKeyAdded) && SkipGridAdd) ds.Tables[2].Rows[jj][pMKeyCol] = pKeyAdded;
                        dKeyAdded = null;
                        bHasErr = ExecSRule("MasterTable <> 'Y'", dvSRule, "OnAdd", "Y", LUser, LImpr, LCurr, ds.Tables[2].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref dKeyAdded, ds.Tables[1].Rows[1]);
                        /* Skip(i.e. Replace) CRUD */
                        bHasErr = ExecSRule("MasterTable <> 'Y'", dvSRule, "OnAdd", "S", LUser, LImpr, LCurr, ds.Tables[2].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref dKeyAdded, ds.Tables[1].Rows[1]);
                        if (!string.IsNullOrEmpty(dKeyAdded)) { ds.Tables[2].Rows[jj][pDKeyCol] = dKeyAdded; };
                    }
                }
                if ((bDeferError || !bHasErr) && !SkipAdd)
                {
                    da.SelectCommand = TransformCmd(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0].ToString())) { row[pMKeyCol] = dt.Rows[0][0].ToString(); }
                }
                if (!bHasErr)
                {
                    if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                    {
                        DataRow typDt = ds.Tables[1].Rows[0]; DataRow disDt = ds.Tables[1].Rows[1]; DataColumnCollection cols = ds.Tables[1].Columns;
                        for (int jj = 0; jj < ds.Tables[2].Rows.Count && !SkipGridAdd; jj++)
                        {
                            sAddDataDt = AddDataDt(pMKeyCol, pMKeyOle, row[pMKeyCol].ToString().Trim(), ds.Tables[2].Rows[jj], typDt, disDt, cols, dtAud.Rows[4][0].ToString(), pDKeyCol, pDKeyOle, cn, tr);
                            if (!string.IsNullOrEmpty(sAddDataDt)) { ds.Tables[2].Rows[jj][pDKeyCol] = sAddDataDt; };
                        }
                    }
                    /* After CRUD rules */
                    bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnAdd", "N", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                    if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                    {
                        DataRow typDt = ds.Tables[1].Rows[0]; DataRow disDt = ds.Tables[1].Rows[1]; DataColumnCollection cols = ds.Tables[1].Columns;
                        for (int jj = 0; jj < ds.Tables[2].Rows.Count; jj++)
                        {
                            bHasErr = ExecSRule("MasterTable <> 'Y'", dvSRule, "OnAdd", "N", LUser, LImpr, LCurr, ds.Tables[2].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, disDt);
                        }
                    }
                    /* before commit */
                    bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnAdd", "C", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                }
                /* Only if both master and detail adds succeed */
                if (!bHasErr) { if (!noTrans) tr.Commit(); }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string err in ErrLst.Keys) { sb.Append(Environment.NewLine + err + "|" + ErrLst[err]); }
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                if (!noTrans) tr.Rollback();
                ApplicationAssert.CheckCondition(false, "AddData", "", e.Message);
            }
            finally { cn.Close(); }
            if (ds.HasErrors)
            {
                ds.Tables[0].GetErrors()[0].ClearErrors();
                if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                {
                    ds.Tables[2].GetErrors()[0].ClearErrors();
                }
            }
            else
            {
                ds.AcceptChanges();
            }
            return row[pMKeyCol].ToString();
        }

        public override bool UpdData(Int32 ScreenId, bool bDeferError, LoginUsr LUser, UsrImpr LImpr, UsrCurr LCurr, DataSet ds, string dbConnectionString, string dbPassword, CurrPrj CPrj, CurrSrc CSrc, bool noTrans = false)
        {
            bool bHasErr = false;
            string sAddDataDt = string.Empty;
            string sRowFilter = string.Empty;
            System.Collections.Generic.Dictionary<string, string> ErrLst = new System.Collections.Generic.Dictionary<string, string>();
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            DataTable dtScr = null;
            DataTable dtAud = null;
            DataView dvSRule = null;
            DataView dvCol = null;
            using (GenScreensAccess dac = new GenScreensAccess())
            {
                dtScr = dac.GetScreenById(ScreenId, CPrj, CSrc);
                dtAud = dac.GetScreenAud(ScreenId, dtScr.Rows[0]["ScreenTypeName"].ToString(), CPrj.SrcDesDatabase, dtScr.Rows[0]["MultiDesignDb"].ToString(), CSrc);
                dvSRule = new DataView(dac.GetServerRule(ScreenId, CPrj, CSrc, LImpr, LCurr));
                dvCol = new DataView(dac.GetScreenColumns(ScreenId, CPrj, CSrc));
            }
            string appDbName = dtScr.Rows[0]["dbAppDatabase"].ToString();
            string screenName = dtScr.Rows[0]["ProgramName"].ToString();
            bool licensedScreen = IsLicensedFeature(appDbName, screenName);
            if (!licensedScreen)
            {
                throw new Exception("please acquire proper license to unlock this feature");
            }

            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcTransaction tr = noTrans ? null : cn.BeginTransaction();
            int ii = 0;
            DataRow row = ds.Tables[0].Rows[0];
            if ("I1,I2".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
            {
                OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON " + dtAud.Rows[2][0].ToString().Replace("RODesign.", Config.AppNameSpace + "Design."), cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = _CommandTimeout;
                da.UpdateCommand = cmd;
                if (!noTrans) da.UpdateCommand.Transaction = tr;
                string pMKeyCol = string.Empty;
                string pMKeyOle = string.Empty;
                string pMKeyVal = string.Empty;
                dvCol.RowFilter = "PrimaryKey = 'Y'";
                DataRow typ = ds.Tables[0].Rows[1]; DataRow dis = ds.Tables[0].Rows[2];
                foreach (DataRowView drv in dvCol)
                {
                    if (drv["MasterTable"].ToString() == "Y")
                    {
                        pMKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                        pMKeyOle = drv["DataTypeDByteOle"].ToString();
                    }
                }
                dvCol.RowFilter = "";
                // this bypass the hyperlink skip below if it is on primary key, sigh !
                // IOW, we must allow hyperlink or other other 'non-edit' control passed in for the sake of primary key
                // they will be ignore below but kept here !!!!
                pMKeyVal = row[pMKeyCol].ToString().Trim();
                cmd.Parameters.Add("@" + pMKeyCol, GetOdbcType(pMKeyOle)).Value = row[pMKeyCol].ToString().Trim();
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (dc.ColumnName != pMKeyCol && "hyperlink,imagelink,hyperpopup,imagepopup,datagridlink,imagebutton,label".IndexOf(dis[dc.ColumnName].ToString().ToLower()) < 0 && !string.IsNullOrEmpty(typ[dc.ColumnName].ToString()))
                    {
                        if (string.IsNullOrEmpty(row[dc.ColumnName].ToString().Trim()) || row[dc.ColumnName].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString())
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = System.DBNull.Value;
                        }
                        //else if (dis[dc.ColumnName].ToString().ToLower() == "currency")
                        //{
                        //    cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = Decimal.Parse(row[dc.ColumnName].ToString().Trim(), System.Globalization.NumberStyles.Currency);
                        //}
                        else if (dis[dc.ColumnName].ToString().ToLower() == "password")
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = new Credential(string.Empty, row[dc.ColumnName].ToString().Trim()).Password;
                        }
                        else if (typ[dc.ColumnName].ToString().ToLower() == "varbinary")    // Not for password
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = Convert.FromBase64String((string)row[dc.ColumnName]);
                        }
                        else
                        {
                            cmd.Parameters.Add("@" + dc.ColumnName, GetOdbcType(typ[dc.ColumnName].ToString())).Value = row[dc.ColumnName].ToString();
                        }
                    }
                }
            }
            try
            {
                /* create the temp table that can be used between SPs */
                OdbcCommand tempTableCmd = new OdbcCommand("SET NOCOUNT ON CREATE TABLE #CRUDTemp (rid int identity, KeyVal varchar(max), ColumnName varchar(50), Val nvarchar(max), mode char(1), MasterTable char(1))", cn, tr);
                tempTableCmd.ExecuteNonQuery(); tempTableCmd.Dispose();

                /* Before CRUD rules */
                bool SkipUpd = false;
                bool SkipGridAdd = false;
                bool SkipGridUpd = false;
                bool SkipGridDel = false;
                string _dummy = null;

                if ("I1,I2".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                {
                    foreach (DataRowView drv in dvSRule)
                    {
                        SkipUpd = SkipUpd || (drv["MasterTable"].ToString() == "Y" && drv["OnUpd"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                    }
                    bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnUpd", "Y", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, ds.Tables[0].Rows[2]);
                    /* Skip(i.e. Replace) CRUD */
                    bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnUpd", "S", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, ds.Tables[0].Rows[2]);
                }
                if ("I2,I3".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                {
                    foreach (DataRowView drv in dvSRule)
                    {
                        SkipGridAdd = SkipGridAdd || (drv["MasterTable"].ToString() == (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I3" ? "Y" : "N") && drv["OnAdd"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                        SkipGridUpd = SkipGridUpd || (drv["MasterTable"].ToString() == (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I3" ? "Y" : "N") && drv["OnUpd"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                        SkipGridDel = SkipGridDel || (drv["MasterTable"].ToString() == (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I3" ? "Y" : "N") && drv["OnDel"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                    }

                    string pMKeyCol = string.Empty; string pDKeyCol = string.Empty;
                    string pMKeyOle = string.Empty; string pDKeyOle = string.Empty;
                    dvCol.RowFilter = "PrimaryKey = 'Y'";
                    DataRow dis = null;
                    foreach (DataRowView drv in dvCol)
                    {
                        if (drv["MasterTable"].ToString() == "Y")
                        {
                            pMKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                            pMKeyOle = drv["DataTypeDByteOle"].ToString();
                        }
                        if (drv["MasterTable"].ToString() != "Y")
                        {
                            pDKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                            pDKeyOle = drv["DataTypeDByteOle"].ToString();
                        }
                    }

                    if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I3") 
                    { 
                        ii = 0; sRowFilter = "MasterTable = 'Y'"; dis = ds.Tables[0].Rows[1];
                    } 
                    else 
                    { 
                        ii = 1; sRowFilter = "MasterTable <> 'Y'"; dis = ds.Tables[1].Rows[1];
                    }
                    ii = ii + 1;
                    for (int jj = 0; jj < ds.Tables[ii].Rows.Count; jj++)
                    {
                        string KeyAdded = null;
                        if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2" && SkipGridAdd) ds.Tables[ii].Rows[jj][pMKeyCol] = row[pMKeyCol].ToString().Trim();
                        bHasErr = ExecSRule(sRowFilter, dvSRule, "OnAdd", "Y", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref KeyAdded, dis);
                        /* Skip(i.e. Replace) CRUD */
                        bHasErr = ExecSRule(sRowFilter, dvSRule, "OnAdd", "S", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref KeyAdded, dis);
                        if (SkipGridAdd && !string.IsNullOrEmpty(KeyAdded)) { ds.Tables[ii].Rows[jj][dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2" ? pDKeyCol : pMKeyCol] = KeyAdded; };
                    }
                    ii = ii + 1;
                    for (int jj = 0; jj < ds.Tables[ii].Rows.Count; jj++)
                    {
                        bHasErr = ExecSRule(sRowFilter, dvSRule, "OnUpd", "Y", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                        /* Skip(i.e. Replace) CRUD */
                        bHasErr = ExecSRule(sRowFilter, dvSRule, "OnUpd", "S", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                    }
                    ii = ii + 1;
                    for (int jj = 0; jj < ds.Tables[ii].Rows.Count; jj++)
                    {
                        bHasErr = ExecSRule(sRowFilter, dvSRule, "OnDel", "Y", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                        /* Skip(i.e. Replace) CRUD */
                        bHasErr = ExecSRule(sRowFilter, dvSRule, "OnDel", "S", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                    }
                }

                if ("I1,I2".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0 && !SkipUpd)
                {
                    if (bDeferError || !bHasErr) { da.UpdateCommand.ExecuteNonQuery(); }
                }
                if (bDeferError || !bHasErr)
                {
                    if ("I2,I3".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                    {
                        string pMKeyCol = string.Empty; string pDKeyCol = string.Empty;
                        string pMKeyOle = string.Empty; string pDKeyOle = string.Empty;
                        dvCol.RowFilter = "PrimaryKey = 'Y'";
                        foreach (DataRowView drv in dvCol)
                        {
                            if (drv["MasterTable"].ToString() == "Y")
                            {
                                pMKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                                pMKeyOle = drv["DataTypeDByteOle"].ToString();
                            }
                            if (drv["MasterTable"].ToString() != "Y")
                            {
                                pDKeyCol = drv["ColumnName"].ToString() + drv["TableId"].ToString();
                                pDKeyOle = drv["DataTypeDByteOle"].ToString();
                            }
                        }
                        if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I3") 
                        { 
                            ii = 0; 
                        } 
                        else 
                        { 
                            ii = 1; 
                        }
                        DataRow typDt = ds.Tables[ii].Rows[0]; DataRow disDt = ds.Tables[ii].Rows[1]; DataColumnCollection cols = ds.Tables[ii].Columns;
                        ii = ii + 1;

                        for (int jj = 0; jj < ds.Tables[ii].Rows.Count && !SkipGridAdd; jj++)
                        {
                            // this is technically not allowed to be skipped as subsequent server rules depend on the created key and there is 
                            // no easy way to return that via ExecSRule(assuming it was added there instead)

                            if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                            {
                                sAddDataDt = AddDataDt(pMKeyCol, pMKeyOle, row[pMKeyCol].ToString().Trim(), ds.Tables[ii].Rows[jj], typDt, disDt, cols, dtAud.Rows[4][0].ToString(), pDKeyCol, pDKeyOle, cn, tr);
                                if (!string.IsNullOrEmpty(sAddDataDt)) { ds.Tables[ii].Rows[jj][pDKeyCol] = sAddDataDt; };
                            }
                            else
                            {
                                sAddDataDt = AddDataDt(pMKeyCol, string.Empty, string.Empty, ds.Tables[ii].Rows[jj], typDt, disDt, cols, dtAud.Rows[1][0].ToString(), pMKeyCol, pMKeyOle, cn, tr);
                                if (!string.IsNullOrEmpty(sAddDataDt)) { ds.Tables[ii].Rows[jj][pMKeyCol] = sAddDataDt; };
                            }
                        }
                        ii = ii + 1;
                        for (int jj = 0; jj < ds.Tables[ii].Rows.Count && !SkipGridUpd; jj++)
                        {
                            if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                            {
                                UpdDataDt(pMKeyCol, ds.Tables[ii].Rows[jj], typDt, disDt, cols, dtAud.Rows[5][0].ToString(), pDKeyCol, pDKeyOle, cn, tr);
                            }
                            else
                            {
                                UpdDataDt(pMKeyCol, ds.Tables[ii].Rows[jj], typDt, disDt, cols, dtAud.Rows[2][0].ToString(), pMKeyCol, pMKeyOle, cn, tr);
                            }
                        }
                        ii = ii + 1;
                        for (int jj = 0; jj < ds.Tables[ii].Rows.Count && !SkipGridDel; jj++)
                        {
                            if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                            {
                                DelDataDt(LUser, ds.Tables[ii].Rows[jj], typDt, disDt, cols, dtAud.Rows[3][0].ToString(), pDKeyCol, pDKeyOle, cn, tr);
                            }
                            else
                            {
                                DelDataDt(LUser, ds.Tables[ii].Rows[jj], typDt, disDt, cols, dtAud.Rows[0][0].ToString(), pMKeyCol, pMKeyOle, cn, tr);
                            }
                        }
                    }
                    /* After CRUD rules */
                    DataRow GridRow = null; string GridRowType = null; DataRow dis = null;
                    if ("I1,I2".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                    {
                        bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnUpd", "N", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, ds.Tables[0].Rows[2]);
                    }
                    if ("I2,I3".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                    {
                        if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I3") 
                        { 
                            ii = 0; sRowFilter = "MasterTable = 'Y'"; dis = ds.Tables[ii].Rows[1];
                        } 
                        else 
                        { 
                            ii = 1; sRowFilter = "MasterTable <> 'Y'"; dis = ds.Tables[ii].Rows[1];
                        }
                        ii = ii + 1;
                        for (int jj = 0; jj < ds.Tables[ii].Rows.Count; jj++)
                        {
                            GridRow = ds.Tables[ii].Rows[jj]; GridRowType = "OnAdd";
                            bHasErr = ExecSRule(sRowFilter, dvSRule, "OnAdd", "N", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                        }
                        ii = ii + 1;
                        for (int jj = 0; jj < ds.Tables[ii].Rows.Count; jj++)
                        {
                            GridRow = ds.Tables[ii].Rows[jj]; ; GridRowType = "OnUpd";
                            bHasErr = ExecSRule(sRowFilter, dvSRule, "OnUpd", "N", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                        }
                        ii = ii + 1;
                        for (int jj = 0; jj < ds.Tables[ii].Rows.Count; jj++)
                        {
                            GridRow = ds.Tables[ii].Rows[jj]; ; GridRowType = "OnDel";
                            bHasErr = ExecSRule(sRowFilter, dvSRule, "OnDel", "N", LUser, LImpr, LCurr, ds.Tables[ii].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, dis);
                        }
                    }
                    /* before commit */
                    if ("I1,I2".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                    {
                        bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnUpd", "C", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, ds.Tables[0].Rows[2]);
                    }
                    else if (GridRow != null) // I3 with at least one row changed
                    {
                        // would only run ONCE using the last row info (delete or update or add, in that order)
                        bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, GridRowType, "C", LUser, LImpr, LCurr, GridRow, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, ds.Tables[0].Rows[1]);
                    }
                }
                /* Only if both master and detail succeed */
                if (!bHasErr) { if (!noTrans) tr.Commit(); }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string err in ErrLst.Keys) { sb.Append(Environment.NewLine + err + "|" + ErrLst[err]); }
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                if (!noTrans) tr.Rollback();
                ApplicationAssert.CheckCondition(false, "UpdData", "", e.Message);
            }
            finally { cn.Close(); }
            if (ds.HasErrors)
            {
                ii = 0;
                if ("I1,I2".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                {
                    ds.Tables[ii].GetErrors()[0].ClearErrors(); ii = ii + 1;
                }
                if ("I2,I3".IndexOf(dtScr.Rows[0]["ScreenTypeName"].ToString()) >= 0)
                {
                    ii = ii + 1; ds.Tables[ii].GetErrors()[0].ClearErrors();
                    ii = ii + 1; ds.Tables[ii].GetErrors()[0].ClearErrors();
                    ii = ii + 1; ds.Tables[ii].GetErrors()[0].ClearErrors();
                }
                return false;
            }
            else
            {
                ds.AcceptChanges(); return true;
            }
        }

        // Only I1 or I2 would call this.
        public override bool DelData(Int32 ScreenId, bool bDeferError, LoginUsr LUser, UsrImpr LImpr, UsrCurr LCurr, DataSet ds, string dbConnectionString, string dbPassword, CurrPrj CPrj, CurrSrc CSrc, bool noTrans = false)
        {
            bool bHasErr = false;
            System.Collections.Generic.Dictionary<string, string> ErrLst = new System.Collections.Generic.Dictionary<string, string>();
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            DataTable dtScr = null;
            DataTable dtAud = null;
            DataView dvSRule = null;
            DataView dvCol = null;
            using (GenScreensAccess dac = new GenScreensAccess())
            {
                dtScr = dac.GetScreenById(ScreenId, CPrj, CSrc);
                dtAud = dac.GetScreenAud(ScreenId, dtScr.Rows[0]["ScreenTypeName"].ToString(), CPrj.SrcDesDatabase, dtScr.Rows[0]["MultiDesignDb"].ToString(), CSrc);
                dvSRule = new DataView(dac.GetServerRule(ScreenId, CPrj, CSrc, LImpr, LCurr));
                dvCol = new DataView(dac.GetScreenColumns(ScreenId, CPrj, CSrc));
            }
            string appDbName = dtScr.Rows[0]["dbAppDatabase"].ToString();
            string screenName = dtScr.Rows[0]["ProgramName"].ToString();
            bool licensedScreen = IsLicensedFeature(appDbName, screenName);
            if (!licensedScreen)
            {
                throw new Exception("please acquire proper license to unlock this feature");
            }

            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcTransaction tr = noTrans ? null : cn.BeginTransaction();
            DataRow row = ds.Tables[0].Rows[0];
            // SQL in dtAud delete the detail rows for I2 as well:
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON " + dtAud.Rows[0][0].ToString().Replace("RODesign.", Config.AppNameSpace + "Design."), cn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = _CommandTimeout;
            if (!noTrans) cmd.Transaction = tr;
            dvCol.RowFilter = "PrimaryKey = 'Y'";
            foreach (DataRowView drv in dvCol)
            {
                if (drv["MasterTable"].ToString() == "Y")
                {
                     cmd.Parameters.Add("@" + drv["ColumnName"].ToString() + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = row[drv["ColumnName"].ToString() + drv["TableId"].ToString()].ToString().Trim();
                     cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = LUser.UsrId.ToString();
                }
            }
            try
            {
                /* create the temp table that can be used between SPs */
                OdbcCommand tempTableCmd = new OdbcCommand("SET NOCOUNT ON CREATE TABLE #CRUDTemp (rid int identity, KeyVal varchar(max), ColumnName varchar(50), Val nvarchar(max), mode char(1), MasterTable char(1))", cn, tr);
                tempTableCmd.ExecuteNonQuery(); tempTableCmd.Dispose();

                bool SkipDel = false;
                string _dummy = null;
                foreach (DataRowView drv in dvSRule)
                {
                    SkipDel = SkipDel || (drv["MasterTable"].ToString() == "Y" && drv["OnDel"].ToString() == "Y" && drv["BeforeCRUD"].ToString() == "S");
                }

                /* Before CRUD rules */
                bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnDel", "Y", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                /* Skip(i.e. Replace) CRUD */
                bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnDel", "S", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                {
                    for (int jj = 0; jj < ds.Tables[4].Rows.Count; jj++)
                    {
                        bHasErr = ExecSRule("MasterTable <> 'Y'", dvSRule, "OnDel", "Y", LUser, LImpr, LCurr, ds.Tables[4].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                        /* Skip(i.e. Replace) CRUD */
                        bHasErr = ExecSRule("MasterTable <> 'Y'", dvSRule, "OnDel", "S", LUser, LImpr, LCurr, ds.Tables[4].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                    }
                }
                if ((bDeferError || !bHasErr) && !SkipDel) { TransformCmd(cmd).ExecuteNonQuery(); }
                bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnDel", "N", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                {
                    for (int jj = 0; jj < ds.Tables[4].Rows.Count; jj++)
                    {
                        bHasErr = ExecSRule("MasterTable <> 'Y'", dvSRule, "OnDel", "N", LUser, LImpr, LCurr, ds.Tables[4].Rows[jj], bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                    }
                }
                /* before commit */
                bHasErr = ExecSRule("MasterTable = 'Y'", dvSRule, "OnDel", "C", LUser, LImpr, LCurr, row, bDeferError, bHasErr, ErrLst, cn, tr, ref _dummy, null);
                if (!bHasErr) { if (!noTrans) tr.Commit(); }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string err in ErrLst.Keys) { sb.Append(Environment.NewLine + err + "|" + ErrLst[err]); }
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                if (!noTrans) tr.Rollback();
                ApplicationAssert.CheckCondition(false, "DelData", "", e.Message);
            }
            finally { cn.Close(); }
            if (ds.HasErrors)
            {
                ds.Tables[0].GetErrors()[0].ClearErrors();
                if (dtScr.Rows[0]["ScreenTypeName"].ToString() == "I2")
                {
                    ds.Tables[4].GetErrors()[0].ClearErrors();
                }
                return false;
            }
            else
            {
                ds.AcceptChanges(); return true;
            }
        }

        public override string DelDoc(string MasterId, string DocId, string UsrId, string DdlKeyTableName, string TableName, string ColumnName, string pMKey, string dbConnectionString, string dbPassword)
        {
            string rtn = string.Empty;
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON"
            + " DECLARE @MasterId numeric(10,0), @DocId numeric(10,0), @UsrId numeric(10,0) SELECT @MasterId=?, @DocId=?, @UsrId=?"
            + " IF EXISTS (SELECT 1 FROM dbo." + DdlKeyTableName + " WHERE DocId = @DocId AND InputBy = @UsrId)"
            + " BEGIN"
                + " DELETE FROM dbo." + DdlKeyTableName + " WHERE DocId = @DocId"
                + " SELECT @DocId = MAX(DocId) FROM dbo." + DdlKeyTableName + " WHERE MasterId = @MasterId"
                + " UPDATE dbo." + TableName + " SET " + ColumnName + " = @DocId WHERE " + pMKey + " = @MasterId"
            + " END"
            + " SELECT @DocId", cn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            cmd.Parameters.Add("@MasterId", OdbcType.Numeric).Value = MasterId;
            cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = DocId;
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = UsrId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0][0].ToString())) { rtn = dt.Rows[0][0].ToString(); }
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback(); ApplicationAssert.CheckCondition(false, string.Empty, string.Empty, e.Message);
            }
            finally { cn.Close(); }
            return rtn;
        }

        // For reports:

        public override DataTable GetIn(Int32 reportId, string procedureName, int TotalCnt, string RequiredValid, bool bAll, string keyId, UsrImpr ui, UsrCurr uc, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@reportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
            cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
            cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            cmd.Parameters.Add("@bAll", OdbcType.Char).Value = bAll ? "Y" : "N";
            cmd.Parameters.Add("@keyId", OdbcType.VarChar).Value = string.IsNullOrEmpty(keyId) ? System.DBNull.Value : (object)keyId;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (RequiredValid != "Y" && dt.Rows.Count >= TotalCnt) { dt.Rows.InsertAt(dt.NewRow(), 0); }
            return dt;
        }

        // To be deleted: for backward compatibility only.
        public override DataTable GetIn(Int32 reportId, string procedureName, bool bAddNew, UsrImpr ui, UsrCurr uc, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand(procedureName, new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@reportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
            cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
            cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
            cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
            cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
            cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
            cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
            cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
            cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
            cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
            cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
            cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (bAddNew) { dt.Rows.InsertAt(dt.NewRow(), 0); }
            return dt;
        }

        public override DataTable GetRptDt(Int32 reportId, string procedureName, UsrImpr ui, UsrCurr uc, DataSet ds, DataView dvCri, string dbConnectionString, string dbPassword, bool bUpd, bool bXls, bool bVal)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
            }
            else
            {
                cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            }

            try
            {
                DataRow dr = ds != null ? ds.Tables[0].Rows[0] : null;
                cn.Open();
                /* create the temp table that can be used between SPs */
                OdbcCommand setupCmd = new OdbcCommand("SET NOCOUNT ON CREATE TABLE #ReportTemp (Name varchar(100), Val nvarchar(max))", cn);
                setupCmd.ExecuteNonQuery();

                if (dr != null && ds.Tables[0].Columns.Contains("tzInfo"))
                {
                    setupCmd.CommandText = string.Format("INSERT INTO #ReportTemp VALUES ('{0}','{1}')", "TZInfo", dr["tzInfo"].ToString().Replace("'","''"));
                    setupCmd.ExecuteNonQuery();
                }
                setupCmd.Dispose();
                
                OdbcCommand cmd = new OdbcCommand(procedureName, cn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@reportId", OdbcType.Numeric).Value = reportId;
                cmd.Parameters.Add("@RowAuthoritys", OdbcType.VarChar).Value = ui.RowAuthoritys;
                cmd.Parameters.Add("@Usrs", OdbcType.VarChar).Value = ui.Usrs;
                cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = ui.UsrGroups;
                cmd.Parameters.Add("@Cultures", OdbcType.VarChar).Value = ui.Cultures;
                cmd.Parameters.Add("@Companys", OdbcType.VarChar).Value = ui.Companys;
                cmd.Parameters.Add("@Projects", OdbcType.VarChar).Value = ui.Projects;
                cmd.Parameters.Add("@Agents", OdbcType.VarChar).Value = ui.Agents;
                cmd.Parameters.Add("@Brokers", OdbcType.VarChar).Value = ui.Brokers;
                cmd.Parameters.Add("@Customers", OdbcType.VarChar).Value = ui.Customers;
                cmd.Parameters.Add("@Investors", OdbcType.VarChar).Value = ui.Investors;
                cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = ui.Members;
                cmd.Parameters.Add("@Vendors", OdbcType.VarChar).Value = ui.Vendors;
                cmd.Parameters.Add("@Borrowers", OdbcType.VarChar).Value = ui.Borrowers;
                cmd.Parameters.Add("@Guarantors", OdbcType.VarChar).Value = ui.Guarantors;
                cmd.Parameters.Add("@Lenders", OdbcType.VarChar).Value = ui.Lenders;
                cmd.Parameters.Add("@currCompanyId", OdbcType.Numeric).Value = uc.CompanyId;
                cmd.Parameters.Add("@currProjectId", OdbcType.Numeric).Value = uc.ProjectId;
                if (dvCri != null && ds != null)
                {
                    foreach (DataRowView drv in dvCri)
                    {
                        if (drv["RequiredValid"].ToString() == "N" && string.IsNullOrEmpty(dr[drv["ColumnName"].ToString()].ToString().Trim()))
                        {
                            if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = System.DBNull.Value; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = System.DBNull.Value; }
                            else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = System.DBNull.Value; }
                        }
                        else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "WChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NChar).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "VarWChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NVarChar).Value = dr[drv["ColumnName"].ToString()]; }
                        else
                        {
                            if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = dr[drv["ColumnName"].ToString()]; }
                            else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = dr[drv["ColumnName"].ToString()]; }
                            else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = dr[drv["ColumnName"].ToString()]; }
                        }
                    }
                }
                if (bUpd) { cmd.Parameters.Add("@bUpd", OdbcType.Char).Value = "Y"; } else { cmd.Parameters.Add("@bUpd", OdbcType.Char).Value = "N"; }
                if (bXls) { cmd.Parameters.Add("@bXls", OdbcType.Char).Value = "Y"; } else { cmd.Parameters.Add("@bXls", OdbcType.Char).Value = "N"; }
                if (bVal) { cmd.Parameters.Add("@bVal", OdbcType.Char).Value = "Y"; } else { cmd.Parameters.Add("@bVal", OdbcType.Char).Value = "N"; }
                da.SelectCommand = TransformCmd(cmd);
                cmd.CommandTimeout = _CommandTimeout;
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            finally
            {
                cn.Close();
            }
        }

        public override bool UpdRptDt(Int32 reportId, string procedureName, Int32 usrId, DataSet ds, DataView dvCri, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand(procedureName, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = _CommandTimeout;
            da.UpdateCommand = cmd;
            da.UpdateCommand.Transaction = tr;
            cmd.Parameters.Add("@reportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@usrId", OdbcType.Numeric).Value = usrId;
            if (dvCri != null && ds != null)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                foreach (DataRowView drv in dvCri)
                {
                    if (drv["RequiredValid"].ToString() == "N" && string.IsNullOrEmpty(dr[drv["ColumnName"].ToString()].ToString().Trim()))
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = System.DBNull.Value; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = System.DBNull.Value; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = System.DBNull.Value; }
                    }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "WChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else if (Config.DoubleByteDb && drv["DataTypeDByteOle"].ToString() == "VarWChar") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.NVarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    else
                    {
                        if (drv["DataTypeSByteOle"].ToString() == "Numeric") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Single") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Real).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Double") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Double).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Currency") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Numeric).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Binary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Binary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "VarBinary") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarBinary).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBTimestamp") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.DateTime).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Decimal") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Decimal).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "DBDate") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Date).Value = dr[drv["ColumnName"].ToString()]; }
                        else if (drv["DataTypeSByteOle"].ToString() == "Char") { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.Char).Value = dr[drv["ColumnName"].ToString()]; }
                        else { cmd.Parameters.Add("@" + drv["ColumnName"].ToString(), OdbcType.VarChar).Value = dr[drv["ColumnName"].ToString()]; }
                    }
                }
            }
            try
            {
                da.UpdateCommand = TransformCmd(da.UpdateCommand);
                da.UpdateCommand.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback(); ApplicationAssert.CheckCondition(false, "UpdRptDt", "", e.Message);
            }
            finally { cn.Close(); }
            if (ds.HasErrors)
            {
                ds.Tables[0].GetErrors()[0].ClearErrors(); return false;
            }
            else
            {
                ds.AcceptChanges(); return true;
            }
        }

        public override DataTable GetPrinterList(string UsrGroups, string Members)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetPrinterList", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@UsrGroups", OdbcType.VarChar).Value = UsrGroups;
            cmd.Parameters.Add("@Members", OdbcType.VarChar).Value = Members;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count <= 0)
            {
                dt.Rows.InsertAt(dt.NewRow(), 0);
                dt.Rows[0]["PrinterPath"] = "*";
                dt.Rows[0]["PrinterName"] = "<Printer Unavailable>";
            }
            return dt;
        }

        // For legacy batch reporting only:
        public override void UpdLastCriteria(Int32 screenId, Int32 reportId, Int32 usrId, Int32 criId, string lastCriteria, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("UpdLastCriteria", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = screenId;
            cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
            cmd.Parameters.Add("@CriId", OdbcType.Numeric).Value = criId;
            if (string.IsNullOrEmpty(lastCriteria))
            {
                cmd.Parameters.Add("@LastCriteria", OdbcType.VarChar).Value = System.DBNull.Value;
            }
            else
            {
                if (Config.DoubleByteDb) { cmd.Parameters.Add("@LastCriteria", OdbcType.NVarChar).Value = lastCriteria; } else { cmd.Parameters.Add("@LastCriteria", OdbcType.VarChar).Value = lastCriteria; }
            }
            cmd.CommandTimeout = _CommandTimeout;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cn.Close(); cmd.Dispose(); cmd = null; }
            return;
        }

        public override DataTable GetReportHlp(Int32 reportId, Int16 cultureId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = null;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cmd = new OdbcCommand("GetReportHlp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            }
            else
            {
                cmd = new OdbcCommand("GetReportHlp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            }
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@reportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ApplicationAssert.CheckCondition(dt.Rows.Count == 1, "GetReportHlp", "Report Issue", "Default help message not available for Report #'" + reportId.ToString() + "' and Culture #'" + cultureId.ToString() + "'!");
            return dt;
        }

        public override DataTable GetReportCriHlp(Int32 reportId, Int16 cultureId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetReportCriHlp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@reportId", OdbcType.Numeric).Value = reportId;
            cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            ApplicationAssert.CheckCondition(dt.Rows.Count > 0, "GetReportCriHlp", "Report Issue", "Report Criteria Column Headers not available for Report #'" + reportId.ToString() + "' and Culture #'" + cultureId.ToString() + "'!");
            return dt;
        }

        public override DataTable GetReportSct()
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetReportSct", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            cmd.CommandType = CommandType.StoredProcedure;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public override DataTable GetReportItem(Int32 ReportId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetReportItem", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = ReportId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        // Need to do this because SQLRS Report Viewer is not accessible here:
        public override string GetRptPwd(string pwd)
        {
            return DecryptString(pwd);
        }

        // For Wizards:

        public override string GetSchemaWizImp(Int32 wizardId, Int16 cultureId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetSchemaWizImp", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@wizardId", OdbcType.Numeric).Value = wizardId;
            cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows) { sb.Append(dr[0].ToString()); }
            return sb.ToString();
        }

        public override string GetWizImpTmpl(Int32 wizardId, Int16 cultureId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcCommand cmd = new OdbcCommand("GetWizImpTmpl", new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))));
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@wizardId", OdbcType.Numeric).Value = wizardId;
            cmd.Parameters.Add("@cultureId", OdbcType.Numeric).Value = cultureId;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            StringBuilder sb = new StringBuilder();
            foreach (DataRow dr in dt.Rows) { sb.Append(dr[0].ToString()); }
            return sb.ToString();
        }

        private void ImportRow(DataView dvCol, string procedureName, string ImportFileName, DataRow row, OdbcConnection cn, OdbcTransaction tr)
        {
            OdbcCommand cmd = new OdbcCommand(procedureName, cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ImportFileName", OdbcType.VarChar).Value = ImportFileName;
            int ii = 0;
            foreach (DataRowView drv in dvCol)
            {
                try
                {
                    if (ii >= row.ItemArray.Length // not enough column data
                        || string.IsNullOrEmpty(row[ii].ToString().Trim())
                        || row[ii].ToString().Trim() == Convert.ToDateTime("0001.01.01").ToString()
                        || row[ii].ToString().Trim() == Convert.ToDateTime("1899.12.30").ToString()
                        || row[ii].ToString().Trim() == "#N/A" // excel formula lookup result, should be another form of null
                        )
                    {
                        cmd.Parameters.Add("@" + Robot.SmallCapToStart(drv["ColumnName"].ToString()) + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = System.DBNull.Value;
                    }
                    else if (
                        (",numeric,decimal,currency,double,".IndexOf(drv["DataTypeSByteOle"].ToString().ToLower()) >= 0
                            &&
                            (row[ii].ToString().Trim().EndsWith("-") && row[ii].ToString().Length <= 2)
                            ) // "-" or "$-" is a form of 0 in excel for certain numeric formatting
                        )
                    {
                        cmd.Parameters.Add("@" + Robot.SmallCapToStart(drv["ColumnName"].ToString()) + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = 0;
                    }
                    else if (",numeric,decimal,double,".IndexOf(drv["DataTypeSByteOle"].ToString().ToLower()) >= 0 && row[ii].ToString().Trim().EndsWith("%"))
                    {
                        cmd.Parameters.Add("@" + Robot.SmallCapToStart(drv["ColumnName"].ToString()) + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = Decimal.Parse(row[ii].ToString().Trim().Left(row[ii].ToString().Trim().Length - 1));
                    }
                    else if (drv["DataTypeSByteOle"].ToString().ToLower() == "currency")
                    {
                        cmd.Parameters.Add("@" + Robot.SmallCapToStart(drv["ColumnName"].ToString()) + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = Decimal.Parse(row[ii].ToString().Trim(), System.Globalization.NumberStyles.Currency);
                    }
                    else if (drv["DataTypeSysName"].ToString().Contains("Date") && !string.IsNullOrEmpty(row[ii].ToString().Trim()))
                    {
                        cmd.Parameters.Add("@" + Robot.SmallCapToStart(drv["ColumnName"].ToString()) + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = DateTime.Parse(row[ii].ToString().Trim(), System.Threading.Thread.CurrentThread.CurrentCulture);
                    }
                    else
                    {
                        cmd.Parameters.Add("@" + Robot.SmallCapToStart(drv["ColumnName"].ToString()) + drv["TableId"].ToString(), GetOdbcType(drv["DataTypeDByteOle"].ToString())).Value = row[ii].ToString().Trim();
                    }
                    ii = ii + 1;
                }
                catch (Exception er) { throw new Exception("Col " + Utils.Num2ExcelCol(ii + 1) + " " + er.Message, er); };
            }
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            TransformCmd(cmd).ExecuteNonQuery();
            cmd.Dispose();
            cmd = null;
            return;
        }

        public override int ImportRows(Int32 wizardId, string procedureName, bool bOverwrite, Int32 usrId, DataSet ds, int iStart, string fileName, string dbConnectionString, string dbPassword, CurrPrj CPrj, CurrSrc CSrc, bool noTrans)
        {
            bool bDeferError = true;       // This can be false if error is to be trapped one at a time.
            bool bHasErr = false;
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            System.Collections.Generic.Dictionary<string, string> ErrLst = new System.Collections.Generic.Dictionary<string, string>();
            DataView dvRul = null;
            DataView dvCol = null;
            using (GenWizardsAccess dac = new GenWizardsAccess())
            {
                dvRul = new DataView(dac.GetWizardRule(wizardId, CPrj, CSrc));
                dvCol = new DataView(dac.GetWizardColumns(wizardId, CPrj, CSrc));
            }
            int ii = 1;
            DataRowCollection rows = ds.Tables[0].Rows;
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString))
            {
                cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
            }
            else
            {
                cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            }
            cn.Open();
            OdbcTransaction tr = noTrans ? null : cn.BeginTransaction();
            try
            {
                bHasErr = ExecWRule(dvRul, "Y", bOverwrite, usrId, fileName, bDeferError, bHasErr, ErrLst, cn, tr);
                for (ii = iStart; ii < rows.Count; ii++)
                {
                    if (rows[ii].RowState != System.Data.DataRowState.Deleted)
                    {
                        try { ImportRow(dvCol, procedureName, fileName, rows[ii], cn, tr); }
                        catch (Exception e)
                        {
                            ApplicationAssert.CheckCondition(false, "ImportRows", "", "Row " + (ii + 1).ToString() + ", " + e.Message);
                        }
                    }
                }
                bHasErr = ExecWRule(dvRul, "N", bOverwrite, usrId, fileName, bDeferError, bHasErr, ErrLst, cn, tr);
                if (!bHasErr) { if (!noTrans) tr.Commit(); }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string err in ErrLst.Keys) { sb.Append(Environment.NewLine + err + "|" + ErrLst[err]); }
                    throw new Exception(sb.ToString());
                }
            }
            catch (Exception e)
            {
                if (!noTrans) tr.Rollback();
                ApplicationAssert.CheckCondition(false, "ImportRows", "", e.Message);
            }
            finally { cn.Close(); }
            if (ds.HasErrors)
            {
                ds.Tables[0].GetErrors()[0].ClearErrors(); return 0;
            }
            else
            {
                ds.AcceptChanges(); return (rows.Count - iStart);
            }
        }

        private bool ExecWRule(DataView dvRul, string beforeCRUD, bool bOverwrite, Int32 usrId, string ImportFileName, bool bDeferError, bool bHasErr, System.Collections.Generic.Dictionary<string, string> ErrLst, OdbcConnection cn, OdbcTransaction tr)
        {
            foreach (DataRowView drv in dvRul)
            {
                if (drv["BeforeCRUD"].ToString() == beforeCRUD && (bDeferError || !bHasErr))
                {
                    try
                    {
                        OdbcCommand cmd = new OdbcCommand(drv["ProcedureName"].ToString(), cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (bOverwrite) { cmd.Parameters.Add("@Overwrite", OdbcType.Char).Value = "Y"; } else { cmd.Parameters.Add("@Overwrite", OdbcType.Char).Value = "N"; }
                        cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = usrId;
                        cmd.Parameters.Add("@ImportFileName", OdbcType.VarChar).Value = ImportFileName;
                        cmd.Transaction = tr; cmd.CommandTimeout = _CommandTimeout; TransformCmd(cmd).ExecuteNonQuery(); cmd.Dispose(); cmd = null;
                    }
                    catch (Exception e) { bHasErr = true; ErrLst[drv["ProcedureName"].ToString()] = e.Message; }
                }
            }
            return bHasErr;
        }

        // For general:

        public override bool IsRegenNeeded(string ProgramName, Int32 ScreenId, Int32 ReportId, Int32 WizardId, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn;
            if (string.IsNullOrEmpty(dbConnectionString)) { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())); } else { cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword))); }
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("IsRegenNeeded", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (string.IsNullOrEmpty(ProgramName))
            {
                cmd.Parameters.Add("@ProgramName", OdbcType.VarChar).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@ProgramName", OdbcType.VarChar).Value = ProgramName;
            }
            cmd.Parameters.Add("@ScreenId", OdbcType.Numeric).Value = ScreenId;
            cmd.Parameters.Add("@ReportId", OdbcType.Numeric).Value = ReportId;
            cmd.Parameters.Add("@WizardId", OdbcType.Numeric).Value = WizardId;
            int rtn = Convert.ToInt32(TransformCmd(cmd).ExecuteScalar());
            cmd.Dispose();
            cmd = null;
            cn.Close();
            if (rtn == 0) { return false; } else { return true; }
        }

        public override string AddDbDoc(string MasterId, string TblName, string DocName, string MimeType, long DocSize, byte[] dc, string dbConnectionString, string dbPassword, LoginUsr lu)
        {
            string rtn = string.Empty;
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON"
                + " DECLARE @DocId numeric(10,0)"
                + " INSERT " + TblName + " (MasterId, DocName, MimeType, DocSize, DocImage, InputBy, InputOn, Active)"
                + " SELECT ?, ?, ?, ?, ?, ?, GETUTCDATE(), 'Y'"
                + " SELECT @DocId = @@IDENTITY"
                + " SELECT @DocId", cn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MasterId", OdbcType.Numeric).Value = MasterId;
            cmd.Parameters.Add("@DocName", OdbcType.NVarChar).Value = DocName;
            cmd.Parameters.Add("@MimeType", OdbcType.VarChar).Value = MimeType;
            cmd.Parameters.Add("@DocSize", OdbcType.Numeric).Value = DocSize;
            cmd.Parameters.Add("@DocImage", OdbcType.VarBinary).Value = dc;
            cmd.Parameters.Add("@InputBy", OdbcType.Numeric).Value = lu.UsrId;
            cmd.CommandTimeout = _CommandTimeout;
            cmd.Transaction = tr;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                rtn = dt.Rows[0][0].ToString();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                cn.Close();
            }
            return rtn;
        }

        // Get the most recent to replace as long as it has the same file name and owned by this user:
        public override string GetDocId(string MasterId, string TblName, string DocName, string UsrId, string dbConnectionString, string dbPassword)
        {
            string rtn = string.Empty;
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON"
                + " SELECT DocId FROM " + TblName + " WHERE InputOn = (SELECT MAX(InputOn) FROM " + TblName + " WHERE MasterId = ? AND DocName = ? AND InputBy = ?)", cn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@MasterId", OdbcType.Numeric).Value = MasterId;
            cmd.Parameters.Add("@DocName", OdbcType.NVarChar).Value = DocName;
            cmd.Parameters.Add("@UsrId", OdbcType.Numeric).Value = UsrId;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0) { rtn = dt.Rows[0][0].ToString(); }
            }
            catch (Exception e)
            {
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                cn.Close();
            }
            return rtn;
        }

        public override void UpdDbDoc(string DocId, string TblName, string DocName, string MimeType, long DocSize, byte[] dc, string dbConnectionString, string dbPassword, LoginUsr lu)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON"
                + " UPDATE " + TblName + " SET DocName = ?, MimeType = ?, DocSize = ?, DocImage = ?, InputBy = ?, InputOn = GETUTCDATE(), Active = 'Y'"
                + " WHERE DocId = ?", cn);
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@DocName", OdbcType.NVarChar).Value = DocName;
            cmd.Parameters.Add("@MimeType", OdbcType.VarChar).Value = MimeType;
            cmd.Parameters.Add("@DocSize", OdbcType.Numeric).Value = DocSize;
            cmd.Parameters.Add("@DocImage", OdbcType.VarBinary).Value = dc;
            cmd.Parameters.Add("@InputBy", OdbcType.Numeric).Value = lu.UsrId;
            cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = DocId;
            cmd.CommandTimeout = _CommandTimeout;
            da.UpdateCommand = cmd;
            da.UpdateCommand.Transaction = tr;
            try
            {
                da.UpdateCommand.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                cn.Close();
            }
        }

        public override void UpdDbImg(string DocId, string TblName, string KeyName, string ColName, byte[] dc, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcTransaction tr = cn.BeginTransaction();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON UPDATE " + TblName + " SET " + ColName + " = ? WHERE " + KeyName + " = ?", cn);
            cmd.CommandType = CommandType.Text;
            if (dc == null)
            {
                cmd.Parameters.Add("@DocImage", OdbcType.VarBinary).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DocImage", OdbcType.VarBinary).Value = dc;
            }
            cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = DocId;
            cmd.CommandTimeout = _CommandTimeout;
            da.UpdateCommand = cmd;
            da.UpdateCommand.Transaction = tr;
            try
            {
                da.UpdateCommand = TransformCmd(da.UpdateCommand);
                da.UpdateCommand.ExecuteNonQuery();
                tr.Commit();
            }
            catch (Exception e)
            {
                tr.Rollback();
                ApplicationAssert.CheckCondition(false, "", "", e.Message);
            }
            finally
            {
                cmd.Dispose();
                cmd = null;
                cn.Close();
            }
        }

        public override bool IsMDesignDb(string TblName)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("IsMDesignDb", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@TblName", OdbcType.VarChar).Value = TblName;
            int rtn = Convert.ToInt32(TransformCmd(cmd).ExecuteScalar());
            cmd.Dispose();
            cmd = null;
            cn.Close();
            if (rtn == 0) { return false; }
            else { return true; }
        }

        public override DataTable GetDbDoc(string DocId, string TblName, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON"
                + " SELECT DocName, MimeType, DocImage FROM " + TblName + " WHERE DocId = ?", cn);
            cmd.CommandType = CommandType.Text;
            if (string.IsNullOrWhiteSpace(DocId))
            {
                cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = DocId;
            }
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try { da.Fill(dt); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message); }
            finally { cmd.Dispose(); cmd = null; cn.Close(); }
            return dt;
        }

        public override DataTable GetDbImg(string DocId, string TblName, string KeyName, string ColName, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON SELECT " + ColName + " FROM " + TblName + " WHERE " + KeyName + " = ?", cn);
            cmd.CommandType = CommandType.Text;
            if (string.IsNullOrWhiteSpace(DocId))
            {
                cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = System.DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@DocId", OdbcType.Numeric).Value = DocId;
            }
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try { da.Fill(dt); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message); }
            finally { cmd.Dispose(); cmd = null; cn.Close(); }
            return dt;
        }

        public override string GetDesignVersion(string ns, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            ns = ns.Trim();
            try
            {
                cn.Open();
                OdbcCommand cmd = new OdbcCommand("SET NOCOUNT ON"
                     + " SELECT TOP 1 1 a.AppInfoDesc FROM " + ns + "Design.dbo.AppInfo a"
                     + " INNER JOIN " + ns + "Design.dbo.AppItem ai on a.AppInfoId = ai.AppInfoId and a.VersionDt is not null"
                     + " ORDER BY a.Version Dt DESC"
                     , cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = _CommandTimeout;
                da.SelectCommand = TransformCmd(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0) return dt.Rows[0][0].ToString();
                else return "";
            }
            finally
            {
                cn.Close();
            }
        }

        public override List<string> HasOutstandReleaseContent(string ns, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
            ns = ns.Trim();
            List<string> outstandingReleaseContentSys = new List<string>();
            try
            {
                cn.Open();
                OdbcCommand cmd = new OdbcCommand(
                    "SET NOCOUNT ON "
                    + "SELECT m.* FROM dbo.Systems s "
                    + "INNER JOIN dbo.Systems m on m.dbDesDatabase LIKE REPLACE(s.dbDesDatabase,'Design','') + '%'  "
                    + "WHERE s.SysProgram = 'Y' AND s.Active = 'Y' "
                    + "AND m.dbAppUserId = s.dbAppUserId AND m.dbAppServer = s.dbAppServer "
                    + "AND (m.SysProgram = 'N' OR '" + ns + "' = 'RO' ) "
                    // + "AND m.SysProgram = 'N' "
                    + "AND EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = s.dbDesDatabase) "
                    + "AND EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = m.dbAppDatabase) ", cn);
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = _CommandTimeout;
                da.SelectCommand = TransformCmd(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    string dbName = dr["dbDesDatabase"].ToString();
                    DataTable dtX = new DataTable();
                    cmd.CommandText = "SET NOCOUNT ON"
                        + " SELECT TOP 1 1 "
                        + " FROM " + dbName + ".dbo.AppInfo a "
                        + " INNER JOIN " + dbName + ".dbo.AppItem ai on a.AppInfoId = ai.AppInfoId and a.VersionDt is null"
                        + "";
                    da.SelectCommand = TransformCmd(cmd);
                    da.Fill(dtX);
                    if (dtX.Rows.Count > 0)
                    {
                        outstandingReleaseContentSys.Add(dr["SystemName"].ToString());
                    }
                }
            }
            finally { cn.Close(); }
            return outstandingReleaseContentSys;
        }

        public override Dictionary<string,List<string>> HasOutstandRegen(string ns, string dbConnectionString, string dbPassword)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr())));
            ns = ns.Trim();
            Dictionary<string, List<string>> regenList = new Dictionary<string, List<string>>();
            try
            {
                cn.Open();
                OdbcCommand cmd = new OdbcCommand(
                    "SET NOCOUNT ON "
                    + "SELECT m.* FROM dbo.Systems s "
                    + "INNER JOIN Systems m on m.dbDesDatabase LIKE REPLACE(s.dbDesDatabase,'Design','') + '%'  "
                    + "WHERE s.SysProgram = 'Y' AND s.Active = 'Y' "
                    + "AND m.dbAppUserId = s.dbAppUserId AND m.dbAppServer = s.dbAppServer "
                    + "AND (m.SysProgram = 'N' OR '" + ns + "' = 'RO' ) "
//                    + "AND m.SysProgram = 'N' "
                    + "AND EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = s.dbDesDatabase) "
                    + "AND EXISTS (SELECT 1 FROM master.dbo.sysdatabases WHERE name = m.dbAppDatabase) ", cn);

                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = _CommandTimeout;
                da.SelectCommand = TransformCmd(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow dr in dt.Rows)
                {
                    string dbName = dr["dbDesDatabase"].ToString();
                    if (dbName.ToLower().EndsWith("design") && ns.ToLower() != "ro") continue;
                    DataTable dtX = new DataTable();
                    cmd.CommandText = "SET NOCOUNT ON"
                        + " SELECT Typ='Screen', ProgramName = ProgramName, Description = ScreenDesc "
                        + " FROM " + dbName + ".dbo.Screen WHERE NeedRegen = 'Y' and (GenerateSc = 'Y' or GenerateSr = 'Y') "
                        + " UNION "
                        + " SELECT Typ='Report', ProgramName = ProgramName, Description = ReportDesc "
                        + " FROM " + dbName + ".dbo.Report where NeedRegen = 'Y' and (Generaterp = 'Y') "
                        + " UNION "
                        + " SELECT Typ='Wizard', ProgramName = ProgramName, Description = WizardTitle "
                        + " FROM " + dbName + ".dbo.Wizard where NeedRegen = 'Y' "
                        + "";
                    da.SelectCommand = TransformCmd(cmd);
                    da.Fill(dtX);
                    if (dtX.Rows.Count > 0)
                    {
                        Dictionary<string, string> l = dtX.AsEnumerable().GroupBy(x => x["Typ"].ToString(), x => x.Field<string>("Description")).ToDictionary(x => x.Key, x => string.Join(",", x.ToArray<string>()));
                        regenList[dr["SystemName"].ToString()] = l.Select(x => x.Key + " : " + x.Value).ToList();
                    }
                }
            }
            finally { cn.Close(); }
            return regenList;
        }

        public override void UpdFxRate(string FrCurrency, string ToCurrency, string ToFxRate)
        {
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("UpdFxRate", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FrCurrency", OdbcType.VarChar).Value = FrCurrency;
            cmd.Parameters.Add("@ToCurrency", OdbcType.VarChar).Value = ToCurrency;
            cmd.Parameters.Add("@ToFxRate", OdbcType.VarChar).Value = ToFxRate; // Must use .VarChar to bypass the unable to convert error.
            cmd.CommandTimeout = _CommandTimeout;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "UpdFxRate", "", e.Message.ToString()); }
            finally { cn.Close(); cmd.Dispose(); cmd = null; }
            return;
        }

        public override DataTable GetFxRate(string FrCurrency, string ToCurrency)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(GetDesConnStr()));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("GetFxRate", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FrCurrency", OdbcType.VarChar).Value = FrCurrency;
            cmd.Parameters.Add("@ToCurrency", OdbcType.VarChar).Value = ToCurrency;
            cmd.CommandTimeout = _CommandTimeout;
            da.SelectCommand = TransformCmd(cmd);
            DataTable dt = new DataTable();
            try { da.Fill(dt); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "GetFxRate", "", e.Message.ToString()); }
            finally { cn.Close(); cmd.Dispose(); cmd = null; }
            return dt;
        }

        public override void MkWfStatus(string ScreenObjId, string MasterTable, string appDatabase, string sysDatabase, string dbConnectionString, string dbPassword)
        {
            if (da == null) { throw new System.ObjectDisposedException(GetType().FullName); }
            OdbcConnection cn = new OdbcConnection(Config.ConvertOleDbConnStrToOdbcConnStr(dbConnectionString + DecryptString(dbPassword)));
            cn.Open();
            OdbcCommand cmd = new OdbcCommand("MkWfStatus", cn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ScreenObjId", OdbcType.Numeric).Value = ScreenObjId;
            cmd.Parameters.Add("@MasterTable", OdbcType.Char).Value = MasterTable;
            cmd.Parameters.Add("@appDatabase", OdbcType.VarChar).Value = appDatabase;
            cmd.Parameters.Add("@sysDatabase", OdbcType.VarChar).Value = sysDatabase;
            try { TransformCmd(cmd).ExecuteNonQuery(); }
            catch (Exception e) { ApplicationAssert.CheckCondition(false, "", "", e.Message.ToString()); }
            finally { cn.Close(); }
        }
    }
}