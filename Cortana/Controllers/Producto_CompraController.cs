using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cortana.Models;

namespace Cortana.Controllers
{
    public class Producto_CompraController : Controller
    {
        // GET: Producto_Compra
        //Index
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto_compra.ToList());
            }

        }

        public static int TotalCompra(int idcompra)
        {
            using (var db = new inventario2021Entities())
            {
                return db.compra.Find(idcompra).id;
            }
        }

        public static string NombreProducto(int idproducto)
        {
            using (var db = new inventario2021Entities())
            {
                return db.producto.Find(idproducto).nombre;
            }
        }

        public ActionResult ListaCompra()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.compra.ToList());
            }
        }

        public ActionResult ListaProducto()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.producto.ToList());
            }
        }

        //Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(producto_compra producto_compra)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto_compra.Add(producto_compra);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception kev)
            {
                ModelState.AddModelError("", "error " + kev);
                return View();
            }
        }

        //details
        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto_compra.Find(id));
            }
        }
        //edit
        public ActionResult Edit(int id)
        {
            using (var db = new inventario2021Entities())
            {
                producto_compra producto_compra_Edit = db.producto_compra.Where(a => a.id == id).FirstOrDefault();
                return View(producto_compra_Edit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(producto_compra producto_compra_Edit)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var old_producto_compra = db.producto_compra.Find(producto_compra_Edit.id);
                    old_producto_compra.id_compra = producto_compra_Edit.id_compra;
                    old_producto_compra.id_producto = producto_compra_Edit.id_producto;
                    old_producto_compra.cantidad = producto_compra_Edit.cantidad;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }
        //deleate
        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_compra producto_Compra = db.producto_compra.Find(id);
                    db.producto_compra.Remove(producto_Compra);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }
    }
}