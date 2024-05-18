namespace MyCrudWebAplicationMvc.Models
{
    public class Usuario : Entity
    {
        public string Name { get; set; }
        public  string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Telefone { get; set; }
        public  string Email { get; set; }
        public Endereco Endereco { get; set; }
    }
}
