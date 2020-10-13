using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNet.Scaffolding.Core.Metadata;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace UtilityClass
{
    public class ScaffoldHelpers
    {
        public static IEnumerable<PropertyMetadata> GetPropertiesInDisplayOrder(string typename, IEnumerable<PropertyMetadata> properties)
        {

            return properties;

            Type type = Type.GetType($"{typename}, {typename.Split('.')[0]}");
            SortedList<string, PropertyMetadata> propertiesList = new SortedList<string, PropertyMetadata>();

            FileStream fs = new FileStream("c:\\cstest\\Test.txt", FileMode.Append);
            TextWriter tmp = Console.Out;
            StreamWriter sw = new StreamWriter(fs);
            Console.SetOut(sw);
            if (type != null)
            {
                Console.WriteLine("Sort type is not null");
            }
            else
            {
                Console.WriteLine("Sort type is null");
            }

            int order = 0;
            foreach (PropertyMetadata property in properties)
            {

                if (type != null)
                {
          

                    var member = type.GetMember(property.PropertyName)[0];
                 
                    var displayAttribute = member.GetCustomAttribute<System.ComponentModel.DataAnnotations.DisplayAttribute>();
                    
                    if (displayAttribute != null)
                    {
                        order = displayAttribute.Order;
                    } else
                    {
                        order += 1;
                    }


                    if (displayAttribute == null) { Console.WriteLine("No display attribute"); };
                }
                propertiesList.Add($"{order:000} - {property.PropertyName}", property);
                if (type == null) { Console.WriteLine("No Type"); };

                if (order == 0)
                {
                    Console.WriteLine($"{order:000} - {property.PropertyName}");
                }
                else
                {
                    Console.WriteLine($"{order} - {property.PropertyName}");
                }
            }

            sw.Close();
            return propertiesList.Values.AsEnumerable();
        }

        public static bool IsNullable(string typename, PropertyMetadata property)
        {
            bool isNullable = false;

            Type typeModel = Type.GetType($"{typename}, {typename.Split('.')[0]}");

            //FileStream fs = new FileStream("c:\\cstest\\Test.txt", FileMode.Append);
            //TextWriter tmp = Console.Out;
            //StreamWriter sw = new StreamWriter(fs);
            //Console.SetOut(sw);

            //Console.WriteLine(typename);
            //Console.WriteLine($"{typename}, {typename.Split('.')[0]}");
            //Console.WriteLine(property.PropertyName);
            if (typeModel != null)
            {
                //Console.WriteLine("Not First Null");
                PropertyInfo pi = typeModel.GetProperty(property.PropertyName);
                Type type = pi.GetType();
                //Console.WriteLine("Not Null");

                isNullable = ((pi.PropertyType.IsValueType) && (Nullable.GetUnderlyingType(pi.PropertyType) != null)) ;
                
                //isNullable = (typeModel.IsGenericType && typeModel.GetGenericTypeDefinition() == typeof(Nullable<>));
                //if (typeModel.IsGenericType) { Console.WriteLine(" Is Generic Type"); };
                //if (type.IsGenericType) { Console.WriteLine(" Is Generic Type1"); };
                //Console.WriteLine("Property Name : " + pi.Name);
                //if ((pi.PropertyType.IsValueType))

                //{
                //    Console.WriteLine("is value type");
                //};
                //if ((Nullable.GetUnderlyingType(pi.PropertyType) != null))
                //{
                //    Console.WriteLine("is nullable");
                //};
                //Console.WriteLine("Type Name : " + type.Name);
                //Console.WriteLine("Type Name : " + typeModel.Name);
                //if (type == typeof(Nullable<>)) { Console.WriteLine(" Is Nullable DateTime"); };
                //if (typeModel.GetGenericTypeDefinition() == typeof(Nullable<DateTime>)) { Console.WriteLine(" Is Generic Type Nullable"); };
                //if (isNullable)
                //{
                //    Console.WriteLine(property.PropertyName + " Is Nullable");

                //}
                //else
                //{
                //    Console.WriteLine(property.PropertyName + " Is not Nullable");
                //}
            }
            else
            {
                //Console.WriteLine("Type is null");
            }
            //sw.Close();
            return isNullable;
        }


    }
}
