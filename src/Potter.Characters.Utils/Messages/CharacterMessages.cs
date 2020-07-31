namespace Potter.Characters.Utils.Messages
{
    public class CharacterMessages
    {
        public static string ExistsInDatabase = "Já existe um personagem com o nome '{0}' no banco de dados.";
        public static string NotExistsInPotterApi = "Não existe um personagem com o nome '{0} na base do PotterApi.'";
        public static string NotExistsInDatabase = "Não existe um personagem com o nome '{0} no Banco de Dados.'";

        // Model Messages
        public static string NameIsRequired = "O Nome é obrigatório.";
        public static string NameLessThen = "A quantidade de caracters do Nome, deve ser inferior a 150.";

        public static string RoleIsRequired = "A Função é obrigatória.";
        public static string RoleLessThen = "A quantidade de caracters da Função, deve ser inferior a 100.";

        public static string SchoolIsRequired = "A Escola é obrigatória.";
        public static string SchoolLessThen = "A quantidade de caracters da Escola, deve ser inferior a 200.";

        //public static string HouseIsRequired = "A Casa é obrigatória.";
        //public static string HouseLessThen = "A quantidade de caracters da Casa, deve ser inferior a 100.";

        public static string PatronusIsRequired = "O Patronus é obrigatório.";
        public static string PatronusLessThen = "A quantidade de caracters do Patronus, deve ser inferior a 50.";
    }
}
