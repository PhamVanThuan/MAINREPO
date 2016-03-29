using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using SAHL.Test;
using SAHL.Common.Security;

using Castle.ActiveRecord;
using NUnit.Framework;
using System.Security.Principal;
using System.Reflection;
using Castle.ActiveRecord.Framework.Internal;
using Castle.ActiveRecord.Framework.Config;
using System.Collections;
using System.Diagnostics;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using System.Linq;
using SAHL.Common.BusinessModel.DAO.Attributes;
using NHibernate.Criterion;
using System.IO;
using SAHL.Test.Exceptions;
using SAHL.Common.Globals;
using Castle.Components.Validator;

namespace SAHL.Common.BusinessModel.DAO.InMemoryTest
{
	public class DAODataConsistancyChecker
	{
		public static Dictionary<int, object> ToDelete = new Dictionary<int, object>();

		public static void CleanUp()
		{
			var objectsToDelete = (from o in ToDelete
								   orderby o.Key descending
								   select o.Value).ToList<object>();

			foreach (object objToDelete in objectsToDelete)
			{
				try
				{
					System.Diagnostics.Debug.WriteLine("cleanup - {0}", objToDelete);
					Type ARBase = GetARBase(objToDelete);
					object deleteobj = ARBase.InvokeMember("DeleteAndFlush", (BindingFlags.InvokeMethod), null, objToDelete, null);
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine("cleanup failed - {0}", objToDelete);
					System.Diagnostics.Debug.WriteLine(ex.StackTrace);
				}
			}
			ToDelete = new Dictionary<int, object>();
			BelongsToType.Clear();
		}

		public static Dictionary<Type, object> BelongsToType = new Dictionary<Type, object>();
		private static string TestString = "RS";// changed cause varchar(10) and the like wont work cause 10 is too short for this mess maybe I should keep typing so you get the point. - Generic Test";
		/// <summary>
		/// Get DAO
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T GetDAO<T>()
		{
			object daoObject = null;
			try
			{
				Type TT = typeof(T);
				string TN = TT.ToString();
				daoObject = Activator.CreateInstance(TT);
				ActiveRecordModel arm = GetModel(TT);

				Dictionary<string, object> Props = new Dictionary<string, object>();
				//List<string> Messages = new List<string>();
				// Value of the primary key.
				//int Key = -1;
				object Key = new object();
				// name of the key field on the DAO is normally Key but can be anything eg RuleItemKey
				string KeyNameProperty = "Key";
				// System.Type of the key. Will normally be int32 (autoinc) but can be string eg ACBBranch
				PropertyInfo piKey = null;
				Type ARBase = null;
				PrimaryKeyModel pkm = null;
				PrimaryKeyType pkType = PrimaryKeyType.Native;

				daoObject = PopulateObject(daoObject, KeyNameProperty, piKey, ARBase, Props, ref Key, arm, pkm, ref pkType);
			}
			catch (Exception ex)
			{
				string s = ex.ToString();
				throw;
			}
			return (T)daoObject;
		}

		/// <summary>
		/// Renew DAO Data
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="DAOObject"></param>
		public static void RenewDAOData<T>(T DAOObject)
		{
			try
			{
				Type TT = typeof(T);
				string TN = TT.ToString();
				ActiveRecordModel arm = GetModel(TT);

				Dictionary<string, object> Props = new Dictionary<string, object>();
				//List<string> Messages = new List<string>();
				// Value of the primary key.
				//int Key = -1;
				object Key = new object();
				// name of the key field on the DAO is normally Key but can be anything eg RuleItemKey
				string KeyNameProperty = "Key";
				// System.Type of the key. Will normally be int32 (autoinc) but can be string eg ACBBranch
				PropertyInfo piKey = null;
				Type ARBase = null;
				PrimaryKeyModel pkm = null;
				PrimaryKeyType pkType = PrimaryKeyType.Native;

				DAOObject = (T)PopulateObject((object)DAOObject, KeyNameProperty, piKey, ARBase, Props, ref Key, arm, pkm, ref pkType);
			}
			catch (Exception ex)
			{
				string s = ex.ToString();
				throw;
			}
		}

