using Employee_Management_System.EmployeeDataManager.IDAL;
using Employee_Management_System.Extension;
using System.Data;
using System.Data.Common;

namespace Employee_Management_System.EmployeeDataManager.DAL
{
    public class DBManager : IDisposable, IDBManager
    {
        private readonly DbConnection _DbConnection;
        private string _DBCmd;
        private CommandType _CommandType;
        private List<DBParam> _InputParam;
        private List<DBParam> _OutParam;
        public DBManager(DbConnection dbcon)
        {
            _DbConnection = dbcon;
        }

        // Open MySQL Connection
        void Open()
        {
            if (_DbConnection.State != ConnectionState.Open)
                _DbConnection.Open();
        }

        // CLose MySQL Connection
        void Close()
        {
            if (_DbConnection.State != ConnectionState.Closed)
                Close();
        }

        // For Stored Procedure
        public DBManager InitDbCommand(string cmd)
        {
            InitDbCommand(cmd, CommandType.StoredProcedure);
            return this;
        }

        public DBManager InitDbCommand(string cmd, CommandType cmdtype)
        {
            _DBCmd = cmd;
            _CommandType = cmdtype;
            _InputParam = new();
            _OutParam = new();
            return this;
        }

        // Add data to database
        public DBManager AddCMDParam(string parametername, object value)
        {
            _InputParam.Add(new()
            {
                ParamName = parametername,
                Value = value
            });
            return this;
        }
        public DBManager AddCMDParam(string parametername, object value, DbType dbtype)
        {
            _InputParam.Add(new()
            {
                ParamName = parametername,
                Value = value,
                DbType = dbtype
            });
            return this;
        }

        public DBManager AddCMDOutParam(string parametername, DbType dbtype, int iSize = 0)
        {

            if (dbtype == DbType.String && iSize > 0)
            {
                _OutParam.Add(new()
                {
                    iSize = iSize,
                    ParamName = parametername,
                    DbType = dbtype
                });

            }
            else
            {
                _OutParam.Add(new()
                {

                    ParamName = parametername,
                    DbType = dbtype
                });
            }
            return this;
        }

        public DataTable ExecuteDataTable()
        {
            DataTable result = null;
            using (DbCommand _DbCommand = CreateDbCMD())
            {
                Open();
                result = _DbCommand.ExecuteDataTable();
                GetOutParam(_DbCommand);
            }
            return result;
        }

        public DataSet ExecuteDataSet()
        {
            DataSet result = null;
            using (DbCommand _DbCommand = CreateDbCMD())
            {
                Open();
                result = _DbCommand.ExecuteDataSet();
                GetOutParam(_DbCommand);
            }
            return result;
        }

        public object? ExecuteScalar()
        {
            object? result = null;
            using (DbCommand _DbCommand = CreateDbCMD())
            {
                Open();
                result = _DbCommand.ExecuteScalar();
                GetOutParam(_DbCommand);
            }
            return result;
        }

        public async Task<object?> ExecuteScalarAsync()
        {
            object? result = null;
            using (DbCommand _DbCommand = CreateDbCMD())
            {
                Open();
                result = await _DbCommand.ExecuteScalarAsync();
                GetOutParam(_DbCommand);
            }
            return result;
        }

        public int ExecuteNonQuery()
        {
            int result = -1;
            using (DbCommand _DbCommand = CreateDbCMD())
            {
                Open();
                result = _DbCommand.ExecuteNonQuery();
                GetOutParam(_DbCommand);
            }
            return result;
        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            int result = -1;
            using (DbCommand _DbCommand = CreateDbCMD())
            {
                Open();
                result = await _DbCommand.ExecuteNonQueryAsync();
                GetOutParam(_DbCommand);
            }
            return result;
        }
        DbCommand CreateDbCMD()
        {
            DbCommand _DbCommand = _DbConnection.CreateDbCMD(_DBCmd, _CommandType);
            AddInputPara(_DbCommand);
            AddOutputParam(_DbCommand);
            return _DbCommand;
        }


        void AddInputPara(DbCommand _DbCommand)
        {
            foreach (DBParam item in _InputParam)
            {
                if (item.DbType.HasValue)
                    _DbCommand.AddCMDParam(item.ParamName, item.Value, (DbType)item.DbType);
                else
                    _DbCommand.AddCMDParam(item.ParamName, item.Value);
            }
        }

        void AddOutputParam(DbCommand _DbCommand)
        {
            foreach (DBParam item in _OutParam)
            {
                if (item.DbType == DbType.String)
                {
                    _DbCommand.AddCMDOutParam(item.ParamName, (DbType)item.DbType, item.iSize);
                }
                else
                {
                    _DbCommand.AddCMDOutParam(item.ParamName, (DbType)item.DbType);
                }
            }
        }

        void GetOutParam(DbCommand _DbCommand)
        {
            foreach (DBParam item in _OutParam)
            {
                item.Value = _DbCommand.Parameters[item.ParamName].Value.ToString();
            }
        }
        public DBManager InitDbCommandText(string cmd)
        {
            InitDbCommand(cmd, CommandType.Text);
            return this;
        }
        public T GetOutParam<T>(string paramname)
        {
            object result = null;
            DBParam outparam = _OutParam.Where(a => a.ParamName == paramname).FirstOrDefault();
            if (outparam != null)
            {
                switch (outparam.DbType)
                {
                    case DbType.Boolean:
                        string strval = outparam.Value.ToString();
                        if (strval.Length == 1)
                        {
                            result = strval == "1";
                        }
                        else
                        {
                            result = Convert.ToBoolean(outparam.Value);
                        }
                        break;
                    case DbType.Date:
                    case DbType.DateTime:
                        result = Convert.ToDateTime(outparam.Value);
                        break;
                    case DbType.Decimal:
                        result = Convert.ToDecimal(outparam.Value);
                        break;
                    case DbType.Double:
                        result = Double.Parse(outparam.Value.ToString());
                        break;
                    case DbType.Int16:
                        result = Convert.ToInt16(outparam.Value);
                        break;
                    case DbType.Int32:
                        result = Convert.ToInt32(outparam.Value);
                        break;
                    case DbType.Int64:
                        result = Convert.ToInt64(outparam.Value);
                        break;
                    case DbType.String:
                        result = Convert.ToString(outparam.Value);
                        break;
                }
            }
            return (T)result;
        }

        public void Dispose()
        {
            _DbConnection.Close();
            _DbConnection.Dispose();
        }

        private class DBParam
        {
            public string ParamName { get; set; }
            public object? Value { get; set; }
            public DbType? DbType { get; set; }
            public int iSize { get; set; }
        }
    }
}
