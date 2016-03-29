using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SAHL.DomainProcessManager.DomainProcesses.Utilities
{
    public class DomainModelMapper
    {
        private Type source;
        private Type destination;

        public void CreateMap<TSourceType, TDestinationType>()
        {
            this.source = typeof(TSourceType);
            this.destination = typeof(TDestinationType);
        }

        public dynamic Map(Object sourceObject)
        {
            var method = typeof(DomainModelMapper).GetMethod("MapperThroughConstructorArgs");

            MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { source, destination });
            return genericMethod.Invoke(this, new Object[] { sourceObject });
        }

        public TDestination MapperThroughConstructorArgs<TSource, TDestination>(TSource sourceObject)
            where TSource : class
            where TDestination : class
        {
            var constructors = typeof(TDestination).GetConstructors();

            if (constructors.Count() > 1)
            {
                var customException = new Exception("Destination object is not expected to have more than one constructor!");
                throw customException;
            }

            var constructorParams = new List<dynamic>();
            var propertiesCollection = sourceObject.GetType().GetProperties();
            foreach (var paramInfo in constructors[0].GetParameters())
            {
                PropertyInfo propInfo = propertiesCollection.SingleOrDefault(p => p.Name.Equals(paramInfo.Name, StringComparison.InvariantCultureIgnoreCase));
                if (propInfo == null)
                {
                    throw new FormatException(String.Format("A property named {0} does not exist on the source type {1} for mapping to type {2}.", paramInfo.Name, 
                        typeof(TSource).FullName, typeof(TDestination).FullName));
                }
                var propValue = GetPropValue(sourceObject, propInfo.Name);

                if (propValue == null)
                {
                    constructorParams.Add(propValue);
                    continue;
                }

                Type sourceType = propValue.GetType();
                Type destinationType = paramInfo.ParameterType;
                if (sourceType.IsAssignableFrom(destinationType) || destinationType.IsAssignableFrom(sourceType))
                {
                    constructorParams.Add(propValue);
                }
                else if (typeof(IEnumerable).IsAssignableFrom(paramInfo.ParameterType))
                {
                    Type destinationItemType = destinationType.GetGenericArguments()[0];
                    var listType = typeof(List<>);
                    var genericListType = listType.MakeGenericType(destinationItemType);
                    dynamic collection = Activator.CreateInstance(genericListType);

                    foreach (var element in propValue)
                    {
                        var mapper = GetGenericMappper(destinationItemType, element);

                        collection.Add(mapper.Map(element));
                    }
                    constructorParams.Add(collection);
                }
                else
                {
                    if (destinationType.IsEnum)
                    {
                        // Domain has SalariedRenumerationTypeEnum and SalariedWithDeductionRenumerationTypeEnum which are both meant to 
                        // be mapped to or from RenumerationTypeEnum.
                        var sourceUnderlyingEnumValue = Convert.ChangeType(propValue, Enum.GetUnderlyingType(propValue.GetType()));
                        var destinationEnum = Enum.ToObject(destinationType, ((int)sourceUnderlyingEnumValue));
                        constructorParams.Add(destinationEnum);
                    }
                    else
                    {
                        var mapper = GetGenericMappper(destinationType, propValue);
                        constructorParams.Add(mapper.Map(propValue));
                    }
                }
            }

            Object[] args = constructorParams.ToArray();

            return (TDestination)Activator.CreateInstance(typeof(TDestination), args);
        }

        private static DomainModelMapper GetGenericMappper(Type destinationItemType, dynamic sourceObject)
        {
            var mapper = new DomainModelMapper();
            // Generically create map
            var genericCreateMap = typeof(DomainModelMapper).GetMethod("CreateMap");
            var specificCreatemap = genericCreateMap.MakeGenericMethod(new Type[] { sourceObject.GetType(), destinationItemType });
            specificCreatemap.Invoke(mapper, new object[0]);
            return mapper;
        }

        public dynamic GetPropValue(object src, string propName)
        {
            var propertyInfo = src.GetType().GetProperty(propName);
            var value = propertyInfo.GetValue(src, null);            

            dynamic safeValue = (value == null) ? null
                                                : value;

            return safeValue;
        }
    }
}