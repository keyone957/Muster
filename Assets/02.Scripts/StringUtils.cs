// 문자열 관련 유틸리티
// 최초 작성자 : 김기홍
// 수정자 : -
// 최종 수정일 : 2024-01-12
public static class StringUtils
{
    private const string PASSWORD_CHARS =
         "0123456789abcdefghijklmnopqrstuvwxyz";

    public static string GeneratePassword(int length)
    {
        var sb = new System.Text.StringBuilder(length);
        var r = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int pos = r.Next(PASSWORD_CHARS.Length);
            char c = PASSWORD_CHARS[pos];
            sb.Append(c);
        }

        return sb.ToString();
    }
}
