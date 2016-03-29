using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = false, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 512)]
[Serializable]
public struct FirstInt : IBinarySerialize
{
	private int m_FirstResult;
	private int m_Cnt;
	public void Init()
	{
		this.m_FirstResult = -2147483648;
		this.m_Cnt = 0;
	}
	public void Accumulate(SqlInt32 value)
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
	public void Merge(FirstInt other)
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
	public SqlInt32 Terminate()
	{
		SqlInt32 result;
		if (this.m_FirstResult != -2147483648)
		{
			result = new SqlInt32(this.m_FirstResult);
		}
		else
		{
			result = default(SqlInt32);
		}
		return result;
	}
	public void Read(BinaryReader r)
	{
		this.m_FirstResult = r.ReadInt32();
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.m_FirstResult);
	}
}
