using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtering
{
	public class Deliquoring:Parameter, IDeliquoringProcess
	{
		PressureDifferenceCakeDeliquoring pressureDifferenceCakeDeliquoring;
		public PressureDifferenceCakeDeliquoring PressureDifferenceCakeDeliquoring
		{
			get { return pressureDifferenceCakeDeliquoring; }
			set
			{
				pressureDifferenceCakeDeliquoring = value;
				OnPropertyChanged("PressureDifferenceCakeDeliquoring");
			}
		}

		CakeHeightForCakeDeliquoring cakeHeightForCakeDeliquoring=new CakeHeightForCakeDeliquoring();
		public CakeHeightForCakeDeliquoring CakeHeightForCakeDeliquoring
		{
			get => cakeHeightForCakeDeliquoring;
			set
			{
				cakeHeightForCakeDeliquoring = value;
				OnPropertyChanged("CakeHeightForCakeDeliquoring");
			}
		}

		DeliquoringTime deliquoringTime;
		public DeliquoringTime DeliquoringTime
		{
			get { return deliquoringTime; }
			set
			{
				deliquoringTime = value;
				OnPropertyChanged("DeliquoringTime");
			}
		}

		CakeSaturation cakeSaturation;
		public CakeSaturation CakeSaturation
		{
			get { return cakeSaturation; }
			set
			{
				cakeSaturation = value;
				OnPropertyChanged("CakeSaturation");
			}
		}

		CakeMoistureContent cakeMoistureContent;
		public CakeMoistureContent CakeMoistureContent
		{
			get { return cakeMoistureContent; }
			set
			{
				cakeMoistureContent = value;
				OnPropertyChanged("CakeMoistureContent");
			}
		}

		DeliquoringIndex deliquoringIndex;
		public DeliquoringIndex DeliquoringIndex
		{
			get { return deliquoringIndex; }
			set
			{
				deliquoringIndex = value;
				OnPropertyChanged("DeliquoringIndex");
			}
		}

		Washing washing;
		public Washing Washing
		{
			get => washing;
			set => washing=value;
		}

		CakeFormation cakeFormation;
		public CakeFormation CakeFormation
		{
			get => cakeFormation;
			set => cakeFormation = value;
		}


		Deliquoring IDeliquoringProcess.Deliquoring
		{
			get => this;
			set { }
		}

		public Deliquoring(IWashingProcess washingProcess)
		{
			Washing = washingProcess.Washing;
			CakeFormation = washingProcess.CakeFormation;
		}
	}

	public class CakeHeightForCakeDeliquoring : Height
	{
		public CakeHeightForCakeDeliquoring()
		{
			Name = "Cake Height for Cake Deliquoring";
			SymbolSuffix = "d";
			converter = new Param2DoubleConverter<CakeHeightForCakeDeliquoring>();
		}

		public CakeHeightForCakeDeliquoring(double? value) : this()
		{
			Value = value;
		}
	}

	public class DeliquoringIndex:Parameter
	{
		public DeliquoringIndex()
		{
			Name = "Deliquoring Index";
			Symbol = "K";
			Unit = "--";
			converter = new Param2DoubleConverter<DeliquoringIndex>();
		}

		public DeliquoringIndex(double? value):this()
		{
			Value = value;
		}
	}

	public class CakeMoistureContent:Parameter
	{
		public CakeMoistureContent()
		{
			Name = "CakeMoistureContent";
			Symbol = "Rf";
			Unit = "%";
			converter = new Param2DoubleConverter<CakeMoistureContent>();
		}

		public CakeMoistureContent(double? value):this()
		{
			Value = value;
		}
	}

	public class CakeSaturation:Parameter
	{
		public CakeSaturation()
		{
			Name = "Cake Saturation";
			Symbol = "S";
			Unit = "%";
			converter = new Param2DoubleConverter<CakeSaturation>();
		}

		public CakeSaturation(double? value):this()
		{
			Value = value;
		}
	}

	public class DeliquoringTime : Time
	{
		public DeliquoringTime()
		{
			Name = "Deliquoring Time";
			SymbolSuffix = "d";
			converter = new Param2DoubleConverter<DeliquoringTime>();
		}

		public DeliquoringTime(double? value) : this()
		{
			Value = value;
		}
	}

	public class PressureDifferenceCakeDeliquoring: PressureDifferenceCakeFormation
	{
		public PressureDifferenceCakeDeliquoring()
		{
			Name = "Pressure difference cake deliquoring";
			SymbolSuffix = "d";
			converter = new Param2DoubleConverter<PressureDifferenceCakeDeliquoring>();
		}

		public PressureDifferenceCakeDeliquoring(double? value):this()
		{
			Value = value;
		}

	}
}
