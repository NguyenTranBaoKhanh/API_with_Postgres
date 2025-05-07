namespace API_with_Postgres.Common.Extentions
{
    public static class StreamExtension
    {
        public static byte[] ReadAsByte(this Stream input)
        {
            if (input.Length == 0)
                return null;

            using var ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
