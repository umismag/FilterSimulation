using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilterSimulation.Classes;

namespace FilterSimulation
{
	class CakeFormation:Parameter
	{
		public CakeFormation(string name, Suspension suspension, Filter filter, Cake cake )
		{
			Name = name;
			
			SubParameters = new List<Parameter> { suspension, filter, cake };
		}
	}

	class Suspension:Parameter
	{
		public Suspension(string name, Filtrate filtrate, Density solidDensity, SolidConcentration solidConcentration, Compressibility compressibility)
		{
			Name = name;
			solidDensity.SymbolSuffix = " s";
			SubParameters = new List<Parameter> { filtrate, solidDensity, solidConcentration, compressibility };

		}
	}
	
	class Filter:Parameter
	{
		public Filter(string name, Resistance filterMediumResistance)
		{
			Name = name;

			SubParameters = new List<Parameter> { filterMediumResistance };
		}
	}

	class Cake : Parameter
	{
		public Cake(string name, Porosity porosity, Permeability permeability, Compressibility compressibility)
		{
			Name = name;
			permeability.SymbolSuffix = "c0";
			SubParameters = new List<Parameter> { porosity, permeability, compressibility };
		}
	}

	class Resistance :Parameter
	{
		public Resistance(double? value)
		{
			Value = value;
			Name = "Resistance";
			Unit = "mm";
			Symbol = "hce";
		}
	}

	class Compressibility:Parameter
	{
		public Compressibility(double? value)
		{
			Value = value;
			Name = "Compressibility";
			Unit = "-";
			Symbol = "nc";
		}
	}

	class Permeability:Parameter
	{
		public Permeability(double? value)
		{
			Value = value;
			Name = "Permeability";
			Unit = "*E-13 m2";
			Symbol = "P";
		}
	}

	class Porosity:Parameter
	{
		public Porosity(double? value)
		{
			Value = value;
			Name = "Porosity";
			Unit = "%";
			Symbol = "E";
		}
	}

	class SolidConcentration:Parameter
	{
		public SolidConcentration(double? value)
		{
			Value = value;
			Name = "Solid Concentration";
			Unit = "%";
			Symbol = "Cm";
		}
	}

	class Filtrate:Parameter
	{
		Filtrate(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			SubParameters = new List<Parameter> { viscosity, density };
		}
	}
}
