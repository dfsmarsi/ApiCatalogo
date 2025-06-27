namespace APICatalogo.Services
{
    public class MeuServico : IMeuServico
    {
        //usando fromservices
       public string Saudacao(string nome) {
            return $"Olá, {nome}!";
       }
    }
}
