using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace Filtering
{
	public class Washing : Parameter
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

	public class WashingRatio : Parameter
	{
		public WashingRatio()
		{
			Name = "Washing Ratio";
			Unit = "-";
			Symbol = "w";
			converter = new Param2DoubleConverter<WashingRatio>();
		}

		public WashingRatio(double? value) : this()
		{
			Value = value;
		}

		new public double? Value
		{
			get { return value; }
			set
			{
				this.value = value;
				//OnPropertyChanged("WashingRatio");
			}
		}

	}

	public class Volume :Parameter
	{
		public Volume()
		{
			Name = "Volume";
			Unit = "l";
			Symbol = "V";
			converter = new Param2DoubleConverter<Volume>();
		}

		public Volume(double? value):this()
		{
			Value = value;
		}
	}

	public class Viscosity : Parameter
	{
		public Viscosity()
		{
			Name = "Viscosity";
			Unit = "mPa*s";
			Symbol = "eta";
			converter = new Param2DoubleConverter<Viscosity>();
		}

		public Viscosity(double? Value) : this()
		{
			this.Value = Value;
		}
	}

	public class Density : Parameter
	{
		public Density()
		{
			Name = "Density";
			Unit = "kg/m3";
			Symbol = "rho";
			converter = new Param2DoubleConverter<Density>();
		}

		public Density(double? Value) : this()
		{
			this.Value = Value;
		}
	}

	public class WashingLiquid : Parameter
	{
		Viscosity viscosity;
		public Viscosity Viscosity
		{
			get { return viscosity; }
			set
			{
				viscosity = value;
				OnPropertyChanged("Viscosity");
			}
		}

		Density density;
		public Density Density
		{
			get { return density; }
			set
			{
				density = value;
				OnPropertyChanged("Density");
			}
		}

		Volume volume;
		public Volume Volume
		{
			get { return volume; }
			set
			{
				volume = value;
				OnPropertyChanged("Volume");
			}
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

	public class Max_wash_out :Parameter
	{
		public Max_wash_out()
		{
			Name = "Max_wash_out";
			Unit = "-";
			Symbol = "X0";
			converter = new Param2DoubleConverter<Max_wash_out>();
		}

		public Max_wash_out(double? value = 100):this()
		{
			Value = value;
		}
	}

	public class Min_wash_out : Parameter
	{
		public Min_wash_out()
		{
			Name = "Min_wash_out";
			Unit = "-";
			Symbol = "Xr";
			converter = new Param2DoubleConverter<Min_wash_out>();
		}

		public Min_wash_out(double? value = 0) : this()
		{
			Value = value;
		}
	}

	public class Washing_Index : Parameter
	{
		public Washing_Index()
		{
			Name = "Washing Index";
			Unit = "-";
			Symbol = "Dn";
			converter = new Param2DoubleConverter<Washing_Index>();
		}

		public Washing_Index(double? value):this()
		{
			Value = value;
		}
	}

	public class Adaptation_ParameterA : Parameter
	{
		public Adaptation_ParameterA()
		{
			Name = "Adaptation Parameter A";
			Unit = "-";
			Symbol = "Aw";
			converter = new Param2DoubleConverter<Adaptation_ParameterA>();
		}

		public Adaptation_ParameterA(double? value = 5):this()
		{
			Value = value;
		}
	}

	public class Adaptation_ParameterB : Parameter
	{
		public Adaptation_ParameterB()
		{
			Name = "Adaptation Parameter B";
			Unit = "-";
			Symbol = "Bw";
			converter = new Param2DoubleConverter<Adaptation_ParameterB>();
		}

		public Adaptation_ParameterB(double? value = 1):this()
		{
			Value = value;
		}
	}

	
}
