using System;
using System.Windows.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections.ObjectModel;

namespace Filtering
{
	public class CakeFormation:Parameter
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
				if (specificCakeVolume == null)
					specificCakeVolume = GetSpecificCakeVolume();
				return specificCakeVolume;
			}
			set
			{
				specificCakeVolume = value;
				OnPropertyChanged("SpecificCakeVolume");
			}
		}

		SpecificCakeVolume GetSpecificCakeVolume()
		{
			double? res = Cake.Height.Value * (1 - Cake.Height.Value / Filter.MachineDiameter.Value);

			return new SpecificCakeVolume(res) { SourceOfParameterChanging=SourceOfChanging.AutomaticallyByCore};
		}

		void SpecificCakeVolumeDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Cake.Height, Filter.MachineDiameter";
			if (dependentParameters.Contains(prop.PropertyName))
			{
				SpecificCakeVolume = GetSpecificCakeVolume();
			}
			else
				return;
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
				OnPropertyChanged("WashingRatio");
			}
		}

		Volume washLiquidvolume;
		public Volume WashLiquidVolume
		{
			get
			{
				return washLiquidvolume=GetWashLiquidVolume();
			}
			set
			{
				washLiquidvolume = value;
				OnPropertyChanged("WashLiquidVolume");
			}
		}

		void WashLiquidVolumeDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "WashingRatio, Area, SpecificCakeVolume, Porosity";
			if (dependentParameters.Contains(prop.PropertyName) )
			{
				WashLiquidVolume= GetWashLiquidVolume();
			}
			else
				return;
		}



		public CakeFormation(string name, Suspension suspension, Filter filter, Cake cake, WashingRatio washingRatio)
		{
			Name = name;
			Suspension = suspension;
			Filter = filter;
			Cake = cake;
			WashingRatio = washingRatio;
			//PropertyChanged += WashingRatioPropertyChanged;
			PropertyChangedStatic += SpecificCakeVolumeDependentParametersChanged;
			PropertyChangedStatic += WashLiquidVolumeDependentParametersChanged;
		}

		public Volume GetWashLiquidVolume()
		{
			//WashingLiquid wl = new WashingLiquid("tmpLiquid", new Viscosity(), new Density(), new Volume());

			double? res;

			res = Cake.Porosity.Value * Filter.Area.Value * SpecificCakeVolume.Value * WashingRatio.Value / 100;
			Volume vl = new Volume(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
			return vl;
		}
	}

	//[ValueConversion(typeof(WashingRatio), typeof(double))]
	//public class WashingRatio2DoubleConverter : IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		WashingRatio tmp = value as WashingRatio;
	//		if (tmp != null)
	//			return tmp.Value;
	//		else
	//			return value;
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		if (value != null)
	//			return new WashingRatio(double.Parse(value.ToString()));
	//		else
	//			return null;
	//	}
	//}

	//[ValueConversion(typeof(CakeFormations), typeof(Parameter))]
	//public class CakeFormation2ListOfParametersConverter : IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		ObservableCollection<Parameter> cf;
	//		cf= MyReflection.PrintParamPropObject((value as CakeFormations)[0] as Parameter);
	//		return cf;
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

	//[ValueConversion(typeof(CakeFormations), typeof(ObjWithParametersList))]
	//public class CakeFormation2ObjWithListOfParametersConverter : IValueConverter
	//{
	//	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		return MyReflection.PrintObjWithParamList((value as CakeFormations)[0] as Parameter).ParametersList;
	//	}

	//	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	//	{
	//		throw new NotImplementedException();
	//	}
	//}

	public class SpecificCakeVolume:Parameter
	{
		public SpecificCakeVolume()
		{
			
			Name = "Specific Cake Volume";
			Unit = "l/m2";
			Symbol = "vc";
			converter = new Param2DoubleConverter<SpecificCakeVolume>();
		}

		public SpecificCakeVolume(double? value):this()
		{
			Value = value;
		}
	}

	public class Suspension :Parameter
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
			set
			{
				solidDensity = value;
				OnPropertyChanged("SolidDensity");
			}
		}

		SolidConcentration solidConcentration;
		public SolidConcentration SolidConcentration
		{
			get { return solidConcentration; }
			set
			{
				solidConcentration = value;
				OnPropertyChanged("SolidConcentration");
			}
		}

		Compressibility compressibility;
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set
			{
				compressibility = value;
				OnPropertyChanged("Compressibility");
			}
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

	public class Filter :Parameter
	{
		Resistance mediumResistance;
		public Resistance MediumResistance
		{
			get { return mediumResistance; }
			set
			{
				mediumResistance = value;
				OnPropertyChanged("MediumResistance");
			}
		}

		Area area;
		public Area Area
		{
			get
			{
				if (area == null)
					return GetArea();
				else
					return area;
			}
			set
			{
				area = value;
				OnPropertyChanged("Area");
			}
		}

		MachineDiameter machineDiameter;
		public MachineDiameter MachineDiameter
		{
			get { return machineDiameter; }
			set
			{
				machineDiameter = value;
				OnPropertyChanged("MachineDiameter");
			}
		}

		MachineWidth machineWidth;
		public MachineWidth MachineWidth
		{
			get { return machineWidth; }
			set
			{
				machineWidth = value;
				OnPropertyChanged("MachineWidth");
			}
		}

		void AreaDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			if(prop.PropertyName.Contains("MachineDiameter")||prop.PropertyName.Contains("MachineWidth"))
			{
				Area = GetArea();
			}
		}

		Area GetArea()
		{
			double? res = Math.PI * MachineDiameter.Value * MachineWidth.Value / 1e6;
			return new Area(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
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
			PropertyChangedStatic += AreaDependentParametersChanged;
		}
	}

	public class MachineDiameter :Parameter
	{
		public MachineDiameter()
		{
			Name = "Machine Diameter";
			Unit = "mm";
			Symbol = "D";
			converter = new Param2DoubleConverter<MachineDiameter>();
		}

		public MachineDiameter(double? value):this()
		{
			if (value != null)
				Value = value;
			else
				return;
		}
	}

	public class MachineWidth :Parameter
	{
		public MachineWidth()
		{
			Name = "Machine Width";
			Unit = "mm";
			Symbol = "b";
			converter = new Param2DoubleConverter<MachineWidth>();
		}

		public MachineWidth(double? value):this()
		{
			Value = value;
		}
	}

	public class Area : Parameter
	{
		public Area()
		{
			Name = "Area";
			Unit = "m2";
			Symbol = "A";
			converter = new Param2DoubleConverter<Area>();
		}

		public Area(double? value) : this()
		{
			Value = value;
		}
	}

	public class Cake : Parameter
	{
		Porosity porosity;
		public Porosity Porosity
		{
			get { return porosity; }
			set
			{
				porosity = value;
				OnPropertyChanged("Porosity");
			}
		}

		Permeability permeability;
		public Permeability Permeability
		{
			get { return permeability; }
			set
			{
				permeability = value;
				OnPropertyChanged("Permeability");
			}
		}

		Compressibility compressibility;
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set
			{
				compressibility = value;
				OnPropertyChanged("Compressibility");
			}
		}

		Height height;
		public Height Height
		{
			get
			{
				return height;
			}
			set
			{
				height = value;
				OnPropertyChanged("Height");
			}
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

	public class Height :Parameter
	{
		public Height()
		{
			Name = "Height";
			Unit = "mm";
			Symbol = "hc";
			converter = new Param2DoubleConverter<Height>();
		}

		public Height(double? value):this()
		{
			Value = value;
		}
	}

	public class Resistance :Parameter
	{
		public Resistance()
		{
			Name = "Resistance";
			Unit = "mm";
			Symbol = "hce";
			converter = new Param2DoubleConverter<Resistance>();
		}
		public Resistance(double? value):this()
		{
			Value = value;
		}
	}

	public class Compressibility :Parameter
	{
		public Compressibility()
		{
			Name = "Compressibility";
			Unit = "-";
			Symbol = "nc";
			converter = new Param2DoubleConverter<Compressibility>();
		}

		public Compressibility(double? value):this()
		{
			Value = value;
		}
	}

	public class Permeability :Parameter
	{
		public Permeability()
		{
			Name = "Permeability";
			Unit = "*E-13 m2";
			Symbol = "P";
			converter = new Param2DoubleConverter<Permeability>();
		}

		public Permeability(double? value):this()
		{
			Value = value;
		}
	}

	public class Porosity :Parameter
	{
		public Porosity()
		{
			Name = "Porosity";
			Unit = "%";
			Symbol = "E";
			converter = new Param2DoubleConverter<Porosity>();
		}

		public Porosity(double? value):this()
		{
			Value = value;
		}
	}

	public class SolidConcentration :Parameter
	{
		public SolidConcentration()
		{
			Name = "Solid Concentration";
			Unit = "%";
			Symbol = "Cm";
			converter = new Param2DoubleConverter<SolidConcentration>();
		}

		public SolidConcentration(double? value):this()
		{
			Value = value;
		}
	}

	public class Filtrate :Parameter
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

		public Filtrate(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			Viscosity = viscosity;
			Density = density;
		}
	}
}
