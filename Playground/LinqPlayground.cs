using Conversor_Monedas_Api.Data;

namespace Conversor_Monedas_Api.Playground
{
    public class LinqPlayground
    {
        private readonly AppDbContext _context;
        public LinqPlayground(AppDbContext context)
        {
            _context = context;
        }
        public void Run()
        {
            Console.WriteLine("=== TODAS ===");
            // --- Aquí puedes experimentar con consultas LINQ usando _context ---
            var monedas = _context.Moneda.ToList(); // SELECT * FROM Moneda

            foreach (var m in monedas)
            {
                Console.WriteLine($"{m.Codigo} - {m.Leyenda}");
            }

            // --- solo btc ---
            Console.WriteLine("=== SOLO BTC ===");

            var monedasBTC = _context.Moneda
            .Where(m => m.Codigo == "BTC")
            .ToList();// toList: Ejecuta la consulta y trae los datos de la base a memoria como una List<T>.

            foreach (var m in monedasBTC)
            {
                Console.WriteLine($"{m.Codigo} - {m.Leyenda}");
            }


            //----------------------------
            Console.WriteLine("=== FILTRADO ===");
            // --- filtro ---
            var query = _context.Moneda.AsQueryable();

            // Agrego filtros
            query = query.Where(m => m.Codigo != "ARS");

            query = query.OrderBy(m => m.Codigo);

            var resultado = query.ToList();

            foreach (var m in resultado)
            {
                Console.WriteLine($"{m.Codigo} - {m.Leyenda}");
            }
        }

    }
}
