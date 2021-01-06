using Commonz;
using PhoneShop.Models.DataContextZ;
using PhoneShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneShop.Controllers
{
    public class AccountsController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string uname, string pass)
        {
            List<Customer> customers = db.Customers.ToList();
            List<Admin> admins = db.Admins.ToList();
            foreach(Customer c in customers)
            {
                if(c.email == uname && c.hashedPassword == pass)
                {
                    Session["CustomerID"] = c.Id.ToString();
                    Session["CustomerName"] = c.customerName.ToString();
                    //work with session and stuff
                    return RedirectToAction("Index", "Home");
                }                
            }
            foreach(Admin a in admins)
            {
                if(a.userName == uname && a.hashedPassword == pass)
                {
                    Session["AdminID"] = a.Id.ToString();
                    Session["AdminName"] = a.adminName.ToString();
                    //work with session and stuff
                    return RedirectToAction("Index", "Admin");
                }
            }
            return View("Login");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Customer customer)
        {

            if (ModelState.IsValid)
            {
                string mailz = customer.email;
                string content = System.IO.File.ReadAllText(Server.MapPath(
                "~/template/RegisterMail.html"));
                new MailHelper().SendMail(mailz, "Đăng kí thành công PhoneShopZ", content);
                db.Customers.Add(customer);                
                db.SaveChanges();
                //Customer c = db.Customers.LastOrDefault();
                Cart cart = new Cart
                {
                    Customer = customer,
                    CustomerId = customer.Id,
                    TotalValue = 0
                };
                db.Carts.Add(cart);
                db.SaveChanges();
                
                return RedirectToAction("Login");

                
            }
            return View();
        }
        public ActionResult Logout()
        {
            int z = -1;
            int di = -1;
            if (Session["CustomerID"] != null)
            {
                z = int.Parse((string)Session["CustomerID"]);
                di = 1;
            }
            else if (Session["AdminID"] != null)
            {
                z = int.Parse((string)Session["AdminID"]);
                di = 2;
            }
            else
            {
                Session.Clear();
                return RedirectToAction("Index", "Home");
            }
            if (di == 1)
            {
                Customer customer = db.Customers.Where(x => x.Id == z).FirstOrDefault();
                customer.lastestAccess = DateTime.Now;
            }
            else if(di==2)
            {
                Admin admin = db.Admins.Where(x => x.Id == z).FirstOrDefault();
                admin.lastestAccess = DateTime.Now;
            }            
            db.SaveChanges();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}