		/// <summary>
		/// Populate Object
		/// </summary>
		/// <param name="daoObject"></param>
		/// <param name="KeyNameProperty"></param>
		/// <param name="piKey"></param>
		/// <param name="ARBase"></param>
		/// <param name="Props"></param>
		/// <param name="Key"></param>
		/// <param name="arm"></param>
		/// <param name="pkm"></param>
		/// <param name="pkType"></param>
		/// <returns></returns>
		private static object PopulateObject(object daoObject, string KeyNameProperty, PropertyInfo piKey, Type ARBase,
			Dictionary<string, object> Props, ref object Key, ActiveRecordModel arm, PrimaryKeyModel pkm, ref PrimaryKeyType pkType)
		{
			#region Get Model and PK

			ARBase = GetARBase(daoObject);
			arm = GetModel(daoObject.GetType());
			//if (arm.IsDiscriminatorSubClass)
			//{
			//    return null;
			//}
			if (null == arm)
			{
				throw new Exception(string.Format("Unable to get ActiveRecordModel for type:{0}", daoObject.GetType()));
			}

			pkm = GetPKModel(arm, daoObject.GetType());
			KeyNameProperty = pkm.Property.Name;
			if (null == pkm)
			{
				throw new Exception(string.Format("Unable to get PrimaryKeyModel for type:{0}", daoObject.GetType()));
			}
			pkType = pkm.PrimaryKeyAtt.Generator;
			if (pkm.PrimaryKeyAtt.Generator == PrimaryKeyType.Native || pkm.PrimaryKeyAtt.Generator == PrimaryKeyType.Assigned)
			{
			}
			else if (pkm.PrimaryKeyAtt.Generator == PrimaryKeyType.Foreign)
			{
				// go look for the onetoone attribute
				int c = arm.OneToOnes.Count;
				foreach (OneToOneModel oto in arm.OneToOnes)
				{
					Type FKType = oto.Property.PropertyType;
					// find in the DB one of the objects that is of the same type as the FK
					Type piBase = GetARBaseForType(FKType);
					DetachedCriteria d = DetachedCriteria.For(FKType);
					// Order o = new Order(KeyNameProperty, false);
					// d.AddOrder(o);
					object obj = piBase.InvokeMember("FindFirst", BindingFlags.InvokeMethod, null, daoObject, new DetachedCriteria[] { d });
					if (obj == null)
					{
						obj = Activator.CreateInstance(FKType);
						LoadSaveLoad(obj, "Key", false);
					}

					PropertyInfo[] pis = obj.GetType().GetProperties();
					foreach (PropertyInfo pi in pis)
					{
						if (pi.Name == KeyNameProperty)
						{
							Key = Convert.ToInt32(pi.GetValue(obj, null));
							continue;
						}
					}
					// set the PK,FK to be the one we looked up in the DB
					try
					{
						oto.Property.SetValue(daoObject, obj, null);
					}
					catch (Exception ex)
					{
						// legalentitytype does not have a set operator this is likely cause of the
						// AR discriminator type. IE we dont want to change an LE type once
						// we have created it.
						if (ex.Message != "Property set method not found.")
							throw;
					}
				}
			}
			else
			{
				throw new Exception(string.Format("{0} not supported, only Native", pkm.PrimaryKeyAtt.Generator));
				//Debug.WriteLine(string.Format("{0} not supported, only Native", pkm.PrimaryKeyAtt.Generator));
			}
			#endregion
			#region Property Type Switch
			KeyNameProperty = pkm.Property.Name;// should be Key
			SetProperties(daoObject, arm, Props);
			#endregion
			#region Belongs to
			SetBelongsTo(daoObject, KeyNameProperty);
			#endregion

			return daoObject;
		}

		/// <summary>
		/// Default stratergy for Load Save Load
		/// Load Save Load
		/// </summary>
		/// <param name="daoObject"></param>
		/// <param name="DefaultKeyColName"></param>
		public static void LoadSaveLoad(object daoObject, string DefaultKeyColName)
		{
			LoadSaveLoad(daoObject, DefaultKeyColName, true);
		}

