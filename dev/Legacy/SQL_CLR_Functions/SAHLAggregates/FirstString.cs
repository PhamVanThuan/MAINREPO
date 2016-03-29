using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = false, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 8000)]
[Serializable]
public struct FirstString : IBinarySerialize
{
	private string m_FirstResult;
	private int m_Cnt;
	public void Init()
	{
		this.m_FirstResult = string.Empty;
		this.m_Cnt = 0;
	}
	public void Accumulate(SqlString value)
	{
		if (value.IsNull)
		{
			this.m_Cnt++;
		}
		else
		{
			this.m_Cnt++;
			if (this.m_Cnt == 1)
			{
				this.m_FirstResult = value.Value;
			}
		}
	}
	public void Merge(FirstString other)
	{
		if (this.m_Cnt == 0)
		{
			this.m_Cnt = other.m_Cnt;
			this.m_FirstResult = other.m_FirstResult;
		}
		else
		{
			this.m_Cnt += other.m_Cnt;
		}
	}
	public SqlString Terminate()
	{
		string data = string.Empty;
		SqlString result;
		if (this.m_FirstResult.Length > 0)
		{
			data = this.m_FirstResult;
			result = new SqlString(data);
		}
		else
		{
			result = default(SqlString);
		}
		return result;
	}
	public void Read(BinaryReader r)
	{
		this.m_FirstResult = r.ReadString();
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.m_FirstResult);
	}
}
