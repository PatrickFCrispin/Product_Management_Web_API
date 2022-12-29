namespace ProductManagement.API.Messages
{
    public class Messages
    {
        public const string ListEmpty = "Nenhum produto cadastrado na base de dados";

        public const string ListEmptyForPage = "Nenhum registro encontrado";

        public const string ProductRegisteredSuccessfully = "Produto cadastrado com sucesso!";

        public const string ProductUpdatedSuccessfully = "Produto atualizado com sucesso!";

        public const string ProductDeactivatedSuccessfully = "Produto inativado com sucesso!";

        public static string ProductNotFound(int id)
        {
            return $"Produto de código {id} não encontrado";
        }
    }
}