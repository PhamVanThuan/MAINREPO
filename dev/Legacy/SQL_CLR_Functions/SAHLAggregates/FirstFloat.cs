using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = false, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 512)]
[Serializable]
public struct FirstFloat : IBinarySerialize
{
	private double m_FirstResult;
	private int m_Cnt;
	public void Init()
	{
		this.m_FirstResult = double.NaN;
		this.m_Cnt = 0;
	}
	public void Accumulate(SqlDouble value)
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
	public void Merge(FirstFloat other)
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
	public SqlDouble Terminate()
	{
		SqlDouble result;
		if (this.m_FirstResult != double.NaN)
		{
			result = new SqlDouble(this.m_FirstResult);
		}
		else
		{
			result = default(SqlDouble);
		}
		return result;
	}
	public void Read(BinaryReader r)
	{
		this.m_FirstResult = r.ReadDouble();
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.m_FirstResult);
	}
}
