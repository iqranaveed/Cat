//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MobileInfo.Models
{
    using System;
    using System.Collections.Generic;
	using System.Web;

	public partial class Picture
    {
        public int Id { get; set; }
        public int MobileId { get; set; }
        public string Image { get; set; }

		public HttpPostedFileBase ImageFile1 { get; set; }

		public virtual Mobile Mobile { get; set; }
    }
}
