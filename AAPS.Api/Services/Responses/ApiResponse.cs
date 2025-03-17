namespace AAPS.Api.Responses
{
    public class ApiResponse<T>
    {
        public bool Sucesso { get; private set; }
        public string Mensagem { get; private set; }
        public T? Dados { get; private set; }
        public List<string>? Erros { get; private set; } = new();

        private ApiResponse(bool sucesso, string mensagem, T? dados, List<string>? erros = null)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
            Erros = erros ?? new List<string>() ;
        }

        public static ApiResponse<T> SucessoResponse(T? dados, string mensagem = "Sucesso!")
        {
            return new ApiResponse<T>(true, mensagem, dados);
        }

        public static ApiResponse<T> ErroResponse(List<string> erros, string mensagem = "Erro!")
        {
            return new ApiResponse<T>(false, mensagem, default, erros);
        }
    }
}
