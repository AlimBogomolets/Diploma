using Microsoft.AspNet.Identity;
using OnlineStore.Enum;
using OnlineStore.Models;
using OnlineStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore.Controllers
{
    public class CustomerController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customer
        public ActionResult Index()
        {
            var products = db.Product.ToList();
            return View(products);
        }

        public ActionResult ShowGPU()
        {
            using (var context = new ApplicationDbContext())
            {
                var GPUs = context.Product.Where(x => x.TypeID == ProductType.GPU).ToList();
                return View(GPUs);
            }
        }
        public ActionResult ShowCPU()
        {
            using (var context = new ApplicationDbContext())
            {
                var CPUs = context.Product.Where(x => x.TypeID == ProductType.CPU).ToList();
                return View(CPUs);
            }
        }
        public ActionResult ShowSSD()
        {
            using (var context = new ApplicationDbContext())
            {
                var SSDs = context.Product.Where(x => x.TypeID == ProductType.SSD).ToList();
                return View(SSDs);
            }
        }
        public ActionResult ShowRAM()
        {
            using (var context = new ApplicationDbContext())
            {
                var RAMs = context.Product.Where(x => x.TypeID == ProductType.RAM).ToList();
                return View(RAMs);
            }
        }
        public ActionResult ShowHDD()
        {
            using (var context = new ApplicationDbContext())
            {
                var HDDs = context.Product.Where(x => x.TypeID == ProductType.HDD).ToList();
                return View(HDDs);
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (product.TypeID == ProductType.GPU)
                return RedirectToAction("GPUDetails", new { id = product.ProductId });
            if (product.TypeID == ProductType.CPU)
                return RedirectToAction("CPUDetails", new { id = product.ProductId });
            if (product.TypeID == ProductType.SSD)
                return RedirectToAction("SSDDetails", new { id = product.ProductId });
            if (product.TypeID == ProductType.HDD)
                return RedirectToAction("HDDDetails", new { id = product.ProductId });
            if (product.TypeID == ProductType.RAM)
                return RedirectToAction("RAMDetails", new { id = product.ProductId });
            return View(product);
        }
        public ActionResult GPUDetails(int? id)
        {
            GPUViewModel gpu = new GPUViewModel
            {
                ProductId = (int)id,
                TypeID = db.Product.Find(id).TypeID,
                Manufacturer = db.Product.Find(id).Manufacturer,
                Name = db.Product.Find(id).Name,
                Price = db.Product.Find(id).Price,
                VRAM = db.GPU.Find(id).VRAM,
                Image = db.Product.Find(id).Image,
                Description = db.Product.Find(id).Description

            };
            return View(gpu);
        }

        public ActionResult CPUDetails(int? id)
        {
            CPUViewModel cpu = new CPUViewModel
            {
                ProductId = (int)id,
                TypeID = db.Product.Find(id).TypeID,
                Manufacturer = db.Product.Find(id).Manufacturer,
                Name = db.Product.Find(id).Name,
                Price = db.Product.Find(id).Price,
                Frequency = db.CPU.Find(id).Frequency,
                Image = db.Product.Find(id).Image,
                Description = db.Product.Find(id).Description
            };
            return View(cpu);
        }

        public ActionResult SSDDetails(int? id)
        {
            SSDViewModel ssd = new SSDViewModel
            {
                ProductId = (int)id,
                TypeID = db.Product.Find(id).TypeID,
                Manufacturer = db.Product.Find(id).Manufacturer,
                Name = db.Product.Find(id).Name,
                Price = db.Product.Find(id).Price,
                Speed = db.SSD.Find(id).Speed,
                Capacity = db.SSD.Find(id).Capacity,
                Image = db.Product.Find(id).Image,
                Description = db.Product.Find(id).Description
            };
            return View(ssd);
        }

        public ActionResult HDDDetails(int? id)
        {
            HDDViewModel hdd = new HDDViewModel
            {
                ProductId = (int)id,
                TypeID = db.Product.Find(id).TypeID,
                Manufacturer = db.Product.Find(id).Manufacturer,
                Name = db.Product.Find(id).Name,
                Price = db.Product.Find(id).Price,
                Speed = db.HDD.Find(id).Speed,
                Capacity = db.HDD.Find(id).Capacity,
                Image = db.Product.Find(id).Image,
                Description = db.Product.Find(id).Description
            };
            return View(hdd);
        }

        public ActionResult RAMDetails(int? id)
        {
            RAMViewModel ram = new RAMViewModel
            {
                ProductId = (int)id,
                TypeID = db.Product.Find(id).TypeID,
                Manufacturer = db.Product.Find(id).Manufacturer,
                Name = db.Product.Find(id).Name,
                Price = db.Product.Find(id).Price,
                Speed = db.RAM.Find(id).Speed,
                Capacity = db.RAM.Find(id).Capacity,
                Image = db.Product.Find(id).Image,
                Description = db.Product.Find(id).Description
            };
            return View(ram);
        }
        [Authorize]
        public ActionResult AddToCart(int? id)
        {
            var userId = User.Identity.GetUserId();
            if(db.Cart.FirstOrDefault(x => x.ProductId == (int)id && x.UserId == userId) != null)
            {
                db.Cart.FirstOrDefault(x => x.ProductId == (int)id && x.UserId == userId).Quantity += 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            db.Cart.Add(new Cart
            {
                ProductId = (int)id,
                UserId = userId,
                Quantity = 1,
                isPurchased = false
            });
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult ShowCart()
        {
            var id = User.Identity.GetUserId();
            var UsersProducts = (from p in db.Product
                                 join c in db.Cart on p.ProductId equals c.ProductId
                                 where c.Quantity > 0 && c.UserId == id
                                 select new CartViewModel{ product = p, Quantity = c.Quantity, CartID = c.CartId}).ToList();
            return View(UsersProducts);
        }

        public ActionResult DeleteFromCart(int? id)
        {
            db.Cart.Remove(db.Cart.Find(id));
            db.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        public ActionResult ClearCart()
        {
            foreach(Cart c in db.Cart)
            {
                if (c.UserId == User.Identity.GetUserId())
                    db.Cart.Remove(c);
            }
            db.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        [HttpGet]
        public ActionResult ChangeQuantity(int? id)
        {
            var cart = (from p in db.Product
                        join c in db.Cart on p.ProductId equals c.ProductId
                        where c.CartId == id
                        select new CartViewModel { product = p, Quantity = c.Quantity, CartID = c.CartId}).FirstOrDefault();
            return View(cart);
        }

        [HttpPost]
        public ActionResult ChangeQuantity(CartViewModel cart)
        {
            if (cart.Quantity < 1)
                db.Cart.Remove(db.Cart.Find(cart.CartID));
            else
                db.Cart.Find(cart.CartID).Quantity = cart.Quantity;
            db.SaveChanges();
            return RedirectToAction("ShowCart");
        }

        public ActionResult Checkout()
        {
            if (db.Users.Find(User.Identity.GetUserId()).Country == null || db.Users.Find(User.Identity.GetUserId()).City == null || db.Users.Find(User.Identity.GetUserId()).Street == null || db.Users.Find(User.Identity.GetUserId()).House == null || db.Users.Find(User.Identity.GetUserId()).Flat == null || db.Users.Find(User.Identity.GetUserId()).ZIP == null)
                return RedirectToAction("ChangeAddress", "Manage");
            var id = User.Identity.GetUserId();
            var UsersProducts = (from p in db.Product
                                 join c in db.Cart on p.ProductId equals c.ProductId
                                 where c.Quantity > 0 && c.UserId == id
                                 select new CartViewModel { product = p, Quantity = c.Quantity, CartID = c.CartId }).ToList();
            if(UsersProducts.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Ваша корзина пуста");
            }
            if(ModelState.IsValid)
            return View(UsersProducts);
            return RedirectToAction("ShowCart");
        }

        [HttpPost, ActionName("Checkout")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckoutConfirmed()
        {
            var id = User.Identity.GetUserId();
            var carts = (from p in db.Product
                        join c in db.Cart on p.ProductId equals c.ProductId
                        where c.Quantity > 0 && c.UserId == id
                        select new CartViewModel { product = p, Quantity = c.Quantity, CartID = c.CartId }).ToList();
            foreach (Cart c in db.Cart)
                if (c.UserId == User.Identity.GetUserId())
                {
                    db.History.Add(new History
                    {
                        ProductId = c.ProductId,
                        UserId = c.UserId,
                        isPurchased = true,
                        PurchaseDate = DateTime.Now,
                        Quantity = c.Quantity
                    });
                    db.Cart.Remove(c);
                }
            db.SaveChanges();
            await SendEmail(carts);
            return RedirectToAction("Index");
        }

        public ActionResult ShowHistory()
        {
            var id = User.Identity.GetUserId();
            var UsersProducts = (from p in db.Product
                                 join h in db.History on p.ProductId equals h.ProductId
                                 where h.UserId == id
                                 select new HistoryViewModel { product = p, Quantity = h.Quantity, HistoryID = h.HistoryId, PurchaseDate = h.PurchaseDate, UserName = db.Users.FirstOrDefault(x => x.Id == id).UserName }).ToList();
            return View(UsersProducts);
        }


        private async Task<ActionResult> Send(Message message)
        {
            var from = "testemailconfirmation11@gmail.com";
            var pass = "KeepItSimple11";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(from, pass);
            smtp.EnableSsl = true;

            var mail = new MailMessage(from, message.MailTo);
            mail.Subject = message.Subject;
            mail.Body = message.MailContent;
            mail.IsBodyHtml = true;

            await smtp.SendMailAsync(mail);
            return View("Index");

        }
        public async Task<ActionResult> SendEmail(List<CartViewModel> carts)
        {
            if (ModelState.IsValid)
            {
                int i = 0;
                string name;
                if (db.Users.Find(User.Identity.GetUserId()).FName == null)
                    name = User.Identity.Name;
                else
                    name = db.Users.Find(User.Identity.GetUserId()).FName;
                var sub = "Заказ на MGS";
                var body = $"<h1>Здравствуйте, {name}!</h1> <br />Мы рады Вам собщить, что приняли Ваш заказ:";
                foreach (CartViewModel c in carts)
                {
                    body += $"<br />{++i}. {c.product.Manufacturer} {c.product.Name}   Цена: {c.product.Price}*{c.Quantity}";
                }
                body += $"<br />Итого:{carts.Sum(x => x.Quantity * x.product.Price)} сом";
                body += "<br /><h2>Благодарим Вас за покупку!</h2>";
                var message = new Message
                {
                    MailContent = body,
                    MailTo = User.Identity.Name,
                    Subject = sub
                };
                await Send(message);
            }
            return RedirectToAction("ShowCart");

        }
    }
}