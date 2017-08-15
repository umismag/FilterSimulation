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
	public class CakeFormation : Parameter, ICakeFormationProcess
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
				if (specificCakeVolume == null || specificCakeVolume.Value == null)
					specificCakeVolume = GetSpecificCakeVolume();
				return specificCakeVolume;
			}
			set
			{
				specificCakeVolume = value;
				OnPropertyChanged("SpecificCakeVolume");
			}
		}

		PressureDifferenceCakeFormation pressureDifferenceCakeFormation = new PressureDifferenceCakeFormation();
		public PressureDifferenceCakeFormation PressureDifferenceCakeFormation
		{
			get { return pressureDifferenceCakeFormation; }
			set
			{
				pressureDifferenceCakeFormation = value;
				OnPropertyChanged("PressureDifferenceCakeFormation");
			}
		}

		FiltrationTime filtrationTime = new FiltrationTime();
		public FiltrationTime FiltrationTime
		{
			get { return filtrationTime; }
			set
			{
				filtrationTime = value;
				OnPropertyChanged("FiltrationTime");
			}
		}

		MassOfSuspension massOfSuspension = new MassOfSuspension();
		public MassOfSuspension MassOfSuspension
		{
			get { return massOfSuspension; }
			set
			{
				massOfSuspension = value;
				OnPropertyChanged("MassOfSuspension");
			}
		}

		CakeFormation ICakeFormationProcess.CakeFormation
		{
			get => this;
			set { }
		}

		SpecificCakeVolume GetSpecificCakeVolume()
		{
			double? res = Cake.CakeHeigth.Value * (1 - Cake.CakeHeigth.Value / Filter.MachineDiameter.Value);

			return new SpecificCakeVolume(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void SpecificCakeVolumeDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Cake.CakeHeigth, Filter.MachineDiameter";
			if (IsNeedToUpdate(dependentParameters, prop, SpecificCakeVolume, GetSpecificCakeVolume, sender))
			{
				SpecificCakeVolume = GetSpecificCakeVolume();
			}
			else
				return;
		}

		// F1.1
		CakeHeigth GetCakeHeight()
		{
			double? res;
			try
			{
				res = Math.Sqrt(
					Math.Pow(
						(Cake.InitialHeight.Value ?? double.NaN) +
						(Filter.SpecificFilterMediumResistance.Value ?? double.NaN)
						, 2) +
						2 * Cake.CakePermeability.Value ?? double.NaN *
						(Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
						1 - (Cake.Porosity.Value ?? double.NaN) -
						(Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						) *
						(PressureDifferenceCakeFormation.Value ?? double.NaN) *
						(FiltrationTime.Value ?? double.NaN) /
						(Suspension.MotherLiquid.MotherLiquidViscosity.Value ?? double.NaN)
						) -
						(Filter.SpecificFilterMediumResistance.Value ?? double.NaN);
			}
			catch
			{
				return new CakeHeigth() { Value = null };
			}
			return new CakeHeigth(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void CakeHeightDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Cake.InitialHeight, Filter.SpecificFilterMediumResistance, Cake.CakePermeability, Suspension.SolidsVolumeFraction, Cake.Porosity, Suspension.SolidsVolumeFraction, PressureDifferenceCakeFormation, FiltrationTime, Suspension.MotherLiquid.MotherLiquidViscosity";
			if (IsNeedToUpdate(dependentParameters, prop, Cake.CakeHeigth, GetCakeHeight, sender))
			{
				Cake.CakeHeigth = GetCakeHeight();
			}
			else
				return;
		}
		// F1.1

		// F1.2
		FiltrationTime GetFiltrationTime()
		{
			double? res, kappa;
			try
			{
				kappa = (Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
							1 - (Cake.Porosity.Value ?? double.NaN) -
							(Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						);
				res =
					(Suspension.MotherLiquid.MotherLiquidViscosity.Value ?? double.NaN) *
					(
					(Cake.CakeHeigth.Value ?? double.NaN) -
					(Cake.InitialHeight.Value ?? double.NaN)
					) * (
					(Cake.CakeHeigth.Value ?? double.NaN) +
					(Cake.InitialHeight.Value ?? double.NaN) +
					2 * (Filter.SpecificFilterMediumResistance.Value ?? double.NaN)
					) /
					(
						2 * (Cake.CakePermeability.Value ?? double.NaN) *
						kappa *
					(PressureDifferenceCakeFormation.Value ?? double.NaN)
					)
					;
			}
			catch
			{
				return new FiltrationTime() { Value = null };
			}
			return new FiltrationTime(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void FiltrationTimeDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.MotherLiquid.MotherLiquidViscosity, Cake.CakeHeigth, Cake.InitialHeight, Filter.SpecificFilterMediumResistance, Cake.CakePermeability, Suspension.SolidsVolumeFraction, Cake.Porosity, PressureDifferenceCakeFormation";
			if (IsNeedToUpdate(dependentParameters, prop, FiltrationTime, GetFiltrationTime, sender))
			{
				FiltrationTime = GetFiltrationTime();
			}
			else
				return;
		}
		// F1.2

		//F 2.1
		MassOfSuspension GetMassOfSuspension()
		{
			double? res, kappa;
			try
			{
				kappa = (Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
							1 - (Cake.Porosity.Value ?? double.NaN) -
							(Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						);
				res =
					(Suspension.SuspensionDensity.Value ?? double.NaN) *
					(Filter.Area.Value ?? double.NaN) *
					(
					(Cake.CakeHeigth.Value ?? double.NaN) -
					(Cake.InitialHeight.Value ?? double.NaN)
					) *
					(1 + kappa) / kappa
					;
			}
			catch
			{
				return new MassOfSuspension() { Value = null };
			}
			return new MassOfSuspension(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void MassOfSuspensionDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.SuspensionDensity, Filter.Area, Cake.CakeHeigth, Cake.InitialHeight, Suspension.SolidsVolumeFraction, Cake.Porosity, Suspension.SolidsVolumeFraction";
			if (IsNeedToUpdate(dependentParameters, prop, MassOfSuspension, GetMassOfSuspension, sender))
			{
				MassOfSuspension = GetMassOfSuspension();
			}
			else
				return;
		}
		// F2.1

		// F2.2
		CakeHeigth GetCakeHeight_With_Msus()
		{
			double? res, kappa;
			try
			{
				kappa = (Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
							1 - (Cake.Porosity.Value ?? double.NaN) -
							(Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						);
				res =
					(Cake.InitialHeight.Value ?? double.NaN) +
					(MassOfSuspension.Value ?? double.NaN) /
					(Suspension.SuspensionDensity.Value ?? double.NaN) /
					(Filter.Area.Value ?? double.NaN) *
					kappa / (1 + kappa)
					;
			}
			catch
			{
				return new CakeHeigth() { Value = null };
			}
			return new CakeHeigth(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void CakeHeight_With_MsusDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.SolidsVolumeFraction, Cake.Porosity, Cake.InitialHeight, MassOfSuspension, Suspension.SuspensionDensity, Filter.Area";
			if (IsNeedToUpdate(dependentParameters, prop, Cake.CakeHeigth, GetCakeHeight_With_Msus, sender))
			{
				Cake.CakeHeigth = GetCakeHeight_With_Msus();
			}
			else
				return;
		}
		// F2.2


		// F3
		CakePermeability GetCakePermeability()
		{
			double? res;
			try
			{
				res =
					(Cake.StandardCakePermeability.Value ?? double.NaN) *
					Math.Pow(
						(PressureDifferenceCakeFormation.Value ?? double.NaN)
						, -(Cake.Compressibility.Value ?? double.NaN)
						)
					;
			}
			catch
			{
				return new CakePermeability() { Value = null };
			}
			return new CakePermeability(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void CakePermeabilityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Cake.StandardCakePermeability, PressureDifferenceCakeFormation, Cake.Compressibility";
			if (IsNeedToUpdate(dependentParameters, prop, Cake.CakePermeability, GetCakePermeability, sender))
			{
				Cake.CakePermeability = GetCakePermeability();
			}
			else
				return;
		}
		// F3

		// F5
		SolidsVolumeFraction GetSolidsVolumeFraction()
		{
			double? res;
			try
			{
				res =
					(Suspension.SolidsMassFraction.Value) /
					(
						(Suspension.SolidDensity.Value) /
						Suspension.MotherLiquid.MotherLiquidDensity.Value *
						(
							1 - Suspension.SolidsMassFraction.Value
						) +
						Suspension.SolidsMassFraction.Value
					)
					;
			}
			catch
			{
				return new SolidsVolumeFraction() { Value = null };
			}
			return new SolidsVolumeFraction(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void SolidsVolumeFractionDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.SolidsMassFraction, Suspension.SolidDensity, Suspension.MotherLiquid.MotherLiquidDensity, Suspension.SolidsMassFraction";
			if (IsNeedToUpdate(dependentParameters, prop, Suspension.SolidsVolumeFraction, GetSolidsVolumeFraction, sender))
			{
				Suspension.SolidsVolumeFraction = GetSolidsVolumeFraction();
			}
			else
				return;
		}
		// F5

		// F6
		Porosity GetPorosity()
		{
			double? res;
			try
			{
				res =
					Cake.StandardPorosity.Value *
					Math.Pow(
					(PressureDifferenceCakeFormation.Value ?? double.NaN),
					-(Cake.Compressibility.Value ?? double.NaN)
					)
					;
			}
			catch
			{
				return new Porosity() { Value = null };
			}
			return new Porosity(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void PorosityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Cake.StandardPorosity, PressureDifferenceCakeFormation, Cake.Compressibility";
			if (IsNeedToUpdate(dependentParameters, prop, Cake.Porosity, GetPorosity, sender))
			{
				Cake.Porosity = GetPorosity();
			}
			else
				return;
		}
		// F6

		//F 7.1
		SuspensionDensity GetSuspensionDensity()
		{
			double? res;
			try
			{
				res = (1 - Suspension.SolidsVolumeFraction.Value) *
					Suspension.MotherLiquid.MotherLiquidDensity.Value +
					Suspension.SolidsVolumeFraction.Value *
					Suspension.SolidDensity.Value
					;
			}
			catch
			{
				return new SuspensionDensity() { Value = null };
			}
			return new SuspensionDensity(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void SuspensionDensityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.SolidsVolumeFraction, Suspension.MotherLiquid.MotherLiquidDensity, Suspension.SolidDensity";
			if (IsNeedToUpdate(dependentParameters, prop, Suspension.SuspensionDensity, GetSuspensionDensity, sender))
			{
				Suspension.SuspensionDensity = GetSuspensionDensity();
			}
			else
				return;
		}
		//F 7.1

		//F 7.2
		SolidDensity GetSolidDensity()
		{
			double? res;
			try
			{
				res =
					(
					Suspension.SuspensionDensity.Value -
					(1 - Suspension.SolidsVolumeFraction.Value) *
					Suspension.MotherLiquid.MotherLiquidDensity.Value
					)/
					Suspension.SolidsVolumeFraction.Value
					;
			}
			catch
			{
				return new SolidDensity() { Value = null };
			}
			return new SolidDensity(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void SolidDensityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.SuspensionDensity, Suspension.SolidsVolumeFraction, Suspension.MotherLiquid.MotherLiquidDensity";
			if (IsNeedToUpdate(dependentParameters, prop, Suspension.SolidDensity, GetSolidDensity, sender))
			{
				Suspension.SolidDensity = GetSolidDensity();
			}
			else
				return;
		}
		//F 7.2

		//F 8
		SolidsVolumeFraction GetSolidsVolumeFraction_Without_Cm()
		{
			double? res;
			try
			{
				res =
					Suspension.SolidConcentration.Value/
					Suspension.SolidDensity.Value
					;
			}
			catch
			{
				return new SolidsVolumeFraction() { Value = null };
			}
			return new SolidsVolumeFraction(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		void SolidsVolumeFraction_Without_Cm_DependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "Suspension.SolidConcentration, Suspension.SolidDensity";
			if (IsNeedToUpdate(dependentParameters, prop, Suspension.SolidsVolumeFraction, GetSolidsVolumeFraction_Without_Cm, sender))
			{
				Suspension.SolidsVolumeFraction = GetSolidsVolumeFraction_Without_Cm();
			}
			else
				return;
		}
		//F 8

		public CakeFormation(string name, Suspension suspension, Filter filter, Cake cake)
		{
			Name = name;
			Suspension = suspension;
			Filter = filter;
			Cake = cake;

			PropertyChangedStatic += SpecificCakeVolumeDependentParametersChanged;
			PropertyChangedStatic += CakeHeightDependentParametersChanged;
			PropertyChangedStatic += FiltrationTimeDependentParametersChanged;
			PropertyChangedStatic += MassOfSuspensionDependentParametersChanged;
			PropertyChangedStatic += CakeHeight_With_MsusDependentParametersChanged;
			PropertyChangedStatic += CakePermeabilityDependentParametersChanged;
			PropertyChangedStatic += SolidsVolumeFractionDependentParametersChanged;
			PropertyChangedStatic += PorosityDependentParametersChanged;
			PropertyChangedStatic += SuspensionDensityDependentParametersChanged;
			PropertyChangedStatic += SolidDensityDependentParametersChanged;
			PropertyChangedStatic += SolidsVolumeFraction_Without_Cm_DependentParametersChanged;
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



	public class FiltrationTime : Time
	{
		public FiltrationTime()
		{
			Name = "Filtration time";
			SymbolSuffix = "f";
			converter = new Param2DoubleConverter<FiltrationTime>();
		}

		public FiltrationTime(double? value) : this()
		{
			Value = value;
		}
	}

	public class Time : Parameter
	{
		public Time()
		{
			Name = "Time";
			Unit = "s";
			Symbol = "t";
			converter = new Param2DoubleConverter<Time>();
		}

		public Time(double? value) : this()
		{
			Value = value;
		}
	}

	public class MassOfSuspension : Mass
	{
		public MassOfSuspension()
		{
			Name = "Mass of Suspension";
			SymbolSuffix = "sus";
			converter = new Param2DoubleConverter<MassOfSuspension>();
		}

		public MassOfSuspension(double? value) : this()
		{
			Value = value;
		}
	}

	public class Mass : Parameter
	{
		public Mass()
		{
			Name = "Mass";
			Unit = "kg";
			Symbol = "M";
			converter = new Param2DoubleConverter<Mass>();
		}

		public Mass(double? value) : this()
		{
			Value = value;
		}
	}

	public class PressureDifferenceCakeFormation : Parameter
	{
		public PressureDifferenceCakeFormation()
		{
			Name = "Pressure difference Cake formation";
			Unit = "bar";
			Symbol = "Dp";
			converter = new Param2DoubleConverter<PressureDifferenceCakeFormation>();
		}

		public PressureDifferenceCakeFormation(double? value) : this()
		{
			Value = value;
		}
	}

	public class SpecificCakeVolume : Parameter
	{
		public SpecificCakeVolume()
		{

			Name = "Specific Cake Volume";
			Unit = "l/m2";
			Symbol = "vc";
			converter = new Param2DoubleConverter<SpecificCakeVolume>();
		}

		public SpecificCakeVolume(double? value) : this()
		{
			Value = value;
		}
	}

	public class Suspension : Parameter
	{
		MotherLiquid motherLiquid;
		public MotherLiquid MotherLiquid
		{
			get { return motherLiquid; }
			set
			{
				motherLiquid = value;
				OnPropertyChanged("MotherLiquid");
			}
		}

		SolidDensity solidDensity;
		public SolidDensity SolidDensity
		{
			get { return solidDensity; }
			set
			{
				solidDensity = value;
				OnPropertyChanged("SolidDensity");
			}
		}

		SuspensionDensity suspensionDensity = new SuspensionDensity();
		public SuspensionDensity SuspensionDensity
		{
			get { return suspensionDensity; }
			set
			{
				suspensionDensity = value;
				OnPropertyChanged("SuspensionDensity");
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

		SolidsMassFraction solidsMassFraction;
		public SolidsMassFraction SolidsMassFraction
		{
			get { return solidsMassFraction; }
			set
			{
				solidsMassFraction = value;
				OnPropertyChanged("SolidsMassFraction");
			}
		}

		SolidsVolumeFraction solidsVolumeFraction;
		public SolidsVolumeFraction SolidsVolumeFraction
		{
			get { return solidsVolumeFraction; }
			set
			{
				solidsVolumeFraction = value;
				OnPropertyChanged("SolidsVolumeFraction");
			}
		}

		public Suspension(string name, MotherLiquid motherLiquid, SolidDensity solidDensity, SuspensionDensity suspensionDensity, SolidConcentration solidConcentration, SolidsMassFraction solidsMassFraction, SolidsVolumeFraction solidsVolumeFraction)
		{
			Name = name;

			MotherLiquid = motherLiquid;
			SolidDensity = solidDensity;
			SuspensionDensity = suspensionDensity;
			SolidConcentration = solidConcentration;
			SolidsMassFraction = solidsMassFraction;
			SolidsVolumeFraction = solidsVolumeFraction;
		}
	}

	public class SuspensionDensity : Density
	{
		public SuspensionDensity()
		{
			Name = "Suspension Density";
			SymbolSuffix = "sus";
			converter = new Param2DoubleConverter<SuspensionDensity>();
		}

		public SuspensionDensity(double? value) : this()
		{
			Value = value;
		}
	}

	public class SolidDensity : Density
	{
		public SolidDensity()
		{
			Name = "Solid Density";
			SymbolSuffix = "s";
			converter = new Param2DoubleConverter<SolidDensity>();
		}

		public SolidDensity(double? value) : this()
		{
			Value = value;
		}
	}

	public class Filter : Parameter
	{
		SpecificFilterMediumResistance specificFilterMediumResistance = new SpecificFilterMediumResistance();
		public SpecificFilterMediumResistance SpecificFilterMediumResistance
		{
			get { return specificFilterMediumResistance; }
			set
			{
				specificFilterMediumResistance = value;
				OnPropertyChanged("SpecificFilterMediumResistance");
			}
		}

		MediumResistance mediumResistance = new MediumResistance();
		public MediumResistance MediumResistance
		{
			get { return mediumResistance; }
			set
			{
				mediumResistance = value;
				OnPropertyChanged("MediumResistance");
			}
		}

		Area area = new Area();
		public Area Area
		{
			get
			{
				if (area == null || area.Value == null)
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

		MachineDiameter machineDiameter = new MachineDiameter();
		public MachineDiameter MachineDiameter
		{
			get { return machineDiameter; }
			set
			{
				machineDiameter = value;
				OnPropertyChanged("MachineDiameter");
			}
		}

		MachineWidth machineWidth = new MachineWidth();
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
			string dependentParameters = "MachineDiameter, MachineWidth";
			if (IsNeedToUpdate(dependentParameters, prop, Area, GetArea, sender))
			{
				Area = GetArea();
			}
			else
				return;
		}

		Area GetArea()
		{
			double? res = Math.PI * MachineDiameter.Value * MachineWidth.Value / 1e6;
			return new Area(res) { SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore };
		}

		public Filter(string name, SpecificFilterMediumResistance specificFilterMediumResistance, MediumResistance mediumResistance)
		{
			Name = name;
			SpecificFilterMediumResistance = specificFilterMediumResistance;
			MediumResistance = mediumResistance;
		}

		public Filter(string name, SpecificFilterMediumResistance specificFilterMediumResistance, MediumResistance mediumResistance, MachineDiameter machineDiameter, MachineWidth machineWidth) : this(name, specificFilterMediumResistance, mediumResistance)
		{
			MachineDiameter = machineDiameter;
			MachineWidth = machineWidth;
			PropertyChangedStatic += AreaDependentParametersChanged;
		}
	}

	public class MachineDiameter : Parameter
	{
		public MachineDiameter()
		{
			Name = "Machine Diameter";
			Unit = "mm";
			Symbol = "D";
			converter = new Param2DoubleConverter<MachineDiameter>();
		}

		public MachineDiameter(double? value) : this()
		{
			if (value != null)
				Value = value;
			else
				return;
		}
	}

	public class MachineWidth : Parameter
	{
		public MachineWidth()
		{
			Name = "Machine Width";
			Unit = "mm";
			Symbol = "b";
			converter = new Param2DoubleConverter<MachineWidth>();
		}

		public MachineWidth(double? value) : this()
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
		Porosity porosity = new Porosity();
		public Porosity Porosity
		{
			get { return porosity; }
			set
			{
				porosity = value;
				OnPropertyChanged("Porosity");
			}
		}

		StandardPorosity standardPorosity = new StandardPorosity();
		public StandardPorosity StandardPorosity
		{
			get { return standardPorosity; }
			set
			{
				standardPorosity = value;
				OnPropertyChanged("StandardPorosity");
			}
		}

		PorosityReductionFactor porosityReductionFactor = new PorosityReductionFactor();
		public PorosityReductionFactor PorosityReductionFactor
		{
			get { return porosityReductionFactor; }
			set
			{
				porosityReductionFactor = value;
				OnPropertyChanged("PorosityReductionFactor");
			}
		}

		CakePermeability cakePermeability = new CakePermeability();
		public CakePermeability CakePermeability
		{
			get { return cakePermeability; }
			set
			{
				cakePermeability = value;
				OnPropertyChanged("CakePermeability");
			}
		}

		StandardCakePermeability standardCakePermeability = new StandardCakePermeability();
		public StandardCakePermeability StandardCakePermeability
		{
			get { return standardCakePermeability; }
			set
			{
				standardCakePermeability = value;
				OnPropertyChanged("StandardCakePermeability");
			}
		}

		Compressibility compressibility = new Compressibility();
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set
			{
				compressibility = value;
				OnPropertyChanged("Compressibility");
			}
		}

		CakeHeigth cakeHeigth = new CakeHeigth();
		public CakeHeigth CakeHeigth
		{
			get
			{
				return cakeHeigth;
			}
			set
			{
				cakeHeigth = value;
				OnPropertyChanged("CakeHeigth");
			}
		}

		InitialHeight initialHeight = new InitialHeight();
		public InitialHeight InitialHeight
		{
			get { return initialHeight; }
			set
			{
				initialHeight = value;
				OnPropertyChanged("InitialHeight");
			}
		}

		public Cake(string name, Porosity porosity, StandardPorosity standardPorosity, PorosityReductionFactor porosityReductionFactor, CakePermeability cakePermeability, StandardCakePermeability standardCakePermeability, Compressibility compressibility, CakeHeigth cakeHeigth)
		{
			Name = name;

			Porosity = porosity;
			StandardPorosity = standardPorosity;
			PorosityReductionFactor = porosityReductionFactor;
			CakePermeability = cakePermeability;
			StandardCakePermeability = standardCakePermeability;
			Compressibility = compressibility;
			CakeHeigth = cakeHeigth;

		}

		public Cake(string name, Porosity porosity, StandardPorosity standardPorosity, PorosityReductionFactor porosityReductionFactor, CakePermeability cakePermeability, StandardCakePermeability standardCakePermeability, Compressibility compressibility, CakeHeigth cakeHeigth, InitialHeight initialHeight = null) : this(name, porosity, standardPorosity, porosityReductionFactor, cakePermeability, standardCakePermeability, compressibility, cakeHeigth)
		{
			InitialHeight = initialHeight;
		}
	}

	public class InitialHeight : Height
	{
		public InitialHeight()
		{
			Name = "Initial Height";
			SymbolSuffix = "0";
			converter = new Param2DoubleConverter<InitialHeight>();
		}

		public InitialHeight(double? value) : this()
		{
			Value = value;
		}
	}

	public class CakeHeigth : Height
	{
		public CakeHeigth()
		{
			Name = "Cake Heigth";
			SymbolSuffix = "";
			converter = new Param2DoubleConverter<CakeHeigth>();
		}

		public CakeHeigth(double? value) : this()
		{
			Value = value;
		}
	}

	public class Height : Parameter
	{
		public Height()
		{
			Name = "Height";
			Unit = "mm";
			Symbol = "hc";
			converter = new Param2DoubleConverter<Height>();
		}

		public Height(double? value) : this()
		{
			Value = value;
		}
	}

	public class SpecificFilterMediumResistance : Parameter
	{
		public SpecificFilterMediumResistance()
		{
			Name = "Specific Filter Medium Resistance";
			Unit = "m";
			Symbol = "hce";
			converter = new Param2DoubleConverter<SpecificFilterMediumResistance>();
		}
		public SpecificFilterMediumResistance(double? value) : this()
		{
			Value = value;
		}
	}

	public class MediumResistance : Parameter
	{
		public MediumResistance()
		{
			Name = "Medium Resistance";
			Unit = "m-1";
			Symbol = "Rm";
			converter = new Param2DoubleConverter<MediumResistance>();
		}

		public MediumResistance(double? value) : this()
		{
			Value = value;
		}
	}

	public class Compressibility : Parameter
	{
		public Compressibility()
		{
			Name = "Compressibility";
			Unit = "-";
			Symbol = "nc";
			converter = new Param2DoubleConverter<Compressibility>();
		}

		public Compressibility(double? value) : this()
		{
			Value = value;
		}
	}

	public class StandardCakePermeability : Permeability
	{
		public StandardCakePermeability()
		{
			Name = "Standard Cake Permeability";
			SymbolSuffix = "c0";
			converter = new Param2DoubleConverter<StandardCakePermeability>();
		}

		public StandardCakePermeability(double? value) : this()
		{
			Value = value;
		}
	}

	public class CakePermeability : Permeability
	{
		public CakePermeability()
		{
			Name = "Cake Permeability";
			SymbolSuffix = "c";
			converter = new Param2DoubleConverter<CakePermeability>();
		}

		public CakePermeability(double? value) : this()
		{
			Value = value;
		}
	}

	public class Permeability : Parameter
	{
		public Permeability()
		{
			Name = "Permeability";
			Unit = "*E-13 m2";
			Symbol = "P";
			converter = new Param2DoubleConverter<Permeability>();
		}

		public Permeability(double? value) : this()
		{
			Value = value;
		}
	}

	public class StandardPorosity : Porosity
	{
		public StandardPorosity()
		{
			Name = "Standard Cake porosity";
			SymbolSuffix = "0";
			converter = new Param2DoubleConverter<StandardPorosity>();
		}

		public StandardPorosity(double? value) : this()
		{
			Value = value;
		}
	}

	public class PorosityReductionFactor : Parameter
	{
		public PorosityReductionFactor()
		{
			Name = "Porosity reduction factor";
			Unit = "--";
			Symbol = "ne";
			converter = new Param2DoubleConverter<PorosityReductionFactor>();
		}

		public PorosityReductionFactor(double? value) : this()
		{
			Value = value;
		}
	}

	public class Porosity : Parameter
	{
		public Porosity()
		{
			Name = "Porosity";
			Unit = "%";
			Symbol = "E";
			converter = new Param2DoubleConverter<Porosity>();
		}

		public Porosity(double? value) : this()
		{
			Value = value;
		}
	}

	public class SolidsVolumeFraction : SolidConcentration
	{
		public SolidsVolumeFraction()
		{
			Name = "Solids volume fraction";
			SymbolSuffix = "v";
			Unit = "--";
			converter = new Param2DoubleConverter<SolidsVolumeFraction>();
		}

		public SolidsVolumeFraction(double? value) : this()
		{
			Value = value;
		}
	}

	public class SolidsMassFraction : SolidConcentration
	{
		public SolidsMassFraction()
		{
			Name = "Solids mass fraction";
			Unit = "--";
			SymbolSuffix = "m";
			converter = new Param2DoubleConverter<SolidsMassFraction>();
		}

		public SolidsMassFraction(double? value) : this()
		{
			Value = value;
		}
	}

	public class SolidConcentration : Parameter
	{
		public SolidConcentration()
		{
			Name = "Solid Concentration";
			Unit = "kg/m3";
			Symbol = "C";
			converter = new Param2DoubleConverter<SolidConcentration>();
		}

		public SolidConcentration(double? value) : this()
		{
			Value = value;
		}
	}

	public class MotherLiquid : Parameter
	{
		MotherLiquidViscosity motherLiquidViscosity;
		public MotherLiquidViscosity MotherLiquidViscosity
		{
			get { return motherLiquidViscosity; }
			set
			{
				motherLiquidViscosity = value;
				OnPropertyChanged("MotherLiquidViscosity");
			}
		}

		MotherLiquidDensity motherLiquidDensity;
		public MotherLiquidDensity MotherLiquidDensity
		{
			get { return motherLiquidDensity; }
			set
			{
				motherLiquidDensity = value;
				OnPropertyChanged("MotherLiquidDensity");
			}
		}

		public MotherLiquid(string name, MotherLiquidViscosity motherLiquidViscosity, MotherLiquidDensity motherLiquidDensity)
		{
			Name = name;
			MotherLiquidViscosity = motherLiquidViscosity;
			MotherLiquidDensity = motherLiquidDensity;
		}
	}

	public class MotherLiquidDensity : Density
	{
		public MotherLiquidDensity()
		{
			Name = "Density mother liquid";
			SymbolSuffix = "f";
			converter = new Param2DoubleConverter<MotherLiquidDensity>();
		}

		public MotherLiquidDensity(double? value) : this()
		{
			Value = value;
		}
	}

	public class MotherLiquidViscosity : Viscosity
	{
		public MotherLiquidViscosity()
		{
			Name = "Viscosity mother liquid";
			SymbolSuffix = "f";
			converter = new Param2DoubleConverter<MotherLiquidViscosity>();
		}

		public MotherLiquidViscosity(double? value) : this()
		{
			Value = value;
		}
	}
}
