using CoursesAPI.Services.Exceptions;

namespace CoursesAPI.Services.Utilities
{
	public class DateTimeUtils
	{
		public static bool IsLeapYear(int year)
		{

			// If divisible by 4
			if(year % 4 == 0){
				// unless it is divisible by 100
				if(year % 100 == 0){
					// except when it is divisible by 400
					if(year % 400 == 0){
						return true;
					}
					else
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
