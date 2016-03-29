using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.IO;
using System.Text;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = true, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 8000)]
[Serializable]
public struct NarrowForwardSlashDelimit : IBinarySerialize
{
	private StringBuilder intermediateResult;
	public void Init()
	{
		this.intermediateResult = new StringBuilder();
	}
	public void Accumulate(SqlString value)
	{
		if (!value.IsNull)
		{
			this.intermediateResult.Append(value.Value).Append("/");
		}
	}
	public void Merge(NarrowForwardSlashDelimit other)
	{
		this.intermediateResult.Append(other.intermediateResult);
	}
	public SqlString Terminate()
	{
		string data = string.Empty;
		if (this.intermediateResult != null && this.intermediateResult.Length > 0)
		{
			data = this.intermediateResult.ToString(0, this.intermediateResult.Length - 1);
		}
		return new SqlString(data);
	}
	public void Read(BinaryReader r)
	{
		this.intermediateResult = new StringBuilder(r.ReadString());
	}
	public void Write(BinaryWriter w)
	{
		w.Write(this.intermediateResult.ToString());
	}
}
