namespace CalculatorLibrary
{
	public class DataTimeProvider : IDateTimeProvider
	{
		public DateTime DateTimeNow => DateTime.Now;
	}
}
