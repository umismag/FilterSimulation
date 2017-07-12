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
		Suspension suspension;
		public Suspension Suspension
		{
			get { return suspension; }
			set { suspension = value; }
		}

		Filter filter;
		public Filter Filter
		{
			get { return filter; }
			set { filter = value; }
		}

		Cake cake;
		public Cake Cake
		{
			get { return cake; }
			set { cake = value; }
		}

		public CakeFormation(string name, Suspension suspension, Filter filter, Cake cake )
		{
			Name = name;
			Suspension = suspension;
			Filter = filter;
			Cake = cake;
		}
	}

	class Suspension:Parameter
	{
		Filtrate filtrate;
		public Filtrate Filtrate
		{
			get { return filtrate; }
			set { filtrate = value; }
		}

		Density solidDensity;
		public Density SolidDensity
		{
			get { return solidDensity; }
			set { solidDensity = value; }
		}

		SolidConcentration solidConcentration;
		public SolidConcentration SolidConcentration
		{
			get { return solidConcentration; }
			set { solidConcentration = value; }
		}

		Compressibility compressibility;
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set { compressibility = value; }
		}

		public Suspension(string name, Filtrate filtrate, Density solidDensity, SolidConcentration solidConcentration, Compressibility compressibility)
		{
			Name = name;
			solidDensity.SymbolSuffix = " s";

			Filtrate = filtrate;
			SolidDensity = solidDensity;
			SolidConcentration = solidConcentration;
			Compressibility = compressibility;
		}
	}
	
	class Filter:Parameter
	{
		Resistance mediumResistance;
		public Resistance MediumResistance
		{
			get { return mediumResistance; }
			set { mediumResistance = value; }
		}

		public Filter(string name, Resistance filterMediumResistance)
		{
			Name = name;
			MediumResistance = filterMediumResistance;
		}
	}

	class Cake : Parameter
	{
		Porosity porosity;
		public Porosity Porosity
		{
			get { return porosity; }
			set { porosity = value; }
		}

		Permeability permeability;
		public Permeability Permeability
		{
			get { return permeability; }
			set { permeability = value; }
		}

		Compressibility compressibility;
		public Compressibility Compressibility
		{
			get { return compressibility; }
			set { compressibility = value; }
		}

		public Cake(string name, Porosity porosity, Permeability permeability, Compressibility compressibility)
		{
			Name = name;
			permeability.SymbolSuffix = "c0";
			Porosity = porosity;
			Permeability = permeability;
			Compressibility = compressibility;
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
		Viscosity viscosity;
		public Viscosity Viscosity
		{
			get { return viscosity; }
			set { viscosity = value; }
		}

		Density density;
		public Density Density
		{
			get { return density; }
			set { density = value; }
		}

		Filtrate(string name, Viscosity viscosity, Density density)
		{
			Name = name;
			Viscosity = viscosity;
			Density = density;
		}
	}
}
