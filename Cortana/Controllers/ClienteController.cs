using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cortana.Models;
using Rotativa;
using System.IO;
using System.Web.Routing;

namespace Cortana.Controllers
{
    public class ClienteController : Controller
    {
        [Authorize]
        // GET: Cliente
        public ActionResult Index()
        {
            using (var bd = new inventario2021Entities())

            {


                return View(bd.cliente.ToList());

            }
        }

        //crear//
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(cliente cliente)
        {


            if (!ModelState.IsValid)
                return View();


            try
            {

                using (var db = new inventario2021Entities())

                {
                    db.cliente.Add(cliente);
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

        //Detalles//
        public ActionResult Details(int id)
        {
            using (var db = new inventario2021Entities())
            {
                var findUser = db.cliente.Find(id);
                return View(findUser);
            }
        }

        //eliminar//

        public ActionResult Delete(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    var findUser = db.cliente.Find(id);
                    db.cliente.Remove(findUser);
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

        //editar

        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    cliente findUser = db.cliente.Where(a => a.id == id).FirstOrDefault();
                    return View(findUser);

                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(cliente editUser)
        {
            try
            {
                using (var db = new inventario2021Entities())
                {
                    cliente user = db.cliente.Find(editUser.id);

                    user.nombre = editUser.nombre;
                    user.documento = editUser.documento;
                    user.email = editUser.email;
                  


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


        public ActionResult Reporte()
        {
            try
            {
                var db = new inventario2021Entities();
                var query = from tabCliente in db.cliente
                            join tabCompra in db.compra on tabCliente.id equals tabCompra.id_cliente
                            select new Reporte
                            {
                                NombreCliente = tabCliente.nombre,
                                Documentocliente = tabCliente.documento,                               
                                TotalCompra = tabCompra.total,
                            };
                return View(query);

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "error" + ex);
                return View();
            }
        }

        public ActionResult PdfReporte()

        {
            return new ActionAsPdf("Reporte") { FileName = "Reporte.pdf" };

        }
        public ActionResult uploadCSV()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uploadCSV(HttpPostedFileBase fileForm)
        {
            try
            {

                //string para guardar la ruta
                string filePath = string.Empty;

                //condicion para saber si el archivo llego
                if (fileForm != null)
                {
                    //ruta de la carpeta que guardara el archivo
                    string path = Server.MapPath("~/Uploads/");

                    //condicion para saber si la carpeta uploads existe
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //obtener el nombre del archivo
                    filePath = path + Path.GetFileName(fileForm.FileName);

                    //obtener la extension del archivo
                    string extension = Path.GetExtension(fileForm.FileName);

                    //guardar el archivo
                    fileForm.SaveAs(filePath);

                    string csvData = System.IO.File.ReadAllText(filePath);

                    foreach (string row in csvData.Split('\n'))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            var newcliente = new cliente
                            {
                                nombre = row.Split(';')[0],
                                documento = row.Split(';')[1],
                                email = row.Split(';')[2],
                                
                            };

                            using (var db = new inventario2021Entities())
                            {
                                db.cliente.Add(newcliente);
                                db.SaveChanges();
                            }
                        }
                    }
                }

                return View();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "error " + ex);
                return View();
            }
        }


        public ActionResult PaginadorIndex(int pagina = 1)
        {
            try
            {
                var cantidadRegistros = 5;

                using (var db = new inventario2021Entities())
                {
                    var cliente = db.cliente.OrderBy(x => x.id).Skip((pagina - 1) * cantidadRegistros).Take(cantidadRegistros).ToList();

                    var totalRegistros = db.cliente.Count();
                    var modelo = new ClientesIndex();
                    modelo.Clientes = cliente;
                    modelo.ActualPage = pagina;
                    modelo.Total = totalRegistros;
                    modelo.RecordsPage = cantidadRegistros;
                    modelo.valueQueryString = new RouteValueDictionary();

                    return View(modelo);
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