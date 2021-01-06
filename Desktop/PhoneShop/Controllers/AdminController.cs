using PhoneShop.Models.DataContextZ;
using PhoneShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PhoneShop.Controllers
{
    public class AdminController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult List()
        {
            return View(db.Admins.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,adminName,userName,hashedPassword,isSuperAdmin,createDate,lastestAccess")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                //string hashed = admin.hashedPassword;
                //MD5CryptoServiceProvider hasher = new MD5CryptoServiceProvider();
                //string zz = string.Concat("ummm salty " + hashed);
                //byte[] t = hasher.ComputeHash(Encoding.Unicode.GetBytes(zz));
                //hashed = Encoding.Unicode.GetString(t);
                //admin.hashedPassword = hashed;
                
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        public ActionResult CustomerInfo(int id)
        {
            Customer cus = db.Customers.Where(x => x.Id == id).FirstOrDefault();
            return View(cus);
        }
        public ActionResult OrderAdmin()
        {
            List<Order> orders = db.Orders.Include("Customers").ToList();            
            return View(orders);
        }
        public ActionResult OrderDetailAdmin(int orderId)
        {
            List<OrderDetail> orderDetails = db.OrderDetails.Include("Product").Where(x => x.OrderId == orderId).ToList();
            return View(orderDetails);
        }
        public ActionResult DeliveryAssign(int orderId)
        {
            Order order = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();
            if(order.isChecked) order.isDeliverAssign = true;
            db.SaveChanges();
            return RedirectToAction("OrderAdmin");
        }
        public ActionResult ApproveOrder(int orderId)
        {
            Order order = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();
            order.isChecked = true;
            db.SaveChanges();
            return RedirectToAction("OrderAdmin");
        }
        public ActionResult FinishOrder(int orderId)
        {
            Order order = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();
            if (order.isChecked) order.isPaid = true;
            db.SaveChanges();
            return RedirectToAction("OrderAdmin");
        }
        public ActionResult DeclineOrder(int orderId)
        {
            //Order order = db.Orders.Where(x => x.Id == orderId).FirstOrDefault();

            return RedirectToAction("OrderAdmin");
        }

        
        public ActionResult OrderByDateRange(DateTime startdate, DateTime enddate)
        {
            List<Order> orders = db.Orders.Include("Customers").Where(x => x.orderDate >= startdate && x.orderDate <= enddate).ToList();
            return View("OrderAdmin",orders);
        }
    }
}