public static class ValidationUtilities
{
    public static int CalculateRemainder(string weightings, int? fudgeFactor, int? modulus, string accountNumber)
    {
        int count = 0, position = 0, sWeight = 0, Total = 0, sAccNo = 0;
        int weightLen = weightings.Length / 2;

        while (weightLen > accountNumber.Length)
        {
            accountNumber = "0" + accountNumber;
        }

        for (count = 0; count < weightLen; count++, position += 2)
        {
            if (accountNumber[count] == ' ')
            {
                break;
            }

            switch (weightings[position + 1])
            {
                case 'A':
                    sWeight = 10;
                    break;

                case 'D':
                    sWeight = 13;
                    break;

                case 'H':
                    sWeight = 17;
                    break;

                case 'J':
                    sWeight = 19;
                    break;

                case 'N':
                    sWeight = 23;
                    break;

                case 'T':
                    sWeight = 29;
                    break;

                default:
                    sWeight = int.Parse(weightings.Substring(position, 2));
                    break;
            }
            sAccNo = int.Parse(accountNumber.Substring(count, 1));
            Total += sWeight * sAccNo;
        }
        Total += fudgeFactor ?? 0;
        if (modulus == 0)
        {
            return Total;
        }
        return Total % modulus ?? 0;
    }

    public static string MirrorStr(string InAccNo)
    {
        char[] inchars = InAccNo.ToCharArray();
        char[] outchars = InAccNo.ToCharArray();
        for (int counter = 0; counter < inchars.GetLength(0); counter++)
        {
            outchars[inchars.GetLength(0) - counter - 1] = inchars[counter];
        }
        return new string(outchars);
    }
}