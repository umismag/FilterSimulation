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
		public CakeFormation(string name, Suspension suspension)
		{
			Name = name;
			SubParameters = new Parameter[] {suspension };
		}
	}

	class Suspension:Parameter
	{
		public Suspension(string name, Filtrate filtrate, Density solid_Density)
		{
			Name = name;
			solid_Density.SymbolSuffix = "s";
			SubParameters = new Parameter[] {filtrate, solid_Density };

		}
	}

	class Solid_Concentration:Parameter
	{
		public Solid_Concentration(double value)
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
			SubParameters = new Parameter[] {viscosity, density };
		}
	}
}
