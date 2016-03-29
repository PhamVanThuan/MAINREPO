using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
[SqlUserDefinedAggregate(Format.UserDefined, IsInvariantToNulls = true, IsInvariantToDuplicates = false, IsInvariantToOrder = false, MaxByteSize = 8000)]
[Serializable]
public struct CompositeCompare : IBinarySerialize
{
	private List<Component> _components;
	public void Init()
	{
		this._components = new List<Component>();
	}
	public void Accumulate(SqlString Value)
	{
		Component item = default(Component);
		item.Sequence = (int)Convert.ToInt16(Value.Value.Substring(0, 1));
		item.Gottit = (int)Convert.ToInt16(Value.Value.Substring(1, 1));
		if (item.Gottit == 1)
		{
			item.TransitionOrder = (int)Convert.ToInt16(Value.Value.Substring(2));
		}
		this._components.Add(item);
	}
	public void Merge(CompositeCompare Group)
	{
		foreach (Component current in Group._components)
		{
			this._components.Add(current);
		}
	}
	public SqlString Terminate()
	{
		this.SortList();
		int num = 1;
		int num2 = 1;
		int num3 = 1;
		int num4 = 0;
		int num5 = 1;
		foreach (Component current in this._components)
		{
			if (current.Sequence - num3 < 0)
			{
				string text = "Data in incorrect sequence, cannot proceed with analysis.  ";
				for (int i = 0; i < this._components.Count; i++)
				{
					string text2 = text;
					string[] array = new string[5];
					array[0] = text2;
					string[] arg_7A_0 = array;
					int arg_7A_1 = 1;
					int sequence = this._components[i].Sequence;
					arg_7A_0[arg_7A_1] = sequence.ToString();
					string[] arg_99_0 = array;
					int arg_99_1 = 2;
					int gottit = this._components[i].Gottit;
					arg_99_0[arg_99_1] = gottit.ToString();
					string[] arg_B8_0 = array;
					int arg_B8_1 = 3;
					int transitionOrder = this._components[i].TransitionOrder;
					arg_B8_0[arg_B8_1] = transitionOrder.ToString();
					array[4] = ", ";
					text = string.Concat(array);
				}
				text = text + " compcount : " + this._components.Count;
				throw new Exception(text);
			}
			if (current.Sequence == num3)
			{
				num4 |= current.Gottit;
				if (current.Gottit == 1)
				{
					if (current.TransitionOrder > num)
					{
						num = current.TransitionOrder;
					}
					if (current.TransitionOrder < num2)
					{
						num5 = 0;
						break;
					}
				}
			}
			if (current.Sequence > num3)
			{
				num2 = num;
				num5 &= num4;
				if (num5 == 0)
				{
					break;
				}
				num4 = current.Gottit;
				if (current.Gottit == 1)
				{
					num = current.TransitionOrder;
					if (current.TransitionOrder < num2)
					{
						num5 = 0;
						break;
					}
				}
			}
			num3 = current.Sequence;
		}
		num5 &= num4;
		if (num5 == 0)
		{
			return new SqlString("0");
		}
		return new SqlString("1");
	}
	private void SortList()
	{
		if (this._components.Count < 2)
		{
			return;
		}
		for (int i = 1; i < this._components.Count; i++)
		{
			Component value = this._components[i];
			int num = i;
			while (num > 0 && this._components[num - 1].Sequence > value.Sequence)
			{
				this._components[num] = this._components[num - 1];
				num--;
			}
			this._components[num] = value;
		}
	}
	public void Sort()
	{
		this.SortList();
	}
	public void Read(BinaryReader r)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		this._components = (List<Component>)binaryFormatter.Deserialize(r.BaseStream);
	}
	public void Write(BinaryWriter w)
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		binaryFormatter.Serialize(w.BaseStream, this._components);
	}
}
