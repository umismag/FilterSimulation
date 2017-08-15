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

		public void OnPropertyChanged(/*[CallerMemberName]*/string prop = "")
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(prop));
			if (PropertyChangedStatic != null)
				PropertyChangedStatic(this, new PropertyChangedEventArgs(prop));
		}

		SourceOfChanging? sourceOfParameterChanging = SourceOfChanging.AutomaticallyByConstructor;
		public SourceOfChanging? SourceOfParameterChanging
		{
			get { return sourceOfParameterChanging; }
			set { sourceOfParameterChanging = value; }
		}

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
			get { return symbol+SymbolSuffix; }
			internal set { symbol = value; }
		}

		string symbolSuffix = string.Empty;
		public string SymbolSuffix
		{
			get { return symbolSuffix; }
			internal set { symbolSuffix = value; }
		}

		double minValue = double.MinValue;
		double maxValue = double.MaxValue;

		protected double? value=null;

		public double? Value
		{
			get { return this.value ; }
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

		protected Parameter()
		{
			SourceOfParameterChanging2BrushConverter = new SourceOfParameterChanging2BrushConverterClass();
		}

		protected IValueConverter converter=null;
		public IValueConverter Converter
		{
			get { return converter; }
		}

		public class Param2DoubleConverter<T>  : IValueConverter where T : Parameter, new()
		{
			public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			{
				T tmp = value as T;
				if (tmp != null)
					return tmp.Value;
				else
					return value;
			}

			public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			{
				bool IsNeedConvert = (value != null) &&(value.ToString()!="")&& value.ToString()[value.ToString().Length-1]!='.' && value.ToString()[value.ToString().Length - 1] != ',';
				if (IsNeedConvert)
				{
					T tmp = new T();
					double res;
					if (double.TryParse(value.ToString().Replace('.',','), out res))
					{
						tmp.Value = res;
						tmp.SourceOfParameterChanging = SourceOfChanging.ManuallyByUser;
					}
					else
					{
						tmp.Value = null;
					}
						
					//Type t = tmp.GetType();
					//tmp.OnPropertyChanged(t.BaseType.Name+"."+tmp.GetType().Name);
					return tmp;
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
								br.Color = new Color() { R= 0, G=255, B = 0, A = 128};
								return br;
								//break;
							}
		
						case SourceOfChanging.AutomaticallyByCore:
							{
								br.Color = new Color() { R = 0, G = 0, B = 255, A = 128 };
								return br;
								//break;
							}
						case SourceOfChanging.AutomaticallyByConstructor:
							{
								br.Color = new Color() { R = 128, G = 128, B = 128, A = 128 };
								return br;
								//break;
							}
						default:
							{
								br.Color = new Color() { R = 255, G = 0, B = 0, A = 128 };
								return br;
								//break;
							}
					}
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

		protected bool IsNeedToUpdate(string dependedParametersString, PropertyChangedEventArgs prop, Parameter parameter, Func<Parameter> function, object sender)
		{
			//callersCount++;
			bool IsNeed= 
				dependedParametersString.Contains(prop.PropertyName) &&
				function().Value.HasValue && 
				!double.IsNaN(function().Value.Value) && 
				!double.IsInfinity(function().Value.Value)&&
				(!parameter.Value.HasValue || 
				double.IsNaN(parameter.Value.Value) || 
				double.IsInfinity(parameter.Value.Value) ||
				Math.Abs((parameter.Value ?? 0) - (function().Value ?? 0)) > double.Epsilon);
			
			
			return IsNeed;
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

	public class ObjWithParametersList:Parameter
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
			Parameter tmp = parameter as Parameter;
			if (tmp != null)
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

		public static Grid PrintGridWithParamList(Parameter obj, Parameter parentObject = null)
		{
			if (obj == null) return null;

			Grid res = new Grid();
			
			if (parentObject == null)
			{
				//res.ShowGridLines = true;
				ColumnDefinition cd1 = new ColumnDefinition();
				cd1.Width = new System.Windows.GridLength(3, System.Windows.GridUnitType.Star);

				ColumnDefinition cd2 = new ColumnDefinition();
				cd2.Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);

				ColumnDefinition cd3 = new ColumnDefinition();
				cd3.Width = new System.Windows.GridLength(2, System.Windows.GridUnitType.Star);

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
				Grid.SetColumn(lb1, 0);
				Grid.SetRow(lb1, 0);

				Label lb2 = new Label();
				lb2.Background = lb1.Background;
				lb2.Content = "Unit";
				lb2.BorderThickness = new Thickness(1, 1, 1, 1);
				lb2.BorderBrush = Brushes.Black;
				Grid.SetColumn(lb2, 1);
				Grid.SetRow(lb2, 0);

				Label lb3 = new Label();
				lb3.Background = lb1.Background;
				lb3.Content = "Value";
				lb3.BorderThickness = new Thickness(0, 1, 1, 1);
				lb3.BorderBrush = Brushes.Black;
				Grid.SetColumn(lb3, 2);
				Grid.SetRow(lb3, 0);

				res.Children.Add(lb1);
				res.Children.Add(lb2);
				res.Children.Add(lb3);
			}

			void AddRowToRes(Parameter param, Parameter parent)
			{
				//Binding groupNumberBinding = new Binding();
				//groupNumberBinding.Source = parent;
				//groupNumberBinding.Mode = BindingMode.OneWay;
				//groupNumberBinding.Path = new PropertyPath(param.Name + ".GroupNumber");
				//groupNumberBinding.Converter = 
				//groupNumberBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;



				Binding backGroundColorBinding = new Binding();
				backGroundColorBinding.Source = parent;
				backGroundColorBinding.Mode = BindingMode.OneWay;
				backGroundColorBinding.Path = new PropertyPath(param.Name+".SourceOfParameterChanging");
				backGroundColorBinding.Converter = param.SourceOfParameterChanging2BrushConverter;
				backGroundColorBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

				Border NameBorder = new Border();
				NameBorder.BorderBrush = Brushes.Black;
				NameBorder.BorderThickness = new Thickness(1, 0, 0, 1);
				NameBorder.Padding = new Thickness(2, 2, 2, 2);
				NameBorder.Margin = new Thickness(0, -1, 0, 0);

				Border UnitBorder = new Border();
				UnitBorder.BorderBrush = Brushes.Black;
				UnitBorder.BorderThickness = new Thickness(1, 0, 0, 1);
				UnitBorder.Padding = NameBorder.Padding;
				UnitBorder.Margin = NameBorder.Margin;

				RowDefinition rd = new RowDefinition();
				res.RowDefinitions.Add(rd);

				TextBlock TextBlockName = new TextBlock();
				TextBlockName.TextWrapping = TextWrapping.Wrap;
				TextBlockName.TextTrimming = TextTrimming.None;
				
				TextBlockName.Text = (parentObject!=null?parent.GetType().Name+" ":"")+ param.Name + " [" + param.Symbol + "]";
				TextOptions.SetTextFormattingMode(TextBlockName,TextFormattingMode.Display);
				//Grid.SetColumn(TextBlockName, 0);
				//Grid.SetRow(TextBlockName, res.RowDefinitions.Count - 1);
				Grid.SetColumn(NameBorder, 0);
				Grid.SetRow(NameBorder,res.RowDefinitions.Count - 1); //param.GroupNumber - 1);

				TextBlock TextBlockUnit = new TextBlock();
				TextBlockUnit.Text = param.Unit;
				TextBlockUnit.TextAlignment = TextAlignment.Center;
				TextOptions.SetTextFormattingMode(TextBlockUnit, TextFormattingMode.Display);
				Grid.SetColumn(UnitBorder, 1);
				Grid.SetRow(UnitBorder, res.RowDefinitions.Count - 1);//param.GroupNumber - 1); 

				Binding binding = new Binding();
				binding.Source = parent;
				binding.Path = new PropertyPath(param.Name);
				binding.Converter = param.Converter;
				//binding.Delay = 300;
				binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;

				TextBox TextBoxValue = new TextBox();
				TextBoxValue.Foreground = Brushes.Blue;
				TextBoxValue.SetBinding(TextBox.TextProperty, binding);
				TextBoxValue.SetBinding(TextBox.BackgroundProperty, backGroundColorBinding);
				TextOptions.SetTextFormattingMode(TextBoxValue, TextFormattingMode.Display);
				//TextBoxValue
				Grid.SetColumn(TextBoxValue, 2);
				Grid.SetRow(TextBoxValue, res.RowDefinitions.Count - 1);// param.GroupNumber - 1);

				NameBorder.Child = TextBlockName;
				res.Children.Add(NameBorder);
				//res.Children.Add(TextBlockName);
				UnitBorder.Child = TextBlockUnit;
				res.Children.Add(UnitBorder);
				//res.Children.Add(TextBlockUnit);
				res.Children.Add(TextBoxValue);
				
			}

			Type t = obj.GetType();

			PropertyInfo[] pi = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			
			foreach (PropertyInfo propinfo in pi)
			{
				Parameter subObj = propinfo.GetValue(obj) as Parameter;
				if (subObj == null) continue;
				subObj.Name = propinfo.Name;

				bool isSubparameter = subObj.Symbol == null || subObj.Unit == null;
				if (isSubparameter)
				{
					List<UIElement> elements = new List<UIElement>();
					Grid grd = PrintGridWithParamList(subObj, obj);      //!!!!
					foreach (UIElement el in grd.Children)
					{
						elements.Add(el);
					}
					grd.Children.Clear();
					int i = 0;
					foreach(UIElement el in elements)
					{
						if (i % 3 == 0)
						{
							RowDefinition rd = new RowDefinition();
							res.RowDefinitions.Add(rd);
						}
						Grid.SetRow(el, res.RowDefinitions.Count-1);
						res.Children.Add(el);
						i++;
					}
					
				}
				else
				{
					bool isPrintableParameter = propinfo.GetValue(obj, null) != null;
					if (isPrintableParameter)
					{
							AddRowToRes(subObj, obj);
					}
				}
			}
			return res;
		}

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

		public static List<ParametersTemplate> PrintParameters(Parameter obj, Parameter parentObject=null)
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
						bool isPrintableParameter = propinfo.GetValue(obj, null) != null ;
						if (isPrintableParameter)
							res.Add(new ParametersTemplate()
							{
								Parameter = obj.Name+" " + subObj.Name,
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
