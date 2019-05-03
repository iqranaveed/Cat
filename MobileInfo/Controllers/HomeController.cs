using MobileInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

/* blah blah blah*/

namespace MobileInfo.Controllers
{
    public class HomeController : Controller
    {

        private DB16Entities db = new DB16Entities();

        public ActionResult Index()
        {
            var entities = new DB16Entities();
            return View(entities.Brands.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult blah()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult BrandMobiles(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.linkableId = id;
            var entities = new DB16Entities();
            return View(entities.Mobiles.ToList());
        }

    }

    //<img src = "@Url.Content(item.Image)" height="100" width="100" />
    /*
     *<div class="container">

            @foreach (var item in Model)
            {
                <div class= "col-md-3" style="margin-bottom:25px">
                    <div class="thumbnail">
                        <div class="Images">
                            <img class="img-responsive" src="~@item.Image" height="350" width="250" /> 
                        </div>
                        <div class="caption" style=" border-top: 3px solid" >
                            <h3> @item.Name</h3>
                            <h4> @item.Country</h4>
                        </div>
                    </div>
                </div>
            }
        </div>*/
}