using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cortana.Models;
using System.IO;

namespace Cortana.Controllers
{
    public class Producto_ImagenController : Controller
    {
        [Authorize]
        // GET: Producto_Imagen
        public ActionResult Index()
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto_imagen.ToList());
            }
        }

        public static string NombreProducto(int idproducto)
        {
            using (var db = new inventario2021Entities())
            {
                return db.producto.Find(idproducto).nombre;
            }
        }

        public ActionResult ListaProducto()
        {
            using (var db = new inventario2021Entities())
                return PartialView(db.producto.ToList());
        }
        //crear
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(producto_imagen productoImg)
        {
            if (!ModelState.IsValid)

                return View();
            try
            {
                using (var db = new inventario2021Entities())
                {
                    db.producto_imagen.Add(productoImg);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
                throw;
            }
        }
        //detalles
        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                return View(db.producto_imagen.Find(id));
            }
        }
        //editar
        public ActionResult Edit(int id)
        {
            using (var db = new inventario2021Entities())
            {
                producto_imagen producto_Image_Edit = db.producto_imagen.Where(kev => kev.id == id).FirstOrDefault();
                return View(producto_Image_Edit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public ActionResult Edit(producto_imagen producto_Imagen_Edit)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var Product_Img = db.producto_imagen.Find(producto_Imagen_Edit.id);
                    Product_Img.imagen = producto_Imagen_Edit.imagen;
                    Product_Img.id_producto = producto_Imagen_Edit.id_producto;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
                throw;
            }
        }


        //eliminar
        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    producto_imagen producto_Image = db.producto_imagen.Find(id);
                    db.producto_imagen.Remove(producto_Image);
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



        public ActionResult CargarImagen()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CargarImagen(int id_producto, HttpPostedFileBase imagen)
        {
            try
            {
                //string para guardar la ruta
                string filePath = string.Empty;
                string nameFile = "";

                //condicion para saber si el archivo llego
                if (imagen != null)
                {
                    //ruta de la carpeta que guardara el archivo
                    string path = Server.MapPath("~/Uploads/Imagenes/");

                    //condicion para saber si la carpeta uploads existe
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    nameFile = Path.GetFileName(imagen.FileName);

                    //obtener el nombre del archivo
                    filePath = path + Path.GetFileName(imagen.FileName);

                    //obtener la extension del archivo
                    string extension = Path.GetExtension(imagen.FileName);

                    //guardar el archivo
                    imagen.SaveAs(filePath);
                }

                using (var db = new inventario2021Entities())
                {
                    var imagenProducto = new producto_imagen();
                    imagenProducto.id_producto = id_producto;
                    imagenProducto.imagen = "/Uploads/Imagenes/" + nameFile;
                    db.producto_imagen.Add(imagenProducto);
                    db.SaveChanges();

                }

                return View();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }

        }


        public ActionResult ListasProducto()
        {
            using (var db = new inventario2021Entities())
            {
                return PartialView(db.producto.ToList());
            }
        }


















    }
}