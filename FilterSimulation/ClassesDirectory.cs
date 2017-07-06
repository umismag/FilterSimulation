﻿using System;
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
		Parameter[] subParameters=null;
		public Parameter[] SubParameters
		{
			get { return subParameters; }
			set { subParameters = value; }
		}


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
			get { return symbol; }
			internal set { symbol = value; }
		}

		double minValue = double.MinValue;
		double maxValue = double.MaxValue;

		double value;
		public double Value
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


	class Viscosity:Parameter
	{						
		public Viscosity()
		{
			Name = "Viscosity";
			Unit = "mPa*s";
			Symbol = "eta";
		}

		public Viscosity(double Value):this()
		{
			this.Value = Value;
		}
	}

	class Density:Parameter
	{
		public Density()
		{
			Name = "Density";
			Unit = "kg/m3";
			Symbol = "rho";
		}

		public Density(double Value):this()
		{
			this.Value = Value;
		}
	}

	class Liquid:Parameter
	{
		
		public Liquid(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			SubParameters = new Parameter[] { viscosity, density };
		}

		public override string ToString()
		{
			return Name;
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
		public static List<ParametersTemplate> PrintParameters(object obj, Parameter parentObj)
		{
			Parameter tmpobj = obj as Parameter;
			if (tmpobj == null) return null;

			
			List<ParametersTemplate> res = new List<ParametersTemplate>();

			if (tmpobj.SubParameters!=null)
			{
				foreach(Parameter par in tmpobj.SubParameters)
				{
					res.AddRange(PrintParameters(par,tmpobj));
				}
				
			}
			else
			{
				if (tmpobj.Unit!=string.Empty)
					res.Add(new ParametersTemplate()
					{
						Parameter =(parentObj.Name ?? "") + " " + tmpobj.Name,
						Units = tmpobj.Unit,
						Value = tmpobj.Value.ToString()
					});
			}

			return res;
		}


		public static List<ParametersTemplate> PrintNewLeadSet(object obj)
		{
			if (obj == null) return null;

			List<ParametersTemplate> res = new List<ParametersTemplate>();

			Type t = obj.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propinfo in pi)
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
					if (propinfo.GetValue(obj, null) !=null)
					res.Add(new ParametersTemplate()
					{
						Parameter =t.Name+" "+ propinfo.Name,
						Units = "",
						Value = propinfo.GetValue(obj, null).ToString()
					});
					//res.Add(new string[] { propinfo.Name,propinfo.GetValue(obj, null).ToString() });
				}
			}

			return res;
		}
	}
}
