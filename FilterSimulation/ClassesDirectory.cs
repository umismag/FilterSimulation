using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Collections;

namespace FilterSimulation.Classes
{
	public interface IParameterDefinition
	{
		string Name { get; }
		string Unit { get; }
		string Symbol { get; }	
	}

	public interface IParameter
	{
		IParameterDefinition Parameter { get; }			
		string Value { get; set; }
	}

	public abstract class Parameter : IParameterDefinition
	{
		
		internal string name;
		public string Name
		{
			get { return name; }
			internal set { name = value; }
		}

		string unit;
		public string Unit
		{
			get { return unit; }
			internal set { unit = value; }
		}

		string symbol;
		public string Symbol
		{
			get { return symbol+SymbolSuffix; }
			internal set { symbol = value; }
		}

		string symbolSuffix = string.Empty;
		public string SymbolSuffix
		{
			get { return symbolSuffix; }
			internal set { symbolSuffix = value; }
		}

		double minValue = double.MinValue;
		double maxValue = double.MaxValue;

		double? value=null;
		public double? Value
		{
			get { return this.value; }
			set
			{
				if (value < minValue)
					this.value = minValue;
				else if (value > maxValue)
					this.value = maxValue;
				else
					this.value = value;
			}
		}

		public override string ToString()
		{
			return Value.ToString();
		}		
	}	
	   
	public class ParametersTemplate
	{
		string parameter;
		public string Parameter
		{
			get { return parameter; }
			set { parameter = value; }
		}

		string units;
		public string Units
		{
			get { return units; }
			set { units = value; }
		}

		string value;
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}
	}

	public static class MyReflection
	{
		//public static List<ParametersTemplate> PrintParameters(object obj, Parameter parentObj)
		//{
		//	Parameter tmpObj = obj as Parameter;
		//	if (tmpObj == null) return null;

			
		//	List<ParametersTemplate> res = new List<ParametersTemplate>();

		//	if (tmpObj.SubParameters!=null)
		//	{
		//		foreach(Parameter par in tmpObj.SubParameters.Values)
		//		{
		//			res.AddRange(PrintParameters(par,tmpObj));
		//		}
				
		//	}
		//	else
		//	{
		//		if (tmpObj.Unit != string.Empty)
		//		{					
		//			res.Add(new ParametersTemplate()
		//			{
		//				Parameter = ((parentObj!=null)?parentObj.Name :"") + " "+ tmpObj.Name+" ["+ tmpObj.Symbol + "]" ,
		//				Units = tmpObj.Unit,
		//				Value = tmpObj.Value.ToString()
		//			});
		//		}
		//	}

		//	return res;
		//}


		public static List<ParametersTemplate> PrintNewLeadSet(Parameter obj, Parameter parentObject=null)
		{
			if (obj == null) return null;

			List<ParametersTemplate> res = new List<ParametersTemplate>();

			Type t = obj.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);


			foreach (PropertyInfo propinfo in pi)
			{
				Parameter subObj = propinfo.GetValue(obj) as Parameter;
				if (subObj == null) continue;

				bool isSubparameter = subObj != null && (subObj.Symbol == null || subObj.Unit == null);
				if (isSubparameter)
				{
					res.AddRange(PrintNewLeadSet(subObj, obj));
				}
				else
				{
					if (propinfo.PropertyType.Name.IndexOf("[]") > 0)
					{

						foreach (var element in propinfo.GetValue(obj, null) as Array)
						{
							if (element != null && element is Parameter)
							{
								Parameter tmppar = (Parameter)element;
								res.Add(new ParametersTemplate()
								{
									Parameter = propinfo.Name + " " + tmppar.Name,
									Units = tmppar.Unit,
									Value = tmppar.ToString()
								});
							}
							else
								break;
						}

						//res.Add(new string[] { propinfo.Name, s });
					}
					else
					{
						bool isPrintableParameter = propinfo.GetValue(obj, null) != null ;
						if (isPrintableParameter)
							res.Add(new ParametersTemplate()
							{
								Parameter = parentObject!=null?parentObject.Name:"" + " " + propinfo.Name,
								Units = (propinfo.GetValue(obj, null) as Parameter).Unit,
								Value = (propinfo.GetValue(obj, null) as Parameter).Value.ToString()
							});
						//res.Add(new string[] { propinfo.Name,propinfo.GetValue(obj, null).ToString() });
					}
				}
			}
			return res;
		}
	}
}
