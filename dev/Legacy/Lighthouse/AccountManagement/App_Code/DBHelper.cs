using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

public class DBHelper
{
    public SqlConnection con = null;
    public SqlDataAdapter adap = null;
    public DataSet ds = new DataSet();
    public int TableCount
    {
        get { return ds.Tables.Count; }
    }
    public int RowCount
    {
        get
        {
            try
            {
                return CurrentTable().Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    private int CurrentTableCounter = 0;
    private int CurrentRowCounter = 0;

    public DBHelper(string Procedure, CommandType type)
    {
        con = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        adap = new SqlDataAdapter(Procedure, con);

        adap.SelectCommand.CommandType = type;
        adap.SelectCommand.CommandTimeout = 0;
    }
    public DBHelper(string Procedure)
    {
        con = new SqlConnection(DBConnection.ConnectionString().Replace("SAHLDB", "2am"));
        adap = new SqlDataAdapter(Procedure, con);

        adap.SelectCommand.CommandType = CommandType.Text;
        adap.SelectCommand.CommandTimeout = 0;
    }

    public SqlDataAdapter Adapter
    {
        get { return adap; }
    }
    public SqlParameterCollection Parameters
    {
        get { return adap.SelectCommand.Parameters; }
    }

    public DataTable Fill(DataTable table)
    {
        try
        {
            adap.Fill(table);
            return table;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataSet Fill()
    {
        try
        {
            adap.Fill(ds);
            return ds;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public int ExecuteNonQuery()
    {
        try
        {
            con.Open();
            return adap.SelectCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            con.Close();
        }
    }
    public SqlCommand Command
    {
        get { return Adapter.SelectCommand; }
        set
        {
            Adapter.SelectCommand = value;
        }
    }
    public string CommandText
    {
        get { return Command.CommandText; }
        set { Command.CommandText = value; }
    }

    public void AddDateParameter(string Name, object value)
    {
        AddParameter(Name, SqlDbType.DateTime, ParameterDirection.Input, value);
    }
    public void AddIntParameter(string Name, object value)
    {
        AddParameter(Name, SqlDbType.Int, ParameterDirection.Input, value);
    }
    public void AddVarcharParameter(string Name, object value)
    {
        AddParameter(Name, SqlDbType.VarChar, ParameterDirection.Input, value);
    }
    public void AddParameter(string Name, SqlDbType dbType, ParameterDirection Direction, object value)
    {
        try
        {
            SqlParameter parm = new SqlParameter(Name, dbType);
            parm.Direction = Direction;
            parm.Value = value;
            Parameters.Add(parm);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public DataTable Table(int index)
    {
        return ds.Tables[index];
    }
    public DataTable Table(string Name)
    {
        return ds.Tables[Name];
    }

    public DataTable FirstTable()
    {
        return ds.Tables[0];
    }
    public DataTable CurrentTable()
    {
        return Table(CurrentTableCounter);
    }
    public DataTable NextTable()
    {
        if (ds.Tables.Count > 0)
        {
            try
            {
                if (++CurrentTableCounter >= TableCount)
                    CurrentTableCounter = 0;
                return ds.Tables[CurrentTableCounter];
            }
            catch
            {
                return null;
            }
        }
        return null;
    }
    public DataTable PrevTable()
    {
        if (ds.Tables.Count > 0)
        {
            try
            {
                if (--CurrentTableCounter < 0)
                    CurrentTableCounter = TableCount - 1;
                return ds.Tables[CurrentTableCounter];
            }
            catch
            {
                return null;
            }
        }
        return null;
    }

    public DataRow FirstRow()
    {
        if (RowCount > 0)
        {
            try
            {
                return CurrentTable().Rows[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return null;
    }
    public DataRow NextRow()
    {
        if (RowCount > 0)
        {
            try
            {
                if (++CurrentRowCounter >= RowCount)
                    CurrentRowCounter = 0;
                return CurrentTable().Rows[CurrentRowCounter];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return null;
    }
    public DataRow PrevRow()
    {
        if (RowCount > 0)
        {
            try
            {
                if (--CurrentRowCounter < 0)
                    CurrentRowCounter = RowCount - 1;
                return CurrentTable().Rows[CurrentRowCounter];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return null;
    }
}