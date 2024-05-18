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

        // GET: Usuarios
        public async Task<IActionResult> Index ( )
        {
            return View ( await _context.Usuarios.ToListAsync ( ) );
        }

        // GET: Usuarios/Details/5
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

        // GET: Usuarios/Create
        public IActionResult Create ( )
        {
            var usuario = new Usuario();
            usuario.Endereco = new Endereco ( );
            return View ( usuario );
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit ( Guid id )
        {
            var usuario = await _context.Usuarios.Include(u => u.Endereco).FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound ( );
            }

            return View ( usuario );
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    // Certifique-se de que o objeto Usuario esteja associado ao contexto do banco de dados
                    _context.Update ( usuario );

                    // Verifique se o Endereco associado ao Usuario está presente e é válido
                    if (usuario.Endereco != null)
                    {
                        // Se o Endereco tiver um ID, é uma atualização
                        if (usuario.Endereco.Id != Guid.Empty)
                        {
                            _context.Update ( usuario.Endereco );
                        }
                        else // Se o Endereco não tiver um ID, é uma inserção
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


        // GET: Usuarios/Delete/5
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

        // POST: Usuarios/Delete/5
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
