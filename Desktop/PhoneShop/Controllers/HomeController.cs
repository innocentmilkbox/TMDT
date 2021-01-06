using Commonz;
using PhoneShop.Models.DataContextZ;
using PhoneShop.Models.DisplayOnView;
using PhoneShop.Models.Domain;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneShop.Controllers
{
    public class HomeController : Controller
    {
        DatabaseContext db = new DatabaseContext();
        
        public ActionResult Index()
        {
            //Session.Clear();
            if (!(Session["SessionCart"] is List<CartDetail>)) Session["SessionCart"] = new List<CartDetail>();
            return View(db.Products.Include("Categories").OrderByDescending(x => x.SoldCount).Take(8).ToList());
        }
        
        
        public ActionResult ProductList(string catName, int? page)
        {            
            int k = 0;
            if (page != null)
            {
                k = (page.GetValueOrDefault()-1) * 12;
            }
            List<Product> products = new List<Product>();
            //t = quantity of selected products
            int t = 0;

            //show all phones (not include accessories)
            if (catName == "all")
            {
                products = db.Products.Where(xx => xx.Categories.Descriptions != "Accessories" && xx.Stocked > 0).OrderBy(p => p.Id).Skip(k).Take(12).Include(x => x.Categories).ToList();
                t = db.Products.Where(xx => xx.Categories.Descriptions != "Accessories").ToList().Count();
            }

            //show all accessories
            else if(catName == "Accessories")
            {
                products = db.Products.Where(xx => xx.Categories.Descriptions == catName && xx.Stocked > 0).OrderBy(p => p.Id).Skip(k).Take(12).Include(x => x.Categories).ToList();
                t = db.Products.Where(xx => xx.Categories.Descriptions == catName).Count();
            }

            //show products of a category
            else
            {
                products = db.Products.Where(x => x.Categories.Name == catName && x.Stocked > 0).OrderBy(p => p.Id).Skip(k).Take(12).Include(x => x.Categories).ToList();
                t = db.Products.Where(x => x.Categories.Name == catName).ToList().Count();
            }
            //get number of pages as [z]
            int z = t / 12;
            if (z % 12 != 0) z++;
            ViewBag.countPage = z;
            ViewBag.Count = products.Count();            
            ViewBag.category = catName;
            return View(products);
        }
        public ActionResult ProductDetails(int? id)
        {
            if (id == null) return RedirectToAction("NotFound");                        
            Product p = db.Products.Include("Categories").Where(k => k.Id == id).First();
            List<Product> products = db.Products.Include("Categories").Where(x => x.CategoryId == p.CategoryId).Take(6).ToList();
            //products.Add(p);  
            ViewBag.id = p.Id;
            ViewBag.img = p.imgPath;
            ViewBag.name = p.productName;
            ViewBag.price = p.PriceOut;
            ViewBag.stock = p.Stocked;
            ViewBag.category = p.Categories.Name;
            return View(products.ToList());
            
        }

        public ActionResult GoToCart()
        {
            if(Session["CustomerID"] != null)
            {
                string str = Session["CustomerID"].ToString();
                Cart cart = db.Carts.Where(x => x.CustomerId.ToString() == str).FirstOrDefault();
                List<CartDetail> cartDetails = db.CartDetails.Where(p => p.CartId == cart.Id).Include("Product").ToList();
                return View("Cart", cartDetails);
            }

            else
            {
                return View("Cart",Session["SessionCart"]);
            }
        }
        public ActionResult AddToCart(int id, int quantity)
        {
            
            //Khach hang da dang nhap
            if (ModelState.IsValid&&Session["CustomerID"]!=null)
            {
                Product p = db.Products.Include("Categories").Where(x => x.Id == id).FirstOrDefault();
                int z = int.Parse((string)Session["CustomerID"]);
                Cart cart = db.Carts.Include("Customer").Where(y => y.CustomerId == z).FirstOrDefault();
                int tt = cart.Id;
                IEnumerable<CartDetail> cartDetails = db.CartDetails.Include("Product").Where(c => c.CartId == cart.Id);
                bool existed = false;
                //int change = 0;
                foreach (CartDetail cd in cartDetails)
                {
                    if (cd.Product.Id == p.Id)
                    {
                        cd.Quantity += quantity;
                        existed = true;
                        //change = cd.Id;
                        
                        break;
                    }
                }
                cart.TotalValue += p.PriceOut * quantity;
                if (!existed)
                {
                    CartDetail newCartItem = new CartDetail
                    {
                        CartId = cart.Id,
                        Product = p,
                        ProductId = p.Id,
                        Quantity = quantity
                    };

                    db.CartDetails.Add(newCartItem);
                    
                }
                db.SaveChanges();
                List<CartDetail> cartDetails1 = db.CartDetails.Include("Product").Where(o => o.CartId == tt).ToList();
                return View("Cart", cartDetails1);
            }
            //Khach vang lai mua hang
            else
            {

                List<CartDetail> sessionCart = (List<CartDetail>)Session["SessionCart"];
                Product p = db.Products.Include("Categories").Where(x => x.Id == id).FirstOrDefault();
                CartDetail cd = new CartDetail
                {
                    CartId = -1,
                    Id = -1,
                    ProductId = id,
                    Product = p,
                    Quantity = quantity                    
                };
                sessionCart.Add(cd);
                Session["SessionCart"] = sessionCart;
                return View("Cart", sessionCart);
            }
            //return RedirectToAction("Login","Accounts");
        }
        
        [HttpPost]
        public ActionResult ProvideAddress(Address address)
        {
            Address a = new Address
            {
                
                Country = address.Country,
                CityOrProvince = address.CityOrProvince,
                District = address.District,
                subDistrict = address.subDistrict,
                Street = address.Street,
                NumberOrDetails = address.NumberOrDetails
            };
            if (ModelState.IsValid) return RedirectToAction("CheckOut", a);
            return RedirectToAction("GoToCart");
        }
        public ActionResult CheckOut(Address address)
        {
            if(Session["CustomerID"]!=null)
            {
                string cusName, cusMail, cusAdd;
                double cusTotal;
                int cId = Int32.Parse((string)Session["CustomerID"]);
                Customer customer = db.Customers.Where(p => p.Id == cId).FirstOrDefault();
                cusName = customer.customerName;
                cusMail = customer.email;
                Cart cart = db.Carts.Where(p => p.CustomerId == customer.Id).FirstOrDefault();

                Order order = new Order
                {
                    Address = address.NumberOrDetails + " -- " +
                    address.Street + " -- " + address.subDistrict + " -- " + address.District + " -- " + address.CityOrProvince + " -- " + address.Country,
                    Customers = customer,
                    CustomersId = customer.Id,
                    isChecked = false,
                    isDeliverAssign = false,
                    isPaid = false,
                    orderDate = DateTime.Now,
                    TotalValue = cart.TotalValue
                };
                cusAdd = order.Address;
                cusTotal = order.TotalValue;
                cart.TotalValue = 0;
                customer.totalTradingValue += cusTotal;
                db.Orders.Add(order);
                db.SaveChanges();


                Customer cus = db.Customers.Where(p => p.Id == cId).FirstOrDefault();
                Cart ca = db.Carts.Where(p => p.CustomerId == cus.Id).FirstOrDefault();
                IEnumerable<CartDetail> cartDetails = db.CartDetails.Where(p => p.CartId == ca.Id);
                Order or = db.Orders.OrderByDescending(p => p.Id).FirstOrDefault();
                int ooo = or.Id;
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (CartDetail c in cartDetails)
                {
                    OrderDetail odetail = new OrderDetail
                    {
                        OrderId = or.Id,
                        Product = c.Product,
                        ProductId = c.ProductId,
                        quantity = c.Quantity,
                        rateTier = 0
                    };
                    orderDetails.Add(odetail);
                }
                foreach (OrderDetail o in orderDetails)
                {
                    Product p = db.Products.Where(x => x.Id == o.ProductId).FirstOrDefault();
                    p.Stocked -= o.quantity;
                    p.SoldCount += o.quantity;
                    db.SaveChanges();
                }

                string content = System.IO.File.ReadAllText(Server.MapPath(
                    "~/template/MailTemplate.html"));
                content = content.Replace("{{CustomerName}}", cusName);
                content = content.Replace("{{Email}}", cusMail);
                content = content.Replace("{{Address}}", cusAdd);
                content = content.Replace("{{Total}}", cusTotal.ToString());

                //var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(cusMail, "Đơn hàng từ PhoneShopZ", content);
                //new MailHelper().SendMail(toEmail, "Đơn hàng từ PhoneShopZ", content);

                db.CartDetails.RemoveRange(db.CartDetails.Where(p => p.CartId == ca.Id));
                db.SaveChanges();
                db.OrderDetails.AddRange(orderDetails);
                db.SaveChanges();

                //return RedirectToAction("PaymentMethod",ooo);
                return View("CheckOutConfirmed");
                //return RedirectToAction("PayPalPayment", "PayPal", ooo);
            }
            else
            {
                return RedirectToAction("OneTimeBuyer", address);
            }
        }
        public ActionResult OneTimeBuyer(Address address)
        {
            string addrr = address.NumberOrDetails + " -- " +
                    address.Street + " -- " + address.subDistrict + " -- " + address.District + " -- " + address.CityOrProvince + " -- " + address.Country;
            ViewBag.Addzz = addrr;

            

            List<CartDetail> cartDetails = (List<CartDetail>)Session["SessionCart"];
            double totalz = 0;
            foreach (CartDetail c in cartDetails)
            {
                double zzz = c.Quantity * c.Product.PriceOut;
                totalz += zzz;
            }

            //Tao Khach hang dai dien cho khach vang lai
            Customer customer = db.Customers.Where(x => x.customerName == "One Time Buyer").First();
            customer.totalTradingValue += totalz;

            Order order = new Order
            {
                CustomersId = customer.Id,
                Customers = customer,
                Address = addrr,
                orderDate = DateTime.Now,
                isChecked = false,
                isDeliverAssign = false,
                isPaid = false,
                TotalValue = totalz
            };
            db.Orders.Add(order);
            db.SaveChanges();
            Order orderzz = db.Orders.OrderByDescending(x => x.Id).FirstOrDefault();

            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (CartDetail c in cartDetails)
            {
                OrderDetail odetail = new OrderDetail
                {
                    OrderId = orderzz.Id,
                    Product = c.Product,
                    ProductId = c.ProductId,
                    quantity = c.Quantity,
                    rateTier = 0
                };
                orderDetails.Add(odetail);
            }
            foreach (OrderDetail o in orderDetails)
            {
                Product p = db.Products.Where(x => x.Id == o.ProductId).FirstOrDefault();
                p.Stocked -= o.quantity;
                p.SoldCount += o.quantity;
                db.SaveChanges();
            }
            db.OrderDetails.AddRange(orderDetails);
            db.SaveChanges();
            return View("OneTimeBuyer");
        }
        public ActionResult OneTimeCheckOut()
        {
            return View("CheckOutConfirmed");
        }
        /*
        public ActionResult PaymentMethod(int orId)
        {
            int orderId = orId;
            Order order = db.Orders.Include("Customers").Where(x => x.Id == orderId).FirstOrDefault();
            return View(order);
        }
        public ActionResult CashOnDelivery(int or)
        {
            return View("CheckOutConfirmed");
        }
        public ActionResult PayPal(int or)
        {
            Order order = db.Orders.Include("Customers").Where(x => x.Id == or).FirstOrDefault();
            order.isPaid = true;
            db.SaveChanges();
            return View("PayPal");
        }*/

        

        public ActionResult AccountInfo()
        {
            int t = 0;
            if (Session["CustomerID"] != null) t = int.Parse((string)Session["CustomerID"]);
            Customer cus = db.Customers.Where(x => x.Id == t).FirstOrDefault();
            return View(cus);
        }
        public ActionResult OrderStatus()
        {
            int cusId = int.Parse((string)Session["CustomerID"]);
            List<Order> orders = db.Orders.Where(p => p.CustomersId == cusId).ToList();
            return View(orders);
        }
        public ActionResult OrderDetailStatus(int orderId)
        {
            //int cusId = int.Parse((string)Session["CustomerID"]);            
            List<OrderDetail> orderDetails = db.OrderDetails.Include("Product").Where(p => p.OrderId == orderId).ToList();
            return View(orderDetails);
        }

        public ActionResult Searching(string search)
        {
            string z = search;
            List<Product> products = db.Products.Include("Categories").Where(p => p.productName.Contains(search)).ToList();
            return View("ProductList", products);
        }

        public ActionResult Promotions()
        {
            return View();
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
    }
}