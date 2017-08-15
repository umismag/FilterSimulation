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
		ICollectionView myCakeFormationsView, myFiltrationView;
		public CakeFormation CurrentCakeFormation;
		//Filtration FLTR = new Filtration();
		Filtration FLTR;
		//delegate GroupsOfParameters Get_Table_Delegate(ICakeFormationProcess cakeFormationProcess);
		Func<IWashingDeliquoringProcess, GroupsOfParameters> func = new Func<IWashingDeliquoringProcess, GroupsOfParameters>(TableOfGroupsOfParameters.Get_Materials_ParametersTable);

		public MainWindow()
		{
			InitializeComponent();

			FLTR = (Filtration)(this.Resources["Filtration"]);
			//myFiltrationView = CollectionViewSource.GetDefaultView(FLTR);


			//myCakeFormations = (CakeFormations)(this.Resources["CakeFormationsCollection"]);
			//myCakeFormationsView = CollectionViewSource.GetDefaultView(myCakeFormations);
			//CurrentCakeFormation = myCakeFormations[0];
			//CakeFormation2GroupBox.DataContext = CurrentCakeFormation;


			//Grid grd = MyReflection.PrintGridWithParamList(FLTR.FP.CakeFormation);//CurrentCakeFormation);
			//CakeFormation2GroupBoxStackPanel.Children.Add(grd);

			func = TableOfGroupsOfParameters.Get_Materials_ParametersTable;
			Grid grd1 =FLTR.FP.GetParametersTable(func);
			MaterialParametersStackPanel.Children.Add(grd1);

			func = TableOfGroupsOfParameters.Get_FOnly_ParametersTableStandard1;
			Grid grd2 = FLTR.FP.GetParametersTable(func);
			CakeFormationStackPanel.Children.Add(grd2);

			func = TableOfGroupsOfParameters.Get_Washing_ParametersTable;
			Grid grd3 = FLTR.FP.GetParametersTable(func);
			WashingStackPanel.Children.Add(grd3);

			func = TableOfGroupsOfParameters.Get_Deliquoring_ParametersTable;
			Grid grd4 = FLTR.FP.GetParametersTable(func);
			DeliquoringStackPanel.Children.Add(grd4);

			func = TableOfGroupsOfParameters.Get_Result_ParametersTable;
			Grid grd5 = FLTR.FP.GetParametersTable(func);
			ResultStackPanel.Children.Add(grd5);

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

	public class Filtration
	{
		public FilteringProcess FP;

		public Filtration()
		{
			CakeFormation cf = new CakeFormation("CakeFormation1",
				new Suspension("Suspension1", new MotherLiquid("Filtrate1", new MotherLiquidViscosity(0.894), new MotherLiquidDensity (1000)), new SolidDensity(1400), new SuspensionDensity(), new SolidConcentration(16), new SolidsMassFraction(), new SolidsVolumeFraction()),
				new Filter("Filter1", new SpecificFilterMediumResistance(), new MediumResistance(15), new MachineDiameter(400), new MachineWidth(200)),
				new Cake("Cake1", new Porosity(58), new StandardPorosity(), new PorosityReductionFactor(), new CakePermeability(25), new StandardCakePermeability(), new Compressibility(35), new CakeHeigth(52)));

			Washing wng = new Washing("Washing1", new WashingLiquid("Water", new Viscosity(0.894), new Density(1000)), new Max_wash_out(90), new Min_wash_out(5), new Adaptation_ParameterA(5), new Adaptation_ParameterB(), new PressureDifferenceCakeWashing(2),cf);

			Deliquoring dlng = new Deliquoring(wng)
			{
				PressureDifferenceCakeDeliquoring = new PressureDifferenceCakeDeliquoring(4),
				CakeSaturation = new CakeSaturation(25),
				CakeMoistureContent = new CakeMoistureContent(5),
				DeliquoringIndex = new DeliquoringIndex(2),
				DeliquoringTime = new DeliquoringTime(5)
			};

			ResultParameter ResParam = new ResultParameter(dlng)
			{
				//CycleTime = new CycleTime(),
				//TechnicalTime=new TechnicalTime(),
				//SolidsThroughput=new SolidsThroughput()
			};

			FP = new FilteringProcess(ResParam, cf, wng, dlng);
			//FP.CakeFormation = cf;
			//FP.Washing = wng;
			//FP.Deliquoring = dlng;
			//FP.ResultParameter = ResParam;
		}
	}

	public class CakeFormations:ObservableCollection<CakeFormation>
	{
		public CakeFormations()
		{
			CakeFormation cf= new CakeFormation("CakeFormation1",
				new Suspension("Suspension1", new MotherLiquid("Filtrate1", new MotherLiquidViscosity(0.894), new MotherLiquidDensity(1000)), new SolidDensity(1400), new SuspensionDensity(), new SolidConcentration(16), new SolidsMassFraction(), new SolidsVolumeFraction()),
				new Filter("Filter1", new SpecificFilterMediumResistance(), new MediumResistance(15), new MachineDiameter(400), new MachineWidth(200)),
				new Cake("Cake1", new Porosity(58), new StandardPorosity(), new PorosityReductionFactor(), new CakePermeability(25), new StandardCakePermeability(), new Compressibility(35), new CakeHeigth(52)));

			this.Add(cf);
		}
	}
}
