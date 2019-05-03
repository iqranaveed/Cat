using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MobileInfo.Models;
using System.Web.UI;

namespace MobileInfo.Controllers
{
    public class AdminController : Controller
	{
		//lalalala
		//lalala

		private DB16Entities db = new DB16Entities();
		SqlConnection con = new SqlConnection(@"Data Source = HAIER - PC\SQLEXPRESS; Initial Catalog = ProjectA; Integrated Security = True");
		SqlConnection con1 = new SqlConnection(@"Data Source =HAIER-PC\SQLEXPRESS;initial catalog = DB16; integrated security = True");
		public ActionResult BIndex()
		{
			using (DB16Entities db = new DB16Entities())
			{
				return View(db.Brands.ToList());
			}

		}

		[HttpGet]
		public ActionResult AdminLogin()
		{
			return View();
		}

		[HttpPost]
		public ActionResult AdminLogin(Administrator l)
		{
			if (l.Email == "admin123@gmail.com")
			{
				if (l.Password == "1")
				{
					return RedirectToAction("ALoggedIn");
				}
				else
				{
					TempData["msg"] = "<script>alert('Login Failed');</script>";
				}
			}
			else
			{
				TempData["msg"] = "<script>alert('Login Failed');</script>";
			}
			return RedirectToAction("AdminLogin");
		}

		public ActionResult ALoggedIn()
		{
			return View();
		}

		[HttpGet]
		public ActionResult RegisterBrand()
		{
			return View();
		}

		[HttpPost]
		public ActionResult RegisterBrand(Brand obj)
		{
			string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile.FileName);
			string extension = Path.GetExtension(obj.ImageFile.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			obj.Image = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			obj.ImageFile.SaveAs(fileName);
			using (DB16Entities db = new DB16Entities())
			{
				db.Brands.Add(obj);
				db.SaveChanges();
				ModelState.Clear();
				obj = null;
	
				ViewBag.Message = "Registered Successful";
				return RedirectToAction("MIndex");
			}
		}

		[HttpGet]
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Brand s = db.Brands.SingleOrDefault(x => x.Id == id);
			if (s == null)
			{
				return HttpNotFound();
			}
			return View(s);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(Brand obj)
		{
			string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile.FileName);
			string extension = Path.GetExtension(obj.ImageFile.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			obj.Image = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			obj.ImageFile.SaveAs(fileName);
			db.Entry(obj).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("BIndex");

		}

		public ActionResult BDelete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Brand user = db.Brands.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}

		[HttpPost, ActionName("BDelete")]
		[ValidateAntiForgeryToken]
		public ActionResult BDeleteConfirmed(int id)
		{
			string que = "SELECT Id From Mobile WHERE BrandId = '" + id + "'";
			if (con1.State == System.Data.ConnectionState.Closed)
			{
				con1.Open();
			}
			SqlCommand cmd = new SqlCommand(que, con1);
			SqlDataReader reader = cmd.ExecuteReader();
			List<Int32> list1 = new List<Int32>();
			int v = 0;
			int z = 0;
			int i = 0;
			while (reader.Read())
			{
				v = Int32.Parse(reader[0].ToString());
				int a = v;
				db.Pictures.Where(x => x.MobileId == a).ToList().ForEach(x => db.Pictures.Remove(x));
				int b = v;
				db.Mobiles.Where(x => x.Id == b).ToList().ForEach(x => db.Mobiles.Remove(x));
				i++;
			}
			Brand user = db.Brands.Find(id);
			db.Brands.Remove(user);
			db.SaveChanges();
			return RedirectToAction("BIndex");
		}

		public ActionResult BDetails(int? id)
		{
			Brand b = new Brand();
			using (DB16Entities db = new DB16Entities())
			{
				b = db.Brands.Where(x => x.Id == id).FirstOrDefault();
			}
			return View(b);
		}

//********************************************* Mobile ***********************************************//
		public ActionResult MIndex()
		{
			using (DB16Entities db = new DB16Entities())
			{
				return View(db.Mobiles.ToList());
			}

		}

