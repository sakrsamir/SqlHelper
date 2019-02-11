using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections;

public class SqlHelper
{
    #region Connection String
    static string constr = ConfigurationManager.ConnectionStrings["cnn"].ConnectionString;
    #endregion


    #region Create Parameters
    public static SqlParameter createInputParamter(string name, SqlDbType type, object value)
    {
        SqlParameter sql = new SqlParameter(name, type);
        sql.Value = value;
        return sql;
    }
    public static SqlParameter createOutputParamter(string name, SqlDbType type, ParameterDirection dir)
    {
        SqlParameter sql = new SqlParameter(name, type);
        sql.Direction = dir;
        return sql;
    }
    #endregion


    #region Disconnected Model
    public static DataSet GetDataSet(string storedName, string tableName, params SqlParameter[] arr)
    {
        SqlConnection cn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(storedName, cn);
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter pram in arr)
        {
            cmd.Parameters.Add(pram);
        }
        DataSet ds = new DataSet();
        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        sda.Fill(ds, tableName);
        return ds;
    }
    #endregion


    #region Connected Model
    /// <summary>
    /// Get DataReader 
    /// </summary>
    /// <param name="storedName">name of stored proceduer</param>
    /// <param name="ConOut">connecton variable to be able to close the original connection</param>
    /// <param name="arr">paramters of the stored</param>
    /// <returns>DataReader that holds data</returns>
    // we must close the sql connection after calling this methods //
    public static SqlDataReader GetDataReader(string storedName, out SqlConnection ConOut, params SqlParameter[] arr)
    {
        SqlConnection cn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(storedName, cn);
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parm in arr)
        {
            cmd.Parameters.Add(parm);
        }
        cn.Open();
        SqlDataReader sdr = cmd.ExecuteReader();
        ConOut = cn;
        return sdr;
    }
    /// <summary>
    /// Excute nonQuery method to update or insert or delete in connected model
    /// </summary>
    /// <param name="storedName">name of strored procedure</param>
    /// <param name="arr">paramters of the stored</param>
    /// <returns>number of effected rows</returns>
    public static int ExcuteNonQuery(string storedName, params SqlParameter[] arr)
    {
        SqlConnection cn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(storedName, cn);
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parm in arr)
        {
            cmd.Parameters.Add(parm);
        }
        cn.Open();
        int x = cmd.ExecuteNonQuery();
        cn.Close();
        return x;
    }
    /// <summary>
    /// Excute Scaler in connected model to insert or update or delete and return one cell
    /// </summary>
    /// <param name="storedName">name of stored procedure</param>
    /// <param name="arr">paramters of the stored</param>
    /// <returns>one cell only</returns>
    public static object ExcuteScaler(string storedName, params SqlParameter[] arr)
    {
        SqlConnection cn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(storedName, cn);
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parm in arr)
        {
            cmd.Parameters.Add(parm);
        }
        cn.Open();
        object o = cmd.ExecuteScalar();
        cn.Close();
        return o;
    }
    /// <summary>
    /// Excute non query output in connected model with two types of paramter input and output
    /// </summary>
    /// <param name="storedName">name of stored procedure</param>
    /// <param name="arr">paramters of the stored</param>
    /// <returns>uses output paramter to set values with it's stored  return more than one item using hashTable</returns>
    public static Hashtable ExcuteNonQueryOutput(string storedName, params SqlParameter[] arr)
    {
        SqlConnection cn = new SqlConnection(constr);
        SqlCommand cmd = new SqlCommand(storedName, cn);
        cmd.CommandType = CommandType.StoredProcedure;
        foreach (SqlParameter parm in arr)
        {
            cmd.Parameters.Add(parm);
        }
        cn.Open();
        int x = cmd.ExecuteNonQuery();
        Hashtable ht = new Hashtable();
        foreach (SqlParameter item in arr)
        {
            if (item.Direction == ParameterDirection.Output)
            {
                ht.Add(item.ParameterName, item.Value);
            }
        }
        cn.Close();
        return ht;
    } 
    #endregion

}

