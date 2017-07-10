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
			SubParameters = new Dictionary<Type, Parameter>();
			SubParameters.Add(liquid.GetType(), liquid);
			SubParameters.Add(max_wash_out.GetType(), max_wash_out);
			SubParameters.Add(min_wash_out.GetType(),min_wash_out);
			SubParameters.Add(adaptation_ParameterA.GetType(), adaptation_ParameterA);
			SubParameters.Add(adaptation_ParameterB.GetType(),adaptation_ParameterB);
			
		}

		public Volume GetWashLiquidVolume()
		{
			WashingLiquid wl = new WashingLiquid("tmpLiquid", new Viscosity(),new Density(),new Volume());
			Volume vl = new Volume();
			wl.SubParameters[vl.GetType()] = vl;
			return null;
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

		public WashingLiquid(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			SubParameters = new Dictionary<Type, Parameter>();
			SubParameters.Add(viscosity.GetType(), viscosity);
			SubParameters.Add(density.GetType(), density);
		}

		public WashingLiquid(string name, Viscosity viscosity, Density density, Volume volume):this(name,viscosity,density)
		{
			volume.SymbolSuffix = "w";
			SubParameters.Add(volume.GetType(),volume);
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
