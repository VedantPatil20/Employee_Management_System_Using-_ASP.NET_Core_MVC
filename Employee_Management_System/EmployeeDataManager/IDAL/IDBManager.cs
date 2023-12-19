using System.Data;

namespace Employee_Management_System.EmployeeDataManager.IDAL
{
    public interface IDBManager
    {
        EmployeeDataManager.DAL.DBManager InitDbCommand(string cmd);
        EmployeeDataManager.DAL.DBManager InitDbCommand(string cmd, CommandType cmdtype);
        EmployeeDataManager.DAL.DBManager AddCMDParam(string parametername, object value);
        EmployeeDataManager.DAL.DBManager AddCMDParam(string parametername, object value, DbType dbtype);
        EmployeeDataManager.DAL.DBManager AddCMDOutParam(string parametername, DbType dbtype, int iSize = 0);

        T GetOutParam<T>(string paramname);
        DataTable ExecuteDataTable();
        DataSet ExecuteDataSet();

        object? ExecuteScalar();

        Task<object?> ExecuteScalarAsync();
        int ExecuteNonQuery();
        Task<int> ExecuteNonQueryAsync();
        EmployeeDataManager.DAL.DBManager InitDbCommandText(string cmd);
    }
}
