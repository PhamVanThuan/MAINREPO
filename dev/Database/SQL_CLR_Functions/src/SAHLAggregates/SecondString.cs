using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = false, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 8000)]
[Serializable]
public struct SecondString : IBinarySerialize
{
	private string m_SecondResult;
	private int m_Cnt;
	public void Init()
	{
		this.m_SecondResult = string.Empty;
		this.m_Cnt = 0;
	}
	public void Accumulate(SqlString value)
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
	public void Merge(SecondString other)
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
	public SqlString Terminate()
	{
		string data = string.Empty;
		SqlString result;
		if (this.m_SecondResult.Length > 0)
		{
			data = this.m_SecondResult;
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
		this.m_SecondResult = r.ReadString();
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.m_SecondResult);
	}
}
