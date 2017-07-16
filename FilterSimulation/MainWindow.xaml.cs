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
		List<WashingLiquid> Liquids = new List<WashingLiquid>();
		List<Washing> Washings = new List<Washing>();
		List<CakeFormation> CakeFormations = new List<CakeFormation>();
		//public ObservableCollection<Liquid> Liquids = new ObservableCollection<Liquid>();
		

		public MainWindow()
		{
			InitializeComponent();

			
			Liquids.Add(new WashingLiquid("Water", new Viscosity(0.894), new Density(1000)));
			Liquids.Add(new WashingLiquid("Acetone", new Viscosity(0.306), new Density(797.05)));
			Liquids.Add(new WashingLiquid("Ethanol", new Viscosity(1.074), new Density(789.3)));
			Liquids.Add(new WashingLiquid("Sulfuric acid", new Viscosity(24.2), new Density(1835.6)));


			Washings.AddRange(new Washing[] {
				new Washing("Water washing",Liquids[0],new Max_wash_out(100), new Min_wash_out(1), new Adaptation_ParameterA(0),new Adaptation_ParameterB(0)),
				new Washing("Washing2",Liquids[1],new Max_wash_out(75), new Min_wash_out(2), new Adaptation_ParameterA(1),new Adaptation_ParameterB(1)),
				new Washing("Washing3",Liquids[2],new Max_wash_out(60), new Min_wash_out(3), new Adaptation_ParameterA(2),new Adaptation_ParameterB(2)),
				new Washing("Washing4",Liquids[3],new Max_wash_out(50), new Min_wash_out(4), new Adaptation_ParameterA(3),new Adaptation_ParameterB(3)),
				});

			CakeFormations.Add(
				new CakeFormation("CakeFormation1",new Suspension("Suspension1",null,new Density(1400),null,null), new Filter("Filter1",null,new MachineDiameter(400),new MachineWidth(200)),new Cake("Cake1",new Porosity(58),null,null,new Height(52)),new WashingRatio(2))
			);

			//LiquidSelectComboBox.ItemsSource = Liquids;
			LiquidPropertiesExpander.DataContext = Liquids;
			WashingGroupBox.DataContext = Washings;

			//CakeFormationGroupBox.DataContext = MyReflection.PrintParamPropObject(CakeFormations[0], null);
			CakeFormationGroupBox.DataContext = CakeFormations;
			//CakeFormationGroupBox.DataContext = MyReflection.PrintParameters(CakeFormations[0], null);

			//WashLiquidVolume.DataContext = CakeFormations[0];
			//washingRatioTextBox.DataContext = CakeFormations[0];


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
			WashingLiquidParametersDataGrid.ItemsSource = MyReflection.PrintParameters(LiquidSelectComboBox.SelectedItem as Parameter,null);
			//new object[] { LiquidSelectComboBox.SelectedItem };
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}
					
		private void WashingSelectComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			WasingParametersDataGrid.ItemsSource = MyReflection.PrintParameters(WashingSelectComboBox.SelectedItem as Parameter, null);
			LiquidSelectComboBox.SelectedItem = ((Washing)WashingSelectComboBox.SelectedItem).Liquid;
		}
	}



	
}
