using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Filtering
{
	public interface IResultParameter
	{
		ResultParameter ResultParameter { get; set; }
	}

	public interface ICakeFormationProcess
	{
		CakeFormation CakeFormation { get; set; }
	}

	public interface IWashingProcess : ICakeFormationProcess
	{
		Washing Washing { get; set; }
	}

	public interface IDeliquoringProcess : IWashingProcess
	{
		Deliquoring Deliquoring { get; set; }
	}

	public interface IWashingDeliquoringProcess : IDeliquoringProcess, IResultParameter
	{

	}

	public class ResultParameter : Parameter, IWashingDeliquoringProcess
	{
		TechnicalTime technicalTime;// = new TechnicalTime();
		public TechnicalTime TechnicalTime
		{
			get
			{
				//if (technicalTime.Value == null)
				//	technicalTime = GetTechnicalTime();
				return technicalTime;
			}
			set
			{
				technicalTime = value;
				OnPropertyChanged("TechnicalTime");
			}
		}

		CycleTime cycleTime;// = new CycleTime();
		public CycleTime CycleTime
		{
			get
			{
				//if (cycleTime.Value == null)
				//	cycleTime.Value = cycleTime.GetCycleTime_Without_SolidsThroughput() ?? cycleTime.GetCycleTime_With_SolidsThroughput();
				return cycleTime;
			}
			set
			{
				cycleTime = value;
				OnPropertyChanged("CycleTime");
			}
		}

		SolidsThroughput solidsThroughput;// = new SolidsThroughput();
		public SolidsThroughput SolidsThroughput
		{
			get
			{
				//if (solidsThroughput.Value == null)
				//	solidsThroughput.Value = solidsThroughput.GetSolidsThroughput();
				return solidsThroughput;
			}
			set
			{
				solidsThroughput = value;
				OnPropertyChanged("SolidsThroughput");
			}
		}

		Washing washing;
		public Washing Washing
		{
			get => washing;
			set
			{
				washing = value;
			}
		}

		CakeFormation cakeFormation;
		public CakeFormation CakeFormation
		{
			get { return cakeFormation; }
			set
			{
				cakeFormation = value;
			}
		}

		Deliquoring deliquoring;
		public Deliquoring Deliquoring
		{
			get => deliquoring;
			set => deliquoring = value;
		}

		ResultParameter IResultParameter.ResultParameter
		{
			get => this;
			set { }
		}		

		public ResultParameter()
		{
			
		}

		public ResultParameter(IDeliquoringProcess deliquoringProcess) : this()
		{
			CakeFormation = deliquoringProcess.CakeFormation;
			Washing = deliquoringProcess.Washing;
			Deliquoring = deliquoringProcess.Deliquoring;

			
		}
	}

	public class CycleTime : Time
	{
		public CycleTime()
		{
			Name = "Cycle Time";
			SymbolSuffix = "c";
			converter = new Param2DoubleConverter<CycleTime>();
			PropertyChangedStatic += CycleTime_Without_SolidsThroughputDependentParametersChanged;
			PropertyChangedStatic += CycleTime_With_SolidsThroughputDependentParametersChanged;
		}

		public CycleTime(double? value) : this()
		{
			Value = value;
		}

		public double? GetCycleTime_Without_SolidsThroughput()
		{
			double? res;
			try
			{
				res =
					(process.CakeFormation.FiltrationTime.Value ?? 0) +
					(process.Washing.WashingTime.Value ?? 0) +
					(process.Deliquoring.DeliquoringTime.Value ?? 0) +
					(process.ResultParameter.TechnicalTime.Value ?? 0);
			}
			catch
			{
				res = null;
			}
			return res;
		}

		void CycleTime_Without_SolidsThroughputDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "CakeFormation.FiltrationTime, Washing.WashingTime, Deliquoring.DeliquoringTime, TechnicalTime";
			IfNeedThenUpdate(dependentParameters, prop, this, GetCycleTime_Without_SolidsThroughput);
		}

		public double? GetCycleTime_With_SolidsThroughput()
		{
			double? res;
			try
			{
				res = (process.CakeFormation.Filter.Area.Value ?? double.NaN) *
				(process.CakeFormation.Suspension.SolidDensity.Value ?? double.NaN) *
				(1 - (process.CakeFormation.Cake.Porosity.Value ?? double.NaN)) *
				(process.CakeFormation.Cake.CakeHeigth.Value ?? double.NaN) /
				(process.ResultParameter.SolidsThroughput.Value ?? double.NaN);
			}
			catch
			{
				res = null;
			}
			return res;
		}

		void CycleTime_With_SolidsThroughputDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "CakeFormation.Filter.Area, CakeFormation.Suspension.SolidDensity, CakeFormation.Cake.Porosity, CakeFormation.Cake.CakeHeigth, SolidsThroughput";

			IfNeedThenUpdate(dependentParameters, prop, this, GetCycleTime_With_SolidsThroughput);
		}
	}

	public class FilteringProcess : IWashingDeliquoringProcess
	{
		CakeFormation cakeFormation;
		public CakeFormation CakeFormation
		{
			get { return cakeFormation; }
			set
			{
				cakeFormation = value;
				//OnPropertyChanged("CakeFormation");
			}
		}

		Washing washing;
		public Washing Washing
		{
			get { return washing; }
			set
			{
				washing = value;
				//OnPropertyChanged("Washing");
			}
		}

		Deliquoring deliquoring;
		public Deliquoring Deliquoring
		{
			get { return deliquoring; }
			set
			{
				deliquoring = value;
				//OnPropertyChanged("Deliquoring");
			}
		}

		ResultParameter resultParameter = new ResultParameter();
		public ResultParameter ResultParameter
		{
			get => resultParameter;
			set
			{
				resultParameter = value;
			}
		}

		public FilteringProcess()
		{


		}

		public FilteringProcess(ResultParameter resultParameter, CakeFormation cakeFormation, Washing washing = null, Deliquoring deliquoring = null) : this()
		{
			CakeFormation = cakeFormation;
			Washing = washing;
			Deliquoring = deliquoring;
			ResultParameter = resultParameter;

			Parameter.process = this;
			ResultParameter.ThatContainsObj = this;
			CakeFormation.SetAllThatContainsObj();
			Washing.SetAllThatContainsObj();
			Deliquoring.SetAllThatContainsObj();
			ResultParameter.SetAllThatContainsObj();
		}

		public Grid GetParametersTable(Func<IWashingDeliquoringProcess, GroupsOfParameters> func)
		{
			IWashingDeliquoringProcess obj = this;
			if (obj == null) return null;
			Grid res = new Grid();

			#region Шапка 
			//---------- Шапка ------------------------\\
			ColumnDefinition cd0 = new ColumnDefinition();
			cd0.Width = new System.Windows.GridLength(0.5, System.Windows.GridUnitType.Star);

			ColumnDefinition cd1 = new ColumnDefinition();
			cd1.Width = new System.Windows.GridLength(3, System.Windows.GridUnitType.Star);

			ColumnDefinition cd2 = new ColumnDefinition();
			cd2.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);

			ColumnDefinition cd3 = new ColumnDefinition();
			cd3.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);

			res.ColumnDefinitions.Add(cd0);
			res.ColumnDefinitions.Add(cd1);
			res.ColumnDefinitions.Add(cd2);
			res.ColumnDefinitions.Add(cd3);

			RowDefinition Rd = new RowDefinition();
			res.RowDefinitions.Add(Rd);

			Label lb1 = new Label();
			lb1.Background = new SolidColorBrush(SystemColors.GradientInactiveCaptionColor);
			lb1.Content = "Parameter";
			lb1.BorderThickness = new Thickness(1, 1, 0, 1);
			lb1.BorderBrush = Brushes.Black;
			Grid.SetColumn(lb1, 1);
			Grid.SetRow(lb1, 0);

			Label lb0 = new Label();
			lb0.Background = lb1.Background;
			lb0.Content = "Gr.";
			lb0.BorderThickness = new Thickness(1, 1, 0, 1);
			lb0.BorderBrush = Brushes.Black;
			Grid.SetColumn(lb0, 0);
			Grid.SetRow(lb0, 0);

			Label lb2 = new Label();
			lb2.Background = lb1.Background;
			lb2.Content = "Unit";
			lb2.BorderThickness = new Thickness(1, 1, 0, 1);
			lb2.BorderBrush = Brushes.Black;
			Grid.SetColumn(lb2, 2);
			Grid.SetRow(lb2, 0);

			Label lb3 = new Label();
			lb3.Background = lb1.Background;
			lb3.Content = "Value";
			lb3.BorderThickness = new Thickness(1, 1, 1, 1);
			lb3.BorderBrush = Brushes.Black;
			Grid.SetColumn(lb3, 3);
			Grid.SetRow(lb3, 0);

			res.Children.Add(lb0);
			res.Children.Add(lb1);
			res.Children.Add(lb2);
			res.Children.Add(lb3);
			//---------- Шапка ------------------------\\
