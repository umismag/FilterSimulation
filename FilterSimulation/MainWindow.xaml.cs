using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;
using FilterSimulation.Classes;
using System.Collections.ObjectModel;

namespace FilterSimulation
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RibbonWindow
	{
		List<Liquid> Liquids = new List<Liquid>();
		List<Washing> Washings = new List<Washing>();
		//public ObservableCollection<Liquid> Liquids = new ObservableCollection<Liquid>();

		public MainWindow()
		{
			InitializeComponent();

			
			Liquids.Add(new Liquid("Water", new Viscosity(0.894), new Density(1000)));
			Liquids.Add(new Liquid("Acetone", new Viscosity(0.306), new Density(797.05)));
			Liquids.Add(new Liquid("Ethanol", new Viscosity(1.074), new Density(789.3)));
			Liquids.Add(new Liquid("Sulfuric acid", new Viscosity(24.2), new Density(1835.6)));


			Washings.AddRange(new Washing[] {
				new Washing("Water washing",Liquids[0],new Max_wash_out(100), new Min_wash_out(1), new Adaptation_ParameterA(0),new Adaptation_ParameterB(0)),
				new Washing("Water washing",Liquids[1],new Max_wash_out(75), new Min_wash_out(2), new Adaptation_ParameterA(1),new Adaptation_ParameterB(1)),
				new Washing("Water washing",Liquids[2],new Max_wash_out(60), new Min_wash_out(3), new Adaptation_ParameterA(2),new Adaptation_ParameterB(2)),
				new Washing("Water washing",Liquids[3],new Max_wash_out(50), new Min_wash_out(4), new Adaptation_ParameterA(3),new Adaptation_ParameterB(3)),
				});

			//LiquidSelectComboBox.ItemsSource = Liquids;
			LiquidPropertiesExpander.DataContext = Liquids;
			
		}

		private void CheckBox_Checked(object sender, RoutedEventArgs e)
		{
			if (WashingStackPanel == null) return;
		if (WasingCheckBox.IsChecked == true)
				WashingStackPanel.Visibility = Visibility.Visible;
			else
				WashingStackPanel.Visibility = Visibility.Collapsed;
		}

		private void LiquidSelectCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			WashingLiquidParametersDataGrid.ItemsSource = MyReflection.PrintParameters(LiquidSelectComboBox.SelectedItem,null);
			//new object[] { LiquidSelectComboBox.SelectedItem };
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void WashingSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
	}



	
}
