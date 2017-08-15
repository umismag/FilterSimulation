using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		Parameter parent;
		public Parameter Parent
		{
			get { return parent; }
			set
			{
				parent = value;
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

		public DisplayParameter(Parameter parameter, Parameter parent, int groupNumber, int subGroupNumber, bool isAlwaysDisplay = false)
		{
			Parameter = parameter;
			Parent = parent;
			GroupNumber = groupNumber;
			SubGroupNumber = subGroupNumber;
			IsAlwaysDisplay = isAlwaysDisplay;
		}
	}

	public enum CalculationTypes { StandardCalculation, FilterDesign }


	public class GroupsOfParameters : SortedList<int, List<DisplayParameter>>
	{
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

		//public GroupsOfParameters()
		//{

		//}
	}

	public static class TableOfGroupsOfParameters
	{
		public static GroupsOfParameters Get_FOnly_ParametersTableStandard1(ICakeFormationProcess cakeFormationProcess)
		{
			GroupsOfParameters res= new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			res.Option = 1;

			DisplayParameter A = new DisplayParameter(cakeFormationProcess.CakeFormation.Filter.Area, cakeFormationProcess.CakeFormation.Filter, 1, 1, true);
			res.Add(1, new List<DisplayParameter>() { A});

			DisplayParameter Dp = new DisplayParameter(cakeFormationProcess.CakeFormation.PressureDifferenceCakeFormation, cakeFormationProcess.CakeFormation, 2, 1, true);
			res.Add(2, new List<DisplayParameter>() { Dp });

			DisplayParameter hc0 = new DisplayParameter(cakeFormationProcess.CakeFormation.Cake.InitialHeight, cakeFormationProcess.CakeFormation.Cake, 3, 1, true);
			res.Add(3, new List<DisplayParameter>() { hc0 });

			DisplayParameter tf = new DisplayParameter(cakeFormationProcess.CakeFormation.FiltrationTime, cakeFormationProcess.CakeFormation, 4, 1, true);
			DisplayParameter hc = new DisplayParameter(cakeFormationProcess.CakeFormation.Cake.CakeHeigth, cakeFormationProcess.CakeFormation.Cake,   4, 2, true);
			res.Add(4, new List<DisplayParameter>() { tf,hc });

			//DisplayParameter t_tech = new DisplayParameter(cakeFormationProcess.ResultParameter.TechnicalTime, cakeFormationProcess.ResultParameter,			5, 1 , true);
			//DisplayParameter tc = new DisplayParameter(cakeFormationProcess.ResultParameter.CycleTime, cakeFormationProcess.ResultParameter,			5, 2, true);
			//DisplayParameter Qms = new DisplayParameter(cakeFormationProcess.ResultParameter.SolidsThroughput, cakeFormationProcess.ResultParameter,			5, 3, true);
			//res.Add(5, new List<DisplayParameter>() {t_tech, tc, Qms});

			
			return res;
		}

		public static GroupsOfParameters Get_F_W_D_ParametersTableStandard1(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			res.Option = 1;

			DisplayParameter A = new DisplayParameter(f_w_d_process.CakeFormation.Filter.Area, f_w_d_process.CakeFormation.Filter, 1, 1, true);
			res.Add(1, new List<DisplayParameter>() { A });

			DisplayParameter Dp = new DisplayParameter(f_w_d_process.CakeFormation.PressureDifferenceCakeFormation, f_w_d_process.CakeFormation, 2, 1, true);
			res.Add(2, new List<DisplayParameter>() { Dp });

			DisplayParameter hc0 = new DisplayParameter(f_w_d_process.CakeFormation.Cake.InitialHeight, f_w_d_process.CakeFormation.Cake, 3, 1, true);
			res.Add(3, new List<DisplayParameter>() { hc0 });

			DisplayParameter tf = new DisplayParameter(f_w_d_process.CakeFormation.FiltrationTime, f_w_d_process.CakeFormation, 4, 1, true);
			DisplayParameter hc = new DisplayParameter(f_w_d_process.CakeFormation.Cake.CakeHeigth, f_w_d_process.CakeFormation.Cake,        4, 2, true);
			DisplayParameter Msus = new DisplayParameter(f_w_d_process.CakeFormation.MassOfSuspension, f_w_d_process.CakeFormation, 4, 3, true);
			res.Add(4, new List<DisplayParameter>() { tf, hc, Msus });


			DisplayParameter Dpw = new DisplayParameter(f_w_d_process.Washing.PressureDifferenceCakeWashing, f_w_d_process.Washing, 5, 1, true);
			res.Add(5, new List<DisplayParameter>() { Dpw });

			DisplayParameter tw = new DisplayParameter(f_w_d_process.Washing.WashingTime, f_w_d_process.Washing,   6, 1, true);
			DisplayParameter w = new DisplayParameter(f_w_d_process.Washing.WashingRatio, f_w_d_process.Washing,   6, 2);
			DisplayParameter Vw = new DisplayParameter(f_w_d_process.Washing.WashLiquidVolume, f_w_d_process.Washing, 6, 3);
			res.Add(6, new List<DisplayParameter>() { tw,w, Vw });

			DisplayParameter Dpd = new DisplayParameter(f_w_d_process.Deliquoring.PressureDifferenceCakeDeliquoring, f_w_d_process.Deliquoring, 7, 1, true);
			res.Add(7, new List<DisplayParameter>() { Dpd });

			DisplayParameter td = new DisplayParameter(f_w_d_process.Deliquoring.DeliquoringTime, f_w_d_process.Deliquoring,   8, 1, true);
			DisplayParameter K = new DisplayParameter(f_w_d_process.Deliquoring.DeliquoringIndex, f_w_d_process.Deliquoring,   8, 2);
			DisplayParameter S = new DisplayParameter(f_w_d_process.Deliquoring.CakeSaturation, f_w_d_process.Deliquoring,       8, 3);
			DisplayParameter Rf = new DisplayParameter(f_w_d_process.Deliquoring.CakeMoistureContent, f_w_d_process.Deliquoring, 8, 4);
			res.Add(8, new List<DisplayParameter>() { td, K, S, Rf });

			DisplayParameter t_tech = new DisplayParameter(f_w_d_process.ResultParameter.TechnicalTime, f_w_d_process.ResultParameter,	9, 1, true);
			DisplayParameter tc = new DisplayParameter(f_w_d_process.ResultParameter.CycleTime, f_w_d_process.ResultParameter,			9, 2, true);
			DisplayParameter Qms = new DisplayParameter(f_w_d_process.ResultParameter.SolidsThroughput, f_w_d_process.ResultParameter,	9, 3, true);
			res.Add(9, new List<DisplayParameter>() { t_tech, tc, Qms });

			return res;
		}

		public static GroupsOfParameters Get_Materials_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			DisplayParameter Etaf = new DisplayParameter(f_w_d_process.CakeFormation.Suspension.MotherLiquid.MotherLiquidViscosity, f_w_d_process.CakeFormation.Suspension.MotherLiquid, 1, 1, true);
			res.Add(1, new List<DisplayParameter>() {Etaf});

			DisplayParameter Rhof = new DisplayParameter(f_w_d_process.CakeFormation.Suspension.MotherLiquid.MotherLiquidDensity, f_w_d_process.CakeFormation.Suspension.MotherLiquid, 2, 1, true);
			res.Add(2, new List<DisplayParameter>() { Rhof });

			DisplayParameter Rhos= new DisplayParameter(f_w_d_process.CakeFormation.Suspension.SolidDensity, f_w_d_process.CakeFormation.Suspension, 3, 1, true);
			DisplayParameter Rhosus = new DisplayParameter(f_w_d_process.CakeFormation.Suspension.SuspensionDensity, f_w_d_process.CakeFormation.Suspension, 3, 2, true);
			res.Add(3, new List<DisplayParameter>() { Rhos,Rhosus });

			DisplayParameter Cm= new DisplayParameter(f_w_d_process.CakeFormation.Suspension.SolidsMassFraction, f_w_d_process.CakeFormation.Suspension, 4, 1, true);
			DisplayParameter Cv = new DisplayParameter(f_w_d_process.CakeFormation.Suspension.SolidsVolumeFraction, f_w_d_process.CakeFormation.Suspension, 4, 2, true);
			DisplayParameter C = new DisplayParameter(f_w_d_process.CakeFormation.Suspension.SolidConcentration, f_w_d_process.CakeFormation.Suspension, 4, 3, true);
			res.Add(4, new List<DisplayParameter>() { Cm, Cv, C });

			DisplayParameter Eps0 = new DisplayParameter(f_w_d_process.CakeFormation.Cake.StandardPorosity, f_w_d_process.CakeFormation.Cake, 5, 1, true);
			DisplayParameter Eps = new DisplayParameter(f_w_d_process.CakeFormation.Cake.Porosity, f_w_d_process.CakeFormation.Cake, 5, 2, true);
			res.Add(5, new List<DisplayParameter>() { Eps0, Eps });

			DisplayParameter ne = new DisplayParameter(f_w_d_process.CakeFormation.Cake.PorosityReductionFactor, f_w_d_process.CakeFormation.Cake, 6, 1, true);
			res.Add(6, new List<DisplayParameter>() { ne });

			DisplayParameter Pc0 = new DisplayParameter(f_w_d_process.CakeFormation.Cake.StandardCakePermeability, f_w_d_process.CakeFormation.Cake, 7, 1, true);
			DisplayParameter Pc = new DisplayParameter(f_w_d_process.CakeFormation.Cake.CakePermeability, f_w_d_process.CakeFormation.Cake, 7, 2, true);
			res.Add(7, new List<DisplayParameter>() { Pc0, Pc });

			DisplayParameter nc = new DisplayParameter(f_w_d_process.CakeFormation.Cake.Compressibility, f_w_d_process.CakeFormation.Cake, 8, 1, true);
			res.Add(8, new List<DisplayParameter>() { nc });

			DisplayParameter hce = new DisplayParameter(f_w_d_process.CakeFormation.Filter.SpecificFilterMediumResistance, f_w_d_process.CakeFormation.Filter, 9, 1, true);
			DisplayParameter Rm = new DisplayParameter(f_w_d_process.CakeFormation.Filter.MediumResistance, f_w_d_process.CakeFormation.Filter, 9, 2, true);
			res.Add(9, new List<DisplayParameter>() { hce, Rm });

			return res;
		}

		public static GroupsOfParameters Get_Washing_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			DisplayParameter Dpw = new DisplayParameter(f_w_d_process.Washing.PressureDifferenceCakeWashing, f_w_d_process.Washing, 1, 1, true);
			res.Add(1, new List<DisplayParameter>(){ Dpw});

			DisplayParameter hcw = new DisplayParameter(f_w_d_process.Washing.CakeHeightForCakeWashing, f_w_d_process.Washing, 2, 1, true);
			res.Add(2, new List<DisplayParameter>(){ hcw });

			DisplayParameter w = new DisplayParameter(f_w_d_process.Washing.WashingRatio, f_w_d_process.Washing, 3, 1, true);
			DisplayParameter Vw = new DisplayParameter(f_w_d_process.Washing.WashLiquidVolume, f_w_d_process.Washing, 3, 2, true);
			DisplayParameter tw = new DisplayParameter(f_w_d_process.Washing.WashingTime, f_w_d_process.Washing, 3, 3, true);
			res.Add(3, new List<DisplayParameter>() { w, Vw, tw});

			return res;
		}

		public static GroupsOfParameters Get_Deliquoring_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			DisplayParameter Dpd = new DisplayParameter(f_w_d_process.Deliquoring.PressureDifferenceCakeDeliquoring, f_w_d_process.Deliquoring, 1, 1, true);
			res.Add(1, new List<DisplayParameter>() { Dpd });

			DisplayParameter hcd = new DisplayParameter(f_w_d_process.Deliquoring.CakeHeightForCakeDeliquoring, f_w_d_process.Deliquoring, 2, 1, true);
			res.Add(2, new List<DisplayParameter>() { hcd });

			DisplayParameter S = new DisplayParameter(f_w_d_process.Deliquoring.CakeSaturation, f_w_d_process.Deliquoring, 3, 1, true);
			DisplayParameter Rf = new DisplayParameter(f_w_d_process.Deliquoring.CakeMoistureContent, f_w_d_process.Deliquoring, 3, 2, true);
			DisplayParameter K = new DisplayParameter(f_w_d_process.Deliquoring.DeliquoringIndex, f_w_d_process.Deliquoring, 3, 3, true);
			DisplayParameter td = new DisplayParameter(f_w_d_process.Deliquoring.DeliquoringTime, f_w_d_process.Deliquoring, 3, 4, true);
			res.Add(3, new List<DisplayParameter>() { S, Rf, K,td });

			return res;
		}

		public static GroupsOfParameters Get_Result_ParametersTable(IWashingDeliquoringProcess f_w_d_process)
		{
			GroupsOfParameters res = new GroupsOfParameters();
			res.CalculationType = CalculationTypes.StandardCalculation;
			//res.Option = 1;

			DisplayParameter t_tech = new DisplayParameter(f_w_d_process.ResultParameter.TechnicalTime, f_w_d_process.ResultParameter, 1, 1, true);
			DisplayParameter tc = new DisplayParameter(f_w_d_process.ResultParameter.CycleTime, f_w_d_process.ResultParameter, 1, 2, true);
			DisplayParameter Qms = new DisplayParameter(f_w_d_process.ResultParameter.SolidsThroughput, f_w_d_process.ResultParameter, 1, 3, true);
			res.Add(1, new List<DisplayParameter>() { t_tech, tc, Qms });

			return res;
		}
	}
}
