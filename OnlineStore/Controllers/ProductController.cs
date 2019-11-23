using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineStore.Enum;
using OnlineStore.Models;
using OnlineStore.ViewModels;

namespace OnlineStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

        // GET: Products/Details/5
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
                return RedirectToAction("GPUDetails", new { id = product.ProductId});
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
        // GET: Products/Create
        public ActionResult CreateGPU()
        {
            return View(new GPUViewModel());
        }

        // POST: Products/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGPU([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,VRAM,Image,Description")] GPUViewModel product, HttpPostedFileBase uploadImage)
        {
            if(product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                var pr = new Product { Manufacturer = product.Manufacturer, Name = product.Name, Price = product.Price, TypeID = product.TypeID, Description = product.Description};
                if(uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        pr.Image = imageData;
                    }
                }
                db.Product.Add(pr);
                db.SaveChanges();
                var gpu = new GPU { ProductId = pr.ProductId, VRAM = product.VRAM };
                db.GPU.Add(gpu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Create
        public ActionResult CreateCPU()
        {
            return View(new CPUViewModel());
        }

        // POST: Products/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCPU([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Frequency,Description")] CPUViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                var pr = new Product { Manufacturer = product.Manufacturer, Name = product.Name, Price = product.Price, TypeID = product.TypeID, Description = product.Description };
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        pr.Image = imageData;
                    }
                }
                db.Product.Add(pr);
                db.SaveChanges();
                var cpu = new CPU { ProductId = pr.ProductId, Frequency = product.Frequency };
                db.CPU.Add(cpu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Create
        public ActionResult CreateSSD()
        {
            return View(new SSDViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSSD([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Speed,Capacity,Description")] SSDViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                var pr = new Product { Manufacturer = product.Manufacturer, Name = product.Name, Price = product.Price, TypeID = product.TypeID, Description = product.Description };
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        pr.Image = imageData;
                    }
                }
                db.Product.Add(pr);
                db.SaveChanges();
                var ssd = new SSD { ProductId = pr.ProductId, Speed = product.Speed, Capacity = product.Capacity};
                db.SSD.Add(ssd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult CreateHDD()
        {
            return View(new HDDViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHDD([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Speed,Capacity,Description")] HDDViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                var pr = new Product { Manufacturer = product.Manufacturer, Name = product.Name, Price = product.Price, TypeID = product.TypeID, Description = product.Description };
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        pr.Image = imageData;
                    }
                }
                db.Product.Add(pr);
                db.SaveChanges();
                var hdd = new HDD { ProductId = pr.ProductId, Speed = product.Speed, Capacity = product.Capacity };
                db.HDD.Add(hdd);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

            // GET: Products/Create
            public ActionResult CreateRAM()
            {
                return View(new RAMViewModel());
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRAM([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Speed,Capacity,Description")] RAMViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                var pr = new Product { Manufacturer = product.Manufacturer, Name = product.Name, Price = product.Price, TypeID = product.TypeID, Description = product.Description };
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        pr.Image = imageData;
                    }
                }
                db.Product.Add(pr);
                db.SaveChanges();
                var ram = new RAM { ProductId = pr.ProductId, Speed = product.Speed, Capacity = product.Capacity };
                db.RAM.Add(ram);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
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
                return RedirectToAction("GPUEdit", new { id = product.ProductId });
            if (product.TypeID == ProductType.CPU)
                return RedirectToAction("CPUEdit", new { id = product.ProductId });
            if (product.TypeID == ProductType.SSD)
                return RedirectToAction("SSDEdit", new { id = product.ProductId });
            if (product.TypeID == ProductType.HDD)
                return RedirectToAction("HDDEdit", new { id = product.ProductId });
            if (product.TypeID == ProductType.RAM)
                return RedirectToAction("RAMEdit", new { id = product.ProductId });
            return View(product);
        }

        // POST: Products/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price")] Product product)
        {
           if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult GPUEdit(int? id)
        {
           var product = db.Product.Find(id);
            var gpu = db.GPU.Find(id);
            return View(new GPUViewModel
            {
                ProductId = (int)id,
                Manufacturer = product.Manufacturer,
                Name = product.Name,
                TypeID = product.TypeID,
                Price = product.Price,
                Image = product.Image,
                VRAM = gpu.VRAM,
                Description = product.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GPUEdit([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,VRAM,Description,Image")] GPUViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        product.Image = imageData;
                    }
                }
                Product newP = new Product
                {
                    ProductId = product.ProductId,
                    Manufacturer = product.Manufacturer,
                    Name = product.Name,
                    Price = product.Price,
                    TypeID = product.TypeID,
                    Image = product.Image,
                    Description = product.Description
                };
                GPU newG = new GPU
                {
                    ProductId = product.ProductId,
                    VRAM = product.VRAM
                };
                db.Entry(newP).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(newG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }
        
        public ActionResult CPUEdit(int? id)
        {
            var product = db.Product.Find(id);
            var cpu = db.CPU.Find(id);
            return View(new CPUViewModel
            {
                ProductId = (int)id,
                Manufacturer = product.Manufacturer,
                Name = product.Name,
                TypeID = product.TypeID,
                Price = product.Price,
                Image = product.Image,
                Frequency = cpu.Frequency,
                Description = product.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CPUEdit([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Frequency,Description,Image")] CPUViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        product.Image = imageData;
                    }
                }
                Product newP = new Product
                {
                    ProductId = product.ProductId,
                    Manufacturer = product.Manufacturer,
                    Name = product.Name,
                    Price = product.Price,
                    TypeID = product.TypeID,
                    Image = product.Image,
                    Description = product.Description
                };
                CPU newС = new CPU
                {
                    ProductId = product.ProductId,
                    Frequency = product.Frequency
                };
                db.Entry(newP).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(newС).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult SSDEdit(int? id)
        {
            var product = db.Product.Find(id);
            var ssd = db.SSD.Find(id);
            return View(new SSDViewModel
            {
                ProductId = (int)id,
                Manufacturer = product.Manufacturer,
                Name = product.Name,
                TypeID = product.TypeID,
                Price = product.Price,
                Image = product.Image,
                Speed = ssd.Speed,
                Capacity = ssd.Capacity,
                Description = product.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SSDEdit([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Speed,Capacity,Description,Image")] SSDViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        product.Image = imageData;
                    }
                }
                Product newP = new Product
                {
                    ProductId = product.ProductId,
                    Manufacturer = product.Manufacturer,
                    Name = product.Name,
                    Price = product.Price,
                    TypeID = product.TypeID,
                    Image = product.Image,
                    Description = product.Description
                };
                SSD newS = new SSD
                {
                    ProductId = product.ProductId,
                    Capacity = product.Capacity,
                    Speed = product.Speed
                };
                db.Entry(newP).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(newS).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult HDDEdit(int? id)
        {
            var product = db.Product.Find(id);
            var hdd = db.HDD.Find(id);
            return View(new HDDViewModel
            {
                ProductId = (int)id,
                Manufacturer = product.Manufacturer,
                Name = product.Name,
                TypeID = product.TypeID,
                Price = product.Price,
                Image = product.Image,
                Speed = hdd.Speed,
                Capacity = hdd.Capacity,
                Description = product.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HDDEdit([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Speed,Capacity,Description,Image")] HDDViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        product.Image = imageData;
                    }
                }
                Product newP = new Product
                {
                    ProductId = product.ProductId,
                    Manufacturer = product.Manufacturer,
                    Name = product.Name,
                    Price = product.Price,
                    TypeID = product.TypeID,
                    Image = product.Image,
                    Description = product.Description
                };
                HDD newH = new HDD
                {
                    ProductId = product.ProductId,
                    Capacity = product.Capacity,
                    Speed = product.Speed
                };
                db.Entry(newP).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(newH).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult RAMEdit(int? id)
        {
            var product = db.Product.Find(id);
            var ram = db.RAM.Find(id);
            return View(new RAMViewModel
            {
                ProductId = (int)id,
                Manufacturer = product.Manufacturer,
                Name = product.Name,
                TypeID = product.TypeID,
                Price = product.Price,
                Image = product.Image,
                Speed = ram.Speed,
                Capacity = ram.Capacity,
                Description = product.Description
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RAMEdit([Bind(Include = "ProductId,TypeID,Manufacturer,Name,Price,Speed,Capacity,Description,Image")] RAMViewModel product, HttpPostedFileBase uploadImage)
        {
            if (product.Price < 1)
            {
                ModelState.AddModelError(string.Empty, "Цена не может быть негативной");
            }
            if (ModelState.IsValid)
            {
                byte[] imageData = null;
                if (uploadImage != null)
                {
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                        product.Image = imageData;
                    }
                }
                Product newP = new Product
                {
                    ProductId = product.ProductId,
                    Manufacturer = product.Manufacturer,
                    Name = product.Name,
                    Price = product.Price,
                    TypeID = product.TypeID,
                    Image = product.Image,
                    Description = product.Description
                };
                RAM newR = new RAM
                {
                    ProductId = product.ProductId,
                    Capacity = product.Capacity,
                    Speed = product.Speed
                };
                db.Entry(newP).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(newR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
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
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            if (product.TypeID == ProductType.GPU)
            {
                db.GPU.Remove(db.GPU.Find(id));
                db.SaveChanges();
            }
            if (product.TypeID == ProductType.CPU)
            {
                db.CPU.Remove(db.CPU.Find(id));
                db.SaveChanges();
            }
            if (product.TypeID == ProductType.SSD)
            {
                db.SSD.Remove(db.SSD.Find(id));
                db.SaveChanges();
            }
            if (product.TypeID == ProductType.RAM)
            {
                db.RAM.Remove(db.RAM.Find(id));
                db.SaveChanges();
            }
            if (product.TypeID == ProductType.HDD)
            {
                db.HDD.Remove(db.HDD.Find(id));
                db.SaveChanges();
            }
            db.Product.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