		/// <summary>
		/// Load Save Load
		/// </summary>
		/// <param name="daoObject"></param>
		/// <param name="DefaultKeyColName"></param>
		/// <param name="requiresAudit"></param>
		public static void LoadSaveLoad(object daoObject, string DefaultKeyColName, bool delete)
		{
			//TransactionScope ts = null;
			Dictionary<string, object> Props = new Dictionary<string, object>();
			List<string> Messages = new List<string>();
			// Value of the primary key.
			//int Key = -1;
			object Key = new object();
			// name of the key field on the DAO is normally Key but can be anything eg RuleItemKey
			string KeyNameProperty = DefaultKeyColName;
			// System.Type of the key. Will normally be int32 (autoinc) but can be string eg ACBBranch
			PropertyInfo piKey = null;
			Type ARBase = null;
			ActiveRecordModel arm = null;
			PrimaryKeyModel pkm = null;
			PrimaryKeyType pkType = PrimaryKeyType.Native;
			//ts = new TransactionScope();
			daoObject = PopulateObject(daoObject, KeyNameProperty, piKey, ARBase, Props, ref Key, arm, pkm, ref pkType);
			if (null == daoObject)
				return;//throw new Exception(string.Format("Unable to populate object:{0}", daoObject.ToString()));
			//ts.Dispose();
			#region Save the object
			// save the object
			//ts = new TransactionScope();
			//using (new TransactionScope())
			{
				Type ttmp = daoObject.GetType();
				piKey = ttmp.GetProperty(KeyNameProperty);
				if (pkType == PrimaryKeyType.Native)
				{
					//object otmp = ARBase.InvokeMember("CreateAndFlush", BindingFlags.InvokeMethod, null, daoObject, null);
					object o = ttmp.InvokeMember("CreateAndFlush", BindingFlags.InvokeMethod, null, daoObject, null);
				}
				else if (pkType == PrimaryKeyType.Foreign)
				{
					piKey.SetValue(daoObject, Key, null);
					object o = ttmp.InvokeMember("CreateAndFlush", BindingFlags.InvokeMethod, null, daoObject, null);
				}
				else
				{

					Key = (Int32.MaxValue - 1);
					if (piKey.PropertyType == typeof(string))
						piKey.SetValue(daoObject, Key.ToString(), null);
					else if (piKey.PropertyType == typeof(decimal))
						piKey.SetValue(daoObject, Convert.ToDecimal(Key), null);
					else if (piKey.PropertyType == typeof(Int64))
						piKey.SetValue(daoObject, Convert.ToInt64(Key), null);
					else if (daoObject is Account_DAO)
					{
						var accountSequenceDAO = new AccountSequence_DAO();
						accountSequenceDAO.CreateAndFlush();
						piKey.SetValue(daoObject, accountSequenceDAO.Key, null); ;
					}
					else
						piKey.SetValue(daoObject, Key, null);
					object o = ttmp.InvokeMember("CreateAndFlush", BindingFlags.InvokeMethod, null, daoObject, null);
				}

				PropertyInfo pi = ttmp.GetProperty(KeyNameProperty);
				if (null == pi)
				{
					// Key Property not found.
					throw new Exception(string.Format("Propery Key does not exist on object {0}", ttmp));
				}
				Key = Convert.ToInt32(pi.GetValue(daoObject, null));
			}
			#endregion
			//ts.Dispose();

			#region Load it back up and verify the values are correct
			//ts = new TransactionScope();
			ARBase = GetARBase(daoObject);
			object retval = null;
			if (piKey.PropertyType == typeof(string))
				retval = ARBase.InvokeMember("Find", (BindingFlags.InvokeMethod), null, daoObject, new object[] { Key.ToString() });
			else if (piKey.PropertyType == typeof(decimal))
				retval = ARBase.InvokeMember("Find", (BindingFlags.InvokeMethod), null, daoObject, new object[] { Convert.ToDecimal(Key) });
			else if (piKey.PropertyType == typeof(Int64))
				retval = ARBase.InvokeMember("Find", (BindingFlags.InvokeMethod), null, daoObject, new object[] { Convert.ToInt64(Key) });
			else
				retval = ARBase.InvokeMember("Find", (BindingFlags.InvokeMethod), null, daoObject, new object[] { Key });

			arm = ActiveRecordModel.GetModel(retval.GetType());
			foreach (PropertyModel pm in arm.Properties)
			{
				PropertyInfo pi = pm.Property;
				string PropName = pi.Name;
				object val = pi.GetValue(daoObject, null);
				//only check it if you can set it
				if (Props.ContainsKey(PropName))
				{
					object ToCompareTo = Props[PropName];
					if (!val.Equals(ToCompareTo))
					{
						Messages.Add(string.Format("Property {0} does not match, Expected {1} was {2}", PropName, ToCompareTo, val));
					}
				}
			}
			#endregion
			//ts.Dispose();

			// Delete the sucker out the DB
			if (delete)
			{
				//ts = new TransactionScope();
				object deleteobj = ARBase.InvokeMember("DeleteAndFlush", (BindingFlags.InvokeMethod), null, daoObject, null);
				//ts.Dispose();
			}
			else
			{
				// required as part of a belongs to attribute so delete after the primary dao has been tested
				int key = ToDelete.Keys.Count > 0 ? ToDelete.Keys.Max() + 1 : 0;
				ToDelete.Add(key, daoObject);
			}
		}

