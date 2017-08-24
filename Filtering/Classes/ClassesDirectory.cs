using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Data;
using System.Globalization;

namespace Filtering
{
	public interface IParameterDefinition
	{
		string Name { get; }
		string Unit { get; }
		string Symbol { get; }
	}

	//public interface IParameter
	//{
	//	IParameterDefinition Parameter { get; }			
	//	string Value { get; set; }
	//}

	public enum SourceOfChanging
	{
		ManuallyByUser,
		AutomaticallyByCore,
		AutomaticallyByConstructor
	}

	public abstract class Parameter :
										IParameterDefinition,
										INotifyPropertyChanged //Класс изменяемого объекта должен реализовывать интерфейс INotifyPropertyChanged - https://metanit.com/sharp/wpf/11.2.php 
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public static event PropertyChangedEventHandler PropertyChangedStatic;

		public static event PropertyChangedEventHandler MaterialParametersPropertyChangedStatic;
		public static event PropertyChangedEventHandler CakeFormationPropertyChangedStatic;
		public static event PropertyChangedEventHandler WashingPropertyChangedStatic;
		public static event PropertyChangedEventHandler DeliquoringPropertyChangedStatic;
		public static event PropertyChangedEventHandler ResultParametersPropertyChangedStatic;

		public void OnPropertyChanged(/*[CallerMemberName]*/string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));

			MaterialParametersPropertyChangedStatic?.Invoke(this, new PropertyChangedEventArgs(prop));
			CakeFormationPropertyChangedStatic?.Invoke(this, new PropertyChangedEventArgs(prop));
			WashingPropertyChangedStatic?.Invoke(this, new PropertyChangedEventArgs(prop));
			DeliquoringPropertyChangedStatic?.Invoke(this, new PropertyChangedEventArgs(prop));
			ResultParametersPropertyChangedStatic?.Invoke(this, new PropertyChangedEventArgs(prop));
			PropertyChangedStatic?.Invoke(this, new PropertyChangedEventArgs(prop));
		}

		public static IWashingDeliquoringProcess process;
		public object ThatContainsObj;

		public string dependentParameters, dependent2Parameters;

		public virtual double? GetUpdatedParameter() { return null; }
		public virtual double? Get2UpdatedParameter() { return null; }

		public virtual void DependentParametersChanged(object sender, PropertyChangedEventArgs prop)
		{
			if (dependentParameters != null)
				IfNeedThenUpdate(dependentParameters, prop, this, GetUpdatedParameter);
			if (dependent2Parameters != null)
				IfNeedThenUpdate(dependent2Parameters, prop, this, Get2UpdatedParameter);
		}

		SourceOfChanging? sourceOfParameterChanging = SourceOfChanging.AutomaticallyByConstructor;
		public SourceOfChanging? SourceOfParameterChanging
		{
			get { return sourceOfParameterChanging; }
			set
			{
				sourceOfParameterChanging = value;
				//OnPropertyChanged("SourceOfParameterChanging");
			}
		}

		protected SourceOfChanging? sourceOfMinMaxChanging = SourceOfChanging.AutomaticallyByConstructor;

		internal string name;
		public string Name
		{
			get { return name; }
			internal set { name = value; }
		}

		string unit;
		public string Unit
		{
			get { return unit; }
			internal set { unit = value; }
		}

		string symbol;
		public string Symbol
		{
			get { return symbol + SymbolSuffix; }
			internal set { symbol = value; }
		}

		string symbolSuffix = string.Empty;
		public string SymbolSuffix
		{
			get { return symbolSuffix; }
			internal set { symbolSuffix = value; }
		}

		double? minValue;// = double.MinValue;
		public double? MinValue
		{
			get { return minValue; }
			set { minValue = value; }
		}

		double? maxValue;// = double.MaxValue;
		public double? MaxValue
		{
			get { return maxValue; }
			set { maxValue = value; }
		}

		protected static CalculateMode calculateMode = CalculateMode.Normal;

		protected double? value = null;
		public double? Value
		{
			get
			{
				switch (calculateMode)
				{
					case CalculateMode.Normal:
						return this.value;
						//break;
					case CalculateMode.Min:
						if (SourceOfParameterChanging == SourceOfChanging.ManuallyByUser)
							return value;
						else
							return MinValue;
						//break;
					case CalculateMode.Max:
						if (SourceOfParameterChanging == SourceOfChanging.ManuallyByUser)
							return value;
						else
							return MaxValue;
						//break;
					default:
						return this.value;
						//break;
				}
			}
			set
			{
				if (value < 0) //minValue)
					this.value = null; //minValue;
				else if (value > 10000) //maxValue)
					this.value = null; //maxValue;
				else
					this.value = value;
				//OnPropertyChanged(this.GetType().Name);// +".Value");
				//callersCount++;
			}
		}

		public override string ToString()
		{
			return Value.ToString();
		}

		public void SetAllThatContainsObj()
		{
			Type T = GetType();
			PropertyInfo[] pis = T.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo pi in pis)
			{
				if (pi.PropertyType.IsSubclassOf(typeof(Parameter)))
				{
					if ((Parameter)pi.GetValue(this) == null)
					{
						Type ct = pi.PropertyType;
						ConstructorInfo ci = ct.GetConstructor(Type.EmptyTypes);
						MethodInfo mi = pi.SetMethod;
						mi.Invoke(this, new object[] { ci.Invoke(Type.EmptyTypes) });
					}
					((Parameter)pi.GetValue(this)).ThatContainsObj = this;
					((Parameter)pi.GetValue(this)).SetAllThatContainsObj();
				}
			}
		}

		protected enum CalculateMode { Normal, Min, Max};

		protected Parameter()
		{
			SourceOfParameterChanging2BrushConverter = new SourceOfParameterChanging2BrushConverterClass();
		}

		protected IValueConverter converter = null;
		public IValueConverter Converter
		{
			get { return converter; }
		}

		public class Param2DoubleConverter<T> : IValueConverter where T : Parameter, new()
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				if (value is T tmp)
					return tmp.Value;
				else
					return value;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				bool IsNeedConvert = (value != null) && parameter is DisplayParameter && (value.ToString() != "") && value.ToString()[value.ToString().Length - 1] != '.' && value.ToString()[value.ToString().Length - 1] != ',';
				if (IsNeedConvert)
				{
					DisplayParameter dispParam = parameter as DisplayParameter;
					double res;
					if (double.TryParse(value.ToString().Replace('.', ','), out res))
					{
						dispParam.Parameter.Value = res;

					}
					else
					{
						dispParam.Parameter.Value = null;
					}
					//dispParam.Parameter.SourceOfParameterChanging = SourceOfChanging.ManuallyByUser;
					dispParam.paramInGroup.SetRepresentateForGroup(dispParam);
					return dispParam.Parameter;
				}
				else
					return value;
			}
		}

		protected IValueConverter sourceOfParameterChanging2BrushConverter = null;
		public IValueConverter SourceOfParameterChanging2BrushConverter
		{
			get { return sourceOfParameterChanging2BrushConverter; }
			internal set { sourceOfParameterChanging2BrushConverter = value; }
		}

		public class SourceOfParameterChanging2BrushConverterClass : IValueConverter
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				SourceOfChanging tmp;
				if (Enum.TryParse<SourceOfChanging>(value.ToString(), out tmp))
				{
					SolidColorBrush br = new SolidColorBrush();
					switch (tmp)
					{
						case SourceOfChanging.ManuallyByUser:
							{
								br.Color = new Color() { R = 0, G = 0, B = 255, A = 255 };
								break;
							}

						case SourceOfChanging.AutomaticallyByCore:
							{
								br.Color = new Color() { R = 0, G = 0, B = 0, A = 255 };
								//return br;
								break;
							}
						case SourceOfChanging.AutomaticallyByConstructor:
							{
								br.Color = new Color() { R = 255, G = 0, B = 0, A = 200 };
								//return br;
								break;
							}
						default:
							{
								br.Color = new Color() { R = 255, G = 0, B = 0, A = 200 };
								//return br;
								break;
							}
					}
					//(parameter as DisplayParameter).Parameter.GetType().Name);
					return br;
				}
				
				else
					return value;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				return null;
			}
		}

		static int callersCount = 0;

		protected void IfNeedThenUpdate(string dependedParametersString, PropertyChangedEventArgs prop, Parameter parameter, Func<double?> function)
		{
			callersCount++;
			Parameter obj = null;
			Type T1;
			PropertyInfo pi1;
			if (parameter != null && parameter.ThatContainsObj != null)
			{
				T1 = parameter.ThatContainsObj.GetType();
				pi1 = T1.GetProperty(parameter.GetType().Name, parameter.GetType());
				obj = pi1.GetValue(parameter.ThatContainsObj) as Parameter;
			}
			else
			{
				//obj = parameter;
				return;
			}

			bool IsNeed =
				obj != null &&
				obj.ThatContainsObj != null &&
				obj.SourceOfParameterChanging != SourceOfChanging.ManuallyByUser &&
				dependedParametersString.Contains(prop.PropertyName) &&
				function().HasValue &&
				!double.IsNaN(function().Value) &&
				!double.IsInfinity(function().Value) &&
				(!obj.Value.HasValue ||
				double.IsNaN(obj.Value.Value) ||
				double.IsInfinity(obj.Value.Value) ||
				Math.Abs((obj.Value ?? 0) - (function() ?? 0)) > double.Epsilon);

			if (IsNeed)
			{
				obj.value = function();
				(obj.ThatContainsObj as Parameter).OnPropertyChanged(obj.GetType().Name);

				if (sourceOfMinMaxChanging != SourceOfChanging.ManuallyByUser)
				{
					calculateMode = CalculateMode.Min;
					obj.MinValue = function();

					calculateMode = CalculateMode.Max;
					obj.MaxValue = function();

					calculateMode = CalculateMode.Normal;
				}
			}
			else
			{
				return;
			}
		}
	}



	public class ParametersTemplate
	{
		string parameter;
		public string Parameter
		{
			get { return parameter; }
			set { parameter = value; }
		}

		string units;
		public string Units
		{
			get { return units; }
			set { units = value; }
		}

		string value;
		public string Value
		{
			get { return value; }
			set { this.value = value; }
		}
	}

	public class ObjWithParametersList : Parameter
	{
		public List<Parameter> ParametersList = new List<Parameter>();

		public int ItemsCount
		{
			get
			{
				return ParametersList.Count;
			}
		}



		public Parameter this[int i]
		{
			get
			{
				return ParametersList[i];
			}
		}

		public void AddParameter(Parameter parameter)
		{
			if (parameter is Parameter tmp)
				ParametersList.Add(tmp);
		}
	}

	public static class MyReflection
	{
		//public static List<ParametersTemplate> PrintParameters(object obj, Parameter parentObj)
		//{
		//	Parameter tmpObj = obj as Parameter;
		//	if (tmpObj == null) return null;


		//	List<ParametersTemplate> res = new List<ParametersTemplate>();

		//	if (tmpObj.SubParameters!=null)
		//	{
		//		foreach(Parameter par in tmpObj.SubParameters.Values)
		//		{
		//			res.AddRange(PrintParameters(par,tmpObj));
		//		}

		//	}
		//	else
		//	{
		//		if (tmpObj.Unit != string.Empty)
		//		{					
		//			res.Add(new ParametersTemplate()
		//			{
		//				Parameter = ((parentObj!=null)?parentObj.Name :"") + " "+ tmpObj.Name+" ["+ tmpObj.Symbol + "]" ,
		//				Units = tmpObj.Unit,
		//				Value = tmpObj.Value.ToString()
		//			});
		//		}
		//	}

		//	return res;
		//}

		//public static Grid PrintGridWithParamList(Parameter obj, Parameter parentObject = null)
		//{
		//	if (obj == null) return null;

		//	Grid res = new Grid();

		//	if (parentObject == null)
		//	{
		//		//res.ShowGridLines = true;
		//		ColumnDefinition cd1 = new ColumnDefinition()
		//		{
		//			Width = new System.Windows.GridLength(3, System.Windows.GridUnitType.Star)
		//		};

		//		ColumnDefinition cd2 = new ColumnDefinition();
		//		cd2.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);

		//		ColumnDefinition cd3 = new ColumnDefinition();
		//		cd3.Width = new System.Windows.GridLength(2, System.Windows.GridUnitType.Star);

		//		res.ColumnDefinitions.Add(cd1);
		//		res.ColumnDefinitions.Add(cd2);
		//		res.ColumnDefinitions.Add(cd3);

		//		RowDefinition Rd = new RowDefinition();
		//		res.RowDefinitions.Add(Rd);

		//		Label lb1 = new Label()
		//		{
		//			Background = new SolidColorBrush(SystemColors.GradientInactiveCaptionColor),
		//			Content = "Parameter",
		//			BorderThickness = new Thickness(1, 1, 0, 1),
		//			BorderBrush = Brushes.Black
		//		};
		//		Grid.SetColumn(lb1, 0);
		//		Grid.SetRow(lb1, 0);

		//		Label lb2 = new Label();
		//		lb2.Background = lb1.Background;
		//		lb2.Content = "Unit";
		//		lb2.BorderThickness = new Thickness(1, 1, 1, 1);
		//		lb2.BorderBrush = Brushes.Black;
		//		Grid.SetColumn(lb2, 1);
		//		Grid.SetRow(lb2, 0);

		//		Label lb3 = new Label();
		//		lb3.Background = lb1.Background;
		//		lb3.Content = "Value";
		//		lb3.BorderThickness = new Thickness(0, 1, 1, 1);
		//		lb3.BorderBrush = Brushes.Black;
		//		Grid.SetColumn(lb3, 2);
		//		Grid.SetRow(lb3, 0);

		//		res.Children.Add(lb1);
		//		res.Children.Add(lb2);
		//		res.Children.Add(lb3);
		//	}

		//	void AddRowToRes(Parameter param, Parameter parent)
		//	{
		//		//Binding groupNumberBinding = new Binding();
		//		//groupNumberBinding.Source = parent;
		//		//groupNumberBinding.Mode = BindingMode.OneWay;
		//		//groupNumberBinding.Path = new PropertyPath(param.Name + ".GroupNumber");
		//		//groupNumberBinding.Converter = 
		//		//groupNumberBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;



		//		Binding backGroundColorBinding = new Binding();
		//		backGroundColorBinding.Source = parent;
		//		backGroundColorBinding.Mode = BindingMode.OneWay;
		//		backGroundColorBinding.Path = new PropertyPath(param.Name + ".SourceOfParameterChanging");
		//		backGroundColorBinding.Converter = param.SourceOfParameterChanging2BrushConverter;
		//		backGroundColorBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

		//		Border NameBorder = new Border();
		//		NameBorder.BorderBrush = Brushes.Black;
		//		NameBorder.BorderThickness = new Thickness(1, 0, 0, 1);
		//		NameBorder.Padding = new Thickness(2, 2, 2, 2);
		//		NameBorder.Margin = new Thickness(0, -1, 0, 0);

		//		Border UnitBorder = new Border();
		//		UnitBorder.BorderBrush = Brushes.Black;
		//		UnitBorder.BorderThickness = new Thickness(1, 0, 0, 1);
		//		UnitBorder.Padding = NameBorder.Padding;
		//		UnitBorder.Margin = NameBorder.Margin;

		//		RowDefinition rd = new RowDefinition();
		//		res.RowDefinitions.Add(rd);

		//		TextBlock TextBlockName = new TextBlock();
		//		TextBlockName.TextWrapping = TextWrapping.Wrap;
		//		TextBlockName.TextTrimming = TextTrimming.None;

		//		TextBlockName.Text = (parentObject != null ? parent.GetType().Name + " " : "") + param.Name + " [" + param.Symbol + "]";
		//		TextOptions.SetTextFormattingMode(TextBlockName, TextFormattingMode.Display);
		//		//Grid.SetColumn(TextBlockName, 0);
		//		//Grid.SetRow(TextBlockName, res.RowDefinitions.Count - 1);
		//		Grid.SetColumn(NameBorder, 0);
		//		Grid.SetRow(NameBorder, res.RowDefinitions.Count - 1); //param.GroupNumber - 1);

		//		TextBlock TextBlockUnit = new TextBlock();
		//		TextBlockUnit.Text = param.Unit;
		//		TextBlockUnit.TextAlignment = TextAlignment.Center;
		//		TextOptions.SetTextFormattingMode(TextBlockUnit, TextFormattingMode.Display);
		//		Grid.SetColumn(UnitBorder, 1);
		//		Grid.SetRow(UnitBorder, res.RowDefinitions.Count - 1);//param.GroupNumber - 1); 

		//		Binding binding = new Binding();
		//		binding.Source = parent;
		//		binding.Path = new PropertyPath(param.Name);
		//		binding.Converter = param.Converter;
		//		//binding.Delay = 300;
		//		binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

		//		TextBox TextBoxValue = new TextBox();
		//		TextBoxValue.Foreground = Brushes.Blue;
		//		TextBoxValue.SetBinding(TextBox.TextProperty, binding);
		//		TextBoxValue.SetBinding(TextBox.BackgroundProperty, backGroundColorBinding);
		//		TextOptions.SetTextFormattingMode(TextBoxValue, TextFormattingMode.Display);
		//		//TextBoxValue
		//		Grid.SetColumn(TextBoxValue, 2);
		//		Grid.SetRow(TextBoxValue, res.RowDefinitions.Count - 1);// param.GroupNumber - 1);

		//		NameBorder.Child = TextBlockName;
		//		res.Children.Add(NameBorder);
		//		//res.Children.Add(TextBlockName);
		//		UnitBorder.Child = TextBlockUnit;
		//		res.Children.Add(UnitBorder);
		//		//res.Children.Add(TextBlockUnit);
		//		res.Children.Add(TextBoxValue);

		//	}

		//	Type t = obj.GetType();

		//	PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);

		//	foreach (PropertyInfo propinfo in pi)
		//	{
		//		Parameter subObj = propinfo.GetValue(obj) as Parameter;
		//		if (subObj == null) continue;
		//		subObj.Name = propinfo.Name;

		//		bool isSubparameter = subObj.Symbol == null || subObj.Unit == null;
		//		if (isSubparameter)
		//		{
		//			List<UIElement> elements = new List<UIElement>();
		//			Grid grd = PrintGridWithParamList(subObj, obj);      //!!!!
		//			foreach (UIElement el in grd.Children)
		//			{
		//				elements.Add(el);
		//			}
		//			grd.Children.Clear();
		//			int i = 0;
		//			foreach (UIElement el in elements)
		//			{
		//				if (i % 3 == 0)
		//				{
		//					RowDefinition rd = new RowDefinition();
		//					res.RowDefinitions.Add(rd);
		//				}
		//				Grid.SetRow(el, res.RowDefinitions.Count - 1);
		//				res.Children.Add(el);
		//				i++;
		//			}

		//		}
		//		else
		//		{
		//			bool isPrintableParameter = propinfo.GetValue(obj, null) != null;
		//			if (isPrintableParameter)
		//			{
		//				AddRowToRes(subObj, obj);
		//			}
		//		}
		//	}
		//	return res;
		//}

		public static ObjWithParametersList PrintObjWithParamList(Parameter obj, Parameter parentObject = null)
		{
			if (obj == null) return null;

			ObjWithParametersList res = new ObjWithParametersList();

			Type t = obj.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);


			foreach (PropertyInfo propinfo in pi)
			{
				Parameter subObj = propinfo.GetValue(obj) as Parameter;
				if (subObj == null) continue;

				bool isSubparameter = subObj != null && (subObj.Symbol == null || subObj.Unit == null);
				if (isSubparameter)
				{
					//foreach (Parameter el in PrintObjWithParamList(subObj, obj))
					for (int i = 0;
						i < PrintObjWithParamList(subObj, obj).ItemsCount;
						i++)
						res.AddParameter(PrintObjWithParamList(subObj, obj)[i]);
				}
				else
				{
					bool isPrintableParameter = propinfo.GetValue(obj, null) != null;
					if (isPrintableParameter)
						res.AddParameter(subObj);
				}
			}
			return res;
		}

		public static ObservableCollection<Parameter> PrintParamPropObject(Parameter obj, Parameter parentObject = null)
		{
			if (obj == null) return null;

			ObservableCollection<Parameter> res = new ObservableCollection<Parameter>();

			Type t = obj.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);


			foreach (PropertyInfo propinfo in pi)
			{
				Parameter subObj = propinfo.GetValue(obj) as Parameter;
				if (subObj == null) continue;

				bool isSubparameter = subObj != null && (subObj.Symbol == null || subObj.Unit == null);
				if (isSubparameter)
				{
					foreach (Parameter el in PrintParamPropObject(subObj, obj))
						res.Add(el);
				}
				else
				{
					bool isPrintableParameter = propinfo.GetValue(obj, null) != null;
					if (isPrintableParameter)
						res.Add(subObj);
				}
			}
			return res;
		}

		public static List<ParametersTemplate> PrintParameters(Parameter obj, Parameter parentObject = null)
		{
			if (obj == null) return null;

			List<ParametersTemplate> res = new List<ParametersTemplate>();

			Type t = obj.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);


			foreach (PropertyInfo propinfo in pi)
			{
				Parameter subObj = propinfo.GetValue(obj) as Parameter;
				if (subObj == null) continue;

				bool isSubparameter = subObj != null && (subObj.Symbol == null || subObj.Unit == null);
				if (isSubparameter)
				{
					res.AddRange(PrintParameters(subObj, obj));
				}
				else
				{
					if (propinfo.PropertyType.Name.IndexOf("[]") > 0)
					{

						foreach (var element in propinfo.GetValue(obj, null) as Array)
						{
							if (element != null && element is Parameter)
							{
								Parameter tmppar = (Parameter)element;
								res.Add(new ParametersTemplate()
								{
									Parameter = propinfo.Name + " " + tmppar.Name,
									Units = tmppar.Unit,
									Value = tmppar.ToString()
								});
							}
							else
								break;
						}

						//res.Add(new string[] { propinfo.Name, s });
					}
					else
					{
						bool isPrintableParameter = propinfo.GetValue(obj, null) != null;
						if (isPrintableParameter)
							res.Add(new ParametersTemplate()
							{
								Parameter = obj.Name + " " + subObj.Name,
								Units = subObj.Unit,
								Value = subObj.Value.ToString()
							});
						//res.Add(new string[] { propinfo.Name,propinfo.GetValue(obj, null).ToString() });
					}
				}
			}
			return res;
		}
	}
}
