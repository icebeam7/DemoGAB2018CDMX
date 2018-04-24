using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DemoGAB2018CDMX.Datos;
using DemoGAB2018CDMX.Helpers;
using DemoGAB2018CDMX.Modelos;

namespace DemoGAB2018CDMX.Servicios
{
    public class ServicioBaseDatos : IServicioBaseDatos
    {
        private readonly BaseDatos bd;

        public ServicioBaseDatos()
        {
            bd = new BaseDatos(Constantes.NombreBD);
        }

        public async Task<IEnumerable<Tarea>> ObtenerTareas()
        {
            try
            {
                return await bd.Tareas.ToListAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Tarea> ObtenerTarea(int id)
        {
            try
            {
                return await bd.Tareas.FindAsync(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AgregarTarea(Tarea tarea)
        {
            try
            {
                await bd.Tareas.AddAsync(tarea);
                await bd.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> ActualizarTarea(Tarea tarea)
        {
            try
            {
                bd.Tareas.Update(tarea);
                await bd.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EliminarTarea(int id)
        {
            try
            {
                var tarea = await ObtenerTarea(id);
                bd.Tareas.Remove(tarea);
                await bd.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Tarea>> BuscarTareas(Func<Tarea, bool> condicion)
        {
            try
            {
                return bd.Tareas.Where(condicion).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Emocion> ObtenerEmocion()
        {
            try
            {
                return await bd.Emocion.FirstAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> AgregarEmocion(Emocion emocion)
        {
            try
            {
                await bd.Emocion.AddAsync(emocion);
                await bd.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> ActualizarEmocion(Emocion emocion)
        {
            try
            {
                bd.Emocion.Update(emocion);
                await bd.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EliminarEmocion()
        {
            try
            {
                var emocion = await ObtenerEmocion();
                bd.Emocion.Remove(emocion);
                await bd.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}