using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace Employee_Management_System.Extension
{
    public static class DBmanagerExtensions
    {
        internal static DbCommand CreateDbCMD(this DbConnection con, string cmdtext, CommandType cmdtype)
        {
            DbCommand cmd = con.CreateCommand();
            cmd.CommandType = cmdtype;
            cmd.CommandText = cmdtext;
            cmd.Connection = con;
            return cmd;
        }
        internal static void AddCMDParam(this DbCommand cmd, string parametername, object value)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = parametername;
            param.Value = value == null ? DBNull.Value : value;
            cmd.Parameters.Add(param);
        }
        internal static void AddCMDParam(this DbCommand cmd, string parametername, object value, DbType dbtype)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = parametername;
            param.Value = value == null ? DBNull.Value : value;
            param.DbType = dbtype;
            cmd.Parameters.Add(param);
        }

        internal static void AddCMDOutParam(this DbCommand cmd, string parametername, DbType dbtype, int iSize = 0)
        {
            DbParameter param = cmd.CreateParameter();
            param.ParameterName = parametername;
            param.DbType = dbtype;
            param.Direction = ParameterDirection.Output;
            if (iSize > 0)
            { param.Size = iSize; }

            cmd.Parameters.Add(param);
        }

        internal static bool IsDBNull(this DbDataReader dataReader, string columnName)
        {
            return dataReader[columnName] == DBNull.Value;
        }

        internal static T GetOutParameter<T>(this DbCommand DbCommand, string parametername)
        {
            object str = null;
            TypeCode typeCode = Type.GetTypeCode(typeof(T));
            switch (typeCode)
            {
                case TypeCode.String:
                    str = DbCommand.Parameters[parametername].Value.ToString();
                    break;

                case TypeCode.Int16:
                case TypeCode.Int32:
                    str = Convert.ToInt32(DbCommand.Parameters[parametername].Value);
                    break;
            }
            return (T)str;
        }

        internal static DataTable ExecuteDataTable(this DbCommand DbCommand)
        {
            DataTable result = new();
            DbDataAdapter oDataAdapter = new MySqlDataAdapter();
            oDataAdapter.SelectCommand = DbCommand;
            oDataAdapter.Fill(result);
            return result;
        }

        internal static DataSet ExecuteDataSet(this DbCommand DbCommand)
        {
            DataSet result = new();
            DbDataAdapter oDataAdapter = new MySqlDataAdapter();
            oDataAdapter.SelectCommand = DbCommand;
            oDataAdapter.Fill(result);
            return result;
        }
    }
}