		[HttpGet]
		public ActionResult RegisterMobile(int id = 0)
		{
			ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RegisterMobile(Mobile obj)
		{
			string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile1.FileName);
			string extension = Path.GetExtension(obj.ImageFile1.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			obj.Picture = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			obj.ImageFile1.SaveAs(fileName);
			using (DB16Entities db = new DB16Entities())
			{
				db.Mobiles.Add(obj);
				db.SaveChanges();
				ModelState.Clear();
				obj = null;

				TempData["msg"] = "<script>alert('Register successfully');</script>";
				return RedirectToAction("MIndex");
			}
		}

		public ActionResult MEdit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Mobile s = db.Mobiles.Find(id);
			if (s == null)
			{
				return HttpNotFound();
			}
			ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", s.BrandId);
			return View(s);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult MEdit(Mobile obj)
		{
			string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile1.FileName);
			string extension = Path.GetExtension(obj.ImageFile1.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			obj.Picture = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			obj.ImageFile1.SaveAs(fileName);
	


			db.Entry(obj).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("MIndex");
		}

		public ActionResult MDelete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Mobile user = db.Mobiles.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}


		[HttpPost, ActionName("MDelete")]
		[ValidateAntiForgeryToken]
		public ActionResult MDeleteConfirmed(int id)
		{
			string que = "SELECT Id From Pictures WHERE MobileId = '" + id + "'";
			if (con1.State == System.Data.ConnectionState.Closed)
			{
				con1.Open();
			}


			SqlCommand cmd = new SqlCommand(que, con1);
			SqlDataReader reader = cmd.ExecuteReader();
			List<Int32> list1 = new List<Int32>();
			int v = 0;
			int z = 0;
			int i = 0;
			while (reader.Read())
			{
				v = Int32.Parse(reader[0].ToString());
				int b = v;
				db.Pictures.Where(x => x.Id == b).ToList().ForEach(x => db.Pictures.Remove(x));
				i++;
			}
			Mobile m = db.Mobiles.Find(id);
			db.Mobiles.Remove(m);
			db.SaveChanges();
			return RedirectToAction("MIndex");
		}

		public ActionResult MDetails(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Mobile m = db.Mobiles.Find(id);
			return View(m);
		}

		// GET: Admin/Details/5
		public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

//***************************************** Pictures ******************************************//
		public ActionResult PIndex()
		{
			using (DB16Entities db = new DB16Entities())
			{
				return View(db.Pictures.ToList());
			}
		}

		[HttpGet]
		public ActionResult RegisterPicture(int id = 0)
		{
			ViewBag.MobileId = new SelectList(db.Mobiles, "Id", "Name");
			return View();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult RegisterPicture(Picture obj)
		{
			string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile1.FileName);
			string extension = Path.GetExtension(obj.ImageFile1.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			obj.Image = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			obj.ImageFile1.SaveAs(fileName);
			using (DB16Entities db = new DB16Entities())
			{
				db.Pictures.Add(obj);
				db.SaveChanges();
				ModelState.Clear();
				obj = null;

				TempData["msg"] = "<script>alert('Register successfully');</script>";
				return RedirectToAction("PIndex");
			}
		}

		public ActionResult PEdit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Picture s = db.Pictures.Find(id);
			if (s == null)
			{
				return HttpNotFound();
			}
			ViewBag.MobileId = new SelectList(db.Mobiles, "Id", "Name", s.MobileId);
			return View(s);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult PEdit(Picture obj)
		{
			string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile1.FileName);
			string extension = Path.GetExtension(obj.ImageFile1.FileName);
			fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
			obj.Image = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			obj.ImageFile1.SaveAs(fileName);

			db.Entry(obj).State = EntityState.Modified;
			db.SaveChanges();
			return RedirectToAction("PIndex");

		}

		public ActionResult PDelete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Picture user = db.Pictures.Find(id);
			if (user == null)
			{
				return HttpNotFound();
			}
			return View(user);
		}


		[HttpPost, ActionName("PDelete")]
		[ValidateAntiForgeryToken]
		public ActionResult PDeleteConfirmed(int id)
		{		
			Picture p = db.Pictures.Find(id);
			db.Pictures.Remove(p);
			db.SaveChanges();
			return RedirectToAction("PIndex");
		}

		public ActionResult PDetails(int? id)
		{
			Picture p = new Picture();
			using (DB16Entities db = new DB16Entities())
			{
				p = db.Pictures.Where(x => x.Id == id).FirstOrDefault();
			}
			return View(p);
		}
		
	}
}
