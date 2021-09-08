using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cortana.Models;

namespace Cortana.Controllers
{
    public class ProductoController : Controller
    {
        [Authorize]
        // GET: Producto
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto.ToList());
            }
        }


        public static string NombreProveedor(int idProveedor)
        {
            using (var db = new inventario2021Entities())
            {
                return db.proveedor.Find(idProveedor).nombre;
            }
        }
        //lista//
        public ActionResult ListaProveedores()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.proveedor.ToList());
            }
        }

        //crear//
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create (producto producto)
        {
            if (!ModelState.IsValid)
                return View();


            try
            {

                using (var db = new inventario2021Entities())

                {
                    db.producto.Add(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
            }
        }

        //detalles//

        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto.Find(id));
            }
        }

        //editar//

        public ActionResult Edit(int id)
        {

            using (var db = new inventario2021Entities())
            {

                producto productoEdit = db.producto.Where(a => a.id == id).FirstOrDefault();
                return View(productoEdit);

            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(producto productoEdit)
        {
           
            try
            {

                using (var db = new inventario2021Entities())

                {
                    var oldProduc = db.producto.Find(productoEdit.id);
                    oldProduc.nombre = productoEdit.nombre;
                    oldProduc.cantidad = productoEdit.cantidad;
                    oldProduc.descripcion = productoEdit.descripcion;
                    oldProduc.percio_unitario = productoEdit.percio_unitario;
                    oldProduc.id_proveedor = productoEdit.id_proveedor;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                

            }            catch (Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
            }
        }

        // eliminar //

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto producto = db.producto.Find(id);
                    db.producto.Remove(producto);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
            }
        }

    }
}
    
