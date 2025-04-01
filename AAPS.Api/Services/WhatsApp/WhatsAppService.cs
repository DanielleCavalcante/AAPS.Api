//namespace AAPS.Api.Services
//{
//    public class WhatsAppService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly string _whatsappToken;
//        private readonly string _phoneNumberId;

//        public WhatsAppService(HttpClient httpClient, IConfiguration configuration)
//        {
//            _httpClient = httpClient;
//            _whatsappToken = configuration["WhatsApp:AccessToken"]; // Token gerado no Meta
//            _phoneNumberId = configuration["WhatsApp:PhoneNumberId"]; // ID do número cadastrado no Meta
//        }

//        public async Task<bool> EnviarCodigoRecuperacaoAsync(string telefone, string codigo)
//        {
//            var mensagem = $"Seu código de recuperação de senha é: {codigo}. Não compartilhe com ninguém!";
//            return await EnviarMensagemAsync(telefone, mensagem);
//        }

//        public async Task<bool> EnviarMensagemAsync(string telefone, string mensagem)
//        {
//            var requestUrl = $"https://graph.facebook.com/v18.0/{_phoneNumberId}/messages";

//            var requestBody = new
//            {
//                messaging_product = "whatsapp",
//                to = telefone,
//                type = "text",
//                text = new { body = mensagem }
//            };

//            var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
//            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "EAATQ1KkZAIWEBOZCWoDiQGuEjoFrpZBwyq98kg3WZCb6ZCWt4nEUnZCC5DiKoNhazJ9KHDF0qQZCcLLrpQr8UGUa5ubo3uywvll6O08KHl3fMI8PdqjY98MJ0GJUHEqPdrTYzRSm06ZCceYdHdxZAOWJtEof3Wuim9t74OqrtJEwouHcXmFh9qvVG5etgoGx9W5fmZCXTKzeqisopXH3uDUD3fneLw1BIZD");
//            request.Content = JsonContent.Create(requestBody);

//            var response = await _httpClient.SendAsync(request);
//            return response.IsSuccessStatusCode;
//        }
//    }
//}