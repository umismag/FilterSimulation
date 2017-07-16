using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtering
{
	class Washing : Parameter
	{
		WashingLiquid liquid;
		public WashingLiquid Liquid
		{
			get { return liquid; }
			set { liquid = value; }
		}

		Max_wash_out max_wash_out;
		public Max_wash_out Max_wash_out
		{
			get { return max_wash_out; }
			set { max_wash_out = value; }
		}

		Min_wash_out min_wash_out;
		public Min_wash_out Min_wash_out
		{
			get { return min_wash_out; }
			set { min_wash_out = value; }
		}

		Adaptation_ParameterA adaptation_ParameterA;
		public Adaptation_ParameterA Adaptation_ParameterA
		{
			get { return adaptation_ParameterA; }
			set { adaptation_ParameterA = value; }
		}

		Adaptation_ParameterB adaptation_ParameterB;
		public Adaptation_ParameterB Adaptation_ParameterB
		{
			get { return adaptation_ParameterB; }
			set { adaptation_ParameterB = value; }
		}

		public Washing(string name, WashingLiquid liquid, Max_wash_out max_wash_out, Min_wash_out min_wash_out, Adaptation_ParameterA adaptation_ParameterA, Adaptation_ParameterB adaptation_ParameterB)
		{
			Name = name;
			Liquid = liquid;
			Max_wash_out = max_wash_out;
			Min_wash_out = min_wash_out;
			Adaptation_ParameterA = adaptation_ParameterA;
			Adaptation_ParameterB = adaptation_ParameterB;
		}
	}

	class WashingRatio:Parameter
	{
		public WashingRatio()
		{
			Name = "Washing Ratio";
			Unit = "-";
			Symbol = "w";
		}

		public WashingRatio(double? value):this()
		{
			Value = value;
		}
	}

	class Volume:Parameter
	{
		public Volume()
		{
			Name = "Volume";
			Unit = "l";
			Symbol = "V";
		}

		public Volume(double? value):this()
		{
			Value = value;
		}
	}

	class Viscosity : Parameter
	{
		public Viscosity()
		{
			Name = "Viscosity";
			Unit = "mPa*s";
			Symbol = "eta";
		}

		public Viscosity(double? Value) : this()
		{
			this.Value = Value;
		}
	}

	class Density : Parameter
	{
		public Density()
		{
			Name = "Density";
			Unit = "kg/m3";
			Symbol = "rho";
		}

		public Density(double? Value) : this()
		{
			this.Value = Value;
		}
	}

	class WashingLiquid : Parameter
	{
		Viscosity viscosity;
		public Viscosity Viscosity
		{
			get { return viscosity; }
			set { viscosity = value; }
		}

		Density density;
		public Density Density
		{
			get { return density; }
			set { density = value; }
		}

		Volume volume;
		public Volume Volume
		{
			get { return volume; }
			set { volume = value; }
		}

		public WashingLiquid(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			Viscosity = viscosity;
			Density = density;			
		}

		public WashingLiquid(string name, Viscosity viscosity, Density density, Volume volume):this(name,viscosity,density)
		{
			volume.SymbolSuffix = "w";
			Volume = volume;
		}

		public override string ToString()
		{
			return Name;
		}
	}

	class Max_wash_out:Parameter
	{
		public Max_wash_out(double? value=100)
		{
			Value = value;
			Name = "Max_wash_out";
			Unit = "-";
			Symbol = "X0";
		}
	}

	class Min_wash_out : Parameter
	{
		public Min_wash_out(double? value=0)
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
		public Adaptation_ParameterA(double? value=5)
		{
			Value = value;
			Name = "Adaptation Parameter A";
			Unit = "-";
			Symbol = "Aw";
		}
	}

	class Adaptation_ParameterB : Parameter
	{
		public Adaptation_ParameterB(double? value=1)
		{
			Value = value;
			Name = "Adaptation Parameter B";
			Unit = "-";
			Symbol = "Bw";
		}
	}

	
}
