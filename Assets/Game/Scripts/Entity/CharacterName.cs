namespace Entity
{
    public class CharacterName
    {
        public string First;
        public string Middle;
        public string Last;
        public string Full => $"{First} {Last}";
        
        public ImpliedGender Gender { get; }
        
        public enum ImpliedGender
        {
            None,
            Neutral,
            Masc,
            Femme,
        }

        public static string AbbreviateGender(ImpliedGender impliedGender)
        {
            switch (impliedGender)
            {
                case ImpliedGender.None:
                    return "-";
                case ImpliedGender.Masc:
                    return "M";
                case ImpliedGender.Femme:
                    return "F";
                case ImpliedGender.Neutral:
                    return "X";
            }

            return "";
        }

        public CharacterName(string firstName, string lastName, ImpliedGender gender)
        {
            First = firstName;
            Last = lastName;
            Gender = gender;
        }

        public CharacterName(string firstName, string lastName, string middleName)
        {
            First = firstName;
            Last = lastName;
            Middle = middleName;
        }
    }
}