#endregion
			SolidColorBrush backColor;

			void AddRowToRes(DisplayParameter dispParam)
			{
				Parameter param = dispParam.Parameter;
				Parameter parent = dispParam.Parent;



				//Binding groupNumberBinding = new Binding();
				//groupNumberBinding.Source = parent;
				//groupNumberBinding.Mode = BindingMode.OneWay;
				//groupNumberBinding.Path = new PropertyPath(param.Name + ".GroupNumber");
				//groupNumberBinding.Converter = 
				//groupNumberBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

				Binding backGroundColorBinding = new Binding();
				if (parent != null)
					backGroundColorBinding.Source = parent;
				else
					backGroundColorBinding.Source = this;

				//backGroundColorBinding.Source = param.GetType();
				backGroundColorBinding.Mode = BindingMode.OneWay;
				backGroundColorBinding.Path = new PropertyPath(
					param.GetType().Name +
					".SourceOfParameterChanging");
				backGroundColorBinding.Converter = param.SourceOfParameterChanging2BrushConverter;
				backGroundColorBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

				Border NameBorder = new Border();
				//NameBorder.Background = backColor;
				NameBorder.BorderBrush = Brushes.Black;
				NameBorder.BorderThickness = new Thickness(1, 0, 0, 1);
				NameBorder.Padding = new Thickness(0, 1, 0, 0);
				NameBorder.Margin = new Thickness(0, -1, 0, 0);

				Border UnitBorder = new Border();
				UnitBorder.BorderBrush = Brushes.Black;
				//UnitBorder.Background = backColor;
				UnitBorder.BorderThickness = new Thickness(1, 0, 0, 1);
				UnitBorder.Padding = NameBorder.Padding;
				UnitBorder.Margin = NameBorder.Margin;

				RowDefinition rd = new RowDefinition();
				res.RowDefinitions.Add(rd);


				TextBlock TextBlockName = new TextBlock();
				TextBlockName.Background = backColor;
				TextBlockName.Margin = new Thickness(0, 0, 0, 0);
				TextBlockName.TextWrapping = TextWrapping.Wrap;
				TextBlockName.TextTrimming = TextTrimming.None;
				string parentName = "";
				if (parent != null && !param.Name.Contains(parent.GetType().Name))
					parentName = parent.GetType().Name + " ";
				TextBlockName.Text = /*parentName +*/  param.Name + " [" + param.Symbol + "]";
				TextOptions.SetTextFormattingMode(TextBlockName, TextFormattingMode.Display);
				Grid.SetColumn(NameBorder, 1);
				Grid.SetRow(NameBorder, res.RowDefinitions.Count - 1); //param.GroupNumber - 1);

				TextBlock TextBlockUnit = new TextBlock();
				TextBlockUnit.Text = param.Unit;
				TextBlockUnit.Background = backColor;
				TextBlockUnit.TextAlignment = TextAlignment.Center;
				TextOptions.SetTextFormattingMode(TextBlockUnit, TextFormattingMode.Display);
				Grid.SetColumn(UnitBorder, 2);
				Grid.SetRow(UnitBorder, res.RowDefinitions.Count - 1);//param.GroupNumber - 1); 

				Binding binding = new Binding();
				binding.Source = parent;
				binding.Path = new PropertyPath(param.GetType().Name);
				binding.Converter = param.Converter;
				//binding.Delay = 50;
				binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

				TextBox TextBoxValue = new TextBox();
				TextBoxValue.BorderThickness = new Thickness(1, 0, 1, 1);
				TextBoxValue.BorderBrush = Brushes.Black;
				TextBoxValue.Foreground = Brushes.Blue;
				TextBoxValue.Background = backColor;
				TextBoxValue.SetBinding(TextBox.TextProperty, binding);
				//TextBoxValue.SetBinding(TextBox.BackgroundProperty, backGroundColorBinding);
				TextOptions.SetTextFormattingMode(TextBoxValue, TextFormattingMode.Display);
				//TextBoxValue
				Grid.SetColumn(TextBoxValue, 3);
				Grid.SetRow(TextBoxValue, res.RowDefinitions.Count - 1);// param.GroupNumber - 1);


				NameBorder.Child = TextBlockName;
				res.Children.Add(NameBorder);
				//res.Children.Add(TextBlockName);
				UnitBorder.Child = TextBlockUnit;
				res.Children.Add(UnitBorder);
				//res.Children.Add(TextBlockUnit);
				res.Children.Add(TextBoxValue);
			}



			foreach (KeyValuePair<int, List<DisplayParameter>> gr_dp in func(obj))
			//TableOfGroupsOfParameters.Get_Materials_ParametersTable(obj))
			{
				if (gr_dp.Key % 2 == 0)
					backColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AFd0FF2f"));
				else
					backColor = Brushes.GreenYellow;

				Border GroupBorder = new Border();
				TextBlock TextBlockGroupN = new TextBlock();
				//GroupBorder.Background = backColor;
				GroupBorder.BorderBrush = Brushes.Black;
				GroupBorder.BorderThickness = new Thickness(1, 0, 0, 1);
				GroupBorder.Padding = new Thickness(0, 1, 0, 0);
				GroupBorder.Margin = new Thickness(0, -1, 0, 0);

				TextBlockGroupN.Text = gr_dp.Key.ToString();
				TextBlockGroupN.Margin = new Thickness(0, 0, 0, 0);
				TextBlockGroupN.Padding = new Thickness(1, 1, 1, 1);
				TextBlockGroupN.Background = backColor;
				TextBlockGroupN.TextAlignment = TextAlignment.Center;
				
				TextOptions.SetTextFormattingMode(TextBlockGroupN, TextFormattingMode.Display);
				Grid.SetColumn(GroupBorder, 0);
				Grid.SetRow(GroupBorder, res.RowDefinitions.Count); //param.GroupNumber - 1);


				foreach (DisplayParameter dp in gr_dp.Value)
				{
					AddRowToRes(dp);
				}

				Grid.SetRowSpan(GroupBorder, gr_dp.Value.Count);
				

				GroupBorder.Child = TextBlockGroupN;
				res.Children.Add(GroupBorder);
			}

			return res;
		}
	}

	public class TechnicalTime : Time
	{
		public TechnicalTime()
		{
			Name = "Technical Time";
			SymbolSuffix = "_tech";
			converter = new Param2DoubleConverter<TechnicalTime>();
			PropertyChangedStatic += TechnicalTimeDependentParametersChanged;
		}

		public TechnicalTime(double? value) : this()
		{
			Value = value;
		}

		public double? GetTechnicalTime()
		{
			double? res;
			try
			{
				res = (process.ResultParameter.CycleTime.Value ?? 0) -
					(
					(process.CakeFormation.FiltrationTime.Value ?? 0) +
					(process.Washing.WashingTime.Value ?? 0) +
					(process.Deliquoring.DeliquoringTime.Value ?? 0)
					);
			}
			catch
			{
				res = null;
			}
			return res;
		}

		void TechnicalTimeDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "CycleTime, CakeFormation.FiltrationTime, Washing.WashingTime, Deliquoring.DeliquoringTime";

			IfNeedThenUpdate(dependentParameters, prop, this, GetTechnicalTime);
		}
	}

	public class SolidsThroughput : Parameter
	{
		public SolidsThroughput()
		{
			Name = "Solids throughput";
			Symbol = "Qms";
			Unit = "kgs-1";
			converter = new Param2DoubleConverter<SolidsThroughput>();
			PropertyChangedStatic += SolidsThroughputDependentParametersChanged;
		}

		public SolidsThroughput(double? value) : this()
		{
			Value = value;
		}

		public double? GetSolidsThroughput()
		{
			double? res;
			try
			{
				res = (process.CakeFormation.Filter.Area.Value ?? double.NaN) *
				(process.CakeFormation.Suspension.SolidDensity.Value ?? double.NaN) *
				(1 - (process.CakeFormation.Cake.Porosity.Value ?? double.NaN)) *
				(process.CakeFormation.Cake.CakeHeigth.Value ?? double.NaN) /
				(process.ResultParameter.CycleTime.Value ?? double.NaN);
			}
			catch
			{
				res = null;
			}
			return res;
		}

		void SolidsThroughputDependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			string dependentParameters = "CycleTime, CakeFormation.Filter.Area, CakeFormation.Suspension.SolidDensity, CakeFormation.Cake.Porosity, CakeFormation.Cake.CakeHeigth";

			IfNeedThenUpdate(dependentParameters, prop, this, GetSolidsThroughput);

		}
	}
}
