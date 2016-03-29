using Microsoft.SqlServer.Server;
using System;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = true, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 8000)]
[Serializable]
public struct AnalyzeResults : IBinarySerialize
{
	private int var1;
	private StringCollection _components;
	public void Init()
	{
	}
	public void Accumulate(SqlString Value)
	{
	}
	public void Merge(AnalyzeResults Group)
	{
	}
	public SqlString Terminate()
	{
		return new SqlString("");
	}
	public void Read(BinaryReader r)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		this._components = (StringCollection)binaryFormatter.Deserialize(r.BaseStream);
	}
	public void Write(BinaryWriter w)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(w.BaseStream, this._components);
	}
}
