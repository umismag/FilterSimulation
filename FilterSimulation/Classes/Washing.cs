using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilterSimulation.Classes
{
	class Washing : Parameter
	{
		public Washing(string name, WashingLiquid liquid, Max_wash_out max_wash_out, Min_wash_out min_wash_out, Adaptation_ParameterA adaptation_ParameterA, Adaptation_ParameterB adaptation_ParameterB)
		{
			Name = name;
			SubParameters = new Parameter[] { liquid, max_wash_out, min_wash_out, adaptation_ParameterA, adaptation_ParameterB };
		}
	}
	
	class Max_wash_out:Parameter
	{
		public Max_wash_out(double? value)
		{
			Value = value;
			Name = "Max_wash_out";
			Unit = "-";
			Symbol = "X0";
		}
	}

	class Min_wash_out : Parameter
	{
		public Min_wash_out(double? value)
		{
			Value = value;
			Name = "Min_wash_out";
			Unit = "-";
			Symbol = "Xr";
		}
	}

	class Washing_Index : Parameter
	{
		public Washing_Index(double? value)
		{
			Value = value;
			Name = "Washing Index";
			Unit = "-";
			Symbol = "Dn";
		}
	}

	class Adaptation_ParameterA : Parameter
	{
		public Adaptation_ParameterA(double? value)
		{
			Value = value;
			Name = "Adaptation Parameter A";
			Unit = "-";
			Symbol = "Aw";
		}
	}

	class Adaptation_ParameterB : Parameter
	{
		public Adaptation_ParameterB(double? value)
		{
			Value = value;
			Name = "Adaptation Parameter B";
			Unit = "-";
			Symbol = "Bw";
		}
	}

	
}
