using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhoneShop.Models.PayPalConfig;
using PhoneShop.Models.DataContextZ;

namespace PhoneShop.Controllers
{
    public class PayPalController : Controller
    {
        // GET: PayPal
        //PayPal Payment No.2
        private Payment payment;
        private DatabaseContext db = new DatabaseContext();
        private Payment CreatePayPalPayment(APIContext apicontext, string redirectURL, int ordId)
        {
            Session["order"] = db.OrderDetails.Include("Product").Where(x => x.OrderId == ordId).ToList();
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            if (Session["order"] != null)
            {
                List<Models.Domain.OrderDetail> order = (List<Models.Domain.OrderDetail>) (Session["order"]);
                /*
                foreach (Item item in (List<Item>)Session["order"])
                {
                    item.name.ToString();
                    item.currency = "USD";
                    item.price.ToString();
                    item.quantity.ToString();
                    item.sku = "sku";

                }
                */
                foreach(var item in order)
                {
                    itemList.items.Add(new Item()
                    {
                        name = item.Product.productName.ToString(),
                        currency = "USD",
                        price = item.Product.PriceOut.ToString(),
                        quantity = item.quantity.ToString(),
                        sku = "sku"
                    });
                    
                }
            }
            var payer = new Payer() { payment_method = "paypal" };
            var redirUrl = new RedirectUrls()
            {
                cancel_url = redirectURL + "@Cancel=true",
                return_url = redirectURL
            };
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
            var amount = new Amount()
            {
                currency = "USD",
                total = Session["TotalValue"].ToString(),
                details = details
            };
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction Desc",
                invoice_number = "#100000",
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent="sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrl
            };
            return this.payment.Create(apicontext);
        }
        private object ExecutePayment(APIContext apicontext, string payerID, string PaymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerID
            };
            this.payment = new Payment()
            {
                id = PaymentId
            };
            return payment.Execute(apicontext, paymentExecution);

        }
        public ActionResult PayPalPayment(int ordId)
        {
            int zz = ordId;
            APIContext apicontext = PayPalConfiguration.GetAPIContextZ();
            try
            {
                string PayerID = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(PayerID))
                {
                    string baseUri = Request.Url.Scheme + "://"
                        + Request.Url.Authority + "PaypalPayment/Paypal";
                    var Guid = Convert.ToString((new Random()).Next(100000000));
                    var createPayment = this.CreatePayPalPayment(apicontext, baseUri
                        + "guid=" + Guid, zz);
                    var link = createPayment.links.GetEnumerator();
                    string paypalRedirectURL = null;
                    while(link.MoveNext())
                    {
                        Links liz = link.Current;
                        if(liz.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectURL = liz.href;

                        }
                    }
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executePayment = ExecutePayment(apicontext, PayerID, Session[guid] as string);
                    if(executePayment.ToString().ToLower()!="approved")
                    {
                        return View("FailedPayPalPayment");
                    }
                }
            }
            catch (Exception)
            {
                return View("FailedPayPalPayment");
            }
            return View("SuccessfulPayment");
        }
    }
}