		/// <summary>
		/// Find First
		/// </summary>
		/// <param name="daoObject"></param>
		/// <param name="DefaultKeyColName"></param>
		public static void FindFirst(object daoObject, string DefaultKeyColName)
		{
			object obj = null;
			Type ARBase = null;
			ARBase = GetARBase(daoObject);
			obj = ARBase.InvokeMember("FindFirst", BindingFlags.InvokeMethod, null, daoObject, null);
			return;
		}

		#region Helpers
		static void Switch(object daoObject, Dictionary<string, object> Props, PropertyModel pm)
		{
			PropertyInfo pi = pm.Property;
			string TypeName = pi.PropertyType.ToString().Replace("System.", "");
			string PropName = pi.Name;
			if (!pm.Property.CanWrite)
				return;

			try
			{
				switch (TypeName)
				{
					case "Nullable`1[Int64]":
					case "Int64":
						{
							Int64 val = GetRandomInt64();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Int16":
					case "Nullable`1[Int16]":
						{
							Int16 val = GetRandomInt16();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Int32":
					case "Nullable`1[Int32]":
						{
							Int32 val = GetRandomInt();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "String":
						{
							string val = TestString;
							// truncate the random string if a length attribute has been specified that is less than the length of the random string
							if (pm.PropertyAtt.Length > 0 && pm.PropertyAtt.Length < val.Length)
								val = val.Substring(0, pm.PropertyAtt.Length);

							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "bool":
					case "Boolean":
					case "Nullable`1[Boolean]":
						{
							bool val = GetRandomBool();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Nullable`1[Double]":
					case "Double":
					case "double":
						{
							double val = GetRandomDouble();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Nullable`1[DateTime]":
					case "DateTime":
						{
							DateTime val = DateTime.Now;
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Char":
						{
							char val = 'I';
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Nullable`1[Byte]":
					case "Byte":
						{
							Byte val = Convert.ToByte(TestString[0]);
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Nullable`1[Decimal]":
					case "Decimal":
						{
							Decimal val = (Decimal)GetRandomDouble();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					case "Nullable`1[Single]":
					case "Single":
						{
							Single val = (Single)GetRandomInt16();
							if (Props.ContainsKey(PropName)) break;
							Props.Add(PropName, val);
							pi.SetValue(daoObject, val, null);
							break;
						}
					default:
						{
							Console.WriteLine("Unsupported Type: {0}", TypeName);
							break;
						}
				}
			}
			catch
			{
				throw;
			}
		}

		static ActiveRecordModel GetModel(Type DAOType)
		{
			ActiveRecordModel arm = ActiveRecordModel.GetModel(DAOType);
			if (null == arm)
			{
				if (DAOType != typeof(System.Object))
					return (GetModel(DAOType.BaseType));
			}
			return arm;
		}

		static PrimaryKeyModel GetPKModel(ActiveRecordModel arm, Type DAOObject)
		{
			PrimaryKeyModel pkm = arm.PrimaryKey;
			if (null == pkm)
			{
				// see if this is a derived type and the PK is defined in the base
				if (DAOObject.BaseType == typeof(Object))
				{
					return null;
				}
				ActiveRecordModel basearm = GetModel(DAOObject.BaseType);
				if (null != basearm)
					return GetPKModel(basearm, DAOObject.BaseType);
			}
			return pkm;
		}

		static void SetProperties(object daoObject, ActiveRecordModel arm, Dictionary<string, object> Props)
		{
			// loop through all the object types until you find one that is generic - this is because the 
			// ActiveRecordModel only gives model info for the properties of the declaring class
			// and ignores the inherited properties.

			Type current = daoObject.GetType();

			while (!current.IsGenericType)
			{
				DateTime dtStart = DateTime.Now;

				ActiveRecordModel armBase = ActiveRecordModel.GetModel(current);
				foreach (PropertyModel pm in armBase.Properties)
				{
					Switch(daoObject, Props, pm);
				}

				TimingObject to = new TimingObject(dtStart, DateTime.Now, daoObject, TestType.None);
				Debug.WriteLine(string.Format("populating type {0}, {1}", current.Name, to.TotalSeconds));
				current = current.BaseType;
			}

		}

		static void SetBelongsTo(object daoObject, string KeyNameProperty)
		{
			// loop through all the object types until you find one that is generic - this is because the 
			// ActiveRecordModel only gives model info for the properties of the declaring class
			// and ignores the inherited properties.

			Type current = daoObject.GetType();

			while (!current.IsGenericType)
			{
				DateTime dtStart = DateTime.Now;
				//Debug.WriteLine(string.Format("populating belongs to {0}", current.Name));
				ActiveRecordModel armBase = ActiveRecordModel.GetModel(current);

				// extract all the mandatory properties
				List<string> mandatoryProperties = new List<string>();
				foreach (NonEmptyValidator v in armBase.Validators)
				{
					mandatoryProperties.Add(v.Name);
				}

				foreach (BelongsToModel blm in armBase.BelongsTo)
				{
					if (blm.BelongsToAtt.NotNull || mandatoryProperties.Contains(blm.Property.Name))
					{
						Type ptype = blm.Property.PropertyType;
						object obj = null;
						if (BelongsToType.ContainsKey(ptype))
						{
							obj = BelongsToType[ptype];
						}
						else
						{
							// we need to go and find one of these else we wont be able to save.
							Type piBase = GetARBaseForType(ptype);
							DetachedCriteria d = DetachedCriteria.For(ptype);
							//This will fail if the object is modelled incorrectly
							obj = piBase.InvokeMember("FindFirst", BindingFlags.InvokeMethod, null, daoObject, new DetachedCriteria[] { d });
							if (obj == null)
							{
								obj = Activator.CreateInstance(ptype);
								LoadSaveLoad(obj, "Key", false);
							}
							BelongsToType.Add(ptype, obj);
						}
						try
						{
							blm.Property.SetValue(daoObject, obj, null);
						}
						catch (Exception ex)
						{
							// legalentitytype does not have a set operator this is likely cause of the
							// AR discriminator type. IE we dont want to change an LE type once
							// we have created it.
							if (ex.Message != "Property set method not found.")
								throw;
						}
					}
				}

				TimingObject to = new TimingObject(dtStart, DateTime.Now, null, TestType.None);
				Debug.WriteLine(string.Format("populating belongs to {0}, {1}", current.Name, to.TotalSeconds));
				current = current.BaseType;
			}
		}

		static Type GetARBaseForType(Type tDAO)
		{
			// find where ActiveRecordBase is in the inheritance heiracy
			int Depth = GetActiveRecordBaseDepth(tDAO);
			// get one of them 
			Type ARBase = GetTypeAtLevel(tDAO, Depth);
			// invoke Find<T> which is defined as a static on that member .. why the hell I cant invoke it
			// on the tDAO I have no idea. Do static members inherit? or do they work differently from 
			// classes with vTables and the like?
			return ARBase;
		}
		static Type GetARBase(object daoObject)
		{
			// load it up and check the properties we set are in fact what we expected them to be.
			Type tDAO = daoObject.GetType();
			return GetARBaseForType(tDAO);
		}

		static Type GetTypeAtLevel(Type t, int objLevels)
		{
			Type tmpType = t;
			while (objLevels > 0)
			{
				tmpType = tmpType.BaseType;
				objLevels--;
			}
			return (tmpType);
		}

		static int GetActiveRecordBaseDepth(Type t)
		{
			Type Base = t.BaseType;
			if (Base == typeof(Object)) return -1;// if we got this deep and didnt find AR it aint a DAO
			int objLevels = 0;
			while (Base != typeof(ActiveRecordBase))
			{
				Base = Base.BaseType;
				if (Base == typeof(Object)) return -1;// if we got this deep and didnt find AR it aint a DAO
				objLevels++;
			}
			return objLevels;
		}

		static int Get2AM_DBBaseDepth(Type t)
		{
			// DAO : 2AM_DB<T> : DB_AuditableBase<T> : DB_Base<T> : ActiveRecordBase<T> : ActiveRecordBase so -4 from where
			// we find ActiveRecordBase will be the depth.
			Stack<string> Heiracy = new Stack<string>();
			Type Base = t.BaseType;
			if (Base == typeof(Object)) return -1;// if we got this deep and didnt find AR it aint a DAO
			int objLevels = 0;
			while (Base != typeof(ActiveRecordBase))
			{
				Heiracy.Push(Base.Name);
				Base = Base.BaseType;
				if (Base == typeof(Object)) return -1;// if we got this deep and didnt find AR it aint a DAO
				objLevels++;
			}
			return objLevels - 4;
		}

		static int GetObjectDepth(Type t)
		{
			Type Base = t.BaseType;
			int objLevels = 0;
			while (Base != typeof(ActiveRecordBase))
			{
				Base = Base.BaseType;
				objLevels++;
			}
			return objLevels;
		}
		#endregion

		#region RandomType Generators
		static double GetRandomDouble()
		{
			Random r = new Random();
			return r.NextDouble();
		}

		static int GetRandomInt()
		{
			Random r = new Random();
			return r.Next(1, Int32.MaxValue);
		}

		static Int64 GetRandomInt64()
		{
			Random r = new Random();
			return (Int64)r.Next(1, Int32.MaxValue);
		}

		static Int16 GetRandomInt16()
		{
			Random r = new Random();
			return (Int16)r.Next(1, Int16.MaxValue);
		}

		static bool GetRandomBool()
		{
			Random r = new Random();
			int i = r.Next(1, 100);
			if (i % 2 == 0)
				return true;
			return false;
		}

		#endregion

		private static int HasAttributeApplied(object[] Attributes, string AttributeName)
		{
			for (int i = 0; i < Attributes.Length; i++)
			{
				if (Attributes[i].GetType().Name == AttributeName)
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Internal class used for timings of LoadSaveLoad calls.
		/// </summary>
		private class TimingObject
		{
			private DateTime _start;
			private DateTime _end;
			private object _obj;
			private double _totalSeconds;
			private TestType _testType;

			public TimingObject(DateTime start, DateTime end, object obj, TestType testType)
			{
				_start = start;
				_end = end;
				_obj = obj;
				_testType = testType;

				// calculate total seconds
				TimeSpan ts = _end - _start;
				_totalSeconds = ts.TotalSeconds;
			}

			public double TotalSeconds
			{
				get
				{
					return _totalSeconds;
				}
			}

			public string Description
			{
				get
				{
					return _obj.GetType().FullName;
				}
			}

			public TestType TestType
			{
				get
				{
					return _testType;
				}
			}
		}
	}
}
