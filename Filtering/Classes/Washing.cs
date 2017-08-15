using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;
using System.ComponentModel;

namespace Filtering
{
	public class Washing : Parameter, IWashingProcess
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

		CakeHeightForCakeWashing cakeHeightForCakeWashing = new CakeHeightForCakeWashing();
		public CakeHeightForCakeWashing CakeHeightForCakeWashing
		{
			get { return cakeHeightForCakeWashing; }
			set
			{
				cakeHeightForCakeWashing = value;
				OnPropertyChanged("CakeHeightForCakeWashing");
			}
		}

		PressureDifferenceCakeWashing pressureDifferenceCakeWashing=new PressureDifferenceCakeWashing();
		public PressureDifferenceCakeWashing PressureDifferenceCakeWashing
		{
			get { return pressureDifferenceCakeWashing; }
			set
			{
				pressureDifferenceCakeWashing = value;
				OnPropertyChanged("PressureDifferenceCakeWashing");
			}
		}

		WashingTime washingTime=new WashingTime();
		public WashingTime WashingTime
		{
			get { return washingTime; }
			set
			{
				washingTime = value;
				OnPropertyChanged("WashingTime");
			}
		}

		WashingRatio washingRatio=new WashingRatio();
		public WashingRatio WashingRatio
		{
			get
			{
				return washingRatio;
			}
			set
			{
				washingRatio = value;
				OnPropertyChanged("WashingRatio");
			}
		}

		WashLiquidVolume washLiquidVolume=new WashLiquidVolume();
		public WashLiquidVolume WashLiquidVolume
		{
			get { return washLiquidVolume; }
			set
			{
				washLiquidVolume = value;
				OnPropertyChanged("WashLiquidVolume");
			}
		}

		CakeFormation cakeFormation;
		public CakeFormation CakeFormation
		{
			get => cakeFormation;
			set => cakeFormation=value;
		}

		Washing IWashingProcess.Washing
		{
			get => this;
			set { }
		}

		void WashLiquidVolumeDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "WashingRatio, Area, SpecificCakeVolume, Porosity";
			if (IsNeedToUpdate(dependentParameters, prop, WashLiquidVolume, GetWashLiquidVolume, sender))
			{
					WashLiquidVolume = GetWashLiquidVolume();
			}
			else
				return;
		}

		public WashLiquidVolume GetWashLiquidVolume()
		{
			double? res;

			res = CakeFormation.Cake.Porosity.Value * CakeFormation.Filter.Area.Value * CakeFormation.SpecificCakeVolume.Value * WashingRatio.Value / 100;
			WashLiquidVolume vl = new WashLiquidVolume(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
			return vl;
		}

		public Washing(ICakeFormationProcess cakeFormationProcess)
		{
			CakeFormation = cakeFormationProcess.CakeFormation;
			
			PropertyChangedStatic += WashLiquidVolumeDependentParametersChanged;
		}

		public Washing(string name, WashingLiquid liquid, Max_wash_out max_wash_out, Min_wash_out min_wash_out, Adaptation_ParameterA adaptation_ParameterA, Adaptation_ParameterB adaptation_ParameterB, PressureDifferenceCakeWashing pressureDifferenceCakeWashing, ICakeFormationProcess cakeFormationProcess) :this(cakeFormationProcess)
		{
			Name = name;
			Liquid = liquid;
			Max_wash_out = max_wash_out;
			Min_wash_out = min_wash_out;
			Adaptation_ParameterA = adaptation_ParameterA;
			Adaptation_ParameterB = adaptation_ParameterB;
			PressureDifferenceCakeWashing = pressureDifferenceCakeWashing;
		}
	}

	public class CakeHeightForCakeWashing:Height
	{
		public CakeHeightForCakeWashing()
		{
			Name = "Cake Height for Cake Washing";
			SymbolSuffix = "w";
			converter = new Param2DoubleConverter<CakeHeightForCakeWashing>();
		}

		public CakeHeightForCakeWashing(double? value):this()
		{
			Value = value;
		}
	}

	public class WashingTime : Time
	{
		public WashingTime()
		{
			Name = "Washing Time";
			SymbolSuffix = "w";
			converter = new Param2DoubleConverter<WashingTime>();
		}

		public WashingTime(double? value) : this()
		{
			Value = value;
		}
	}

	public class PressureDifferenceCakeWashing: PressureDifferenceCakeFormation
	{
		public PressureDifferenceCakeWashing()
		{
			Name = "Pressure Difference Cake Washing";
			SymbolSuffix = "w";
			converter = new Param2DoubleConverter<PressureDifferenceCakeWashing>();
		}

		public PressureDifferenceCakeWashing(double? value):this()
		{
			Value = value;
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

	public class WashLiquidVolume:Volume
	{
		public WashLiquidVolume()
		{
			Name = "Wash liquid volume";
			SymbolSuffix = "w";
			converter = new Param2DoubleConverter<WashLiquidVolume>();
		}

		public WashLiquidVolume(double? value):this()
		{
			Value = value;
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
