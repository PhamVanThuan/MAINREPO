using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = false, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 8000)]
[Serializable]
public struct SecondFloat : IBinarySerialize
{
	private double m_SecondResult;
	private int m_Cnt;
	public void Init()
	{
		this.m_SecondResult = double.NaN;
		this.m_Cnt = 0;
	}
	public void Accumulate(SqlDouble value)
	{
		this.m_Cnt++;
		if (!value.IsNull)
		{
			if (this.m_Cnt == 2)
			{
				this.m_SecondResult = value.Value;
			}
		}
	}
	public void Merge(SecondFloat other)
	{
		if (other.m_Cnt != 0)
		{
			if (this.m_Cnt == 0)
			{
				this.m_Cnt = other.m_Cnt;
				this.m_SecondResult = other.m_SecondResult;
			}
			else
			{
				if (this.m_Cnt >= 2)
				{
					this.m_Cnt += other.m_Cnt;
				}
				else
				{
					this.m_Cnt += other.m_Cnt;
					this.m_SecondResult = other.m_SecondResult;
				}
			}
		}
	}
	public SqlDouble Terminate()
	{
		SqlDouble result;
		if (this.m_SecondResult != double.NaN)
		{
			result = new SqlDouble(this.m_SecondResult);
		}
		else
		{
			result = new SqlDouble(0.0);
		}
		return result;
	}
	public void Read(BinaryReader r)
	{
		this.m_SecondResult = r.ReadDouble();
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.m_SecondResult);
	}
}
