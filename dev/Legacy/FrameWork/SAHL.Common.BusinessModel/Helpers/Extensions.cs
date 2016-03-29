using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SAHL.Common.Collections.Interfaces;
using SAHL.Common.Factories;
using SAHL.Common.Interfaces;
using SAHL.Common.BusinessModel.DAO.Database;
using System.Linq.Expressions;
using System.ComponentModel;

namespace SAHL.Common.BusinessModel.Helpers
{
	/// <summary>
	/// Extensions
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Get Previous Value
		/// </summary>
		/// <typeparam name="InterfaceType">The Business Model Interface that you are passing in/></typeparam>
		/// <typeparam name="ExpectedType">The Expected Type of the Value that you are requesting</typeparam>
		/// <param name="businessModelObject"></param>
		/// <param name="property"></param>
		/// <returns></returns>
		public static ExpectedType GetPreviousValue<InterfaceType, ExpectedType>(this IBusinessModelObject businessModelObject, Expression<Func<InterfaceType, ExpectedType>> property)
		{
			if (property == null)
			{
				return default(ExpectedType);
			}
			//Retrieve the property's name and type
			string propertyName = (((System.Linq.Expressions.MemberExpression)(((System.Linq.Expressions.LambdaExpression)(property)).Body)).Member).Name;

			//Business Model Type Mapper
			IBusinessModelTypeMapper businessModelTypeMapper = TypeFactory.CreateType<IBusinessModelTypeMapper>();
			IDAOObject daoObject = businessModelObject as IDAOObject;

			//Get the Real DAO Object
			object realDAOObject = daoObject.GetDAOObject();
			Type daoType = realDAOObject.GetType();

			//Get the Property Value
			object propertyValue = daoType.GetMethod("GetPreviousValue").Invoke(realDAOObject, new object[] { propertyName });

			if (propertyValue != null && typeof(ExpectedType).GetInterface(typeof(IBusinessModelObject).Name) != null)
			{
				daoType = propertyValue.GetType();
				ExpectedType type = businessModelTypeMapper.GetMappedType<ExpectedType>(propertyValue);
				return type;
			}

			//If the type is nullable
			NullableConverter nullableConverter = null;
			if(typeof(ExpectedType).IsGenericType && typeof(ExpectedType).GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			{
				nullableConverter = new NullableConverter(typeof(ExpectedType));
			}

			if (propertyValue != null && (propertyValue.GetType() == typeof(ExpectedType) || (nullableConverter != null && nullableConverter.UnderlyingType == propertyValue.GetType())))
			{
				return (ExpectedType)propertyValue;
			}
			return default(ExpectedType);
		}
	}
}
