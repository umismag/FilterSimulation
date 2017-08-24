using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Filtering
{
	public class DisplayParameter
	{
		Parameter parameter;
		public Parameter Parameter
		{
			get { return parameter; }
			set
			{
				parameter = value;
			}
		}

		int groupNumber;
		public int GroupNumber
		{
			get { return groupNumber; }
			set
			{
				groupNumber = value;
			}
		}

		int subGroupNumber;
		public int SubGroupNumber
		{
			get { return subGroupNumber; }
			set
			{
				subGroupNumber = value;
			}
		}

		bool isAlwaysDisplay = false;
		internal bool IsAlwaysDisplay
		{
			get { return isAlwaysDisplay; }
			set
			{
				isAlwaysDisplay = value;
			}
		}

		public GroupsOfParameters paramInGroup;

		public DisplayParameter(Parameter parameter, int groupNumber, int subGroupNumber, bool isAlwaysDisplay = false)
		{
			Parameter = parameter;
			GroupNumber = groupNumber;
			SubGroupNumber = subGroupNumber;
			IsAlwaysDisplay = isAlwaysDisplay;
		}
	}

	public enum CalculationTypes { StandardCalculation, FilterDesign }


	public class GroupsOfParameters : SortedList<int, List<DisplayParameter>>
	{
		public void SetRepresentateForGroup(DisplayParameter dispPar)
		{
			foreach(DisplayParameter dp in Values[dispPar.GroupNumber-1])
			{
				if (dp==dispPar)
				{
					dp.Parameter.SourceOfParameterChanging = SourceOfChanging.ManuallyByUser;
				}
				else
				{
					dp.Parameter.SourceOfParameterChanging = SourceOfChanging.AutomaticallyByCore;
					dp.Parameter.OnPropertyChanged("SourceOfParameterChanging");
				}
			}
		}

		CalculationTypes calculationType;
		public CalculationTypes CalculationType
		{
			get { return calculationType; }
			set
			{
				calculationType = value;
			}
		}

		int option;
		public int Option
		{
			get { return option; }
			set
			{
				option = value;
			}
		}
	}

	public static class TableOfGroupsOfParameters
	{
		public static GroupsOfParameters Get_FOnly_ParametersTableStandard1(ICakeFormationProcess cakeFormationProcess)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			res.Option = 1;

			AddGroup2Table(TableType.FOnly_ParametersTableStandard1, ref res, 1, new KeyValuePair<Parameter, bool>(cakeFormationProcess.CakeFormation.Filter.Area, true));

			AddGroup2Table(TableType.FOnly_ParametersTableStandard1, ref res, 2, new KeyValuePair<Parameter, bool>(cakeFormationProcess.CakeFormation.PressureDifferenceCakeFormation, true));

			AddGroup2Table(TableType.FOnly_ParametersTableStandard1, ref res,3 , new KeyValuePair<Parameter, bool>(cakeFormationProcess.CakeFormation.Cake.InitialHeight, true));

			AddGroup2Table(TableType.FOnly_ParametersTableStandard1, ref res, 4 , new KeyValuePair<Parameter, bool>(cakeFormationProcess.CakeFormation.FiltrationTime, true), new KeyValuePair<Parameter, bool>(cakeFormationProcess.CakeFormation.Cake.CakeHeigth, true));

			return res;
		}

		public static GroupsOfParameters Get_F_W_D_ParametersTableStandard1(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			res.Option = 1;

			DisplayParameter A = new DisplayParameter(f_w_d_process.CakeFormation.Filter.Area, 1, 1, true);
			res.Add(1, new List<DisplayParameter>() { A });

			DisplayParameter Dp = new DisplayParameter(f_w_d_process.CakeFormation.PressureDifferenceCakeFormation, 2, 1, true);
			res.Add(2, new List<DisplayParameter>() { Dp });

			DisplayParameter hc0 = new DisplayParameter(f_w_d_process.CakeFormation.Cake.InitialHeight, 3, 1, true);
			res.Add(3, new List<DisplayParameter>() { hc0 });

			DisplayParameter tf = new DisplayParameter(f_w_d_process.CakeFormation.FiltrationTime, 4, 1, true);
			DisplayParameter hc = new DisplayParameter(f_w_d_process.CakeFormation.Cake.CakeHeigth, 4, 2, true);
			DisplayParameter Msus = new DisplayParameter(f_w_d_process.CakeFormation.MassOfSuspension, 4, 3, true);
			res.Add(4, new List<DisplayParameter>() { tf, hc, Msus });


			DisplayParameter Dpw = new DisplayParameter(f_w_d_process.Washing.PressureDifferenceCakeWashing, 5, 1, true);
			res.Add(5, new List<DisplayParameter>() { Dpw });

			DisplayParameter tw = new DisplayParameter(f_w_d_process.Washing.WashingTime, 6, 1, true);
			DisplayParameter w = new DisplayParameter(f_w_d_process.Washing.WashingRatio, 6, 2);
			DisplayParameter Vw = new DisplayParameter(f_w_d_process.Washing.WashLiquidVolume, 6, 3);
			res.Add(6, new List<DisplayParameter>() { tw, w, Vw });

			DisplayParameter Dpd = new DisplayParameter(f_w_d_process.Deliquoring.PressureDifferenceCakeDeliquoring, 7, 1, true);
			res.Add(7, new List<DisplayParameter>() { Dpd });

			DisplayParameter td = new DisplayParameter(f_w_d_process.Deliquoring.DeliquoringTime, 8, 1, true);
			DisplayParameter K = new DisplayParameter(f_w_d_process.Deliquoring.DeliquoringIndex, 8, 2);
			DisplayParameter S = new DisplayParameter(f_w_d_process.Deliquoring.CakeSaturation, 8, 3);
			DisplayParameter Rf = new DisplayParameter(f_w_d_process.Deliquoring.CakeMoistureContent, 8, 4);
			res.Add(8, new List<DisplayParameter>() { td, K, S, Rf });

			DisplayParameter t_tech = new DisplayParameter(f_w_d_process.ResultParameter.TechnicalTime, 9, 1, true);
			DisplayParameter tc = new DisplayParameter(f_w_d_process.ResultParameter.CycleTime, 9, 2, true);
			DisplayParameter Qms = new DisplayParameter(f_w_d_process.ResultParameter.SolidsThroughput, 9, 3, true);
			res.Add(9, new List<DisplayParameter>() { t_tech, tc, Qms });

			return res;
		}

		enum TableType { Materials_ParametersTable, FOnly_ParametersTableStandard1, Washing_ParametersTable, Deliquoring_ParametersTable, Result_ParametersTable }

		static void AddGroup2Table(TableType tableType, ref GroupsOfParameters groupOfParam, int group, params KeyValuePair<Parameter, bool>[] parameters)
		{
			List<DisplayParameter> paramInGroup = new List<DisplayParameter>();
			int subGroupNum = 1;
			foreach (KeyValuePair<Parameter, bool> param in parameters)
			{
				DisplayParameter dp = new DisplayParameter(param.Key, group, subGroupNum, param.Value);
				paramInGroup.Add(dp);
				dp.paramInGroup = groupOfParam;
				subGroupNum++;
			}


			for(int i=parameters.Length-1;i>=0;i--)
			{
				switch (tableType)
				{
					case TableType.Materials_ParametersTable:
						Parameter.MaterialParametersPropertyChangedStatic += parameters[i].Key.DependentParametersChanged;
						break;
					case TableType.FOnly_ParametersTableStandard1:
						Parameter.CakeFormationPropertyChangedStatic += parameters[i].Key.DependentParametersChanged;
						break;
					case TableType.Washing_ParametersTable:
						Parameter.WashingPropertyChangedStatic += parameters[i].Key.DependentParametersChanged;
						break;
					case TableType.Deliquoring_ParametersTable:
						Parameter.DeliquoringPropertyChangedStatic += parameters[i].Key.DependentParametersChanged;
						break;
					case TableType.Result_ParametersTable:
						Parameter.ResultParametersPropertyChangedStatic += parameters[i].Key.DependentParametersChanged;
						break;
					default: Parameter.PropertyChangedStatic += parameters[i].Key.DependentParametersChanged;
						break;
				}
				
			}

			groupOfParam.Add(group, paramInGroup);
		}

		public static GroupsOfParameters Get_Materials_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 1, new KeyValuePair<Parameter, bool>(f_w_d_process.CakeFormation.Suspension.MotherLiquid.MotherLiquidViscosity, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 2, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Suspension.MotherLiquid.MotherLiquidDensity, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 3, new KeyValuePair<Parameter, bool>(f_w_d_process.CakeFormation.Suspension.SolidDensity, true), new KeyValuePair<Parameter, bool>(f_w_d_process.CakeFormation.Suspension.SuspensionDensity, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 4, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Suspension.SolidsMassFraction, true), new KeyValuePair<Parameter, bool>(f_w_d_process.CakeFormation.Suspension.SolidsVolumeFraction, true), new KeyValuePair<Parameter, bool>(f_w_d_process.CakeFormation.Suspension.SolidConcentration, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 5, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Cake.StandardPorosity, true), new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Cake.Porosity, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 6, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Cake.PorosityReductionFactor, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 7, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Cake.StandardCakePermeability, true), new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Cake.CakePermeability, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 8, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Cake.Compressibility, true));

			AddGroup2Table(TableType.Materials_ParametersTable, ref res, 9, new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Filter.SpecificFilterMediumResistance, true), new KeyValuePair<Parameter, bool>( f_w_d_process.CakeFormation.Filter.MediumResistance, true));

			return res;
		}

		public static GroupsOfParameters Get_Washing_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			AddGroup2Table(TableType.Washing_ParametersTable, ref res, 1, new KeyValuePair<Parameter, bool>(f_w_d_process.Washing.PressureDifferenceCakeWashing, true));

			AddGroup2Table(TableType.Washing_ParametersTable, ref res, 2, new KeyValuePair<Parameter, bool>(f_w_d_process.Washing.CakeHeightForCakeWashing, true));

			AddGroup2Table(TableType.Washing_ParametersTable, ref res, 3, new KeyValuePair<Parameter, bool>(f_w_d_process.Washing.WashingRatio, true), new KeyValuePair<Parameter, bool>(f_w_d_process.Washing.WashLiquidVolume, true), new KeyValuePair<Parameter, bool>(f_w_d_process.Washing.WashingTime, true));

			return res;
		}

		public static GroupsOfParameters Get_Deliquoring_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			AddGroup2Table(TableType.Deliquoring_ParametersTable, ref res, 1, 
				new KeyValuePair<Parameter, bool>(f_w_d_process.Deliquoring.PressureDifferenceCakeDeliquoring, true));

			AddGroup2Table(TableType.Deliquoring_ParametersTable, ref res, 2, 
				new KeyValuePair<Parameter, bool>(f_w_d_process.Deliquoring.CakeHeightForCakeDeliquoring, true));

			AddGroup2Table(TableType.Deliquoring_ParametersTable, ref res, 3,
				new KeyValuePair<Parameter, bool>(f_w_d_process.Deliquoring.CakeSaturation, true), 
				new KeyValuePair<Parameter, bool>(f_w_d_process.Deliquoring.CakeMoistureContent, true), 
				new KeyValuePair<Parameter, bool>(f_w_d_process.Deliquoring.DeliquoringIndex, true), 
				new KeyValuePair<Parameter, bool>(f_w_d_process.Deliquoring.DeliquoringTime, true));

			return res;
		}

		public static GroupsOfParameters Get_Result_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			AddGroup2Table(TableType.Result_ParametersTable, ref res, 1, 
				new KeyValuePair<Parameter, bool>(f_w_d_process.ResultParameter.TechnicalTime, true),
				new KeyValuePair<Parameter, bool>(f_w_d_process.ResultParameter.CycleTime, true),
				new KeyValuePair<Parameter, bool>(f_w_d_process.ResultParameter.SolidsThroughput, true));

			return res;
		}
	}
}
