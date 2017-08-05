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
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Filtering
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		//List<CakeFormation> CakeFormationList = new List<CakeFormation>();
		//ObservableCollection<Parameter> CakeFormationList = new ObservableCollection<Parameter>();
		//List<WashingLiquid> Liquids = new List<WashingLiquid>();
		CakeFormations myCakeFormations;
		ICollectionView myCakeFormationsView;
		public CakeFormation CurrentCakeFormation;

		public MainWindow()
		{
			InitializeComponent();

			myCakeFormations = (CakeFormations)(this.Resources["CakeFormationsCollection"]);
			myCakeFormationsView = CollectionViewSource.GetDefaultView(myCakeFormations);
			CurrentCakeFormation = myCakeFormations[0];
			CakeFormation2GroupBox.DataContext = CurrentCakeFormation;

			
			Grid grd = MyReflection.PrintGridWithParamList(CurrentCakeFormation);
			CakeFormation2GroupBoxStackPanel.Children.Add(grd);

			//Liquids.Add(new WashingLiquid("Water", new Viscosity(0.894), new Density(1000)));
			//Liquids.Add(new WashingLiquid("Acetone", new Viscosity(0.306), new Density(797.05)));
			//Liquids.Add(new WashingLiquid("Ethanol", new Viscosity(1.074), new Density(789.3)));
			//Liquids.Add(new WashingLiquid("Sulfuric acid", new Viscosity(24.2), new Density(1835.6)));

			



			//CakeFormationGroupBox.DataContext =CakeFormation1; //CakeFormationList; //

			
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			//CakeFormationGroupBox.DataContext = CakeFormationList;
		}

		private void CakeFormationDataGrid_SourceUpdated(object sender, DataTransferEventArgs e)
		{
			//myCakeFormationsView.Refresh();

		}

		private void CakeFormationDataGrid_TargetUpdated(object sender, DataTransferEventArgs e)
		{

		}

		private void CakeFormationDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			//(sender as DataGrid).GetBindingExpression(e.EditingElement).UpdateTarget();
			//(sender as DataGrid).CommitEdit();
			//(sender as DataGrid).Items.Refresh();
		}
	}

	public class CakeFormations:ObservableCollection<CakeFormation>
	{
		public CakeFormations()
		{
			CakeFormation cf= new CakeFormation("CakeFormation1", new Suspension("Suspension1", new Filtrate("Filtrate1", new Viscosity(0.894), new Density(1000)), new Density(1400), new SolidConcentration(16), new Compressibility(0.5)), new Filter("Filter1", new Resistance(15), new MachineDiameter(400), new MachineWidth(200)), new Cake("Cake1", new Porosity(58), new Permeability(25), new Compressibility(35), new Height(52)), new WashingRatio(2));

			//cf.PropertyChanged += (x, y) => 
			//{
			//	OnPropertyChanged(y);
			//};
				

			this.Add(cf);
		}


	}
}
