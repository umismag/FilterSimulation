using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtering
{
	class CakeFormation:Parameter,System.ComponentModel.INotifyPropertyChanged
	{
		Suspension suspension;
		public Suspension Suspension
		{
			get { return suspension; }
			set { suspension = value; }
		}

		Filter filter;
		public Filter Filter
		{
			get { return filter; }
			set { filter = value; }
		}

		Cake cake;
		public Cake Cake
		{
			get { return cake; }
			set { cake = value; }
		}

		SpecificCakeVolume specificCakeVolume;
		public SpecificCakeVolume SpecificCakeVolume
		{
			get
			{
				if (specificCakeVolume==null)
				{
					double? res = Cake.Height.Value * (1 - Cake.Height.Value/Filter.MachineDiameter.Value);
					return new SpecificCakeVolume(res);
				}
				return specificCakeVolume;
			}
			set { specificCakeVolume = value; }
		}

		WashingRatio washingRatio;
		public WashingRatio WashingRatio
		{
			get
			{
				return washingRatio;
			}
			set
			{
				washingRatio = value;
				WashLiquidVolume = GetWashLiquidVolume(WashingRatio.Value);
				//PropertyChanged(this, new PropertyChangedEventArgs("WashingRatio"));
			}
		}

		Volume washLiquidvolume;
		public Volume WashLiquidVolume
		{
			get
			{
				return GetWashLiquidVolume(WashingRatio.Value);
			}
			set
			{
				washLiquidvolume = value;
				//PropertyChanged(this, new PropertyChangedEventArgs("WashLiquidVolume"));
			}
		}

		public CakeFormation(string name, Suspension suspension, Filter filter, Cake cake, WashingRatio washingRatio)
		{
			Name = name;
			Suspension = suspension;
			Filter = filter;
			Cake = cake;
			WashingRatio = washingRatio;
		}

		

		public event PropertyChangedEventHandler PropertyChanged;
		//	= new PropertyChangedEventHandler((x,y)=> 
		//{
		//	if(y.PropertyName== "WashingRatio")
		//	{
				
		//	}
		//});

		

		public Volume GetWashLiquidVolume(double? washingRatio=1)
		{
			if (washingRatio == null) washingRatio = 3;
			WashingLiquid wl = new WashingLiquid("tmpLiquid", new Viscosity(), new Density(), new Volume());

			double? res;

			res = Cake.Porosity.Value * Filter.Area.Value * SpecificCakeVolume.Value * washingRatio/100;
			Volume vl = new Volume(res);
			return vl;
		}
	}

	class SpecificCakeVolume:Parameter
	{
		public SpecificCakeVolume()
		{
			Name = "Specific Cake Volume";
			Unit = "l/m2";
			Symbol = "vc";
		}

		public SpecificCakeVolume(double? value):this()
		{
			Value = value;
		}
	}

	class Suspension:Parameter
	{
		Filtrate filtrate;
		public Filtrate Filtrate
		{
			get { return filtrate; }
			set { filtrate = value; }
		}

		Density solidDensity;
		public Density SolidDensity
		{
			get { return solidDensity; }
			set { solidDensity = value; }
		}

		SolidConcentration solidConcentration;
		public SolidConcentration SolidConcentration
		{
			get { return solidConcentration; }
			set { solidConcentration = value; }
		}

		Compressibility compressibility;
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set { compressibility = value; }
		}

		public Suspension(string name, Filtrate filtrate, Density solidDensity, SolidConcentration solidConcentration, Compressibility compressibility)
		{
			Name = name;
			solidDensity.SymbolSuffix = " s";

			Filtrate = filtrate;
			SolidDensity = solidDensity;
			SolidConcentration = solidConcentration;
			Compressibility = compressibility;
		}
	}
	
	class Filter:Parameter
	{
		Resistance mediumResistance;
		public Resistance MediumResistance
		{
			get { return mediumResistance; }
			set { mediumResistance = value; }
		}

		Area area;
		public Area Area
		{
			get
			{
				if (area==null)
				{
					double? res = Math.PI * machineDiameter.Value * machineWidth.Value / 1e6;
					return new Area(res);
				}
				else 
					return area;
			}
			set
			{
				area = value;
			}
		}

		MachineDiameter machineDiameter;
		public MachineDiameter MachineDiameter
		{
			get { return machineDiameter; }
			set { machineDiameter = value; }
		}

		MachineWidth machineWidth;
		public MachineWidth MachineWidth
		{
			get { return machineWidth; }
			set { machineWidth = value; }
		}

		public Filter(string name, Resistance filterMediumResistance)
		{
			Name = name;
			MediumResistance = filterMediumResistance;
		}

		public Filter(string name, Resistance filterMediumResistance, MachineDiameter machineDiameter, MachineWidth machineWidth):this(name,filterMediumResistance)
		{
			MachineDiameter = machineDiameter;
			MachineWidth = machineWidth;
		}
	}

	class MachineDiameter:Parameter
	{
		public MachineDiameter()
		{
			Name = "Machine Diameter";
			Unit = "mm";
			Symbol = "D";
		}

		public MachineDiameter(double? value):this()
		{
			if (value != null)
				Value = value;
			else
				return;
		}
	}

	class MachineWidth:Parameter
	{
		public MachineWidth()
		{
			Name = "Machine Width";
			Unit = "mm";
			Symbol = "b";
		}

		public MachineWidth(double? value):this()
		{
			Value = value;
		}
	}

	class Area: Parameter
	{
		public Area()
		{
			Name = "Area";
			Unit = "m2";
			Symbol = "A";
		}

		public Area(double? value) : this()
		{
			Value = value;
		}
	}

	class Cake : Parameter
	{
		Porosity porosity;
		public Porosity Porosity
		{
			get { return porosity; }
			set { porosity = value; }
		}

		Permeability permeability;
		public Permeability Permeability
		{
			get { return permeability; }
			set { permeability = value; }
		}

		Compressibility compressibility;
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set { compressibility = value; }
		}

		Height height;
		public Height Height
		{
			get
			{
				return height;
			}
			set { height = value; }
		}

		public Cake(string name, Porosity porosity, Permeability permeability, Compressibility compressibility)
		{
			Name = name;
			if(permeability!=null) permeability.SymbolSuffix = "c0";
			Porosity = porosity;
			Permeability = permeability;
			Compressibility = compressibility;
		}

		public Cake(string name, Porosity porosity, Permeability permeability, Compressibility compressibility, Height height):this(name,porosity,permeability,compressibility)
		{
			Height = height;
		}
	}

	class Height:Parameter
	{
		public Height()
		{
			Name = "Height";
			Unit = "mm";
			Symbol = "hc";
		}

		public Height(double? value)
		{
			Value = value;
		}
	}

	class Resistance :Parameter
	{
		public Resistance()
		{
			Name = "Resistance";
			Unit = "mm";
			Symbol = "hce";
		}
		public Resistance(double? value):this()
		{
			Value = value;
		}
	}

	class Compressibility:Parameter
	{
		public Compressibility(double? value)
		{
			Value = value;
			Name = "Compressibility";
			Unit = "-";
			Symbol = "nc";
		}
	}

	class Permeability:Parameter
	{
		public Permeability(double? value)
		{
			Value = value;
			Name = "Permeability";
			Unit = "*E-13 m2";
			Symbol = "P";
		}
	}

	class Porosity:Parameter
	{
		public Porosity(double? value)
		{
			Value = value;
			Name = "Porosity";
			Unit = "%";
			Symbol = "E";
		}
	}

	class SolidConcentration:Parameter
	{
		public SolidConcentration(double? value)
		{
			Value = value;
			Name = "Solid Concentration";
			Unit = "%";
			Symbol = "Cm";
		}
	}

	class Filtrate:Parameter
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

		Filtrate(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			Viscosity = viscosity;
			Density = density;
		}
	}
}
