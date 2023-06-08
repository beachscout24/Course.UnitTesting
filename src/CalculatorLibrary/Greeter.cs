using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace CalculatorLibrary
{
	public class Greeter
	{

		private readonly IDateTimeProvider _dataTimeProvider;

        public Greeter(IDateTimeProvider dateTimeProvider)
        {
			_dataTimeProvider = dateTimeProvider;
		}

        public string GenerateGreetMessage()
		{
			var dataTimeNow = _dataTimeProvider.DateTimeNow;
			return dataTimeNow.Hour switch
			{
				>= 5 and < 12 => "Good Morning",
				>= 12 and < 18 => "Good Afternoon",
				_ => "Good Evening"
			};
		}
	}
}
