using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Context;
using BackEnd.Models.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EjemploController : ControllerBase
    {

        private readonly InMemoryContext _IMcontext;

        public EjemploController(InMemoryContext IMcontext)
        {
            _IMcontext = IMcontext;
        }

        [HttpDelete]
        [Route("BorrarUsuario/{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Delete(long id)
        {
            var usuario = _IMcontext.Users.Where(x => x.Id == id).FirstOrDefault();
            if(usuario != null)
            {
                _IMcontext.Users.Remove(usuario);
                _IMcontext.SaveChanges();
            }
            return Ok($"El usuario: {usuario.FirstName} {usuario.LastName} ha sido eliminado.");
        }
        
        [HttpPost]
        [Route("AgregarUsuario")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult Post(User NuevoUsuario)
        {
            try
            {
                NuevoUsuario.Id = ObtenerIdUsuario();
                _IMcontext.Users.Add(NuevoUsuario);
                _IMcontext.SaveChanges();
                return Ok($"UsuarioId:{NuevoUsuario.Id}, Nombre complreto {NuevoUsuario.FirstName} {NuevoUsuario.LastName}");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        [HttpGet]
        [Route("ObtenerUsuarios")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get()
        {
            var users = await _IMcontext.Users
                .Include(u => u.Posts)
                .ToArrayAsync();

            var response = users.Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                posts = u.Posts.Select(p => p.Content)
            });

            return Ok(response);
        }

        private int ObtenerIdUsuario()
        {
            try
            {
                var maxId = _IMcontext.Users.Select(t => t.Id)
                    .DefaultIfEmpty()
                    .Max();

                if (maxId == null)
                    maxId = 1;
                else
                    maxId++;

                return maxId;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}