using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = false, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 512)]
[Serializable]
public struct FirstDate : IBinarySerialize
{
	private DateTime m_FirstResult;
	private int m_Cnt;
	public void Init()
	{
		this.m_FirstResult = DateTime.MinValue;
		this.m_Cnt = 0;
	}
	public void Accumulate(SqlDateTime value)
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
	public void Merge(FirstDate other)
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
	public SqlDateTime Terminate()
	{
		SqlDateTime result;
		if (this.m_FirstResult != DateTime.MinValue)
		{
			result = new SqlDateTime(this.m_FirstResult);
		}
		else
		{
			result = default(SqlDateTime);
		}
		return result;
	}
	public void Read(BinaryReader r)
	{
		this.m_FirstResult = DateTime.FromBinary(r.ReadInt64());
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.m_FirstResult.ToBinary());
	}
}
