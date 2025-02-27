namespace ExamenPractico.API.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public decimal Salario { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}
