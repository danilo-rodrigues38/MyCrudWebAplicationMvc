using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCrudWebAplicationMvc.Context;
using MyCrudWebAplicationMvc.Models;

namespace MyCrudWebAplicationMvc.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly MyConnection _context;

        public UsuariosController ( MyConnection context )
        {
            _context = context;
        }

        public async Task<IActionResult> Index ( )
        {
            return View ( await _context.Usuarios.ToListAsync ( ) );
        }

        public async Task<IActionResult> Details ( Guid? id )
        {
            if (id == null)
            {
                return NotFound ( );
            }

            //var usuario = await _context.Usuarios.Include(u => u.Endereco).FirstOrDefaultAsync(u => u.Id == id);
            var usuario = await _context.Usuarios.Include(u => u.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound ( );
            }

            return View ( usuario );
        }

        public IActionResult Create ( )
        {
            var usuario = new Usuario();
            usuario.Endereco = new Endereco ( );
            return View ( usuario );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ( Usuario usuario, Endereco endereco )
        {
            if (ModelState.IsValid)
            {
                usuario.Endereco = endereco;
                usuario.Endereco.Id = usuario.Id;

                _context.Usuarios.Add ( usuario );
                await _context.SaveChangesAsync ( );
                return RedirectToAction ( nameof ( Index ) );
            }
            return View ( usuario );
        }

        public async Task<IActionResult> Edit ( Guid id )
        {
            var usuario = await _context.Usuarios.Include(u => u.Endereco).FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound ( );
            }

            return View ( usuario );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit ( Guid id, Usuario usuario, Endereco endereco )
        {
            if (id != usuario.Id)
            {
                return NotFound ( );
            }

            if (ModelState.IsValid)
            {
                usuario.Endereco = endereco;
                usuario.Endereco.Id = usuario.Id;

                try
                {
                    _context.Update ( usuario );

                    if (usuario.Endereco != null)
                    {
                        if (usuario.Endereco.Id != Guid.Empty)
                        {
                            _context.Update ( usuario.Endereco );
                        }
                        else
                        {
                            _context.Add ( usuario.Endereco );
                        }
                    }

                    await _context.SaveChangesAsync ( );
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists ( usuario.Id ))
                    {
                        return NotFound ( );
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction ( nameof ( Index ) );
            }
            return View ( usuario );
        }

        public async Task<IActionResult> Delete ( Guid? id )
        {
            if (id == null)
            {
                return NotFound ( );
            }

            var usuario = await _context.Usuarios.Include(u => u.Endereco)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound ( );
            }

            return View ( usuario );
        }

        [HttpPost, ActionName ( "Delete" )]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed ( Guid id )
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove ( usuario );
            }

            await _context.SaveChangesAsync ( );
            return RedirectToAction ( nameof ( Index ) );
        }

        private bool UsuarioExists ( Guid id )
        {
            return _context.Usuarios.Any ( e => e.Id == id );
        }
    }
}
