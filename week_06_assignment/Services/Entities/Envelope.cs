using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace CoursesAPI.Services.Models.Entities
{
	/// <summary>
	/// An object to represent a list of items for paging
	/// </summary>
	[Table("Envelope")]
	public class Envelope<T>
	{
		/// <summary>
		/// A List of items to display
		/// </summary>
		public IEnumerable<T> Items {get; set;}
        
        /// <summary>
		/// A number of items to display on each page
		/// </summary>
		public int PageSize {get; set;}

        /// <summary>
		/// A number that represents the number of the current page
		/// </summary>
		public int CurrentPage {get; set;}

        /// <summary>
		/// A number that represents how many pages are available where
        /// each page has PageSize items
		/// </summary>
		public int TotalPages {get; set;}
	}
}
