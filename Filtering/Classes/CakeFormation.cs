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

		SpecificCakeVolume specificCakeVolume;// = new SpecificCakeVolume();
		public SpecificCakeVolume SpecificCakeVolume
		{
			get
			{
				//if (specificCakeVolume == null || specificCakeVolume.Value == null)
				//	specificCakeVolume.Value = specificCakeVolume.GetSpecificCakeVolume();
				return specificCakeVolume;
			}
			set
			{
				specificCakeVolume = value;
				OnPropertyChanged("SpecificCakeVolume");
			}
		}

		PressureDifferenceCakeFormation pressureDifferenceCakeFormation;// = new PressureDifferenceCakeFormation();
		public PressureDifferenceCakeFormation PressureDifferenceCakeFormation
		{
			get { return pressureDifferenceCakeFormation; }
			set
			{
				pressureDifferenceCakeFormation = value;
				OnPropertyChanged("PressureDifferenceCakeFormation");
			}
		}

		FiltrationTime filtrationTime;// = new FiltrationTime();
		public FiltrationTime FiltrationTime
		{
			get { return filtrationTime; }
			set
			{
				filtrationTime = value;
				OnPropertyChanged("FiltrationTime");
			}
		}

		MassOfSuspension massOfSuspension;// = new MassOfSuspension();
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

		public CakeFormation(string name, Suspension suspension, Filter filter, Cake cake)
		{
			Name = name;
			Suspension = suspension;
			Filter = filter;
			Cake = cake;
		}

	}

	

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
			dependentParameters = "Suspension.MotherLiquid.MotherLiquidViscosity, Cake.CakeHeigth, Cake.InitialHeight, Filter.SpecificFilterMediumResistance, Cake.CakePermeability, Suspension.SolidsVolumeFraction, Cake.Porosity, PressureDifferenceCakeFormation";
		}

		public FiltrationTime(double? value) : this()
		{
			Value = value;
		}

		// F1.2
		public override double?  GetUpdatedParameter()//GetFiltrationTime()
		{
			double? res, kappa;
			try
			{
				kappa = (process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
							1 - (process.CakeFormation.Cake.Porosity.Value ?? double.NaN) -
							(process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						);
				res =
					(process.CakeFormation.Suspension.MotherLiquid.MotherLiquidViscosity.Value ?? double.NaN) *
					(
					(process.CakeFormation.Cake.CakeHeigth.Value ?? double.NaN) -
					(process.CakeFormation.Cake.InitialHeight.Value ?? double.NaN)
					) * (
					(process.CakeFormation.Cake.CakeHeigth.Value ?? double.NaN) +
					(process.CakeFormation.Cake.InitialHeight.Value ?? double.NaN) +
					2 * (process.CakeFormation.Filter.SpecificFilterMediumResistance.Value ?? double.NaN)
					) /
					(
						2 * (process.CakeFormation.Cake.CakePermeability.Value ?? double.NaN) *
						kappa *
					(process.CakeFormation.PressureDifferenceCakeFormation.Value ?? double.NaN)
					)
					;
			}
			catch
			{
				res = null;
			}
			return res;
		}
		// F1.2
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
			dependentParameters = "Suspension.SuspensionDensity, Filter.Area, Cake.CakeHeigth, Cake.InitialHeight, Suspension.SolidsVolumeFraction, Cake.Porosity, Suspension.SolidsVolumeFraction";
		}

		public MassOfSuspension(double? value) : this()
		{
			Value = value;
		}

		//F 2.1
		public override double? GetUpdatedParameter()//GetMassOfSuspension()
		{
			double? res, kappa;
			try
			{
				kappa = (process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
							1 - (process.CakeFormation.Cake.Porosity.Value ?? double.NaN) -
							(process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						);
				res =
					(process.CakeFormation.Suspension.SuspensionDensity.Value ?? double.NaN) *
					(process.CakeFormation.Filter.Area.Value ?? double.NaN) *
					(
					(process.CakeFormation.Cake.CakeHeigth.Value ?? double.NaN) -
					(process.CakeFormation.Cake.InitialHeight.Value ?? double.NaN)
					) *
					(1 + kappa) / kappa
					;
			}
			catch
			{
				res = null;
			}
			return res;
		}
		// F2.1
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
			MinValue = 0.5;
			MaxValue = 6;
			sourceOfMinMaxChanging = SourceOfChanging.ManuallyByUser;
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
			dependentParameters = "Cake.CakeHeigth, Filter.MachineDiameter";
		}

		public SpecificCakeVolume(double? value) : this()
		{
			Value = value;
		}

		// F -- GetSpecificCakeVolume
		public override double? GetUpdatedParameter()//GetSpecificCakeVolume()
		{
			double? res;
			try
			{
				res =
					process.CakeFormation.Cake.CakeHeigth.Value *
					(1 - process.CakeFormation.Cake.CakeHeigth.Value /
					process.CakeFormation.Filter.MachineDiameter.Value);
			}
			catch
			{
				res = null;
			}
			return res;
		}
		// F -- GetSpecificCakeVolume
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

		SuspensionDensity suspensionDensity;// = new SuspensionDensity();
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
			dependentParameters = "Suspension.SolidsVolumeFraction, Suspension.MotherLiquid.MotherLiquidDensity, Suspension.SolidDensity";
			//PropertyChangedStatic += SuspensionDensityDependentParametersChanged;
		}

		public SuspensionDensity(double? value) : this()
		{
			Value = value;
		}

		//F 7.1
		public override double? GetUpdatedParameter()//double? GetSuspensionDensity()
		{
			double? res;
			try
			{
				res = (1 - process.CakeFormation.Suspension.SolidsVolumeFraction.Value) *
					process.CakeFormation.Suspension.MotherLiquid.MotherLiquidDensity.Value +
					process.CakeFormation.Suspension.SolidsVolumeFraction.Value *
					process.CakeFormation.Suspension.SolidDensity.Value
					;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void SuspensionDensityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{


		//	IfNeedThenUpdate(dependentParameters, prop, this, GetSuspensionDensity);
		//}
		//F 7.1
	}

	public class SolidDensity : Density
	{
		public SolidDensity()
		{
			Name = "Solid Density";
			SymbolSuffix = "s";
			converter = new Param2DoubleConverter<SolidDensity>();
			dependentParameters = "Suspension.SuspensionDensity, Suspension.SolidsVolumeFraction, Suspension.MotherLiquid.MotherLiquidDensity";
			//PropertyChangedStatic += SolidDensityDependentParametersChanged;
		}

		public SolidDensity(double? value) : this()
		{
			Value = value;
		}

		//F 7.2
		public override double? GetUpdatedParameter()//double? GetSolidDensity()
		{
			double? res;
			try
			{
				res =
					(
					process.CakeFormation.Suspension.SuspensionDensity.Value -
					(1 - process.CakeFormation.Suspension.SolidsVolumeFraction.Value) *
					process.CakeFormation.Suspension.MotherLiquid.MotherLiquidDensity.Value
					) /
					process.CakeFormation.Suspension.SolidsVolumeFraction.Value
					;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void SolidDensityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{
		

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetSolidDensity);
		//}
		//F 7.2
	}

	public class Filter : Parameter
	{
		SpecificFilterMediumResistance specificFilterMediumResistance;// = new SpecificFilterMediumResistance();
		public SpecificFilterMediumResistance SpecificFilterMediumResistance
		{
			get { return specificFilterMediumResistance; }
			set
			{
				specificFilterMediumResistance = value;
				OnPropertyChanged("SpecificFilterMediumResistance");
			}
		}

		MediumResistance mediumResistance;// = new MediumResistance();
		public MediumResistance MediumResistance
		{
			get { return mediumResistance; }
			set
			{
				mediumResistance = value;
				OnPropertyChanged("MediumResistance");
			}
		}

		Area area;// = new Area();
		public Area Area
		{
			get
			{
				//if (area == null || area.Value == null)
				//	return GetArea();
				//else
					return area;
			}
			set
			{
				area = value;
				OnPropertyChanged("Area");
			}
		}

		MachineDiameter machineDiameter;// = new MachineDiameter();
		public MachineDiameter MachineDiameter
		{
			get { return machineDiameter; }
			set
			{
				machineDiameter = value;
				OnPropertyChanged("MachineDiameter");
			}
		}

		MachineWidth machineWidth;// = new MachineWidth();
		public MachineWidth MachineWidth
		{
			get { return machineWidth; }
			set
			{
				machineWidth = value;
				OnPropertyChanged("MachineWidth");
			}
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
			dependentParameters = "MachineDiameter, MachineWidth";
			//PropertyChangedStatic += AreaDependentParametersChanged;
		}

		public Area(double? value) : this()
		{
			Value = value;
		}

		// -- F
		public override double? GetUpdatedParameter()//public double? GetArea()
		{
			double? res;
			try
			{
				res = Math.PI * process.CakeFormation.Filter.MachineDiameter.Value * process.CakeFormation.Filter.MachineWidth.Value / 1e6;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void AreaDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetArea);
		//}
		// -- F
	}

	public class Cake : Parameter
	{
		Porosity porosity;// = new Porosity();
		public Porosity Porosity
		{
			get { return porosity; }
			set
			{
				porosity = value;
				OnPropertyChanged("Porosity");
			}
		}

		StandardPorosity standardPorosity;// = new StandardPorosity();
		public StandardPorosity StandardPorosity
		{
			get { return standardPorosity; }
			set
			{
				standardPorosity = value;
				OnPropertyChanged("StandardPorosity");
			}
		}

		PorosityReductionFactor porosityReductionFactor;// = new PorosityReductionFactor();
		public PorosityReductionFactor PorosityReductionFactor
		{
			get { return porosityReductionFactor; }
			set
			{
				porosityReductionFactor = value;
				OnPropertyChanged("PorosityReductionFactor");
			}
		}

		CakePermeability cakePermeability;// = new CakePermeability();
		public CakePermeability CakePermeability
		{
			get { return cakePermeability; }
			set
			{
				cakePermeability = value;
				OnPropertyChanged("CakePermeability");
			}
		}

		StandardCakePermeability standardCakePermeability;// = new StandardCakePermeability();
		public StandardCakePermeability StandardCakePermeability
		{
			get { return standardCakePermeability; }
			set
			{
				standardCakePermeability = value;
				OnPropertyChanged("StandardCakePermeability");
			}
		}

		Compressibility compressibility;// = new Compressibility();
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set
			{
				compressibility = value;
				OnPropertyChanged("Compressibility");
			}
		}

		CakeHeigth cakeHeigth;// = new CakeHeigth();
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

		InitialHeight initialHeight;// = new InitialHeight();
		public InitialHeight InitialHeight
		{
			get { return initialHeight; }
			set
			{
				initialHeight = value;
				OnPropertyChanged("InitialHeight");
			}
		}

		public Cake()
		{
			
		}

		public Cake(string name, Porosity porosity, StandardPorosity standardPorosity, PorosityReductionFactor porosityReductionFactor, CakePermeability cakePermeability, StandardCakePermeability standardCakePermeability, Compressibility compressibility, CakeHeigth cakeHeigth) : this()
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
			dependentParameters = "Cake.InitialHeight, Filter.SpecificFilterMediumResistance, Cake.CakePermeability, Suspension.SolidsVolumeFraction, Cake.Porosity, Suspension.SolidsVolumeFraction, PressureDifferenceCakeFormation, FiltrationTime, Suspension.MotherLiquid.MotherLiquidViscosity";
			dependent2Parameters = "Suspension.SolidsVolumeFraction, Cake.Porosity, Cake.InitialHeight, MassOfSuspension, Suspension.SuspensionDensity, Filter.Area";
			MinValue = 50;
			MaxValue = 500;
			sourceOfMinMaxChanging = SourceOfChanging.ManuallyByUser;
		}

		public CakeHeigth(double? value) : this()
		{
			Value = value;
		}

		// F1.1
		public override double? GetUpdatedParameter()//double? GetCakeHeight()
		{
			double? res;
			try
			{
				res = Math.Sqrt(
					Math.Pow(
						(process.CakeFormation.Cake.InitialHeight.Value ?? double.NaN) +
						(process.CakeFormation.Filter.SpecificFilterMediumResistance.Value ?? double.NaN)
						, 2) +
						2 * process.CakeFormation.Cake.CakePermeability.Value ?? double.NaN *
						(process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
						1 - (process.CakeFormation.Cake.Porosity.Value ?? double.NaN) -
						(process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						) *
						(process.CakeFormation.PressureDifferenceCakeFormation.Value ?? double.NaN) *
						(process.CakeFormation.FiltrationTime.Value ?? double.NaN) /
						(process.CakeFormation.Suspension.MotherLiquid.MotherLiquidViscosity.Value ?? double.NaN)
						) -
						(process.CakeFormation.Filter.SpecificFilterMediumResistance.Value ?? double.NaN);
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void CakeHeightDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{
			
		//	IfNeedThenUpdate(dependentParameters, prop, this, GetCakeHeight);
		//}
		// F1.1

		// F2.2
		public override double? Get2UpdatedParameter()//double? GetCakeHeight_With_Msus()
		{
			double? res, kappa;
			try
			{
				kappa = (process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN) /
						(
							1 - (process.CakeFormation.Cake.Porosity.Value ?? double.NaN) -
							(process.CakeFormation.Suspension.SolidsVolumeFraction.Value ?? double.NaN)
						);
				res =
					(process.CakeFormation.Cake.InitialHeight.Value ?? double.NaN) +
					(process.CakeFormation.MassOfSuspension.Value ?? double.NaN) /
					(process.CakeFormation.Suspension.SuspensionDensity.Value ?? double.NaN) /
					(process.CakeFormation.Filter.Area.Value ?? double.NaN) *
					kappa / (1 + kappa)
					;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void CakeHeight_With_MsusDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{
			

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetCakeHeight_With_Msus);
		//}
		// F2.2
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
			dependentParameters = "Cake.StandardCakePermeability, PressureDifferenceCakeFormation, Cake.Compressibility";
			//PropertyChangedStatic += CakePermeabilityDependentParametersChanged;
		}

		public CakePermeability(double? value) : this()
		{
			Value = value;
		}

		// F3
		public override double? GetUpdatedParameter()//double? GetCakePermeability()
		{
			double? res;
			try
			{
				res =
					(process.CakeFormation.Cake.StandardCakePermeability.Value ?? double.NaN) *
					Math.Pow(
						(process.CakeFormation.PressureDifferenceCakeFormation.Value ?? double.NaN)
						, -(process.CakeFormation.Cake.Compressibility.Value ?? double.NaN)
						)
					;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void CakePermeabilityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{
	

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetCakePermeability);
		//}
		// F3
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
			dependentParameters = "Cake.StandardPorosity, PressureDifferenceCakeFormation, Cake.Compressibility";
			//PropertyChangedStatic += PorosityDependentParametersChanged;
		}

		public Porosity(double? value) : this()
		{
			Value = value;
		}

		// F6
		public override double? GetUpdatedParameter()//double? GetPorosity()
		{
			double? res;
			try
			{
				res =
					process.CakeFormation.Cake.StandardPorosity.Value *
					Math.Pow(
					(process.CakeFormation.PressureDifferenceCakeFormation.Value ?? double.NaN),
					-(process.CakeFormation.Cake.Compressibility.Value ?? double.NaN)
					)
					;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void PorosityDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetPorosity);
		//}
		// F6
	}

	public class SolidsVolumeFraction : SolidConcentration
	{
		public SolidsVolumeFraction()
		{
			Name = "Solids volume fraction";
			SymbolSuffix = "v";
			Unit = "--";
			converter = new Param2DoubleConverter<SolidsVolumeFraction>();
			dependentParameters = "Suspension.SolidsMassFraction, Suspension.SolidDensity, Suspension.MotherLiquid.MotherLiquidDensity, Suspension.SolidsMassFraction";
			dependent2Parameters = "Suspension.SolidConcentration, Suspension.SolidDensity";
			//PropertyChangedStatic += SolidsVolumeFractionDependentParametersChanged;
			//PropertyChangedStatic += SolidsVolumeFraction_Without_Cm_DependentParametersChanged;
		}

		public SolidsVolumeFraction(double? value) : this()
		{
			Value = value;
		}

		// F5
		public override double? GetUpdatedParameter()//double? GetSolidsVolumeFraction()
		{
			double? res;
			try
			{
				res =
					(process.CakeFormation.Suspension.SolidsMassFraction.Value) /
					(
						(process.CakeFormation.Suspension.SolidDensity.Value) /
						process.CakeFormation.Suspension.MotherLiquid.MotherLiquidDensity.Value *
						(
							1 - process.CakeFormation.Suspension.SolidsMassFraction.Value
						) +
						process.CakeFormation.Suspension.SolidsMassFraction.Value
					)
					;
			}
			catch
			{
				res = null;
			}

			
			return res;
		}

		//void SolidsVolumeFractionDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{
			

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetSolidsVolumeFraction);
		//}
		// F5

		//F 8
		public override double? Get2UpdatedParameter()//double? GetSolidsVolumeFraction_Without_Cm()
		{
			double? res;
			try
			{
				res =
					process.CakeFormation.Suspension.SolidConcentration.Value /
					process.CakeFormation.Suspension.SolidDensity.Value
					;
			}
			catch
			{
				res = null;
			}
			
			return res;
		}

		//void SolidsVolumeFraction_Without_Cm_DependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		//{
			

		//	IfNeedThenUpdate(dependentParameters, prop, this, GetSolidsVolumeFraction_Without_Cm);
		//}
		//F 8
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
			dependentParameters = "Suspension.SolidsVolumeFraction, Suspension.SolidDensity";
		}

		public SolidConcentration(double? value) : this()
		{
			Value = value;
		}

		public override double? GetUpdatedParameter()//public double? GetWashLiquidVolume()
		{
			double? res;
			try
			{
				res =
					process.CakeFormation.Suspension.SolidsVolumeFraction.Value *
					process.CakeFormation.Suspension.SolidDensity.Value;
			}
			catch
			{
				res = null;
			}
			return res;
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
